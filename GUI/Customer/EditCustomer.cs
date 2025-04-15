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
using System.Text.RegularExpressions;
using QLVT.Helper;

namespace QLVT
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
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
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
                string name = TbName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }
                if (Regex.IsMatch(name, @"^\d+$")) 
                {
                    MessageBox.Show("Tên khách hàng không hợp lệ (không thể chỉ chứa số).", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }
                if (name.Length < 3 || name.Length > 100) 
                {
                    MessageBox.Show("Tên khách hàng quá ngắn", "Cảnh báo",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }
                // Ví dụ: Kiểm tra ký tự không hợp lệ (ngoài chữ cái, số, khoảng trắng, dấu nháy đơn, gạch nối)
                // if (Regex.IsMatch(name, @"[^\p{L}\p{N}\s'-]")) { ... return; }

                if (DateTime1.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không được là ngày trong tương lai!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DateTime1.Focus();
                    return;
                }

                if (CbGender.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn giới tính!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CbGender.Focus();
                    return;
                }

                string sdt = TbSdt.Text.Trim();
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbSdt.Focus();
                    return;
                }
                if (!Regex.IsMatch(sdt, @"^\d+$"))
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa các chữ số!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbSdt.Focus();
                    return;
                }
                 if (sdt.Length < 11 || sdt.Length > 12)
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbSdt.Focus();
                    return;
                }


                string email = TbEmail.Text.Trim();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!Regex.IsMatch(email, emailPattern))
                    {
                        MessageBox.Show("Định dạng Email không hợp lệ!", "Cảnh báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TbEmail.Focus();
                        return;
                    }
                }

                _khachHang.Ten = name; 
                _khachHang.NgaySinh = DateTime1.Value;
                _khachHang.GioiTinh = CbGender.SelectedItem.ToString();
                _khachHang.SDT = sdt;
                _khachHang.Email = email;

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
               
                    MessageBox.Show($"Đã xảy ra lỗi không mong muốn: {ex.Message}",
                                    "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}