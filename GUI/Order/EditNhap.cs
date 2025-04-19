using BUS;
using DAL; // Giả sử có DAL_Order để update
using DTO; // Cần DTO_OrderDetail, DTO_ChiTietDonNhap (với TenKho), DTO_TK, CurrentUser
using GUI.Helpler;
using Microsoft.VisualBasic;
using QLVT.Helper;
using QLVT.HoaDon; // Namespace cho FrmXuat?
using QLVT.Order; // Namespace cho PopupOrder?
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class EditNhap : Form
    {
        private bool _isImportMode = false;
        private int _orderId;
        private PopupOrder _popupOrder;
        private readonly BUS_Order _busOrder = new BUS_Order();
        private readonly BUS_Ncc _busNcc = new BUS_Ncc();
        private readonly BUS_TK _busTk = new BUS_TK();
        private readonly BUS_VatLieu _busKho = new BUS_VatLieu();
        private DTO_ChiTietDonNhap _detail;
        private DTO_OrderDetail _originalOrderData;
        private BindingList<DTO_ChiTietDonNhap> _editedDetailsBindingList;
        private List<VatLieuExcelDayDu> _loadedExcelData = null;
        private System.Windows.Forms.Timer debounceTimer;

        public EditNhap(PopupOrder popupOrder, int orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            _popupOrder = popupOrder;
            LoadNccComboBoxDataSource();
            LoadKhoComboBoxDataSource();
            LoadComboBoxes();
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += DebounceTimer_Tick;
            CbKho.AllowDrop = false;
            if (!LoadOrderDataForEditing())
            {
                this.Shown += (s, ev) => this.Close();
                return;
            }
            SetupEditDataGridView();
            ConfigureDgvChiTietStyles();
            CalculateAndDisplayTotals();
            dgvChiTietEdit.CellContentClick -= DgvChiTietEdit_CellContentClick; 
            dgvChiTietEdit.CellContentClick += DgvChiTietEdit_CellContentClick; 
            dgvChiTietEdit.CellValueChanged -= DgvChiTietEdit_CellValueChanged;
            dgvChiTietEdit.CellValueChanged += DgvChiTietEdit_CellValueChanged;
            dgvChiTietEdit.RowsRemoved -= DgvChiTietEdit_RowsRemoved;
            dgvChiTietEdit.RowsRemoved += DgvChiTietEdit_RowsRemoved;
            dgvChiTietEdit.DataError += DgvChiTietEdit_DataError; 
            TinhTongSoLuongVaGiaNhap();
            btnNhapHang.Text = "Lưu Thay Đổi";
        }
        private async void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string searchQuery = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ketqua.Visible = false;
                if (_isImportMode) { FilterAndDisplayDgvChiTiet(); }
                return;
            }
            if (_isImportMode)
            {
                FilterAndDisplayDgvChiTiet(); // Lọc lưới trước
                await PerformExcelDataSearchAndSuggest(searchQuery);
            }
            else { await PerformDatabaseSearchAndSuggest(searchQuery); }
        }
        private async Task PerformExcelDataSearchAndSuggest(string searchQuery)
        {
            List<DTO_VatLieu> results = new List<DTO_VatLieu>();

            if (_loadedExcelData != null && _loadedExcelData.Any())
            {
                string lowerQuery = searchQuery.ToLowerInvariant();
                results = _loadedExcelData
                   .Where(d => d.RowError == null &&
                               (
                                   (d.MaVatLieuStr?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                   (d.TenVatLieu?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                   (d.DonViTinh?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                   (d.TenKho?.ToLowerInvariant().Contains(lowerQuery) ?? false) ||
                                   (d.TenNCC?.ToLowerInvariant().Contains(lowerQuery) ?? false)
                               ))
                   .Select(d => new DTO_VatLieu
                   {
                       MaVatLieu = int.TryParse(d.MaVatLieuStr, out int ma) ? ma : 0,
                       Ten = d.TenVatLieu,
                       DonViTinh = d.DonViTinh,
                   })
                   .DistinctBy(v => v.MaVatLieu > 0 ? (object)v.MaVatLieu : (object)(v.Ten + v.DonViTinh)) 
                   .OrderBy(v => v.Ten)
                   .ToList();
            }

            Console.WriteLine($"Excel Data Search Query: {searchQuery}, Suggestion Results Found: {results.Count}");

            SearchHelper.UpdateSearchSuggestions<DTO_VatLieu>(
                resultPanel: ketqua,
                suggestionResults: results,
                txtSearch: txtSearch,
                itemHeight: 35,
                maxPanelHeight: 210,
                displayFunc: item =>
                {
                    string maVlText = item.MaVatLieu > 0 ? $" ({item.MaVatLieu})" : "";
                    return $"{item.Ten}{maVlText} - {item.DonViTinh}";
                },
                clickAction: selectedItem =>
                {
                    if (!_isImportMode)
                    {
                        AddVatLieuToDgvChiTiet(selectedItem);
                        txtSearch.Text = string.Empty;
                        PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm");
                        txtSearch.Focus();
                    }
                    else
                    {
                        txtSearch.Text = selectedItem.Ten;
                        FilterAndDisplayDgvChiTiet();
                    }
                }
            );

            await Task.CompletedTask;
        }
        private async Task PerformDatabaseSearchAndSuggest(string searchQuery)
        {
            List<DTO_VatLieu> results = null;
            try
            {
                QLVatLieu qlv = new QLVatLieu();
                results = await Task.Run(() => qlv.SearchProducts(searchQuery));
                Console.WriteLine($"DB Search Query: {searchQuery}, Results Found: {results?.Count ?? 0}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm vật liệu trong CSDL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                results = new List<DTO_VatLieu>();
            }
            finally
            {
                SearchHelper.UpdateSearchSuggestions<DTO_VatLieu>(
                    resultPanel: ketqua,
                    suggestionResults: results,
                    txtSearch: txtSearch,
                    itemHeight: 35,
                    maxPanelHeight: 210,
                    displayFunc: item =>
                    {
                        string maVlText = item.MaVatLieu > 0 ? $" ({item.MaVatLieu})" : "";
                        return $"{item.Ten}{maVlText} - {item.DonViTinh} - {item.TenKho}";
                    },
                    clickAction: selectedItem =>
                    {
                        if (!_isImportMode)
                        {
                            AddVatLieuToDgvChiTiet(selectedItem);
                            txtSearch.Text = string.Empty;
                            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm");
                            txtSearch.Focus();
                        }
                        else
                        {
                            txtSearch.Text = ($"{selectedItem.Ten}-{selectedItem.TenKho}");
                            FilterAndDisplayDgvChiTiet();
                        }
                    }
                );
            }
        }

        private bool _isFilteringDgv = false;

        private void FilterAndDisplayDgvChiTiet()
        {
            if (!_isImportMode || _loadedExcelData == null)
            {
                if (!_isImportMode && dgvChiTietEdit.Rows.Count > 0) dgvChiTietEdit.Rows.Clear();
                return;
            }

            _isFilteringDgv = true;
            dgvChiTietEdit.SuspendLayout();
            dgvChiTietEdit.Rows.Clear();

            try
            {
                string selectedNccName = CbLocNCC?.SelectedValue?.ToString();
                bool showAllNcc = (selectedNccName == "-- Tất cả Nhà Cung Cấp --" || string.IsNullOrEmpty(selectedNccName));
                string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();

                IEnumerable<VatLieuExcelDayDu> dataToDisplay = _loadedExcelData;

                if (!showAllNcc)
                {
                    dataToDisplay = dataToDisplay.Where(d => d.TenNCC == selectedNccName);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    dataToDisplay = dataToDisplay.Where(d =>
                        (d.MaVatLieuStr?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (d.TenVatLieu?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (d.DonViTinh?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (d.TenKho?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (showAllNcc && (d.TenNCC?.ToLowerInvariant().Contains(searchQuery) ?? false))
                    );
                }

                var filteredList = dataToDisplay.ToList();
                foreach (var dataRow in filteredList)
                {
                    object maVatLieuValue = DBNull.Value;
                    if (int.TryParse(dataRow.MaVatLieuStr, out int maVatLieu) && maVatLieu > 0) { maVatLieuValue = maVatLieu; }

                    object soLuongValue = DBNull.Value;
                    if (int.TryParse(dataRow.SoLuongStr, out int soLuong) && soLuong > 0) { soLuongValue = soLuong; }

                    object giaNhapValue = DBNull.Value;
                    if (decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0) { giaNhapValue = giaNhap; }

                    int rowIndex = dgvChiTietEdit.Rows.Add(
                        dataRow.TenNCC,
                        dataRow.TenKho, // Thêm Tên Kho
                        maVatLieuValue,
                        dataRow.TenVatLieu,
                        dataRow.DonViTinh,
                        soLuongValue,
                        giaNhapValue,
                        "+", "-", "X"
                    );

                    if (!string.IsNullOrEmpty(dataRow.RowError))
                    {
                        DataGridViewRow gridRow = dgvChiTietEdit.Rows[rowIndex];
                        gridRow.DefaultCellStyle.BackColor = Color.LightPink;
                        gridRow.DefaultCellStyle.ForeColor = Color.DarkRed;
                        gridRow.Cells[0].ToolTipText = $"Lỗi dòng {dataRow.RowNum} (Excel): {dataRow.RowError}";
                        // gridRow.ReadOnly = true;
                    }
                }

            }
            finally
            {
                if (dgvChiTietEdit.IsHandleCreated)
                {
                    dgvChiTietEdit.ResumeLayout();
                }
                _isFilteringDgv = false;
            }

            TinhTongSoLuongVaGiaNhap();
            UpdateNhapHangButtonState();
        }
        private void UpdateNhapHangButtonState()
        {
            if (_isImportMode)
            {
                bool hasValidDataToImport = dgvChiTietEdit.Rows.Cast<DataGridViewRow>()
                                               .Any(r => !r.IsNewRow && r.DefaultCellStyle.BackColor != Color.LightPink);
                btnNhapHang.Enabled = hasValidDataToImport;
            }
            else
            {
                bool isNccSelected = Cbncc.SelectedIndex >= 0 && Cbncc.SelectedItem is DTO_Ncap;
                bool isKhoSelected = CbKho.SelectedIndex >= 0 && CbKho.SelectedItem is DTO_Kho;
                bool hasGridRows = dgvChiTietEdit.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
                bool isNgNhapSelected = CbNgNhap.SelectedIndex >= 0;

                btnNhapHang.Enabled = isNccSelected && isKhoSelected && isNgNhapSelected && hasGridRows;
            }
        }
        private void TinhTongSoLuongVaGiaNhap()
        {
            int tongSoLuong = 0;
            decimal tongGiaNhap = 0;
            foreach (DataGridViewRow row in dgvChiTietEdit.Rows)
            {
                if (row.IsNewRow) continue;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(),
                                 NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap);
                tongSoLuong += soLuong;
                tongGiaNhap += giaNhap * soLuong;
            }
            lblSl.Text = tongSoLuong.ToString("N0");
            lblTongTien.Text = tongGiaNhap.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            if (_isImportMode) return;

            if (Cbncc.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp trước khi thêm vật liệu.", "Thiếu Nhà Cung Cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Cbncc.Focus(); return;
            }
            if (CbKho.SelectedItem == null || !(CbKho.SelectedItem is DTO_Kho selectedKho) || selectedKho.MaKho <= 0)
            {
                MessageBox.Show("Vui lòng chọn Kho hợp lệ trước khi thêm vật liệu.", "Thiếu Kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbKho.Focus(); return;
            }

            DataGridViewRow existingRow = dgvChiTietEdit.Rows.Cast<DataGridViewRow>()
                                        .FirstOrDefault(row => !row.IsNewRow &&
                                                               row.Cells["MaVatLieu"].Value != null &&
                                                               row.Cells["MaVatLieu"].Value.ToString() == selectedItem.MaVatLieu.ToString() &&
                                                               row.Cells["TenKho"].Value != null && 
                                                               row.Cells["TenKho"].Value.ToString().Equals(selectedKho.TenKho, StringComparison.OrdinalIgnoreCase)); // So sánh cả Tên Kho

            if (existingRow != null)
            {
                try
                {
                    int currentQty = Convert.ToInt32(existingRow.Cells["SoLuong"].Value);
                    existingRow.Cells["SoLuong"].Value = currentQty + 1; // Tăng số lượng lên 1
                    Console.WriteLine($"Incremented quantity for item {selectedItem.Ten} in Kho {selectedKho.TenKho}");
                    TinhTongSoLuongVaGiaNhap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật số lượng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                Console.WriteLine($"Adding new item {selectedItem.Ten} to Kho {selectedKho.TenKho} (Manual Mode)");

                try
                {
                    int rowIndex = dgvChiTietEdit.Rows.Add(
                        DBNull.Value,
                        selectedKho.TenKho,       
                        selectedItem.MaVatLieu,   
                        selectedItem.Ten,        
                        selectedItem.DonViTinh,  
                        1,                      
                        selectedItem.DonGiaNhap,  
                        "+",                      
                        "-"                   
                    );

                    if (rowIndex >= 0) 
                    {
                        dgvChiTietEdit.ClearSelection();
                        dgvChiTietEdit.Rows[rowIndex].Selected = true;
                        if (!dgvChiTietEdit.Rows[rowIndex].Displayed)
                        {
                            dgvChiTietEdit.FirstDisplayedScrollingRowIndex = rowIndex;
                        }
                        if (dgvChiTietEdit.Columns.Contains("SoLuong"))
                        {
                            dgvChiTietEdit.CurrentCell = dgvChiTietEdit.Rows[rowIndex].Cells["SoLuong"];
                        }

                        TinhTongSoLuongVaGiaNhap();
                        UpdateNhapHangButtonState();
                    }
                    else
                    {
                        Console.WriteLine("Lỗi: Rows.Add không trả về index hợp lệ.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm chi tiết mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadComboBoxes()
        {
            try
            {
                if (CurrentUser.ChucVu == "Admin")
                {
                    CbNgNhap.Enabled = true;
                    CbNgNhap.DataSource = _busTk.LayDSNgNhap();
                    CbNgNhap.DisplayMember = "TenDangNhap";
                    CbNgNhap.ValueMember = "MaTK";
                    CbNgNhap.DropDownStyle = ComboBoxStyle.DropDownList;
                    CbNgNhap.SelectedIndex = -1;
                }
                else if (CurrentUser.ChucVu == "Quản lý")
                {
                    CbNgNhap.DataSource = _busTk.LayDSNgNhapNv();
                    CbNgNhap.DisplayMember = "TenDangNhap";
                    CbNgNhap.ValueMember = "MaTK";
                    CbNgNhap.SelectedValue = CurrentUser.MaTK;
                    CbNgNhap.Enabled = true;
                }
                else
                {
                    CbNgNhap.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Cho cột tên chiếm phần còn lại
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKho",
                HeaderText = "Kho",
                DataPropertyName = "TenKho", // Cần có TenKho trong DTO_ChiTietDonNhap
                ReadOnly = true,
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
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
                ValueType = typeof(int), // Kiểu số nguyên
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 90
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhap",
                HeaderText = "Giá Nhập",
                DataPropertyName = "GiaNhap",
                ReadOnly = false,
                ValueType = typeof(decimal), // Kiểu số thập phân
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 110
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành Tiền", // Không cần DataPropertyName vì sẽ tính toán
                ReadOnly = true,
                ValueType = typeof(decimal),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 120
            });

            // Thêm nút Tăng/Giảm số lượng
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Tang",
                HeaderText = "Tăng",
                Text = "+",
                UseColumnTextForButtonValue = true,
                Width = 50,
                FlatStyle = FlatStyle.System // Giảm độ rộng nút
            });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Giam",
                HeaderText = "Giảm",
                Text = "-",
                UseColumnTextForButtonValue = true,
                Width = 50,
                FlatStyle = FlatStyle.System // Giảm độ rộng nút
            });

            // Thêm nút Xóa chi tiết (nếu muốn cho phép xóa)
            /*
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn {
                Name = "XoaChiTiet", HeaderText = "Xóa", Text = "X", UseColumnTextForButtonValue = true,
                Width = 50, FlatStyle = FlatStyle.System
            });
            */

            // Không cần gọi ResizeColumns() ở đây vì nó sẽ được gọi sau khi load dữ liệu
        }

        private void ConfigureDgvChiTietStyles()
        {
            // Giữ nguyên phần style của bạn
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
            dgvChiTietEdit.RowTemplate.Height = 35;
        }

        private void ResizeColumns() // Có thể gọi lại khi form resize nếu cần
        {
            // Giữ nguyên logic ResizeColumns của bạn, đảm bảo nó hoạt động với các cột mới
            // (Đã copy logic cũ của bạn vào đây, có thể cần điều chỉnh lại Width nếu thêm/bớt cột)
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
                // Cập nhật độ rộng cố định nếu cần
                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
               {
                   { "MaVatLieu", 80 },
                   { "TenKho", 120 }, // Thêm Kho
                   { "DonViTinh", 80 },
                   { "SoLuong", 90 },
                   { "GiaNhap", 100 },
                   { "ThanhTien", 120 },
                   { "Tang", 50 }, // Giảm nút
                   { "Giam", 50 },
                   //{ "XoaChiTiet", 50 } // Nếu có nút xóa
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
                    else // Các cột không xác định khác (nếu có)
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        fixedWidthTotal += col.Width; // Cộng độ rộng hiện tại
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during column resize: {ex.Message}");
            }
        }
        private void LoadKhoComboBoxDataSource(int? selectMaKho = null)
        {
            try
            {
                List<DTO_Kho> dataSource = _busKho.LayDanhSachKhokho();
                CbKho.DataSource = null;
                CbKho.DisplayMember = "TenKho";
                CbKho.ValueMember = "MaKho";
                CbKho.DataSource = dataSource;

                bool selectionMade = false;
                if (selectMaKho.HasValue && dataSource != null && dataSource.Any(k => k.MaKho == selectMaKho.Value))
                {
                    CbKho.SelectedValue = selectMaKho.Value;
                    selectionMade = CbKho.SelectedValue != null && CbKho.SelectedValue.Equals(selectMaKho.Value);
                }

                if (!selectionMade)
                {
                    CbKho.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CbKho.DataSource = null;
                CbKho.SelectedIndex = -1;
            }
        }
        private void LoadNccComboBoxDataSource(int? selectMaNCC = null)
        {
            try
            {
                List<DTO_Ncap> dataSource = _busNcc.LayDSNcc();
                Cbncc.DataSource = dataSource;
                Cbncc.DisplayMember = "TenNCC";
                Cbncc.ValueMember = "MaNCC";
                Cbncc.DataSource = dataSource;

                bool selectionMade = false;
                if (selectMaNCC.HasValue && dataSource != null && dataSource.Any(n => n.MaNCC == selectMaNCC.Value))
                {
                    Cbncc.SelectedValue = selectMaNCC.Value;
                    selectionMade = Cbncc.SelectedValue != null && Cbncc.SelectedValue.Equals(selectMaNCC.Value);
                }

                if (!selectionMade)
                {
                    Cbncc.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cbncc.DataSource = null;
                Cbncc.SelectedIndex = -1;
            }
        }
        private bool LoadOrderDataForEditing()
        {
            if (_orderId <= 0)
            {
                MessageBox.Show("Mã đơn hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                _originalOrderData = _busOrder.GetOrderDetailById(_orderId);
                if (_originalOrderData != null && _originalOrderData.ChiTietDonNhap != null)
                {
                    Cbncc.SelectedValue = _originalOrderData.MaNCC;
                    ID.Text = _originalOrderData.MaDonNhap.ToString();
                    dtpNgayNhap.Value = _originalOrderData.NgayNhap;
                    dtpNgayNhap.MaxDate = DateTime.Now;
                    Tbnote.Text = _originalOrderData.GhiChu;
                    lblNgTao.Text = _originalOrderData.NguoiTao ?? "N/A";
                    if (CurrentUser.ChucVu == "Admin")
                    {
                        CbNgNhap.SelectedValue = _originalOrderData.NguoiNhap;
                        if (CbNgNhap.SelectedValue == null || (int)CbNgNhap.SelectedValue != _originalOrderData.NguoiNhap)
                        {
                            var originalNhapUser = _busTk.LayDSNgNhap();
                            MessageBox.Show("Người nhập hàng gốc không còn tồn tại hoặc không hợp lệ. Vui lòng chọn người nhập khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (CurrentUser.ChucVu == "Quản lý")
                    {
                        var originalNhapUser = _busTk.LayDSNgNhapNv();
                    }
                    _editedDetailsBindingList = new BindingList<DTO_ChiTietDonNhap>(_originalOrderData.ChiTietDonNhap);
                    dgvChiTietEdit.DataSource = _editedDetailsBindingList;

                    CalculateThanhTienForAllRows();

                    ResizeColumns();

                    return true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn hàng hoặc chi tiết đơn hàng.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn hàng để sửa: {ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CalculateThanhTienForAllRows()
        {
            if (dgvChiTietEdit.Columns.Contains("ThanhTien"))
            {
                foreach (DataGridViewRow row in dgvChiTietEdit.Rows)
                {
                    if (row.IsNewRow) continue;
                    UpdateThanhTienForRow(row);
                }
            }
        }

        private void UpdateThanhTienForRow(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow) return;

            if (dgvChiTietEdit.Columns.Contains("SoLuong") &&
                dgvChiTietEdit.Columns.Contains("GiaNhap") &&
                dgvChiTietEdit.Columns.Contains("ThanhTien") &&
                row.Cells["SoLuong"].Value != null &&
                row.Cells["GiaNhap"].Value != null)
            {
                int.TryParse(row.Cells["SoLuong"].Value.ToString(), out int sl);
                decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gn);
                row.Cells["ThanhTien"].Value = sl * gn;
            }
        }


        private void CalculateAndDisplayTotals()
        {
            int tongSoLuong = 0;
            decimal tongGiaTri = 0;

            // Tính tổng từ BindingList (nguồn dữ liệu hiện tại)
            if (_editedDetailsBindingList != null)
            {
                foreach (var item in _editedDetailsBindingList)
                {
                    // Kiểm tra null phòng trường hợp dữ liệu không hợp lệ
                    tongSoLuong += item.SoLuong; // Giả sử SoLuong luôn hợp lệ sau khi validate
                    tongGiaTri += item.SoLuong * item.GiaNhap; // Giả sử GiaNhap luôn hợp lệ
                }
            }

            lblSl.Text = tongSoLuong.ToString("N0");
            lblTongTien.Text = tongGiaTri.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        // Xử lý khi giá trị ô Số Lượng hoặc Giá Nhập thay đổi
        private void DgvChiTietEdit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
                if (colName == "SoLuong" || colName == "GiaNhap")
                {
                    DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
                    // Validate dữ liệu nhập vào ô vừa thay đổi
                    ValidateCellInput(row.Cells[e.ColumnIndex]);
                    // Cập nhật lại thành tiền cho dòng đó
                    UpdateThanhTienForRow(row);
                    // Tính lại tổng
                    CalculateAndDisplayTotals();
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

            if (colName == "SoLuong")
            {
                if (cellValue == null || !int.TryParse(cellValue.ToString(), out int sl) || sl <= 0)
                {
                    isValid = false;
                    errorMessage = "Số lượng phải là số nguyên lớn hơn 0.";
                    cell.Value = 1; // Đặt lại giá trị mặc định nếu lỗi
                }
            }
            else if (colName == "GiaNhap")
            {
                if (cellValue == null || !decimal.TryParse(cellValue.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gn) || gn < 0)
                {
                    isValid = false;
                    errorMessage = "Giá nhập phải là số không âm.";
                    cell.Value = 0m; // Đặt lại giá trị mặc định nếu lỗi
                }
            }

            if (!isValid)
            {
                MessageBox.Show(errorMessage, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Có thể set lỗi cho ô: cell.ErrorText = errorMessage;
            }
            else
            {
                // Xóa lỗi nếu trước đó có: cell.ErrorText = string.Empty;
            }
        }


        private void DgvChiTietEdit_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // Tính lại tổng sau khi xóa
            CalculateAndDisplayTotals();
        }

        private void DgvChiTietEdit_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

            string errorText = $"Lỗi dữ liệu tại dòng {e.RowIndex + 1}, cột '{dgvChiTietEdit.Columns[e.ColumnIndex].HeaderText}':\n{e.Exception.Message}";
            MessageBox.Show(errorText, "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
            if (colName == "SoLuong") dgvChiTietEdit.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
            else if (colName == "GiaNhap") dgvChiTietEdit.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0m;


            e.Cancel = false;
        }


        private void DgvChiTietEdit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Bỏ qua header/khu vực không hợp lệ

            string colName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
            if (row.IsNewRow) return; // Bỏ qua dòng mới

            if (colName == "Tang" || colName == "Giam")
            {
                if (row.Cells["SoLuong"].Value != null && int.TryParse(row.Cells["SoLuong"].Value.ToString(), out int currentSoLuong))
                {
                    if (colName == "Tang")
                    {
                        row.Cells["SoLuong"].Value = currentSoLuong + 1;
                    }
                    else // Giảm
                    {
                        if (currentSoLuong > 1) // Chỉ giảm nếu lớn hơn 1
                        {
                            row.Cells["SoLuong"].Value = currentSoLuong - 1;
                        }
                        else
                        {
                            MessageBox.Show("Số lượng không thể nhỏ hơn 1.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    // CellValueChanged sẽ tự động cập nhật Thành tiền và Tổng
                }
            }
            else if (colName == "XoaChiTiet") // Nếu có nút Xóa
            {
                // Xác nhận trước khi xóa
                if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này khỏi đơn hàng?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Kiểm tra xem có phải là dòng cuối cùng không (phải còn ít nhất 1 chi tiết)
                    if (dgvChiTietEdit.Rows.Count <= 1) // Chỉ còn 1 dòng (không tính dòng new row nếu có)
                    {
                        MessageBox.Show("Đơn nhập hàng phải có ít nhất một chi tiết.", "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Xóa dòng khỏi BindingList (nếu đang dùng) hoặc trực tiếp khỏi Grid
                    if (_editedDetailsBindingList != null && row.DataBoundItem is DTO_ChiTietDonNhap itemToRemove)
                    {
                        _editedDetailsBindingList.Remove(itemToRemove);
                        // DataSource của Grid sẽ tự cập nhật nếu là BindingList
                    }
                    else if (!row.IsNewRow) // Nếu không dùng BindingList
                    {
                        dgvChiTietEdit.Rows.Remove(row);
                    }
                    // Sự kiện RowsRemoved sẽ tự động tính lại tổng
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // if (HasChanges()) {}

            Control parentControl = this.Parent;
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this);
                this.Dispose();

                try
                {
                    FrmOrder order = new FrmOrder()
                    {
                        TopLevel = false,
                        Dock = DockStyle.Fill,
                        FormBorderStyle = FormBorderStyle.None
                    };
                    parentControl.Controls.Add(order);
                    order.Show();
                }
                catch (Exception exShow)
                {
                    MessageBox.Show($"Không thể mở lại form trước đó: {exShow.Message}", "Lỗi Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else { this.Close(); }
        }


        private async void btnLuuThayDoi_Click(object sender, EventArgs e) // Đổi tên hàm xử lý sự kiện
        {
            if (!DateTime.TryParse(dtpNgayNhap.Value.ToString(), out DateTime ngayNhapMoi)) // Lấy cả giờ nếu cần, hoặc .Date
            {
                MessageBox.Show("Ngày nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string ghiChuMoi = Tbnote.Text.Trim();
            int maTkNguoiNhapMoi = _originalOrderData.NguoiNhap;
            if (CbNgNhapEdit.Enabled && CbNgNhapEdit.SelectedValue != null) // Nếu ComboBox bật và có giá trị được chọn
            {
                if (int.TryParse(CbNgNhapEdit.SelectedValue.ToString(), out int selectedTk))
                {
                    maTkNguoiNhapMoi = selectedTk;
                }
                else
                {
                    MessageBox.Show("Người nhập hàng được chọn không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            List<DTO_ChiTietDonNhap> chiTietDaSua = new List<DTO_ChiTietDonNhap>();
            List<string> validationErrors = new List<string>();
            bool hasValidDetails = false;
            foreach (DataGridViewRow row in dgvChiTietEdit.Rows)
            {
                if (row.IsNewRow) continue; 

                bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL) && maVL > 0;
                bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong) && soLuong > 0;
                bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0;
                string tenKhoTrongGrid = row.Cells["TenKho"].Value?.ToString(); // Giả sử cột hiển thị tên kho là "TenKho"

                if (!isMaVLValid || !isSoLuongValid || !isGiaNhapValid)
                {
                    validationErrors.Add($"Dòng {row.Index + 1}: Dữ liệu Mã VL/SL/Giá không hợp lệ.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(tenKhoTrongGrid))
                {
                    validationErrors.Add($"Dòng {row.Index + 1} (VL: {maVL}): Thiếu thông tin Kho trên lưới.");
                    continue;
                }
                DTO_Kho khoInfo = _busKho.TimkhotheoTen(tenKhoTrongGrid);
                if (khoInfo == null || khoInfo.MaKho <= 0)
                {
                    validationErrors.Add($"Dòng {row.Index + 1} (VL: {maVL}): Kho '{tenKhoTrongGrid}' không hợp lệ hoặc không tìm thấy.");
                    continue;
                }



                chiTietDaSua.Add(new DTO_ChiTietDonNhap
                {
                    MaDonNhap = _orderId, 
                    MaVatLieu = maVL,
                    SoLuong = soLuong,
                    GiaNhap = giaNhap,
                    MaKho = khoInfo.MaKho 
                });
                hasValidDetails = true;

            }

            if (validationErrors.Any())
            {
                MessageBox.Show("Dữ liệu chi tiết không hợp lệ:\n- " + string.Join("\n- ", validationErrors), "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!chiTietDaSua.Any())
            {
                MessageBox.Show("Đơn nhập hàng phải có ít nhất một chi tiết.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = false;
            try
            {
                DTO_Order headerUpdate = new DTO_Order
                {
                    MaDonNhap = _orderId,
                    NgayNhap = ngayNhapMoi,
                    GhiChu = ghiChuMoi,
                    NguoiNhap = maTkNguoiNhapMoi,
                    MaNCC = _originalOrderData.MaNCC,
                    NguoiTao = _originalOrderData.NguoiTao,
                    NguoiCapNhat = CurrentUser.MaTK
                };

                btnNhapHang.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                success = await _busOrder.UpdateOrderAsync(headerUpdate, chiTietDaSua, CurrentUser.MaTK);

                if (success)
                {
                    MessageBox.Show("Lưu thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;

                    if (_popupOrder != null && !_popupOrder.IsDisposed)
                    {
                        try
                        {
                            _popupOrder.LoadOrderDetails();
                            Console.WriteLine("[EditNhap] Đã gọi RefreshData() trên PopupOrder.");
                        }
                        catch (ObjectDisposedException disposedEx)
                        {
                            Console.WriteLine($"[EditNhap] Lỗi khi gọi RefreshData trên PopupOrder (đã bị disposed): {disposedEx.Message}");
                        }
                        catch (Exception refreshEx)
                        {
                            Console.WriteLine($"[EditNhap] Lỗi khác khi gọi RefreshData trên PopupOrder: {refreshEx.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[EditNhap] PopupOrder is null or disposed, không gọi RefreshData.");
                    }

                    Control parentControl = this.Parent;
                    if (parentControl != null)
                    {
                        parentControl.Controls.Remove(this);
                        this.Dispose();

                        try
                        {
                            FrmOrder order = new FrmOrder()
                            {
                                TopLevel = false,
                                Dock = DockStyle.Fill,
                                FormBorderStyle = FormBorderStyle.None
                            };
                            parentControl.Controls.Add(order);
                            order.Show();
                        }
                        catch (Exception exShow)
                        {
                            MessageBox.Show($"Không thể mở lại form trước đó: {exShow.Message}", "Lỗi Điều Hướng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else { this.Close(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi đơn hàng:\n{ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = false;
            }
            finally
            {
                btnNhapHang.Enabled = true;
                this.Cursor = Cursors.Default;
            }

            if (success)
            {
                MessageBox.Show("Lưu thay đổi đơn nhập hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
    }
} 

