using GUI.TaiKhoan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Type
{
    public partial class PopupType : Form
    {
        public PopupType()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var add = new Edit())
            {
                add.Deactivate += (s, e) => add.TopMost = true;

                add.StartPosition = FormStartPosition.CenterParent;

                add.ShowDialog();
            }
        }
    }
}
