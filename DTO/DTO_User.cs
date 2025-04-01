using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string Ghichu { get; set; }
        public string NgayTao { get; set; }
        public string TrangThai { get; set; }
    }
    public static class CurrentUser
    {
        public static int MaTK { get; set; }
        public static string TenDangNhap { get; set; }
        public static string ChucVu { get; set; }

        public static void SetUser(int maTK, string tenDangNhap, string chucVu)
        {
            MaTK = maTK;
            TenDangNhap = tenDangNhap;
            ChucVu = chucVu;
        }

        public static void ClearUser()
        {
            MaTK = 0;
            TenDangNhap = null;
            ChucVu = null;
        }
    }
}
