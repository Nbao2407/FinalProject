using GUI.Helpler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{

    public partial class PopupCmer :Form
    {
        public PopupCmer()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // Ẩn viền cửa sổ
            this.BackColor = Color.White;
            this.Load += PopupForm_Load;
            this.Resize += PopupForm_Resize;
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this, 10   );
        }

        private void PopupForm_Resize(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this,10);
        }
        private void roundedPictureBox1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void PopupCmer_Load(object sender, EventArgs e)
        {

        }
    }
}
