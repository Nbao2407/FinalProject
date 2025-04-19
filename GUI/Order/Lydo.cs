using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.Order
{
    public partial class LydoHuy : Form
    {
        public string LyDoHuy { get; private set; }
        public LydoHuy()
        {
            InitializeComponent();
        }
        private void hopeRoundButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLyDo.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LyDoHuy = txtLyDo.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void hopeRoundButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtLyDo_Enter(object sender, EventArgs e)
        {
            if(txtLyDo.Text == "Vui lòng nhập lý do hủy")
            {
                txtLyDo.Text = "";
                txtLyDo.ForeColor = Color.Black;
            }
        }
        private void txtLyDo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLyDo.Text))
            {
                txtLyDo.Text = "Vui lòng nhập lý do hủy hóa đơn";
                txtLyDo.ForeColor = Color.Gray;
            }
        }

    }
}
