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
        public string NCC { get; set; }
        public string TrangThai { get; set; }
        public int MaVatLieu { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }

        public DTO_Order(int MaDonNhap, DateTime ngayNhap, string NCC, string trangthai)
        {
            this.MaDonNhap = MaDonNhap;
            this.NgayNhap = ngayNhap;
            this.NCC = NCC;
            this.TrangThai = trangthai;
        }
        public DTO_Order(int MaDonNhap)
        {
            this.MaDonNhap = MaDonNhap;
           
        }
        public DTO_Order() { }
      
    }
}
