using BUS;
using DAL;
using DTO;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualBasic;
using QLVT.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GUI.Helpler;
using System.Data;

namespace QLVT
{
    public partial class AddXuat : Form
    {
        private readonly BUS_Order busOrder = new BUS_Order();
        private readonly BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private BUS_HoaDon Bus = new BUS_HoaDon();
        private readonly BUS_TK busTk = new BUS_TK();
        private readonly BUS_Ncc busNcc = new BUS_Ncc();
        private readonly BUS_DonXuat bus = new BUS_DonXuat();
        private List<VatLieuExcelDayDu> _loadedExcelData = null;
        private System.Windows.Forms.Timer _debounceSearchTimer;
        private System.Windows.Forms.Timer _debounceKhachTimer;
        private System.Windows.Forms.Timer _debounceHoaDonTimer;
        private bool _isImportMode = false;
        private bool _isFilteringDgv = false;
        private Form1 form1;
        private readonly Color defaultLabelColor = Color.White;
        private readonly Color hoverLabelColor = Color.FromArgb(230, 240, 255);
        private readonly BUS_Khach bUS_Khach = new BUS_Khach();
        private List<DTO_Khach> danhSachKhach = new List<DTO_Khach>();
        private List<DTO_HoaDon> danhSachHd = new List<DTO_HoaDon>();
        private DataTable _dtAllKhos = new DataTable();

        public AddXuat()
        {
            InitializeComponent();
            LoadInitialData();
            InitializeCustomComponents();
            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm");
            dtpNgayXuat.Value = DateTime.Now.Date;
            dtpNgayXuat.MaxDate = DateTime.Now.Date;
            CbKhoDich.Visible = false;
            label1.Visible = false;
            CbKhoDich.Enabled = false;
        }

        private void InitializeCustomComponents()
        {
            this.Resize += Frm_Resize;
            _debounceSearchTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _debounceSearchTimer.Tick += DebounceSearchTimer_Tick;

            _debounceKhachTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _debounceKhachTimer.Tick += DebounceKhachTimer_Tick; ;

            _debounceHoaDonTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _debounceHoaDonTimer.Tick += DebounceHoaDonTimer_Tick; ;
            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm mã, tên vật liệu...");
            ConfigureDgvChiTiet();
            ConfigureDgvChiTietStyles();
            TinhTongSoLuongVaTienBan();
            CenterPanelInDataGridView();
            panelImportExcel.Visible = (dgvChiTiet.RowCount == 0);
        }

