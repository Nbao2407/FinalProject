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
    public partial class PopupOrder : Form
    {
        public PopupOrder()
        {
            InitializeComponent();
        }

        private void PopupOrder_Load(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
