namespace DTO
{
    public class DTO_Tke
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public string KhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucThanhToan { get; set; }
    }

    public class TopProductDTO
    {
        public string TenVatLieu { get; set; }
        public int SoLuongBan { get; set; }
    }

    public class UnderstockDTO
    {
        public string TenVatLieu { get; set; }
        public decimal SoLuongTon { get; set; }
    }
}