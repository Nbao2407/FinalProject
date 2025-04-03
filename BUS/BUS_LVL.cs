using DAL;
using DTO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BUS
{
    public class BUS_LVL
    {
        private DAL_LVL dalLoaiVatLieu = new DAL_LVL();

        public List<DTO_LVL> GetAllLoaiVatLieu()
        {
            return dalLoaiVatLieu.LayTatCaLVL();
        }
        public List<DTO_LVL> SearchLoaiVatLieu(string keyword)
        {
            return dalLoaiVatLieu.SearchProductTypes(keyword);
        }
        public void Update(int maLoai, string tenLoai)
        {
            if (string.IsNullOrWhiteSpace(tenLoai))
                throw new Exception("Tên loại vật liệu không được để trống!");
            dalLoaiVatLieu.Update(maLoai, tenLoai);
        }

        public void Delete(int maLoai)
        {
            if (dalLoaiVatLieu.IsLoaiVatLieuInUse(maLoai))
                throw new Exception("Không thể xóa loại vật liệu vì đang được sử dụng! Hệ Thống sẽ chuyển sang ngừng sử dụng loại vật liệu này");
            dalLoaiVatLieu.UpdateTrangThai(maLoai, "Ngừng sử dụng");
        }                                           
        public void Addd(string tenLoai)
        {
            if (string.IsNullOrWhiteSpace(tenLoai))
                throw new Exception("Tên loại vật liệu không được để trống!");

            if (dalLoaiVatLieu.IsTenLoaiExists(tenLoai))
                throw new Exception("Tên loại vật liệu đã tồn tại!");

            dalLoaiVatLieu.Add(tenLoai);
        }
        public int DeleteLoaiVatLieu(int maLoai)
        {
            return dalLoaiVatLieu.Deltele2(maLoai);
        }
    }
}