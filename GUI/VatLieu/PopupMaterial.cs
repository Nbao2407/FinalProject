using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.VatLieu
{
    public partial class PopupMaterial : Form
    {
        public PopupMaterial()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            using (var edit = new EditMaterial())
            {
                edit.Deactivate += (s, e) => edit.TopMost = true;
                edit.StartPosition = FormStartPosition.CenterParent;
                edit.ShowDialog();
            }
        }

        private void roundedPictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
