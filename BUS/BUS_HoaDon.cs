using DAL;
using DTO;
using System;
using System.Linq;

namespace BUS
{
    public class BUS_HoaDon
    {
        private DAL_HoaDon dalHoaDon = new DAL_HoaDon();

        public List<DTO_HoaDon> LayTatCaHoaDon()
        {
            return dalHoaDon.LayTatCaHoaDon();
        }

    
    }
}
