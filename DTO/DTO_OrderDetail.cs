namespace DTO
{
    public class DTO_OrderDetail
    {
        public int MaDonNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaTK { get; set; }
        public string NguoiTao { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string NguoiCapNhatTen { get; set; } 
        public int NguoiNhap { get; set; }
        public string TenNguoiNhap { get; set; }
        public int MaKho {  get; set; }
        public string TenKho { get; set; }
        public List<DTO_ChiTietDonNhap> ChiTietDonNhap { get; set; }

        public DTO_OrderDetail()
        {
            ChiTietDonNhap = new List<DTO_ChiTietDonNhap>();
        }
    }
}