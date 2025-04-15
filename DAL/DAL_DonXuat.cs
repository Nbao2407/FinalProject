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
        public int ThemDonXuat(DTO_DonXuatInput donXuatInput, int? maKhoNguon, int nguoiThucHien)
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
                    cmd.Parameters.AddWithValue("@MaHoaDon", (object)donXuatInput.MaHoaDon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaKhachHang", (object)donXuatInput.MaKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(donXuatInput.GhiChu) ? DBNull.Value : (object)donXuatInput.GhiChu);
                    cmd.Parameters.AddWithValue("@ChiTietXuat", chiTietJson);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiThucHien);
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && reader.FieldCount > 1 && reader.GetName(1).Equals("MaDonXuat", StringComparison.OrdinalIgnoreCase))
                            {
                                newMaDonXuat = Convert.ToInt32(reader["MaDonXuat"]);
                            }
                            else
                            {
                                // Có thể đọc Message trả về từ SP nếu cần
                                // Console.WriteLine(reader["Message"]?.ToString());
                            }
                        }
                        // Nếu SP không trả về MaDonXuat qua SELECT, bạn cần Output Parameter
                        // SqlParameter outputIdParam = new SqlParameter("@NewMaDonXuatOutput", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        // cmd.Parameters.Add(outputIdParam);
                        // cmd.ExecuteNonQuery();
                        // if (outputIdParam.Value != DBNull.Value) newMaDonXuat = Convert.ToInt32(outputIdParam.Value);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi DAL ThemDonXuat (SQL): {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi chi tiết
                        Console.WriteLine($"Lỗi DAL ThemDonXuat: {ex.Message}");
                        throw; // Ném lại để tầng trên xử lý
                    }
                }
            }
            return newMaDonXuat;
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
            string query = @"
        SELECT TOP 1
            dx.MaDonXuat, dx.NgayXuat, dx.MaTK, tkLap.TenDangNhap AS TenNguoiLap,
            dx.LoaiXuat, dx.MaHoaDon, dx.MaKhachHang, kh.Ten AS TenKhachHang,
            dx.TrangThai, dx.GhiChu, dx.NgayCapNhat, dx.NguoiCapNhat,
            tkCapNhat.TenDangNhap AS TenNguoiCapNhat
        FROM QLDonXuat dx
        INNER JOIN QLTK tkLap ON dx.MaTK = tkLap.MaTK
        LEFT JOIN QLKH kh ON dx.MaKhachHang = kh.MaKhachHang
        LEFT JOIN QLTK tkCapNhat ON dx.NguoiCapNhat = tkCapNhat.MaTK
        WHERE dx.MaDonXuat = @MaDonXuat;";

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
                            if (reader.Read())
                            {
                                donXuat = new DTO_DonXuat
                                {
                                    MaDonXuat = reader.GetInt32(reader.GetOrdinal("MaDonXuat")),
                                    NgayXuat = reader.GetDateTime(reader.GetOrdinal("NgayXuat")),
                                    MaTK = reader.GetInt32(reader.GetOrdinal("MaTK")),
                                    TenNguoiLap = reader.GetString(reader.GetOrdinal("TenNguoiLap")),
                                    LoaiXuat = reader.GetString(reader.GetOrdinal("LoaiXuat")),
                                    MaHoaDon = reader.IsDBNull(reader.GetOrdinal("MaHoaDon")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MaHoaDon")),
                                    MaKhachHang = reader.IsDBNull(reader.GetOrdinal("MaKhachHang")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MaKhachHang")),
                                    TenKhachHang = reader.IsDBNull(reader.GetOrdinal("TenKhachHang")) ? null : reader.GetString(reader.GetOrdinal("TenKhachHang")),
                                    TrangThai = reader.GetString(reader.GetOrdinal("TrangThai")),
                                    GhiChu = reader.IsDBNull(reader.GetOrdinal("GhiChu")) ? null : reader.GetString(reader.GetOrdinal("GhiChu")),
                                    NgayCapNhat = reader.IsDBNull(reader.GetOrdinal("NgayCapNhat")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayCapNhat")),
                                    NguoiCapNhat = reader.IsDBNull(reader.GetOrdinal("NguoiCapNhat")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("NguoiCapNhat")),
                                    TenNguoiCapNhat = reader.IsDBNull(reader.GetOrdinal("TenNguoiCapNhat")) ? null : reader.GetString(reader.GetOrdinal("TenNguoiCapNhat"))
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi DAL GetDonXuatById (MaDX={maDonXuat}): {ex.Message}");
                    }
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