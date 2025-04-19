using System;
using System.Linq;
using DTO;
using DAL;
using System.Data;
using QLVT.Helper;


namespace BUS
{
    public class BUS_TK
    {
        private QLTK DAL_TK = new QLTK();
        public List<DTO_TK> TimKiemTk(string keyword)
        {
            return DAL_TK.TimKiemTk(keyword);
        }
        public List<DTO_TK> GetAllTk()
        {
            return DAL_TK.GetAllTk();
        }
        public DataTable GetTkByMa(int id)
        {
            return DAL_TK.GetTkByID(id);
        }
        public byte[] GetAvatar(int maTK)
        {
            return DAL_TK.GetAvatar(maTK);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public DataTable LayDSNgNhapNv()
        {
            return DAL_TK.LayDSNgNhapNv();
        }
        private bool IsNumeric(string value)
        {
            return value.All(char.IsDigit);
        }
        public bool UpdateAccount(int maTK, string tenDangNhap, string email, string sdt, byte[] avatar, int nguoiCapNhat)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || tenDangNhap.Length > 50)
                throw new ArgumentException("Tên đăng nhập không hợp lệ!");
            if (string.IsNullOrWhiteSpace(email) || email.Length > 100 || !IsValidEmail(email))
                throw new ArgumentException("Email không hợp lệ!");
            if (!string.IsNullOrEmpty(sdt) && (sdt.Length < 10 || sdt.Length > 15 || !IsNumeric(sdt)))
                throw new ArgumentException("Số điện thoại không hợp lệ!");
            if (avatar != null && avatar.Length > 10485760) 
                throw new ArgumentException("Kích thước avatar vượt quá 10MB!");

            return DAL_TK.UpdateAccount(maTK, tenDangNhap, email, sdt, avatar,nguoiCapNhat);
        }
        public void ThemTaiKhoan(DTO_User user, int nguoiTao)
        {
            string chucVuNguoiTao = CurrentUser.ChucVu;
            if (chucVuNguoiTao == "Quản lý" && user.Role != "Nhân viên")
            {
                throw new Exception("Quản lý chỉ có thể tạo tài khoản nhân viên!");
            }
            DAL_TK.ThemTaiKhoan(user, nguoiTao);
        }

        public bool SuaTaiKhoan(DTO_TK tk, string? matKhauMoi, int nguoiCapNhat)
        {
            return DAL_TK.SuaTaiKhoan(tk, matKhauMoi, nguoiCapNhat);
        }


        public void CapNhatMatKhau(int maTK, string matKhauMoi)
        {
            string errorMessage;
            if (!ValidationHelper2.IsValidMatKhau(matKhauMoi, out errorMessage))
            {
                throw new Exception(errorMessage);
            }
            DAL_TK.CapNhatMatKhau(maTK, matKhauMoi);
        }

        public void XoaTaiKhoan(int maTK, int nguoiCapNhat)
        {
            if (maTK == nguoiCapNhat)
            {
                throw new Exception("Không thể xóa tài khoản của chính bạn!");
            }
            DAL_TK.XoaTaiKhoan(maTK, nguoiCapNhat);
        }
        public DataTable LayDSNgNhap()
        {
            return DAL_TK.LayDSNgNhap();
        }
    }
}
