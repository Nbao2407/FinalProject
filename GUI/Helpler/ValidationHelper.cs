using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace GUI.Helper;

 public class ValidationHelper
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
            errorMessage = "Email vượt quá 100 ký tự!";
            return false;
        }
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorMessage = "Email không đúng định dạng!";
            return false;
        }
        return true;
    }
    public static bool IsValidTenNCC(string tenNCC, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (string.IsNullOrEmpty(tenNCC))
        {
            errorMessage = "Tên NCC không được để trống!";
            return false;
        }
        if (tenNCC.Length > 100)
        {
            errorMessage = "Tên NCC tối đa 100 ký tự!";
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
            errorMessage = "Địa chỉ tối đa 255 ký tự!";
            return false;
        }
        return true;
    }
}
