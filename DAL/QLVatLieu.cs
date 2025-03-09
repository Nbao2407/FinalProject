using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
namespace DAL
{
    public class QLVatLieu : DBConnect
    {
        public List<DTO_VatLieu> LayTatCaVatLieu()
        {
            List<DTO_VatLieu> danhSach = new List<DTO_VatLieu>();
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = @"
                            SELECT vl.MaVatLieu, vl.Ten, lv.TenLoai, vl.DonGiaNhap, vl.DonGiaBan, vl.DonViTinh, vl.SoLuong
                            FROM QLVatLieu vl
                            JOIN QLLoaiVatLieu lv ON vl.Loai = lv.MaLoaiVatLieu";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DTO_VatLieu vatLieu = new DTO_VatLieu(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDecimal(3),
                            reader.GetDecimal(4),
                            reader.GetString(5),
                            reader.GetInt32(6)
                        );
                        danhSach.Add(vatLieu);
                    }
                }

            }
            return danhSach;
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
                                SoLuong = reader.GetInt32(6)

                            });
                        }
                    }
                }
            }
            return danhSach;
        }
    }
}

