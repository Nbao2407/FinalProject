using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using QLVT.Helper;
using System.Text.RegularExpressions;

namespace QLVT.Type
{
    public partial class AddType : Form
    {
        private BUS_LVL loaiVatLieuBLL = new BUS_LVL();
        private FrmType frmType;
        private DAL_LVL dal = new DAL_LVL();
        public AddType(FrmType parent)
        {
            InitializeComponent();
            frmType = parent;
            TbName.Focus();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = TbName.Text.Trim(); 
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Vui lòng nhập tên loại", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }
                if (Regex.IsMatch(name, @"^\d+$"))
                {
                    MessageBox.Show("Tên không hợp lệ (không thể chỉ chứa số).", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TbName.Focus();
                    return;
                }
                dal.IsTenLoaiExists(TbName.Text);
                loaiVatLieuBLL.Addd(TbName.Text);
                MessageBox.Show("Thêm loại vật liệu thành công!");
                frmType.LoadData();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearInput()
        {
            TbName.Text = string.Empty;
        }
    }
}
