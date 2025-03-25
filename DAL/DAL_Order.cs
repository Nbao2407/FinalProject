using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace DAL
{
    public class DAL_Order : DBConnect
    {
        public List<DTO_Order> TimKiemDonNhap(int ID)
        {
            List<DTO_Order> danhSachOrder = new List<DTO_Order>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemQLDonNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaDonNhap", ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachOrder.Add(new DTO_Order
                            {
                                MaDonNhap = reader.GetInt32(0),
                                NgayNhap = reader.GetDateTime(1),
                                NCC = reader.GetString(2),
                                TrangThai = reader.GetString(3),
                            });
                        }
                    }
                }
            }
            return danhSachOrder;
        }
        public List<DTO_Order> GetAllOrder()
        {
            List<DTO_Order> danhSachOrder = new List<DTO_Order>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT QLDonNhap.MaDonNhap, QLDonNhap.NgayNhap, NCC.TenNCC, QLDonNhap.TrangThai " +
                    "FROM QLDonNhap " +
                    "LEFT JOIN NCC ON QLDonNhap.MaNCC = NCC.MaNCC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachOrder.Add(new DTO_Order
                            {
                                MaDonNhap = reader.GetInt32(0),
                                NgayNhap = reader.GetDateTime(1),
                                NCC = reader.IsDBNull(2) ? null : reader.GetString(2),
                                TrangThai = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return danhSachOrder;
        }
    }
}
