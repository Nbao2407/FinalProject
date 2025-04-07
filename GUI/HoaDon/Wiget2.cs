using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class Wiget2 : UserControl
    {
        // Thuộc tính lưu trữ thông tin
        public int MaVatLieu { get; set; }
        public int MaHoaDon { get; set; }
        public double DonGia { get; set; } // Giá đơn vị để tính lại tổng giá khi thay đổi số lượng

        // Sự kiện để thông báo khi xóa hoặc cập nhật số lượng
        public event EventHandler OnDelete;
        public event EventHandler OnQuantityChanged;

        public Wiget2()
        {
            InitializeComponent();
            // Gán sự kiện cho các nút
            roundedPictureBox1.Click += (s, e) => OnDelete?.Invoke(this, EventArgs.Empty); // Nút xóa
            roundedPictureBox2.Click += DecreaseQuantity; 
            roundedPictureBox3.Click += IncreaseQuantity; 
            TbSL.TextChanged += TbSL_TextChanged; 
        }

        private void DecreaseQuantity(object sender, EventArgs e)
        {
            int soLuong = int.Parse(TbSL.Text);
            if (soLuong > 1)
            {
                soLuong--;
                TbSL.Text = soLuong.ToString();
                UpdateGia(soLuong);
            }
        }

        private void IncreaseQuantity(object sender, EventArgs e)
        {
            int soLuong = int.Parse(TbSL.Text);
            soLuong++;
            TbSL.Text = soLuong.ToString();
            UpdateGia(soLuong);
        }

        private void TbSL_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(TbSL.Text, out int soLuong) && soLuong > 0)
            {
                UpdateGia(soLuong);
                OnQuantityChanged?.Invoke(this, EventArgs.Empty); 
            }
            else
            {
                TbSL.Text = "1";
                UpdateGia(1);
            }
        }
        public void UpdateGia(int soLuong)
        {
            lblGia.Text = (DonGia * soLuong).ToString("#,##0"); 
        }
    }
}
