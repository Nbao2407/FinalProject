using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GUI
{
    public partial class EditCustomer : Form
    {
        private BUS_Khach busKhach = new BUS_Khach();
        private FrmCustomer _parentForm;
        private PopupCmer _popupCmer; 
        private DTO_Khach _khachHang;

        public EditCustomer(FrmCustomer parentForm, DTO_Khach khach, PopupCmer popupCmer)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _popupCmer = popupCmer; 
            _khachHang = khach;
            LoadData();
        }

        private void LoadData()
        {
            TbName.Text = _khachHang.Ten;
            DateTime1.Value = _khachHang.NgaySinh;
            CbGender.SelectedItem = _khachHang.GioiTinh;
            TbSdt.Text = _khachHang.SDT;
            TbEmail.Text = _khachHang.Email;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TbName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }

                _khachHang.Ten = TbName.Text.Trim();
                _khachHang.NgaySinh = DateTime1.Value;
                _khachHang.GioiTinh = CbGender.SelectedItem?.ToString();
                _khachHang.SDT = TbSdt.Text.Trim();
                _khachHang.Email = TbEmail.Text.Trim();

                bool updateResult = await busKhach.SuaKhachHang(_khachHang);

                if (updateResult)
                {
                    MessageBox.Show("Chỉnh sửa thông tin khách hàng thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DTO_Khach updatedKhachHang = busKhach.GetCustomerById(_khachHang.MaKhachHang);
                    if (updatedKhachHang != null && _popupCmer != null)
                    {
                        _popupCmer.LoadDataToControls(updatedKhachHang);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật thông tin khách hàng!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}