using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
namespace DAL
{
    public class DAL_NCcap : DBConnect
    {
        private static readonly DAL_NCcap dal = new DAL_NCcap();
        private SqlConnection conn = DBConnect.GetConnection();
        private DTO_Ncap MapReaderToNcc(SqlDataReader reader)
        { 
            return new DTO_Ncap
            {
                MaNCC = reader.GetInt32(reader.GetOrdinal("MaNCC")),
                TenNCC = reader.GetString(reader.GetOrdinal("TenNCC")),
                DiaChi = reader.IsDBNull(reader.GetOrdinal("DiaChi")) ? null : reader.GetString(reader.GetOrdinal("DiaChi")),
                SDT = reader.IsDBNull(reader.GetOrdinal("SDT")) ? null : reader.GetString(reader.GetOrdinal("SDT")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                NguoiTao =reader.GetInt32(reader.GetOrdinal("NguoiTao"))
            };
        }
        public bool TuChoiNcc(int maNcc, int maTK_NguoiTuChoi)
        {
                using (SqlCommand command = new SqlCommand("sp_TuChoiNhaCungCap", conn)) 
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaNCC", maNcc);
                    command.Parameters.AddWithValue("@MaTK_NguoiTuChoi", maTK_NguoiTuChoi);
                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery(); 
                        Console.WriteLine($"[DAL_Ncc] Đã gọi sp_TuChoiNhaCungCap thành công cho MaNCC: {maNcc}");
                        return true; 
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi SQL khi từ chối NCC {maNcc}: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi khác khi từ chối NCC {maNcc}: {ex.Message}");
                        throw; 
                    }
                } 
        }

