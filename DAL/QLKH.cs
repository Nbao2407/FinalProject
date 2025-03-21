using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace DAL
{
    public class QLKH : DBConnect
    {
        public List<DTO_Khach> TimKiemKhachHang(string keyword)
        {
            List<DTO_Khach> danhSachKH = new List<DTO_Khach>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemKH", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachKH.Add(new DTO_Khach
                            {
                                MaKhachHang = reader.GetInt32(0),
                                Ten = reader.GetString(1),
                                GioiTinh = reader.GetString(2),
                                NgaySinh = reader.GetDateTime(3),
                                SDT = reader.GetString(4),
                                Email = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSachKH;
        }
        public List<DTO_Khach> GetAllKhach()
        {
            List<DTO_Khach> danhSachKhach = new List<DTO_Khach>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT MaKhachHang, Ten, GioiTinh, NgaySinh, SDT, Email FROM QLKH", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachKhach.Add(new DTO_Khach
                            {
                                MaKhachHang = reader.GetInt32(0),
                                Ten = reader.GetString(1),
                                GioiTinh = reader.GetString(2),
                                NgaySinh = reader.GetDateTime(3), 
                                SDT = reader.GetString(4),
                                Email = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSachKhach;
        }

    }
}
