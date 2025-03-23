namespace DTO
{
    public class DTO_Ncap
    {
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayTao { get; set; }

        public DTO_Ncap(int MaNcc, string TenNcc, string diaChi, string sdt, string email, string nguoiTao, DateTime ngayTao)
        {
            MaNCC = MaNcc;
            TenNCC = TenNcc;
            DiaChi = diaChi;
            SDT = sdt;
            Email = email;
            NguoiTao = nguoiTao;
            NgayTao = ngayTao;
        }

        public DTO_Ncap()
        { }
    }
}