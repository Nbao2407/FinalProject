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
    }

}
