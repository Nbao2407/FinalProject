using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace QLVT
{
    public partial class AddCustomer : Form
    {
        private FrmCustomer _parentForm;
        private BUS_Khach busKhach = new BUS_Khach();
        public AddCustomer(FrmCustomer parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ten = TbName.Text.Trim();
                DateTime ngaySinh = dtpNgaySinh.Value;
                string gioiTinh = CbGender.SelectedItem?.ToString();
                string sdt = TbSdt.Text.Trim();
                string email = TbEmail.Text.Trim();

                if (string.IsNullOrEmpty(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(sdt) || sdt.Length < 10 || !sdt.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Phải là số và từ 10-15 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbSdt.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    MessageBox.Show("Email không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbEmail.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(gioiTinh))
                {
                    MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CbGender.Focus();
                    return;
                }

                DTO_Khach khach = new DTO_Khach
                {
                    Ten = ten,
                    NgaySinh = ngaySinh,
                    GioiTinh = gioiTinh,
                    SDT = sdt,
                    Email = email,
                    NguoiTao = CurrentUser.MaTK,
                };

                busKhach.ThemKhachHang(khach);

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_parentForm != null)
                {
                    _parentForm.LoadKhachHang();
                }

                this.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi SQL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged_1(object sender, EventArgs e)
        {

        }
    }
}