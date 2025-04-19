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
        private static readonly List<string> _validStatuses = new List<string> { "Chờ duyệt", "Hoàn thành",  "Đã Hủy","Từ chối" };

        public async Task<(bool Success, string Message, int MaDonNhap)> NhapHangAsync(
        DateOnly ngayNhap,
        int maNCC,
        int maTK,
        string ghiChu,
        List<DTO_Order> chiTietNhap,
        int nguoiCapNhat,
        int makho
        )
        {
            if (maNCC <= 0 || maTK <= 0 || nguoiCapNhat <= 0)
                return (false, "Thông tin mã NCC, tài khoản hoặc người cập nhật không hợp lệ.", 0);

            if (chiTietNhap == null || chiTietNhap.Count == 0)
                return (false, "Danh sách chi tiết nhập hàng không được để trống.", 0);

            foreach (var chiTiet in chiTietNhap)
            {
                    return (false, "Chi tiết nhập hàng chứa thông tin không hợp lệ.", 0);
            }

            string chiTietNhapJson = JsonConvert.SerializeObject(chiTietNhap);

            try
            {
                var dalResult = await dal.NhapHangAsync(ngayNhap, maNCC, maTK, ghiChu, chiTietNhapJson, nguoiCapNhat,makho);

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
        public bool DuyetDonNhap(int maDonNhap, int maTK_NguoiDuyet, out string errorMessage)
        {
            errorMessage = string.Empty; 

            if (maDonNhap <= 0)
            {
                errorMessage = "Mã đơn nhập không hợp lệ.";
                return false;
            }
            if (maTK_NguoiDuyet <= 0)
            {
                errorMessage = "Mã người duyệt không hợp lệ.";
                return false;
            }
            try
            {
                bool success = dal.DuyetDonNhap(maDonNhap, maTK_NguoiDuyet);
                return success;
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine($"[BUS_DonNhap] Lỗi SQL khi duyệt đơn {maDonNhap}: {sqlEx.Message}");
                errorMessage = sqlEx.Message;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BUS_DonNhap] Lỗi không xác định khi duyệt đơn {maDonNhap}: {ex.Message}");
                errorMessage = "Đã xảy ra lỗi không mong muốn trong quá trình duyệt đơn.";
                return false;
            }
        }

        public async Task<bool> CapNhatTrangThaiDonNhapAsync(int maDonNhap, string trangThaiMoi, int maNguoiCapNhat)
        {
            if (maDonNhap <= 0)
            {
                Console.WriteLine("Mã đơn nhập không hợp lệ.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(trangThaiMoi))
            {
                Console.WriteLine("Trạng thái mới không được để trống.");
                return false;
            }
            if (maNguoiCapNhat <= 0)
            {
                Console.WriteLine("Mã người cập nhật không hợp lệ.");
                return false;
            }

            if (!_validStatuses.Contains(trangThaiMoi)) 
            {
                Console.WriteLine($"Trạng thái '{trangThaiMoi}' không hợp lệ.");
                return false;
            }

            // DTO_DonNhap currentOrder = await _dalOrder.GetOrderByIdAsync(maDonNhap);
            // if (currentOrder != null && !CanTransitionToStatus(currentOrder.TrangThai, trangThaiMoi))
            // {
            //      Console.WriteLine($"Không thể chuyển từ trạng thái '{currentOrder.TrangThai}' sang '{trangThaiMoi}'.");
            //      return false;
            // }
            // --- Kết thúc kiểm tra nghiệp vụ ---


            try
            {
                return await dal.CapNhatTrangThaiDonNhapAsync(maDonNhap, trangThaiMoi, maNguoiCapNhat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Business Logic Error updating status for order {maDonNhap}: {ex.Message}");
                return false;
            }
        }

    }
}
