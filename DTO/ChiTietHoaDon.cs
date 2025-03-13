using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
        public class ChiTietHoaDon
        {
            public int MaCTHD { get; set; }
            public int MaHoaDon { get; set; }
            public int MaVatLieu { get; set; }
            public string TenVatLieu { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien { get; set; }
            public string DonViTinh { get; set; }
        }
}
