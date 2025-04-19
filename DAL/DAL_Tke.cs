using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DAL_Tke : DBConnect
    {
        public List<DTO_Tke> GetThongKeDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            List<DTO_Tke> result = new List<DTO_Tke>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_ThongKeDoanhThu", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TuNgay", (object)tuNgay ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DenNgay", (object)denNgay ?? DBNull.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DTO_Tke dto = new DTO_Tke
                        {
                            MaHoaDon = reader.GetInt32(reader.GetOrdinal("MaHoaDon")),
                            NgayLap = reader.GetDateTime(reader.GetOrdinal("NgayLap")),
                            NguoiLap = reader.GetString(reader.GetOrdinal("NguoiLap")),
                            KhachHang = reader.IsDBNull(reader.GetOrdinal("KhachHang")) ? "Khách vãng lai" : reader.GetString(reader.GetOrdinal("KhachHang")),
                            TongTien = reader.GetDecimal(reader.GetOrdinal("TongTien")),
                            TrangThai = reader.GetString(reader.GetOrdinal("TrangThai")),
                            HinhThucThanhToan = reader.GetString(reader.GetOrdinal("HinhThucThanhToan"))
                        };
                        result.Add(dto);
                    }
                }
            }
            return result;
        }
        public List<TopProductDTO> GetTopProducts(DateTime? tuNgay, DateTime? denNgay)
        {
            List<TopProductDTO> result = new List<TopProductDTO>();
            using (SqlConnection conn = DBConnect.GetConnection())

            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_ThongKeTopProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TuNgay", (object)tuNgay ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DenNgay", (object)denNgay ?? DBNull.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new TopProductDTO
                        {
                            TenVatLieu = reader.GetString("TenVatLieu"),
                            SoLuongBan = reader.GetInt32("SoLuongBan")
                        });
                    }
                }
            }
            return result;
        }

        public List<UnderstockDTO> GetUnderstock()
        {
            List<UnderstockDTO> result = new List<UnderstockDTO>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_ThongKeUnderstock", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new UnderstockDTO
                        {
                            TenVatLieu = reader.GetString("TenVatLieu"),
                            SoLuongTon = reader.GetDecimal("SoLuongTon")
                        });
                    }
                }
            }
            return result;
        }

        // Đếm số nhà cung cấp
        public int GetSoNhaCungCap()
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DemSoNhaCungCap", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}