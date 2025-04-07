using BUS;
using DAL;
using DTO;
using System;
using System.Windows.Forms;
using System.Drawing;
using QLVT.Helper;
using QLVT.Helper;

namespace QLVT.NCC
{
    public partial class EditNcc : Form
    {
        private DTO_Ncap _ncc;
        private FrmNCc _parentForm;
        private BUS_Ncc _busNcc = new BUS_Ncc();
        private PopNcc _popupNcc;
        private ErrorProvider errorProvider; 

        public EditNcc(FrmNCc parentForm, DTO_Ncap ncc, PopNcc popNcc)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _ncc = ncc;
            _popupNcc = popNcc;
            errorProvider = new ErrorProvider(); 
            LoadData();
            TbName.Validating += TbName_Validating;
            TbSdt.Validating += TbSdt_Validating;
            TbEmail.Validating += TbEmail_Validating;
            TbAddress.Validating += TbAddress_Validating;
            TbID.ReadOnly = true;
            TbID.Enabled = false;
        }

        private void LoadData()
        {
            {TbID.Text = _ncc.MaNCC.ToString(); }
            PlaceholderHelper.SetDataAsPlaceholder(TbSdt, _ncc.SDT);
            PlaceholderHelper.SetDataAsPlaceholder(TbName, _ncc.TenNCC);
            PlaceholderHelper.SetDataAsPlaceholder(TbEmail, _ncc.Email);
            PlaceholderHelper.SetDataAsPlaceholder(TbAddress, _ncc.DiaChi);
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
            this.Close();
            _popupNcc.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateChildren())
                {
                    MessageBox.Show("Vui lòng kiểm tra và sửa các thông tin không hợp lệ!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int Id = int.Parse(TbID.Text);
                int nguoiCapNhat = CurrentUser.MaTK;

                string sdt = TbSdt.Text.Trim();

                if (!ValidationHelper.IsValidTenNCC(TbName.Text.Trim(), out string tenError))
                    throw new Exception(tenError);
                if (!ValidationHelper.IsValidSDT(sdt, out string sdtError))
                    throw new Exception(sdtError);
                if (!ValidationHelper.IsValidEmail(TbEmail.Text.Trim(), out string emailError))
                    throw new Exception(emailError);
                if (!ValidationHelper.IsValidDiaChi(TbAddress.Text.Trim(), out string diaChiError))
                    throw new Exception(diaChiError);

                _busNcc.CapNhatNCC(Id, TbName.Text.Trim(), TbAddress.Text,sdt,TbEmail.Text.Trim(), nguoiCapNhat);
                MessageBox.Show("Cập nhật nhà cung cấp thành công!");
                _parentForm.Loaddata();
                _popupNcc.Hide();
                _popupNcc.LoadDataToControls(_ncc);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}