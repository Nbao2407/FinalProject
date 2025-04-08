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

        public DataTable GetAllOrder()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT QLDonNhap.MaDonNhap, QLDonNhap.NgayNhap, NCC.TenNCC, QLDonNhap.TrangThai " +
                    "FROM QLDonNhap " +
                    "LEFT JOIN NCC ON QLDonNhap.MaNCC = NCC.MaNCC", conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(DateOnly ngayNhap, int maNCC, int maTK, string ghiChu, string chiTietNhap, int nguoiCapNhat)
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

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string message = reader["Message"].ToString();
                            int maDonNhap = Convert.ToInt32(reader["MaDonNhap"]);
                            return (true, message, maDonNhap);
                        }
                    }

                    return (false, "Không nhận được phản hồi từ cơ sở dữ liệu.", 0);
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
            var results = new List<DTO_DonNhapSearchResult>();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return results; 
            }
            const string storedProcedure = "sp_TimKiemQLDonNhap";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open(); 

                using (var cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 100).Value = keyword;

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int maDonNhapOrd = reader.GetOrdinal("MaDonNhap");
                            int ngayNhapOrd = reader.GetOrdinal("NgayNhap");
                            int tenNhaCungCapOrd = reader.GetOrdinal("TenNhaCungCap");
                            int trangThaiOrd = reader.GetOrdinal("TrangThai");

                            while (reader.Read())
                            {
                                var dto = new DTO_DonNhapSearchResult
                                {
                                    MaDonNhap = reader.GetInt32(maDonNhapOrd),
                                    TenNhaCungCap = reader.IsDBNull(tenNhaCungCapOrd)
                                                                                    ? null // Gán null nếu là DBNull
                                                                                    : reader.GetString(tenNhaCungCapOrd),
                                    NgayNhap = reader.IsDBNull(ngayNhapOrd)
                                               ? default // Giá trị mặc định cho DateTime (MinValue)
                                               : reader.GetDateTime(ngayNhapOrd),

                                    

                                    TrangThai = reader.IsDBNull(trangThaiOrd)
                                                ? null
                                                : reader.GetString(trangThaiOrd),

                                    

                                };
                                results.Add(dto);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi khi thực thi SP '{storedProcedure}' với Keyword='{keyword}': {ex.Message}");
                        throw;
                    }
                }
            }
            return results;
        }
    }
}
