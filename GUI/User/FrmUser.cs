using BUS;
using DTO;
using QLVT.Helper;
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

namespace QLVT
{
    public partial class FrmUser : Form
    {
        private List<DTO_User> danhSachTk;
        private BUS_TK busTk = new BUS_TK();
        private int TaiKhoan = CurrentUser.MaTK;
        private DTO_TK _tk;
        private byte[] avatarData;

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
            LoadData();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            RoundedPictureBox roundedPictureBox = new RoundedPictureBox();
        }

        private void LoadData()
        {
            DataTable dt = busTk.GetTkByMa(TaiKhoan);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                LblID.Text = row["MaTK"].ToString();
                TxtVaitro.Text = row["ChucVu"].ToString();
                UserName.Text = row["TenDangNhap"].ToString();
                PlaceholderHelper.SetDataAsPlaceholder(TbName, row["TenDangNhap"].ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbEmail, row["Email"].ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbPhone, row["SDT"].ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbNote, row["Ghichu"].ToString());
                if (row["Avatar"] != DBNull.Value)
                {
                    byte[] hinhAnh = (byte[])row["Avatar"];
                    using (MemoryStream ms = new MemoryStream(hinhAnh))
                    {
                        Avatar.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    Avatar.Image = null;
                }

            }
            else
            {
                MessageBox.Show("Lỗi!");
                this.Close();
            }
        }



        private void FrmUser_Load(object sender, EventArgs e)
        {
        }

        private void parrotPictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TbPhone.Text = string.Empty;
            TbName.Text = string.Empty;
            TbEmail.Text = string.Empty;
            TbNote.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Chọn ảnh avatar <3 mb"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                avatarData = File.ReadAllBytes(openFileDialog.FileName);
                using (MemoryStream ms = new MemoryStream(avatarData))
                {
                    Avatar.Image = Image.FromStream(ms);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = TbName.Text;
                string email = TbEmail.Text;
                string sdt = TbPhone.Text;
                int nguoiCapNhat = CurrentUser.MaTK;
                string ghichu = TbNote.Text;
                int MaTk = int.Parse(LblID.Text);
                busTk.UpdateAccount(MaTk, tenDangNhap, email, sdt, avatarData, ghichu, nguoiCapNhat);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangePass_Click(object sender, EventArgs e)
        {
            using (var changePassForm = new ChangePW())
            {
                changePassForm.Deactivate += (s, e) => changePassForm.TopMost = true;
                changePassForm.StartPosition = FormStartPosition.CenterParent;
                changePassForm.ShowDialog();
            }
        }
    }
}
