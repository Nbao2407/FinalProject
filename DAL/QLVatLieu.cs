﻿using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DAL
{
    public class QLVatLieu : DBConnect
    {
        public List<DTO_VatLieu> LayTatCaVatLieu()
        {
            List<DTO_VatLieu> danhSach = new List<DTO_VatLieu>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = @"
                            SELECT vl.MaVatLieu, vl.Ten, lv.TenLoai, vl.DonGiaNhap, vl.DonGiaBan, vl.DonViTinh, vl.SoLuong
                            FROM QLVatLieu vl
                            JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_VatLieu vatLieu = new DTO_VatLieu(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3),
                            reader.GetDecimal(4),
                            reader.GetString(5),
                            reader.GetInt32(6)
                        );
                        danhSach.Add(vatLieu);
                    }
                }
            }
            return danhSach;
        }
        public DataTable LayVatLieuTheoMa(int maVatLieu)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu", conn);
                cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable LayTenKhoTheoMa(int maKho)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TenKho FROM Kho WHERE MaKho = @MaKho", conn);
                cmd.Parameters.AddWithValue("@MaKho", maKho);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable LayTenNgTaoTheoMa(int maTk)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TenDangNhap FROM QLTK WHERE MaTk = @MaTk", conn);
                cmd.Parameters.AddWithValue("@MaTk", maTk);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable LayTenLoaiTheoMa(int maLoai)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TenLoai FROM QLLoaiVatLieu WHERE MaLoaiVatLieu = @MaLoaiVatLieu And TrangThai='Hoạt động'", conn);
                cmd.Parameters.AddWithValue("@MaLoaiVatLieu", maLoai);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable LayVatLieuTheoMa2(int maVatLieu)
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM VatLieu WHERE MaVatLieu = @MaVatLieu  And TrangThai='Hoạt động'";
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public List<DTO_VatLieu> SearchProducts(string keyword)
        {
            List<DTO_VatLieu> danhSach = new List<DTO_VatLieu>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemVatLieu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new DTO_VatLieu
                            {
                                MaVatLieu = reader.GetInt32(0),
                                Ten = reader.GetString(1),
                                TenLoai = reader.GetString(2),
                                DonGiaBan = reader.GetDecimal(3),
                                DonGiaNhap = reader.GetDecimal(4),
                                DonViTinh = reader.GetString(5),
                                SoLuong = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public void ThemVatLieu(string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, int maKho, byte[] hinhAnh, string ghiChu,int NguoiTao)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemVatLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@Loai", loai);
                cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                cmd.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                cmd.Parameters.AddWithValue("@DonViTinh", donViTinh);
                cmd.Parameters.AddWithValue("@MaKho", maKho);
                cmd.Parameters.AddWithValue("@HinhAnh", hinhAnh == null ? (object)DBNull.Value : hinhAnh);
                cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);
                cmd.Parameters.AddWithValue("@NguoiTao",NguoiTao );

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void CapNhatVatLieu(int maVatLieu, string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, byte[] hinhAnh, string ghiChu, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())

            {
                SqlCommand cmd = new SqlCommand("sp_CapNhatVatLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@Loai", loai);
                cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                cmd.Parameters.AddWithValue("@DonGiaBan", donGiaBan);
                cmd.Parameters.AddWithValue("@DonViTinh", donViTinh);
                cmd.Parameters.AddWithValue("@HinhAnh", hinhAnh == null ? (object)DBNull.Value : hinhAnh);
                cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void XoaVatLieu(int maVatLieu, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())

            {
                SqlCommand cmd = new SqlCommand("sp_XoaVatLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

      public DataTable LayDanhSachLoaiVatLieu()
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LayDanhSachLoaiVatLieu", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable LayDanhSachDonViTinh()
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_LayDanhSachDonViTinh", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable LayDanhSachKho()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Kho", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách kho: {ex.Message}");
            }
            return dt;
        }
       
    }
}