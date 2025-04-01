using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
namespace DAL
{
    public class DAL_Login
    {
        public DTO_User kiemtradangnhap(string username, string password, string email)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_KiemTraDangNhap", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DTO_User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Email = reader.GetString(2),
                                Role = reader.GetString(3),
                                TrangThai = reader.GetString(4)
                            };
                        }
                        return null;
                    }
                }
            }
        }
    }
}