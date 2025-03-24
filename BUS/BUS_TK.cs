using System;
using System.Linq;
using DTO;
using DAL;


namespace BUS
{
    public class BUS_TK
    {
        private QLTK DAL_TK = new QLTK();
        public List<DTO_TK> TimKiemTk(string keyword)
        {
            return DAL_TK.TimKiemTk(keyword);
        }
        public List<DTO_TK> GetAllTk()
        {
            return DAL_TK.GetAllTk();
        }
    }
}
