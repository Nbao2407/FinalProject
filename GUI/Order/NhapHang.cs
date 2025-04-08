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

namespace QLVT
{
    public partial class NhapHang : Form
    {
        private readonly BUS_Order busOrder = new BUS_Order();
        private readonly BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private readonly BUS_TK busTk = new BUS_TK();
        private readonly BUS_Ncc busNcc = new BUS_Ncc();
        private readonly DAL_Order dalOrder = new DAL_Order();

        private Form1 form1; // Assuming Form1 is the main form or parent

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

            // Define Columns
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã", ReadOnly = true, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "Đơn Vị", ReadOnly = true });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", ReadOnly = false, ValueType = typeof(int) });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá Nhập", ReadOnly = false, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } }); // Editable, formatted

            // Button Columns
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xoá", Text = "X", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });

            // Event Handlers for DGV
            dgvChiTiet.CellClick += DgvChiTiet_CellClick;
            dgvChiTiet.CellEndEdit += DgvChiTiet_CellEndEdit;
            dgvChiTiet.RowsAdded += (s, e) => panelImportExcel.Visible = (dgvChiTiet.RowCount == 0);
            dgvChiTiet.RowsRemoved += (s, e) => panelImportExcel.Visible = (dgvChiTiet.RowCount == 0);

            // Apply initial column sizing
            ResizeColumns();
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
            if (CbNcc.SelectedValue == null || !(CbNcc.SelectedValue is int))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Nhà cung cấp hợp lệ (từ Excel hoặc chọn).", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNcc.Focus();
                return;
            }

            int maTK;
            if (CbNgNhap.SelectedValue == null || !int.TryParse(CbNgNhap.SelectedValue.ToString(), out maTK))
            {
                MessageBox.Show("Vui lòng chọn Người nhập hợp lệ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNgNhap.Focus();
                return;
            }

            int maNCC = (int)CbNcc.SelectedValue;

            if (dgvChiTiet.Rows.Count == 0 || dgvChiTiet.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow)) // More robust check for empty DGV
            {
                MessageBox.Show("Vui lòng thêm ít nhất một vật liệu vào đơn nhập.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearch.Focus();
                return;
            }

            try
            {
                System.DateOnly ngayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value);
                string ghiChu = Tbnote.Text.Trim();

                var chiTietNhap = new List<DTO_Order>();
                bool dataValid = true;
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue;

                    int maVL = 0;
                    int soLuong = 0;
                    decimal giaNhap = 0m;

                    bool isMaVLValid = row.Cells["MaVatLieu"].Value != null && int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out maVL);
                    bool isSoLuongValid = row.Cells["SoLuong"].Value != null && int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out soLuong) && soLuong > 0;
                    bool isGiaNhapValid = row.Cells["GiaNhap"].Value != null && decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out giaNhap) && giaNhap >= 0;

                    if (!isMaVLValid || !isSoLuongValid || !isGiaNhapValid)
                    {
                        MessageBox.Show($"Dữ liệu không hợp lệ tại dòng {row.Index + 1}. Vui lòng kiểm tra lại Mã vật liệu, Số lượng (> 0) và Giá nhập (>= 0).",
                                        "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvChiTiet.ClearSelection();
                        row.Selected = true;
                        dgvChiTiet.CurrentCell = row.Cells[isMaVLValid ? (isSoLuongValid ? "GiaNhap" : "SoLuong") : "MaVatLieu"];
                        dgvChiTiet.BeginEdit(true);
                        dataValid = false;
                        break;
                    }

                    chiTietNhap.Add(new DTO_Order
                    {
                        MaVatLieu = maVL,
                        SoLuong = soLuong,
                        GiaNhap = (int)giaNhap
                    });
                }

                if (!dataValid)
                {
                    return;
                }

                if (!chiTietNhap.Any())
                {
                    MessageBox.Show("Không có chi tiết vật liệu hợp lệ nào để nhập.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maNguoiThucHien = CurrentUser.MaTK;

                var result = await busOrder.NhapHangAsync(ngayNhap, maNCC, maTK, ghiChu, chiTietNhap, maNguoiThucHien);

                if (result.Success)
                {
                    MessageBox.Show($"Nhập hàng thành công! Mã đơn nhập: {result.MaDonNhap}\nTổng tiền: {lblTongTien.Text}\nSố lượng: {lblSl.Text}",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                }
                else
                {
                    MessageBox.Show($"Nhập hàng thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException fEx)
            {
                MessageBox.Show($"Lỗi định dạng dữ liệu: {fEx.Message}. Vui lòng kiểm tra lại các giá trị nhập.", "Lỗi Định Dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định khi nhập hàng trong btnNhapHang_Click: {ex.ToString()}");
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình xử lý: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnNhapHang.Enabled = true;
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
                        // Optional: Add a thin border between items
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Hover effects
                    lbl.MouseEnter += (s, e) => { lbl.BackColor = hoverLabelColor; lbl.ForeColor = Color.FromArgb(0, 102, 204); }; // Use defined hover color
                    lbl.MouseLeave += (s, e) => { lbl.BackColor = defaultLabelColor; lbl.ForeColor = Color.FromArgb(33, 37, 41); };

                    // Click event to add item to DGV
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

                // Prevent panel from going outside DGV bounds (optional, depends on desired look)
                panelX = Math.Max(dgvChiTiet.Left, panelX);
                panelY = Math.Max(dgvChiTiet.Top, panelY);
                // Ensure it doesn't go past the right/bottom edge either
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

        private async void hopeRoundButton2_Click(object sender, EventArgs e) // Make async void
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Chọn file Excel để nhập hàng";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    // Show wait cursor
                    this.Cursor = Cursors.WaitCursor;
                    hopeRoundButton2.Enabled = false; // Disable button during import
                    try
                    {
                        // Call the async import method
                        await ImportExcelDataAsync(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Catch any unexpected errors from the async method itself
                        MessageBox.Show($"Lỗi không mong muốn trong quá trình import: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"Lỗi Import Excel chi tiết: {ex.ToString()}");
                    }
                    finally
                    {
                        // Restore cursor and button
                        this.Cursor = Cursors.Default;
                        hopeRoundButton2.Enabled = true;
                    }
                }
            }
        }

        private async Task ImportExcelDataAsync(string filePath) // Change to async Task
        {
            var importedItems = new List<DTO_VatLieuImport>();
            var errorRows = new List<string>();
            int? determinedMaNCC = null;
            string determinedTenNCC = null;
            bool nccDeterminedByFile = false;

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        MessageBox.Show("File Excel không hợp lệ hoặc không có sheet nào.", "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Return Task directly
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        bool rowHasData = false;
                        for (int c = 1; c <= Math.Min(5, colCount); c++)
                        {
                            if (worksheet.Cells[row, c].Value != null && !string.IsNullOrWhiteSpace(worksheet.Cells[row, c].Value.ToString()))
                            {
                                rowHasData = true;
                                break;
                            }
                        }
                        if (!rowHasData) continue;

                        string tenNccFromCell = null;
                        if (colCount >= 6)
                        {
                            tenNccFromCell = worksheet.Cells[row, 6].Value?.ToString().Trim();
                        }

                        if(!string.IsNullOrEmpty(tenNccFromCell) && !nccDeterminedByFile)
                   {
                            determinedTenNCC = tenNccFromCell;
                            Console.WriteLine($"Tìm thấy NCC trong Excel (dòng {row}): {determinedTenNCC}");

                            determinedMaNCC = await busNcc.TimHoacThemNccAsync(determinedTenNCC);

                            if (determinedMaNCC.HasValue && determinedMaNCC.Value > 0) // Tìm/Tạo thành công!
                            {
                                int validMaNCC = determinedMaNCC.Value;

                                Console.WriteLine($"Đã xác định/tạo NCC: {determinedTenNCC} (ID: {validMaNCC})");
                                nccDeterminedByFile = true;

                                LoadNccComboBoxDataSource(validMaNCC);

                                if (CbNcc.SelectedValue == null || !CbNcc.SelectedValue.Equals(validMaNCC))
                                {
                                    Console.WriteLine($"*** Cảnh báo: Không thể tự động chọn NCC mới (ID: {validMaNCC}) trong ComboBox sau khi tải lại. Kiểm tra hàm LoadNccComboBoxDataSource.");
                                }
                                else
                                {
                                    Console.WriteLine($"Đã chọn thành công NCC ID: {CbNcc.SelectedValue} trong ComboBox.");
                                }
                                break;
                            }
                            else 
                            {
                                errorRows.Add($"Dòng {row}: Không thể tìm hoặc tạo NCC '{determinedTenNCC}'. Dữ liệu từ file này sẽ không được nhập.");
                                MessageBox.Show(errorRows.LastOrDefault() ?? "Lỗi không xác định khi tìm/tạo NCC", "Lỗi Nhà Cung Cấp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; // Thoát khỏi ImportExcelDataAsync vì không có NCC hợp lệ
                            }
                        }
                    }

                    if (!nccDeterminedByFile)
                    {
                        if (CbNcc.SelectedValue != null && CbNcc.SelectedValue is int selectedMaNcc)
                        {
                            determinedMaNCC = selectedMaNcc;
                            determinedTenNCC = CbNcc.Text;
                            Console.WriteLine($"Sử dụng NCC đã chọn: {determinedTenNCC} (ID: {determinedMaNCC})");
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn Nhà cung cấp trước khi import hoặc đảm bảo file Excel có cột 'TenNhaCungCap' (cột 6).", "Thiếu Nhà Cung Cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            string maVatLieuStr = worksheet.Cells[row, 1].Value?.ToString().Trim() ?? "";
                            string tenVatLieu = worksheet.Cells[row, 2].Value?.ToString().Trim() ?? "";
                            string donViTinh = worksheet.Cells[row, 3].Value?.ToString().Trim() ?? "";
                            string soLuongStr = worksheet.Cells[row, 4].Value?.ToString().Trim() ?? "";
                            string giaNhapStr = worksheet.Cells[row, 5].Value?.ToString().Trim() ?? "";

                            if ((string.IsNullOrEmpty(maVatLieuStr) && string.IsNullOrEmpty(tenVatLieu)) || string.IsNullOrEmpty(soLuongStr) || string.IsNullOrEmpty(giaNhapStr))
                            {
                                if (!string.IsNullOrEmpty(maVatLieuStr) || !string.IsNullOrEmpty(tenVatLieu) || !string.IsNullOrEmpty(donViTinh) || !string.IsNullOrEmpty(soLuongStr) || !string.IsNullOrEmpty(giaNhapStr))
                                {
                                    errorRows.Add($"Dòng {row}: Thiếu thông tin Mã/Tên VL hoặc Số lượng hoặc Giá nhập.");
                                }
                                continue; // Skip blank or incomplete rows
                            }

                            int maVatLieu = 0; // Default
                            int soLuong;
                            decimal giaNhap;
                            DTO_VatLieu vatLieuInfo = null;

                            bool hasMaVL = int.TryParse(maVatLieuStr, out maVatLieu);
                            if (hasMaVL && maVatLieu > 0)
                            {
                                vatLieuInfo = await busVatLieu.LayThongTinVatLieuAsync(maVatLieu);
                                if (vatLieuInfo == null)
                                {
                                    errorRows.Add($"Dòng {row}: Không tìm thấy vật liệu với Mã '{maVatLieu}'.");
                                    continue;
                                }
                                tenVatLieu = string.IsNullOrEmpty(tenVatLieu) ? vatLieuInfo.Ten : tenVatLieu; // Use name from excel if provided
                                donViTinh = string.IsNullOrEmpty(donViTinh) ? vatLieuInfo.DonViTinh : donViTinh; // Use DVT from excel if provided
                            }
                            else if (!string.IsNullOrEmpty(tenVatLieu))
                            {
                                vatLieuInfo = busVatLieu.TimVatLieuTheoTenChinhXac(tenVatLieu); // Requires async method in BUS
                                if (vatLieuInfo == null)
                                {
                                    errorRows.Add($"Dòng {row}: Không tìm thấy vật liệu với Tên '{tenVatLieu}'.");
                                    continue;
                                }
                                maVatLieu = vatLieuInfo.MaVatLieu;
                                donViTinh = string.IsNullOrEmpty(donViTinh) ? vatLieuInfo.DonViTinh : donViTinh; // Use DVT from excel if provided
                            }
                            else
                            {
                                errorRows.Add($"Dòng {row}: Thiếu Mã Vật Liệu hoặc Tên Vật Liệu.");
                                continue;
                            }

                            if (!int.TryParse(soLuongStr, out soLuong) || soLuong <= 0)
                            {
                                errorRows.Add($"Dòng {row} (VL: {maVatLieu}): Số lượng '{soLuongStr}' không hợp lệ (> 0).");
                                continue;
                            }
                            if (!decimal.TryParse(giaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out giaNhap) || giaNhap < 0)
                            {
                                errorRows.Add($"Dòng {row} (VL: {maVatLieu}): Giá nhập '{giaNhapStr}' không hợp lệ (>= 0).");
                                continue;
                            }

                            importedItems.Add(new DTO_VatLieuImport
                            {
                                RowNumber = row,
                                MaVatLieu = maVatLieu,
                                Ten = tenVatLieu,
                                DonViTinh = donViTinh,
                                SoLuong = soLuong,
                                GiaNhap = giaNhap
                            });
                        }
                        catch (Exception exRow)
                        {
                            Console.WriteLine($"Lỗi xử lý dòng {row} trong Excel: {exRow.Message}");
                            errorRows.Add($"Dòng {row}: Lỗi xử lý dữ liệu ({exRow.Message.Split('\n')[0]})"); // Add concise error
                        }
                    } 
                    if (importedItems.Any())
                    {
                        dgvChiTiet.SuspendLayout();
                        int addedCount = 0;
                        int updatedCount = 0;

                        foreach (var item in importedItems)
                        {
                            bool exists = false;
                            foreach (DataGridViewRow gridRow in dgvChiTiet.Rows)
                            {
                                if (gridRow.IsNewRow) continue;
                                if (gridRow.Cells["MaVatLieu"].Value != null && (int)gridRow.Cells["MaVatLieu"].Value == item.MaVatLieu)
                                {
                                    int currentQty = (int)gridRow.Cells["SoLuong"].Value;
                                    gridRow.Cells["SoLuong"].Value = currentQty + item.SoLuong;
                                    gridRow.Cells["GiaNhap"].Value = item.GiaNhap; // Update price to the one from Excel
                                    gridRow.Cells["DonViTinh"].Value = item.DonViTinh; // Update DVT just in case
                                    gridRow.Cells["TenVatLieu"].Value = item.Ten; // Update Name just in case

                                    exists = true;
                                    updatedCount++;
                                    break;
                                }
                            }

                            if (!exists)
                            {
                                dgvChiTiet.Rows.Add(
                                    item.MaVatLieu,
                                    item.Ten,
                                    item.DonViTinh,
                                    item.SoLuong,
                                    item.GiaNhap
                                );
                                addedCount++;
                            }
                        }
                        dgvChiTiet.ResumeLayout();

                        TinhTongSoLuongVaGiaNhap();
                        panelImportExcel.Visible = false;
                        CenterPanelInDataGridView();

                        string message = $"Import hoàn tất cho NCC: '{determinedTenNCC}'.\n" +
                                         $"- Đã thêm mới: {addedCount} vật liệu.\n" +
                                         $"- Đã cập nhật số lượng/giá: {updatedCount} vật liệu.";

                        if (errorRows.Any())
                        {
                            message += $"\n\nCó {errorRows.Count} dòng lỗi không được import:\n- " +
                                       string.Join("\n- ", errorRows.Take(10));
                            if (errorRows.Count > 10) message += "\n- ... (và các lỗi khác)";
                            MessageBox.Show(message, "Import Hoàn Tất (Có Lỗi)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(message, "Import Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (!errorRows.Any())
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu vật liệu hợp lệ để import trong file Excel.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string errorMessage = $"Import thất bại. Không có vật liệu nào được thêm.\n" +
                                             $"Có {errorRows.Count} dòng lỗi:\n- " +
                                             string.Join("\n- ", errorRows.Take(10)); 
                        if (errorRows.Count > 10) errorMessage += "\n- ... (và các lỗi khác)";
                        MessageBox.Show(errorMessage, "Lỗi Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Lỗi truy cập file: {ioEx.Message}\nFile có thể đang được mở bởi ứng dụng khác.", "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException invOpEx) when (invOpEx.Message.Contains("license")) 
            {
                MessageBox.Show($"Lỗi thư viện Excel (EPPlus): {invOpEx.Message}\nVui lòng đảm bảo EPPlus license context được thiết lập (ExcelPackage.LicenseContext).", "Lỗi License EPPlus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(invOpEx.ToString());
            }
            catch (InvalidOperationException invOpEx)
            {
                MessageBox.Show($"Lỗi đọc file Excel: {invOpEx.Message}\nĐịnh dạng file không được hỗ trợ hoặc file bị lỗi.", "Lỗi Định Dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình import: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi Import Excel chi tiết: {ex.ToString()}");
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

            Control parentControl = this.Parent; // Get the container this form is in
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this); // Remove this form
                this.Dispose(); // Dispose resources of this form

                FrmOrder order = new FrmOrder()
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                    FormBorderStyle = FormBorderStyle.None // Ensure it fills correctly
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