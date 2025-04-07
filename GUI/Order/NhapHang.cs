using BUS;
using DAL;
using DTO;
using System.IO; // Cho FileInfo
using OfficeOpenXml; // Thư viện EPPlus
using OfficeOpenXml.Style; // Cho định dạng (nếu cần)
using System.Collections.Generic; // Cho List
using System.Linq; // Cho LINQ (FirstOrDefault, Any)
using System.Globalization;
namespace QLVT
{
    public partial class NhapHang : Form
    {
        private readonly BUS_Order busOrder = new BUS_Order();
        private readonly BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private readonly BUS_TK busTk = new BUS_TK();
        private readonly BUS_Ncc busNcc = new BUS_Ncc();
        private readonly DAL_Order dalOrder = new DAL_Order();

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
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System });
            dgvChiTiet.Columns.Add(new DataGridViewButtonColumn { Name = "Xoa", HeaderText = "Xoá", Text = "X", UseColumnTextForButtonValue = true, Width = 50, FlatStyle = FlatStyle.System });

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

            dgvChiTiet.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247); // Very light blue
            dgvChiTiet.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);    // Dark grey text
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
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Format = "N0"; // Format as integer
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
            if (e.RowIndex < 0 || e.ColumnIndex < dgvChiTiet.Columns["Tang"].Index) return;

            DataGridViewRow row = dgvChiTiet.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;
            int currentSoLuong = 0;
            bool parsed = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out currentSoLuong);

            if (!parsed) currentSoLuong = 1;

            switch (columnName)
            {
                case "Tang":
                    row.Cells["SoLuong"].Value = currentSoLuong + 1;
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

        private void DgvChiTiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewCell cell = dgvChiTiet.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string columnName = dgvChiTiet.Columns[e.ColumnIndex].Name;

            if (columnName == "SoLuong")
            {
                string input = cell.Value?.ToString();
                if (int.TryParse(input, out int soLuong) && soLuong > 0)
                {
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
                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal giaNhap) && giaNhap >= 0) // Allow 0 price? Adjust if needed
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
            if (CbNcc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNcc.Focus();
                return; // Dừng xử lý
            }

            int maTK; // Mã tài khoản người tạo/nhập
            if (CbNgNhap.SelectedValue == null || !int.TryParse(CbNgNhap.SelectedValue.ToString(), out maTK))
            {
                MessageBox.Show("Vui lòng chọn Người nhập hợp lệ.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CbNgNhap.Focus();
                return;
            }

            if (!int.TryParse(CbNcc.SelectedValue.ToString(), out int maNCC))
            {
                MessageBox.Show("Mã Nhà cung cấp không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvChiTiet.Rows.Count == 0 || (dgvChiTiet.Rows.Count == 1 && dgvChiTiet.Rows[0].IsNewRow)) // Kiểm tra cả trường hợp chỉ có dòng mới
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
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow) continue; // Bỏ qua dòng mới (dòng trống ở cuối)

                    bool isMaVLValid = int.TryParse(row.Cells["MaVatLieu"].Value?.ToString(), out int maVL);
                    bool isSoLuongValid = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                    bool isGiaNhapValid = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(),
                                                         System.Globalization.NumberStyles.Any, 
                                                         System.Globalization.CultureInfo.CurrentCulture, 
                                                         out decimal giaNhap);

                    if (!isMaVLValid || !isSoLuongValid || soLuong <= 0 || !isGiaNhapValid || giaNhap < 0)
                    {
                        MessageBox.Show($"Dữ liệu không hợp lệ tại dòng {row.Index + 1}. Vui lòng kiểm tra lại Mã vật liệu, Số lượng (phải > 0) và Giá nhập (phải >= 0).",
                                        "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvChiTiet.CurrentCell = row.Cells["SoLuong"];
                        dgvChiTiet.BeginEdit(true);
                        return; 
                    }

                    chiTietNhap.Add(new DTO_Order
                    {
                        MaVatLieu = maVL,
                        SoLuong = soLuong,
                        GiaNhap = giaNhap
                    });
                }

                var result = await busOrder.NhapHangAsync(ngayNhap, maNCC, maTK, ghiChu, chiTietNhap, maTK);

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
                MessageBox.Show($"Lỗi định dạng dữ liệu: {fEx.Message}. Vui lòng kiểm tra lại các giá trị nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Lỗi không xác định khi nhập hàng trong btnNhapHang_Click: {ex.ToString()}");
                MessageBox.Show($"Đã xảy ra lỗi không mong muốn trong quá trình xử lý: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<DTO_VatLieu> results = await dalOrder.TimKiemVatLieuAsync(searchQuery);
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

            if (results.Any())
            {
                ketqua.Height = Math.Min(results.Count * 35, 210);
                ketqua.BackColor = Color.WhiteSmoke;

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = $"{item.Ten} ({item.MaVatLieu}) - ĐVT: {item.DonViTinh}",
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
                            txtSearch.Text = String.Empty;
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

        private void AddVatLieuToDgvChiTiet(DTO_VatLieu selectedItem)
        {
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["MaVatLieu"].Value != null && (int)row.Cells["MaVatLieu"].Value == selectedItem.MaVatLieu)
                {
                    MessageBox.Show("Vật liệu này đã có trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optional: Highlight the existing row or increase its quantity?
                    // row.Cells["SoLuong"].Value = (int)row.Cells["SoLuong"].Value + 1;
                    return;
                }
            }

            int rowIndex = dgvChiTiet.Rows.Add(
                selectedItem.MaVatLieu,
                selectedItem.Ten,
                selectedItem.DonViTinh,
                1,
                selectedItem.DonGiaNhap
            );

            dgvChiTiet.FirstDisplayedScrollingRowIndex = rowIndex;
            dgvChiTiet.ClearSelection();
            dgvChiTiet.Rows[rowIndex].Selected = true;

            TinhTongSoLuongVaGiaNhap();
            panelImportExcel.Visible = false;
        }

        private void TinhTongSoLuongVaGiaNhap()
        {
            int tongSoLuong = 0;
            decimal tongGiaNhap = 0;

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.IsNewRow) continue;

                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int soLuong);
                decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal giaNhap);

                tongSoLuong += soLuong;
                tongGiaNhap += giaNhap * soLuong;
            }

            lblSl.Text = tongSoLuong.ToString();
            lblTongTien.Text = tongGiaNhap.ToString("N0") + " VND";
        }

        private void Frm_Resize(object sender, EventArgs e)
        {
            ResizeColumns();
            CenterPanelInDataGridView();
        }

        private void ResizeColumns()
        {
            if (dgvChiTiet.Columns.Count == 0 || dgvChiTiet.ClientSize.Width == 0) return;

            int fixedWidth = 0;
            int variableColumnCount = 0;
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
            {
                if (col.Name == "MaVatLieu" || col.Name == "DonViTinh" || col.Name == "SoLuong" || col.Name == "Tang" || col.Name == "Giam" || col.Name == "Xoa")
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    switch (col.Name)
                    {
                        case "MaVatLieu": col.Width = 80; break;
                        case "DonViTinh": col.Width = 80; break;
                        case "SoLuong": col.Width = 70; break;
                        case "Tang": col.Width = 40; break;
                        case "Giam": col.Width = 40; break;
                        case "Xoa": col.Width = 40; break;
                    }
                    fixedWidth += col.Width;
                }
                else if (col.Name == "GiaNhap")
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = 100;
                    fixedWidth += col.Width;
                }
                else if (col.Visible)
                {
                    variableColumnCount++;
                }
            }

            int availableWidth = dgvChiTiet.ClientSize.Width - fixedWidth - SystemInformation.VerticalScrollBarWidth;
            int variableColumnWidth = (variableColumnCount > 0) ? Math.Max(50, availableWidth / variableColumnCount) : 50;
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
            {
                if (col.Visible && col.Name != "MaVatLieu" && col.Name != "DonViTinh" && col.Name != "SoLuong" && col.Name != "GiaNhap" && col.Name != "Tang" && col.Name != "Giam" && col.Name != "Xoa")
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = variableColumnWidth;
                }
            }

            if (dgvChiTiet.Columns.Contains("TenVatLieu"))
            {
                dgvChiTiet.Columns["TenVatLieu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void CenterPanelInDataGridView()
        {
            if (panelImportExcel != null && dgvChiTiet != null)
            {
                int centerX = dgvChiTiet.Left + (dgvChiTiet.Width - panelImportExcel.Width) / 2;
                int centerY = dgvChiTiet.Top + (dgvChiTiet.Height - panelImportExcel.Height) / 2;

                centerX = Math.Max(dgvChiTiet.Left, centerX);
                centerY = Math.Max(dgvChiTiet.Top, centerY);

                panelImportExcel.Left = centerX;
                panelImportExcel.Top = centerY;
            }
        }

        private void ClearForm()
        {
            dtpNgayNhap.Value = DateTime.Now;
            CbNcc.SelectedIndex = -1;
            Tbnote.Text = String.Empty;
            txtSearch.Text = String.Empty;
            dgvChiTiet.Rows.Clear();
            ketqua.Visible = false;
            panelImportExcel.Visible = true;
            TinhTongSoLuongVaGiaNhap();
            CbNcc.Focus();
        }

        private void hopeRoundButton2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Chọn file Excel để nhập hàng";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ImportExcelData(filePath);
                }
            }
        }
        private void ImportExcelData(string filePath)
        {
            var importedItems = new List<DTO_VatLieuImport>();
            var errorRows = new List<int>();


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

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            string maVatLieuStr = worksheet.Cells[row, 1].Value?.ToString().Trim() ?? "";
                            string tenVatLieu = worksheet.Cells[row, 2].Value?.ToString().Trim() ?? "";
                            string donViTinh = worksheet.Cells[row, 3].Value?.ToString().Trim() ?? "";
                            string soLuongStr = worksheet.Cells[row, 4].Value?.ToString().Trim() ?? "";
                            string giaNhapStr = worksheet.Cells[row, 5].Value?.ToString().Trim() ?? "";

                            if (string.IsNullOrEmpty(maVatLieuStr) && string.IsNullOrEmpty(tenVatLieu) && string.IsNullOrEmpty(soLuongStr))
                            {
                                continue;
                            }

                            int maVatLieu;
                            int soLuong;
                            decimal giaNhap;

                            bool hasMaVL = int.TryParse(maVatLieuStr, out maVatLieu);

                            if (hasMaVL)
                            {
                                if (!int.TryParse(soLuongStr, out soLuong) || soLuong <= 0)
                                {
                                    Console.WriteLine($"Lỗi dòng {row}: Số lượng '{soLuongStr}' không hợp lệ.");
                                    errorRows.Add(row);
                                    continue;
                                }
                                if (!decimal.TryParse(giaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out giaNhap) || giaNhap < 0)
                                {
                                    Console.WriteLine($"Lỗi dòng {row}: Giá nhập '{giaNhapStr}' không hợp lệ.");
                                    errorRows.Add(row);
                                    continue;
                                }

                                importedItems.Add(new DTO_VatLieuImport
                                {
                                    RowNumber = row,
                                    MaVatLieu = maVatLieu,
                                    Ten = string.IsNullOrEmpty(tenVatLieu) ? $"Vật liệu {maVatLieu}" : tenVatLieu,
                                    DonViTinh = donViTinh,
                                    SoLuong = soLuong,
                                    GiaNhap = giaNhap
                                });
                            }
                            else if (!string.IsNullOrEmpty(tenVatLieu))
                            {
                                DTO_VatLieu foundVL = busVatLieu.TimVatLieuTheoTenChinhXac(tenVatLieu);

                                if (foundVL == null)
                                {
                                    Console.WriteLine($"Lỗi dòng {row}: Không tìm thấy vật liệu tên '{tenVatLieu}' trong hệ thống.");
                                    errorRows.Add(row);
                                    continue;
                                }

                                if (!int.TryParse(soLuongStr, out soLuong) || soLuong <= 0) { errorRows.Add(row); continue; }
                                if (!decimal.TryParse(giaNhapStr, NumberStyles.Any, CultureInfo.CurrentCulture, out giaNhap) || giaNhap < 0) { errorRows.Add(row); continue; }

                                importedItems.Add(new DTO_VatLieuImport
                                {
                                    RowNumber = row,
                                    MaVatLieu = foundVL.MaVatLieu,
                                    Ten = foundVL.Ten,
                                    DonViTinh = foundVL.DonViTinh,
                                    SoLuong = soLuong,
                                    GiaNhap = giaNhap
                                });
                            }
                            else
                            {
                                Console.WriteLine($"Lỗi dòng {row}: Thiếu Mã Vật Liệu hoặc Tên Vật Liệu.");
                                errorRows.Add(row);
                                continue;
                            }
                        }
                        catch (Exception exRow)
                        {
                            Console.WriteLine($"Lỗi xử lý dòng {row} trong Excel: {exRow.Message}");
                            errorRows.Add(row);
                        }
                    }

                    if (importedItems.Any())
                    {
                        dgvChiTiet.SuspendLayout();
                        int addedCount = 0;
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
                                    exists = true;
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
                        panelImportExcel.Visible = (dgvChiTiet.RowCount == 0);

                        string message = $"Đã import thành công {addedCount} vật liệu.";

                        MessageBox.Show(message, "Import Hoàn Tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (!errorRows.Any())
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu hợp lệ để import trong file Excel.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Import thất bại. Có lỗi tại các dòng Excel sau: {string.Join(", ", errorRows)}.\nXem Console hoặc Log để biết chi tiết.", "Lỗi Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Lỗi truy cập file: {ioEx.Message}\nFile có thể đang được mở bởi ứng dụng khác.", "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      
    }
}