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
            List<DTO_Order> orders = new List<DTO_Order>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = @"SELECT QLDonNhap.MaDonNhap, QLDonNhap.NgayNhap, NCC.TenNCC, QLDonNhap.TrangThai " +
                    "FROM QLDonNhap " +
                    "LEFT JOIN NCC ON QLDonNhap.MaNCC = NCC.MaNCC;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DTO_Order order = new DTO_Order
                        {
                            MaDonNhap = reader.GetInt32(0),
                            NgayNhap = reader.GetDateTime(1),
                            TenNhaCungCap = reader.GetString(2),
                            TrangThai = reader.GetString(3)
                        };
                        orders.Add(order);
                    }
                }
            }
            return orders;
        }

        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(DateOnly ngayNhap, int maNCC, int maTK, string ghiChu, string chiTietJson, int nguoiCapNhat)
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
                    command.Parameters.AddWithValue("@ChiTietNhap", chiTietJson);
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
        public List<DTO_Order> SearchOrder(string keyword)
        {
            List<DTO_Order> danhSachL = new List<DTO_Order>();

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
                            danhSachL.Add(new DTO_Order
                            {
                                MaDonNhap = reader.GetInt32(0),
                                TenNCC = reader.GetString(1),
                                TrangThai = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            return danhSachL;
        }
        public DTO_OrderDetail GetOrderDetailById(int? orderId)
        {
            DTO_OrderDetail orderDetail = null; // Khởi tạo là null

            string queryHeader = @"
                SELECT
                    dn.MaDonNhap, dn.NgayNhap, dn.MaNCC, ncc.TenNCC, dn.MaTK,
                    tk.TenDangNhap AS TenNguoiTao, dn.TrangThai, dn.GhiChu,
                    dn.NgayCapNhat, tkUpdate.TenDangNhap AS TenNguoiCapNhat, dn.NgayTao
                FROM QLDonNhap dn
                LEFT JOIN NCC ncc ON dn.MaNCC = ncc.MaNCC
                LEFT JOIN QLTK tk ON dn.MaTK = tk.MaTK
                LEFT JOIN QLTK tkUpdate ON dn.NguoiCapNhat = tkUpdate.MaTK
                WHERE dn.MaDonNhap = @OrderId";

            string queryDetails = @"
                SELECT
                    ctdn.MaDonNhap, ctdn.MaVatLieu, vl.Ten AS TenVatLieu,
                    vl.DonViTinh, ctdn.SoLuong, ctdn.GiaNhap
                FROM ChiTietDonNhap ctdn
                INNER JOIN QLVatLieu vl ON ctdn.MaVatLieu = vl.MaVatLieu
                WHERE ctdn.MaDonNhap = @OrderId";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmdHeader = new SqlCommand(queryHeader, conn))
                    {
                        cmdHeader.Parameters.AddWithValue("@OrderId", orderId);
                        using (SqlDataReader readerHeader = cmdHeader.ExecuteReader())
                        {
                            if (readerHeader.Read()) // Chỉ đọc 1 dòng header
                            {
                                orderDetail = new DTO_OrderDetail // Tạo đối tượng
                                {
                                    MaDonNhap = readerHeader.GetInt32(readerHeader.GetOrdinal("MaDonNhap")),
                                    NgayNhap = readerHeader.GetDateTime(readerHeader.GetOrdinal("NgayNhap")),
                                    MaNCC = readerHeader.GetInt32(readerHeader.GetOrdinal("MaNCC")),
                                    TenNCC = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenNCC")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenNCC")),
                                    MaTK = readerHeader.GetInt32(readerHeader.GetOrdinal("MaTK")),
                                    NguoiTao = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenNguoiTao")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenNguoiTao")),
                                    TrangThai = readerHeader.IsDBNull(readerHeader.GetOrdinal("TrangThai")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TrangThai")),
                                    GhiChu = readerHeader.IsDBNull(readerHeader.GetOrdinal("GhiChu")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("GhiChu")),
                                    NgayCapNhat = readerHeader.GetDateTime(readerHeader.GetOrdinal("NgayCapNhat")),
                                    NguoiCapNhatTen = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenNguoiCapNhat")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenNguoiCapNhat")),
                                    NgayTao = readerHeader.GetDateTime("NgayTao") ,
                                    ChiTietDonNhap = new List<DTO_ChiTietDonNhap>() // Khởi tạo list chi tiết
                                };
                            }
                        } 
                    } 

                    if (orderDetail != null)
                    {
                        using (SqlCommand cmdDetails = new SqlCommand(queryDetails, conn))
                        {
                            cmdDetails.Parameters.AddWithValue("@OrderId", orderId);
                            using (SqlDataReader readerDetails = cmdDetails.ExecuteReader())
                            {
                                while (readerDetails.Read())
                                {
                                    try
                                    {
                                        DTO_ChiTietDonNhap detailItem = new DTO_ChiTietDonNhap
                                        {
                                            MaDonNhap = readerDetails.GetInt32(readerDetails.GetOrdinal("MaDonNhap")),
                                            MaVatLieu = readerDetails.GetInt32(readerDetails.GetOrdinal("MaVatLieu")),
                                            TenVatLieu = readerDetails.GetString(readerDetails.GetOrdinal("TenVatLieu")),
                                            DonViTinh = readerDetails.GetString(readerDetails.GetOrdinal("DonViTinh")),
                                            SoLuong = readerDetails.GetInt32(readerDetails.GetOrdinal("SoLuong")),
                                            GiaNhap = readerDetails.GetDecimal(readerDetails.GetOrdinal("GiaNhap")) 
                                        };
                                        orderDetail.ChiTietDonNhap.Add(detailItem);
                                    }
                                    catch (Exception exRow) { Console.WriteLine($"Error reading order detail row: {exRow.Message}"); }
                                }
                            } 
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database error in GetOrderDetailById: {ex.Message}");
                }
            }

            return orderDetail;
        }

    }
}
