namespace DTO
{
    public class DTO_LVL
    {
        public int MaLoaiVatLieu { get; set; }
        public string TenLoai { get; set; }
        public string TrangThai { get; set; }

        public DTO_LVL()
        { }

        public DTO_LVL(int maLoai, string tenLoai, string trangThai)
        {
            MaLoaiVatLieu = maLoai;
            TenLoai = tenLoai;
            TrangThai = trangThai;
        }
    }
}