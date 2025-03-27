namespace GUI
{
    partial class Wiget
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblGia = new Label();
            TitleSp = new Label();
            roundedPanel1 = new GUI.Helpler.RoundedPanel();
            roundedPictureBox1 = new GUI.Helpler.RoundedPictureBox();
            roundedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblGia
            // 
            lblGia.AutoSize = true;
            lblGia.BackColor = Color.White;
            lblGia.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGia.ForeColor = Color.FromArgb(0, 192, 0);
            lblGia.Location = new Point(90, 47);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(32, 20);
            lblGia.TabIndex = 0;
            lblGia.Text = "Giá";
            // 
            // TitleSp
            // 
            TitleSp.AutoSize = true;
            TitleSp.BackColor = Color.White;
            TitleSp.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TitleSp.ForeColor = Color.Black;
            TitleSp.Location = new Point(90, 16);
            TitleSp.Name = "TitleSp";
            TitleSp.Size = new Size(106, 20);
            TitleSp.TabIndex = 1;
            TitleSp.Text = "Tên sản phẩm";
            // 
            // roundedPanel1
            // 
            roundedPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.Controls.Add(roundedPictureBox1);
            roundedPanel1.Controls.Add(TitleSp);
            roundedPanel1.Controls.Add(lblGia);
            roundedPanel1.Cursor = Cursors.Hand;
            roundedPanel1.Location = new Point(3, 3);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(244, 81);
            roundedPanel1.TabIndex = 3;
            roundedPanel1.Paint += roundedPanel1_Paint;
            roundedPanel1.MouseEnter += Wiget_MouseEnter;
            roundedPanel1.MouseLeave += Wiget_MouseLeave;
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BackColor = Color.Transparent;
            roundedPictureBox1.BackgroundImageLayout = ImageLayout.Center;
            roundedPictureBox1.BorderColor = Color.Black;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 2.5F;
            roundedPictureBox1.Image = Properties.Resources.freepik__background__674361;
            roundedPictureBox1.Location = new Point(14, 6);
            roundedPictureBox1.Name = "roundedPictureBox1";
            roundedPictureBox1.Size = new Size(60, 60);
            roundedPictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            roundedPictureBox1.TabIndex = 3;
            roundedPictureBox1.TabStop = false;
            // 
            // Wiget
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(roundedPanel1);
            Name = "Wiget";
            Size = new Size(250, 87);
            Load += Wiget_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Helpler.RoundedPanel roundedPanel1;
        public Label lblGia;
        public Label TitleSp;
        public Helpler.RoundedPictureBox roundedPictureBox1;
    }
}
