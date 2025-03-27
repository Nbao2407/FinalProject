using System;
using System.Linq;
using DTO;
using DAL;
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
        public void SuaKhachHang(DTO_Khach khach)
        {
            dalKH.SuaKhachHang(khach);
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
