using BUS;
using DTO;
using QLVT.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.NCC
{
    public partial class AddNcc : Form
    {
        private BUS_Ncc nccBLL = new BUS_Ncc(); 
        private ErrorProvider errorProvider;
        private FrmNCc _parentForm;
        public AddNcc(FrmNCc frmNCc)
        {
            InitializeComponent();
            _parentForm = frmNCc;
            TbName.Validating += TbName_Validating;
            TbSdt.Validating += TbSdt_Validating;
            TbEmail.Validating += TbEmail_Validating;
            TbAddress.Validating += TbAddress_Validating;
            errorProvider = new ErrorProvider();
            TbID.ReadOnly = true;

        }

        private void AddNcc_Load(object sender, EventArgs e)
        {
        }
        private void TbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidationHelper.IsValidTenNCC(TbName.Text, out string error))
            {
                e.Cancel = true;
                TbName.BackColor = Color.LightPink;
                errorProvider.SetError(TbName, error);
            }
            else
            {
                e.Cancel = false;
                TbName.BackColor = SystemColors.Window;
                errorProvider.SetError(TbName, "");
            }
        }

        private void TbSdt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string input = TbSdt.Text;
            if (!ValidationHelper.IsValidSDT(input, out string error))
            {
                e.Cancel = true;
                TbSdt.BackColor = Color.LightPink;
                errorProvider.SetError(TbSdt, error);
            }
            else
            {
                e.Cancel = false;
                TbSdt.BackColor = SystemColors.Window;
                errorProvider.SetError(TbSdt, "");
            }
        }

        private void TbEmail_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidationHelper.IsValidEmail(TbEmail.Text, out string error))
            {
                e.Cancel = true;
                TbEmail.BackColor = Color.LightPink;
                errorProvider.SetError(TbEmail, error);
            }
            else
            {
                e.Cancel = false;
                TbEmail.BackColor = SystemColors.Window;
                errorProvider.SetError(TbEmail, "");
            }
        }

        private void TbAddress_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidationHelper.IsValidDiaChi(TbAddress.Text, out string error))
            {
                e.Cancel = true;
                TbAddress.BackColor = Color.LightPink;
                errorProvider.SetError(TbAddress, error);
            }
            else
            {
                e.Cancel = false;
                TbAddress.BackColor = SystemColors.Window;
                errorProvider.SetError(TbAddress, "");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateChildren()) 
                {
                    int nguoiTao = CurrentUser.MaTK; 
                    nccBLL.ThemNCC(TbName.Text,TbAddress.Text,TbSdt.Text,TbEmail.Text,nguoiTao);
                    MessageBox.Show("Thêm nhà cung cấp thành công!");
                    _parentForm.Loaddata();
                    ClearInputFields();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearInputFields()
        {
            TbName.Text = string.Empty;
            TbSdt.Text = string.Empty;
            TbEmail.Text = string.Empty;    
            TbAddress.Text = string.Empty;
        }
    }
}
