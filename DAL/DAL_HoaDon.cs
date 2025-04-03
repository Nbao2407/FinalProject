using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_HoaDon
    {
        public List<DTO_HoaDon> LayTatCaHoaDon()
        {
            List<DTO_HoaDon> danhSach = new List<DTO_HoaDon>();

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT hd.MaHoaDon, 
                       hd.NgayLap, 
                       tk.TenDangNhap AS NguoiLap, 
                       kh.MaKhachHang,
                       COALESCE(kh.Ten, N'Khách lẻ') AS TenKhachHang,
                       hd.TongTien, 
                       hd.TrangThai, 
                       hd.HinhThucThanhToan
                FROM QLHoaDon hd
                JOIN QLTK tk ON hd.MaTKLap = tk.MaTK
                LEFT JOIN QLKH kh ON hd.MaKhachHang = kh.MaKhachHang;
            ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            DTO_HoaDon hd = new DTO_HoaDon
                            {
                                MaHoaDon = reader.GetInt32(0),
                                NgayLap = reader.GetDateTime(1),
                                NguoiLap = reader.GetString(2),
                                MaKhachHang = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                TenKhachHang = reader.GetString(4),
                                TongTien = reader.GetDecimal(5),
                                TrangThai = reader.GetString(6),
                                HinhThucThanhToan = reader.GetString(7)
                            };
                            danhSach.Add(hd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy danh sách hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return danhSach;
        }

        public int CreateHoaDon(int maTKLap, int? maKhachHang)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO QLHoaDon (NgayLap, MaTKLap, MaKhachHang, TongTien, TrangThai) OUTPUT INSERTED.MaHoaDon VALUES (GETDATE(), @MaTKLap, @MaKhachHang, 0, N'Chờ thanh toán')", conn))
                {
                    cmd.Parameters.AddWithValue("@MaTKLap", maTKLap);
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang ?? (object)DBNull.Value);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<DTO_HoaDon> TimKiemHoaDon(int? maHoaDon, DateTime? tuNgay, DateTime? denNgay, int? maKhachHang,
                                           string tenKhachHang, int? maTK, string trangThai, string hinhThucThanhToan,
                                           decimal? tongTienMin, decimal? tongTienMax)
        {
            List<DTO_HoaDon> danhSachHoaDon = new List<DTO_HoaDon>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", (object)maHoaDon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TuNgay", (object)tuNgay ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DenNgay", (object)denNgay ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaKhachHang", (object)maKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenKhachHang", (object)tenKhachHang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTK", (object)maTK ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TrangThai", (object)trangThai ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HinhThucThanhToan", (object)hinhThucThanhToan ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TongTienMin", (object)tongTienMin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TongTienMax", (object)tongTienMax ?? DBNull.Value);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTO_HoaDon hoaDon = new DTO_HoaDon()
                            {
                                MaHoaDon = reader.GetInt32(0),
                                NgayLap = reader.GetDateTime(1),
                                NguoiLap = reader.GetString(2),
                                MaTK = reader.GetInt32(3),
                                TenKhachHang = reader.IsDBNull(4) ? null : reader.GetString(4),
                                MaKhachHang = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                SDTKhachHang = reader.IsDBNull(6) ? null : reader.GetString(6),
                                TongTien = reader.GetDecimal(7),
                                TrangThai = reader.GetString(8),
                                HinhThucThanhToan = reader.GetString(9),
                                SoMatHang = reader.GetInt32(10),
                                TongSoLuong = reader.GetInt32(11)
                            };
                            danhSachHoaDon.Add(hoaDon);
                        }
                    }
                }
            }
            return danhSachHoaDon;
        }
      

        public DataTable GetVatLieu()
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaVatLieu, Ten, DonGiaBan, HinhAnh FROM QLVatLieu WHERE TrangThai = N'Hoạt động'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
        public DataTable GetChiTietHoaDon(int maHoaDon)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT cthd.MaVatLieu, vl.Ten, cthd.DonGia, cthd.SoLuong, vl.HinhAnh " +
                                   "FROM ChiTietHoaDon cthd " +
                                   "JOIN QLVatLieu vl ON cthd.MaVatLieu = vl.MaVatLieu " +
                                   "WHERE cthd.MaHoaDon = @MaHoaDon";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        Console.WriteLine($"Không có dữ liệu cho MaHoaDon: {maHoaDon}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi load chi tiết hóa đơn: " + ex.Message);
                }
            }
            return dt;
        }
        public DataTable CheckSoLuongTonKho(int maVatLieu)
        {
            string query = "SELECT SoLuong FROM QLVatLieu WHERE MaVatLieu = @MaVatLieu";
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public void ThemChiTietHoaDon(int maHoaDon, int maVatLieu, int soLuong)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_ThemChiTietHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable GetHoaDonById(int maHoaDon)
        {
            using (SqlConnection conn = DBConnect.GetConnection())

            {
                conn.Open();
                string query = @"
                    SELECT hd.MaHoaDon, hd.NgayLap, hd.MaKhachHang, hd.TongTien, hd.TrangThai, 
                           tk.TenDangNhap, kh.Ten
                    FROM QLHoaDon hd
                    LEFT JOIN QLTK tk ON hd.MaTKLap = tk.MaTK
                    LEFT JOIN QLKH kh ON hd.MaKhachHang = kh.MaKhachHang
                    WHERE hd.MaHoaDon = @MaHoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetChiTietHoaDonById(int maHoaDon)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT cthd.MaVatLieu, vl.Ten AS TenVatLieu, cthd.SoLuong, cthd.DonGia
                    FROM ChiTietHoaDon cthd
                    INNER JOIN QLVatLieu vl ON cthd.MaVatLieu = vl.MaVatLieu
                    WHERE cthd.MaHoaDon = @MaHoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public string HuyHoaDon(int maHoaDon, int nguoiCapNhat, string lyDoHuy)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_HuyHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                    cmd.Parameters.AddWithValue("@LyDoHuy", lyDoHuy);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Message"].ToString();
                        }
                        return "Hủy hóa đơn thất bại!";
                    }
                }
            }
        }
        public void CapNhatHoaDon(int maHoaDon, int? maKhachHang, string hinhThucThanhToan, string trangThai, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CapNhatHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HinhThucThanhToan", hinhThucThanhToan);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void XoaChiTietHoaDon(int maHoaDon, int maVatLieu)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_XoaChiTietHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTrangThaiHoaDon(int maHoaDon, string trangThai)
        {
            string query = "UPDATE QLHoaDon SET TrangThai = @TrangThai WHERE MaHoaDon = @MaHoaDon";
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@TrangThai", trangThai);
                    command.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void CapNhatChiTietHoaDon(int maHoaDon, int maVatLieu, int soLuong)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_CapNhatChiTietHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@MaVatLieu", maVatLieu);
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteHoaDonTam(int maHoaDon, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_XoaHoaDonTam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                    cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable LoadThongTinHoaDon(int maHoaDon)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT  hd.MaKhachHang, hd.TongTien,
                                   kh.Ten AS TenKhachHang
                            FROM QLHoaDon hd
                            LEFT JOIN QLKH kh ON hd.MaKhachHang = kh.MaKhachHang
                            WHERE hd.MaHoaDon = @MaHoaDon";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi load thông tin hóa đơn: " + ex.Message);
                }
            }
            return dt;
        }
    }
}
