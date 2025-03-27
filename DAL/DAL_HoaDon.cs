using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace DAL
{
    public  class DAL_HoaDon
    {
        public List<DTO_HoaDon> LayTatCaHoaDon()
        {
            List<DTO_HoaDon> danhSach = new List<DTO_HoaDon>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT hd.MaHoaDon, 
                   hd.NgayLap, 
                   tk.TenDangNhap AS N'Người Lập', 
                   COALESCE(kh.Ten, N'Khách lẻ') + ' (' + COALESCE(CAST(kh.MaKhachHang AS NVARCHAR), N'Không có') + ')' AS N'Khách Hàng',
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
                        int maHoaDon = reader.GetInt32(0);
                        DateTime ngayLap = reader.GetDateTime(1);
                        string tenTaiKhoan = reader.GetString(2);
                        string tenKhachHang = reader.IsDBNull(3) ? "Khách lẻ" : reader.GetString(3);
                        decimal tongTien = reader.GetDecimal(4);
                        string trangThai = reader.GetString(5);
                        string hinhThucThanhToan = reader.GetString(6);

                        DTO_HoaDon hd = new DTO_HoaDon
                        {
                            MaHoaDon = maHoaDon,
                            NgayLap = ngayLap,
                            NguoiLap = tenTaiKhoan,
                            TenKhachHang = tenKhachHang,
                            TongTien = tongTien,
                            TrangThai = trangThai,
                            HinhThucThanhToan = hinhThucThanhToan
                        };
                        danhSach.Add(hd);
                    }

                }
            }
            return danhSach;
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
    }


}
