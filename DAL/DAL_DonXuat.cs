using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Linq;

namespace DAL
{
    public class DAL_DonXuat : DBConnect
    {
        public int ThemDonXuat(DTO_DonXuatInput donXuatInput, int? maKhoNguon, int? maKhoDich, int nguoiThucHien) 
        {
            int newMaDonXuat = -1;

            string chiTietJson = JsonSerializer.Serialize(donXuatInput.ChiTiet);

            const string storedProcedure = "sp_ThemDonXuat";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NgayXuat", donXuatInput.NgayXuat);
                    cmd.Parameters.AddWithValue("@MaTK", donXuatInput.MaTK); 
                    cmd.Parameters.AddWithValue("@LoaiXuat", donXuatInput.LoaiXuat);

                    cmd.Parameters.AddWithValue("@MaKhoNguon", (object)maKhoNguon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaKhoDich", (object)maKhoDich ?? DBNull.Value); 
                    cmd.Parameters.AddWithValue("@MaHoaDon", (object)donXuatInput.MaHoaDon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaKhachHang", (object)donXuatInput.MaKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(donXuatInput.GhiChu) ? DBNull.Value : (object)donXuatInput.GhiChu);

                    cmd.Parameters.AddWithValue("@ChiTietXuat", chiTietJson);

                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiThucHien);

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                string message = reader["Message"]?.ToString() ?? "Không có thông báo.";
                                Console.WriteLine($"Thông báo từ SP: {message}");

                                if (reader.FieldCount > 1 && reader.GetName(1).Equals("MaDonXuat", StringComparison.OrdinalIgnoreCase) && reader["MaDonXuat"] != DBNull.Value)
                                {
                                    newMaDonXuat = Convert.ToInt32(reader["MaDonXuat"]);
                                }
                                else
                                {
                                    Console.WriteLine("SP không trả về MaDonXuat hợp lệ.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("SP không trả về kết quả nào.");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi SQL khi gọi sp_ThemDonXuat: {ex.Message} (Số lỗi: {ex.Number})");
                        throw new Exception($"Lỗi từ cơ sở dữ liệu: {ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi hệ thống trong ThemDonXuat (DAL): {ex.Message}");
                        throw; // Ném lại lỗi gốc
                    }
                } 
            } 
            return newMaDonXuat;
        }
        public async Task<bool> UpdateDonXuatAsync(DTO_DonXuat headerUpdate, string chiTietMoiJson)
        {
            using (SqlConnection conn = DBConnect.GetConnection())

            {
                using (SqlCommand command = new SqlCommand("sp_UpdateDonXuat", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaDonXuat", headerUpdate.MaDonXuat);
                    command.Parameters.AddWithValue("@NgayXuat", headerUpdate.NgayXuat); 
                    command.Parameters.AddWithValue("@GhiChu", (object)headerUpdate.GhiChu ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaKH", (object)headerUpdate.MaKhachHang ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@MaKhoNguon_Header", headerUpdate.Makhonguon);
                    command.Parameters.AddWithValue("@MaKhoDich_Header", (object)headerUpdate.MaKhoDich ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@LoaiPhieu", headerUpdate.LoaiXuat);
                    command.Parameters.AddWithValue("@NguoiCapNhat", headerUpdate.NguoiCapNhat); 
                    command.Parameters.AddWithValue("@ChiTietMoi", chiTietMoiJson);

                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync(); 
                        Console.WriteLine($"[DAL_DonXuat] Đã gọi sp_UpdateDonXuat thành công cho MaDonXuat: {headerUpdate.MaDonXuat}");
                        return true; 
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_DonXuat] Lỗi SQL khi cập nhật đơn xuất {headerUpdate.MaDonXuat}: {ex.Message}");
                        throw; 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_DonXuat] Lỗi khác khi cập nhật đơn xuất {headerUpdate.MaDonXuat}: {ex.Message}");
                        throw; 
                    }
                } 
            } 
        }
        public bool DuyetDonXuat(int maDonXuat, int nguoiDuyet, string ghiChu = null)
        {
            bool success = false;
            const string storedProcedure = "dbo.sp_DuyetDonXuat"; 

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MaDonXuat", maDonXuat);
                        cmd.Parameters.AddWithValue("@NguoiDuyet", nguoiDuyet);
                        cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(ghiChu) ? DBNull.Value : (object)ghiChu);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine($"Thông báo từ sp_DuyetDonXuat: {reader["Message"]?.ToString()}");
                                success = true; 
                            }
                        }
                    } 
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Lỗi SQL khi duyệt đơn xuất {maDonXuat}: {ex.Message} (Số lỗi: {ex.Number})");
                    success = false; // Đánh dấu thất bại
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi hệ thống trong DuyetDonXuat (DAL) cho đơn {maDonXuat}: {ex.Message}");
                    success = false; // Đánh dấu thất bại
                }
            }

            return success;
        }

        public bool HuyDonXuat(int maDonXuat, int nguoiHuy, string lyDo)
        {
            const string storedProcedure = "sp_HuyDonXuat";
            int rowsAffected = 0;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaDonXuat", maDonXuat);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiHuy);
                    cmd.Parameters.AddWithValue("@LyDoHuy", string.IsNullOrEmpty(lyDo) ? DBNull.Value : (object)lyDo);
                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi DAL HuyDonXuat (SQL): {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DAL HuyDonXuat: {ex.Message}");
                        throw;
                    }
                }
            }
            return false;
        }


        public async Task<bool> TuChoiDonXuatAsync(int maDonXuat, int maTK_NguoiTuChoi)
        {
            using (SqlConnection conn = GetConnection())

            {
                using (SqlCommand command = new SqlCommand("sp_TuChoiDonXuat", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaDonXuat", maDonXuat);
                    command.Parameters.AddWithValue("@MaTK_NguoiTuChoi", maTK_NguoiTuChoi);
                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync(); 
                        Console.WriteLine($"[DAL_DonXuat] Đã gọi sp_TuChoiDonXuat thành công cho MaDonXuat: {maDonXuat}");
                        return true; 
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_DonXuat] Lỗi SQL khi từ chối đơn xuất {maDonXuat}: {ex.Message}");
                        throw; 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_DonXuat] Lỗi khác khi từ chối đơn xuất {maDonXuat}: {ex.Message}");
                        throw;
                    }
                } 
            } 
        }
        public List<DTO_DonXuat> GetAllDonXuat()
        {
            List<DTO_DonXuat> danhSach = new List<DTO_DonXuat>();

            string query = @"
        SELECT
            dx.MaDonXuat, dx.NgayXuat, dx.MaTK, tkLap.TenDangNhap AS TenNguoiLap,
            dx.LoaiXuat, dx.MaHoaDon, dx.MaKhachHang, kh.Ten AS TenKhachHang,
            dx.TrangThai, dx.GhiChu, dx.NgayCapNhat, dx.NguoiCapNhat,
            tkCapNhat.TenDangNhap AS TenNguoiCapNhat
        FROM QLDonXuat dx
        INNER JOIN QLTK tkLap ON dx.MaTK = tkLap.MaTK                 -- JOIN để lấy người lập
        LEFT JOIN QLKH kh ON dx.MaKhachHang = kh.MaKhachHang           -- LEFT JOIN khách hàng (có thể null)
        LEFT JOIN QLTK tkCapNhat ON dx.NguoiCapNhat = tkCapNhat.MaTK -- LEFT JOIN người cập nhật (có thể null)
        ORDER BY dx.NgayXuat DESC, dx.MaDonXuat DESC;";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int maDonXuatOrd = reader.GetOrdinal("MaDonXuat");
                            int ngayXuatOrd = reader.GetOrdinal("NgayXuat");
                            int maTKOrd = reader.GetOrdinal("MaTK");
                            int tenNguoiLapOrd = reader.GetOrdinal("TenNguoiLap");
                            int loaiXuatOrd = reader.GetOrdinal("LoaiXuat");
                            int maHoaDonOrd = reader.GetOrdinal("MaHoaDon");
                            int maKhachHangOrd = reader.GetOrdinal("MaKhachHang");
                            int tenKhachHangOrd = reader.GetOrdinal("TenKhachHang");
                            int trangThaiOrd = reader.GetOrdinal("TrangThai");
                            int ghiChuOrd = reader.GetOrdinal("GhiChu");
                            int ngayCapNhatOrd = reader.GetOrdinal("NgayCapNhat");
                            int nguoiCapNhatOrd = reader.GetOrdinal("NguoiCapNhat");
                            int tenNguoiCapNhatOrd = reader.GetOrdinal("TenNguoiCapNhat");

                            while (reader.Read())
                            {
                                DTO_DonXuat dx = new DTO_DonXuat
                                {
                                    MaDonXuat = reader.GetInt32(maDonXuatOrd),
                                    NgayXuat = reader.GetDateTime(ngayXuatOrd),
                                    MaTK = reader.GetInt32(maTKOrd),
                                    TenNguoiLap = reader.GetString(tenNguoiLapOrd),
                                    LoaiXuat = reader.GetString(loaiXuatOrd),
                                    MaHoaDon = reader.IsDBNull(maHoaDonOrd) ? (int?)null : reader.GetInt32(maHoaDonOrd),
                                    MaKhachHang = reader.IsDBNull(maKhachHangOrd) ? (int?)null : reader.GetInt32(maKhachHangOrd),
                                    TenKhachHang = reader.IsDBNull(tenKhachHangOrd) ? null : reader.GetString(tenKhachHangOrd),
                                    TrangThai = reader.GetString(trangThaiOrd),
                                    GhiChu = reader.IsDBNull(ghiChuOrd) ? null : reader.GetString(ghiChuOrd),
                                    NgayCapNhat = reader.IsDBNull(ngayCapNhatOrd) ? (DateTime?)null : reader.GetDateTime(ngayCapNhatOrd),
                                    NguoiCapNhat = reader.IsDBNull(nguoiCapNhatOrd) ? (int?)null : reader.GetInt32(nguoiCapNhatOrd),
                                    TenNguoiCapNhat = reader.IsDBNull(tenNguoiCapNhatOrd) ? null : reader.GetString(tenNguoiCapNhatOrd)
                                };
                                danhSach.Add(dx);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DAL GetAllDonXuat: {ex.Message}\n{ex.StackTrace}");

                        return new List<DTO_DonXuat>();
                    }
                }
            }
            return danhSach;
        }

        public DTO_DonXuat GetDonXuatById(int maDonXuat)
        {
            DTO_DonXuat donXuat = null;
            List<DTO_ChiTietDonXuat> chiTietList = new List<DTO_ChiTietDonXuat>();

            string queryHeader = @"
                                SELECT TOP 1
                     dx.MaDonXuat, dx.NgayXuat, dx.MaTK, tkLap.TenDangNhap AS TenNguoiLap,
                     dx.LoaiXuat, dx.MaHoaDon, dx.MaKhachHang AS MaKhachHang, kh.Ten AS TenKhachHang, -- Sửa lại tên cột MaKH nếu cần
                     dx.TrangThai, dx.GhiChu, dx.NgayCapNhat, dx.NguoiCapNhat,
                     tkCapNhat.TenDangNhap AS TenNguoiCapNhat
                 FROM dbo.QLDonXuat dx -- Thêm dbo.
                 INNER JOIN dbo.QLTK tkLap ON dx.MaTK = tkLap.MaTK
                 LEFT JOIN dbo.QLKH kh ON dx.MaKhachHang = kh.MaKhachHang -- Sửa lại tên cột MaKH nếu cần
                 LEFT JOIN dbo.QLTK tkCapNhat ON dx.NguoiCapNhat = tkCapNhat.MaTK
                 WHERE dx.MaDonXuat =@MaDonXuat;";

            string queryDetails = @"
               SELECT
                  ct.MaCTDX, ct.MaDonXuat, ct.MaVatLieu, vl.Ten AS TenVatLieu, vl.DonViTinh,
                  ct.MaKhoNguon, kn.TenKho AS TenKhoNguon, -- Lấy tên kho nguồn
                  ct.MaKhoDich, kd.TenKho AS TenKhoDich,   -- Lấy tên kho đích
                  ct.SoLuong, ct.DonGia
              FROM dbo.ChiTietDonXuat ct -- Thêm dbo.
              INNER JOIN dbo.QLVatLieu vl ON ct.MaVatLieu = vl.MaVatLieu
              LEFT JOIN dbo.Kho kn ON ct.MaKhoNguon = kn.MaKho -- Join để lấy tên Kho Nguồn
              LEFT JOIN dbo.Kho kd ON ct.MaKhoDich = kd.MaKho   -- Join để lấy tên Kho Đích (có thể NULL)
              WHERE ct.MaDonXuat = @MaDonXuat;";

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmdHeader = new SqlCommand(queryHeader, conn))
                    {
                        cmdHeader.Parameters.AddWithValue("@MaDonXuat", maDonXuat);
                        using (SqlDataReader readerHeader = cmdHeader.ExecuteReader())
                        {
                            if (readerHeader.Read())
                            {
                                donXuat = new DTO_DonXuat
                                {
                                    MaDonXuat = readerHeader.GetInt32(readerHeader.GetOrdinal("MaDonXuat")),
                                    NgayXuat = readerHeader.GetDateTime(readerHeader.GetOrdinal("NgayXuat")),
                                    MaTK = readerHeader.GetInt32(readerHeader.GetOrdinal("MaTK")),
                                    TenNguoiLap = readerHeader.GetString(readerHeader.GetOrdinal("TenNguoiLap")),
                                    LoaiXuat = readerHeader.GetString(readerHeader.GetOrdinal("LoaiXuat")),
                                    MaHoaDon = readerHeader.IsDBNull(readerHeader.GetOrdinal("MaHoaDon")) ? (int?)null : readerHeader.GetInt32(readerHeader.GetOrdinal("MaHoaDon")),
                                    MaKhachHang = readerHeader.IsDBNull(readerHeader.GetOrdinal("MaKhachHang")) ? (int?)null : readerHeader.GetInt32(readerHeader.GetOrdinal("MaKhachHang")),
                                    TenKhachHang = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenKhachHang")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenKhachHang")),
                                    TrangThai = readerHeader.GetString(readerHeader.GetOrdinal("TrangThai")),
                                    GhiChu = readerHeader.IsDBNull(readerHeader.GetOrdinal("GhiChu")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("GhiChu")),
                                    NgayCapNhat = readerHeader.IsDBNull(readerHeader.GetOrdinal("NgayCapNhat")) ? (DateTime?)null : readerHeader.GetDateTime(readerHeader.GetOrdinal("NgayCapNhat")),
                                    NguoiCapNhat = readerHeader.IsDBNull(readerHeader.GetOrdinal("NguoiCapNhat")) ? (int?)null : readerHeader.GetInt32(readerHeader.GetOrdinal("NguoiCapNhat")),
                                    TenNguoiCapNhat = readerHeader.IsDBNull(readerHeader.GetOrdinal("TenNguoiCapNhat")) ? null : readerHeader.GetString(readerHeader.GetOrdinal("TenNguoiCapNhat")), // Có thể không cần lưu tên người cập nhật trong DTO này
                                };
                            }
                        } 
                    }

                    if (donXuat != null)
                    {
                        using (SqlCommand cmdDetails = new SqlCommand(queryDetails, conn))
                        {
                            cmdDetails.Parameters.AddWithValue("@MaDonXuat", maDonXuat);
                            using (SqlDataReader readerDetails = cmdDetails.ExecuteReader())
                            {
                                var columns = Enumerable.Range(0, readerDetails.FieldCount)
                                                        .Select(i => readerDetails.GetName(i))
                                                        .ToList();

                                string[] requiredColumns = { "MaCTDX", "MaDonXuat", "MaVatLieu", "TenVatLieu", "DonViTinh",
                                                             "MaKhoNguon", "TenKhoNguon", "MaKhoDich", "TenKhoDich",
                                                             "SoLuong", "DonGia" };

                                foreach (string colName in requiredColumns)
                                {
                                    if (!columns.Any(c => c.Equals(colName, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        string errorMsg = $"Lỗi DAL GetDonXuatById: Câu lệnh queryDetails không trả về cột cần thiết '{colName}'. Các cột trả về: {string.Join(", ", columns)}";
                                        Console.WriteLine(errorMsg);
                                        throw new InvalidOperationException(errorMsg); // Ném lỗi rõ ràng hơn IndexOutOfRange
                                    }
                                }
                                int maCTDXOrdinal = readerDetails.GetOrdinal("MaCTDX");
                                int maDonXuatOrdinal = readerDetails.GetOrdinal("MaDonXuat");
                                int maVatLieuOrdinal = readerDetails.GetOrdinal("MaVatLieu");
                                int tenVatLieuOrdinal = readerDetails.GetOrdinal("TenVatLieu");
                                int donViTinhOrdinal = readerDetails.GetOrdinal("DonViTinh");
                                int maKhoNguonOrdinal = readerDetails.GetOrdinal("MaKhoNguon"); 
                                int tenKhoNguonOrdinal = readerDetails.GetOrdinal("TenKhoNguon");
                                int maKhoDichOrdinal = readerDetails.GetOrdinal("MaKhoDich");
                                int tenKhoDichOrdinal = readerDetails.GetOrdinal("TenKhoDich");
                                int soLuongOrdinal = readerDetails.GetOrdinal("SoLuong");
                                int donGiaOrdinal = readerDetails.GetOrdinal("DonGia");

                                while (readerDetails.Read())
                                {
                                    try
                                    {
                                        chiTietList.Add(new DTO_ChiTietDonXuat
                                        {
                                            MaCTDX = readerDetails.GetInt32(maCTDXOrdinal),
                                            MaDonXuat = readerDetails.GetInt32(maDonXuatOrdinal),
                                            MaVatLieu = readerDetails.GetInt32(maVatLieuOrdinal),
                                            TenVatLieu = readerDetails.GetString(tenVatLieuOrdinal),
                                            DonViTinh = readerDetails.GetString(donViTinhOrdinal),
                                            MaKhoNguon = readerDetails.GetInt32(maKhoNguonOrdinal),
                                            TenKhoNguon = readerDetails.IsDBNull(tenKhoNguonOrdinal) ? null : readerDetails.GetString(tenKhoNguonOrdinal),
                                            MaKhoDich = readerDetails.IsDBNull(maKhoDichOrdinal) ? (int?)null : readerDetails.GetInt32(maKhoDichOrdinal), // Đọc MaKhoDich
                                            TenKhoDich = readerDetails.IsDBNull(tenKhoDichOrdinal) ? null : readerDetails.GetString(tenKhoDichOrdinal),
                                            SoLuong = readerDetails.GetInt32(soLuongOrdinal),
                                            DonGia = readerDetails.GetDecimal(donGiaOrdinal)
                                        });
                                    }
                                    catch (Exception exRow)
                                    {
                                        Console.WriteLine($"--- Lỗi DAL khi đọc chi tiết dòng (MaDX={maDonXuat}): {exRow.ToString()} ---");
                                        continue; // Bỏ qua dòng lỗi
                                    }
                                }
                            }
                        }
                        donXuat.ChitietDonXuat = chiTietList;
                    }
                }
                catch (Exception ex) // Bắt lỗi chung
                {
                    Console.WriteLine($"Lỗi DAL GetDonXuatById (MaDX={maDonXuat}): {ex.ToString()}");
                    return null;
                }
            } 

            return donXuat;
        }

        public List<DTO_DonXuat> TimKiemDonXuat(string keyword, string trangThai)
        {
            // Logic tương tự GetAll nhưng thêm điều kiện WHERE dựa trên keyword và trangThai
            // WHERE (dx.MaDonXuat LIKE @KeywordInt OR tkLap.TenDangNhap LIKE @KeywordStr OR kh.Ten LIKE @KeywordStr)
            //       AND (@TrangThai IS NULL OR dx.TrangThai = @TrangThai)
            Console.WriteLine("Chức năng Tìm kiếm Đơn Xuất chưa được triển khai trong DAL.");
            return new List<DTO_DonXuat>(); // Trả về rỗng tạm thời
        }

        public int GetSoLuongTonKho(int maVatLieu, int maKho)
        {
            int soLuongTon = 0;
            string query = "SELECT SoLuong FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu AND MaKho = @MaKho";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    cmd.Parameters.AddWithValue("@MaKho", maKho);

                    try
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            soLuongTon = Convert.ToInt32(result);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi SQL trong GetSoLuongTonKho (VL:{maVatLieu}, Kho:{maKho}): {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi trong GetSoLuongTonKho (VL:{maVatLieu}, Kho:{maKho}): {ex.Message}");
                    }
                }
            }
            return soLuongTon;
        }

        public List<DTO_ChiTietDonXuat> GetChiTietDonXuat(int maDonXuat)
        {
            List<DTO_ChiTietDonXuat> danhSachChiTiet = new List<DTO_ChiTietDonXuat>();
            string query = @"
        SELECT
            ct.MaCTDX, ct.MaDonXuat, ct.MaVatLieu, vl.Ten AS TenVatLieu,
            vl.DonViTinh, ct.SoLuong, ct.DonGia, ct.MaKhoNguon, k.TenKho
        FROM ChiTietDonXuat ct
        INNER JOIN QLVatLieu vl ON ct.MaVatLieu = vl.MaVatLieu
        INNER JOIN Kho k ON ct.MaKhoNguon = k.MaKho 
        WHERE ct.MaDonXuat = @MaDonXuat;"; 

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDonXuat", maDonXuat);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int maCTDXOrd = reader.GetOrdinal("MaCTDX");
                            int maDonXuatOrd = reader.GetOrdinal("MaDonXuat");
                            int maVatLieuOrd = reader.GetOrdinal("MaVatLieu");
                            int tenVatLieuOrd = reader.GetOrdinal("TenVatLieu");
                            int donViTinhOrd = reader.GetOrdinal("DonViTinh");
                            int soLuongOrd = reader.GetOrdinal("SoLuong");
                            int donGiaOrd = reader.GetOrdinal("DonGia");
                            int maKhoNguonOrd = reader.GetOrdinal("MaKhoNguon");
                            int tenKhoOrd = reader.GetOrdinal("TenKho"); 

                            while (reader.Read())
                            {
                                DTO_ChiTietDonXuat ct = new DTO_ChiTietDonXuat
                                {
                                    MaCTDX = reader.GetInt32(maCTDXOrd),
                                    MaDonXuat = reader.GetInt32(maDonXuatOrd), 
                                    MaVatLieu = reader.GetInt32(maVatLieuOrd),
                                    TenVatLieu = reader.GetString(tenVatLieuOrd),
                                    DonViTinh = reader.GetString(donViTinhOrd),
                                    SoLuong = reader.GetInt32(soLuongOrd),
                                    DonGia = reader.GetDecimal(donGiaOrd),
                                    MaKhoNguon = reader.GetInt32(maKhoNguonOrd), 
                                    TenKho = reader.GetString(tenKhoOrd)
                                };
                                danhSachChiTiet.Add(ct);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DAL GetChiTietDonXuat (MaDX={maDonXuat}): {ex.Message}\n{ex.StackTrace}");
                        return new List<DTO_ChiTietDonXuat>();
                    }
                }
            }
            return danhSachChiTiet;
        }

    }
}