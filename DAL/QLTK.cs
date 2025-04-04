﻿using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

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
        public DTO_ngdung KiemTraDangNhap(string username, string password, string email)
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
                            return new DTO_ngdung
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("MaTK")),
                                username = reader.GetString(reader.GetOrdinal("TenDangNhap")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                role = reader.GetString(reader.GetOrdinal("ChucVu")),
                                TrangThai = reader.GetString(reader.GetOrdinal("TrangThai"))
                            };
                        }
                        return null; 
                    }
                }
            }
        }
        public List<DTO_TK> GetTkByID(int id)
        {
            List<DTO_TK> danhSachTk = new List<DTO_TK>();

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT maTK, TenDangNhap, Email, ChucVu,SDT, GHICHU ,DiaChi FROM QLTK WHERE maTk = @maTk";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTk", id); 
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
                                    reader.GetString(6)
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
    }
}
