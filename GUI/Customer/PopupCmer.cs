﻿using BUS;
using DAL;
using DTO;
using QLVT.Helper;
using Microsoft.Data.SqlClient;

namespace QLVT
{
    public partial class PopupCmer : Form
    {
        private FrmCustomer _parentForm;
        private DTO_Khach _khachHang;
        private BUS_Khach BUS_Khach = new BUS_Khach();
        private bool isManualToggle = true;
        public PopupCmer(FrmCustomer parentForm, DTO_Khach khach)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += PopupForm_Load;
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            this.Resize += PopupForm_Resize;
            LoadDataToControls(_khachHang);
            _parentForm = parentForm;
            _khachHang = khach;
        }
        public void LoadDataToControls(DTO_Khach khach)
        {
            if (khach != null)
            {
                if (khach == null) return;

                isManualToggle = false;

                toggleEdit1.Toggled = khach.TrangThai?.ToString() == "Hoạt động";

                isManualToggle = true;

                lblName.Text = khach.Ten;
                dtpNgaySinh.Format = DateTimePickerFormat.Custom;
                dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
                if (khach.NgaySinh >= dtpNgaySinh.MinDate && khach.NgaySinh <= dtpNgaySinh.MaxDate)
                {
                    dtpNgaySinh.Value = khach.NgaySinh;
                }
                else
                {
                    dtpNgaySinh.Value = DateTime.Today;
                }
                smallTextBox1.Text = khach.GioiTinh;
                phone.Text = khach.SDT;
                email.Text = khach.Email;
                txtNgTao.Text = khach.TenNguoiTao ?? string.Empty;
                lblID.Text = khach.MaKhachHang.ToString();
                address.Text = khach.DiaChi;
                if (khach.NgayTao >= dtpNgayTao.MinDate && khach.NgayTao <= dtpNgayTao.MaxDate)
                {
                    dtpNgayTao.Value = khach.NgayTao;
                    Ngay.Text = khach.NgayTao.ToString("dd/MM/yyyy");
                }
                else
                {
                    dtpNgayTao.Value = DateTime.Today;
                    Ngay.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
            }
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this, 10);
        }

        private void PopupForm_Resize(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this, 10);
        }

        private void roundedPictureBox1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void PopupCmer_Load(object sender, EventArgs e)
        {
            
                LoadDataToControls(_khachHang);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int maKhachHang;
                if (!int.TryParse(lblID.Text, out maKhachHang))
                {
                    MessageBox.Show("Mã khách hàng trong Label không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show(
                    $"Hệ thống sẽ xóa hoàn toàn khách hàng {maKhachHang} nhưng vẫn giữ những giao dịch lịch sự nếu có.Bạn có chắc muốn xóa?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                int nguoiCapNhat = 1;

                QLKH qlkh = new QLKH();
                bool result = qlkh.XoaKhachHang(maKhachHang, nguoiCapNhat);
                if (result)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _parentForm.LoadKhachHang();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDisable_Click(object sender, EventArgs e)
        {
            try
            {
                int maKhachHang;
                if (!int.TryParse(lblID.Text, out maKhachHang))
                {
                    MessageBox.Show("Mã khách hàng trong Label không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show(
                    $"Bạn có chắc chắn ngừng sử dụng khách hàng này? Thông tin và giao dịch của khách hàng vẫn được giữ.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                int nguoiCapNhat = CurrentUser.MaTK;

                QLKH qlkh = new QLKH();
                bool result = qlkh.DisableKhachHang(maKhachHang, nguoiCapNhat);
                if (result)
                {
                    MessageBox.Show("Disable khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _parentForm.LoadKhachHang();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void UpdateKhachHang(DTO_Khach updatedKhach)
        {
            _khachHang = updatedKhach;
            LoadDataToControls(_khachHang);
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DBConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM QLHoaDon WHERE MaKhachHang = @MaKhachHang AND TrangThai = N'Chờ thanh toán'", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", int.Parse(lblID.Text));
                        int pendingOrders = (int)cmd.ExecuteScalar();

                        if (pendingOrders > 0)
                        {
                            BtnEdit.PrimaryColor = Color.Gray;
                            MessageBox.Show($"Khách hàng có {pendingOrders} hóa đơn đang chờ thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            BtnEdit.Enabled = false;
                        }
                        else
                        {

                            using (var popup = new EditCustomer(_parentForm, _khachHang, this))
                            {
                                popup.Deactivate += (s, e) => popup.TopMost = true;
                                popup.StartPosition = FormStartPosition.CenterParent;
                                popup.ShowDialog();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void toggleEdit1_ToggledChanged()
        {
            if (!isManualToggle) return;

            try
            {
                if (!int.TryParse(lblID.Text, out int maKhachHang))
                {
                    MessageBox.Show("Mã khách hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int nguoiCapNhat = CurrentUser.MaTK;
                bool isActive = toggleEdit1.Toggled;

                string message = isActive
                    ? "Bạn có chắc chắn kích hoạt lại khách hàng này?"
                    : "Bạn có chắc chắn ngừng sử dụng khách hàng này? Thông tin và giao dịch của khách hàng vẫn được giữ.";
                string title = "Xác nhận";

                DialogResult confirmResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.No) return;

                QLKH qlkh = new QLKH();
                bool result = isActive
                    ? qlkh.ActiveKhachHang(maKhachHang, nguoiCapNhat)
                    : qlkh.DisableKhachHang(maKhachHang, nguoiCapNhat);

                if (result)
                {
                    string successMessage = isActive ? "Kích hoạt lại khách hàng thành công!" : "Disable khách hàng thành công!";
                    MessageBox.Show(successMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _parentForm.LoadKhachHang();
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

        private void SetToggleState(bool state)
        {
            isManualToggle = false;
            toggleEdit1.Toggled = state;
            isManualToggle = true;
        }

        private void Ngay_TextChanged(object sender, EventArgs e)
        {

        }

        private void smallTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}