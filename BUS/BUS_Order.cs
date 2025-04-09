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

            string chiTietNhapJson = JsonSerializer.Serialize(chiTietNhap);

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

        public async Task<List<DTO_VatLieu>> TimKiemVatLieuAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<DTO_VatLieu>();

            return await dal.TimKiemVatLieuAsync(keyword);
        }

        public List<DTO_DonNhapSearchResult> TimKiemDonNhap(string keyword)
        {
            return dal.TimKiemDonNhapTheoKeyword(keyword);
        }
       

    }
}