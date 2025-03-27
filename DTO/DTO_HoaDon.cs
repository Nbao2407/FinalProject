namespace DTO
{
    public class DTO_HoaDon
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayLap { get; set; }
        private string nguoiLap;
        public string NguoiLap { get => nguoiLap; set => nguoiLap = value; }
        public int MaTK { get; set; }
        public string TenKhachHang { get; set; }
        public int? MaKhachHang { get; set; }
        public string SDTKhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucThanhToan { get; set; }
        public int SoMatHang { get; set; }
        public int TongSoLuong { get; set; }
        public string KhachHangGop => $"{TenKhachHang} ({MaKhachHang})";
    }
}