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
    public partial class FrmUser : Form
    {
  
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public FrmUser()
        {
            InitializeComponent();

            // Bo tròn cho form
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            RoundedPictureBox roundedPictureBox = new RoundedPictureBox();

        }



        private void FrmUser_Load(object sender, EventArgs e)
        {

        }

   


        private void parrotPictureBox2_Click(object sender, EventArgs e)
        {
          this.Close();
        }
    }
}