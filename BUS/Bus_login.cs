using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Data.SqlClient;

namespace BUS
{
    public class BUS_Login
    {
        private QLTK dangNhapDAL = new QLTK();

        public DTO_User kiemtradangnhap(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return dangNhapDAL.KiemTraDangNhap(username, password, email);
        }
        public bool CheckAccountExists(string username, string email)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM QLTK WHERE (TenDangNhap = @TenDangNhap OR Email = @Email)", conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", string.IsNullOrEmpty(username) ? (object)DBNull.Value : username);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        public bool IsAccountActive(string username, string email)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TrangThai FROM QLTK WHERE (TenDangNhap = @TenDangNhap OR Email = @Email)", conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", string.IsNullOrEmpty(username) ? (object)DBNull.Value : username);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                conn.Open();
                string trangThai = (string)cmd.ExecuteScalar();
                return trangThai == "Hoạt động";
            }
        }
    }
}
