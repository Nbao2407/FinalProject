﻿using DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class QLKH : DBConnect
    {
        private string GetTenDangNhap(string maTK)
        {
            if (!int.TryParse(maTK, out int maTKInt))
            {
                return null;
            }
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "SELECT TenDangNhap FROM QLTK WHERE MaTK = @MaTK";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTKInt);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }

    

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
                                NgaySinh = reader.IsDBNull(3) ? DateTime.Today : reader.GetDateTime(3),
                                SDT = reader.GetString(4),
                                Email = reader.GetString(5),
                                NguoiTao = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                TenNguoiTao = reader.IsDBNull(7) ? null : reader.GetString(7),
                                NgayTao = reader.IsDBNull(8) ? DateTime.Today : reader.GetDateTime(8)
                            });
                        }
                    }
                }
            }
            return danhSachKH;
        }
        public DTO_Khach GetCustomerById(int maKhachHang) 
        {
            
            return new DTO_Khach();
        }
        public void SuaKhachHang(DTO_Khach khach)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "UPDATE QLKH SET Ten = @Ten, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, " +
                               "SDT = @SDT, Email = @Email, NguoiTao = @NguoiTao, TenNguoiTao = @TenNguoiTao, NgayTao = @NgayTao " +
                               "WHERE MaKhachHang = @MaKhachHang";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhachHang", khach.MaKhachHang);
                    cmd.Parameters.AddWithValue("@Ten", khach.Ten);
                    cmd.Parameters.AddWithValue("@NgaySinh", khach.NgaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", khach.GioiTinh);
                    cmd.Parameters.AddWithValue("@SDT", khach.SDT);
                    cmd.Parameters.AddWithValue("@Email", khach.Email);
                    cmd.Parameters.AddWithValue("@NguoiTao", khach.NguoiTao);
                    cmd.Parameters.AddWithValue("@TenNguoiTao", GetTenDangNhap(khach.NguoiTao.ToString()) ?? "Unknown");
                    cmd.Parameters.AddWithValue("@NgayTao", khach.NgayTao);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public List<DTO_Khach> GetAllKhach()
        {
            List<DTO_Khach> danhSach = new List<DTO_Khach>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaKhachHang, Ten, NgaySinh, GioiTinh, SDT, Email, TrangThai, NguoiTao, TenNguoiTao, NgayTao FROM QLKH";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_Khach khach = new DTO_Khach
                        {
                            MaKhachHang = reader.GetInt32(0),
                            Ten = reader.GetString(1),
                            NgaySinh = reader.IsDBNull(2) ? DateTime.Today : reader.GetDateTime(2),
                            GioiTinh = reader.GetString(3),
                            SDT = reader.GetString(4),
                            Email = reader.GetString(5),
                            TrangThai = reader.GetString(6),
                            NguoiTao = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                            TenNguoiTao = reader.IsDBNull(8) ? null : reader.GetString(8),
                            NgayTao = reader.IsDBNull(9) ? DateTime.Today : reader.GetDateTime(9)
                        };
                        danhSach.Add(khach);
                    }
                }
                conn.Close();
            }
            return danhSach;
        }

        public void ThemKhachHang(DTO_Khach khach)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO QLKH (Ten, NgaySinh, GioiTinh, SDT, Email, NguoiTao, TenNguoiTao, NgayTao) " +
                               "VALUES (@Ten, @NgaySinh, @GioiTinh, @SDT, @Email, @NguoiTao, @TenNguoiTao, @NgayTao)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ten", khach.Ten);
                    cmd.Parameters.AddWithValue("@NgaySinh", khach.NgaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", khach.GioiTinh);
                    cmd.Parameters.AddWithValue("@SDT", khach.SDT);
                    cmd.Parameters.AddWithValue("@Email", khach.Email);
                    cmd.Parameters.AddWithValue("@NguoiTao", khach.NguoiTao);
                    cmd.Parameters.AddWithValue("@TenNguoiTao", GetTenDangNhap(khach.NguoiTao.ToString()) ?? "Unknown");
                    cmd.Parameters.AddWithValue("@NgayTao", khach.NgayTao);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public bool XoaKhachHang(int maKhachHang, int nguoiCapNhat)
        {
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaKhachHang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string message = reader.GetString(0);
                                if (message.Contains("thành công"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    throw new Exception("Khách hàng không tồn tại!");
                else if (ex.Number == 50005)
                    throw new Exception("Không thể xóa khách hàng vì còn hóa đơn 'Chờ thanh toán'!");
                else
                    throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}