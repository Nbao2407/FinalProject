using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace BUS
{
    public class BUS_Order
    {
        private DAL_Order dal = new DAL_Order();

        public List<DTO_Order> GetAllOrder()
        {
            return dal.GetAllOrder();
        }
    
        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(
        DateOnly ngayNhap,
        int maNCC,
        int maTK,
        string ghiChu,
        List<DTO_Order> chiTietNhap,
        int nguoiCapNhat
        )
        {
            if (maNCC <= 0 || maTK <= 0 || nguoiCapNhat <= 0)
                return (false, "Thông tin mã NCC, tài khoản hoặc người cập nhật không hợp lệ.", 0);

            if (chiTietNhap == null || chiTietNhap.Count == 0)
                return (false, "Danh sách chi tiết nhập hàng không được để trống.", 0);

            foreach (var chiTiet in chiTietNhap)
            {
                if (chiTiet.MaVatLieu <= 0 || chiTiet.SoLuong <= 0 || chiTiet.GiaNhap < 0)
                    return (false, "Chi tiết nhập hàng chứa thông tin không hợp lệ.", 0);
            }

            string chiTietNhapJson = JsonConvert.SerializeObject(chiTietNhap);

            try
            {
                var dalResult = await dal.NhapHangAsync(ngayNhap, maNCC, maTK, ghiChu, chiTietNhapJson, nguoiCapNhat);

                return dalResult;
            }
            catch (SqlException ex)
            {
                return (false, $"Lỗi cơ sở dữ liệu khi nhập hàng: {ex.Message}", 0);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống khi nhập hàng: {ex.Message}", 0);
            }
        }
        public DTO_OrderDetail GetOrderDetailById(int? orderId)
        {
            if (orderId <= 0) return null; 

            return dal.GetOrderDetailById(orderId); 
        }
        public async Task<bool> UpdateOrderAsync(DTO_Order updatedHeader, List<DTO_ChiTietDonNhap> updatedDetails, int userId)
        {
            if (updatedHeader == null || updatedDetails == null || updatedHeader.MaDonNhap <= 0)
            {
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ để cập nhật đơn hàng.");
            }
            if (!updatedDetails.Any()) 
            {
                throw new InvalidOperationException("Đơn nhập hàng phải có ít nhất một chi tiết.");
            }
            if (updatedDetails.Any(d => d.SoLuong <= 0 || d.GiaNhap < 0))
            {
                throw new InvalidOperationException("Số lượng hoặc giá nhập trong chi tiết không hợp lệ.");
            }
            var jsonDetails = updatedDetails.Select(d => new {
                d.MaVatLieu,
                d.SoLuong,
                d.GiaNhap
            }).ToList();
            string chiTietJson = JsonConvert.SerializeObject(jsonDetails);

            try
            {
                bool result = await dal.UpdateOrderFromJsonAsync(updatedHeader, chiTietJson, userId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BUS UpdateOrderAsync: {ex.Message}");
                throw;
            }
        }

        // Giữ lại hàm gọi SP dùng TVP nếu muốn dùng thay thế
        // public bool UpdateOrderWithDetailsStoredProcedure(...) { ... }
    }
}
