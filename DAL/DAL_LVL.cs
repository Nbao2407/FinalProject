using DAL;
using DTO;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
namespace DAL;
public class DAL_LVL : DBConnect
{

    public List<DTO_LVL> LayTatCaLVL()
    {
        List<DTO_LVL> danhSach = new List<DTO_LVL>();
        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            string query = @"
                        SELECT lvl.MaLoaiVatLieu, lvl.TenLoai,lvl.TrangThai
                        FROM QLLoaiVatLieu lvl  ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DTO_LVL lvatLieu = new DTO_LVL(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    );
                    danhSach.Add(lvatLieu);
                }
            }

        }
        return danhSach;
    }
    public List<DTO_LVL> SearchProductTypes(string keyword)
    {
        List<DTO_LVL> danhSachL = new List<DTO_LVL>();

        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("sp_TimKiemLoaiVatLieu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", keyword);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSachL.Add(new DTO_LVL
                        {
                            MaLoaiVatLieu = reader.GetInt32(0),
                            TenLoai = reader.GetString(1),
                            TrangThai = reader.GetString(2)
                        });
                    }
                }
            }
        }
        return danhSachL;
    }

    public void Add(string tenLoai)
    {
        using (SqlConnection conn = DBConnect.GetConnection())

        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO QLLoaiVatLieu (TenLoai) VALUES (@TenLoai)", conn);
            cmd.Parameters.AddWithValue("@TenLoai", tenLoai);
            cmd.ExecuteNonQuery();
        }
    }

    public void Update(int maLoai, string tenLoai)
    {
        using (SqlConnection conn = DBConnect.GetConnection())

        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE QLLoaiVatLieu SET TenLoai = @TenLoai WHERE MaLoaiVatLieu = @MaLoai", conn);
            cmd.Parameters.AddWithValue("@MaLoai", maLoai);
            cmd.Parameters.AddWithValue("@TenLoai", tenLoai);
            cmd.ExecuteNonQuery();
        }
    }

    public bool IsLoaiVatLieuInUse(int maLoai)
    {
        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM QLVatLieu WHERE Loai = @MaLoai";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaLoai", maLoai);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
    public void UpdateTrangThai(int maLoai, string trangThai)
    {
        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            string query = "UPDATE QLLoaiVatLieu SET TrangThai = @TrangThai WHERE MaLoaiVatLieu = @MaLoai";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TrangThai", trangThai);
            cmd.Parameters.AddWithValue("@MaLoai", maLoai);
            cmd.ExecuteNonQuery();
        }
    }
    public void Delete(int maLoai)
    {
        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            string query = "DELETE FROM QLLoaiVatLieu WHERE MaLoaiVatLieu = @MaLoai";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MaLoai", maLoai);
            cmd.ExecuteNonQuery();
        }
    }
    public bool IsTenLoaiExists(string tenLoai, int? maLoai = null)
    {
        using (SqlConnection conn = DBConnect.GetConnection())
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM QLLoaiVatLieu WHERE TenLoai = @TenLoai AND (@MaLoai IS NULL OR MaLoaiVatLieu != @MaLoai)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TenLoai", tenLoai);
            cmd.Parameters.AddWithValue("@MaLoai", (object)maLoai ?? DBNull.Value);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
    public bool Disable(int MaLoaiVatLieu , int nguoiCapNhat)
    {
        try
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "UPDATE QLLoaiVatLieu SET Trangthai = N'Ngừng sử dụng' WHERE MaLoaiVatLieu = @MaLoaiVatLieu";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@MaLoaiVatLieu", MaLoaiVatLieu);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Number == 50003)
                throw new Exception("loại vật liệu không tồn tại!");
            else
                throw new Exception("Lỗi khi disable loại vật liệu: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi hệ thống: " + ex.Message);
        }
    }

    public bool Active(int MaLoaiVatLieu, int nguoiCapNhat)
    {
        try
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "UPDATE QLLoaiVatLieu SET Trangthai = N'Hoạt động' WHERE MaLoaiVatLieu = @MaLoaiVatLieu";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@MaLoaiVatLieu", MaLoaiVatLieu);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Number == 50003)
                throw new Exception("loại vật liệu không tồn tại!");
            else
                throw new Exception("Lỗi khi disable loại vật liệu " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Lỗi hệ thống: " + ex.Message);
        }
    }
   public int Deltele2(int MaLoaiVatLieu)
    {
        try
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM QLLoaiVatLieu WHERE MaLoaiVatLieu = @MaLoaiVatLieu";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLoaiVatLieu", MaLoaiVatLieu);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Number == 50003)
                throw new Exception("loại vật liệu không tồn tại!");
            else
                throw new Exception("Lỗi khi xóa loại vật liệu: " + ex.Message);
        }
    }
}


