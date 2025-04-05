using BUS;
using DTO;
using GUI.Helpler;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmUser : Form
    {
        private List<DTO_User> danhSachTk;
        private BUS_TK busTk = new BUS_TK();
        private int TaiKhoan = CurrentUser.MaTK;
        private DTO_TK _tk;

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public FrmUser(DTO_TK tk)
        {
            InitializeComponent();
            LoadData(tk); 
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            RoundedPictureBox roundedPictureBox = new RoundedPictureBox();
        }

        private void LoadData(DTO_TK tk)
        {
            _tk = tk;

            if (tk != null)
            {
                LblID.Text = tk.maTK.ToString();

                PlaceholderHelper.SetDataAsPlaceholder(TbName, tk.tenDangNhap ?? "Không có tên đăng nhập");
                PlaceholderHelper.SetDataAsPlaceholder(TbEmail, tk.email ?? "Không có email");
                PlaceholderHelper.SetDataAsPlaceholder(TxtVaitro, tk.quyen ?? "Không có vai trò");
                PlaceholderHelper.SetDataAsPlaceholder(TbPhone, tk.sdt ?? "Không có số điện thoại");
                PlaceholderHelper.SetDataAsPlaceholder(TbAddress, tk.diaChi ?? "Không có địa chỉ");
                PlaceholderHelper.SetDataAsPlaceholder(TbNote, tk.ghichu ?? "Không có ghi chú");

                
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng!");
            }
        }
           


        private void FrmUser_Load(object sender, EventArgs e)
        {
        }

        private void parrotPictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}