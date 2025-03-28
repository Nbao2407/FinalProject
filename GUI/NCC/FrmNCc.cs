﻿using BUS;
using DAL;
using DTO;
using GUI.Helpler;

namespace GUI
{
    public partial class FrmNCc : Form
    {
        private Panel overlayPanel;
        private BUS_Nccap busNcc = new BUS_Nccap();
        private DAL_NCcap ncc = new DAL_NCcap();

        public FrmNCc()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            this.Load += FrmNCc_Load;
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);

        }

        private void Loaddata()
        {
            try
            {
                List<DTO_Ncap> nccList = busNcc.GetAllNcc();
                if (nccList == null || nccList.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu!");
                }

                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("MaNCC", "Mã NCC");
                dataGridView1.Columns.Add("TenNCC", "Tên NCC");
                dataGridView1.Columns.Add("SDT", "SĐT");
                dataGridView1.Columns.Add("Email", "Email");
                dataGridView1.AutoGenerateColumns = false;

                foreach (var ncc in nccList)
                {
                    dataGridView1.Rows.Add(ncc.MaNCC, ncc.TenNCC, ncc.SDT, ncc.Email);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void FrmNCc_Load(object sender, EventArgs e)
        {
            Loaddata();
            dataGridView1.Visible = true;
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
                        txtSearch.Text = item.TenNCC;
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
            ShowPopup();
        }

        private void ShowPopup()
        {
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(50, 0, 0, 0),
                Parent = this,
                Visible = true
            };
            this.Controls.Add(overlay);

            overlay.BringToFront();
            this.Resize += (s, e) =>
            {
                overlay.Size = this.ClientSize;
            };
            using (var popup = new PopNcc())
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
            }
            overlay.Dispose();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}