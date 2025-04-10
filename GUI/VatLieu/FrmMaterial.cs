using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.TaiKhoan;
using QLVT.VatLieu;

namespace QLVT
{
    public partial class FrmMaterial : Form
    {
        private BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private QLVatLieu vl = new QLVatLieu();
        private System.Windows.Forms.Timer debounceTimer;
        private List<DTO_VatLieu> danhSach;

        public FrmMaterial()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(FrmMaterial_Resize);
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            SetupDebounceTimer();
            LoadComboBoxes();
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
            dataGridView1.Columns["DonGiaNhap"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["DonGiaBan"].DefaultCellStyle.Format = "N0";

            List<DTO_VatLieu> danhSach = busVatLieu.GetAllVatLieu();
            if (danhSach == null || danhSach.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu vật liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dataGridView1.DataSource = danhSach;
            FrmMaterial_Resize(null, null);
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
            string selectedLoai = CbLoai.DisplayMember?.ToString();
            if (selectedLoai == "Tất cả") { selectedLoai = null; }
            try
            {
                IEnumerable<DTO_VatLieu> suggestionSource = danhSach;

               
                if (!string.IsNullOrEmpty(selectedLoai))
                {
                    suggestionSource = suggestionSource.Where(order =>
                        order.TenLoai != null &&
                        order.TenLoai.Equals(selectedLoai, StringComparison.OrdinalIgnoreCase)
                    );
                }
                List<DTO_VatLieu> suggestionResults = new List<DTO_VatLieu>(); 
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        bool match = false;
                        if (order.MaVatLieu.ToString().Contains(searchQuery)) match = true;
                        if (!match && order.Ten != null && order.Ten .ToLowerInvariant().Contains(searchQuery)) match = true;
                        return match;
                    }).ToList();
                }
                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");
                Func<DTO_VatLieu, string> displayFunc = item => $"{item.MaVatLieu} - {item.Ten ?? "N/A"}";

                Action<DTO_VatLieu> clickAction = selectedItem => {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.MaVatLieu.ToString(); 
                    txtSearch.TextChanged += txtSearch_TextChanged;
                    var itemToShow = new List<DTO_VatLieu> { selectedItem };
                    dataGridView1.DataSource = itemToShow;
                    Console.WriteLine($"DGV DataSource updated to show only item {selectedItem.MaVatLieu}");
                    ResizeColumns();
                };

                SearchHelper.UpdateSearchSuggestions(
                    result,
                    suggestionResults,
                    txtSearch,
                    38,
                    190,
                    displayFunc,
                    clickAction
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm gợi ý: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
            }
        }

        private void LoadComboBoxes()
        {
            BUS_LVL vl = new BUS_LVL();
            CbLoai.DataSource = vl.GetAllLoaiVatLieu();
            CbLoai.SelectedIndex = -1;
         
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
    }
}