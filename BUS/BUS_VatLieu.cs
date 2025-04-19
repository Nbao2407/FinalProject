using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using DTO;

namespace BUS
{
    public class BUS_VatLieu
    {
        private QLVatLieu dalVatLieu = new QLVatLieu();

        public List<DTO_VatLieu> GetAllVatLieu()
        {
            var danhSachVatLieu = dalVatLieu.LayTatCaVatLieu();

            foreach (var item in danhSachVatLieu)
            {
                Console.WriteLine($"MaVatLieu: {item.MaVatLieu}, Ten: {item.Ten}, Loai: {item.Loai}");
            }

            return danhSachVatLieu;
        }
        public DataTable LayVatLieuTheoMa(int maVatLieu)
        {
            return dalVatLieu.LayVatLieuTheoMa(maVatLieu);
        }
   
        public void ThemVatLieu(string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, int maKho, byte[] hinhAnh, string ghiChu,int NguoiTao)
        {
            if (string.IsNullOrEmpty(ten) || donGiaNhap < 0 || donGiaBan < 0 || string.IsNullOrEmpty(donViTinh))
                throw new Exception("Dữ liệu không hợp lệ!");
            dalVatLieu.ThemVatLieu(ten, loai, donGiaNhap, donGiaBan, donViTinh, maKho, hinhAnh, ghiChu,NguoiTao);
        }
        public int GetSoLuongTonKho(int maVatLieu, int maKho)
        {
            if (maVatLieu <= 0 || maKho <= 0)
            {
                Console.WriteLine("BUS GetSoLuongTonKho: Mã vật liệu hoặc Mã kho không hợp lệ.");
                return 0;
            }

            try
            {
                return dalVatLieu.GetSoLuongTonKho(maVatLieu, maKho);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi BUS khi gọi DAL GetSoLuongTonKho: {ex.Message}");
                return 0;
            }
        }
        public void CapNhatVatLieu(int maVatLieu, string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, byte[] hinhAnh, string ghiChu, int nguoiCapNhat)
        {
            if (maVatLieu <= 0 || string.IsNullOrEmpty(ten) || donGiaNhap < 0 || donGiaBan < 0 || string.IsNullOrEmpty(donViTinh))
                throw new Exception("Dữ liệu không hợp lệ!");
            dalVatLieu.CapNhatVatLieu(maVatLieu, ten, loai, donGiaNhap, donGiaBan, donViTinh, hinhAnh, ghiChu, nguoiCapNhat);
        }
        public List<DTO_VatLieu> TimKiemVatLieuTheoKho(int maKho, string keyword)
        {
            if (maKho <= 0)
            {
                Console.WriteLine("BUS_VatLieu.TimKiemVatLieuTheoKho: Mã kho không hợp lệ.");
                return new List<DTO_VatLieu>(); 
            }

            string processedKeyword = keyword?.Trim() ?? "";

            try
            {
                return dalVatLieu.TimKiemVatLieuTheoKho(maKho, processedKeyword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BUS_VatLieu.TimKiemVatLieuTheoKho: {ex.Message}");
                return new List<DTO_VatLieu>(); 
            }
        }
        public DTO_Kho TimkhotheoTen(string tenkho)
        {
            if (string.IsNullOrWhiteSpace(tenkho))
            {
                return null; 
            }

            try
            {
                var khoList = dalVatLieu.TimKhoTheoTen(tenkho);
                return khoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void XoaVatLieu(int maVatLieu, int nguoiCapNhat)
        {
            if (maVatLieu <= 0)
                throw new Exception("Mã vật tư không hợp lệ!");
            dalVatLieu.XoaVatLieu(maVatLieu, nguoiCapNhat);
        }
        public DataTable LayDanhSachLoaiVatLieu()
        {
            return dalVatLieu.LayDanhSachLoaiVatLieu();
        }
        public DataTable LayDanhSachDVT()
        {
            return dalVatLieu.LayDanhSachDonViTinh();
        }
        public DataTable LayDanhSachKho()
        {
            return dalVatLieu.LayDanhSachKho();
        }
        public List<DTO_Kho> LayDanhSachKhokho()
        {
            return dalVatLieu.LayDanhSachKhokho();
        }
        public string LayTenKhoTheoMa(int maKho)
        {
            DataTable dt = dalVatLieu.LayTenKhoTheoMa(maKho);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TenKho"].ToString();
            return string.Empty;
        }
        public string LayTenKhoTheoten(string ten)
        {
            DataTable dt = dalVatLieu.LayTenKhoTheoTen(ten);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TenKho"].ToString();
            return string.Empty;
        }

        public string LayTenLoaiTheoMa(int maLoai)
        {
            DataTable dt = dalVatLieu.LayTenLoaiTheoMa(maLoai);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TenLoai"].ToString();
            return string.Empty;
        }
        public string LayTenNgTaoTheoMa(int maTk)
        {
            DataTable dt = dalVatLieu.LayTenNgTaoTheoMa(maTk);
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["TenDangNhap"].ToString();
            return string.Empty;
        }
    }
}
