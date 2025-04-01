using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class BUS_Login
    {
        private QLTK dangNhapDAL = new QLTK();

        public DTO_ngdung kiemtradangnhap(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return dangNhapDAL.KiemTraDangNhap(username, password, email);
        }
    }
}
