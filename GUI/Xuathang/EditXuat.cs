using BUS;
using DAL; // Nếu BUS cần DAL
using DTO;
using GUI.Helpler; // Namespace của SearchHelper
using Microsoft.VisualBasic;
using QLVT.Helper; // Namespace của PlaceholderHelper
using QLVT.Order; // Namespace của PopupXuat
using System;
using System.Collections.Generic;
using System.ComponentModel; // Cần cho BindingList
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class EditXuat : Form
    {
        private int Id; 
        private PopupXuat _popupOrder; 
        private readonly BUS_DonXuat bus = new BUS_DonXuat(); 
        private readonly BUS_TK _busTk = new BUS_TK(); 
        private readonly BUS_VatLieu bUS_VatLieu = new BUS_VatLieu(); 
        private DTO_DonXuat _originalOrderData; 
        private BindingList<DTO_ChiTietDonXuat> _editedDetailsBindingList;
        private readonly Color defaultLabelColor = Color.White;
        private readonly Color hoverLabelColor = Color.FromArgb(230, 240, 255);
        private bool _isFilteringDgv = false; 
        private DataTable _dtAllKhos = null; 
        private System.Windows.Forms.Timer debounceTimer;

        public EditXuat(PopupXuat popupXuat, int id)
        {
            InitializeComponent();
            Id = id;
            _popupOrder = popupXuat;

            if (!LoadOrderDataForEditing())
            {
                this.Shown += (s, ev) => this.Close(); 
                return;
            }

            SetupEditDataGridView();
            ConfigureDgvChiTietStyles();
            TinhTongSoLuongVaTienBan();
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += DebounceTimer_Tick;

            dgvChiTietEdit.CellClick -= DgvChiTietEdit_CellContentClick;
            dgvChiTietEdit.CellClick += DgvChiTietEdit_CellContentClick; 
            dgvChiTietEdit.CellValueChanged -= DgvChiTietEdit_CellValueChanged;
            dgvChiTietEdit.CellValueChanged += DgvChiTietEdit_CellValueChanged; 
            dgvChiTietEdit.RowsRemoved -= DgvChiTietEdit_RowsRemoved;
            dgvChiTietEdit.RowsRemoved += DgvChiTietEdit_RowsRemoved; 
            dgvChiTietEdit.DataError += DgvChiTietEdit_DataError; 
            btnXuatHang.Text = "Lưu Thay Đổi";
            UpdateControlsForOrderType();
            EnableTaoPhieuButton();
        }

        private async void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop(); 
            string searchQuery = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                await PerformDatabaseSearchAndSuggest(searchQuery);
            }
            else
            {
                ketqua.Visible = false;
            }
        }
        private async Task PerformDatabaseSearchAndSuggest(string searchQuery)
        {
            int? maKhoFilter = null;
            if (CbKhoNguon.SelectedValue is int selectedMaKhoNguon && selectedMaKhoNguon > 0)
            {
                maKhoFilter = selectedMaKhoNguon;
            }

            try
            {
                QLVatLieu qlv = new QLVatLieu();

                List<DTO_VatLieu> results = await Task.Run(() => qlv.TimKiemVatLieuTheoKho(maKhoFilter, searchQuery));
                Console.WriteLine($"DB Search Query: {searchQuery}, Kho Filter: {maKhoFilter}, Results Found: {results.Count}");
                UpdateSearchSuggestions(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm vật liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ketqua.Visible = false;
            }
        }
     
        private void UpdateSearchSuggestions(List<DTO_VatLieu> searchResults)
        {
            ketqua.SuspendLayout();
            ketqua.Controls.Clear();
            if (searchResults != null && searchResults.Any())
            {
                ketqua.Height = Math.Min(searchResults.Count * 35, 210);
                ketqua.BackColor = Color.WhiteSmoke;
                foreach (var item in searchResults)
                {
                    Label lbl = new Label
                    {
                        Text = $"{item.Ten} ({item.MaVatLieu}) - {item.DonViTinh}",
                        Tag = item,
                        Dock = DockStyle.Top,
                        Height = 35,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Padding = new Padding(10, 0, 0, 0),
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = Color.FromArgb(33, 37, 41),
                        BackColor = defaultLabelColor,
                        Cursor = Cursors.Hand,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    lbl.MouseEnter += (s, e) => { lbl.BackColor = hoverLabelColor; lbl.ForeColor = Color.FromArgb(0, 102, 204); };
                    lbl.MouseLeave += (s, e) => { lbl.BackColor = defaultLabelColor; lbl.ForeColor = Color.FromArgb(33, 37, 41); };
                    lbl.Click += (s, e) =>
                    {
                        if (lbl.Tag is DTO_VatLieu selectedItem)
                        {
                                AddVatLieuToDgvChiTiet(selectedItem);
                                ketqua.Visible = false;
                                txtSearch.Text = string.Empty;
                                txtSearch.Focus();
                        }
                    };
                    ketqua.Controls.Add(lbl);
                    lbl.BringToFront();
                }
                ketqua.Visible = true;
            }
            else
            {
                ketqua.Visible = false;
            }
            ketqua.ResumeLayout();
        }

        private void SetupEditDataGridView()
        {
            dgvChiTietEdit.AutoGenerateColumns = false;
            dgvChiTietEdit.Columns.Clear();

            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaVatLieu",
                HeaderText = "Mã",
                DataPropertyName = "MaVatLieu",
                ReadOnly = true,
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenVatLieu",
                HeaderText = "Tên Vật Liệu",
                DataPropertyName = "TenVatLieu",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
             dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKho", 
                 HeaderText = "Kho Nguồn", 
                 DataPropertyName = "TenKho", 
                 ReadOnly = true, 
                 Width = 120 
             });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonViTinh",
                HeaderText = "ĐVT",
                DataPropertyName = "DonViTinh",
                ReadOnly = true,
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong",
                HeaderText = "Số Lượng",
                DataPropertyName = "SoLuong",
                ReadOnly = false,
                ValueType = typeof(int),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 90
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonGia",
                HeaderText = "Giá Bán",
                DataPropertyName = "DonGia", 
                ReadOnly = false,
                ValueType = typeof(decimal),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 110
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành Tiền",
                ReadOnly = true,
                ValueType = typeof(decimal),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 120
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "+", Text = "+", UseColumnTextForButtonValue = true, Width = 40, FlatStyle = FlatStyle.System });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "-", Text = "-", UseColumnTextForButtonValue = true, Width = 40, FlatStyle = FlatStyle.System });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xóa", Text = "X", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System });
            this.Load += (s, e) => ResizeColumns();
        }

        private void ConfigureDgvChiTietStyles()
        {
            dgvChiTietEdit.BorderStyle = BorderStyle.None;
            dgvChiTietEdit.GridColor = Color.Gainsboro;
            dgvChiTietEdit.RowHeadersVisible = false;
            dgvChiTietEdit.BackgroundColor = Color.White;
            dgvChiTietEdit.EnableHeadersVisualStyles = false;
            dgvChiTietEdit.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(66, 139, 202);
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTietEdit.ColumnHeadersHeight = 40;
            dgvChiTietEdit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTietEdit.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
            dgvChiTietEdit.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTietEdit.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvChiTietEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvChiTietEdit.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvChiTietEdit.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvChiTietEdit.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTietEdit.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgvChiTietEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dgvChiTietEdit.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvChiTietEdit.AllowUserToResizeColumns = false;
            dgvChiTietEdit.AllowUserToResizeRows = false;
            if (dgvChiTietEdit.Columns.Contains("MaVatLieu")) dgvChiTietEdit.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (dgvChiTietEdit.Columns.Contains("DonViTinh")) dgvChiTietEdit.Columns["DonViTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (dgvChiTietEdit.Columns.Contains("SoLuong")) dgvChiTietEdit.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (dgvChiTietEdit.Columns.Contains("DonGia")) dgvChiTietEdit.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
            if (dgvChiTietEdit.Columns.Contains("ThanhTien")) dgvChiTietEdit.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void ResizeColumns()
        {
            if (dgvChiTietEdit.Columns.Count == 0 || dgvChiTietEdit.ClientSize.Width <= 0) return;
            try
            {
                int availableWidth = dgvChiTietEdit.ClientSize.Width;
                if (dgvChiTietEdit.Controls.OfType<VScrollBar>().Any(v => v.Visible))
                {
                    availableWidth -= SystemInformation.VerticalScrollBarWidth;
                }
                int fixedWidthTotal = 0;
                int fillColumnsCount = 0;
                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
                 {
                     { "MaVatLieu", 80 },
                     { "TenKho", 120 }, 
                     { "DonViTinh", 80 },
                     { "SoLuong", 90 },
                     { "DonGia", 100 }, 
                     { "ThanhTien", 120 },
                     { "Tang", 40 },
                     { "Giam", 40 },
                     { "Xoa", 50 }
                 };

                DataGridViewColumn tenVatLieuCol = null;

                foreach (DataGridViewColumn col in dgvChiTietEdit.Columns)
                {
                    if (!col.Visible) continue;
                    if (fixedWidths.ContainsKey(col.Name))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        int targetWidth = fixedWidths[col.Name];
                        col.Width = Math.Max(20, targetWidth);
                        fixedWidthTotal += col.Width;
                    }
                    else if (col.Name == "TenVatLieu")
                    {
                        tenVatLieuCol = col;
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        fillColumnsCount++;
                    }
                    else { col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; fixedWidthTotal += col.Width; }
                }
                if (tenVatLieuCol != null) { tenVatLieuCol.MinimumWidth = 150; }
            }
            catch (Exception ex) { Console.WriteLine($"Error during column resize: {ex.Message}"); }
        }

        private  bool  LoadOrderDataForEditing() 
        {
            if (Id <= 0) return false;
            try
            {
                _originalOrderData = bus.GetDonXuatById(Id); 

                if (_originalOrderData != null && _originalOrderData.ChitietDonXuat != null)
                {
                    ID.Text = _originalOrderData.MaDonXuat.ToString();
                    dtpNgayXuat.Value = _originalOrderData.NgayXuat;
                    dtpNgayXuat.MaxDate = DateTime.Now.Date;
                    Tbnote.Text = _originalOrderData.GhiChu;
                    lblNgTao.Text = _originalOrderData.TenNguoiLap ?? "N/A"; 

                    if (_dtAllKhos == null)
                    {
                        _dtAllKhos = bUS_VatLieu.LayDanhSachKho();
                        if (_dtAllKhos == null)
                        {
                            MessageBox.Show("Không thể lấy danh sách kho.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    string displayMemberName = "TenKho";
                    string valueMemberName = "MaKho";
                    DataTable dtKhoNguon = _dtAllKhos.Copy();
                    DataRow allRowNguon = dtKhoNguon.NewRow();
                    allRowNguon[displayMemberName] = "-- Chọn Kho Nguồn --";
                    allRowNguon[valueMemberName] = -1;
                    dtKhoNguon.Rows.InsertAt(allRowNguon, 0);
                    CbKhoNguon.DataSource = null;
                    CbKhoNguon.DisplayMember = displayMemberName;
                    CbKhoNguon.ValueMember = valueMemberName;
                    CbKhoNguon.DataSource = dtKhoNguon;
                    CbKhoNguon.SelectedValue = _originalOrderData.Makhonguon;
                    if (CbKhoNguon.SelectedValue == null) CbKhoNguon.SelectedIndex = -1;
                    DataTable dtKhoDich = _dtAllKhos.Copy();
                    CbKhoDich.DataSource = null;
                    CbKhoDich.DisplayMember = displayMemberName;
                    CbKhoDich.ValueMember = valueMemberName;
                    CbKhoDich.DataSource = dtKhoDich;
                    CbKhoDich.SelectedValue = _originalOrderData.MaKhoDich ?? -1; 
                    if (CbKhoDich.SelectedValue == null) CbKhoDich.SelectedIndex = -1;

                     SearchKh.Tag = _originalOrderData.MaKhachHang; 
                     SearchKh.Text = _originalOrderData.TenKhachHang;
                    if (_originalOrderData != null && _originalOrderData.ChitietDonXuat != null)
                    {
                        _editedDetailsBindingList = new BindingList<DTO_ChiTietDonXuat>(_originalOrderData.ChitietDonXuat);
                        dgvChiTietEdit.DataSource = _editedDetailsBindingList;
                    }
                    else // Trường hợp _originalOrderData là null HOẶC ChitietDonXuat là null
                    {
                        MessageBox.Show("Không tìm thấy thông tin đơn hàng hoặc chi tiết đơn hàng.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Trả về false, form sẽ đóng lại
                    }

                    CalculateThanhTienForAllRows();

                    TgTrangthai.Checked = _originalOrderData.LoaiXuat == "Chuyển kho"; 

                    return true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn hàng để sửa.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn hàng để sửa: {ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


        private void TinhTongSoLuongVaTienBan()
        {
            int tongSoLuong = 0;
            decimal tongTienBan = 0;

            if (_editedDetailsBindingList != null)
            {
                foreach (var item in _editedDetailsBindingList)
                {
                    tongSoLuong += item.SoLuong;
                    // *** SỬA LẠI: Dùng DonGia ***
                    tongTienBan += item.SoLuong * item.DonGia; // Giả sử DTO có DonGia
                }
            }

            lblSl.Text = tongSoLuong.ToString("N0");
            lblTongTien.Text = tongTienBan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private void DgvChiTietEdit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
                if (colName == "SoLuong" || colName == "DonGia")
                {
                    DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
                    ValidateCellInput(row.Cells[e.ColumnIndex]); 
                    UpdateThanhTienForRow(row); 
                    TinhTongSoLuongVaTienBan(); 
                }
            }
        }

        private void ValidateCellInput(DataGridViewCell cell)
        {
            if (cell == null || cell.OwningRow.IsNewRow) return;
            string colName = cell.OwningColumn.Name;
            object cellValue = cell.Value;
            bool isValid = true;
            string errorMessage = string.Empty;
            int maVL = Convert.ToInt32(cell.OwningRow.Cells["MaVatLieu"].Value); // Lấy mã VL để kiểm tra tồn kho

            if (colName == "SoLuong")
            {
                if (cellValue == null || !int.TryParse(cellValue.ToString(), out int sl) || sl <= 0)
                {
                    isValid = false;
                    errorMessage = "Số lượng phải là số nguyên lớn hơn 0.";
                    cell.Value = 1; // Đặt lại giá trị mặc định
                }
                else
                {
                    int maKhoNguon = -1;
                    if (CbKhoNguon.SelectedValue != null && CbKhoNguon.SelectedValue != DBNull.Value)
                    {
                        int.TryParse(CbKhoNguon.SelectedValue.ToString(), out maKhoNguon);
                    }
                    if (maKhoNguon > 0)
                    {
                        var originalDetail = _originalOrderData?.ChitietDonXuat.FirstOrDefault(d => d.MaVatLieu == maVL);
                        int soLuongGoc = originalDetail?.SoLuong ?? 0;
                        int tonKhoHienTai = bUS_VatLieu.GetSoLuongTonKho(maVL, maKhoNguon); 
                        int tonKhoKhaDung = tonKhoHienTai + soLuongGoc; 

                        if (sl > tonKhoKhaDung)
                        {
                            isValid = false;
                            errorMessage = $"Số lượng xuất ({sl}) vượt quá số lượng tồn khả dụng ({tonKhoKhaDung}) tại kho nguồn.";
                            cell.Value = Math.Max(1, soLuongGoc); 
                        }
                    }
                    else
                    {
                        isValid = false; 
                        errorMessage = "Chưa chọn kho nguồn hợp lệ để kiểm tra tồn kho.";
                        cell.Value = 1;
                    }
                }
            }
            else if (colName == "DonGia") // *** SỬA LẠI: Kiểm tra cột DonGia ***
            {
                if (cellValue == null || !decimal.TryParse(cellValue.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal dg) || dg < 0)
                {
                    isValid = false;
                    errorMessage = "Giá bán phải là số không âm.";
                    cell.Value = 0m; // Đặt lại giá trị mặc định
                }
            }

            if (!isValid) { MessageBox.Show(errorMessage, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void CalculateThanhTienForAllRows()
        {
            if (dgvChiTietEdit.Columns.Contains("ThanhTien")) { foreach (DataGridViewRow row in dgvChiTietEdit.Rows) { UpdateThanhTienForRow(row); } }
        }
        private void UpdateThanhTienForRow(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow) return;
            // *** SỬA LẠI: Dùng cột DonGia ***
            if (dgvChiTietEdit.Columns.Contains("SoLuong") && dgvChiTietEdit.Columns.Contains("DonGia") && dgvChiTietEdit.Columns.Contains("ThanhTien") && row.Cells["SoLuong"].Value != null && row.Cells["DonGia"].Value != null)
            {
                int.TryParse(row.Cells["SoLuong"].Value.ToString(), out int sl);
                decimal.TryParse(row.Cells["DonGia"].Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal dg);
                row.Cells["ThanhTien"].Value = sl * dg;
            }
        }

        // --- Xử lý khi xóa dòng ---
        private void DgvChiTietEdit_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            TinhTongSoLuongVaTienBan();
            EnableTaoPhieuButton(); // Cập nhật trạng thái nút Lưu
        }

        // --- Xử lý lỗi nhập liệu Grid ---
        private void DgvChiTietEdit_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            string errorText = $"Lỗi dữ liệu tại dòng {e.RowIndex + 1}, cột '{dgvChiTietEdit.Columns[e.ColumnIndex].HeaderText}':\n{e.Exception.Message}";
            MessageBox.Show(errorText, "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
            if (colName == "SoLuong") dgvChiTietEdit.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
            else if (colName == "DonGia") dgvChiTietEdit.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0m; // Sửa thành DonGia
            e.Cancel = false;
        }

        // --- Xử lý click nút trên Grid ---
        private void DgvChiTietEdit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            if (colName == "Tang" || colName == "Giam")
            {
                if (row.Cells["SoLuong"].Value != null && int.TryParse(row.Cells["SoLuong"].Value.ToString(), out int currentSoLuong))
                {
                    int newSoLuong = currentSoLuong;
                    if (colName == "Tang") { newSoLuong = currentSoLuong + 1; }
                    else if (currentSoLuong > 1) { newSoLuong = currentSoLuong - 1; }

                    if (newSoLuong != currentSoLuong)
                    {
                        int maVL = Convert.ToInt32(row.Cells["MaVatLieu"].Value);
                        int maKhoNguon = -1;
                        if (CbKhoNguon.SelectedValue != null && CbKhoNguon.SelectedValue != DBNull.Value)
                        {
                            int.TryParse(CbKhoNguon.SelectedValue.ToString(), out maKhoNguon);
                        }
                        if (maKhoNguon > 0)
                        {
                            var originalDetail = _originalOrderData?.ChitietDonXuat.FirstOrDefault(d => d.MaVatLieu == maVL);
                            int soLuongGoc = originalDetail?.SoLuong ?? 0;
                            int tonKhoHienTai = bUS_VatLieu.GetSoLuongTonKho(maVL, maKhoNguon);
                            int tonKhoKhaDung = tonKhoHienTai + soLuongGoc;

                            if (newSoLuong <= tonKhoKhaDung)
                            {
                                row.Cells["SoLuong"].Value = newSoLuong;
                                // CellValueChanged sẽ tự cập nhật Thành tiền và Tổng
                            }
                            else
                            {
                                MessageBox.Show($"Số lượng xuất ({newSoLuong}) vượt quá số lượng tồn khả dụng ({tonKhoKhaDung}) tại kho nguồn.", "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Chưa chọn kho nguồn hợp lệ để kiểm tra tồn kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (colName == "Giam")
                    {
                        // Đã ở mức 1, không giảm được nữa
                        MessageBox.Show("Số lượng không thể nhỏ hơn 1.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (colName == "Xoa") // Nếu là nút Xóa
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvChiTietEdit.Rows.Count <= 1 && !dgvChiTietEdit.Rows[0].IsNewRow) // Kiểm tra nếu chỉ còn 1 dòng thực tế
                    {
                        MessageBox.Show("Đơn xuất hàng phải có ít nhất một chi tiết.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Xóa dòng khỏi BindingList nếu đang dùng
                    if (_editedDetailsBindingList != null && row.DataBoundItem is DTO_ChiTietDonXuat itemToRemove)
                    {
                        _editedDetailsBindingList.Remove(itemToRemove);
                    }
                    else if (!row.IsNewRow) // Nếu không dùng BindingList
                    {
                        dgvChiTietEdit.Rows.Remove(row);
                    }
                    // RowsRemoved sẽ tự tính lại tổng
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnXuatHang_Click(object sender, EventArgs e) // Đổi tên thành btnLuuThayDoi_Click?
        {
            if (!DateTime.TryParse(dtpNgayXuat.Value.ToString(), out DateTime ngayXuatMoi))
            { MessageBox.Show("Ngày xuất không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string ghiChuMoi = Tbnote.Text.Trim();
            int maKhachHang = -1;
            int maKhoNguon = -1;
            int? maKhoDich = null;
            string loaiPhieu = TgTrangthai.Checked ? "PXCK" : "PX"; 

            if (TgTrangthai.Checked) // Chuyển kho
            {
                if (CbKhoNguon.SelectedValue == null || !int.TryParse(CbKhoNguon.SelectedValue.ToString(), out maKhoNguon) || maKhoNguon <= 0)
                { MessageBox.Show("Vui lòng chọn Kho nguồn hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (CbKhoDich.SelectedValue == null || !int.TryParse(CbKhoDich.SelectedValue.ToString(), out int mkd) || mkd <= 0)
                { MessageBox.Show("Vui lòng chọn Kho đích hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (maKhoNguon == mkd)
                { MessageBox.Show("Kho nguồn và Kho đích không được trùng nhau.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                maKhoDich = mkd;
            }
            else 
            {
                if (CbKhoNguon.SelectedValue == null || !int.TryParse(CbKhoNguon.SelectedValue.ToString(), out maKhoNguon) || maKhoNguon <= 0)
                { MessageBox.Show("Vui lòng chọn Kho nguồn hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (SearchKh.Tag == null || !int.TryParse(SearchKh.Tag.ToString(), out maKhachHang) || maKhachHang <= 0)
                { MessageBox.Show("Vui lòng chọn Khách hàng hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            }


            if (_editedDetailsBindingList == null || !_editedDetailsBindingList.Any())
            { MessageBox.Show("Đơn xuất hàng phải có ít nhất một chi tiết.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            List<DTO_ChiTietDonXuat> chiTietDaSua = new List<DTO_ChiTietDonXuat>();
            List<string> validationErrors = new List<string>();

            foreach (var detail in _editedDetailsBindingList) 
            {
                if (detail.SoLuong <= 0) { validationErrors.Add($"Vật liệu (Mã: {detail.MaVatLieu}): Số lượng phải lớn hơn 0."); }
                if (detail.DonGia < 0) { validationErrors.Add($"Vật liệu (Mã: {detail.MaVatLieu}): Giá bán không được âm."); }

                var originalDetail = _originalOrderData?.ChitietDonXuat.FirstOrDefault(d => d.MaVatLieu == detail.MaVatLieu);
                int soLuongGoc = originalDetail?.SoLuong ?? 0;
                int tonKhoHienTai = bUS_VatLieu.GetSoLuongTonKho(detail.MaVatLieu, maKhoNguon);
                int tonKhoKhaDung = tonKhoHienTai + soLuongGoc;
                if (detail.SoLuong > tonKhoKhaDung)
                {
                    validationErrors.Add($"Vật liệu (Mã: {detail.MaVatLieu}): Số lượng xuất ({detail.SoLuong}) vượt quá tồn khả dụng ({tonKhoKhaDung}).");
                }

                chiTietDaSua.Add(new DTO_ChiTietDonXuat
                {
                    MaDonXuat = Id, 
                    MaVatLieu = detail.MaVatLieu,
                    SoLuong = detail.SoLuong,
                    DonGia = detail.DonGia 
                });
            }

            if (validationErrors.Any())
            { MessageBox.Show("Dữ liệu chi tiết không hợp lệ:\n- " + string.Join("\n- ", validationErrors), "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }


            bool success = false;
            try
            {
                DTO_DonXuat headerUpdate = new DTO_DonXuat
                {
                    MaDonXuat = Id,
                    NgayXuat = ngayXuatMoi,
                    GhiChu = ghiChuMoi,
                    MaKhachHang = loaiPhieu == "PX" ? maKhachHang : null, 
                    Makhonguon = maKhoNguon,
                    MaKhoDich = maKhoDich, 
                    LoaiXuat = loaiPhieu,
                    MaTK = _originalOrderData.MaTK,
                    NguoiCapNhat = CurrentUser.MaTK
                };

                btnXuatHang.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                success = await bus.UpdateDonXuatAsync(headerUpdate,
                                    _originalOrderData.ChitietDonXuat,
                                    chiTietDaSua);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi đơn xuất:\n{ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = false;
            }
            finally
            {
                btnXuatHang.Enabled = true;
                this.Cursor = Cursors.Default;
            }

            if (success)
            {
                MessageBox.Show("Lưu thay đổi đơn xuất hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                this.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { debounceTimer.Stop(); debounceTimer.Start(); }
        private void CbKhoNguon_SelectedIndexChanged(object sender, EventArgs e) { FilterKhoDichComboBox(); EnableTaoPhieuButton(); } 
        private void FilterKhoDichComboBox() {/* ... code lọc CbKhoDich dựa trên CbKhoNguon ... */}
        private void TgTrangthai_CheckedChanged(object sender, EventArgs e) { UpdateControlsForOrderType(); EnableTaoPhieuButton(); }
        private void UpdateControlsForOrderType() {/* ... code ẩn/hiện control theo TgTrangthai ... */}
        private void EnableTaoPhieuButton() {/* ... code kiểm tra điều kiện để bật/tắt nút Lưu ... */}
        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            int selectedMaKhoNguon = -1;
            if (CbKhoNguon.SelectedValue is int mk) { selectedMaKhoNguon = mk; }
            else if (CbKhoNguon.SelectedValue is long mkl) { selectedMaKhoNguon = (int)mkl; } // Xử lý nếu ValueMember là long
            else if (CbKhoNguon.SelectedValue != null) { int.TryParse(CbKhoNguon.SelectedValue.ToString(), out selectedMaKhoNguon); }


            if (selectedMaKhoNguon <= 0)
            {
                MessageBox.Show("Vui lòng chọn Kho nguồn hợp lệ.", "Thiếu Kho Nguồn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbKhoNguon.Focus();
                return;
            }

            int tonKho = bUS_VatLieu.GetSoLuongTonKho(selectedItem.MaVatLieu, selectedMaKhoNguon); // Cần hàm LayTonKho
            if (tonKho <= 0)
            {
                MessageBox.Show($"Vật liệu '{selectedItem.Ten}' đã hết hàng tại kho '{CbKhoNguon.Text}'.", "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TgTrangthai.Checked) 
            {
            }
            else 
            {
                if (selectedItem.DonGiaBan == null || selectedItem.DonGiaBan < 0)
                {
                    MessageBox.Show($"Vật liệu '{selectedItem.Ten}' chưa có giá bán hợp lệ.", "Thiếu Giá Bán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (_editedDetailsBindingList != null && _editedDetailsBindingList.Any(d => d.MaVatLieu == selectedItem.MaVatLieu))
            {
                MessageBox.Show($"Vật liệu '{selectedItem.Ten}' đã có trong danh sách.", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DTO_ChiTietDonXuat newItem = new DTO_ChiTietDonXuat
            {
                MaDonXuat = this.Id, 
                MaVatLieu = selectedItem.MaVatLieu,
                TenVatLieu = selectedItem.Ten, 
                DonViTinh = selectedItem.DonViTinh, 
                SoLuong = 1,
                DonGia = TgTrangthai.Checked ? 0m : selectedItem.DonGiaBan
            };

            if (_editedDetailsBindingList != null)
            {
                _editedDetailsBindingList.Add(newItem);
                TinhTongSoLuongVaTienBan(); 
                EnableTaoPhieuButton();

                dgvChiTietEdit.ClearSelection();
                int newRowIndex = dgvChiTietEdit.Rows.Count - 1; 
                for (int i = 0; i < dgvChiTietEdit.Rows.Count; i++)
                {
                    if (dgvChiTietEdit.Rows[i].DataBoundItem == newItem)
                    {
                        newRowIndex = i;
                        break;
                    }
                }

                if (newRowIndex >= 0)
                {
                    dgvChiTietEdit.Rows[newRowIndex].Selected = true;
                    if (dgvChiTietEdit.Columns.Contains("SoLuong")) { dgvChiTietEdit.CurrentCell = dgvChiTietEdit.Rows[newRowIndex].Cells["SoLuong"]; }
                }
            }
            else
            {
                MessageBox.Show("Lỗi: Không tìm thấy danh sách chi tiết để thêm vào.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    } 

}
