using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class Order
    {
        public List<DTO_Order> GetAllOrder()
        {
            DAL.DAL_Order dal = new DAL.DAL_Order();
            return dal.GetAllOrder();
        }
        public List<DTO_Order> TimKiemDonNhap(int ID)
        {
            DAL.DAL_Order dal = new DAL.DAL_Order();
            return dal.TimKiemDonNhap(ID);
        }
    }
}
