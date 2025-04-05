using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using GUI.NCC;

namespace GUI
{
    public partial class FrmNCc : Form
    {
        private Panel overlayPanel;
        private BUS_Ncc busNcc = new BUS_Ncc();
        private DAL_NCcap ncc = new DAL_NCcap();
        private List<DTO_Ncap> danhSach;
        private DTO_Ncap _ncc;
        public FrmNCc()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            this.Load += FrmNCc_Load;
            ConfigureDataGridView();

        }
        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNCC", DataPropertyName = "MaNCC", HeaderText = "Mã NCC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", DataPropertyName = "TenNCC", HeaderText = "Tên NCC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SDT", DataPropertyName = "SDT", HeaderText = "Số Điện Thoại" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email" });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            ResizeColumns();
        }
        public void Loaddata()
        {
            danhSach =busNcc.GetAllNcc();
            dataGridView1.DataSource = danhSach;
        }

        private void FrmNCc_Load(object sender, EventArgs e)
        {
            Loaddata();
        }


        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView1.Width = 1150;
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
                column.Width = fixedColumnWidth;
                column.Width = variableColumnWidth;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            var results = busNcc.TimKiemNcc(searchQuery);

            if (results != null && results.Count > 0)
            {
                result.Controls.Clear();
                result.Height = Math.Min(results.Count * 40, 200);

                foreach (var item in results)
                {
                    var lbl = new Label
                    {
                        Text = item.TenNCC,
                        AutoSize = false,
                        Width = result.Width,
                        Height = 40,
                        Padding = new Padding(5),
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        BackColor = Color.White,
                        ForeColor = Color.Black,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    lbl.Click += (s, ev) =>
                    {
                        txtSearch.Text = $"{item.TenNCC} + {item.MaNCC}";
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

            dataGridView1.DataSource = results;
        }

        private void PopupFrm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DTO_Ncap ncc = dataGridView1.Rows[e.RowIndex].DataBoundItem as DTO_Ncap;
                if (ncc != null)
                {
                    ShowPopup(ncc);
                }
            }
        }

        private void ShowPopup(DTO_Ncap ncc)
        {

            using (var popup = new PopNcc(this,ncc))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
                Loaddata();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var popup = new AddNcc(this))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
            }
        }
    }
}