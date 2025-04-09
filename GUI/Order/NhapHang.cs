using BUS;
using DAL;
using DTO;
using System.IO; // Cho FileInfo
using OfficeOpenXml; // Thư viện EPPlus
using OfficeOpenXml.Style; // Cho định dạng (nếu cần)
using System.Collections.Generic; // Cho List
using System.Linq; // Cho LINQ (FirstOrDefault, Any)
using System.Globalization;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualBasic;

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
            dgvChiTiet.Columns.Clear(); // Ensure it's clean before adding

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
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá Nhập", ReadOnly = false, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } }); // Editable, formatted

            // Button Columns
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
            dgvChiTiet.RowHeadersVisible = false; // Hide the default row header column
            dgvChiTiet.BackgroundColor = Color.White; // Background if rows don't fill space

            dgvChiTiet.EnableHeadersVisualStyles = false;

            dgvChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single; // Simple border
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(66, 139, 202); // A nice blue shade (adjust if needed)
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // White text
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // Font: Segoe UI, 10pt, Bold
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center text
            dgvChiTiet.ColumnHeadersHeight = 40; // Increase header height
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; // Optional: Prevent user resizing header height

            dgvChiTiet.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
            dgvChiTiet.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTiet.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);          // Font: Segoe UI, 9.5pt, Regular
            dgvChiTiet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Default alignment
            dgvChiTiet.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0); // Add horizontal padding within cells

            dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = Color.White; // White background
            dgvChiTiet.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51); // Same dark grey text

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
        // --- DebounceTimer_Tick, txtSearch_TextChanged, DgvChiTiet_CellClick, DgvChiTiet_CellEndEdit (Keep as is) ---
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            PerformSearch();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Check column index too

            // Check if the click is on a button column
            if (dgvChiTiet.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
                int currentSoLuong = 0;
                // Try parsing, default to 0 if invalid/null before increment/decrement
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out currentSoLuong);

                switch (columnName)
                {
                    case "Tang":
                        row.Cells["SoLuong"].Value = Math.Max(1, currentSoLuong + 1); // Ensure it doesn't go below 1 implicitly
                        break;

                    case "Giam":
                        if (currentSoLuong > 1) // Only decrease if greater than 1
                        {
                            row.Cells["SoLuong"].Value = currentSoLuong - 1;
                        }
                        break;

                    case "Xoa":
                        // Optional: Confirmation dialog
                        // if (MessageBox.Show("Bạn có chắc muốn xoá dòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        // {
                        dgvChiTiet.Rows.RemoveAt(e.RowIndex);
                        // }
                        break;
                }

                // Recalculate totals if quantity changed or row removed
                if (columnName == "Tang" || columnName == "Giam" || columnName == "Xoa")
                {
                    TinhTongSoLuongVaGiaNhap();
                }
            }
        }

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex]; // Get the row
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (columnName == "SoLuong")
            {
                string input = cell.Value?.ToString();
                if (int.TryParse(input, out int soLuong) && soLuong > 0)
                {
                    // Value is valid, keep it
                    cell.Value = soLuong; // Ensure it's stored as int
                }
                else
                {
                    MessageBox.Show("Số lượng phải là một số nguyên lớn hơn 0.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Revert to a default value or previous value if available
                    // For simplicity, setting to 1 if invalid
                    cell.Value = 1;
                    // Optional: You could store the previous value in CellBeginEdit and restore it here.
                }
            }
            else if (columnName == "GiaNhap")
            {
                string input = cell.Value?.ToString();
                // Use NumberStyles.Any to handle potential currency symbols or separators
                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0)
                {
                    cell.Value = giaNhap; // Store as decimal
                }
                else
                {
                    MessageBox.Show("Giá nhập phải là một số không âm.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = 0m; // Default to 0 if invalid
                }
            }

            TinhTongSoLuongVaGiaNhap();
        }

        private async void btnNhapHang_Click(object sender, EventArgs e)
        {
            if (_isImportMode)
            {
                this.Cursor = Cursors.WaitCursor;
                btnNhapHang.Enabled = false; // Vô hiệu hóa nút trong khi xử lý

                var groupedDataFromGrid = new Dictionary<string, List<DTO_Order>>(); // Key là TenNCC
                var newNccPotentialInfo = new Dictionary<string, VatLieuExcelDayDu>(); // Lưu chi tiết NCC mới tiềm năng
                var rowsWithErrorAfterEdit = new List<string>(); // Lỗi phát hiện khi đọc lại DGV

                // --- 1. Đọc và nhóm dữ liệu từ DGV ---
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue;

                    string tenNCC = row.Cells["TenNCC"].Value?.ToString();
                    if (string.IsNullOrEmpty(tenNCC))
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1}: Thiếu tên Nhà cung cấp sau khi sửa đổi.");
                        continue;
                    }

                    bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL) && maVL > 0; // Cần Mã VL hợp lệ
                    bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong) && soLuong > 0;
                    bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0;

                    if (!isMaVLValid || !isSoLuongValid || !isGiaNhapValid)
                    {
                        rowsWithErrorAfterEdit.Add($"Dòng {row.Index + 1} (NCC: {tenNCC}): Dữ liệu Mã VL/SL/Giá không hợp lệ sau khi sửa đổi.");
                        continue; // Bỏ qua dòng lỗi vật liệu
                    }

                    DTO_Order chiTiet = new DTO_Order
                    {
                        MaVatLieu = maVL,
                        SoLuong = soLuong,
                        GiaNhap = (int)giaNhap // Hoặc decimal
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
                            newNccPotentialInfo[tenNCC] = originalData; // Lưu lại để lấy SĐT, Email... sau
                        }
                    }
                } // End foreach DGV rows

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
                    DTO_Ncap existingNcc = da.TimNccTheoTen(tenNCC); // Tìm trong DB

                    if (existingNcc != null)
                    {
                        ordersForExistingNcc[existingNcc.MaNCC] = details; // Thêm vào nhóm NCC đã có
                    }
                    else
                    {
                        ordersForNewNcc[tenNCC] = details; // Thêm vào nhóm NCC mới cần tạo

                        if (!createdNccMapping.ContainsKey(tenNCC) && !nccCreationErrors.Contains(tenNCC)) // Chỉ tạo 1 lần
                        {
                            VatLieuExcelDayDu nccDetails = null;
                            newNccPotentialInfo.TryGetValue(tenNCC, out nccDetails); // Lấy SĐT, Email... đã lưu

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
                    else // NCC này tạo bị lỗi
                    {
                        failCount++; // Coi như đơn hàng thất bại
                        results.Add($" - {tenNCC} (Mới): Thất bại (Không tạo được NCC)");
                    }
                }

                string finalMessage = $"Kết quả Tạo Phiếu:\nThành công: {successCount} đơn.\nThất bại: {failCount} đơn.\n\nChi tiết:\n" + string.Join("\n", results);
                MessageBoxIcon icon = (failCount > 0 || nccCreationErrors.Any()) ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
                MessageBox.Show(finalMessage, "Tạo Phiếu thành công", MessageBoxButtons.OK, icon);

                ResetToManualMode(); 

                this.Cursor = Cursors.Default;
            }
            else
            {
                // Dán code gốc của btnNhapHang_Click vào đây
                // ...
                // Ví dụ gọi lại hàm cũ nếu bạn tách ra:
                // await XuLyNhapHangThuCongAsync();
            }
        }

        private async void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ketqua.Visible = false;
                return;
            }

            try
            {
                QLVatLieu qlv = new QLVatLieu();
                List<DTO_VatLieu> results = qlv.SearchProducts(searchQuery);
                Console.WriteLine($"Search Query: {searchQuery}, Results Found: {results.Count}");
                UpdateSearchSuggestions(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm vật liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ketqua.Visible = false;
            }
        }

        private void UpdateSearchSuggestions(List<DTO_VatLieu> results)
        {
            ketqua.SuspendLayout();
            ketqua.Controls.Clear();

            if (results != null && results.Any()) // Add null check
            {
                ketqua.Height = Math.Min(results.Count * 35, 210); // Max height 210px
                ketqua.BackColor = Color.WhiteSmoke; // Or a suitable background

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        // Display relevant info, e.g., Name and Code
                        Text = $"{item.Ten} ({item.MaVatLieu}) - {item.DonViTinh}", // Added DVT
                        Tag = item, // Store the full DTO object
                        Dock = DockStyle.Top,
                        Height = 35,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Padding = new Padding(10, 0, 0, 0), // Indent text
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = Color.FromArgb(33, 37, 41), // Dark text color
                        BackColor = defaultLabelColor, // Use defined default color
                        Cursor = Cursors.Hand,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    lbl.MouseEnter += (s, e) => { lbl.BackColor = hoverLabelColor; lbl.ForeColor = Color.FromArgb(0, 102, 204); }; // Use defined hover color
                    lbl.MouseLeave += (s, e) => { lbl.BackColor = defaultLabelColor; lbl.ForeColor = Color.FromArgb(33, 37, 41); };

                    lbl.Click += (s, e) =>
                    {
                        if (lbl.Tag is DTO_VatLieu selectedItem)
                        {
                            AddVatLieuToDgvChiTiet(selectedItem);
                            ketqua.Visible = false; // Hide suggestions panel
                            txtSearch.Text = String.Empty; ; // Clear search box
                            txtSearch.Focus(); // Return focus to search box
                        }
                    };

                    ketqua.Controls.Add(lbl);
                    lbl.BringToFront(); // Ensure labels are drawn correctly top-down
                }
                ketqua.Visible = true; // Show the panel
            }
            else
            {
                ketqua.Visible = false; // Hide if no results
            }
            ketqua.ResumeLayout(); // Apply layout changes
        }

        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            // Check if the material already exists in the DGV
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue; // Skip the template row
                // Safe check for null before accessing Value
                if (row.Cells["MaVatLieu"].Value != null && (int)row.Cells["MaVatLieu"].Value == selectedItem.MaVatLieu)
                {
                    // Material exists, increment quantity
                    int currentSoLuong = 0;
                    int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out currentSoLuong);
                    row.Cells["SoLuong"].Value = Math.Max(1, currentSoLuong + 1); // Increment, ensure > 0

                    // Optional: Highlight the updated row
                    dgvChiTiet.ClearSelection();
                    row.Selected = true;
                    dgvChiTiet.FirstDisplayedScrollingRowIndex = row.Index; // Scroll to the row

                    TinhTongSoLuongVaGiaNhap(); // Recalculate totals
                    return; // Exit the method
                }
            }

            // Material does not exist, add a new row
            int rowIndex = dgvChiTiet.Rows.Add(
                selectedItem.MaVatLieu,
                selectedItem.Ten,
                selectedItem.DonViTinh,
                1, // Default quantity
                   // Use DonGiaNhap if available in DTO_VatLieu, otherwise default to 0 or prompt user
                selectedItem.DonGiaNhap // Assuming DTO_VatLieu has DonGiaNhap
                                        // If not, use 0m or fetch default price:
                                        // 0m // Default to 0
            );

            // Optional: Select the newly added row
            dgvChiTiet.ClearSelection();
            dgvChiTiet.Rows[rowIndex].Selected = true;
            dgvChiTiet.FirstDisplayedScrollingRowIndex = rowIndex; // Scroll to the new row

            TinhTongSoLuongVaGiaNhap(); // Recalculate totals
            panelImportExcel.Visible = false; // Hide the import prompt panel
            CenterPanelInDataGridView(); // Recenter if needed (though visibility handles it)
        }

        private void TinhTongSoLuongVaGiaNhap()
        {
            int tongSoLuong = 0;
            decimal tongGiaNhap = 0;

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue; // Skip the 'new row' template

                // Safely parse values, defaulting to 0 if parsing fails or value is null
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(),
                                 System.Globalization.NumberStyles.Any, // Allow various number formats
                                 System.Globalization.CultureInfo.CurrentCulture, // Use system's culture settings for parsing
                                 out decimal giaNhap);

                tongSoLuong += soLuong;
                tongGiaNhap += giaNhap * soLuong; // Calculate total value for the row
            }

            lblSl.Text = tongSoLuong.ToString("N0"); // Format quantity with separators
            lblTongTien.Text = tongGiaNhap.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VND"; // Format currency for Vietnam
        }

        // --- Frm_Resize, ResizeColumns, CenterPanelInDataGridView (Keep as is, adjust ResizeColumns if needed after docking) ---
        private void Frm_Resize(object sender, EventArgs e)
        {
            // ResizeColumns might need less work if Dock=Fill handles main sizing
            ResizeColumns();
            CenterPanelInDataGridView(); // Keep centering the import panel
        }

        private void ResizeColumns()
        {
            if (dgvChiTiet.Columns.Count == 0 || dgvChiTiet.ClientSize.Width <= 0) return;

            try // Add try-catch for safety during resize events
            {
                // Calculate total width available, excluding vertical scrollbar if visible
                int availableWidth = dgvChiTiet.ClientSize.Width;
                if (dgvChiTiet.Controls.OfType<VScrollBar>().Any(v => v.Visible))
                {
                    availableWidth -= SystemInformation.VerticalScrollBarWidth;
                }

                // Define fixed widths for specific columns
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
                    if (!col.Visible) continue; // Skip hidden columns

                    if (fixedWidths.ContainsKey(col.Name))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        // Ensure width is not negative or excessively large if availableWidth is small
                        col.Width = Math.Max(20, Math.Min(fixedWidths[col.Name], availableWidth - fixedWidthTotal - 50)); // Min width 20
                        fixedWidthTotal += col.Width;
                    }
                    // Identify columns that should fill remaining space (like TenVatLieu)
                    else if (col.Name == "TenVatLieu") // Explicitly target fill column
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        fillColumnsCount++;
                    }
                    else // Other columns might get a default small width or be set later
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        //col.Width = 50; // Assign a default minimum if needed
                        //fixedWidthTotal += col.Width;
                    }
                }

                // Adjust the fill column(s) if necessary (though AutoSizeMode.Fill usually handles this)
                // This part might be redundant if AutoSizeMode.Fill works well with Dock=Fill
                // int remainingWidth = availableWidth - fixedWidthTotal;
                // int widthPerFillColumn = fillColumnsCount > 0 ? Math.Max(80, remainingWidth / fillColumnsCount) : 80; // Min width 80 for fill column
                //
                // if (dgvChiTiet.Columns.Contains("TenVatLieu") && dgvChiTiet.Columns["TenVatLieu"].Visible)
                // {
                //     dgvChiTiet.Columns["TenVatLieu"].Width = widthPerFillColumn;
                // }

                // Ensure the 'Fill' column actually fills correctly after setting others
                if (dgvChiTiet.Columns.Contains("TenVatLieu"))
                {
                    dgvChiTiet.Columns["TenVatLieu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    // Optional: Set a MinimumWidth for the fill column
                    dgvChiTiet.Columns["TenVatLieu"].MinimumWidth = 150;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during column resize: {ex.Message}");
                // Handle or log the exception appropriately
            }
        }

        private void CenterPanelInDataGridView()
        {
            if (panelImportExcel != null && dgvChiTiet != null && panelImportExcel.Parent == dgvChiTiet.Parent) // Ensure they share the same parent
            {
                // Center relative to the DataGridView's bounds within its parent container
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
                    panelImportExcel.BringToFront(); // Make sure it's on top if visible
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
            _loadedExcelData = new List<VatLieuExcelDayDu>(); // Khởi tạo list mới
            var errorMessages = new List<string>(); // Lưu lỗi chung khi đọc
            bool hasValidData = false; // Cờ kiểm tra có dữ liệu hợp lệ không

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        MessageBox.Show("File Excel không hợp lệ hoặc không có sheet nào.", "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Thoát sớm
                    }

                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int colCount = worksheet.Dimension?.Columns ?? 0;

                    if (rowCount <= 1)
                    {
                        MessageBox.Show("File Excel không có dữ liệu.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Thoát sớm
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
                        string rowError = null; // Lỗi của riêng dòng này

                        try // Bắt lỗi trên từng dòng đọc
                        {
                            dataRow.MaVatLieuStr = worksheet.Cells[row, 1]?.Value?.ToString().Trim() ?? "";
                            dataRow.TenVatLieu = worksheet.Cells[row, 2]?.Value?.ToString().Trim() ?? "";
                            dataRow.DonViTinh = worksheet.Cells[row, 3]?.Value?.ToString().Trim() ?? "";
                            dataRow.SoLuongStr = worksheet.Cells[row, 4]?.Value?.ToString().Trim() ?? "";
                            dataRow.GiaNhapStr = worksheet.Cells[row, 5]?.Value?.ToString().Trim() ?? "";
                            dataRow.TenNCC = worksheet.Cells[row, 6]?.Value?.ToString().Trim(); // Lấy cả tên NCC có thể null/empty

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
                                hasValidData = true; // Có ít nhất 1 dòng có vẻ hợp lệ
                            }
                        }
                        catch (Exception exReadCell)
                        {
                            rowError = $"Lỗi đọc ô: {exReadCell.Message}";
                        }

                        dataRow.RowError = rowError; // Lưu lỗi vào dòng
                        _loadedExcelData.Add(dataRow); // Thêm cả dòng lỗi vào list để xử lý sau

                    } // end for loop

                    if (!hasValidData && _loadedExcelData.Any()) // Có đọc được dòng nhưng toàn lỗi
                    {
                        MessageBox.Show("Không tìm thấy dòng dữ liệu vật liệu hợp lệ nào trong file Excel.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _loadedExcelData = null; // Đánh dấu là không có dữ liệu để load
                        return;
                    }
                    if (!_loadedExcelData.Any()) 
                    {
                        MessageBox.Show("Không đọc được dữ liệu nào từ file Excel.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    var nccFilterList = new List<NccFilterItem>();
                    nccFilterList.Add(new NccFilterItem { TenNCC = "-- Tất cả Nhà Cung Cấp --" }); // Mục chọn tất cả

                    var uniqueNccNames = _loadedExcelData
                                        .Where(d => !string.IsNullOrEmpty(d.TenNCC) && d.RowError == null) // Chỉ lấy NCC từ các dòng hợp lệ
                                        .Select(d => d.TenNCC)
                                        .Distinct()
                                        .OrderBy(name => name);

                    foreach (var name in uniqueNccNames)
                    {
                        nccFilterList.Add(new NccFilterItem { TenNCC = name });
                    }

                    CbLocNCC.DataSource = null;
                    CbLocNCC.DisplayMember = "TenNCC";
                    CbLocNCC.ValueMember = "TenNCC"; // Dùng tên để lọc
                    CbLocNCC.DataSource = nccFilterList;
                    CbLocNCC.SelectedIndex = 0; // Chọn "Tất cả"

                    FilterAndDisplayDgvChiTiet();

                } // end using package
            }
            catch (IOException ioEx) { /*...*/ errorMessages.Add($"Lỗi IO: {ioEx.Message}"); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) when (invOpEx.Message.Contains("license")) { /*...*/ errorMessages.Add($"Lỗi License: {invOpEx.Message}"); _loadedExcelData = null; }
            catch (InvalidOperationException invOpEx) { /*...*/ errorMessages.Add($"Lỗi định dạng file: {invOpEx.Message}"); _loadedExcelData = null; }
            catch (Exception ex) { /*...*/ errorMessages.Add($"Lỗi không xác định: {ex.Message}"); _loadedExcelData = null; }

            // Hiển thị lỗi chung nếu có
            if (errorMessages.Any())
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc file Excel:\n- " + string.Join("\n- ", errorMessages), "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FilterAndDisplayDgvChiTiet()
        {
            dgvChiTiet.Rows.Clear();
            if (_loadedExcelData == null || !_loadedExcelData.Any()) return; // Không có dữ liệu để hiển thị

            string selectedNccName = CbLocNCC.SelectedValue?.ToString();
            bool showAll = (selectedNccName == "-- Tất cả Nhà Cung Cấp --");

            var filteredData = showAll
                ? _loadedExcelData // Lấy tất cả
                : _loadedExcelData.Where(d => d.TenNCC == selectedNccName); // Lọc theo tên

            dgvChiTiet.SuspendLayout(); // Tạm dừng vẽ
            foreach (var dataRow in filteredData)
            {
                int.TryParse(dataRow.SoLuongStr, out int soLuong);
                decimal.TryParse(dataRow.GiaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal giaNhap);
                int.TryParse(dataRow.MaVatLieuStr, out int maVatLieu); // Parse cả mã VL

                string tenVL = dataRow.TenVatLieu;
                string dvt = dataRow.DonViTinh;

                dgvChiTiet.Rows.Add(
                    dataRow.TenNCC,
                    maVatLieu > 0 ? (object)maVatLieu : DBNull.Value, 
                    tenVL,
                    dvt,
                    soLuong > 0 ? (object)soLuong : DBNull.Value,
                    giaNhap >= 0 ? (object)giaNhap : DBNull.Value, 
                    "+", "-", "X" 
                );

                if (!string.IsNullOrEmpty(dataRow.RowError))
                {
                    dgvChiTiet.Rows[dgvChiTiet.RowCount - 1].DefaultCellStyle.BackColor = Color.LightPink;

                    if (!string.IsNullOrEmpty(dataRow.RowError))
                    {
                        DataGridViewCell errorCell = dgvChiTiet.Rows[dgvChiTiet.RowCount - 1].Cells[0]; // Use the first cell of the row to display the tooltip
                        errorCell.ToolTipText = dataRow.RowError; // Set the tooltip text for the cell
                    }
                }
            }
            dgvChiTiet.ResumeLayout();
            TinhTongSoLuongVaGiaNhap(); // Tính tổng dựa trên DGV hiện tại
            UpdatePanelVisibility(); // Cập nhật panel import
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
            if (_isImportMode)
            {
                CbNcc.Visible = false;       
                CbLocNCC.Visible = true;      // Hiện C
                txtSearch.Enabled = false;    // Tắt tìm kiếm tay
                ketqua.Visible = false;
                btnNhapHang.Text = "Tạo Phiếu Nhập Từ Excel";
                btnNhapHang.Enabled = _loadedExcelData != null && _loadedExcelData.Any(d => d.RowError == null);
            }
            else
            {
                CbNcc.Visible = true;       
                CbLocNCC.Visible = false;     
                CbLocNCC.DataSource = null;  
                txtSearch.Enabled = true;    
                btnNhapHang.Text = "Nhập Hàng";
                btnNhapHang.Enabled = CbNcc.SelectedIndex >= 0 && dgvChiTiet.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            }
            UpdatePanelVisibility();
        }
        private void ResetToManualMode()
        {
            _isImportMode = false;
            _loadedExcelData = null;

            dgvChiTiet.Rows.Clear();
            TinhTongSoLuongVaGiaNhap();

            LoadNccComboBoxDataSource(); // Gọi hàm load CbNcc gốc

            UpdateUIForMode(); // Cập nhật giao diện về trạng thái nhập tay

            CbNcc.Focus(); // Đặt focus về CbNcc
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
                    this.Cursor = Cursors.WaitCursor;
                    hopeRoundButton2.Enabled = false; // Tạm tắt nút load
                    btnNhapHang.Enabled = false; // Tạm tắt nút tạo phiếu

                    try
                    {
                        await LoadExcelToGridAndFilterAsync(filePath); // Gọi hàm load mới

                        // Chỉ chuyển sang chế độ Import nếu load thành công và có dữ liệu
                        if (_loadedExcelData != null && _loadedExcelData.Any())
                        {
                            _isImportMode = true;
                            UpdateUIForMode(); // Hàm cập nhật giao diện theo chế độ
                        }
                        else
                        {
                            // Nếu load không thành công hoặc không có dữ liệu, reset về manual
                            ResetToManualMode();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi không mong muốn khi xử lý file Excel: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetToManualMode(); // Reset về manual nếu có lỗi nghiêm trọng
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        hopeRoundButton2.Enabled = true; // Bật lại nút load
                                                         // Nút btnNhapHang sẽ được bật/tắt trong UpdateUIForMode
                    }
                }
            }
        }
        private void LoadNccComboBoxDataSource(int? selectMaNCC = null) // Thêm tham số tùy chọn
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
    }
}