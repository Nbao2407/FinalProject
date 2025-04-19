using System;
using System.Linq;
using DTO;
using DAL;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BUS
{
    public class BUS_Ncc
    {
        private DAL_NCcap DAL_Nccap = new DAL_NCcap();
        private readonly DAL_Order dalDonNhap;
        public List<DTO_Ncap> TimKiemNcc(string keyword)
        {
            return DAL_Nccap.TimKiemNcc(keyword);
        }

        public List<DTO_Ncap> GetAllNcc()
        {
            return DAL_Nccap.GetAllNcc();
        }
        public bool ToggleKhoaNcc(int maNcc, int maTK_NguoiThucHien, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (maNcc <= 0)
            {
                errorMessage = "Mã nhà cung cấp không hợp lệ.";
                return false;
            }
            if (maTK_NguoiThucHien <= 0)
            {
                errorMessage = "Mã người thực hiện không hợp lệ.";
                return false;
            }
            try
            {
                bool success = DAL_Nccap.ToggleKhoaNcc(maNcc, maTK_NguoiThucHien);
                return success; 
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi SQL khi Toggle khóa NCC {maNcc}: {sqlEx.Message}");
                errorMessage = sqlEx.Message; 
                return false;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi không xác định khi Toggle khóa NCC {maNcc}: {ex.Message}");
                errorMessage = "Đã xảy ra lỗi không mong muốn trong quá trình xử lý Nhà Cung Cấp.";
                return false;
            }
        }

        public bool TuChoiNcc(int maNcc, int maTK_NguoiTuChoi, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (maNcc <= 0)
            {
                errorMessage = "Mã nhà cung cấp không hợp lệ.";
                return false;
            }
            if (maTK_NguoiTuChoi <= 0)
            {
                errorMessage = "Mã người từ chối không hợp lệ.";
                return false;
            }

            try
            { 

                bool success = DAL_Nccap.TuChoiNcc(maNcc, maTK_NguoiTuChoi);
                return success; 
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi SQL khi từ chối NCC {maNcc}: {sqlEx.Message}");
                errorMessage = sqlEx.Message;
                return false;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi không xác định khi từ chối NCC {maNcc}: {ex.Message}");
                errorMessage = "Đã xảy ra lỗi không mong muốn trong quá trình từ chối Nhà Cung Cấp.";
                return false;
            }
        }
        public void ThemNCC(string tenNCC, string diaChi, string sdt, string email, int nguoiTao)
        {
            DAL_Nccap.ThemNCC(tenNCC, diaChi, sdt, email, nguoiTao);
        }

        public void CapNhatNCC(int maNCC, string tenNCC, string diaChi, string sdt, string email, int nguoiCapNhat)
        {
            DAL_Nccap.CapNhatNCC(maNCC, tenNCC, diaChi, sdt, email, nguoiCapNhat);
        }

        public void XoaNCC(int maNCC, int nguoiCapNhat)
        {
            DAL_Nccap.XoaNCC(maNCC, nguoiCapNhat);
        }

        public List<DTO_Ncap> LayDSNcc()
        {
            try
            {
                return DAL_Nccap.LayDanhSachNccDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong BUS khi gọi LayDanhSachNccDto: {ex.Message}");
                return new List<DTO_Ncap>();
            }
        }

        public bool DuyetNcc(int maNcc, int maTK_NguoiDuyet, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (maNcc <= 0)
            {
                errorMessage = "Mã nhà cung cấp không hợp lệ.";
                return false;
            }
            if (maTK_NguoiDuyet <= 0)
            {
                errorMessage = "Mã người duyệt không hợp lệ.";
                return false;
            }
            try
            {
                bool success = DAL_Nccap.DuyetNcc(maNcc, maTK_NguoiDuyet);
                return success;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi SQL khi duyệt NCC {maNcc}: {sqlEx.Message}");
                errorMessage = sqlEx.Message;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BUS_Ncc] Lỗi không xác định khi duyệt NCC {maNcc}: {ex.Message}");
                errorMessage = "Đã xảy ra lỗi không mong muốn trong quá trình duyệt Nhà Cung Cấp.";
                return false;
            }
        }

        public DTO_Ncap LayThongTinNCC(int maNCC)
        {
            if (maNCC <= 0)
            {
                return null; // Mã không hợp lệ
            }
            try
            {
                return DAL_Nccap.LayThongTinNCC(maNCC);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong BUS_Ncc.LayThongTinNCC cho mã {maNCC}: {ex.Message}");
                // Xử lý lỗi
                return null;
            }
        }

        public async Task<int?> TimHoacThemNccAsync(string tenNCC)
        {
            if (string.IsNullOrWhiteSpace(tenNCC))
            {
                Console.WriteLine("BUS Error: Tên nhà cung cấp không được để trống.");
                return null;
            }

            string trimmedTenNcc = tenNCC.Trim();

            try
            {
                DTO_Ncap existingNcc = DAL_Nccap.TimNccTheoTen(trimmedTenNcc);

                if (existingNcc != null)
                {
                    Console.WriteLine($"BUS: Tìm thấy NCC ID: {existingNcc.MaNCC} cho tên '{trimmedTenNcc}'");
                    return existingNcc.MaNCC;
                }
                else
                {
                    Console.WriteLine($"BUS: Không tìm thấy NCC '{trimmedTenNcc}'. Đang tạo mới...");
                    DTO_Ncap nccToAdd = new DTO_Ncap
                    {
                        TenNCC = trimmedTenNcc,
                        DiaChi = null,
                        SDT = null,
                        Email = null,
                        NguoiTao = CurrentUser.MaTK
                    };

                    int newMaNCC = await DAL_Nccap.ThemNccAsync(nccToAdd); // Quan trọng: Phải có hàm này trong DAL

                    if (newMaNCC > 0)
                    {
                        Console.WriteLine($"BUS: Đã tạo NCC mới thành công: ID {newMaNCC}, Tên '{nccToAdd.TenNCC}'");
                        return newMaNCC; // Trả về ID MỚI được trả từ DAL
                    }
                    else
                    {
                        Console.WriteLine($"*** BUS: Lỗi khi tạo NCC '{trimmedTenNcc}'. Hàm DAL trả về {newMaNCC}.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"*** BUS Lỗi nghiêm trọng trong TimHoacThemNccAsync cho tên '{trimmedTenNcc}': {ex.ToString()}");
                return null; // Báo hiệu thất bại do exception
            }
        }
    }
}