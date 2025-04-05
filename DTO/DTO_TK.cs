using System;

namespace DTO
{
    public class DTO_TK
    {
        public int maTK { get; set; }
        public string tenDangNhap { get; set; }
        public string matKhau { get; set; }
        public string quyen { get; set; }
        public string email { get; set; }
        public string sdt { get; set; }
        public string diaChi { get; set; }
        public string ghichu { get; set; }
        public string trangThai { get; set; }
        public DateTime ngayTao { get; set; }
        public DTO_TK(int Matk, string tenDangNhap, string matKhau, string quyen, string email, string sdt, string diaChi, string tranthai, string ghichu, DateTime ngayTao)
        {
            this.maTK = Matk;
            this.tenDangNhap = tenDangNhap;
            this.matKhau = matKhau;
            this.quyen = quyen;
            this.email = email;
            this.sdt = sdt;
            this.trangThai = tranthai;
            this.diaChi = diaChi;
            this.ghichu = ghichu;
            this.ngayTao = ngayTao;
        }
        public DTO_TK(int Matk, string tenDangNhap, string email, string sdt, string quyen, string tranthai, DateTime ngayTao)
        {
            this.maTK = Matk;
            this.tenDangNhap = tenDangNhap;
            this.email = email;
            this.sdt = sdt;
            this.sdt = sdt;
            this.quyen = quyen;
            this.trangThai = tranthai;
            this.ngayTao = ngayTao;
        }
        public DTO_TK(int Matk, string tenDangNhap, string quyen, string email, string sdt, string diaChi, string ghichu, DateTime ngayTao)
        {
            this.maTK = Matk;
            this.tenDangNhap = tenDangNhap;
            this.quyen = quyen;
            this.email = email;
            this.sdt = sdt;
            this.diaChi = diaChi;
            this.ghichu = ghichu;
            this.ngayTao = ngayTao;
        }
        public DTO_TK()
        { }

       
    }
}