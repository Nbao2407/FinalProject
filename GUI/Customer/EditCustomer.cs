using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace GUI
{
    public partial class EditCustomer : Form
    {
        private FrmCustomer _parentForm;
        private DTO_Khach _khachHang;
        private BUS_Khach busKhach = new BUS_Khach();
        private PopupCmer _popupForm;

        public EditCustomer(FrmCustomer parentForm, DTO_Khach khach, PopupCmer popupForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _khachHang = khach;
            _popupForm = popupForm;
            LoadData();
        }

        private void LoadData()
        {
            txtID.Text = _khachHang.MaKhachHang.ToString();
            TbName.Text = _khachHang.Ten;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _khachHang.Ten = TbName.Text;
                busKhach.SuaKhachHang(_khachHang);
                MessageBox.Show("Chỉnh sửa thông tin khách hàng thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DTO_Khach updatedKhachHang = busKhach.GetCustomerById(_khachHang.MaKhachHang);

                if (_popupForm != null)
                {
                    _popupForm.UpdateKhachHang(updatedKhachHang);
                }
                this.Close();
                _popupForm.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
            _popupForm.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}