using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using GUI.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace GUI
{
    public partial class FrmType : Form
    {
        private BUS_LVL lVL = new BUS_LVL();
        private DAL_LVL dal = new DAL_LVL();
        public FrmType()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            txtSearch.KeyDown += txtSearch_KeyDown;

        }
        private void LoadData()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaLoaiVatLieu", HeaderText = "Mã Loại Vật Liệu", DataPropertyName = "MaLoaiVatLieu" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenLoai", HeaderText = "Loại Vật Liệu", DataPropertyName = "TenLoai" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Trangthai", HeaderText = "Trạng Thái", DataPropertyName = "Trangthai" });

            List<DTO_LVL> danhSach = dal.LayTatCaLVL();
            if (danhSach == null || danhSach.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu vật liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dataGridView.DataSource = danhSach;
        }
        private void FrmType_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
            LoadData();
        }
        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView.Width = 1150;
                dataGridView.Height = 642;
                dataGridView.Left = (this.ClientSize.Width) / 2;
                dataGridView.Top = (this.ClientSize.Height - 642) / 2;

            }
            else
            {
                dataGridView.Width = this.ClientSize.Width;
                dataGridView.Height = this.ClientSize.Height - 80;
                dataGridView.Left = 25;
                dataGridView.Top = 80;
            }

            ResizeColumns();
        }
        private void ResizeColumns()
        {
            if (dataGridView.Columns.Count == 0) return;

            int totalWidth = dataGridView.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Width = variableColumnWidth;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (searchQuery.Length > 0)
            {
                List<DTO_LVL> results = dal.SearchProductTypes(searchQuery);
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

            // Load dữ liệu lên DataGridView
            dataGridView.DataSource = searchQuery.Length > 0
                ? dal.SearchProductTypes(searchQuery)
                : dal.LayTatCaLVL();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtSearch.Focus();

            }
        }

        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var Pop = new PopupType())
            {
                Pop.Deactivate += (s, e) => Pop.TopMost = true;
                Pop.StartPosition = FormStartPosition.CenterParent;
                Pop.ShowDialog();
            }
        }
        private void ShowPopup()
        {

            using (var popup = new AddType())
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }
    }
}
