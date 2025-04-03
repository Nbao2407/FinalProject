using BUS;
using DAL;
using DTO;
using GUI.TaiKhoan;
using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Type
{
    public partial class PopupType : Form
    {
        private FrmType frmType;
        private bool isManualToggle = true;
        private DTO_LVL _lvatLieu;
        private DAL_LVL dal = new DAL_LVL();
        private BUS_LVL lVL = new BUS_LVL();

        public PopupType(FrmType parent, DTO_LVL lvatLieu)
        {
            InitializeComponent();
            frmType = parent;
            _lvatLieu = lvatLieu;
            LoadData(lvatLieu);

            TbName.KeyPress += TbName_KeyPress;
        }

        private void PopupType_Load(object sender, EventArgs e)
        {
        }

        private void LoadData(DTO_LVL lvatLieu)
        {
            if (lvatLieu != null)
            {
                if (lvatLieu == null) return;

                isManualToggle = false;

                foreverToggle1.Checked = lvatLieu.TrangThai?.ToString() == "Hoạt động";

                isManualToggle = true;
                TbID.Text = lvatLieu.MaLoaiVatLieu.ToString();
                TbName.Text = lvatLieu.TenLoai;
                TbID.Enabled = false;
                TbName.Enabled = true;

                changeColor();
            }
        }

        private void TbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(TbID.Text);

            if (MessageBox.Show("Bạn có chắc muốn xóa loại vật liệu này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    lVL.Delete(Id);
                    MessageBox.Show("Xóa loại vật liệu thành công!");
                    lVL.DeleteLoaiVatLieu(Id);
                    frmType.LoadData();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dal.Disable(Id, CurrentUser.MaTK);
                    frmType.LoadData();
                    this.Close();
                }
            }
        }

        private void roundedPictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(TbID.Text);
            try
            {
                dal.Update(ID, TbName.Text);
                MessageBox.Show("Cập nhật loại vật liệu thành công!");
                frmType.LoadData();
                TbName.Text = String.Empty;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void foreverToggle1_CheckedChanged(object sender)
        {
            if (!isManualToggle) return;

            try
            {
                if (!int.TryParse(TbID.Text, out int MaLoaiVatLieu))
                {
                    MessageBox.Show("Mã khách hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int nguoiCapNhat = CurrentUser.MaTK;
                bool isActive = foreverToggle1.Checked;

                string message = isActive
                    ? "Bạn có chắc chắn kích hoạt lại loại vật liệu này?"
                    : "Bạn có chắc chắn ngừng sử dụng loại vật liệu này? Các Thông tin và giao dịch của vẫn được giữ.";
                string title = "Xác nhận";

                DialogResult confirmResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.No) return;

                DAL_LVL dAL = new DAL_LVL();
                bool result = isActive
                    ? dAL.Active(MaLoaiVatLieu, nguoiCapNhat)
                    : dAL.Disable(MaLoaiVatLieu, nguoiCapNhat);
                if (result)
                {
                    string successMessage = isActive ? "Kích hoạt lại loại vật liệu thành công!" : "Disable loại vật liệu thành công!";
                    MessageBox.Show(successMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    changeColor();
                    frmType.LoadData();
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
            if (foreverToggle1.Checked == false)
            {
                foreverToggle1.ToggleColor = Color.Crimson;
            }
            else
            {
                foreverToggle1.ToggleColor = Color.LimeGreen;
            }
        }
    }
}
