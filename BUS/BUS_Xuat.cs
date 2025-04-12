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

        public List<DTO_ChiTietDonXuat> GetChiTietDonXuat(int maDonXuat)
        {
            if (maDonXuat <= 0)
            {
                Console.WriteLine("BUS GetChiTietDonXuat: MaDonXuat không hợp lệ.");
                return new List<DTO_ChiTietDonXuat>();
            }
            return dalDonXuat.GetChiTietDonXuat(maDonXuat);
        }

        public int ThemDonXuat(DTO_DonXuatInput donXuatInput, int nguoiThucHien)
        {
            if (donXuatInput == null)
                throw new ArgumentNullException(nameof(donXuatInput), "Thông tin đơn xuất không được rỗng.");
            if (donXuatInput.MaTK <= 0)
                throw new ArgumentException("Mã người lập không hợp lệ.", nameof(donXuatInput.MaTK));
            if (donXuatInput.ChiTiet == null || !donXuatInput.ChiTiet.Any())
                throw new ArgumentException("Chi tiết đơn xuất không được rỗng.", nameof(donXuatInput.ChiTiet));

            foreach (var item in donXuatInput.ChiTiet)
            {
                if (item.MaVatLieu <= 0)
                    throw new ArgumentException("Chi tiết chứa Mã vật liệu không hợp lệ.", nameof(item.MaVatLieu));
                if (item.SoLuong <= 0)
                    throw new ArgumentException($"Số lượng cho vật liệu {item.MaVatLieu} phải lớn hơn 0.", nameof(item.SoLuong));
                if (item.DonGia < 0)
                    throw new ArgumentException($"Đơn giá cho vật liệu {item.MaVatLieu} không được âm.", nameof(item.DonGia));
            }
            if (string.IsNullOrWhiteSpace(donXuatInput.LoaiXuat))
                throw new ArgumentException("Loại xuất không được để trống.", nameof(donXuatInput.LoaiXuat));
            List<string> validTypes = new List<string> { "Xuất hàng", "Chuyển kho" };
            if (!validTypes.Contains(donXuatInput.LoaiXuat, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException($"Loại xuất '{donXuatInput.LoaiXuat}' không hợp lệ.", nameof(donXuatInput.LoaiXuat));
            if (donXuatInput.LoaiXuat.Equals("Xuất hàng", StringComparison.OrdinalIgnoreCase) && !donXuatInput.MaKhachHang.HasValue)
                throw new ArgumentException("Phải chọn khách hàng cho đơn xuất bán hàng.", nameof(donXuatInput.MaKhachHang));
            if (!donXuatInput.LoaiXuat.Equals("Xuất hàng", StringComparison.OrdinalIgnoreCase) && donXuatInput.MaKhachHang.HasValue)
                throw new ArgumentException("Không cần chọn khách hàng cho loại xuất này.", nameof(donXuatInput.MaKhachHang));
            if (nguoiThucHien <= 0)
                throw new ArgumentException("Mã người thực hiện không hợp lệ.", nameof(nguoiThucHien));
            try
            {
                return dalDonXuat.ThemDonXuat(donXuatInput, nguoiThucHien);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"BUS ThemDonXuat SQL Error: {ex.Message}");
                throw new Exception("Lỗi cơ sở dữ liệu khi thêm đơn xuất. Vui lòng thử lại.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BUS ThemDonXuat Error: {ex.Message}");
                throw;
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