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
using Newtonsoft.Json;
using System.Windows.Forms; 
using System.Drawing;
using System;
using System.Data;
using GUI.Helpler; 

namespace QLVT
{


    public partial class NhapHang : Form
    {
        private readonly BUS_Order busOrder = new BUS_Order();
        private readonly BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private readonly BUS_TK busTk = new BUS_TK();
        private readonly BUS_Ncc busNcc = new BUS_Ncc();
        private readonly DAL_Order dalOrder = new DAL_Order();
        private readonly DAL_NCcap dalNcc = new DAL_NCcap();

        private List<VatLieuExcelDayDu> _loadedExcelData = null;
        private bool _isImportMode = false;
        private Form1 form1;
        private System.Windows.Forms.Timer debounceTimer;
        private readonly Color defaultLabelColor = Color.White;
        private readonly Color hoverLabelColor = Color.FromArgb(230, 240, 255);

        public NhapHang()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadInitialData();
            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm");
            dtpNgayNhap.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
            PlaceholderHelper.SetPlaceholder(Tbnote, "Ghi chú");
            DateTime now = DateTime.Now;
            if (now >= dtpNgayNhap.MinDate && now <= dtpNgayNhap.MaxDate) { dtpNgayNhap.Value = now; }
            else if (now < dtpNgayNhap.MinDate) { dtpNgayNhap.Value = dtpNgayNhap.MinDate; }
            else { dtpNgayNhap.Value = dtpNgayNhap.MaxDate; }
        }

        private void InitializeCustomComponents()
        {
            this.Resize += Frm_Resize;
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += DebounceTimer_Tick;
            ConfigureDgvChiTiet(); 
            ConfigureDgvChiTietStyles();

            TinhTongSoLuongVaGiaNhap();
            CenterPanelInDataGridView();
            panelImportExcel.Visible = (dgvChiTiet.RowCount == 0);
        }

