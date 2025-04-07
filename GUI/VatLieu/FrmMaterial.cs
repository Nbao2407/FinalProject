using BUS;
using DAL;
using DTO;
using QLVT.Helper;
using QLVT.TaiKhoan;
using QLVT.VatLieu;

namespace QLVT
{
    public partial class FrmMaterial : Form
    {
        private BUS_VatLieu busVatLieu = new BUS_VatLieu();
        private QLVatLieu vl = new QLVatLieu();

        public FrmMaterial()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(FrmMaterial_Resize);
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
        }

        private void FrmMaterial_Load(object sender, EventArgs e)
        {
          
            ResizeColumns();
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
            string searchQuery = txtSearch.Text.Trim();

            if (searchQuery.Length > 0)
            {
                List<DTO_VatLieu> results = vl.SearchProducts(searchQuery);
                result.Controls.Clear();
                result.Height = Math.Min(results.Count * 40, 200); // Giới hạn chiều cao

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = item.TenLoai,
                        AutoSize = false,
                        Width = result.Width,
                        Height = 40,
                        Padding = new Padding(5),
                        Font = new Font("Arial", 11, FontStyle.Bold),
                        BackColor = Color.White,
                        ForeColor = Color.Black,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    lbl.Click += (s, ev) =>
                    {
                        txtSearch.Text = item.TenLoai;
                        result.Visible = false;
                    };

                    result.Controls.Add(lbl);
                }
                result.Visible = true;
            }
            else
            {
                result.Visible = false;
            }

            dataGridView1.DataSource = searchQuery.Length > 0
                ? vl.SearchProducts(searchQuery)
                : vl.LayTatCaVatLieu();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void plotView1_Click(object sender, EventArgs e)
        {
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