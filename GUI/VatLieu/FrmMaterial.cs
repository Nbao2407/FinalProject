using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.TaiKhoan;
using QLVT.VatLieu;
using System.Data;
using System.Runtime.InteropServices;
namespace QLVT
{
    public partial class FrmMaterial : Form
    {
        private BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private QLVatLieu vl = new QLVatLieu();
        private System.Windows.Forms.Timer debounceTimer;
        private List<DTO_VatLieu> danhSach = new List<DTO_VatLieu>();

        public FrmMaterial()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(FrmMaterial_Resize);
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            SetupDebounceTimer();
            LoadComboBoxes();
            if(CurrentUser.ChucVu=="Nhân viên")
                button1.Enabled = false;
            else button1.Enabled = true;

        }

        private void FrmMaterial_Load(object sender, EventArgs e)
        {

            ResizeColumns();
        }

        private void SetupDebounceTimer()
        {
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop();
                PerformSearch();
            };
        }

        public void LoadData()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã Vật Liệu", DataPropertyName = "MaVatLieu" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên", DataPropertyName = "Ten" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenLoai", HeaderText = "Loại Vật Liệu", DataPropertyName = "TenLoai" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaNhap", HeaderText = "Đơn Giá Nhập", DataPropertyName = "DonGiaNhap" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaBan", HeaderText = "Đơn Giá Bán", DataPropertyName = "DonGiaBan" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "Đơn Vị Tính", DataPropertyName = "DonViTinh" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", DataPropertyName = "SoLuong" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKho", HeaderText = "Kho", DataPropertyName = "TenKho" });
            dataGridView1.Columns["DonGiaNhap"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["DonGiaBan"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns[ "SoLuong"].DefaultCellStyle.Format = "N0";
            List<DTO_VatLieu> tempData = null;
            try
            {
                tempData = busVatLieu.GetAllVatLieu();

                if (tempData == null)
                {
                    MessageBox.Show("Không tải được danh sách vật liệu (kết quả null từ lớp BUS/DAL).", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    danhSach = new List<DTO_VatLieu>();
                }
                else
                {
                    danhSach = tempData.AsEnumerable().Reverse().ToList();
                    if (danhSach.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu vật liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nghiêm trọng khi tải hoặc xử lý dữ liệu vật liệu: {ex.Message}", "Lỗi Tải Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                danhSach = new List<DTO_VatLieu>();
            }
            finally
            {
                dataGridView1.DataSource = danhSach;
            }
            ResizeColumns();
        }

        private void FrmMaterial_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView1.Width = 1200;
                dataGridView1.Height = 642;
                dataGridView1.Left = (this.ClientSize.Width) / 2;
                dataGridView1.Top = (this.ClientSize.Height - 642) / 2;
            }
            else
            {
                dataGridView1.Width = this.ClientSize.Width;
                dataGridView1.Height = this.ClientSize.Height - 80;
                dataGridView1.Left = 25;
                dataGridView1.Top = 80;
            }

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            if (dataGridView1.Columns.Count == 0) return;

            int totalWidth = dataGridView1.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView1.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Width = variableColumnWidth;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.CurrentRow != null)
            //{
            //    DataGridViewRow row = dataGridView1.CurrentRow;

            //    string ten = row.Cells["Ten"].Value.ToString();
            //    int loai = Convert.ToInt32(row.Cells["Loai"].Value);
            //    decimal donGiaNhap = Convert.ToDecimal(row.Cells["DonGiaNhap"].Value);
            //    decimal donGiaBan = Convert.ToDecimal(row.Cells["DonGiaBan"].Value);
            //    string donViTinh = row.Cells["DonViTinh"].Value?.ToString();
            //    int maKho = Convert.ToInt32(row.Cells["MaKho"].Value);
            //    string ghiChu = row.Cells["GhiChu"].Value?.ToString();
            //    byte[] hinhAnh = row.Cells["HinhAnh"].Value != DBNull.Value ? (byte[])row.Cells["HinhAnh"].Value : null;

            //    FrmVatLieu frmVatLieu = new FrmVatLieu(this, ten, loai, donGiaNhap, donGiaBan, donViTinh, maKho, ghiChu, hinhAnh);
            //    frmVatLieu.ShowDialog();
            //}
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();

            string selectedLoaiText = null; 
            int? selectedMaLoaiValue = null;
            if (CbLoai.SelectedIndex > 0)
            {
                selectedLoaiText = CbLoai.Text;
                if (CbLoai.SelectedValue != null && CbLoai.SelectedValue != DBNull.Value)
                {
                    if (int.TryParse(CbLoai.SelectedValue.ToString(), out int maLoai) && maLoai != -1) 
                    {
                        selectedMaLoaiValue = maLoai;
                    }
                }
            }

            string selectedTenKho = null;
            if (CbKho.SelectedIndex > 0)
            {
                selectedTenKho = CbKho.Text;
                Console.WriteLine($"Filter TenKho: '{selectedTenKho}'");
            }
            Console.WriteLine($"Starting search. danhSach count: {danhSach.Count}");

            try
            {
                if (danhSach == null)
                {
                    Console.WriteLine("Lỗi");
                    return;
                }

                IEnumerable<DTO_VatLieu> suggestionSource = danhSach;

          
                 if (!string.IsNullOrEmpty(selectedLoaiText) && CbLoai.SelectedIndex > 0)
                {
                    suggestionSource = suggestionSource.Where(vl =>
                        vl.TenLoai != null &&
                        vl.TenLoai.Equals(selectedLoaiText, StringComparison.OrdinalIgnoreCase)
                    );
                }
                if (!string.IsNullOrEmpty(selectedTenKho)) 
                {
                    suggestionSource = suggestionSource.Where(vl =>
                        vl.TenKho != null && 
                        vl.TenKho.Equals(selectedTenKho, StringComparison.OrdinalIgnoreCase) 
                    );
                    Console.WriteLine($"After Kho (TenKho) filter. Count: {suggestionSource.Count()}"); 
                }

                List<DTO_VatLieu> suggestionResults;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(vl =>
                    {
                        // ... logic tìm theo mã hoặc tên ...
                        string maVatLieuStr = vl.MaVatLieu.ToString().ToLowerInvariant();
                        string tenVatLieu = vl.Ten;
                        bool maKhop = maVatLieuStr.Contains(searchQuery);
                        bool tenKhop = !maKhop && tenVatLieu != null &&
                                       tenVatLieu.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase);
                        return maKhop || tenKhop;
                    }).ToList();
                }
                else
                {
                    suggestionResults = suggestionSource.ToList();
                }

                // Gán DataSource
                dataGridView1.DataSource = suggestionResults;

                // Cập nhật giao diện khác (Resize, Suggestion List...)
                if (suggestionResults.Any())
                {
                    ResizeColumns();
                }


                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");
                if (result != null) // Kiểm tra result trước khi dùng
                {
                    Func<DTO_VatLieu, string> displayFunc = item => $"{item.MaVatLieu} - {item.Ten ?? "N/A"}";
                    Action<DTO_VatLieu> clickAction = selectedItem =>
                    {

                        txtSearch.TextChanged -= txtSearch_TextChanged;

                        txtSearch.Text = selectedItem.Ten;

                        txtSearch.TextChanged += txtSearch_TextChanged;

                    };

                        SearchHelper.UpdateSearchSuggestions(result, suggestionResults, txtSearch, 38, 190, displayFunc, clickAction);

                    result.Visible = suggestionResults.Any() && txtSearch.Focused && !string.IsNullOrEmpty(txtSearch.Text);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm gợi ý: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                // Không nên gán null ở đây nếu muốn giữ cấu trúc cột
                // dataGridView1.DataSource = null;
                // Thay vào đó, có thể xóa các dòng nếu cần:
                // if (dataGridView1.DataSource is List<DTO_VatLieu> currentList) currentList.Clear();
                // hoặc gán list rỗng:
                dataGridView1.DataSource = new List<DTO_VatLieu>();
            }
        }
        private void CbLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
        private void LoadComboBoxes()
        {
            try
            {
                DataTable dt = busVatLieu.LayDanhSachLoaiVatLieu();
                DataTable kho = busVatLieu.LayDanhSachKho();
                if (dt == null || kho==null)
                {
                    MessageBox.Show("Không thể lấy danh sách loại vật liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dt = new DataTable();

                }

                DataRow allRow = dt.NewRow();
                DataRow dataRow = kho.NewRow();
                dataRow["TenKho"] = "Tất cả";
                dataRow["MaKho"] = -1;
                allRow["TenLoai"] = "Tất cả";
                allRow["MaLoaiVatLieu"] = -1;
                kho.Rows.InsertAt(dataRow, 0);
                dt.Rows.InsertAt(allRow, 0);
                CbKho.DataSource = kho;
                CbKho.DisplayMember = "TenKho";
                CbKho.ValueMember = "MaKho";
                CbLoai.DataSource = dt;
                CbLoai.DisplayMember = "TenLoai";
                CbLoai.ValueMember = "MaLoaiVatLieu";

                CbLoai.SelectedIndex = 0;
                CbKho.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại vật liệu: {ex.Message}", "Lỗi ComboBox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CbLoai.DataSource = null;
                CbLoai.Items.Clear();
                CbLoai.Items.Add("Lỗi tải dữ liệu");
            }
        }
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int maVatLieu = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MaVatLieu"].Value);
                PopupMaterial popup = new PopupMaterial(this, maVatLieu);
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (var add = new AddMaterial(this))
            {
                add.Deactivate += (s, e) => add.TopMost = true;
                add.StartPosition = FormStartPosition.CenterParent;
                add.ShowDialog();
            }
        }

        private void CbKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
    }
}