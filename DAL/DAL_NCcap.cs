using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic; 
namespace DAL
{
    public class DAL_NCcap : DBConnect
    {
        public List<DTO_Ncap> TimKiemNcc(string keyword)
        {
            List<DTO_Ncap> DanhsachNcc = new List<DTO_Ncap>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemNCC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTO_Ncap ncc = new DTO_Ncap
                            {
                              MaNCC = reader.GetInt32(0),
                              TenNCC = reader.GetString(1),
                              SDT = reader.GetString(2),
                              Email = reader.GetString(3),
                              DiaChi = reader.GetString(4),
                              NguoiTao = reader.GetString(5),
                              NgayTao = reader.GetDateTime(6)
                            };
                            DanhsachNcc.Add(ncc);
                        }
                    }
                }
            }

            return DanhsachNcc; 
        }
        public List<DTO_Ncap> GetAllNcc()
        {
            List<DTO_Ncap> DanhsachNcc = new List<DTO_Ncap>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT MaNCC, TenNCC,SDT, Email FROM NCC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DanhsachNcc.Add(new DTO_Ncap
                            {
                                MaNCC = reader.GetInt32(0),
                                TenNCC = reader.GetString(1),
                                SDT = reader.GetString(2),
                                Email = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return DanhsachNcc;
        }
    }
}
