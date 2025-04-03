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
        public int? NguoiTao { get; set; } 
        public string TenNguoiTao { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public string DiaChi { get; set; }

        public DTO_Khach(int maKH, string ten, string gioiTinh, DateTime ngaySinh, string sdt, string email,string trangthai,String D, int nguoiTao, string tenNguoiTao, DateTime ngayTao, string diaChi)
        {
            MaKhachHang = maKH;
            Ten = ten;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            SDT = sdt;
            Email = email;
            TrangThai = trangthai;
            NguoiTao = nguoiTao;
            TenNguoiTao = tenNguoiTao;
            NgayTao = ngayTao;
            DiaChi = diaChi;
        }

        public DTO_Khach()
        {
            NgaySinh = DateTime.Today; 
            NgayTao = DateTime.Today;  
        }
    }
}