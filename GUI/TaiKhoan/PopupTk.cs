using BUS;
using DAL;
using DTO;
using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.TaiKhoan
{
    public partial class PopupTk : Form
    {
        private BUS_TK BUS = new BUS_TK();
        private bool isManualToggle = true;
        private FrmNV _parentFrm;
        private int? maTK;
        private DTO_TK _tk;
        public PopupTk(FrmNV frmNV, int? maTK)
        {
            InitializeComponent();
            this.maTK = maTK;
            _parentFrm = frmNV;
            LoadDataFromDatabase();
        }

        public void LoadDataFromDatabase()
        {
            if (maTK.HasValue)
            {
                DataTable dt = BUS.GetTkByMa(maTK.Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    isManualToggle = false;
                    string TrangThai = row["TrangThai"].ToString();
                    if (TrangThai == "Hoạt động")
                    {
                        TgTrangthai.Checked = true;
                        TgTrangthai.ToggleColor = Color.LimeGreen;

                    }
                    else
                    {
                        TgTrangthai.Checked = false;
                        TgTrangthai.ToggleColor = Color.Crimson;
                    }
                    isManualToggle = true;
                    lblID.Text = row["MaTK"].ToString();
                    lblName.Text = row["TenDangNhap"].ToString();
                    TbSdt.Text = row["SDT"].ToString();
                    TbNote.Text = row["GhiChu"].ToString();
                    TbEmail.Text = row["Email"].ToString();
                    lblChucvu.Text = row["ChucVu"].ToString();
                    txtNgTao.Text = row["NguoiTao"].ToString();
                    Ngay.Text = row["NgayTao"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vật liệu với mã này!");
                    this.Close();
                }
            }
        }
        private void PopupTk_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int? maTk =lblID.Text == "" ? null : int.Parse(lblID.Text); 
            if (maTk.HasValue)
            {
                this.Close();
                EditTk editForm = new EditTk(_parentFrm, maTk);
                editForm.FormClosed += (s, args) =>
                {
                    LoadDataFromDatabase();
                    _parentFrm.LoadData();
                };
                editForm.StartPosition = FormStartPosition.CenterParent;
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản để chỉnh sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int maTK = int.Parse(lblID.Text);
            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    BUS.XoaTaiKhoan(maTK,CurrentUser.MaTK);
                    MessageBox.Show("Xóa tài khoản thành công!");
                    _parentFrm.LoadData();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TgTrangthai_CheckedChanged(object sender)
        {
            if (!isManualToggle) return;

            try
            {
                if (!int.TryParse(lblID.Text, out int maTk))
                {
                    MessageBox.Show("Mã Tài khoản không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(CurrentUser.MaTK == maTk)
                {
                    MessageBox.Show("Bạn không thể tự tắt tài khoản của mình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int nguoiCapNhat = CurrentUser.MaTK;
                bool isActive = TgTrangthai.Checked;

                string message = isActive
                    ? "Bạn có chắc chắn kích hoạt tài khoản này?"
                    : "Bạn có chắc chắn ngừng sử dụng tài khoản này? Các Thông tin và giao dịch của vẫn được giữ.";
                string title = "Xác nhận";

                DialogResult confirmResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.No) return;

                QLTK dAL = new QLTK();
                bool result = isActive
                    ? dAL.Active(maTk, nguoiCapNhat)
                    : dAL.Disable(maTk, nguoiCapNhat);
                if (result)
                {
                    string successMessage = isActive ? "Kích hoạt lại tài khoản thành công!" : "Disable tài khoản thành công!";
                    MessageBox.Show(successMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    changeColor();
                    _parentFrm.LoadData();
                }
                else
                {
                    MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void changeColor()
        {
            if (TgTrangthai.Checked == false)
            {
                TgTrangthai.ToggleColor = Color.Crimson;
            }
            else
            {
                TgTrangthai.ToggleColor = Color.LimeGreen;
            }
        }
        private void Tai()
        {
            if (int.Parse(lblID.Text) == CurrentUser.MaTK && lblChucvu.Text == "Admin" && CurrentUser.ChucVu == "Admin")
            {
                TgTrangthai.Enabled = false;
                BtnDelete.Enabled = false;
            }
            else if (int.Parse(lblID.Text) == CurrentUser.MaTK && lblChucvu.Text == "Quản Lý" && CurrentUser.ChucVu != "Admin")
            {
                TgTrangthai.Enabled = false;
                BtnDelete.Enabled = true;
            }
          
        }
        
   }
}
