using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BUS
{
    public class BUS_DonXuat
    {
        private DAL_DonXuat dalDonXuat = new DAL_DonXuat();

        public List<DTO_DonXuat> GetAllDonXuat()
        {
            return dalDonXuat.GetAllDonXuat();
        }


        public int ThemDonXuat(DTO_DonXuatInput donXuatInput, int? maKhoNguon, int nguoiThucHien)
        {
            if (donXuatInput == null) throw new ArgumentNullException(nameof(donXuatInput));
            if (donXuatInput.MaTK <= 0) throw new ArgumentException("Mã người lập không hợp lệ.", nameof(donXuatInput.MaTK));
            if (string.IsNullOrWhiteSpace(donXuatInput.LoaiXuat)) throw new ArgumentException("Loại xuất không được trống.", nameof(donXuatInput.LoaiXuat));
            if (donXuatInput.ChiTiet == null || !donXuatInput.ChiTiet.Any()) throw new ArgumentException("Chi tiết đơn xuất không được rỗng.", nameof(donXuatInput.ChiTiet));

            if (donXuatInput.LoaiXuat.Equals("Chuyển kho", StringComparison.OrdinalIgnoreCase))
            {
                if (!maKhoNguon.HasValue || maKhoNguon.Value <= 0)
                {
                    throw new ArgumentException("Mã kho nguồn là bắt buộc và phải hợp lệ khi chuyển kho.", nameof(maKhoNguon));
                }
            }
            if (donXuatInput.LoaiXuat.Equals("Xuất hàng", StringComparison.OrdinalIgnoreCase) && !donXuatInput.MaKhachHang.HasValue)
                throw new ArgumentException("Phải chọn khách hàng cho đơn xuất bán hàng.", nameof(donXuatInput.MaKhachHang));
            foreach (var item in donXuatInput.ChiTiet)
            {
                if (item.MaVatLieu <= 0) throw new ArgumentException("Chi tiết chứa Mã vật liệu không hợp lệ.", nameof(item.MaVatLieu));
                if (item.SoLuong <= 0) throw new ArgumentException($"Số lượng VL {item.MaVatLieu} phải > 0.", nameof(item.SoLuong));
                if (item.DonGia < 0) throw new ArgumentException($"Đơn giá VL {item.MaVatLieu} không được âm.", nameof(item.DonGia));
            }

            if (nguoiThucHien <= 0) throw new ArgumentException("Mã người thực hiện không hợp lệ.", nameof(nguoiThucHien));

            try
            {
                return dalDonXuat.ThemDonXuat(donXuatInput, maKhoNguon, nguoiThucHien);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"BUS ThemDonXuat SQL Error: {ex.Number} - {ex.Message}");
                throw new Exception($"Lỗi cơ sở dữ liệu khi thêm đơn xuất (SQL Error {ex.Number}). Vui lòng thử lại.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BUS ThemDonXuat Error: {ex.Message}");
                throw;
            }
        }

        public List<DTO_ChiTietDonXuat> GetChiTietDonXuat(int dispatchOrderId)
        {
            if (dispatchOrderId <= 0)
            {
                Console.WriteLine("BUS GetChiTietDonXuat: Invalid DispatchOrderID.");
                return new List<DTO_ChiTietDonXuat>();
            }

            try
            {
                return dalDonXuat.GetChiTietDonXuat(dispatchOrderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BUS calling DAL GetChiTietDonXuat: {ex.Message}");
                return new List<DTO_ChiTietDonXuat>();
            }
        }

        public DTO_DonXuat GetDonXuatById(int maDonXuat)
        {
            if (maDonXuat <= 0)
            {
                Console.WriteLine("BUS GetDonXuatById: MaDonXuat không hợp lệ.");
                return null;
            }

            try
            {
                return dalDonXuat.GetDonXuatById(maDonXuat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi BUS khi gọi DAL GetDonXuatById: {ex.Message}");
                return null;
            }
        }
        public bool HuyDonXuat(int maDonXuat, int nguoiHuy, string lyDo)
        {
            if (maDonXuat <= 0)
                throw new ArgumentException("Mã đơn xuất không hợp lệ.", nameof(maDonXuat));
            if (nguoiHuy <= 0)
                throw new ArgumentException("Mã người hủy không hợp lệ.", nameof(nguoiHuy));
            if (string.IsNullOrWhiteSpace(lyDo))
                throw new ArgumentException("Lý do hủy không được để trống.", nameof(lyDo));
            try
            {
                return dalDonXuat.HuyDonXuat(maDonXuat, nguoiHuy, lyDo);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"BUS HuyDonXuat SQL Error: {ex.Message}");
                throw new Exception($"Lỗi cơ sở dữ liệu khi hủy đơn xuất {maDonXuat}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BUS HuyDonXuat Error: {ex.Message}");
                throw;
            }
        }

        public List<DTO_DonXuat> TimKiemDonXuat(string keyword, string trangThai)
        {
            return dalDonXuat.TimKiemDonXuat(keyword, trangThai);
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
                return dalDonXuat.GetSoLuongTonKho(maVatLieu, maKho);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi BUS khi gọi DAL GetSoLuongTonKho: {ex.Message}");
                return 0;
            }
        }
    }
}