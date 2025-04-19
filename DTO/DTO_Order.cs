using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_Order
    {
        public int MaDonNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string TenNCC { get; set; }
        public string NguoiTao { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public int MaVatLieu { get; set; }
        public decimal SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        public int NguoiNhap { get; set; }
        public int MaNCC { get; set; }
        public int MaKho {  get; set; }
        public int NguoiCapNhat { get; set; }   
        public DTO_Order(int MaDonNhap)
        {
            this.MaDonNhap = MaDonNhap;
        }
      

        public DTO_Order()
        { }

    }
}
