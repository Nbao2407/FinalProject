using System.Drawing;

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
    public class ChiTietHoaDonTemp
    {
        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public Image HinhAnh { get; set; }
    }
}