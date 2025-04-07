using System;
using System.Text.RegularExpressions;

namespace QLVT.Helper
{
    public class ValidationHelper2
    {
        public static bool IsValidEmail(string email, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(email))
            {
                errorMessage = "Email không được để trống!";
                return false;
            }
            if (email.Length > 100)
            {
                errorMessage = "Email không được vượt quá 100 ký tự!";
                return false;
            }
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                errorMessage = "Email không đúng định dạng (ví dụ: user@domain.com)!";
                return false;
            }
            return true;
        }

        public static bool IsValidTen(string ten, string fieldName, int maxLength, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(ten))
            {
                errorMessage = $"{fieldName} không được để trống!";
                return false;
            }
            if (ten.Length > maxLength)
            {
                errorMessage = $"{fieldName} không được vượt quá {maxLength} ký tự!";
                return false;
            }
            return true;
        }

        public static bool IsValidSDT(string sdt, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(sdt))
            {
                errorMessage = "Số điện thoại không được để trống!";
                return false;
            }
            if (!Regex.IsMatch(sdt, @"^[0-9]{10,15}$"))
            {
                errorMessage = "Số điện thoại phải là số, từ 10 đến 15 ký tự!";
                return false;
            }
            return true;
        }

        public static bool IsValidDiaChi(string diaChi, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(diaChi) && diaChi.Length > 255)
            {
                errorMessage = "Địa chỉ không được vượt quá 255 ký tự!";
                return false;
            }
            return true;
        }

        public static bool IsValidTenDangNhap(string tenDangNhap, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                errorMessage = "Tên đăng nhập không được để trống!";
                return false;
            }
            if (tenDangNhap.Length > 50)
            {
                errorMessage = "Tên đăng nhập không được vượt quá 50 ký tự!";
                return false;
            }
            if (!Regex.IsMatch(tenDangNhap, @"^[a-zA-Z0-9_]+$"))
            {
                errorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới!";
                return false;
            }
            return true;
        }

        public static bool IsValidMatKhau(string matKhau, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(matKhau))
            {
                errorMessage = "Mật khẩu không được để trống!";
                return false;
            }
            if (matKhau.Length < 6 || matKhau.Length > 255)
            {
                errorMessage = "Mật khẩu phải từ 6 đến 255 ký tự!";
                return false;
            }
            return true;
        }

        public static bool IsValidChucVu(string chucVu, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(chucVu))
            {
                errorMessage = "Chức vụ không được để trống!";
                return false;
            }
            if (chucVu.Length > 50)
            {
                errorMessage = "Chức vụ không được vượt quá 50 ký tự!";
                return false;
            }
            string[] validChucVu = { "Quản lý", "Nhân viên" };
            if (!validChucVu.Contains(chucVu))
            {
                errorMessage = "Chức vụ không hợp lệ! Chỉ chấp nhận: Quản lý, Nhân viên.";
                return false;
            }
            return true;
        }

        public static bool IsValidTrangThai(string trangThai, string tableName, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(trangThai))
            {
                errorMessage = "Trạng thái không được để trống!";
                return false;
            }

            string[] validTrangThai;
            switch (tableName.ToLower())
            {
                case "qltk":
                    validTrangThai = new[] { "Hoạt động", "Bị khóa" };
                    break;
                case "qlkh":
                case "qlvatlieu":
                    validTrangThai = new[] { "Hoạt động", "Ngừng sử dụng" };
                    break;
                case "qlhoadon":
                    validTrangThai = new[] { "Chờ thanh toán", "Đã thanh toán", "Đã hủy" };
                    break;
                default:
                    errorMessage = "Bảng không được hỗ trợ!";
                    return false;
            }

            if (!validTrangThai.Contains(trangThai))
            {
                errorMessage = $"Trạng thái không hợp lệ! Chỉ chấp nhận: {string.Join(", ", validTrangThai)}.";
                return false;
            }
            return true;
        }

        public static bool IsValidSoLuong(int soLuong, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (soLuong <= 0)
            {
                errorMessage = "Số lượng phải lớn hơn 0!";
                return false;
            }
            return true;
        }

        public static bool IsValidGiaTien(decimal giaTien, string fieldName, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (giaTien < 0)
            {
                errorMessage = $"{fieldName} không được nhỏ hơn 0!";
                return false;
            }
            if (giaTien > 9999999999999999.99m) 
            {
                errorMessage = $"{fieldName} vượt quá giá trị tối đa cho phép!";
                return false;
            }
            return true;
        }
    }
}