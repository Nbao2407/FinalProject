using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DonNhapSearchResult
    {
        public int MaDonNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string TrangThai { get; set; }
        public DTO_DonNhapSearchResult(int maDonNhap, string tenNhaCungCap, DateTime ngayNhap, string trangThai)
        {
            MaDonNhap = maDonNhap;
            TenNhaCungCap = tenNhaCungCap;
            NgayNhap = ngayNhap;
            TrangThai = trangThai;
        }
        public DTO_DonNhapSearchResult() { }

    }
}
