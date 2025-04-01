using BUS;
using DAL;
using DTO;
using GUI.Helpler;
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
        private List<DTO_Khach> danhSachKhach; 

        public FrmCustomer()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            this.Load += new EventHandler(FrmCustomer_Load);
            ConfigureDataGridView();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            CbTrangThai.Items.Add("Tất cả");
            CbTrangThai.Items.Add("Hoạt động");
            CbTrangThai.Items.Add("Ngừng sử dụng");
            CbTrangThai.SelectedIndex = 1;
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaKhachHang", DataPropertyName = "MaKhachHang", HeaderText = "Mã Khách Hàng" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", DataPropertyName = "Ten", HeaderText = "Tên" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgaySinh", DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SDT", DataPropertyName = "SDT", HeaderText = "Số Điện Thoại" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng Thái" }); // Thêm cột TrangThai

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            ResizeColumns();
        }

        public void LoadKhachHang()
        {
            danhSachKhach = busKhach.LayDanhSachKhach(); 
            dataGridView1.DataSource = danhSachKhach;
            DataGridViewHelper.FormatDateColumns(dataGridView1, "NgaySinh");
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
                column.Width = variableColumnWidth;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (searchQuery.Length > 0)
            {
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DTO_Khach khach = dataGridView1.Rows[e.RowIndex].DataBoundItem as DTO_Khach;
                if (khach != null)
                {
                    ShowPopupWithData(khach);
                }
            }
        }

        private void ShowPopupWithData(DTO_Khach khach)
        {
            using (var popup = new PopupCmer(this, khach))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
                LoadKhachHang();
            }
        }

        private void ShowPopup()
        {
            
            using (var popup = new AddCustomer(this))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
                LoadKhachHang();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }

        private void CbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = CbTrangThai.SelectedItem.ToString();
            if (filter == "Tất cả")
            {
                dataGridView1.DataSource = danhSachKhach; 
            }
            else
            {
                var filteredList = danhSachKhach.Where(k => k.TrangThai == filter).ToList();
                dataGridView1.DataSource = filteredList;
            }
        }
    }
}