using System;
using System.Linq;
using DTO;
using DAL;
namespace BUS
{
    public class BUS_Nccap
    {
        private DAL_NCcap DAL_Nccap=new DAL_NCcap();
        public List<DTO_Ncap>TimKiemNcc(string keyword)
        {
            return DAL_Nccap.TimKiemNcc(keyword);   
        }
        public List<DTO_Ncap> GetAllNcc()
        {
            return DAL_Nccap.GetAllNcc();
        }
    }
}
