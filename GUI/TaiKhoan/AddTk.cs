using BUS;
using DTO;
using QLVT.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLVT.Helper;
using GUI.Helpler;

namespace QLVT.TaiKhoan
{
    public partial class AddTk : Form
    {
        private BUS_TK Bus = new BUS_TK();
        private FrmNV _parentForm;
        public AddTk(FrmNV frmNV)
        {
            InitializeComponent();
            _parentForm = frmNV;
            CbChucVuLoad();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string errorMessage;
                if (!ValidationHelper.IsValidTenDangNhap(TbName.Text, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidationHelper.IsValidEmail(TbEmail.Text, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidationHelper.IsValidSDT(TbSdt.Text, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidationHelper.IsValidMatKhau(Tbpass.Text, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (CbChucVu.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn chức vụ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!ValidationHelper.IsValidChucVu(CbChucVu.SelectedItem.ToString(), out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DTO_User user = new DTO_User
                {
                    Username = TbName.Text,
                    Email = TbEmail.Text,
                    Password = Tbpass.Text,
                    Sdt = TbSdt.Text,
                    Role = CbChucVu.SelectedItem.ToString(),
                    Ghichu = TbNote.Text,
                    TrangThai = "Hoạt động"
                };
                Bus.ThemTaiKhoan(user, CurrentUser.MaTK);
                MessageBox.Show("Thêm tài khoản thành công!");
                ClearTxtHelper clearTxtHelper = new ClearTxtHelper();
                clearTxtHelper.ClearTextBoxes(this);
                _parentForm.LoadData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CbChucVuLoad()
        {
            if (CurrentUser.ChucVu == "Admin")
            {
                CbChucVu.Items.Add("Admin");
                CbChucVu.Items.Add("Nhân viên");
                CbChucVu.Items.Add("Quản lý");
            }
            else if (CurrentUser.ChucVu == "Quản lý")
            {
                CbChucVu.Items.Add("Nhân viên");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Tbpass.UseSystemPasswordChar = !Tbpass.UseSystemPasswordChar;

        }
    }
}
