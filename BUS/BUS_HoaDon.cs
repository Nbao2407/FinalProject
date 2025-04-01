using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace BUS
{
    public class BUS_HoaDon
    {
        private DAL_HoaDon dalHoaDon = new DAL_HoaDon();

        public List<DTO_HoaDon> LayTatCaHoaDon()
        {
            return dalHoaDon.LayTatCaHoaDon();
        }
        public DataTable LoadVatLieu()
        {
            return dalHoaDon.GetVatLieu();
        }
        public void DeleteHoaDonTam(int maHoaDon,int nguoiCapNhat)
        {
            dalHoaDon.DeleteHoaDonTam(maHoaDon,nguoiCapNhat);
        }
        public DataTable LoadChiTietHoaDon(int maHoaDon)
        {
            return dalHoaDon.GetChiTietHoaDon(maHoaDon);
        }

        public void AddChiTietHoaDon(int maHoaDon, int maVatLieu, int soLuong)
        {
            if (soLuong <= 0)
                throw new Exception("Số lượng phải lớn hơn 0!");
            dalHoaDon.ThemChiTietHoaDon(maHoaDon, maVatLieu, soLuong);
        }
        public int CreateHoaDon(int maTKLap, int? maKhachHang)
        {
           return dalHoaDon.CreateHoaDon(maTKLap, maKhachHang);
        }
        public DataTable LoadThongTinHoaDon(int maHoaDon)
        {
            return dalHoaDon.LoadThongTinHoaDon(maHoaDon);
        }
        public Image ConvertByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null)
            {
                Bitmap defaultImage = new Bitmap(60, 60);
                using (Graphics g = Graphics.FromImage(defaultImage))
                {
                    g.Clear(Color.Red);
                }
                return defaultImage;
            }
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
       public void UpdateTHoaDon(int maHoaDon, string trangThai)
        {
            dalHoaDon.UpdateTrangThaiHoaDon(maHoaDon, trangThai);
        }
        public void UpdateHoaDon(int maHoaDon, int? maKhachHang, string hinhThucThanhToan, string trangThai, int nguoiCapNhat)
        {
            dalHoaDon.CapNhatHoaDon(maHoaDon, maKhachHang, hinhThucThanhToan, trangThai, nguoiCapNhat);
        }

        public void DeleteChiTietHoaDon(int maHoaDon, int maVatLieu)
        {
            dalHoaDon.XoaChiTietHoaDon(maHoaDon, maVatLieu);
        }

        public void UpdateChiTietHoaDon(int maHoaDon, int maVatLieu, int soLuong)
        {
            if (soLuong <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0!");
            }
            dalHoaDon.CapNhatChiTietHoaDon(maHoaDon, maVatLieu, soLuong);
        }
        public DTO_HoaDon GetHoaDonById(int maHoaDon)
        {
            var danhSach = dalHoaDon.LayTatCaHoaDon();
            return danhSach.FirstOrDefault(hd => hd.MaHoaDon == maHoaDon);

        }
        public DataTable GetChiTietHoaDon(int maHoaDon)
        {
            return dalHoaDon.GetChiTietHoaDon(maHoaDon);
        }
      
        public string CancelHoaDon(int maHoaDon, int nguoiCapNhat, string lyDoHuy)
        {
            return dalHoaDon.HuyHoaDon(maHoaDon, nguoiCapNhat, lyDoHuy);
        }
        public DataTable CheckSoLuongTonKho(int maVatLieu)
        {
            return dalHoaDon.CheckSoLuongTonKho(maVatLieu);
        }
    }
}