        private void ConfigureDgvChiTiet()
        {
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Clear();
            dgvChiTiet.AllowUserToResizeColumns = false;
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã VL", ReadOnly = true, ValueType = typeof(int), Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên Vật Liệu", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 150 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKho", HeaderText = "Kho Nguồn", ReadOnly = true, Width = 100 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "ĐVT", ReadOnly = true, Width = 70 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", ReadOnly = false, ValueType = typeof(int), Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaBan", HeaderText = "Giá Bán", ReadOnly = false, ValueType = typeof(decimal), Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền", ReadOnly = true, ValueType = typeof(decimal), Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            var btnStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 30, FlatStyle = FlatStyle.System, DefaultCellStyle = btnStyle });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 30, FlatStyle = FlatStyle.System, DefaultCellStyle = btnStyle });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xoá", Text = "X", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.Flat, DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Red, Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvChiTiet.CellValueChanged += dgvChiTiet_CellValueChanged;
            dgvChiTiet.CellClick += DgvChiTiet_CellClick;
            dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            dgvChiTiet.RowsAdded += (s, e) => UpdateControlsState();
            dgvChiTiet.RowsRemoved += (s, e) => UpdateControlsState();
            dgvChiTiet.DataError += DgvChiTiet_DataError;
            ResizeColumns();
        }

        private void DgvChiTiet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine($"DataError in DGV: Row {e.RowIndex}, Col {e.ColumnIndex}. Context: {e.Context}. Error: {e.Exception.Message}");
            MessageBox.Show($"Lỗi nhập liệu tại ô [{dgvChiTiet.Columns[e.ColumnIndex].HeaderText}]:\n{e.Exception.Message}\nVui lòng nhập lại giá trị hợp lệ.",
                            "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.ThrowException = false;
            e.Cancel = false;
        }

        private void UpdateControlsState()
        {
            bool hasItems = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);

            panelImportExcel.Visible = !_isImportMode && !hasItems;
            CenterPanelInDataGridView();

            EnableTaoPhieuButton();

            TinhTongSoLuongVaTienBan();
        }

        private void UpdatePanelVisibility()
        {
            panelImportExcel.Visible = !_isImportMode && !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            CenterPanelInDataGridView();
        }

        private void ConfigureDgvChiTietStyles()
        {
            dgvChiTiet.BorderStyle = BorderStyle.None;
            dgvChiTiet.GridColor = Color.Gainsboro;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(66, 139, 202);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTiet.ColumnHeadersHeight = 40;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
            dgvChiTiet.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTiet.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvChiTiet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvChiTiet.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvChiTiet.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTiet.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgvChiTiet.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dgvChiTiet.DefaultCellStyle.SelectionForeColor = Color.White;
            if (dgvChiTiet.Columns.Contains("MaVatLieu"))
            {
                dgvChiTiet.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvChiTiet.Columns.Contains("SoLuong"))
            {
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
            }
            dgvChiTiet.RowTemplate.Height = 35;
        }

        private async void LoadInitialData()
        {
            try
            {
                if (CurrentUser.TenDangNhap != null)
                {
                    lblNgTao.Text = CurrentUser.TenDangNhap;
                }
                else
                {
                    lblNgTao.Text = "N/A";
                    MessageBox.Show("Không thể xác định người dùng hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                _dtAllKhos = await Task.Run(() => busVatLieu.LayDanhSachKho());

                if (_dtAllKhos == null)
                {
                    MessageBox.Show("Không thể lấy danh sách kho.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _dtAllKhos = new DataTable();
                    if (!_dtAllKhos.Columns.Contains("MaKho")) _dtAllKhos.Columns.Add("MaKho", typeof(int));
                    if (!_dtAllKhos.Columns.Contains("TenKho")) _dtAllKhos.Columns.Add("TenKho", typeof(string));
                }
                string displayMemberName = "TenKho";
                string valueMemberName = "MaKho";
                DataTable dtKhoNguon = _dtAllKhos.Copy();
                DataRow allRowNguon = dtKhoNguon.NewRow();
                allRowNguon[displayMemberName] = "-- Tất cả Kho --";
                allRowNguon[valueMemberName] = -1;
                dtKhoNguon.Rows.InsertAt(allRowNguon, 0);
                CbKhoNguon.DataSource = null;
                CbKhoNguon.DisplayMember = displayMemberName;
                CbKhoNguon.ValueMember = valueMemberName;
                CbKhoNguon.DataSource = dtKhoNguon;
                CbKhoNguon.SelectedIndex = 0;
                DataTable dtKhoDich = _dtAllKhos.Copy();
                CbKhoDich.DataSource = null;
                CbKhoDich.DisplayMember = displayMemberName;
                CbKhoDich.ValueMember = valueMemberName;
                CbKhoDich.DataSource = dtKhoDich;
                CbKhoDich.SelectedIndex = -1;
                danhSachKhach = await Task.Run(() => bUS_Khach.LayDanhSachKhach());
                danhSachHd = await Task.Run(() => Bus.LayTatCaHoaDon());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DebounceSearchTimer_Tick(object sender, EventArgs e)
        {
            _debounceSearchTimer.Stop();
            string searchQuery = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ketqua.Visible = false;
                if (_isImportMode) FilterAndDisplayDgvChiTiet();
                return;
            }

            if (_isImportMode)
            {
                FilterAndDisplayDgvChiTiet();
                await PerformExcelDataSearchAndSuggest(searchQuery);
            }
            else
            {
                await PerformDatabaseSearchAndSuggest(searchQuery);
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

        private async Task PerformExcelDataSearchAndSuggest(string searchQuery)
        {
            if (_loadedExcelData == null || !_loadedExcelData.Any())
            {
                ketqua.Visible = false;
                return;
            }
            string lowerQuery = searchQuery.ToLowerInvariant();
            var results = _loadedExcelData
                .Where(d => d.RowError == null &&
                            (
                                (d.MaVatLieuStr?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                (d.TenVatLieu?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                (d.DonViTinh?.ToLowerInvariant().Contains(lowerQuery) ?? false)
                            ))
                .Select(d => new DTO_VatLieu
                {
                    MaVatLieu = int.TryParse(d.MaVatLieuStr, out int ma) ? ma : 0,
                    Ten = d.TenVatLieu,
                    DonViTinh = d.DonViTinh,
                })
                .DistinctBy<DTO_VatLieu, object>(v => v.MaVatLieu > 0 ? v.MaVatLieu : (object)(v.Ten + v.DonViTinh))
                .OrderBy(v => v.Ten)
                .ToList();
            Console.WriteLine($"Excel Data Search Query: {searchQuery}, Suggestion Results Found: {results.Count}");
            UpdateSearchSuggestions(results);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _debounceSearchTimer.Stop();
            _debounceSearchTimer.Start();
        }

        private void DgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvChiTiet.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
                if (row.IsNewRow) return;
                string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
                int currentSoLuong = 0;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out currentSoLuong);
                switch (columnName)
                {
                    case "Tang":
                        row.Cells["SoLuong"].Value = Math.Max(1, currentSoLuong + 1);
                        break;

                    case "Giam":
                        if (currentSoLuong > 1)
                        {
                            row.Cells["SoLuong"].Value = currentSoLuong - 1;
                        }
                        break;

                    case "Xoa":
                        dgvChiTiet.Rows.RemoveAt(e.RowIndex);
                        break;
                }
            }
        }

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
            bool isValid = true;
            string errorMsg = "";

            if (columnName == "SoLuong")
            {
                if (!int.TryParse(cell.Value?.ToString(), out int soLuong) || soLuong <= 0)
                {
                    errorMsg = "Số lượng phải là số nguyên lớn hơn 0.";
                    cell.Value = 1;
                    isValid = false;
                }
            }
            else if (columnName == "GiaBan")
            {
                if (!decimal.TryParse(cell.Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaBan) || giaBan < 0)
                {
                    errorMsg = "Giá bán phải là số không âm.";
                    cell.Value = 0m;
                    isValid = false;
                }
            }

            if (!isValid)
            {
                MessageBox.Show(errorMsg, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvChiTiet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
            if (columnName == "SoLuong" || columnName == "GiaBan")
            {
                DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                decimal.TryParse(row.Cells["GiaBan"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaBan);

                if (dgvChiTiet.Columns.Contains("ThanhTien"))
                {
                    row.Cells["ThanhTien"].Value = soLuong * giaBan;
                }

                TinhTongSoLuongVaTienBan();
            }
        }

        private void TinhTongSoLuongVaTienBan()
        {
            int tongSoLuong = 0;
            decimal tongTienBan = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                decimal.TryParse(row.Cells["GiaBan"].Value?.ToString(),
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.CurrentCulture,
                                 out decimal giaBan);
                tongSoLuong += soLuong;
                tongTienBan += giaBan * soLuong;
            }
            lblSl.Text = tongSoLuong.ToString("N0");
            lblTongTien.Text = tongTienBan.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private async void btnTaoPhieuXuat_Click(object sender, EventArgs e)
        {
            if (_isImportMode) { /* Xử lý Excel */ return; }

            DTO_DonXuatInput donXuatInput = new DTO_DonXuatInput();
            int? selectedMaKhoNguonFinal = null;

            if (TgTrangthai.Checked)
            {
                donXuatInput.LoaiXuat = "Chuyển kho";
                donXuatInput.MaKhachHang = null;
                donXuatInput.MaHoaDon = null;

                if (CbKhoNguon.SelectedValue is int maKhoN && maKhoN > 0)
                {
                    selectedMaKhoNguonFinal = maKhoN;
                    Console.WriteLine($"Debug: Selected Kho Nguon Value = {selectedMaKhoNguonFinal}");
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Kho nguồn HỢP LỆ.", "Thiếu Kho Nguồn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CbKhoNguon.Focus(); return;
                }

                if (!(CbKhoDich.SelectedValue is int maKhoD) || maKhoD <= 0) { MessageBox.Show("Vui lòng chọn Kho đích.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); CbKhoDich.Focus(); return; }
                if (selectedMaKhoNguonFinal == maKhoD) { MessageBox.Show("Kho nguồn và Kho đích không được trùng nhau.", "Lỗi logic", MessageBoxButtons.OK, MessageBoxIcon.Warning); CbKhoDich.Focus(); return; } // Focus kho đích nếu trùng

                donXuatInput.GhiChu = $"Chuyển từ kho: {CbKhoNguon.Text} [{selectedMaKhoNguonFinal}] đến kho: {CbKhoDich.Text} [{maKhoD}]. {Tbnote.Text.Trim()}";
            }
            else
            {
                donXuatInput.LoaiXuat = "Xuất hàng";
                if (SearchKh.Tag is int maKH && maKH > 0) { donXuatInput.MaKhachHang = maKH; }
                else { MessageBox.Show("Vui lòng tìm và chọn khách hàng hợp lệ.", "Thiếu Khách Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning); SearchKh.Focus(); return; }

                if (SearchHoaDon.Tag is int maHD && maHD > 0) { donXuatInput.MaHoaDon = maHD; }
                else { donXuatInput.MaHoaDon = null; }
                donXuatInput.GhiChu = Tbnote.Text.Trim();
            }

            donXuatInput.NgayXuat = dtpNgayXuat.Value.Date;
            donXuatInput.MaTK = CurrentUser.MaTK;
            if (donXuatInput.MaTK <= 0) { MessageBox.Show("Không xác định được người lập đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            donXuatInput.ChiTiet = new List<DTO_ChiTietXuatInput>();
            if (!dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow)) { MessageBox.Show("Vui lòng thêm chi tiết đơn xuất.", "Thiếu Chi Tiết", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (this.busVatLieu == null) { MessageBox.Show("Lỗi: Chưa khởi tạo BUS Vật Liệu.", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;
                int maVl;
                int sl;
                decimal gb = 0m;

                bool maVlValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out maVl) && maVl > 0;
                if (!maVlValid)
                {
                    MessageBox.Show($"Mã vật liệu không hợp lệ tại dòng {row.Index + 1}.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvChiTiet.CurrentCell = row.Cells["MaVatLieu"];
                    return;
                }
                bool slValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out sl) && sl > 0;
                if (!slValid)
                {
                    MessageBox.Show($"Số lượng không hợp lệ tại dòng {row.Index + 1} (phải là số > 0).", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvChiTiet.CurrentCell = row.Cells["SoLuong"];
                    return;
                }
                bool gbValid = decimal.TryParse(row.Cells["GiaBan"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out gb) && gb >= 0;
                if (!gbValid)
                {
                    MessageBox.Show($"Giá bán không hợp lệ tại dòng {row.Index + 1} (phải là số >= 0).", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvChiTiet.CurrentCell = row.Cells["GiaBan"];
                    return;
                }

                int? maKhoCuaVatLieu = row.Tag as int?;
                if (!maKhoCuaVatLieu.HasValue || maKhoCuaVatLieu.Value <= 0) { return; }

                if (TgTrangthai.Checked && maKhoCuaVatLieu.Value != selectedMaKhoNguonFinal.Value)
                {
                    MessageBox.Show($"Vật liệu '{row.Cells["TenVatLieu"].Value}' tại dòng {row.Index + 1} không thuộc Kho nguồn '{CbKhoNguon.Text}' đã chọn.", "Sai Kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuongTon = this.busVatLieu.GetSoLuongTonKho(maVl, maKhoCuaVatLieu.Value);
                if (sl > soLuongTon)
                {
                    MessageBox.Show($"SL xuất ({sl}) VL '{row.Cells["TenVatLieu"].Value}' vượt tồn ({soLuongTon}) tại kho '{row.Cells["TenKho"].Value}'.", "Không Đủ Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvChiTiet.CurrentCell = row.Cells["SoLuong"];
                    return;
                }

                donXuatInput.ChiTiet.Add(new DTO_ChiTietXuatInput { MaVatLieu = maVl, SoLuong = sl, DonGia = gb });

                btnXuatHang.Enabled = false;
                try
                {
                    int newMaDonXuat = await Task.Run(() => this.bus.ThemDonXuat(donXuatInput, selectedMaKhoNguonFinal, CurrentUser.MaTK));
                    if (newMaDonXuat > 0)
                    {
                        MessageBox.Show($"Tạo đơn xuất {newMaDonXuat} thành công!", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                    else { MessageBox.Show("Tạo đơn xuất thất bại.", "Thất Bại", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception ex) { MessageBox.Show($"Lỗi khi tạo đơn xuất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                finally { this.Cursor = Cursors.Default; btnXuatHang.Enabled = true; EnableTaoPhieuButton(); }
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
                            if (!_isImportMode)
                            {
                                AddVatLieuToDgvChiTiet(selectedItem);
                                ketqua.Visible = false;
                                txtSearch.Text = string.Empty;
                                txtSearch.Focus();
                            }
                            else
                            {
                                txtSearch.Text = selectedItem.Ten;
                                ketqua.Visible = false;
                            }
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

        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            int selectedMaKhoNguon = -1;
            if (CbKhoNguon.SelectedValue is int mk) { selectedMaKhoNguon = mk; }

            if (TgTrangthai.Checked)
            {
                if (selectedMaKhoNguon <= 0)
                {
                    MessageBox.Show("Vui lòng chọn Kho nguồn cụ thể khi xuất chuyển kho.", "Thiếu Kho Nguồn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CbKhoNguon.Focus();
                    return;
                }
                if (selectedItem.MaKho != selectedMaKhoNguon)
                {
                    MessageBox.Show($"Vật liệu '{selectedItem.Ten}' không thuộc kho nguồn '{CbKhoNguon.Text}' đã chọn cho việc chuyển kho.", "Sai Kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (selectedItem.DonGiaBan == null || selectedItem.DonGiaBan < 0)
            {
                MessageBox.Show($"Vật liệu '{selectedItem.Ten}' chưa có giá bán hợp lệ.", "Thiếu Giá Bán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal giaXuat = TgTrangthai.Checked ? 0m : (selectedItem.DonGiaBan);

            if (dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow && Convert.ToInt32(r.Cells["MaVatLieu"].Value) == selectedItem.MaVatLieu))
            {
                MessageBox.Show($"Vật liệu '{selectedItem.Ten}' đã có trong danh sách.", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rowIndex = dgvChiTiet.Rows.Add(
               selectedItem.MaVatLieu,
               selectedItem.Ten,
               selectedItem.TenKho,
               selectedItem.DonViTinh,
               1,
               giaXuat,
               giaXuat * 1,
               "+", "-", "X"
            );

            if (rowIndex >= 0 && rowIndex < dgvChiTiet.Rows.Count)
            {
                if (selectedItem.MaKho > 0)
                {
                    dgvChiTiet.Rows[rowIndex].Tag = selectedItem.MaKho;
                    Console.WriteLine($"Assigned MaKho {selectedItem.MaKho} to Tag for row {rowIndex}");
                }
                else
                {
                    Console.WriteLine($"Warning: MaKho is invalid ({selectedItem.MaKho}) for item '{selectedItem.Ten}'. Tag not set.");
                }
                dgvChiTiet.ClearSelection();
                dgvChiTiet.Rows[rowIndex].Selected = true;
                if (dgvChiTiet.Columns.Contains("SoLuong")) { dgvChiTiet.CurrentCell = dgvChiTiet.Rows[rowIndex].Cells["SoLuong"]; }
                TinhTongSoLuongVaTienBan();
                UpdateControlsState();
            }
            else
            {
                Console.WriteLine($"Error: Invalid rowIndex {rowIndex} after adding row.");
            }
        }

        private void Frm_Resize(object sender, EventArgs e)
        {
            ResizeColumns();
            CenterPanelInDataGridView();
        }

        private void ResizeColumns()
        {
            if (dgvChiTiet.Columns.Count == 0 || dgvChiTiet.ClientSize.Width <= 0) return;
            try
            {
                int availableWidth = dgvChiTiet.ClientSize.Width;
                if (dgvChiTiet.Controls.OfType<VScrollBar>().Any(v => v.Visible))
                {
                    availableWidth -= SystemInformation.VerticalScrollBarWidth;
                }
                int fixedWidthTotal = 0;
                int fillColumnsCount = 0;
                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
                {
                    { "MaVatLieu", 80 },
                    { "DonViTinh", 80 },
                    { "SoLuong", 90 },
                    { "GiaBan", 100 },
                    { "Tang", 60 },
                    { "Giam", 60 },
                    { "Xoa", 60 }
                };
                foreach (DataGridViewColumn col in dgvChiTiet.Columns)
                {
                    if (!col.Visible) continue;
                    if (fixedWidths.ContainsKey(col.Name))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        col.Width = Math.Max(20, Math.Min(fixedWidths[col.Name], availableWidth - fixedWidthTotal - 50));
                        fixedWidthTotal += col.Width;
                    }
                    else if (col.Name == "TenVatLieu")
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        fillColumnsCount++;
                    }
                    else
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    }
                }
                if (dgvChiTiet.Columns.Contains("TenVatLieu"))
                {
                    dgvChiTiet.Columns["TenVatLieu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvChiTiet.Columns["TenVatLieu"].MinimumWidth = 150;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during column resize: {ex.Message}");
            }
        }

        private void CenterPanelInDataGridView()
        {
            if (panelImportExcel != null && dgvChiTiet != null && panelImportExcel.Parent == dgvChiTiet.Parent)
            {
                int dgvCenterX = dgvChiTiet.Left + dgvChiTiet.Width / 2;
                int dgvCenterY = dgvChiTiet.Top + dgvChiTiet.Height / 2;
                int panelX = dgvCenterX - panelImportExcel.Width / 2;
                int panelY = dgvCenterY - panelImportExcel.Height / 2;
                panelX = Math.Max(dgvChiTiet.Left, panelX);
                panelY = Math.Max(dgvChiTiet.Top, panelY);
                if (panelX + panelImportExcel.Width > dgvChiTiet.Right)
                    panelX = dgvChiTiet.Right - panelImportExcel.Width;
                if (panelY + panelImportExcel.Height > dgvChiTiet.Bottom)
                    panelY = dgvChiTiet.Bottom - panelImportExcel.Height;
                panelImportExcel.Left = panelX;
                panelImportExcel.Top = panelY;
                panelImportExcel.Visible = !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
                if (panelImportExcel.Visible)
                {
                    panelImportExcel.BringToFront();
                }
            }
            else if (panelImportExcel != null)
            {
                panelImportExcel.Visible = !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            }
        }

        private void ClearForm()
        {
            dtpNgayXuat.MaxDate = DateTime.Now.Date;
            Tbnote.Text = string.Empty;
            txtSearch.Text = string.Empty;
            ketqua.Visible = false;
            dgvChiTiet.Rows.Clear();
            TinhTongSoLuongVaTienBan();
            CenterPanelInDataGridView();
        }

        private async Task LoadExcelToGridAndFilterAsync(string filePath)
        {
            _loadedExcelData = new List<VatLieuExcelDayDu>();
            var errorMessages = new List<string>();
            bool hasValidData = false;
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        MessageBox.Show("File Excel không hợp lệ hoặc không có sheet nào.", "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int colCount = worksheet.Dimension?.Columns ?? 0;
                    if (rowCount <= 1)
                    {
                        MessageBox.Show("File Excel không có dữ liệu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    for (int row = 2; row <= rowCount; row++)
                    {
                        bool rowHasData = false;
                        for (int col = 1; col <= 6; col++)
                        {
                            if (worksheet.Cells[row, col]?.Value != null &&
                                !string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value.ToString()))
                            {
                                rowHasData = true;
                                break;
                            }
                        }
                        if (!rowHasData) continue;
                        var dataRow = new VatLieuExcelDayDu { RowNum = row };
                        string rowError = null;
                        try
                        {
                            dataRow.MaVatLieuStr = worksheet.Cells[row, 1]?.Value?.ToString().Trim() ?? "";
                            dataRow.TenVatLieu = worksheet.Cells[row, 2]?.Value?.ToString().Trim() ?? "";
                            dataRow.DonViTinh = worksheet.Cells[row, 3]?.Value?.ToString().Trim() ?? "";
                            dataRow.SoLuongStr = worksheet.Cells[row, 4]?.Value?.ToString().Trim() ?? "";
                            dataRow.GiaNhapStr = worksheet.Cells[row, 5]?.Value?.ToString().Trim() ?? "";
                            dataRow.TenNCC = worksheet.Cells[row, 6]?.Value?.ToString().Trim();
                            if (colCount >= 7) dataRow.SdtNcc = worksheet.Cells[row, 7]?.Value?.ToString().Trim();
                            if (colCount >= 8) dataRow.DiaChiNcc = worksheet.Cells[row, 8]?.Value?.ToString().Trim();
                            if (colCount >= 9) dataRow.EmailNcc = worksheet.Cells[row, 9]?.Value?.ToString().Trim();
                            if (string.IsNullOrEmpty(dataRow.TenNCC))
                            {
                                rowError = "Thiếu tên NCC";
                            }
                            else if ((string.IsNullOrEmpty(dataRow.MaVatLieuStr) && string.IsNullOrEmpty(dataRow.TenVatLieu)))
                            {
                                rowError = "Thiếu Mã/Tên VL";
                            }
                            else if (string.IsNullOrEmpty(dataRow.SoLuongStr) || !int.TryParse(dataRow.SoLuongStr, out int sl) || sl <= 0)
                            {
                                rowError = $"Số lượng '{dataRow.SoLuongStr}' không hợp lệ";
                            }
                            else if (string.IsNullOrEmpty(dataRow.GiaNhapStr) || !decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gn) || gn < 0)
                            {
                                rowError = $"Giá nhập '{dataRow.GiaNhapStr}' không hợp lệ";
                            }
                            else
                            {
                                hasValidData = true;
                            }
                        }
                        catch (Exception exReadCell)
                        {
                            rowError = $"Lỗi đọc ô: {exReadCell.Message}";
                        }
                        dataRow.RowError = rowError;
                        _loadedExcelData.Add(dataRow);
                    }
                    if (!hasValidData && _loadedExcelData.Any())
                    {
                        MessageBox.Show("Không tìm thấy dòng dữ liệu vật liệu hợp lệ nào trong file Excel.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _loadedExcelData = null;
                        return;
                    }
                    if (!_loadedExcelData.Any())
                    {
                        MessageBox.Show("Không đọc được dữ liệu nào từ file Excel.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    var nccFilterList = new List<NccFilterItem>();
                    nccFilterList.Add(new NccFilterItem { TenNCC = "-- Tất cả Nhà Cung Cấp --" });
                    var uniqueNccNames = _loadedExcelData
                                        .Where(d => !string.IsNullOrEmpty(d.TenNCC) && d.RowError == null)
                                        .Select(d => d.TenNCC)
                                        .Distinct()
                                        .OrderBy(name => name);
                    foreach (var name in uniqueNccNames)
                    {
                        nccFilterList.Add(new NccFilterItem { TenNCC = name });
                    }
                    FilterAndDisplayDgvChiTiet();
                }
            }
            catch (IOException ioEx) { errorMessages.Add($"Lỗi IO: {ioEx.Message}"); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) when (invOpEx.Message.Contains("license")) { errorMessages.Add($"Lỗi License: {invOpEx.Message}"); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) { errorMessages.Add($"Lỗi định dạng file: {invOpEx.Message}"); _loadedExcelData = null; }
            catch (Exception ex) { errorMessages.Add($"Lỗi không xác định: {ex.Message}"); _loadedExcelData = null; }
            if (errorMessages.Any())
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc file Excel:\n- " + string.Join("\n- ", errorMessages), "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterAndDisplayDgvChiTiet()
        {
            _isFilteringDgv = true;
            try
            {
                dgvChiTiet.SuspendLayout();
                dgvChiTiet.Rows.Clear();
                string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
                IEnumerable<VatLieuExcelDayDu> dataToDisplay = _loadedExcelData;
                var filteredList = dataToDisplay.ToList();
                foreach (var dataRow in filteredList)
                {
                    int.TryParse(dataRow.SoLuongStr, out int soLuong);
                    decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal giaNhap);
                    int.TryParse(dataRow.MaVatLieuStr, out int maVatLieu);
                    dgvChiTiet.Rows.Add(
                        dataRow.TenNCC,
                        maVatLieu > 0 ? (object)maVatLieu : DBNull.Value,
                        dataRow.TenVatLieu,
                        dataRow.DonViTinh,
                        soLuong > 0 ? (object)soLuong : DBNull.Value,
                        giaNhap >= 0 ? (object)giaNhap : DBNull.Value,
                        "+", "-", "X"
                    );
                    if (!string.IsNullOrEmpty(dataRow.RowError))
                    {
                        DataGridViewRow gridRow = dgvChiTiet.Rows[dgvChiTiet.RowCount - 1];
                        gridRow.DefaultCellStyle.BackColor = Color.LightPink;
                        gridRow.Cells[0].ToolTipText = dataRow.RowError;
                    }
                }
            }
            finally
            {
                if (dgvChiTiet.IsHandleCreated)
                {
                    dgvChiTiet.ResumeLayout();
                }
                _isFilteringDgv = false;
            }
            TinhTongSoLuongVaTienBan();
            UpdatePanelVisibility();
        }

        private void dgvChiTiet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_isFilteringDgv)
            {
                return;
            }
            TinhTongSoLuongVaTienBan();
            if (!_isImportMode)
            {
                UpdateNhapHangButtonState();
                UpdatePanelVisibility();
            }
            else
            {
                bool hasAnyRowLeft = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
                if (!hasAnyRowLeft)
                {
                    Console.WriteLine("!!! No rows left in Import mode (user delete), preparing to reset !!!");
                    this.BeginInvoke(new Action(() =>
                    {
                        ResetToManualMode();
                    }));
                }
                else
                {
                    UpdatePanelVisibility();
                }
            }
        }

        private void UpdateUIForMode()
        {
            bool isManualMode = !_isImportMode;

            CbKhoDich.Enabled = isManualMode;
            txtSearch.Enabled = true;
            TgTrangthai.Enabled = isManualMode;
            SearchKh.Enabled = isManualMode && !TgTrangthai.Checked;
            dtpNgayXuat.Enabled = true;
            Tbnote.Enabled = true;
            dgvChiTiet.ReadOnly = false;
            dgvChiTiet.AllowUserToAddRows = false;

            btnImportExcel.Enabled = isManualMode;
            btnCancelImport.Visible = _isImportMode;
            panelImportExcel.Visible = isManualMode && !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);

            label1.Visible = true;
            label2.Visible = isManualMode && !TgTrangthai.Checked;
            label3.Visible = isManualMode && !TgTrangthai.Checked;

            if (_isImportMode)
            {
                btnXuatHang.Text = "Tạo Phiếu Từ Excel";
                btnXuatHang.Enabled = _loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null);
            }
            else
            {
                btnXuatHang.Text = "Tạo Phiếu Xuất";
                if (dgvChiTiet.DataSource != null || dgvChiTiet.Rows.Count > 1)
                {
                }
                EnableTaoPhieuButton();
            }
            CenterPanelInDataGridView();
        }

        private void EnableTaoPhieuButton()
        {
            if (_isImportMode) { btnXuatHang.Enabled = false; return; }

            bool hasGridRows = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            bool headerValid = false;

            if (TgTrangthai.Checked)
            {
                headerValid = (CbKhoNguon.SelectedValue is int mkN && mkN > 0) &&
                              (CbKhoDich.SelectedValue is int mkD && mkD > 0) &&
                              (mkN != mkD);
            }
            else
            {
                headerValid = (SearchKh.Tag is int maKH && maKH > 0);
            }

            btnXuatHang.Enabled = hasGridRows && headerValid;
        }

        private void UpdateNhapHangButtonState()
        {
            if (_isImportMode)
            {
                return;
            }
            bool hasGridRows = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
        }

        private void ResetToManualMode()
        {
            _isImportMode = false;
            _loadedExcelData = null;
            UpdateUIForMode();
        }

        private void dgvChiTiet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!_isImportMode)
            {
                UpdateNhapHangButtonState();
            }
            UpdatePanelVisibility();
        }

        private async void hopeRoundButton2_Click(object sender, EventArgs e)
        {
            if (_isImportMode)
            {
                var confirm = MessageBox.Show("Bạn đang ở chế độ xem dữ liệu Excel. Load file mới sẽ xóa dữ liệu hiện tại. Tiếp tục?",
                                              "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Chọn file Excel để load dữ liệu nhập hàng";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    txtSearch.Text = string.Empty;
                    this.Cursor = Cursors.WaitCursor;
                    btnImportExcel.Enabled = false;
                    btnXuatHang.Enabled = false;
                    try
                    {
                        await LoadExcelToGridAndFilterAsync(filePath);
                        if (_loadedExcelData != null && _loadedExcelData.Any())
                        {
                            _isImportMode = true;
                            UpdateUIForMode();
                        }
                        else
                        {
                            ResetToManualMode();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi không mong muốn khi xử lý file Excel: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetToManualMode();
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        btnImportExcel.Enabled = true;
                    }
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
            {
                var confirmResult = MessageBox.Show("Bạn có dữ liệu chưa lưu. Thoát mà không lưu?",
                                                 "Xác nhận Thoát",
                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    return;
                }
            }
            Control parentControl = this.Parent;
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this);
                this.Dispose();
                FrmXuat order = new FrmXuat()
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                    FormBorderStyle = FormBorderStyle.None
                };
                parentControl.Controls.Add(order);
                order.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"CellContentClick: Row {e.RowIndex}, Col {e.ColumnIndex}");
        }

        private void btnCancelImport_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc muốn hủy bỏ dữ liệu đã tải từ Excel và quay lại chế độ nhập thủ công?",
                                                 "Xác nhận Hủy Import",
                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Console.WriteLine("Resetting to Manual Mode...");
                ResetToManualMode();
            }
        }

        private void TgTrangthai_CheckedChanged(object sender, EventArgs e)
        {
            bool isChuyenKho = TgTrangthai.Checked = false;

            label3.Visible = !isChuyenKho;
            SearchKh.Visible = !isChuyenKho;
            resultKh.Visible = false;

            label2.Visible = !isChuyenKho;
            SearchHoaDon.Visible = !isChuyenKho;
            resultHD.Visible = false;

            label1.Visible = isChuyenKho;
            CbKhoDich.Visible = isChuyenKho;

            label9.Visible = true;
            CbKhoNguon.Visible = true;


            if (isChuyenKho)
            {
                SearchKh.Text = ""; SearchKh.Tag = null;
                SearchHoaDon.Text = ""; SearchHoaDon.Tag = null;
                LoadInitialData();
            }
            else
            {
                CbKhoDich.SelectedIndex = -1;
            }

            EnableTaoPhieuButton();
        }

        private void PerformSearchKh()
        {
            string searchQuery = SearchKh.Text.Trim().ToLowerInvariant();
            try
            {
                if (danhSachKhach == null || !danhSachKhach.Any())
                {
                    MessageBox.Show("Không có dữ liệu khách hàng để tìm kiếm.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (resultKh != null) resultKh.Visible = false;
                    return;
                }

                IEnumerable<DTO_Khach> filteredSource = danhSachKhach;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    filteredSource = filteredSource.Where(kh =>
                        kh != null &&
                        (
                            kh.MaKhachHang.ToString().Contains(searchQuery) ||
                            (kh.Ten != null && kh.Ten.ToLowerInvariant().Contains(searchQuery))
                        )
                    );
                }

                var finalResults = filteredSource.Take(10).ToList();

                Func<DTO_Khach, string> displayFunc = item => $"{item.MaKhachHang}-{item.Ten}";
                Action<DTO_Khach> clickAction = selectedItem =>
                {
                    SearchKh.TextChanged -= SearchKh_TextChanged;
                    SearchKh.Text = selectedItem.Ten;
                    SearchKh.Tag = selectedItem.MaKhachHang;
                    SearchKh.TextChanged += SearchKh_TextChanged;
                    resultKh.Visible = false;
                    SearchKh.Focus();
                    EnableTaoPhieuButton();
                };

                SearchHelper.UpdateSearchSuggestions<DTO_Khach>(
                    resultKh,
                    finalResults,
                    SearchKh,
                    35,
                    200,
                    displayFunc,
                    clickAction
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resultKh.Visible = false;
            }
        }

        private void PerformSearchHD()
        {
            string searchQuery = SearchHoaDon.Text.Trim().ToLowerInvariant();
            try
            {
                if (danhSachHd == null || !danhSachHd.Any())
                {
                    return;
                }

                IEnumerable<DTO_HoaDon> filteredSource = danhSachHd;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    filteredSource = filteredSource.Where(kh =>
                        kh != null &&
                        (
                            kh.MaHoaDon.ToString().Contains(searchQuery) ||
                            (kh.TenKhachHang != null && kh.TenKhachHang.ToLowerInvariant().Contains(searchQuery))
                        )
                    );
                }

                var finalResults = filteredSource.Take(10).ToList();

                Func<DTO_HoaDon, string> displayFunc = item => $"{item.MaHoaDon}-{item.TenKhachHang}";
                Action<DTO_HoaDon> clickAction = selectedItem =>
                {
                    SearchHoaDon.TextChanged -= SearchHoaDon_TextChanged;
                    SearchHoaDon.Text = selectedItem.MaHoaDon.ToString();
                    SearchHoaDon.Tag = selectedItem.MaHoaDon;
                    SearchHoaDon.TextChanged += SearchHoaDon_TextChanged;
                    resultHD.Visible = false;
                    SearchHoaDon.Focus();
                    EnableTaoPhieuButton();
                };

                SearchHelper.UpdateSearchSuggestions<DTO_HoaDon>(
                    resultHD,
                    finalResults,
                    SearchHoaDon,
                    35,
                    200,
                    displayFunc,
                    clickAction
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm : {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resultHD.Visible = false;
            }
        }

        private void SearchKh_TextChanged(object sender, EventArgs e)
        {
            if (SearchKh.ContainsFocus)
            {
                SearchKh.Tag = null;
                EnableTaoPhieuButton();
            }
            _debounceKhachTimer.Stop();
            _debounceKhachTimer.Start();
        }

        private void SearchHoaDon_TextChanged(object sender, EventArgs e)
        {
            if (SearchHoaDon.ContainsFocus)
            {
                SearchHoaDon.Tag = null;
                EnableTaoPhieuButton();
            }
            _debounceHoaDonTimer.Stop();
            _debounceHoaDonTimer.Start();
        }

        private void DebounceKhachTimer_Tick(object sender, EventArgs e)
        {
            _debounceKhachTimer.Stop();
            PerformSearchKh();
        }

        private void DebounceHoaDonTimer_Tick(object sender, EventArgs e)
        {
            _debounceHoaDonTimer.Stop();
            PerformSearchHD();
            ValidateHoaDonInput();
        }

        private void ValidateHoaDonInput()
        {
            string maHDText = SearchHoaDon.Text.Trim();
            if (!string.IsNullOrEmpty(maHDText))
            {
                if (int.TryParse(maHDText, out int maHD) && maHD > 0)
                {
                    SearchHoaDon.Tag = maHD;
                }
                else
                {
                    SearchHoaDon.Tag = null;
                }
            }
            else
            {
                SearchHoaDon.Tag = null;
            }
            EnableTaoPhieuButton();
        }

        private void CbKhoNguon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllKhos == null || _dtAllKhos.Rows.Count == 0)
            {
                MessageBox.Show("LỖI: _dtAllKhos rỗng hoặc null sau khi load!");
                return;
            }
            else
            {
                string columnNames = string.Join(", ", _dtAllKhos.Columns.Cast<DataColumn>().Select(c => $"'{c.ColumnName}'"));
            }
            int? selectedMaKhoNguon = null;
            if (CbKhoNguon.SelectedValue != null && CbKhoNguon.SelectedValue != DBNull.Value)
            {
                if (int.TryParse(CbKhoNguon.SelectedValue.ToString(), out int maKhoNguon) && maKhoNguon > 0)
                {
                    selectedMaKhoNguon = maKhoNguon;
                }
            }

            int? currentMaKhoDich = null;
            if (CbKhoDich.SelectedValue != null && CbKhoDich.SelectedValue != DBNull.Value)
            {
                if (int.TryParse(CbKhoDich.SelectedValue.ToString(), out int maKhoDich) && maKhoDich > 0)
                {
                    currentMaKhoDich = maKhoDich;
                }
            }

            DataView dvKhoDich = new DataView(_dtAllKhos);

            string valueMemberName = "MaKho";
            if (selectedMaKhoNguon.HasValue)
            {
                dvKhoDich.RowFilter = $"{valueMemberName} <> {selectedMaKhoNguon.Value}";
            }
            else
            {
                dvKhoDich.RowFilter = string.Empty;
            }

            CbKhoDich.DataSource = null;
            CbKhoDich.DataSource = dvKhoDich;
            CbKhoDich.DisplayMember = "TenKho";
            CbKhoDich.ValueMember = valueMemberName;

            bool restored = false;
            if (currentMaKhoDich.HasValue)
            {
                dvKhoDich.Sort = valueMemberName;
                int rowIndex = dvKhoDich.Find(currentMaKhoDich.Value);
                if (rowIndex != -1)
                {
                    CbKhoDich.SelectedValue = currentMaKhoDich.Value;
                    restored = true;
                }
            }

            if (!restored)
            {
                CbKhoDich.SelectedIndex = -1;
            }

            TinhTongSoLuongVaTienBan();
            EnableTaoPhieuButton();
        }

        private void CbKhoDich_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void TgTrangthai_CheckedChanged(object sender)
        {
            bool isChuyenKho = TgTrangthai.Checked;

            label3.Visible = !isChuyenKho;
            SearchKh.Visible = !isChuyenKho;
            resultKh.Visible = false;

            label2.Visible = !isChuyenKho;
            SearchHoaDon.Visible = !isChuyenKho;
            resultHD.Visible = false;

            label1.Visible = isChuyenKho;
            CbKhoDich.Visible = isChuyenKho;

            label9.Visible = true;
            CbKhoNguon.Visible = true;


            if (isChuyenKho)
            {
                SearchKh.Text = ""; SearchKh.Tag = null;
                SearchHoaDon.Text = ""; SearchHoaDon.Tag = null;
                LoadInitialData();
            }
            else
            {
                CbKhoDich.SelectedIndex = -1;
            }

            EnableTaoPhieuButton();
        }

        private void dtpNgayXuat_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
