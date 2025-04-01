using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GUI
{

    public partial class Wiget : UserControl
    {
        public event EventHandler OnSelect = null;
        private bool isHovered = false;
        public Wiget()
        {
            InitializeComponent();
            this.BackColor = Color.White; 

            this.MouseEnter += Wiget_MouseEnter;
            this.MouseLeave += Wiget_MouseLeave;
        }

        private void Wiget_MouseEnter(object sender, EventArgs e)
        {
            isHovered = true;
            this.BackColor = ColorTranslator.FromHtml("#27ae61");
            this.Invalidate();
        }

        private void Wiget_MouseLeave(object sender, EventArgs e)
        {
            isHovered = false;
            this.BackColor = Color.White; 
            this.Invalidate();
        }
        private void Wiget_Load(object sender, EventArgs e)
        {

        }

        private void roundedPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public string Title
        {
            get => TitleSp.Text;
            set => TitleSp.Text = value;
        }

        public double Gia
        {
            get => double.Parse(lblGia.Text);
            set => lblGia.Text = value.ToString("N0");
        }

        public Image Images
        {
            get => roundedPictureBox1.Image;
            set => roundedPictureBox1.Image = value;
        }
    }
}
