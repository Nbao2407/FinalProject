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
        private BUS_Khach busKhach = new BUS_Khach();
        private FrmCustomer _parentForm;
        private DTO_Khach _khachHang;

        public EditCustomer(FrmCustomer parentForm, DTO_Khach khach,PopupCmer popupCmer)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _khachHang = khach;
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

               
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}