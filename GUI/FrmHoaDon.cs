using BUS;
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
using static IronPython.Modules._ast;

namespace GUI
{
    public partial class FrmHoaDon : Form
    {
        private BUS_HoaDon busHoaDon = new BUS_HoaDon();
        public FrmHoaDon()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
          


        }

        private void LoadData()
        {
            try
            {
            

                List<DTO_HoaDon> danhSach = busHoaDon.LayTatCaHoaDon();
                if (danhSach == null || danhSach.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (dataGridView1 != null)
                        dataGridView1.DataSource = null;
                    return;
                }
                var data = danhSach.Select(hd => new
                {
                    hd.MaHoaDon,
                    hd.NgayLap,
                    hd.NguoiLap,
                    KhachHang = hd.TenKhachHang,
                    hd.TongTien,
                    hd.TrangThai,
                    hd.HinhThucThanhToan
                }).ToList();

                dataGridView1.DataSource = data;

                if (dataGridView1.Columns.Contains("MaHoaDon"))
                    dataGridView1.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";

                if (dataGridView1.Columns.Contains("NgayLap"))
                    dataGridView1.Columns["NgayLap"].HeaderText = "Ngày Lập";

                if (dataGridView1.Columns.Contains("NguoiLap"))
                    dataGridView1.Columns["NguoiLap"].HeaderText = "Người Lập";

                if (dataGridView1.Columns.Contains("KhachHang"))
                    dataGridView1.Columns["KhachHang"].HeaderText = "Khách Hàng";

                if (dataGridView1.Columns.Contains("TongTien"))
                {
                    dataGridView1.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dataGridView1.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["TongTien"].DefaultCellStyle.ForeColor = Color.Blue;
                }

                if (dataGridView1.Columns.Contains("TrangThai"))
                    dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";

                if (dataGridView1.Columns.Contains("HinhThucThanhToan"))
                    dataGridView1.Columns["HinhThucThanhToan"].HeaderText = "Hình Thức TT";


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //private void dgv_SelectionChanged(object sender, EventArgs e)
        //{
        //    int tongSl = DataGridViewHelper.TinhTongSoLuongChon(dataGridView1, "SoLuong");
        //    Tong.Text = $"Tổng: {tongSl}";
        //}

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

        }
        private void OpenAddHoaDon()
        {
            this.Controls.Clear();
            AddHoaDon addHoaDon = new AddHoaDon
            {
                TopLevel = false,
                Dock = DockStyle.Fill 
            };
            this.Controls.Add(addHoaDon);
            addHoaDon.Show();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenAddHoaDon();

        }
            //private void txtSearch_TextChanged(object sender, EventArgs e)
            //{
            //    string searchQuery = txtSearch.Text.Trim();

            //    if (searchQuery.Length > 0)
            //    {
            //        List<DTO_VatLieu> results = busHoaDon.Sea(searchQuery);
            //        result.Controls.Clear();
            //        result.Height = Math.Min(results.Count * 40, 200); // Giới hạn chiều cao

            //        foreach (var item in results)
            //        {
            //            Label lbl = new Label
            //            {
            //                Text = item.TenLoai,
            //                AutoSize = false,
            //                Width = result.Width,
            //                Height = 40,
            //                Padding = new Padding(5),
            //                Font = new Font("Arial", 11, FontStyle.Bold),
            //                BackColor = Color.White,
            //                ForeColor = Color.Black,
            //                BorderStyle = BorderStyle.FixedSingle
            //            };

            //            lbl.Click += (s, ev) =>
            //            {
            //                txtSearch.Text = item.TenLoai;
            //                result.Visible = false;
            //            };

            //            result.Controls.Add(lbl);
            //        }
            //        result.Visible = true;
            //    }
            //    else
            //    {
            //        result.Visible = false;
            //    }

            //    // Load dữ liệu lên DataGridView
            //    dataGridView1.DataSource = searchQuery.Length > 0
            //        ? vl.SearchProducts(searchQuery)
            //        : vl.LayTatCaVatLieu();
            //}
        }
}
