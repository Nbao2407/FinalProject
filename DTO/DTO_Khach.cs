namespace DTO
{
    public class DTO_Khach
    {
        public int MaKhachHang { get; set; }
        public string Ten { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }

        public DTO_Khach(int maKH, string ten, string gioiTinh, DateTime ngaySinh, string sdt, string email)
        {
            MaKhachHang = maKH;
            Ten = ten;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            SDT = sdt;
            Email = email;
        }

        public DTO_Khach()
        { }
    }
}