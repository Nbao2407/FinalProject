using DAL;
using DTO;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using Microsoft.Data.SqlClient;
namespace DAL;
public class DAL_LVL : DBConnect
{

    // Lấy danh sách loại vật liệu
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

}

