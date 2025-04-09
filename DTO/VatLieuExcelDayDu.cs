using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class VatLieuExcelDayDu
    {
        public int RowNum { get; set; }
        public string MaVatLieuStr { get; set; }
        public string TenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public string SoLuongStr { get; set; }
        public string GiaNhapStr { get; set; }
        public string TenNCC { get; set; }
        public string SdtNcc { get; set; }
        public string DiaChiNcc { get; set; }
        public string EmailNcc { get; set; }
        public string RowError { get; set; }
    }

    public class NccFilterItem
    {
        public string TenNCC { get; set; } 
        public override string ToString() => TenNCC; 
    }
}
