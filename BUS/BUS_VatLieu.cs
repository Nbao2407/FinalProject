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
        public DataTable LayVatLieuTheoMa2(int maVatLieu)
        {
            return dalVatLieu.LayVatLieuTheoMa2(maVatLieu);
        }
        public DTO_VatLieu TimVatLieuTheoTenChinhXac(string tenVatLieu)
        {
            if (string.IsNullOrWhiteSpace(tenVatLieu))
            {
                return null; 
            }
            try
            {
                return dalVatLieu.TimVatLieuTheoTenChinhXac(tenVatLieu);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong BUS_VatLieu.TimVatLieuTheoTenChinhXac: {ex.Message}");
                return null; 
            }
        }

        public void ThemVatLieu(string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, int maKho, byte[] hinhAnh, string ghiChu,int NguoiTao)
        {
            if (string.IsNullOrEmpty(ten) || donGiaNhap < 0 || donGiaBan < 0 || string.IsNullOrEmpty(donViTinh))
                throw new Exception("Dữ liệu không hợp lệ!");
            dalVatLieu.ThemVatLieu(ten, loai, donGiaNhap, donGiaBan, donViTinh, maKho, hinhAnh, ghiChu,NguoiTao);
        }

        public void CapNhatVatLieu(int maVatLieu, string ten, int loai, decimal donGiaNhap, decimal donGiaBan, string donViTinh, byte[] hinhAnh, string ghiChu, int nguoiCapNhat)
        {
            if (maVatLieu <= 0 || string.IsNullOrEmpty(ten) || donGiaNhap < 0 || donGiaBan < 0 || string.IsNullOrEmpty(donViTinh))
                throw new Exception("Dữ liệu không hợp lệ!");
            dalVatLieu.CapNhatVatLieu(maVatLieu, ten, loai, donGiaNhap, donGiaBan, donViTinh, hinhAnh, ghiChu, nguoiCapNhat);
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
        public string LayTenKhoTheoMa(int maKho)
        {
            DataTable dt = dalVatLieu.LayTenKhoTheoMa(maKho);
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
