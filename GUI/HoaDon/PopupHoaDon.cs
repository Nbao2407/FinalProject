using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using DAL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using BUS;
using GUI.Helpler;

namespace GUI.HoaDon
{
    public partial class PopupHoaDon : Form
    {
        private FrmHoaDon _parentForm;
        private DTO_HoaDon _hoaDon;
        private BUS_HoaDon hoaDonBLL = new BUS_HoaDon();
        private int? maHoaDon;
        public PopupHoaDon(int? maHoaDon)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.maHoaDon = maHoaDon;
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
        }


        private void PopupHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDonData();
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
            DataGridViewHelper.FillColumnsToDgvWidth(dataGridView);
            DataGridViewHelper.DisableColumnSorting(dataGridView);
        }


        private void LoadHoaDonData()
        {
            try
            {
                if (maHoaDon.HasValue)
                {
                    DTO_HoaDon hoaDon = hoaDonBLL.GetHoaDonById(maHoaDon.Value);
                    if (hoaDon != null)
                    {
                        lblID.Text = hoaDon.MaHoaDon.ToString();
                        TenKH.Text = hoaDon.TenKhachHang ?? "Khách lẻ";
                        Ngay.Text = hoaDon.NgayLap.ToString("dd/MM/yyyy");
                        txtNgTao.Text = hoaDon.NguoiLap;
                        lblTrangthai.Text = hoaDon.TrangThai;
                        lblTongCong.Text = hoaDon.TongTien.ToString("N0");
                    }

                    DataTable dtChiTiet = hoaDonBLL.GetChiTietHoaDon(maHoaDon.Value);
                    dataGridView.DataSource = dtChiTiet;

                    dataGridView.Columns["MaVatLieu"].HeaderText = "Mã Vật Liệu";
                    dataGridView.Columns["Ten"].HeaderText = "Tên Vật Liệu";
                    dataGridView.Columns["SoLuong"].HeaderText = "Số Lượng";
                    dataGridView.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dataGridView.Columns.Contains("MaCTHD"))
                    {
                        dataGridView.Columns.Remove("MaCTHD");
                    }
                    if (dataGridView.Columns.Contains("HinhAnh"))
                    {
                        dataGridView.Columns.Remove("HinhAnh");
                    }
                    if (hoaDon.TrangThai == "Đã hủy" || hoaDon.TrangThai == "Đã thanh toán")
                    {
                        BtnHuy.Enabled = false;
                    }
                    if (!dataGridView.Columns.Contains("ThanhTien"))
                    {
                        DataGridViewTextBoxColumn thanhTienColumn = new DataGridViewTextBoxColumn
                        {
                            Name = "ThanhTien",
                            HeaderText = "Thành Tiền",
                            Width = 120
                        };
                        dataGridView.Columns.Add(thanhTienColumn);
                    }
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);
                        decimal thanhTien = soLuong * donGia;
                        row.Cells["ThanhTien"].Value = thanhTien.ToString("N0");
                    }
                    int tongSoLuong = dtChiTiet.AsEnumerable().Sum(row => row.Field<int>("SoLuong"));
                    lblSoLuong.Text = tongSoLuong.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                using (LydoHuy formLyDo = new LydoHuy())
                {
                    if (formLyDo.ShowDialog() == DialogResult.OK)
                    {
                        string lyDoHuy = formLyDo.LyDoHuy;
                        DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn hủy hóa đơn này?\nLý do: {lyDoHuy}", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            string message = hoaDonBLL.CancelHoaDon((int)maHoaDon, CurrentUser.MaTK, lyDoHuy);
                            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            
                if (lblTrangthai.Text != "Chờ thanh toán")
                {
                    MessageBox.Show("Chỉ có thể chỉnh sửa hóa đơn ở trạng thái 'Chờ thanh toán'!");
                    return;
                }
                int maHoaDon = Convert.ToInt32(lblID.Text);
            Form1 frm1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (frm1 != null)
            {
                EditHoaDon editForm = new EditHoaDon(maHoaDon);
                frm1.OpenChildForm(editForm);
            }
            else
            {
                MessageBox.Show("Không tìm thấy form cha Frm1!");
                return;
            }

            this.Close();
        }

    }
}