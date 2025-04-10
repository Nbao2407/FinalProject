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
namespace QLVT
{
    public partial class NhapHang : Form
    {
        private readonly BUS_Order busOrder = new BUS_Order();
        private readonly BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private readonly BUS_TK busTk = new BUS_TK();
        private readonly BUS_Ncc busNcc = new BUS_Ncc();
        private readonly DAL_Order dalOrder = new DAL_Order();
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
            dtpNgayNhap.Value = DateTime.Now;
            dtpNgayNhap.MaxDate = DateTime.Now;
        }

        private void InitializeCustomComponents()
        {
            this.Resize += Frm_Resize;
            debounceTimer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
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
                Width = 180
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã", ReadOnly = true, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "Đơn Vị", ReadOnly = true });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", ReadOnly = false, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá Nhập", ReadOnly = false, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xoá", Text = "X", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTiet.CellClick += DgvChiTiet_CellClick;
            dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            dgvChiTiet.RowsAdded += (s, e) => UpdatePanelVisibility();
            dgvChiTiet.RowsRemoved += (s, e) => UpdatePanelVisibility();
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
            if (dgvChiTiet.Columns.Contains("MaVatLieu"))
            {
                dgvChiTiet.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
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
                if (CurrentUser.TenDangNhap != null)
                {
                    lblNgTao.Text = CurrentUser.TenDangNhap;
                }
                else
                {
                    lblNgTao.Text = "N/A";
                    MessageBox.Show("Không thể xác định người dùng hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                CbNcc.DataSource = busNcc.LayDSNcc();
                CbNcc.DisplayMember = "TenNCC";
                CbNcc.ValueMember = "MaNCC";
                CbNcc.SelectedIndex = -1;
                CbNgNhap.DataSource = busTk.LayDSNgNhap();
                CbNgNhap.DisplayMember = "TenDangNhap";
                CbNgNhap.ValueMember = "MaTK";
                CbNgNhap.SelectedValue = CurrentUser.MaTK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string searchQuery = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ketqua.Visible = false;
                if (_isImportMode)
                {
                    FilterAndDisplayDgvChiTiet();
                }
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
            try
            {
                QLVatLieu qlv = new QLVatLieu();
                List<DTO_VatLieu> results = await Task.Run(() => qlv.SearchProducts(searchQuery));
                Console.WriteLine($"DB Search Query: {searchQuery}, Results Found: {results.Count}");
                UpdateSearchSuggestions(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm vật liệu trong CSDL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (columnName == "Tang" || columnName == "Giam" || columnName == "Xoa")
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
                if (int.TryParse(input, out int soLuong) && soLuong > 0)
                {
                    cell.Value = soLuong;
                }
                else
                {
                    MessageBox.Show("Số lượng phải là một số nguyên lớn hơn 0.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = 1;
                }
            }
            else if (columnName == "GiaNhap")
            {
                string input = cell.Value?.ToString();
                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0)
                {
                    cell.Value = giaNhap;
                }
                else
                {
                    MessageBox.Show("Giá nhập phải là một số không âm.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = 0m;
                }
            }
            TinhTongSoLuongVaGiaNhap();
        }

        private async void btnNhapHang_Click(object sender, EventArgs e)
        {
            if (_isImportMode)
            {
                this.Cursor = Cursors.WaitCursor;
                btnNhapHang.Enabled = false;
                var groupedDataFromGrid = new Dictionary<string, List<DTO_Order>>();
                var newNccPotentialInfo = new Dictionary<string, VatLieuExcelDayDu>();
                var rowsWithErrorAfterEdit = new List<string>();
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue;
                    string tenNCC = row.Cells["TenNCC"].Value?.ToString();
                    if (string.IsNullOrEmpty(tenNCC))
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1}: Thiếu tên Nhà cung cấp sau khi sửa đổi.");
                        continue;
                    }
                    bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL) && maVL > 0;
                    bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong) && soLuong > 0;
                    bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0;
                    if (!isMaVLValid || !isSoLuongValid || !isGiaNhapValid)
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}): Dữ liệu Mã VL/SL/Giá không hợp lệ sau khi sửa đổi.");
                        continue;
                    }
                    DTO_Order chiTiet = new DTO_Order
                    {
                        MaVatLieu = maVL,
                        SoLuong = soLuong,
                        GiaNhap = (int)giaNhap
                    };
                    if (!groupedDataFromGrid.ContainsKey(tenNCC))
                    {
                        groupedDataFromGrid[tenNCC] = new List<DTO_Order>();
                    }
                    groupedDataFromGrid[tenNCC].Add(chiTiet);
                    if (!newNccPotentialInfo.ContainsKey(tenNCC))
                    {
                        var originalData = _loadedExcelData?.FirstOrDefault(d => d.TenNCC == tenNCC);
                        if (originalData != null)
                        {
                            newNccPotentialInfo[tenNCC] = originalData;
                        }
                    }
                }
                if (rowsWithErrorAfterEdit.Any())
                {
                    MessageBox.Show("Có lỗi dữ liệu trên lưới sau khi sửa đổi:\n- " + string.Join("\n- ", rowsWithErrorAfterEdit), "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Cursor = Cursors.Default;
                    btnNhapHang.Enabled = true;
                    return;
                }
                if (!groupedDataFromGrid.Any())
                {
                    MessageBox.Show("Không có dữ liệu hợp lệ nào trên lưới để tạo phiếu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                    btnNhapHang.Enabled = true;
                    ResetToManualMode();
                }
                var ordersForExistingNcc = new Dictionary<int, List<DTO_Order>>();
                var ordersForNewNcc = new Dictionary<string, List<DTO_Order>>();
                var createdNccMapping = new Dictionary<string, int>();
                var nccCreationErrors = new List<string>();
                foreach (var kvp in groupedDataFromGrid)
                {
                    string tenNCC = kvp.Key;
                    List<DTO_Order> details = kvp.Value;
                    DAL_NCcap da = new DAL_NCcap();
                    DTO_Ncap existingNcc = da.TimNccTheoTen(tenNCC);
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
                                int newId = await da.ThemNccAsync(new DTO_Ncap
                                {
                                    TenNCC = tenNCC,
                                    SDT = nccDetails?.SdtNcc,
                                    DiaChi = nccDetails?.DiaChiNcc,
                                    Email = nccDetails?.EmailNcc,
                                    NguoiTao = CurrentUser.MaTK
                                });
                                if (newId > 0)
                                {
                                    createdNccMapping[tenNCC] = newId;
                                }
                                else
                                {
                                    nccCreationErrors.Add(tenNCC);
                                }
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
                if (!int.TryParse(CbNgNhap.SelectedValue?.ToString(), out int maTK)) { return; }
                System.DateOnly ngayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value);
                string ghiChu = Tbnote.Text.Trim();
                int maNguoiThucHien = CurrentUser.MaTK;
                foreach (var kvp in ordersForExistingNcc)
                {
                    int maNCC = kvp.Key;
                    List<DTO_Order> chiTiet = kvp.Value;
                    string tenNCCDisplay = busNcc.LayThongTinNCC(maNCC)?.TenNCC ?? $"NCC {maNCC}";
                    try
                    {
                        string chiTietJson = Newtonsoft.Json.JsonConvert.SerializeObject(chiTiet);
                        var result = await dalOrder.NhapHangAsync(ngayNhap, maNCC, maTK, ghiChu, chiTietJson, maNguoiThucHien);
                        if (result.Success) { successCount++; results.Add($" - {tenNCCDisplay}: Thành công (Mã ĐN: {result.MaDonNhap})"); }
                        else { failCount++; results.Add($" - {tenNCCDisplay}: Thất bại ({result.Message})"); }
                    }
                    catch (Exception ex) { failCount++; results.Add($" - {tenNCCDisplay}: Lỗi hệ thống ({ex.Message})"); }
                }
                foreach (var kvp in ordersForNewNcc)
                {
                    string tenNCC = kvp.Key;
                    List<DTO_Order> chiTiet = kvp.Value;
                    if (createdNccMapping.TryGetValue(tenNCC, out int newMaNCC))
                    {
                        try
                        {
                            var result = await busOrder.NhapHangAsync(ngayNhap, newMaNCC, maTK, ghiChu, chiTiet, maNguoiThucHien);
                            if (result.Success) { pendingCount++; results.Add($" - {tenNCC} (Mới): Thành công - Chờ xử lý (Mã ĐN: {result.MaDonNhap})"); }
                            else { failCount++; results.Add($" - {tenNCC} (Mới): Thất bại tạo đơn ({result.Message})"); }
                        }
                        catch (Exception ex) { failCount++; results.Add($" - {tenNCC} (Mới): Lỗi hệ thống khi tạo đơn ({ex.Message})"); }
                    }
                    else
                    {
                        failCount++;
                        results.Add($" - {tenNCC} (Mới): Thất bại (Không tạo được NCC)");
                    }
                }
                string finalMessage = $"Kết quả Tạo Phiếu:\nThất bại: {failCount} đơn.\n\nChi tiết:\n" + string.Join("\n", results);
                MessageBoxIcon icon = (failCount > 0 || nccCreationErrors.Any()) ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                MessageBox.Show(finalMessage, "Tạo Phiếu thành công", MessageBoxButtons.OK, icon);
                ResetToManualMode();
                this.Cursor = Cursors.Default;
            }
            else
            {
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
            if (!_isImportMode && CbNcc.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp từ danh sách trước khi thêm vật liệu.", "Thiếu Nhà Cung Cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNcc.Focus();
                return;
            }
            Console.WriteLine($"Adding new item {selectedItem.Ten} (Manual Mode)");
            int rowIndex = dgvChiTiet.Rows.Add(
                DBNull.Value,
                selectedItem.MaVatLieu,
                selectedItem.Ten,
                selectedItem.DonViTinh,
                1,
                selectedItem.DonGiaNhap,
                "+", "-", "X"
            );
            dgvChiTiet.ClearSelection();
            dgvChiTiet.Rows[rowIndex].Selected = true;
            if (!dgvChiTiet.Rows[rowIndex].Displayed)
            {
                dgvChiTiet.FirstDisplayedScrollingRowIndex = rowIndex;
            }
            if (dgvChiTiet.Columns.Contains("TenNCC") && _isImportMode == false)
            {
                dgvChiTiet.Columns["TenNCC"].Visible = false;
            }
            dgvChiTiet.CurrentCell = dgvChiTiet.Rows[rowIndex].Cells["SoLuong"];
            TinhTongSoLuongVaGiaNhap();
            UpdatePanelVisibility();
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
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.CurrentCulture,
                                 out decimal giaNhap);
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
                int fixedWidthTotal = 0;
                int fillColumnsCount = 0;
                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
                {
                    { "MaVatLieu", 80 },
                    { "DonViTinh", 80 },
                    { "SoLuong", 90 },
                    { "GiaNhap", 100 },
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
            dtpNgayNhap.Value = DateTime.Now;
            CbNcc.SelectedIndex = -1;
            Tbnote.Text = string.Empty;
            txtSearch.Text = string.Empty;
            ketqua.Visible = false;
            dgvChiTiet.Rows.Clear();
            TinhTongSoLuongVaGiaNhap();
            CenterPanelInDataGridView();
            CbNcc.Focus();
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
                    CbLocNCC.DataSource = null;
                    CbLocNCC.DisplayMember = "TenNCC";
                    CbLocNCC.ValueMember = "TenNCC";
                    CbLocNCC.DataSource = nccFilterList;
                    CbLocNCC.SelectedIndex = 0;
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

        private bool _isFilteringDgv = false;

        private void FilterAndDisplayDgvChiTiet()
        {
            if (!_isImportMode || _loadedExcelData == null || !_loadedExcelData.Any())
            {
                CbLocNCC.Visible = false;
                return;
            }
            _isFilteringDgv = true;
            try
            {
                dgvChiTiet.SuspendLayout();
                dgvChiTiet.Rows.Clear();
                string selectedNccName = CbLocNCC?.SelectedValue?.ToString();
                bool showAllNcc = (selectedNccName == "-- Tất cả Nhà Cung Cấp --" || string.IsNullOrEmpty(selectedNccName));
                string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
                IEnumerable<VatLieuExcelDayDu> dataToDisplay = _loadedExcelData;
                if (!showAllNcc) { dataToDisplay = dataToDisplay.Where(d => d.TenNCC == selectedNccName); }
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    dataToDisplay = dataToDisplay.Where(d =>
                        (d.MaVatLieuStr?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (d.TenVatLieu?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (d.DonViTinh?.ToLowerInvariant().Contains(searchQuery) ?? false) ||
                        (showAllNcc && (d.TenNCC?.ToLowerInvariant().Contains(searchQuery) ?? false))
                    );
                }
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
            TinhTongSoLuongVaGiaNhap();
            UpdatePanelVisibility();
        }

        private void dgvChiTiet_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_isFilteringDgv)
            {
                return;
            }
            TinhTongSoLuongVaGiaNhap();
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

        private void CbLocNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isImportMode && CbLocNCC.Focused)
            {
                FilterAndDisplayDgvChiTiet();
            }
        }

        private void UpdateUIForMode()
        {
            bool isManualMode = !_isImportMode;
            CbNcc.Visible = isManualMode;
            CbLocNCC.Visible = _isImportMode;
            txtSearch.Enabled = true;
            CbNgNhap.Enabled = true;
            dtpNgayNhap.Enabled = true;
            Tbnote.Enabled = isManualMode;
            ketqua.Visible = false;
            btnCancelImport.Visible = true;
            btnCancelImport.Enabled = true;
            if (dgvChiTiet.Columns.Contains("TenNCC"))
            {
                dgvChiTiet.Columns["TenNCC"].Visible = _isImportMode;
            }
            if (_isImportMode)
            {
                btnNhapHang.Text = "Tạo Phiếu";
                btnNhapHang.Enabled = _loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null);
                if (_loadedExcelData != null && _loadedExcelData.Any()) { LoadNccFilterComboBox(); } else { CbLocNCC.DataSource = null; CbLocNCC.Enabled = false; }
                FilterAndDisplayDgvChiTiet();
            }
            else
            {
                CbLocNCC.DataSource = null;
                CbLocNCC.Visible = false;
                btnNhapHang.Text = "Tạo phiếu";
                UpdateNhapHangButtonState();
                if (dgvChiTiet.Rows.Count > 0)
                {
                    dgvChiTiet.Rows.Clear();
                }
                TinhTongSoLuongVaGiaNhap();
            }
            UpdatePanelVisibility();
        }

        private void UpdateNhapHangButtonState()
        {
            if (_isImportMode)
            {
                return;
            }
            bool isNccSelected = CbNcc.SelectedIndex >= 0 && CbNcc.SelectedItem is DTO_Ncap;
            bool hasGridRows = dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            btnNhapHang.Enabled = isNccSelected && hasGridRows;
        }

        private void LoadNccFilterComboBox()
        {
            if (!_isImportMode || _loadedExcelData == null)
            {
                CbLocNCC.DataSource = null;
                CbLocNCC.Items.Clear();
                CbLocNCC.Enabled = false;
                return;
            }
            var nccFilterList = new List<NccFilterItem>();
            nccFilterList.Add(new NccFilterItem { TenNCC = "-- Tất cả Nhà Cung Cấp --" });
            var uniqueNccNames = _loadedExcelData
                                .Where(d => d.RowError == null && !string.IsNullOrEmpty(d.TenNCC))
                                .Select(d => d.TenNCC)
                                .Distinct()
                                .OrderBy(name => name);
            foreach (var name in uniqueNccNames)
            {
                nccFilterList.Add(new NccFilterItem { TenNCC = name });
            }
            CbLocNCC.DataSource = null;
            CbLocNCC.DisplayMember = "TenNCC";
            CbLocNCC.ValueMember = "TenNCC";
            CbLocNCC.DataSource = nccFilterList;
            CbLocNCC.SelectedIndex = 0;
            CbLocNCC.Enabled = true;
        }

        private void ResetToManualMode()
        {
            _isImportMode = false;
            _loadedExcelData = null;
            LoadNccComboBoxDataSource();
            UpdateUIForMode();
            CbNcc.Focus();
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
                    hopeRoundButton2.Enabled = false;
                    btnNhapHang.Enabled = false;
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
                        hopeRoundButton2.Enabled = true;
                    }
                }
            }
        }

        private void LoadNccComboBoxDataSource(int? selectMaNCC = null)
        {
            try
            {
                List<DTO_Ncap> dataSource = busNcc.LayDSNcc();
                if (selectMaNCC.HasValue && dataSource != null)
                {
                    bool foundInList = dataSource.Any(n => n.MaNCC == selectMaNCC.Value);
                    Console.WriteLine($"LoadNccComboBoxDataSource: Trying to select ID {selectMaNCC.Value}. Found in fetched list: {foundInList}.");
                }
                CbNcc.DataSource = null;
                CbNcc.DisplayMember = "TenNCC";
                CbNcc.ValueMember = "MaNCC";
                CbNcc.DataSource = dataSource;
                bool selectionMade = false;
                if (selectMaNCC.HasValue && dataSource != null && dataSource.Any(n => n.MaNCC == selectMaNCC.Value))
                {
                    CbNcc.SelectedValue = selectMaNCC.Value;
                    if (CbNcc.SelectedValue != null && CbNcc.SelectedValue.Equals(selectMaNCC.Value))
                    {
                        Console.WriteLine($"LoadNccComboBoxDataSource: Successfully selected ID {selectMaNCC.Value}.");
                        selectionMade = true;
                    }
                    else
                    {
                        var itemToSelect = dataSource.FirstOrDefault(n => n.MaNCC == selectMaNCC.Value);
                        if (itemToSelect != null)
                        {
                            CbNcc.SelectedItem = itemToSelect;
                            if (CbNcc.SelectedItem == itemToSelect)
                            {
                                Console.WriteLine($"LoadNccComboBoxDataSource: Successfully selected ID {selectMaNCC.Value} using SelectedItem.");
                                selectionMade = true;
                            }
                            else
                            {
                                Console.WriteLine($"*** LoadNccComboBoxDataSource: FAILED to select ID {selectMaNCC.Value} using SelectedValue AND SelectedItem.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"*** LoadNccComboBoxDataSource: FAILED to select ID {selectMaNCC.Value} using SelectedValue. Item could not be refound for SelectedItem attempt.");
                        }
                    }
                }
                if (!selectionMade)
                {
                    CbNcc.SelectedIndex = -1;
                    if (selectMaNCC.HasValue)
                    {
                        Console.WriteLine($"LoadNccComboBoxDataSource: Could not select requested ID {selectMaNCC.Value}. Set SelectedIndex to -1.");
                    }
                    else
                    {
                        Console.WriteLine($"LoadNccComboBoxDataSource: No specific ID requested. Set SelectedIndex to -1.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"*** EXCEPTION in LoadNccComboBoxDataSource: {ex.ToString()}");
                CbNcc.DataSource = null;
                CbNcc.SelectedIndex = -1;
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
                FrmOrder order = new FrmOrder()
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
    }
}