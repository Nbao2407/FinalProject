using DTO;
using Microsoft.Data.SqlClient;
using System.Data;

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
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DTO_Order order = new DTO_Order
                                {
                                    MaDonNhap = reader.GetInt32(0),
                                    NgayNhap = reader.GetDateTime(1),

                                    TenNCC = reader.IsDBNull(2) ? null : reader.GetString(2),

                                    TrangThai = reader.IsDBNull(3) ? null : reader.GetString(3)
                                    // ------------------
                                };
                                orders.Add(order);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi đọc dữ liệu đơn nhập: {ex.Message}");
                        return new List<DTO_Order>();
                    }
                }
            }
            return orders;
        }

        public bool DuyetDonNhap(int maDonNhap, int maTK_NguoiDuyet)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("sp_DuyetDonNhap", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaDonNhap", maDonNhap);
                    command.Parameters.AddWithValue("@MaTK_NguoiDuyet", maTK_NguoiDuyet);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine($"[DAL_DonNhap] Đã gọi sp_DuyetDonNhap thành công cho MaDonNhap: {maDonNhap}");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_DonNhap] Lỗi SQL khi duyệt đơn nhập {maDonNhap}: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_DonNhap] Lỗi khác khi duyệt đơn nhập {maDonNhap}: {ex.Message}");
                        throw;
                    }
                } // SqlCommand được dispose
            } // SqlConnection được dispose
        }
        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(DateOnly ngayNhap, int maNCC, int maTK, string ghiChu, string chiTietJson, int nguoiCapNhat,int makho)
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
                    command.Parameters.AddWithValue("@MaKhoNhap", makho);
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
            DTO_OrderDetail orderDetail = null;

            string queryHeader = @"
                SELECT
                dn.MaDonNhap, dn.NgayNhap, dn.MaNCC, ncc.TenNCC, dn.MaTK,
                tk.TenDangNhap AS TenNguoiTao, dn.TrangThai, dn.GhiChu,
                dn.NgayCapNhat, dn.NguoiNhap, tkNhap.TenDangNhap AS TenNguoiNhap,
                tkUpdate.TenDangNhap AS TenNguoiCapNhat, dn.NgayTao
            FROM QLDonNhap dn
            LEFT JOIN NCC ncc ON dn.MaNCC = ncc.MaNCC
            LEFT JOIN QLTK tk ON dn.MaTK = tk.MaTK
            LEFT JOIN QLTK tkUpdate ON dn.NguoiCapNhat = tkUpdate.MaTK
            LEFT JOIN QLTK tkNhap ON dn.NguoiNhap = tkNhap.MaTK
                WHERE dn.MaDonNhap = @OrderId";

            string queryDetails = @"
               SELECT
                    ctdn.MaDonNhap,        
                    ctdn.MaVatLieu,       
                    vl.Ten AS TenVatLieu,  
                    vl.DonViTinh,        
                    ctdn.MaKho,            
                    k.TenKho AS TenKho,   
                    ctdn.SoLuong,       
                    ctdn.GiaNhap          
                FROM
                    dbo.ChiTietDonNhap ctdn
                INNER JOIN
                    dbo.QLVatLieu vl ON ctdn.MaVatLieu = vl.MaVatLieu 
                INNER JOIN
                    dbo.Kho k ON ctdn.MaKho = k.MaKho
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
                            if (readerHeader.Read())
                            {
                                orderDetail = new DTO_OrderDetail
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
                                    NgayTao = readerHeader.GetDateTime("NgayTao"),
                                    NguoiNhap =readerHeader.GetInt32(readerHeader.GetOrdinal("NguoiNhap")),
                                    TenNguoiNhap = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenNguoiNhap")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenNguoiNhap")),
                                    ChiTietDonNhap = new List<DTO_ChiTietDonNhap>()
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
                                            GiaNhap = readerDetails.GetDecimal(readerDetails.GetOrdinal("GiaNhap")),
                                            MaKho = readerDetails.GetInt32(readerDetails.GetOrdinal("MaKho")),
                                            TenKho = readerDetails.GetString(readerDetails.GetOrdinal("TenKho")),
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
        public bool KiemTraDonNhapMoCuaNCC(int maNcc)
        {
            bool coDonMo = false;
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                string query = @"
                    SELECT TOP 1 
                    FROM dbo.QLDonNhap
                    WHERE MaNCC = @MaNCC
                      AND TrangThai NOT IN (N'Hoàn thành', N'Chờ duyệt', N'Từ chối');";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@MaNCC", maNcc);
                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar();
                        coDonMo = (result != null && result != DBNull.Value);
                        Console.WriteLine($"[DAL_DonNhap] Kết quả kiểm tra đơn mở cho NCC {maNcc}: {coDonMo}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_DonNhap] Lỗi khi kiểm tra đơn nhập mở của NCC {maNcc}: {ex.Message}");
                        throw;
                    }
                }
            }
            return coDonMo;
        }
        public async Task<bool> UpdateOrderFromJsonAsync(DTO_Order header, string chiTietJson, int userId)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_CapNhatDonNhap", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaDonNhap", header.MaDonNhap);
                    cmd.Parameters.AddWithValue("@NgayNhap", header.NgayNhap); 
                    cmd.Parameters.AddWithValue("@MaNCC", header.MaNCC);
                    cmd.Parameters.AddWithValue("@MaTK", CurrentUser.MaTK);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)header.GhiChu ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChiTietNhap", chiTietJson);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", userId);

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                        return true; // Trả về true nếu không có lỗi
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine($"DAL SQL Error calling sp_CapNhatDonNhap: {sqlEx.Message}");
                        throw new Exception($"Lỗi từ cơ sở dữ liệu: {sqlEx.Message}", sqlEx); // Bọc lỗi SQL
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL General Error calling sp_CapNhatDonNhap: {ex.Message}");
                        throw; // Ném lại các lỗi khác
                    }
                }
            }
        }

        public async Task<bool> CapNhatTrangThaiDonNhapAsync(int maDonNhap, string trangThaiMoi, int maNguoiCapNhat)
        {
            string sql = @"
            UPDATE QLDonNhap
            SET
                TrangThai = @TrangThai,
                NguoiCapNhat = @NguoiCapNhat,
                NgayCapNhat = @NgayCapNhat
                WHERE
                MaDonNhap = @MaDonNhap;";
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                using (var command = new SqlCommand(sql, conn))
                {
                    command.Parameters.Add("@MaDonNhap", SqlDbType.Int).Value = maDonNhap;
                    command.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value = trangThaiMoi;
                    command.Parameters.Add("@NguoiCapNhat", SqlDbType.Int).Value = maNguoiCapNhat;
                    command.Parameters.Add("@NgayCapNhat", SqlDbType.DateTime).Value = DateTime.Now; 
                    await conn.OpenAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected == 1;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error updating order status for MaDonNhap {maDonNhap}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error updating order status for MaDonNhap {maDonNhap}: {ex.Message}");
                return false;
            }
        }
    }
}
