using BUS;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmCustomer : Form
    {
        private BUS_Khach busKhach = new BUS_Khach();
        private QLKH kh = new QLKH();
        public FrmCustomer()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(FrmMaterial_Resize);
        }
        private void FrmKhach_Load(object sender, EventArgs e)
        {
            LoadKhachHang();

        }

        private void LoadKhachHang()
        {
            List<DTO_Khach> danhSachKhach = busKhach.LayDanhSachKhach();
            dataGridView1.DataSource = danhSachKhach;

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            DataGridViewHelper.AddEditDeleteColumns(dataGridView1);
            ResizeColumns();


        }
        private void FrmMaterial_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView1.Width = 1150;
                dataGridView1.Height = 642;
                dataGridView1.Left = (this.ClientSize.Width - 1050) / 2;
                dataGridView1.Top = (this.ClientSize.Height - 642) / 2;

            }
            else
            {
                dataGridView1.Width = this.ClientSize.Width - 40;
                dataGridView1.Height = this.ClientSize.Height - 80;
                dataGridView1.Left = 20;
                dataGridView1.Top = 80;
            }

            ResizeColumns();
        }


        private void ResizeColumns()
        {
            if (dataGridView1.Columns.Count == 0) return;

            int totalWidth = dataGridView1.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView1.Columns.Count - 3;
            int variableColumnWidth = (totalWidth - (3 * fixedColumnWidth)) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name == "Edit" || column.Name == "Delete" || column.Name == "Disable")
                {
                    column.Width = fixedColumnWidth;
                }
                else
                {
                    column.Width = variableColumnWidth;
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (searchQuery.Length > 0)
            {
                // 🛠 Lấy danh sách khách hàng tìm kiếm
                List<DTO_Khach> results = kh.TimKiemKhachHang(searchQuery);
                result.Controls.Clear();
                result.Height = Math.Min(results.Count * 40, 200); 

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = item.Ten, 
                        
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
                        txtSearch.Text = item.Ten;
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
                ? kh.TimKiemKhachHang(searchQuery)
                : kh.GetAllKhach();
        }

    }
}