        public DTO_Ncap LayThongTinNCC(int maNCC) 
        {
            DTO_Ncap ncc = null; 
            string query = @"
                SELECT MaNCC, TenNCC, DiaChi, SDT, Email, NguoiTao, NgayTao, TrangThai
                FROM NCC
                WHERE MaNCC = @MaNCC
                AND TrangThai !='Bị khoá'";

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection()) 
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                ncc = MapReaderToNcc(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL.LayThongTinNCC for ID {maNCC}: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in DAL.LayThongTinNCC for ID {maNCC}: {ex.Message}");
            }
            return ncc; 
        }
        public bool DuyetNcc(int maNcc, int maTK_NguoiDuyet)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("sp_DuyetNhaCungCap", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaNCC", maNcc);
                    command.Parameters.AddWithValue("@MaTK_NguoiDuyet", maTK_NguoiDuyet);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine($"[DAL_Ncc] Đã gọi sp_DuyetNhaCungCap thành công cho MaNCC: {maNcc}");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi SQL khi duyệt NCC {maNcc}: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi khác khi duyệt NCC {maNcc}: {ex.Message}");
                        throw; 
                    }
                }
            } 
        }
        public List<DTO_Ncap> TimKiemNcc(string keyword)
        {
            List<DTO_Ncap> DanhsachNcc = new List<DTO_Ncap>();

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimKiemNCC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTO_Ncap ncc = new DTO_Ncap
                            {
                                MaNCC = reader.GetInt32(0),
                                TenNCC = reader.GetString(1),
                                SDT = reader.GetString(2),
                                Email = reader.GetString(3),
                                DiaChi = reader.GetString(4),
                                NguoiTao = reader.GetInt32(6),
                                NgayTao = reader.GetDateTime(7)
                            };
                            DanhsachNcc.Add(ncc);
                        }
                    }
                }
            }

            return DanhsachNcc;
        }

        public List<DTO_Ncap> GetAllNcc()
        {
            List<DTO_Ncap> DanhsachNcc = new List<DTO_Ncap>();
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT MaNCC, TenNCC, DiaChi, SDT, Email, NgayTao, NguoiTao,TrangThai FROM NCC", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DanhsachNcc.Add(new DTO_Ncap
                                {
                                    MaNCC = reader.GetInt32(0),
                                    TenNCC = reader.GetString(1),
                                    DiaChi = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    SDT = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    NgayTao = reader.GetDateTime(5),
                                    NguoiTao = reader.GetInt32(6),
                                    TrangThai = reader.GetString(7)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi trong DAL_NCcap.GetAllNcc: " + ex.Message);
            }
            return DanhsachNcc;
        }

        public string GetCreatorNameById(int creatorId)
        {
            string creatorName = null;

            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT TenDangNhap FROM QLTK WHERE MaTK = @MaTK AND TrangThai = N'Hoạt động'";

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@MaTK", creatorId);

                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            creatorName = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving creator name: {ex.Message}");
            }

            return creatorName ?? "Unknown";
        }
        public bool ToggleKhoaNcc(int maNcc, int maTK_NguoiThucHien)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("sp_ToggleKhoaNhaCungCap", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaNCC", maNcc);
                    command.Parameters.AddWithValue("@MaTK_NguoiThucHien", maTK_NguoiThucHien);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery(); 
                        Console.WriteLine($"[DAL_Ncc] Đã gọi sp_ToggleKhoaNhaCungCap thành công cho MaNCC: {maNcc}");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi SQL khi Toggle khóa NCC {maNcc}: {ex.Message}");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DAL_Ncc] Lỗi khác khi Toggle khóa NCC {maNcc}: {ex.Message}");
                        throw;
                    }
                } 
            } 
        }
        public void ThemNCC(string tenNCC, string diaChi, string sdt, string email, int nguoiTao)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ThemNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NguoiTao", nguoiTao);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void CapNhatNCC(int maNCC, string tenNCC, string diaChi, string sdt, string email, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_CapNhatNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void XoaNCC(int maNCC, int nguoiCapNhat)
        {
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_XoaNCC", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@NguoiCapNhat", nguoiCapNhat);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<DTO_Ncap> LayDanhSachNccDto()
        {
            List<DTO_Ncap> danhSachNcc = new List<DTO_Ncap>();
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT MaNCC, TenNCC FROM NCC WHERE TrangThai = N'Hoạt động'", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    DTO_Ncap ncc = new DTO_Ncap
                                    {
                                        MaNCC = reader.GetInt32(reader.GetOrdinal("MaNCC")),
                                        TenNCC = reader.GetString(reader.GetOrdinal("TenNCC"))
                                    };
                                    danhSachNcc.Add(ncc); // Thêm vào danh sách
                                }
                            }
                        }
                    } 
                } 
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in LayDanhSachNccDto: {sqlEx.Message}");
                // throw;
                return new List<DTO_Ncap>(); // Trả về danh sách rỗng khi lỗi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in LayDanhSachNccDto: {ex.Message}");
                // throw;
                return new List<DTO_Ncap>(); // Trả về danh sách rỗng khi lỗi
            }
            return danhSachNcc; // Trả về danh sách DTO
        }
        public async Task<int> ThemNccAsync(DTO_Ncap ncc)
        {
            string query = @"
            INSERT INTO NCC (TenNCC, DiaChi, SDT, Email, NguoiTao,Trangthai)
            OUTPUT INSERTED.MaNCC
            VALUES (@TenNCC, @DiaChi, @SDT, @Email, @NguoiTao,@TrangThai);";
            int newId = 0; 

            try
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNCC", ncc.TenNCC);
                    cmd.Parameters.AddWithValue("@DiaChi", (object)ncc.DiaChi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SDT", (object)ncc.SDT ?? DBNull.Value); 
                    cmd.Parameters.AddWithValue("@Email", (object)ncc.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NguoiTao", (object)ncc.NguoiTao ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TrangThai", (object)ncc.TrangThai ?? DBNull.Value);
                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        newId = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine($"DAL: Failed to retrieve new MaNCC after inserting '{ncc.TenNCC}'. ExecuteScalarAsync returned null/DBNull.");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL.ThemNccAsync for '{ncc.TenNCC}': {sqlEx.Message}");
                newId = 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in DAL.ThemNccAsync for '{ncc.TenNCC}': {ex.Message}");
                newId = 0; 
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    await conn.CloseAsync();
                }
            }
            return newId;
        }
        public DTO_Ncap TimNccTheoTen(string tenNcc)
        {
            DTO_Ncap ncc = null;

            string query = "SELECT MaNCC, TenNCC, DiaChi, SDT, TrangThai FROM NCC WHERE TenNCC COLLATE Latin1_General_CI_AS = @TenNCC COLLATE Latin1_General_CI_AS AND TrangThai =N'Hoạt động'";

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNCC", tenNcc);

                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ncc = new DTO_Ncap
                        {
                            MaNCC = reader.GetInt32(reader.GetOrdinal("MaNCC")),
                            TenNCC = reader.GetString(reader.GetOrdinal("TenNCC")),
                            DiaChi = reader.IsDBNull(reader.GetOrdinal("DiaChi")) ? null : reader.GetString(reader.GetOrdinal("DiaChi")),
                            SDT = reader.IsDBNull(reader.GetOrdinal("Sdt")) ? null : reader.GetString(reader.GetOrdinal("Sdt")),
                        };
                    }
                } 
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in DAL.TimNccTheoTen for '{tenNcc}': {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in DAL.TimNccTheoTen for '{tenNcc}': {ex.Message}");
            }
            finally
            {
                reader?.Close(); 
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close(); 
                }
            }
            return ncc; 
        }

        public async Task<int> TimHoacThemNccAsync(string tenNcc)
        {
            string trimmedTenNcc = tenNcc?.Trim();

            if (string.IsNullOrWhiteSpace(trimmedTenNcc))
            {
                Console.WriteLine("Tên NCC không được để trống khi tìm hoặc thêm.");
                return 0; 
            }

            try
            {
                DTO_Ncap existingNcc = dal.TimNccTheoTen(trimmedTenNcc);

                if (existingNcc != null)
                {
                    Console.WriteLine($"BUS: Tìm thấy NCC ID: {existingNcc.MaNCC} cho tên '{trimmedTenNcc}'");
                    return existingNcc.MaNCC;
                }
                else
                {
                    Console.WriteLine($"BUS: Không tìm thấy NCC '{trimmedTenNcc}'. Đang tạo mới...");
                    DTO_Ncap newNcc = new DTO_Ncap
                    {
                        TenNCC = trimmedTenNcc,
                        DiaChi = "Chưa cập nhật (từ Excel)", 
                        SDT = "N/A",                       
                    };

                    int newMaNCC = await dal.ThemNccAsync(newNcc);

                    if (newMaNCC > 0)
                    {
                        Console.WriteLine($"BUS: Đã tạo NCC mới thành công: ID {newMaNCC}, Tên '{newNcc.TenNCC}'");
                        return newMaNCC; 
                    }
                    else
                    {
                        Console.WriteLine($"*** BUS: Lỗi khi tạo NCC mới tên '{trimmedTenNcc}'. DAL trả về ID không hợp lệ ({newMaNCC}).");
                        return 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*** BUS: Lỗi nghiêm trọng trong TimHoacThemNccAsync cho tên '{trimmedTenNcc}': {ex.ToString()}");
                return 0; 
            }
        }
    }
}