using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class BUS_VatLieu
    {
        private QLVatLieu dalVatLieu = new QLVatLieu();

        // Lấy danh sách vật liệu từ DAL
        public List<DTO_VatLieu> GetAllVatLieu()
        {
            var danhSachVatLieu = dalVatLieu.LayTatCaVatLieu();

            // In dữ liệu để kiểm tra
            foreach (var item in danhSachVatLieu)
            {
                Console.WriteLine($"MaVatLieu: {item.MaVatLieu}, Ten: {item.Ten}, Loai: {item.Loai}");
            }

            return danhSachVatLieu;
        }

    }
}
