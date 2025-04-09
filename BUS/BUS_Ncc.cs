using System;
using System.Linq;
using DTO;
using DAL;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Data;

namespace BUS
{
    public class BUS_Ncc
    {
        private DAL_NCcap DAL_Nccap = new DAL_NCcap();

         public List<DTO_Ncap> TimKiemNcc(string keyword)
            {
                return DAL_Nccap.TimKiemNcc(keyword);
            }

            public List<DTO_Ncap> GetAllNcc()
            {
                return DAL_Nccap.GetAllNcc();
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
                        NguoiTao =CurrentUser.MaTK
                        
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
