using System;
using System.Linq;
using DTO;
using DAL;

namespace BUS
{
    public class BUS_TKe
    {
        private DAL_Tke thongKeDAL = new DAL_Tke();

        public List<DTO_Tke> GetThongKeDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            return thongKeDAL.GetThongKeDoanhThu(tuNgay, denNgay);
        }

        public decimal TinhTongDoanhThu(DateTime? tuNgay, DateTime? denNgay)
        {
            var danhSach = thongKeDAL.GetThongKeDoanhThu(tuNgay, denNgay);
            decimal tongDoanhThu = 0;

            foreach (var item in danhSach)
            {
                if (item.TrangThai == "Đã thanh toán")
                {
                    tongDoanhThu += item.TongTien;
                }
            }
            return tongDoanhThu;
        }
        public List<TopProductDTO> GetTopProducts(DateTime? tuNgay, DateTime? denNgay)
        {
            return thongKeDAL.GetTopProducts(tuNgay, denNgay);
        }

        public List<UnderstockDTO> GetUnderstock()
        {
            return thongKeDAL.GetUnderstock();
        }

        public int GetSoNhaCungCap()
        {
            return thongKeDAL.GetSoNhaCungCap();
        }
    }
}
