﻿using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DAL
{
    public class QLVatLieu : DBConnect
    {
        private SqlConnection conn = DBConnect.GetConnection();

        public List<DTO_VatLieu> LayTatCaVatLieu()
        {
            List<DTO_VatLieu> danhSachVatLieu = new List<DTO_VatLieu>();
            string query = @"
                       SELECT
                    tkv.MaVatLieu,
                    vl.Ten,
                    tkv.MaKho,
                    k.TenKho,
                    tkv.SoLuongTon,
                    vl.DonViTinh,
                    vl.DonGiaNhap,
                    vl.DonGiaBan,
                    vl.TrangThai AS TrangThaiVatLieu,
                    lv.TenLoai,
                    vl.GhiChu,
                    vl.NgayTao,
                    vl.NguoiTao,
                    tkTao.TenDangNhap AS TenNguoiTao,
                    vl.NgayCapNhat,
                    vl.NguoiCapNhat,
                    tkCapNhat.TenDangNhap AS TenNguoiCapNhat
                FROM
                    dbo.TonKhoVatLieu tkv -- Bắt đầu từ bảng tồn kho
                INNER JOIN
                    dbo.QLVatLieu vl ON tkv.MaVatLieu = vl.MaVatLieu -- Lấy thông tin vật liệu
                INNER JOIN
                    dbo.Kho k ON tkv.MaKho = k.MaKho -- Lấy tên kho
                LEFT JOIN
                    dbo.QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu -- Lấy tên loại vật liệu
                LEFT JOIN
                    dbo.QLTK tkTao ON vl.NguoiTao = tkTao.MaTK -- Lấy tên người tạo
                LEFT JOIN
                    dbo.QLTK tkCapNhat ON vl.NguoiCapNhat = tkCapNhat.MaTK -- Lấy tên người cập nhật";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DTO_VatLieu vl = new DTO_VatLieu();
                                vl.MaVatLieu = reader.GetInt32(reader.GetOrdinal("MaVatLieu"));
                                vl.Ten = reader.GetString(reader.GetOrdinal("Ten"));
                                vl.TenLoai = reader["Ten"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("TenLoai")) : string.Empty;
                                vl.DonGiaNhap = reader.GetDecimal(reader.GetOrdinal("DonGiaNhap"));
                                vl.DonGiaBan = reader.GetDecimal(reader.GetOrdinal("DonGiaBan"));
                                vl.DonViTinh = reader.GetString(reader.GetOrdinal("DonViTinh"));
                                vl.SoLuong = reader.GetDecimal(reader.GetOrdinal("SoLuongTon"));
                                vl.TrangThai = reader.GetString(reader.GetOrdinal("TrangThaiVatLieu"));
                                vl.TenKho = reader.GetString(reader.GetOrdinal("TenKho"));
                                vl.NgayTao = reader.GetDateTime(reader.GetOrdinal("NgayTao"));
                                vl.NguoiTao = reader["NguoiTao"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("NguoiTao")) : (int?)null;
                                vl.NgayCapNhat = reader["NgayCapNhat"] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("NgayCapNhat")) : (DateTime?)null;
                                vl.NguoiCapNhat = reader["NguoiCapNhat"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("NguoiCapNhat")) : (int?)null;
                                vl.TenNguoiTao = reader["TenNguoiTao"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("TenNguoiTao")) : null;
                                vl.TenNguoiCapNhat = reader["TenNguoiCapNhat"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("TenNguoiCapNhat")) : null;

                                danhSachVatLieu.Add(vl);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Lỗi SQL khi lấy danh sách vật liệu: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi hệ thống khi lấy danh sách vật liệu: {ex.Message}");
                    throw;
                }
            }

            return danhSachVatLieu;
        }

        public async Task<DTO_VatLieu> LayThongTinVatLieuAsync(int maVatLieu)
        {
            DTO_VatLieu vatLieu = null;
            string query = @"
    SELECT
        MaVatLieu,
        Ten ,
        DonViTinh,
        SoLuong,
        DonGiaNhap,
         TrangThai
    FROM QLVatLieu
    WHERE MaVatLieu = @MaVatLieu
      AND TrangThai = N'Hoạt động';";
            try
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            vatLieu = MapReaderToVatLieu(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL_VatLieu.LayThongTinVatLieuAsync for ID {maVatLieu}: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in DAL_VatLieu.LayThongTinVatLieuAsync for ID {maVatLieu}: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    await conn.CloseAsync();
                }
            }
            return vatLieu;
        }

        private DTO_VatLieu MapReaderToVatLieu(SqlDataReader reader)
        {
            return new DTO_VatLieu
            {
                MaVatLieu = reader.GetInt32(reader.GetOrdinal("MaVatLieu")),
                Ten = reader.GetString(reader.GetOrdinal("Ten")),
                DonViTinh = reader.GetString(reader.GetOrdinal("DonViTinh")),
                SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong")),
                DonGiaNhap = reader.IsDBNull(reader.GetOrdinal("DonGiaNhap")) ? 0m : reader.GetDecimal(reader.GetOrdinal("DonGiaNhap"))
                // Không đọc TrangThai nếu SELECT không có
            };
        }

        public DTO_VatLieu TimVatLieuTheoTenChinhXac(string tenVatLieu)
        {
            DTO_VatLieu vatLieu = null;
            string query = "SELECT MaVatLieu, Ten, DonViTinh, DonGiaNhap, SoLuong" +
                           "FROM QLVatLieu" +
                           "WHERE LOWER(Ten) = LOWER(N'@Ten')";
            try
            {
                using (SqlConnection _conn = DBConnect.GetConnection())

                {
                    _conn.Open();
                    SqlCommand cmd = new SqlCommand(query, _conn);
                    cmd.Parameters.AddWithValue("@Ten", tenVatLieu.Trim());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vatLieu = new DTO_VatLieu
                            {
                                MaVatLieu = Convert.ToInt32(reader["MaVatLieu"]),
                                Ten = reader["Ten"].ToString(),
                                DonViTinh = reader["DonViTinh"].ToString(),
                                DonGiaNhap = reader["DonGiaNhap"] != DBNull.Value ? Convert.ToDecimal(reader["DonGiaNhap"]) : 0,
                                SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToDecimal(reader["SoLuong"]) : 0,
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi DAL_VatLieu.TimVatLieuTheoTenChinhXac: {ex.Message}");
                throw;
            }
            return vatLieu;
        }
        public List<DTO_Kho> LayDanhSachKhokho()
        {
            List<DTO_Kho> danhSachKho = new List<DTO_Kho>();
            using (SqlConnection _conn = DBConnect.GetConnection())
            {
                string query = "SELECT MaKho, TenKho FROM Kho"; 
                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    try
                    {
                        _conn.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DTO_Kho kho = new DTO_Kho
                                {
                                    MaKho = Convert.ToInt32(reader["MaKho"]),
                                    TenKho = reader["TenKho"].ToString()
                                };
                                danhSachKho.Add(kho);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_Kho] Lỗi khi lấy danh sách kho: {ex.Message}");
                        throw;
                    }
                }
            }
            return danhSachKho;
        }
        public DTO_Kho TimKhoTheoTen(string tenKho)
        {
            if (string.IsNullOrWhiteSpace(tenKho))
            {
                return null;
            }
            string query = "SELECT MaKho, TenKho FROM dbo.Kho WHERE TenKho = @TenKho";
            DTO_Kho kho = null;

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenKho", tenKho);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kho = new DTO_Kho
                                {
                                    MaKho = reader.GetInt32(reader.GetOrdinal("MaKho")),
                                    TenKho = reader.GetString(reader.GetOrdinal("TenKho"))
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                }
            }

            return kho;
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

        public DataTable LayTenKhoTheoTen(string Ten)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT TenKho FROM Kho WHERE TenKho = @TenKho", conn);
                cmd.Parameters.AddWithValue("@MaKho", Ten);
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
                SqlCommand cmd = new SqlCommand("SELECT TenLoai FROM QLLoaiVatLieu WHERE MaLoaiVatLieu = @MaLoaiVatLieu And TrangThai=N'Hoạt động'", conn);
                cmd.Parameters.AddWithValue("@MaLoaiVatLieu", maLoai);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public async Task<List<DTO_VatLieu>> TimKiemVatLieuAsync(string searchQuery)
        {
            List<DTO_VatLieu> dsVatLieu = new List<DTO_VatLieu>();
            string query = @"
                SELECT MaVatLieu, Ten, DonViTinh, SoLuong, DonGiaNhap -- Các cột cần lấy
                FROM QlVatLieu -- Thay tên bảng nếu khác
                WHERE CAST(MaVatLieu AS VARCHAR(20)) LIKE @SearchQuery OR TenVatLieu LIKE @SearchQuery
                -- AND TrangThai = 1 -- Chỉ lấy vật liệu đang hoạt động (tùy chọn)
                ORDER BY TenVatLieu;";

            try
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm ký tự đại diện '%' vào tham số
                    cmd.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%");

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dsVatLieu.Add(MapReaderToVatLieu(reader));
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL_VatLieu.TimKiemVatLieuAsync: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in DAL_VatLieu.TimKiemVatLieuAsync: {ex.Message}");
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    await conn.CloseAsync();
                }
            }
            return dsVatLieu;
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
                                SoLuong = reader.GetDecimal(6),
                                TenKho = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public List<DTO_VatLieu> TimKiemVatLieuTheoKho(int? maKho, string keyword)
        {
            List<DTO_VatLieu> ketQua = new List<DTO_VatLieu>();
            string searchKeyword = string.IsNullOrEmpty(keyword) ? "" : keyword;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_TimKiemVatLieuTheoKho", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaKho", maKho);
                cmd.Parameters.AddWithValue("@Keyword", searchKeyword);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DTO_VatLieu vl = new DTO_VatLieu
                        {
                            MaVatLieu = Convert.ToInt32(reader["MaVatLieu"]),
                            Ten = reader["Ten"] != DBNull.Value ? reader["Ten"].ToString() : string.Empty,
                            DonGiaBan = reader["DonGiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["DonGiaBan"]) : 0m,
                            DonViTinh = reader["DonViTinh"] != DBNull.Value ? reader["DonViTinh"].ToString() : string.Empty,
                            SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToDecimal(reader["SoLuong"]) : 0,
                            TenKho = reader["TenKho"] != DBNull.Value ? reader["TenKho"].ToString() : string.Empty,
                            MaKho = reader.IsDBNull(reader.GetOrdinal("MaKho")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaKho"))
                        };
                        ketQua.Add(vl);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL_VatLieu.TimKiemVatLieuTheoKho: {sqlEx.Message}, SP Params: MaKho={maKho}, Keyword='{searchKeyword}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DAL_VatLieu.TimKiemVatLieuTheoKho: {ex.Message}");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return ketQua;
        }

        public void ThemVatLieu(string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, int maKho, byte[] hinhAnh, string ghiChu, int NguoiTao)
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
                cmd.Parameters.AddWithValue("@NguoiTao", NguoiTao);

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

        public int GetSoLuongTonKho(int maVatLieu, int maKho)
        {
            int soLuongTon = 0;
            string query = "SELECT SoLuong FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu AND MaKho = @MaKho";

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    cmd.Parameters.AddWithValue("@MaKho", maKho);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            soLuongTon = Convert.ToInt32(result);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Lỗi SQL trong DAL GetSoLuongTonKho(VL={maVatLieu}, Kho={maKho}): {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi chung trong DAL GetSoLuongTonKho(VL={maVatLieu}, Kho={maKho}): {ex.Message}");
                    }
                }
            }
            return soLuongTon;
        }
    }
}