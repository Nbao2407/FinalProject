using DAL;
using DTO;
using System.Collections.Generic;

namespace BUS
{
    public class BUS_LVL
    {
        private DAL_LVL dalLoaiVatLieu = new DAL_LVL();

        public List<DTO_LVL> GetAllLoaiVatLieu()
        {
            return dalLoaiVatLieu.LayTatCaLVL();
        }

        //public bool ThemLoaiVatLieu(string tenLoai, string trangThai)
        //{
        //    return dalLoaiVatLieu.ThemLoaiVatLieu(tenLoai, trangThai);
        //}

        //// Sửa loại vật liệu
        //public bool SuaLoaiVatLieu(int maLoai, string tenLoai, string trangThai)
        //{
        //    return dalLoaiVatLieu.SuaLoaiVatLieu(maLoai, tenLoai, trangThai);
        //}

        //// Xóa loại vật liệu
        //public bool XoaLoaiVatLieu(int maLoai)
        //{
        //    return dalLoaiVatLieu.XoaLoaiVatLieu(maLoai);
        //}

        //// Tìm kiếm loại vật liệu
        //public List<DTO_LVL> TimKiemLoaiVatLieu(string keyword)
        //{
        //    return dalLoaiVatLieu.TimKiemLoaiVatLieu(keyword);
        //}
    }
}