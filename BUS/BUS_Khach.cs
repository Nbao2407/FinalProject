using System;
using System.Linq;
using DTO;
using DAL;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
namespace BUS
{

    public class BUS_Khach
    {
        private QLKH dalKH = new QLKH();

        public List<DTO_Khach> TimKiemKhachHang(string keyword)
        {
            return dalKH.TimKiemKhachHang(keyword);
        }
        public List<DTO_Khach> LayDanhSachKhach()
        {
            return dalKH.GetAllKhach();
        }
        public void ThemKhachHang(DTO_Khach khach)
        {
           dalKH.ThemKhachHang(khach);
        }
        public async Task<bool> SuaKhachHang(DTO_Khach khach)
        {
            try
            {
                if (khach == null)
                {
                    throw new ArgumentException("Thông tin khách hàng không hợp lệ");
                }

                bool result = await dalKH.SuaKhachHang(khach);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật khách hàng: {ex.Message}", ex);
            }
        }
        public void XoaKhachHang(int maKhachHang,int nguoiCapNhat)
        {
            dalKH.XoaKhachHang(maKhachHang, nguoiCapNhat);
        }
        public DTO_Khach GetCustomerById(int maKhachHang)
        {
            return dalKH.GetCustomerById(maKhachHang);
        }
    }

}
