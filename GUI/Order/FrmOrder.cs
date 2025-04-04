﻿using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using GUI.Order;

namespace GUI
{
    public partial class FrmOrder : Form
    {
        public FrmOrder()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Frm_Resize);
        }

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
        }
        private void LoadData()
        {
            try
            {
                DAL_Order dalOrder = new DAL_Order();
                var orders = dalOrder.GetAllOrder();
                if (orders != null)
                {
                    dataGridView1.DataSource = orders;

                    // Đặt tiêu đề cho các cột cần hiển thị
                    dataGridView1.Columns["MaDonNhap"].HeaderText = "Mã Đơn Nhập";
                    dataGridView1.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                    dataGridView1.Columns["NCC"].HeaderText = "Nhà Cung Cấp";
                    dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";


                    // Định dạng cột ngày
                    dataGridView1.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    // Tự động điều chỉnh kích thước cột
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowPopup();
        }
        private void ShowPopup()
        {

            using (var popup = new PopupOrder())
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using(var popup = new NhapHang()) 
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }
    }
}
