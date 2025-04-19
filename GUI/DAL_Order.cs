using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_Order : DBConnect
    {
        public List<DTO_Order> GetAllOrder()
        {
            List<DTO_Order> dt = new List<DTO_Order>();
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
                            dt.Add(new DTO_Order
                            {
                                MaDonNhap = reader.GetInt32(0),
                                NgayNhap = reader.GetDateTime(1),
                                TenNCC = reader.IsDBNull(2) ? null : reader.GetString(2),
                                TrangThai = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return dt;
        }

        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(DateOnly ngayNhap, int maNCC, int maTK, string ghiChu, string chiTietNhap, int nguoiCapNhat,int NguoiNhap)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                await conn.OpenAsync();

                using (SqlCommand command = new SqlCommand("sp_NhapHang", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@MaNCC", maNCC);
                    command.Parameters.AddWithValue("@MaTK", maTK);
                    command.Parameters.AddWithValue("@GhiChu", (object)ghiChu ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ChiTietNhap", chiTietNhap);
                    command.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                    command.Parameters.AddWithValue("@NguoiNhap", NguoiNhap);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string message = reader["Message"].ToString();
                            int maDonNhap = Convert.ToInt32(reader["MaDonNhap"]);
                            return (true, message, maDonNhap);
                        }
                    }

                    return (false, "Không nh?n ???c ph?n h?i t? c? s? d? li?u.", 0);
                }
            }
        }


        public async Task<List<DTO_VatLieu>> TimKiemVatLieuAsync(string keyword)
        {
            var result = new List<DTO_VatLieu>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                await conn.OpenAsync();

                using (SqlCommand command = new SqlCommand("sp_TimKiemVatLieuNameID", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new DTO_VatLieu
                            {
                                MaVatLieu = reader.GetInt32("MaVatLieu"),
                                Ten = reader.GetString("Ten"),
                                DonViTinh = reader.GetString("DonViTinh"),
                                DonGiaNhap = reader.GetDecimal("DonGiaNhap"),
                                DonGiaBan = reader.GetDecimal("DonGiaBan"),
                                SoLuong = reader.GetInt32("SoLuong")
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<DTO_DonNhapSearchResult> TimKiemDonNhapTheoKeyword(string keyword)
        {
            List<DTO_DonNhapSearchResult> orderList = new List<DTO_DonNhapSearchResult>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemQLDonNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword); // Truyền keyword gốc

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                DTO_DonNhapSearchResult order = new DTO_DonNhapSearchResult
                                {
                                    MaDonNhap = reader.GetInt32(reader.GetOrdinal("MaDonNhap")),
                                    NgayNhap = reader.GetDateTime(reader.GetOrdinal("NgayNhap")), // Đọc đúng kiểu DateTime
                                    TenNhaCungCap = reader.IsDBNull(reader.GetOrdinal("TenNCC")) ? null : reader.GetString(reader.GetOrdinal("TenNCC")), // Tên cột khớp với AS trong SP
                                    TrangThai = reader.IsDBNull(reader.GetOrdinal("TrangThai")) ? null : reader.GetString(reader.GetOrdinal("TrangThai")),
                                };
                                orderList.Add(order);
                            }
                            catch (Exception ex) { Console.WriteLine($"Error reading search result row from SP: {ex.Message}"); }
                        }
                    }
                }
            }
            return orderList;
        }

        public List<DTO_DonNhapSearchResult> SearchOrder(string keyword)
        {
            List<DTO_DonNhapSearchResult> danhSachL = new List<DTO_DonNhapSearchResult>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemQLDonNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachL.Add(new DTO_DonNhapSearchResult
                            {
                                MaDonNhap = reader.GetInt32(0),
                                TenNhaCungCap = reader.GetString(1),
                                NgayNhap = reader.GetDateTime(2),
                                TrangThai = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return danhSachL;
        }
    }
}
