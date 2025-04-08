using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DAL
{
    public class QLTK : DBConnect
    {
        public List<DTO_TK> TimKiemTk(string keyword)
        {
            List<DTO_TK> danhSachTk = new List<DTO_TK>();

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemQLTK", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Keyword", keyword ?? (object)DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachTk.Add(new DTO_TK(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetDateTime(6)
                                ));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error occurred while searching accounts: {ex.Message}", ex);
            }

            return danhSachTk;
        }

        public List<DTO_TK> GetAllTk()
        {
            List<DTO_TK> danhSachTk = new List<DTO_TK>();

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT maTK, tenDangNhap, email, sdt, ChucVu,trangThai, ngayTao FROM QLTK";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachTk.Add(new DTO_TK(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetDateTime(6)
                                ));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error occurred while retrieving accounts: {ex.Message}", ex);
            }

            return danhSachTk;
        }
        public DTO_User KiemTraDangNhap(string username, string password, string email)
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
                                Id = reader.GetInt32(reader.GetOrdinal("MaTK")),
                                Username = reader.GetString(reader.GetOrdinal("TenDangNhap")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Role = reader.GetString(reader.GetOrdinal("ChucVu")),
                                TrangThai = reader.GetString(reader.GetOrdinal("TrangThai"))
                            };
                        }
                        return null;
                    }
                }
            }
        }
        public DataTable GetTkByID(int maTk)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT maTK, TenDangNhap,MatKhau, Email, ChucVu,SDT,GHICHU,Avatar,NgayTao,NguoiTao, TrangThai FROM QLTK WHERE maTk = @maTk", conn);
                cmd.Parameters.AddWithValue("@maTK", maTk);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
        public bool UpdateAccount(int maTK, string tenDangNhap, string email, string sdt,  byte[] avatar, string ghichu,int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CapNhatThongTinTK", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenDangNhap", SqlDbType.NVarChar, 50) { Value = tenDangNhap });
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = email });
                    cmd.Parameters.Add(new SqlParameter("@SDT", SqlDbType.NVarChar, 15) { Value = sdt ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Avatar", SqlDbType.VarBinary, -1) { Value = avatar ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Ghichu", SqlDbType.NVarChar, 255) { Value = ghichu ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@NguoiCapNhat", SqlDbType.Int) { Value = nguoiCapNhat });

                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Lỗi khi cập nhật tài khoản: " + ex.Message);
                }
            }
        }
        public byte[] GetAvatar(int maTK)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Avatar FROM QLTK WHERE MaTK = @MaTK", conn);
                cmd.Parameters.AddWithValue("@MaTK", maTK);

                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? null : (byte[])result;
            }
        }
        public void ThemTaiKhoan(DTO_User user, int nguoiTao)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemTaiKhoan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenDangNhap", user.Username);
                cmd.Parameters.AddWithValue("@MatKhau", user.Password); 
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@SDT", user.Sdt);
                cmd.Parameters.AddWithValue("@ChucVu", user.Role);
                cmd.Parameters.AddWithValue("@NguoiTao", nguoiTao);
                cmd.Parameters.AddWithValue("@GHICHU", user.Ghichu);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool SuaTaiKhoan(DTO_TK tk, string matKhauMoi, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SuaTaiKhoan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaTK", tk.maTK);
                cmd.Parameters.AddWithValue("@TenDangNhap", tk.tenDangNhap);
                cmd.Parameters.AddWithValue("@Email", tk.email);
                cmd.Parameters.AddWithValue("@SDT", tk.sdt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ChucVu", tk.quyen);
                cmd.Parameters.AddWithValue("@TrangThai", tk.trangThai);
                cmd.Parameters.AddWithValue("@GhiChu", tk.ghichu ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MatKhau", string.IsNullOrEmpty(matKhauMoi) ? (object)DBNull.Value : matKhauMoi);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
        }

        public void CapNhatMatKhau(int maTK, string matKhauMoi)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CapNhatMatKhau", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaTK", maTK);
                cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void XoaTaiKhoan(int maTK, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaTaiKhoan", conn); 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaTK", maTK);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool Disable(int MaTk, int nguoiCapNhat)
        {
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE dbo.QLTK\r\nSET TrangThai =  N'Bị khóa'\r\nWHERE MaTK= @MaTK;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MaTk", MaTk);
                        cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                    throw new Exception("Lỗi khi disable loại vật liệu: " + ex.Message);
            }
          
        }

        public bool Active(int maTk, int nguoiCapNhat)
        {
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE QLTK SET TrangThai = N'Hoạt động' WHERE MaTk = @MaTk";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MaTk", maTk);
                        cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                    throw new Exception("Lỗi khi disable Tài khoản " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi hệ thống: " + ex.Message);
            }
        }
        public DataTable LayDSNgNhap()
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT MaTK, TenDangNhap FROM QLTK WHERE TrangThai = N'Hoạt động'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
