using System;
using System.Linq;
using DTO;
using DAL;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace BUS
{
    public class BUS_Ncc
    {
        private DAL_NCcap DAL_Nccap = new DAL_NCcap();

         public List<DTO_Ncap> TimKiemNcc(string keyword)
            {
                return DAL_Nccap.TimKiemNcc(keyword);
            }

            public List<DTO_Ncap> GetAllNcc()
            {
                return DAL_Nccap.GetAllNcc();
            }

            public void ThemNCC(string tenNCC, string diaChi, string sdt, string email, int nguoiTao)
            {
                DAL_Nccap.ThemNCC(tenNCC, diaChi, sdt, email, nguoiTao);
            }

            public void CapNhatNCC(int maNCC, string tenNCC, string diaChi, string sdt, string email, int nguoiCapNhat)
            {
                DAL_Nccap.CapNhatNCC(maNCC, tenNCC, diaChi, sdt, email, nguoiCapNhat);
            }   
        public void XoaNCC(int maNCC, int nguoiCapNhat)
        {
            DAL_Nccap.XoaNCC(maNCC, nguoiCapNhat);
        }
    }
}
