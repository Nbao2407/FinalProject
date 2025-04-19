using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_ChiTietXuatGrid
    {
        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia; 
        public int MaKhoNguon { get; set; }
        public string TenKhoNguon { get; set; }

    }
}
