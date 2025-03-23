using BUS;
using DAL;
using DTO;
using GUI.Helpler;

namespace GUI
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
        }

        private void FrmMaterial_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
        }

        private void LoadData()
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
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            int tongSl = DataGridViewHelper.TinhTongSoLuongChon(dataGridView1, "SoLuong");
            Tong.Text = $"Tổng: {tongSl}";
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
            //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Đảm bảo không click vào tiêu đề
            //    {
            //        string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            //        string id = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();

            //        if (columnName == "Edit")
            //        {
            //            MessageBox.Show($"Chỉnh sửa vật liệu ID: {id}");
            //            // TODO: Viết code để mở form chỉnh sửa vật liệu
            //        }
            //        else if (columnName == "Delete")
            //        {
            //            DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa vật liệu ID: {id}?",
            //                                                  "Xác nhận xóa",
            //                                                  MessageBoxButtons.YesNo,
            //                                                  MessageBoxIcon.Warning);
            //            if (result == DialogResult.Yes)
            //            {
            //                MessageBox.Show("xóa");
            //                LoadData();
            //            }
            //        }
            //        else
            //        {
            //            // Nếu click vào chỗ khác, không làm gì cả
            //            dataGridView1.ClearSelection();
            //        }
            //    }
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
    }
}