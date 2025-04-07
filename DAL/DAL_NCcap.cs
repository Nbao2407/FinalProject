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
                                NguoiTao = reader.GetInt32(6),
                                NgayTao = reader.GetDateTime(7)
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
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT MaNCC, TenNCC, DiaChi, SDT, Email, NgayTao, NguoiTao FROM NCC", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DanhsachNcc.Add(new DTO_Ncap
                                {
                                    MaNCC = reader.GetInt32(0),
                                    TenNCC = reader.GetString(1),
                                    DiaChi = reader.GetString(2),
                                    SDT = reader.GetString(3),
                                    Email = reader.GetString(4),
                                    NgayTao = reader.GetDateTime(5),
                                    NguoiTao = reader.GetInt32(6),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi trong DAL_NCcap.GetAllNcc: " + ex.Message);
            }
            return DanhsachNcc;
        }

        public string GetCreatorNameById(int creatorId)
        {
            string creatorName = null;

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT TenDangNhap FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động'";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@MaTK", creatorId);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            creatorName = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving creator name: {ex.Message}");
            }

            return creatorName ?? "Unknown";
        }

        public void ThemNCC(string tenNCC, string diaChi, string sdt, string email, int nguoiTao)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NguoiTao", nguoiTao);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void CapNhatNCC(int maNCC, string tenNCC, string diaChi, string sdt, string email, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CapNhatNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void XoaNCC(int maNCC, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public DataTable LayDSNcc()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELect MaNCC,TenNCC from NCC Where TrangThai =N'Hoạt động'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}