        private void ConfigureDgvChiTiet()
        {
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Clear();
            dgvChiTiet.AllowUserToResizeColumns = false;

            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNCC",
                HeaderText = "Nhà Cung Cấp",
                ReadOnly = true,
                Frozen = true,
                Width = 180,
                Visible = false // Ban đầu ẩn
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKho",
                HeaderText = "Kho",
                ReadOnly = true,
                Width = 120 // Kho sẽ read-only trên lưới
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã", ReadOnly = true, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "Đơn Vị", ReadOnly = true });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", ReadOnly = false, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá Nhập", ReadOnly = false, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System }); // Giảm Width
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System }); // Giảm Width
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xoá", Text = "X", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System }); // Giảm Width

            dgvChiTiet.CellClick += DgvChiTiet_CellClick;
            dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            dgvChiTiet.RowsAdded += dgvChiTiet_RowsAdded;
            dgvChiTiet.RowsRemoved += dgvChiTiet_RowsRemoved;

            ResizeColumns();
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

            if (dgvChiTiet.Columns.Contains("MaVatLieu")) { dgvChiTiet.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }
            if (dgvChiTiet.Columns.Contains("TenKho")) { dgvChiTiet.Columns["TenKho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }
            if (dgvChiTiet.Columns.Contains("DonViTinh")) { dgvChiTiet.Columns["DonViTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; }
            if (dgvChiTiet.Columns.Contains("SoLuong"))
            {
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
            }
            if (dgvChiTiet.Columns.Contains("GiaNhap"))
            {
                dgvChiTiet.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvChiTiet.RowTemplate.Height = 35;
        }

        private void LoadInitialData()
        {
            try
            {
                if (CurrentUser.TenDangNhap != null) { lblNgTao.Text = CurrentUser.TenDangNhap; }
                else
                {
                    lblNgTao.Text = "N/A";
                    MessageBox.Show("Không thể xác định người dùng hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                LoadNccComboBoxDataSource(); // Load NCC
                LoadKhoComboBoxDataSource(); // Load Kho

                if (CurrentUser.ChucVu == "Admin")
                {
                    CbNgNhap.DataSource = busTk.LayDSNgNhap();
                    CbNgNhap.DisplayMember = "TenDangNhap";
                    CbNgNhap.ValueMember = "MaTK";
                }
                else if (CurrentUser.ChucVu == "Quản lý")
                {
                    CbNgNhap.DataSource = busTk.LayDSNgNhapNv();
                    CbNgNhap.DisplayMember = "TenDangNhap";
                    CbNgNhap.ValueMember = "MaTK";
                }
                else
                {
                    CbNgNhap.DataSource = busTk.LayDSNgNhap();
                    CbNgNhap.DisplayMember = "TenDangNhap";
                    CbNgNhap.ValueMember = "MaTK";
                    CbNgNhap.Enabled = false;
                }
                CbNgNhap.SelectedValue = CurrentUser.MaTK; // Mặc định chọn người dùng hiện tại

                UpdateUIForMode(); // Cập nhật UI ban đầu (Manual Mode)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKhoComboBoxDataSource(int? selectMaKho = null)
        {
            try
            {
                List<DTO_Kho> dataSource = busVatLieu.LayDanhSachKhokho();
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
                    CbKho.SelectedIndex = -1; // Không chọn gì cả
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CbKho.DataSource = null;
                CbKho.SelectedIndex = -1;
            }
            UpdateNhapHangButtonState();
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
                   .DistinctBy(v => v.MaVatLieu > 0 ? (object)v.MaVatLieu : (object)(v.Ten + v.DonViTinh)) // Loại trùng
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


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvChiTiet.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
                if (row.IsNewRow) return;
                string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int currentSoLuong);
                switch (columnName)
                {
                    case "Tang": row.Cells["SoLuong"].Value = Math.Max(1, currentSoLuong + 1); break;
                    case "Giam": if (currentSoLuong > 1) { row.Cells["SoLuong"].Value = currentSoLuong - 1; } break;
                    case "Xoa":
                        dgvChiTiet.Rows.RemoveAt(e.RowIndex);
                        break;
                }
                if (columnName == "Tang" || columnName == "Giam")
                {
                    TinhTongSoLuongVaGiaNhap();
                }
            }
        }

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (columnName == "SoLuong")
            {
                string input = cell.Value?.ToString();
                if (int.TryParse(input, out int soLuong) && soLuong > 0) { cell.Value = soLuong; }
                else
                {
                    MessageBox.Show("Số lượng phải là một số nguyên lớn hơn 0.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = 1;
                }
            }
            else if (columnName == "GiaNhap")
            {
                string input = cell.Value?.ToString();
                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0)
                {
                    cell.Value = giaNhap;
                    cell.Style.Format = "N0";
                }
                else
                {
                    MessageBox.Show("Giá nhập phải là một số không âm.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = 0m; 
                    cell.Style.Format = "N0";
                }
            }
            TinhTongSoLuongVaGiaNhap();
        }

        private async void btnNhapHang_Click(object sender, EventArgs e)
        {
            if (_isImportMode) 
            {
                await ProcessExcelImport();
            }
            else 
            {
                await ProcessManualEntry();
            }
        }

        private async Task ProcessExcelImport()
        {
            this.Cursor = Cursors.WaitCursor;
            btnNhapHang.Enabled = false;
            btnCancelImport.Enabled = false;
            hopeRoundButton2.Enabled = false;

            try
            {
                var groupedDataFromGrid = new Dictionary<string, List<(DTO_Order ChiTiet, string TenKho)>>(); // Lưu cả TenKho
                var newNccPotentialInfo = new Dictionary<string, VatLieuExcelDayDu>();
                var rowsWithErrorAfterEdit = new List<string>();
                var warehouseLookupErrors = new Dictionary<string, string>(); 
                var validWarehouses = new Dictionary<string, int>(); 

                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue;

                    string tenNCC = row.Cells["TenNCC"].Value?.ToString();
                    string tenKho = row.Cells["TenKho"].Value?.ToString();
                    if (string.IsNullOrEmpty(tenNCC)) { rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1}: Thiếu tên NCC."); continue; }
                    if (string.IsNullOrEmpty(tenKho)) { rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}): Thiếu tên Kho."); continue; }

                    bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL) && maVL > 0; // Mã VL phải > 0
                    bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong) && soLuong > 0;
                    bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0;

                    if (!isMaVLValid || !isSoLuongValid || !isGiaNhapValid)
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}, Kho: {tenKho}): Dữ liệu Mã VL/SL/Giá không hợp lệ.");
                        continue;
                    }
                    int maKho = 0;
                    if (validWarehouses.ContainsKey(tenKho))
                    {
                        maKho = validWarehouses[tenKho];
                    }
                    else if (!warehouseLookupErrors.ContainsKey(tenKho))
                    {
                        DTO_Kho kho = busVatLieu.TimkhotheoTen(tenKho);
                        if (kho != null)
                        {
                            maKho = kho.MaKho;
                            validWarehouses.Add(tenKho, maKho);
                        }
                        else
                        {
                            warehouseLookupErrors.Add(tenKho, $"Không tìm thấy Kho '{tenKho}' trong hệ thống.");
                            rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}): Kho '{tenKho}' không tồn tại.");
                            continue;
                        }
                    }
                    else
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}): Kho '{tenKho}' không tồn tại (đã báo lỗi trước đó).");
                        continue;
                    }
                    DTO_Order chiTiet = new DTO_Order { MaVatLieu = maVL, SoLuong = soLuong, GiaNhap = giaNhap, MaKho = maKho }; // Gán MaKho
                    if (!groupedDataFromGrid.ContainsKey(tenNCC)) { groupedDataFromGrid[tenNCC] = new List<(DTO_Order, string)>(); }
                    groupedDataFromGrid[tenNCC].Add((chiTiet, tenKho)); 

                    if (!newNccPotentialInfo.ContainsKey(tenNCC))
                    {
                        var originalData = _loadedExcelData?.FirstOrDefault(d => d.TenNCC == tenNCC && d.TenKho == tenKho && d.MaVatLieuStr == maVL.ToString()); // Cố gắng tìm chính xác hơn
                        if (originalData == null) originalData = _loadedExcelData?.FirstOrDefault(d => d.TenNCC == tenNCC); // Nếu không thì lấy dòng đầu tiên của NCC đó
                        if (originalData != null) { newNccPotentialInfo[tenNCC] = originalData; }
                    }
                } 
                if (rowsWithErrorAfterEdit.Any())
                {
                    MessageBox.Show("Có lỗi dữ liệu trên lưới hoặc Kho không hợp lệ. Vui lòng kiểm tra lại:\n- " + string.Join("\n- ", rowsWithErrorAfterEdit), "Lỗi Dữ Liệu Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                if (!groupedDataFromGrid.Any())
                {
                    MessageBox.Show("Không có dữ liệu hợp lệ nào để tạo phiếu nhập hàng.", "Thông Báo Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var ordersForExistingNcc = new Dictionary<int, List<DTO_Order>>(); 
                var ordersForNewNcc = new Dictionary<string, List<DTO_Order>>(); 
                var createdNccMapping = new Dictionary<string, int>(); 
                var nccCreationErrors = new List<string>();


                foreach (var kvp in groupedDataFromGrid)
                {
                    string tenNCC = kvp.Key;
                    List<DTO_Order> details = kvp.Value.Select(item => item.ChiTiet).ToList(); 

                    DTO_Ncap existingNcc = dalNcc.TimNccTheoTen(tenNCC); 

                    if (existingNcc != null)
                    {
                        ordersForExistingNcc[existingNcc.MaNCC] = details;
                    }
                    else 
                    {
                        ordersForNewNcc[tenNCC] = details;
                        if (!createdNccMapping.ContainsKey(tenNCC) && !nccCreationErrors.Contains(tenNCC))
                        {
                            VatLieuExcelDayDu nccDetails = null;
                            newNccPotentialInfo.TryGetValue(tenNCC, out nccDetails); 
                            try
                            {
                                int newId = await dalNcc.ThemNccAsync(new DTO_Ncap
                                {
                                    TenNCC = tenNCC,
                                    SDT = nccDetails?.SdtNcc,
                                    DiaChi = nccDetails?.DiaChiNcc,
                                    Email = nccDetails?.EmailNcc,
                                    NguoiTao = CurrentUser.MaTK,
                                    TrangThai = "Chờ duyệt"
                                });

                                if (newId > 0) { createdNccMapping[tenNCC] = newId; }
                                else { nccCreationErrors.Add(tenNCC); } 
                            }
                            catch (Exception exCreateNcc)
                            {
                                Console.WriteLine($"Lỗi khi tạo NCC '{tenNCC}': {exCreateNcc}");
                                nccCreationErrors.Add(tenNCC); 
                            }
                        }
                    }
                }


                int successCount = 0;
                int pendingCount = 0;
                int failCount = 0;
                var results = new List<string>();

                System.DateOnly ngayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value);
                string ghiChu = Tbnote.Text.Trim();
                int maTK_NguoiTao = CurrentUser.MaTK;
                int nguoiCapNhatId = CurrentUser.MaTK;
                int nguoiNhapId;
                if (!int.TryParse(CbNgNhap.SelectedValue?.ToString(), out nguoiNhapId))
                {
                    MessageBox.Show("Vui lòng chọn Người nhập hàng hợp lệ.", "Lỗi Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CbNgNhap.Focus();
                    return;
                }

                foreach (var kvp in ordersForExistingNcc)
                {
                    int maNCC = kvp.Key;
                    List<DTO_Order> chiTietList = kvp.Value;
                    string tenNCCDisplay = busNcc.LayThongTinNCC(maNCC)?.TenNCC ?? $"NCC ID {maNCC}"; // Lấy tên để hiển thị

                    try
                    {
                        string chiTietJson = JsonConvert.SerializeObject(chiTietList);
                        var result = await dalOrder.NhapHangAsync(ngayNhap, maNCC, maTK_NguoiTao, ghiChu, chiTietJson, nguoiCapNhatId, nguoiNhapId);

                        if (result.Success) { successCount++; results.Add($" - {tenNCCDisplay}: Thành công (Mã ĐN: {result.MaDonNhap})"); }
                        else { failCount++; results.Add($" - {tenNCCDisplay}: Thất bại ({result.Message})"); }
                    }
                    catch (Exception ex) { failCount++; results.Add($" - {tenNCCDisplay}: Lỗi hệ thống ({ex.Message})"); }
                }

                foreach (var kvp in ordersForNewNcc)
                {
                    string tenNCC = kvp.Key;
                    List<DTO_Order> chiTietList = kvp.Value;

                    if (createdNccMapping.TryGetValue(tenNCC, out int newMaNCC))
                    {
                        try
                        {
                            string chiTietJson = JsonConvert.SerializeObject(chiTietList);
                            var result = await dalOrder.NhapHangAsync(ngayNhap, newMaNCC, maTK_NguoiTao, ghiChu, chiTietJson, nguoiCapNhatId, nguoiNhapId);

                            if (result.Success) { pendingCount++; results.Add($" - {tenNCC} (Mới): Thành công - Chờ duyệt NCC (Mã ĐN: {result.MaDonNhap})"); }
                            else { failCount++; results.Add($" - {tenNCC} (Mới): Thất bại tạo đơn ({result.Message})"); }
                        }
                        catch (Exception ex) { failCount++; results.Add($" - {tenNCC} (Mới): Lỗi hệ thống khi tạo đơn ({ex.Message})"); }
                    }
                    else
                    {
                        failCount++; results.Add($" - {tenNCC} (Mới): Thất bại (Không tạo được NCC)");
                    }
                }

                failCount += nccCreationErrors.Count;
                if (nccCreationErrors.Any()) { results.AddRange(nccCreationErrors.Select(name => $" - {name} (Mới): Thất bại tạo NCC")); }

                string finalMessage = "Kết quả Import Phiếu Nhập Hàng:\n";
                if (successCount > 0) finalMessage += $"Thành công: {successCount} đơn (NCC đã có).\n";
                if (pendingCount > 0) finalMessage += $"Chờ duyệt NCC: {pendingCount} đơn (NCC mới).\n";
                if (failCount > 0) finalMessage += $"Thất bại: {failCount} đơn/NCC.\n";
                finalMessage += "\nChi tiết:\n" + string.Join("\n", results);

                MessageBoxIcon icon = (failCount > 0) ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                MessageBox.Show(finalMessage, "Kết Quả Import", MessageBoxButtons.OK, icon);

                ResetToManualMode();

            }
            catch (Exception exGlobal)
            {
                MessageBox.Show($"Lỗi không mong muốn trong quá trình import: {exGlobal.Message}\n{exGlobal.StackTrace}", "Lỗi Hệ Thống Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetToManualMode(); // Reset nếu có lỗi lớn
            }
            finally
            {
                this.Cursor = Cursors.Default;
                UpdateNhapHangButtonState();
                btnCancelImport.Visible = false; // Ẩn nút Hủy Import
                hopeRoundButton2.Enabled = true;
                CbNcc.Enabled = true;
                CbKho.Enabled = true;
                txtSearch.Enabled = true;
                CbNgNhap.Enabled = !(CurrentUser.ChucVu != "Admin" && CurrentUser.ChucVu != "Quản lý"); // Kích hoạt lại nếu là Admin/QL
                dtpNgayNhap.Enabled = true;
            }
        }
        private async Task ProcessManualEntry()
        {
            if (CbNcc.SelectedIndex < 0 || !(CbNcc.SelectedItem is DTO_Ncap selectedNcc))
            { MessageBox.Show("Vui lòng chọn Nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); CbNcc.Focus(); return; }

            if (CbKho.SelectedIndex < 0 || !(CbKho.SelectedItem is DTO_Kho selectedKho))
            { MessageBox.Show("Vui lòng chọn Kho nhập hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); CbKho.Focus(); return; }

            if (!dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
            { MessageBox.Show("Vui lòng thêm ít nhất một vật liệu vào phiếu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtSearch.Focus(); return; }

            int nguoiNhapId;
            if (!int.TryParse(CbNgNhap.SelectedValue?.ToString(), out nguoiNhapId))
            {
                MessageBox.Show("Vui lòng chọn Người nhập hàng hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNgNhap.Focus(); return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnNhapHang.Enabled = false;

            int maNCC = selectedNcc.MaNCC;
            int maKho = selectedKho.MaKho; // Lấy mã kho đã chọn
            System.DateOnly ngayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value);
            string ghiChu = Tbnote.Text.Trim();
            int maTK_NguoiTao = CurrentUser.MaTK;
            int nguoiCapNhatId = CurrentUser.MaTK;
            bool hasValidDetails = false;
            List<DTO_Order> orderDetails = new List<DTO_Order>();
            List<string> validationErrors = new List<string>();

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue; // Bỏ qua dòng mới nếu có

                bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL) && maVL > 0;
                bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong) && soLuong > 0;
                bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0;
                string tenKhoTrongGrid = row.Cells["TenKho"].Value?.ToString(); // Lấy tên kho từ cột "TenKho" của dòng này

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

                DTO_Kho khoInfo = busVatLieu.TimkhotheoTen(tenKhoTrongGrid); 
                if (khoInfo == null || khoInfo.MaKho <= 0)
                {
                    validationErrors.Add($"Dòng {row.Index + 1} (VL: {maVL}): Kho '{tenKhoTrongGrid}' không hợp lệ hoặc không tìm thấy.");
                    continue;
                }

                orderDetails.Add(new DTO_Order
                {
                    MaVatLieu = maVL,
                    SoLuong = soLuong,
                    GiaNhap = giaNhap,
                    MaKho = khoInfo.MaKho 
                });
                hasValidDetails = true;

            } 

            if (validationErrors.Any())
            {
                MessageBox.Show("Có lỗi dữ liệu trong danh sách chi tiết:\n- " + string.Join("\n- ", validationErrors), "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Cursor = Cursors.Default;
                btnNhapHang.Enabled = true; 
                return; 
            }

            if (!hasValidDetails)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một chi tiết vật liệu hợp lệ vào phiếu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Cursor = Cursors.Default;
                btnNhapHang.Enabled = true;
                return;
            }
            try
            {
                string chiTietJson = JsonConvert.SerializeObject(orderDetails);

                var result = await dalOrder.NhapHangAsync(ngayNhap, maNCC, maTK_NguoiTao, ghiChu, chiTietJson, nguoiCapNhatId, nguoiNhapId /*, maKho - Bỏ comment nếu MaKho áp dụng cho toàn đơn */);

                if (result.Success)
                {
                    MessageBox.Show($"Tạo phiếu nhập hàng thành công!\nMã Đơn Nhập: {result.MaDonNhap}", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm(); // Xóa form sau khi thành công
                }
                else
                {
                    MessageBox.Show($"Tạo phiếu nhập hàng thất bại.\nLỗi: {result.Message}", "Thất Bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnNhapHang.Enabled = true; // Kích hoạt lại nút nếu thất bại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống khi tạo phiếu nhập hàng.\nChi tiết: {ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNhapHang.Enabled = true; // Kích hoạt lại nút nếu có exception
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void ClearForm()
        {
            dtpNgayNhap.Value = DateTime.Now; // Reset ngày về hiện tại
            CbNcc.SelectedIndex = -1; 
            CbKho.SelectedIndex = -1;
            Tbnote.Text = string.Empty;
            PlaceholderHelper.SetPlaceholder(Tbnote, "Ghi chú"); // Đặt lại placeholder
            txtSearch.Text = string.Empty;
            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm"); // Đặt lại placeholder
            ketqua.Visible = false;
            dgvChiTiet.Rows.Clear(); 

            if (_isImportMode)
            {
                ResetToManualMode(); // Hàm này sẽ gọi UpdateUIForMode và các thứ khác
            }
            else 
            {
                TinhTongSoLuongVaGiaNhap();
                UpdatePanelVisibility();
                UpdateNhapHangButtonState();
                CbNcc.Focus(); 
            }
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

        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            if (_isImportMode) return;

            if (CbNcc.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp trước khi thêm vật liệu.", "Thiếu Nhà Cung Cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNcc.Focus(); return;
            }
            if (CbKho.SelectedItem == null || !(CbKho.SelectedItem is DTO_Kho selectedKho) || selectedKho.MaKho <= 0)
            {
                MessageBox.Show("Vui lòng chọn Kho hợp lệ trước khi thêm vật liệu.", "Thiếu Kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbKho.Focus(); return;
            }

            DataGridViewRow existingRow = dgvChiTiet.Rows.Cast<DataGridViewRow>()
                                        .FirstOrDefault(row => !row.IsNewRow &&
                                                               row.Cells["MaVatLieu"].Value != null &&
                                                               row.Cells["MaVatLieu"].Value.ToString() == selectedItem.MaVatLieu.ToString() &&
                                                               row.Cells["TenKho"].Value != null && // Giả sử cột Kho tên là "TenKho"
                                                               row.Cells["TenKho"].Value.ToString().Equals(selectedKho.TenKho, StringComparison.OrdinalIgnoreCase)); // So sánh cả Tên Kho

            if (existingRow != null) 
            {
                try
                {
                    int currentQty = Convert.ToInt32(existingRow.Cells["SoLuong"].Value);
                    existingRow.Cells["SoLuong"].Value = currentQty + 1; // Tăng số lượng lên 1
                    Console.WriteLine($"Incremented quantity for item {selectedItem.Ten} in Kho {selectedKho.TenKho}");
                    // Gọi tính lại tổng sau khi tăng số lượng
                    TinhTongSoLuongVaGiaNhap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật số lượng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Nếu không tìm thấy dòng trùng (hoặc trùng Mã VL nhưng khác Kho) -> Thêm dòng mới
            {
                Console.WriteLine($"Adding new item {selectedItem.Ten} to Kho {selectedKho.TenKho} (Manual Mode)");

                try
                {
                    int rowIndex = dgvChiTiet.Rows.Add(
                        DBNull.Value,            
                        selectedKho.TenKho,       // Tên Kho đã chọn
                        selectedItem.MaVatLieu,   // Mã Vật liệu
                        selectedItem.Ten,         // Tên Vật liệu
                        selectedItem.DonViTinh,   // Đơn vị tính
                        1,                        // Số lượng mặc định là 1
                        selectedItem.DonGiaNhap,  // Giá nhập (lấy từ DTO nếu có)
                        "+",                      // Nút tăng
                        "-",                      // Nút giảm
                        "X"                       // Nút xóa
                    );

                    if (rowIndex >= 0) // Đảm bảo rowIndex hợp lệ
                    {
                        dgvChiTiet.ClearSelection();
                        dgvChiTiet.Rows[rowIndex].Selected = true;
                        if (!dgvChiTiet.Rows[rowIndex].Displayed)
                        {
                            dgvChiTiet.FirstDisplayedScrollingRowIndex = rowIndex;
                        }
                        if (dgvChiTiet.Columns.Contains("SoLuong"))
                        {
                            dgvChiTiet.CurrentCell = dgvChiTiet.Rows[rowIndex].Cells["SoLuong"];
                        }

                        TinhTongSoLuongVaGiaNhap();
                        UpdatePanelVisibility();
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



        private void TinhTongSoLuongVaGiaNhap()
        {
            int tongSoLuong = 0;
            decimal tongGiaNhap = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
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

                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
                {
                    // Cột cố định
                    { "MaVatLieu", 80 },
                    { "DonViTinh", 80 },
                    { "SoLuong", 90 },
                    { "GiaNhap", 100 },
                    { "Tang", 50 }, 
                    { "Giam", 50 },
                    { "Xoa", 50 },
                    { "TenKho", 120 },
                    { "TenNCC", 180 }
                };

                int totalFixedWidth = 0;
                int fillColumnsCount = 0;
                DataGridViewColumn tenVatLieuCol = null;

                foreach (DataGridViewColumn col in dgvChiTiet.Columns)
                {
                    if (!col.Visible) continue; 

                    if (fixedWidths.ContainsKey(col.Name))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        col.Width = fixedWidths[col.Name];
                        totalFixedWidth += col.Width;
                    }
                    else if (col.Name == "TenVatLieu") // Cột Tên Vật Liệu sẽ là Fill
                    {
                        tenVatLieuCol = col;
                        fillColumnsCount++;
                    }
                    else 
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        totalFixedWidth += col.Width; // Cộng cả độ rộng hiện tại của cột không xác định
                    }
                }

                if (tenVatLieuCol != null && fillColumnsCount > 0)
                {
                    tenVatLieuCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    tenVatLieuCol.MinimumWidth = 150; // Đảm bảo cột tên không quá nhỏ
                }
                else if (tenVatLieuCol != null) // Nếu TenVatLieu không phải là Fill (ví dụ chỉ có 1 cột)
                {
                    tenVatLieuCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    tenVatLieuCol.Width = Math.Max(150, availableWidth - totalFixedWidth); // Chiếm phần còn lại
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

                // Đảm bảo panel không bị lệch ra ngoài DataGridView
                panelX = Math.Max(dgvChiTiet.Left, panelX);
                panelY = Math.Max(dgvChiTiet.Top, panelY);
                if (panelX + panelImportExcel.Width > dgvChiTiet.Right) { panelX = dgvChiTiet.Right - panelImportExcel.Width; }
                if (panelY + panelImportExcel.Height > dgvChiTiet.Bottom) { panelY = dgvChiTiet.Bottom - panelImportExcel.Height; }

                panelImportExcel.Left = panelX;
                panelImportExcel.Top = panelY;

                panelImportExcel.Visible = !_isImportMode && !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);

                if (panelImportExcel.Visible) { panelImportExcel.BringToFront(); }
            }
            else if (panelImportExcel != null) 
            {
                panelImportExcel.Visible = !_isImportMode && !dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            }
        }

        private async Task LoadExcelToGridAndFilterAsync(string filePath)
        {
            _loadedExcelData = new List<VatLieuExcelDayDu>();
            var errorMessages = new List<string>();
            bool hasValidDataRow = false; 

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        errorMessages.Add("File Excel không hợp lệ hoặc không có sheet nào.");
                        _loadedExcelData = null; // Đặt lại nếu file không hợp lệ
                        return; // Thoát sớm
                    }

                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int colCount = worksheet.Dimension?.Columns ?? 0;

                    if (rowCount <= 1) 
                    {
                        errorMessages.Add("File Excel không có dữ liệu (chỉ có dòng tiêu đề).");
                        _loadedExcelData = null;
                        return;
                    }

                    const int COL_MA_VL = 1;
                    const int COL_TEN_VL = 2;
                    const int COL_DVT = 3;
                    const int COL_SL = 4;
                    const int COL_GIA_NHAP = 5;
                    const int COL_TEN_NCC = 6;
                    const int COL_SDT_NCC = 7;
                    const int COL_DC_NCC = 8;
                    const int COL_EMAIL_NCC = 9;
                    const int COL_TEN_KHO = 10; 

                    for (int row = 2; row <= rowCount; row++)
                    {
                        bool rowHasMeaningfulData = false;
                        for (int col = COL_MA_VL; col <= COL_TEN_KHO && col <= colCount; col++) // Kiểm tra đến cột Kho
                        {
                            if (worksheet.Cells[row, col]?.Value != null && !string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value.ToString()))
                            {
                                rowHasMeaningfulData = true;
                                break;
                            }
                        }
                        if (!rowHasMeaningfulData) continue; // Bỏ qua dòng trống

                        var dataRow = new VatLieuExcelDayDu { RowNum = row };
                        string rowError = null;

                        try
                        {
                            dataRow.MaVatLieuStr = worksheet.Cells[row, COL_MA_VL]?.Value?.ToString().Trim();
                            dataRow.TenVatLieu = worksheet.Cells[row, COL_TEN_VL]?.Value?.ToString().Trim();
                            dataRow.DonViTinh = worksheet.Cells[row, COL_DVT]?.Value?.ToString().Trim();
                            dataRow.SoLuongStr = worksheet.Cells[row, COL_SL]?.Value?.ToString().Trim();
                            dataRow.GiaNhapStr = worksheet.Cells[row, COL_GIA_NHAP]?.Value?.ToString().Trim();
                            dataRow.TenNCC = worksheet.Cells[row, COL_TEN_NCC]?.Value?.ToString().Trim();

                            if (colCount >= COL_SDT_NCC) dataRow.SdtNcc = worksheet.Cells[row, COL_SDT_NCC]?.Value?.ToString().Trim();
                            if (colCount >= COL_DC_NCC) dataRow.DiaChiNcc = worksheet.Cells[row, COL_DC_NCC]?.Value?.ToString().Trim();
                            if (colCount >= COL_EMAIL_NCC) dataRow.EmailNcc = worksheet.Cells[row, COL_EMAIL_NCC]?.Value?.ToString().Trim();

                            if (colCount >= COL_TEN_KHO) dataRow.TenKho = worksheet.Cells[row, COL_TEN_KHO]?.Value?.ToString().Trim();
                            else dataRow.TenKho = null; 

                            if (string.IsNullOrEmpty(dataRow.TenNCC)) { rowError = "Thiếu Tên Nhà Cung Cấp"; }
                            else if (string.IsNullOrEmpty(dataRow.TenKho)) { rowError = "Thiếu Tên Kho"; } 
                            else if (string.IsNullOrEmpty(dataRow.MaVatLieuStr) && string.IsNullOrEmpty(dataRow.TenVatLieu)) { rowError = "Thiếu Mã Vật Liệu hoặc Tên Vật Liệu"; }
                            else if (string.IsNullOrEmpty(dataRow.DonViTinh)) { rowError = "Thiếu Đơn Vị Tính"; } // Thêm kiểm tra ĐVT
                            else if (string.IsNullOrEmpty(dataRow.SoLuongStr) || !int.TryParse(dataRow.SoLuongStr, out int sl) || sl <= 0) { rowError = $"Số lượng không hợp lệ ('{dataRow.SoLuongStr}')"; }
                            else if (string.IsNullOrEmpty(dataRow.GiaNhapStr) || !decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gn) || gn < 0) { rowError = $"Giá nhập không hợp lệ ('{dataRow.GiaNhapStr}')"; }
                            else
                            {
                                if (!string.IsNullOrEmpty(dataRow.MaVatLieuStr) && !int.TryParse(dataRow.MaVatLieuStr, out _))
                                {
                                    rowError = $"Mã vật liệu không phải là số ('{dataRow.MaVatLieuStr}')";
                                }
                                else
                                {
                                    hasValidDataRow = true; // Đánh dấu là có ít nhất 1 dòng hợp lệ
                                }
                            }
                        }
                        catch (Exception exReadCell) { rowError = $"Lỗi đọc dữ liệu dòng {row}: {exReadCell.Message}"; }

                        dataRow.RowError = rowError; // Gán lỗi (nếu có) vào đối tượng
                        _loadedExcelData.Add(dataRow); // Thêm vào danh sách (kể cả dòng lỗi để hiển thị)
                    } // Kết thúc vòng lặp qua các dòng Excel

                    // Kiểm tra sau khi đọc xong
                    if (!_loadedExcelData.Any()) // Không đọc được dòng nào cả
                    {
                        errorMessages.Add("Không đọc được dữ liệu nào từ file Excel.");
                        _loadedExcelData = null;
                        return;
                    }
                    if (!hasValidDataRow) // Đọc được nhưng không có dòng nào hợp lệ
                    {
                        errorMessages.Add("Không tìm thấy dòng dữ liệu vật liệu hợp lệ nào trong file Excel. Vui lòng kiểm tra lại định dạng và dữ liệu.");
                        // Giữ lại _loadedExcelData để hiển thị lỗi trên lưới
                        // _loadedExcelData = null; // Không đặt lại để FilterAndDisplay có thể hiển thị lỗi
                    }

                    // Load ComboBox lọc NCC (chỉ load nếu có dữ liệu hợp lệ)
                    if (hasValidDataRow)
                    {
                        LoadNccFilterComboBox(); // Load filter NCC
                    }
                    else
                    {
                        CbLocNCC.DataSource = null; // Không có dữ liệu hợp lệ thì không cần lọc
                        CbLocNCC.Enabled = false;
                    }

                    // Hiển thị dữ liệu (kể cả lỗi) lên lưới
                    FilterAndDisplayDgvChiTiet();
                } 
            }
            catch (IOException ioEx) { errorMessages.Add($"Lỗi truy cập file: {ioEx.Message}. File có thể đang được mở bởi ứng dụng khác."); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) when (invOpEx.Message.Contains("license")) { errorMessages.Add($"Lỗi bản quyền EPPlus: {invOpEx.Message}. Cần cài đặt LicenseContext."); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) { errorMessages.Add($"Lỗi định dạng file Excel không hợp lệ: {invOpEx.Message}"); _loadedExcelData = null; }
            catch (Exception ex) { errorMessages.Add($"Lỗi không xác định khi đọc file: {ex.Message}"); _loadedExcelData = null; }

            // Hiển thị tất cả lỗi thu thập được
            if (errorMessages.Any())
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc file Excel:\n- " + string.Join("\n- ", errorMessages), "Lỗi Đọc File Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Nếu có lỗi nghiêm trọng (_loadedExcelData bị null), reset về manual
                if (_loadedExcelData == null)
                {
                    ResetToManualMode();
                }
            }
        }


        private bool _isFilteringDgv = false; 

        private void FilterAndDisplayDgvChiTiet()
        {
            if (!_isImportMode || _loadedExcelData == null) // Không cần ktra Any() vì có thể muốn hiển thị lưới rỗng
            {
                if (!_isImportMode && dgvChiTiet.Rows.Count > 0) dgvChiTiet.Rows.Clear();
                // CbLocNCC.Visible = false; // Sẽ được xử lý trong UpdateUIForMode
                return;
            }

            _isFilteringDgv = true; // Bắt đầu lọc
            dgvChiTiet.SuspendLayout(); // Tạm dừng layout để tăng tốc độ
            dgvChiTiet.Rows.Clear(); // Xóa các dòng hiện có

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

                var filteredList = dataToDisplay.ToList(); // Chuyển sang List để duyệt
                foreach (var dataRow in filteredList)
                {
                    // Cố gắng parse dữ liệu, sử dụng DBNull nếu không parse được hoặc không hợp lệ
                    object maVatLieuValue = DBNull.Value;
                    if (int.TryParse(dataRow.MaVatLieuStr, out int maVatLieu) && maVatLieu > 0) { maVatLieuValue = maVatLieu; }

                    object soLuongValue = DBNull.Value;
                    if (int.TryParse(dataRow.SoLuongStr, out int soLuong) && soLuong > 0) { soLuongValue = soLuong; }

                    object giaNhapValue = DBNull.Value;
                    if (decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0) { giaNhapValue = giaNhap; }

                    int rowIndex = dgvChiTiet.Rows.Add(
                        dataRow.TenNCC,
                        dataRow.TenKho, // Thêm Tên Kho
                        maVatLieuValue,
                        dataRow.TenVatLieu,
                        dataRow.DonViTinh,
                        soLuongValue,
                        giaNhapValue,
                        "+", "-", "X"
                    );

                    // Đánh dấu dòng lỗi nếu có lỗi từ Excel
                    if (!string.IsNullOrEmpty(dataRow.RowError))
                    {
                        DataGridViewRow gridRow = dgvChiTiet.Rows[rowIndex]; // Lấy dòng vừa thêm
                        gridRow.DefaultCellStyle.BackColor = Color.LightPink; // Đổi màu nền
                        gridRow.DefaultCellStyle.ForeColor = Color.DarkRed; // Đổi màu chữ
                        // Thêm ToolTip cho ô đầu tiên để hiển thị lỗi chi tiết
                        gridRow.Cells[0].ToolTipText = $"Lỗi dòng {dataRow.RowNum} (Excel): {dataRow.RowError}";
                        // Vô hiệu hóa chỉnh sửa cho dòng lỗi (tùy chọn)
                        // gridRow.ReadOnly = true;
                    }
                } // Kết thúc foreach

            }
            finally
            {
                if (dgvChiTiet.IsHandleCreated) // Đảm bảo handle đã được tạo trước khi ResumeLayout
                {
                    dgvChiTiet.ResumeLayout(); // Tiếp tục layout
                }
                _isFilteringDgv = false; // Kết thúc lọc
            }

            TinhTongSoLuongVaGiaNhap(); // Tính lại tổng sau khi lọc
            UpdatePanelVisibility(); // Cập nhật hiển thị panel import
            UpdateNhapHangButtonState(); // Cập nhật trạng thái nút Nhập hàng (quan trọng cho import mode)
        }


        private void dgvChiTiet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_isFilteringDgv) { return; } // Bỏ qua nếu đang trong quá trình lọc/clear

            TinhTongSoLuongVaGiaNhap();

            if (!_isImportMode) // Chế độ thủ công
            {
                UpdateNhapHangButtonState();
                UpdatePanelVisibility(); // Hiển thị lại panel nếu xóa hết dòng
            }
            else // Chế độ Import
            {
                bool hasAnyValidRowLeft = dgvChiTiet.Rows.Cast<DataGridViewRow>()
                                            .Any(r => !r.IsNewRow && r.DefaultCellStyle.BackColor != Color.LightPink); // Giả sử dòng lỗi có màu LightPink

                bool hasAnyRowLeftAtAll = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);


                if (!hasAnyRowLeftAtAll)
                {
                    Console.WriteLine("All rows removed by user in Import mode, resetting to Manual.");
                    this.BeginInvoke(new Action(() =>
                    {
                        if (_isImportMode) ResetToManualMode(); // Kiểm tra lại _isImportMode phòng trường hợp đã reset
                    }));
                }
                else
                {
                    // Nếu vẫn còn dòng, chỉ cập nhật trạng thái nút và panel
                    UpdateNhapHangButtonState(); // Nút Nhập hàng có thể bị disable nếu chỉ còn dòng lỗi
                    UpdatePanelVisibility(); // Panel import vẫn ẩn vì còn dòng
                }
            }
        }


        private void CbLocNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ lọc lại khi ComboBox này đang được focus (người dùng chủ động chọn)
            // và đang ở chế độ Import
            if (_isImportMode && CbLocNCC.Focused)
            {
                FilterAndDisplayDgvChiTiet();
            }
        }

        private void UpdateUIForMode()
        {
            bool isManualMode = !_isImportMode;

            labelNcc.Visible = isManualMode;
            CbNcc.Visible = isManualMode;
            CbNcc.Enabled = isManualMode;

            labelKho.Visible = isManualMode;
            CbKho.Visible = isManualMode;
            CbKho.Enabled = isManualMode;

            CbLocNCC.Visible = _isImportMode;
            CbLocNCC.Enabled = _isImportMode && _loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null); // Chỉ bật khi có dữ liệu hợp lệ

            txtSearch.Enabled = true;
            CbNgNhap.Enabled = true;
            dtpNgayNhap.Enabled = true;

            ketqua.Visible = false;

            btnCancelImport.Visible = _isImportMode;
            btnCancelImport.Enabled = _isImportMode;

            if (dgvChiTiet.Columns.Contains("TenNCC"))
            {
                dgvChiTiet.Columns["TenNCC"].Visible = _isImportMode;
            }
            if (dgvChiTiet.Columns.Contains("TenKho"))
            {
                dgvChiTiet.Columns["TenKho"].Visible = true;
            }


            if (_isImportMode)
            {
                btnNhapHang.Text = "Tạo Phiếu (Excel)";
                btnNhapHang.Enabled = _loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null);
                if (_loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null)) { LoadNccFilterComboBox(); }
                else { CbLocNCC.DataSource = null; CbLocNCC.Enabled = false; }

                FilterAndDisplayDgvChiTiet();
            }
            else
            {
                CbLocNCC.DataSource = null; // Xóa dữ liệu bộ lọc NCC
                btnNhapHang.Text = "Tạo Phiếu Nhập";

                if (dgvChiTiet.Rows.Count > 0) dgvChiTiet.Rows.Clear();

                UpdateNhapHangButtonState(); // Cập nhật trạng thái nút (sẽ bị disable ban đầu)
                TinhTongSoLuongVaGiaNhap(); // Reset tổng tiền/số lượng
            }

            UpdatePanelVisibility(); // Cập nhật panel import (sẽ hiển thị nếu lưới trống ở Manual)
            ResizeColumns(); // Chỉnh lại kích thước cột cho phù hợp với mode
        }


        private void UpdateNhapHangButtonState()
        {
            if (_isImportMode)
            {
                bool hasValidDataToImport = dgvChiTiet.Rows.Cast<DataGridViewRow>()
                                               .Any(r => !r.IsNewRow && r.DefaultCellStyle.BackColor != Color.LightPink); // Kiểm tra dòng hợp lệ trên lưới
                btnNhapHang.Enabled = hasValidDataToImport;
            }
            else
            {
                bool isNccSelected = CbNcc.SelectedIndex >= 0 && CbNcc.SelectedItem is DTO_Ncap;
                bool isKhoSelected = CbKho.SelectedIndex >= 0 && CbKho.SelectedItem is DTO_Kho;
                bool hasGridRows = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
                bool isNgNhapSelected = CbNgNhap.SelectedIndex >= 0; // Kiểm tra người nhập đã chọn chưa

                btnNhapHang.Enabled = isNccSelected && isKhoSelected && isNgNhapSelected && hasGridRows;
            }
        }

        private void LoadNccFilterComboBox()
        {
            if (!_isImportMode || _loadedExcelData == null)
            {
                CbLocNCC.DataSource = null; CbLocNCC.Enabled = false; return;
            }

            var nccFilterList = new List<NccFilterItem>();
            nccFilterList.Add(new NccFilterItem { TenNCC = "-- Tất cả Nhà Cung Cấp --" }); // Mục chọn tất cả

            var uniqueNccNames = _loadedExcelData
                .Where(d => d.RowError == null && !string.IsNullOrEmpty(d.TenNCC)) // Chỉ lấy từ dòng hợp lệ
                .Select(d => d.TenNCC)
                .Distinct()
                .OrderBy(name => name);

            foreach (var name in uniqueNccNames)
            {
                nccFilterList.Add(new NccFilterItem { TenNCC = name });
            }

            CbLocNCC.DataSource = null; // Xóa nguồn cũ
            CbLocNCC.DisplayMember = "TenNCC";
            CbLocNCC.ValueMember = "TenNCC"; // Value cũng là tên NCC
            CbLocNCC.DataSource = nccFilterList;

            // Chỉ bật nếu có nhiều hơn 1 lựa chọn (Tất cả + ít nhất 1 NCC)
            CbLocNCC.Enabled = nccFilterList.Count > 1;
            CbLocNCC.SelectedIndex = 0; // Mặc định chọn "Tất cả"
        }

        // Class phụ trợ cho ComboBox lọc NCC
        private class NccFilterItem
        {
            public string TenNCC { get; set; }
            // Có thể thêm MaNCC nếu cần, nhưng ở đây chỉ cần Tên
        }


        private void ResetToManualMode()
        {
            _isImportMode = false;
            _loadedExcelData = null; // Xóa dữ liệu Excel đã load

            LoadNccComboBoxDataSource(); // Load lại danh sách NCC cho ComboBox chính
            LoadKhoComboBoxDataSource(); // Load lại danh sách Kho

            txtSearch.Text = string.Empty; // Xóa tìm kiếm
            PlaceholderHelper.SetPlaceholder(txtSearch, "Tìm kiếm vật liệu và thêm");

            UpdateUIForMode(); // Cập nhật giao diện về trạng thái Manual

            CbNcc.Focus(); // Focus vào ComboBox NCC
        }

        private void dgvChiTiet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_isFilteringDgv) return; // Bỏ qua nếu đang lọc

            if (!_isImportMode) // Chỉ cập nhật nút ở chế độ Manual khi thêm dòng
            {
                UpdateNhapHangButtonState();
            }
            UpdatePanelVisibility(); // Ẩn panel import khi có dòng được thêm
        }

        private async void hopeRoundButton2_Click(object sender, EventArgs e) // Nút "Chọn File Excel"
        {
            if (_isImportMode)
            {
                var confirm = MessageBox.Show("Bạn đang ở chế độ xem dữ liệu Excel. Load file mới sẽ xóa dữ liệu hiện tại và quay về chế độ nhập thủ công nếu file mới lỗi. Tiếp tục?",
                                              "Xác nhận Load File Mới", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Chọn file Excel chứa dữ liệu nhập hàng";
                openFileDialog.RestoreDirectory = true; // Nhớ thư mục cuối cùng

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    txtSearch.Text = string.Empty; // Xóa tìm kiếm cũ
                    ketqua.Visible = false; // Ẩn kết quả cũ

                    this.Cursor = Cursors.WaitCursor;
                    hopeRoundButton2.Enabled = false; // Vô hiệu hóa nút chọn file
                    btnNhapHang.Enabled = false; // Vô hiệu hóa nút nhập hàng
                    btnCancelImport.Enabled = false; // Vô hiệu hóa nút hủy

                    try
                    {
                        // Load dữ liệu từ Excel, hàm này sẽ xử lý cả việc lọc và hiển thị lỗi
                        await LoadExcelToGridAndFilterAsync(filePath);

                        // Sau khi load xong, kiểm tra xem có dữ liệu hợp lệ không
                        if (_loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null))
                        {
                            _isImportMode = true; // Chuyển sang chế độ Import
                            UpdateUIForMode(); // Cập nhật UI cho chế độ Import
                            // Trạng thái các nút sẽ được cập nhật trong UpdateUIForMode
                        }
                        else // Nếu không có dữ liệu hợp lệ hoặc có lỗi nghiêm trọng khi đọc file
                        {
                            _isImportMode = false;
                            _loadedExcelData = null; // Xóa data lỗi
                            UpdateUIForMode(); // Cập nhật UI về manual
                                               // MessageBox.Show("Không có dữ liệu hợp lệ trong file Excel hoặc file bị lỗi. Vui lòng kiểm tra lại file.", "Lỗi Dữ Liệu Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex) // Bắt các lỗi không mong muốn khác
                    {
                        MessageBox.Show($"Lỗi không mong muốn khi xử lý file Excel: {ex.Message}\n{ex.StackTrace}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetToManualMode(); // Reset về Manual nếu có lỗi nghiêm trọng
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        hopeRoundButton2.Enabled = true;
                    }
                }
            }
        }
        private void LoadNccComboBoxDataSource(int? selectMaNCC = null)
        {
            try
            {
                List<DTO_Ncap> dataSource = busNcc.LayDSNcc(); // Lấy danh sách NCC từ BUS
                CbNcc.DataSource = null; // Ngắt kết nối nguồn dữ liệu cũ
                CbNcc.DisplayMember = "TenNCC";
                CbNcc.ValueMember = "MaNCC";
                CbNcc.DataSource = dataSource; // Gán nguồn dữ liệu mới

                bool selectionMade = false;
                if (selectMaNCC.HasValue && dataSource != null && dataSource.Any(n => n.MaNCC == selectMaNCC.Value))
                {
                    CbNcc.SelectedValue = selectMaNCC.Value;
                    selectionMade = CbNcc.SelectedValue != null && CbNcc.SelectedValue.Equals(selectMaNCC.Value);
                }

                if (!selectionMade)
                {
                    CbNcc.SelectedIndex = -1; // Không chọn gì cả nếu không có yêu cầu hoặc không tìm thấy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CbNcc.DataSource = null;
                CbNcc.SelectedIndex = -1;
            }
            UpdateNhapHangButtonState();
        }

        private void pictureBox3_Click(object sender, EventArgs e) // Nút thoát/quay lại
        {
            if (dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
            {
                var confirmResult = MessageBox.Show("Phiếu nhập hàng hiện tại có dữ liệu chưa được lưu. Bạn có chắc chắn muốn thoát và hủy bỏ thay đổi?",
                                                      "Xác nhận Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.No) { return; /* Không làm gì cả */ }
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


        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnCancelImport_Click(object sender, EventArgs e)
        {
            if (!_isImportMode) return; // Chỉ hoạt động ở chế độ import

            var confirmResult = MessageBox.Show("Bạn có chắc muốn hủy bỏ dữ liệu đã tải từ Excel và quay lại chế độ nhập thủ công?",
                                                  "Xác nhận Hủy Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Console.WriteLine("User cancelled Import mode. Resetting to Manual Mode...");
                ResetToManualMode();
            }
        }

        private void CbNgNhap_SelectedIndexChanged(object sender, EventArgs e) { UpdateNhapHangButtonState(); /* Cập nhật nút khi đổi người nhập */ }
        private void Tbnote_TextChanged(object sender, EventArgs e) { /* Không cần xử lý đặc biệt */ }
        private void CbNcc_SelectedIndexChanged(object sender, EventArgs e) { UpdateNhapHangButtonState(); /* Cập nhật nút khi đổi NCC */ }
        private void CbKho_SelectedIndexChanged(object sender, EventArgs e) { UpdateNhapHangButtonState(); /* Cập nhật nút khi đổi Kho */ }

        private void ketqua_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
