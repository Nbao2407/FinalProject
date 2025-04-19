using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_ChiTietDonNhap
    {
        public int MaDonNhap { get; set; }

        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; }   
        public string DonViTinh { get; set; }      
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }      
        public int MaKho {  get; set; }
        public string TenKho {  get ; set;}
        public decimal ThanhTien => SoLuong * GiaNhap; 

        public DTO_ChiTietDonNhap() { }
    }
}
