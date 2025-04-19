using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_DonXuat
    {
        public int MaDonXuat { get; set; }
        public DateTime NgayXuat { get; set; }
        public int MaTK { get; set; }
        public string LoaiXuat { get; set; }
        public string TenNguoiLap { get; set; }
        public int? MaHoaDon { get; set; }
        public int? MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? NguoiCapNhat { get; set; }
        public string TenNguoiCapNhat { get; set; }
        public int MaCTDX { get; set; }
        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public int Makhonguon { get; set; }
        public int? MaKhoDich { get; set; }
        public List<DTO_ChiTietDonXuat> ChitietDonXuat { get; set; }
        public DTO_DonXuat ()
        {
            ChitietDonXuat = new List<DTO_ChiTietDonXuat> ();
        }
    }

    public class DTO_ChiTietDonXuat
    {
        public int MaCTDX { get; set; }
        public int MaDonXuat { get; set; }
        public int MaVatLieu { get; set; }
        public string TenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public int MaKhoNguon { get; set; }
        public string TenKho { get; set; }
        public string TenKhoDich { get;set; }
        public string TenKhoNguon { get; set; }

        public int? MaKhoDich { get; set; }  
        public decimal ThanhTien => SoLuong * DonGia;
        }
         


        public class DTO_ChiTietXuatInput
    {
        public int MaVatLieu { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }

    public class DTO_DonXuatInput
    {
        public DateTime NgayXuat { get; set; }
        public int MaTK { get; set; }
        public string LoaiXuat { get; set; }
        public int? MaHoaDon { get; set; }
        public int? MaKhachHang { get; set; }
        public string GhiChu { get; set; }
        public List<DTO_ChiTietXuatInput> ChiTiet { get; set; }

        public DTO_DonXuatInput()
        {
            ChiTiet = new List<DTO_ChiTietXuatInput>();
            NgayXuat = DateTime.Now.Date;
        }
    }
}