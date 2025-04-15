using BUS;
using DTO;
using Microsoft.Data.SqlClient;
using QLVT.Helper;
using System.Text.RegularExpressions;

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
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
        }

        private void btnSave_Click(object sender, EventArgs e)
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

                if (dtpNgaySinh.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không được là ngày trong tương lai!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpNgaySinh.Focus();
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
                    string address = TbAddress.Text.Trim();
                    if (string.IsNullOrWhiteSpace(address))
                    {
                        string addressPattern = @"^[a-zA-Z0-9\s,.'-]{3,}$";
                        if (!Regex.IsMatch(address, emailPattern));
                        MessageBox.Show("Vui lòng nhập địa chỉ!", "Cảnh báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TbAddress.Focus();
                        return;
                    }

                    DTO_Khach khach = new DTO_Khach
                    {
                        Ten = TbName.Text,
                        NgaySinh = dtpNgaySinh.Value,
                        GioiTinh = CbGender.SelectedItem.ToString(),
                        SDT = TbSdt.Text,
                        Email = TbEmail.Text,
                        NguoiTao = CurrentUser.MaTK,
                        DiaChi = TbAddress.Text,
                    };
                    busKhach.ThemKhachHang(khach);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (_parentForm != null)
                    {
                        _parentForm.LoadKhachHang();
                    }
                    this.Close();
                }
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
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}