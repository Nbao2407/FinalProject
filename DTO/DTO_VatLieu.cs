namespace DTO
{
    public class DTO_VatLieu
    {
        public int MaVatLieu { get; set; }
        public string Ten { get; set; }
        public int Loai { get; set; }
        public string TenLoai { get; set; }
        public decimal DonGiaNhap { get; set; }
        public decimal DonGiaBan { get; set; }
        public string DonViTinh { get; set; }
        public string TrangThai { get; set; }
        public Decimal SoLuong { get; set; }
        public string TenKho { get; set; }
        public decimal TongSoLuongTon { get; set; }
        public int MaKho { get; set; }
        public int MaLoaiVatLieu { get; set; }
        public byte[] HinhAnhBytes { get; set; }
        public DateTime NgayTao { get; set; }
        public int? NguoiTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? NguoiCapNhat { get; set; }
        public string TenNguoiTao { get; set; } 
        public string TenNguoiCapNhat { get; set; }
        public DTO_VatLieu()
        { }

        public DTO_VatLieu(int maVatLieu, string ten, string tenLoai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, Decimal soLuong,string tenKho)
        {
            MaVatLieu = maVatLieu;
            Ten = ten;
            TenLoai = tenLoai;
            DonGiaNhap = donGiaNhap;
            DonGiaBan = donGiaBan;
            DonViTinh = donViTinh;
            SoLuong = soLuong;
            TenKho = tenKho;
        }
    }
}