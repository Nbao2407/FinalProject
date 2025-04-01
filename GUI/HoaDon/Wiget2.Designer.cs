namespace GUI
{
    partial class Wiget2
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
            IdSp = new Label();
            lblGia = new Label();
            roundedPanel1 = new GUI.Helpler.RoundedPanel();
            panel1 = new Panel();
            Title2 = new Label();
            roundedPictureBox3 = new GUI.Helpler.RoundedPictureBox();
            roundedPictureBox2 = new GUI.Helpler.RoundedPictureBox();
            TbSL = new TextBox();
            roundedPictureBox1 = new GUI.Helpler.RoundedPictureBox();
            roundedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // IdSp
            // 
            IdSp.AutoSize = true;
            IdSp.BackColor = Color.White;
            IdSp.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IdSp.ForeColor = Color.Black;
            IdSp.Location = new Point(71, 16);
            IdSp.Name = "IdSp";
            IdSp.Size = new Size(62, 20);
            IdSp.TabIndex = 1;
            IdSp.Text = "SP0001";
            // 
            // lblGia
            // 
            lblGia.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblGia.AutoSize = true;
            lblGia.BackColor = Color.White;
            lblGia.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGia.ForeColor = Color.Black;
            lblGia.Location = new Point(483, 53);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(68, 21);
            lblGia.TabIndex = 0;
            lblGia.Text = "700,000";
            // 
            // roundedPanel1
            // 
            roundedPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.Controls.Add(panel1);
            roundedPanel1.Controls.Add(Title2);
            roundedPanel1.Controls.Add(roundedPictureBox3);
            roundedPanel1.Controls.Add(roundedPictureBox2);
            roundedPanel1.Controls.Add(TbSL);
            roundedPanel1.Controls.Add(roundedPictureBox1);
            roundedPanel1.Controls.Add(IdSp);
            roundedPanel1.Controls.Add(lblGia);
            roundedPanel1.Location = new Point(3, 3);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(568, 88);
            roundedPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Location = new Point(90, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(46, 1);
            panel1.TabIndex = 22;
            // 
            // Title2
            // 
            Title2.AutoSize = true;
            Title2.BackColor = Color.White;
            Title2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Title2.ForeColor = Color.Black;
            Title2.Location = new Point(200, 16);
            Title2.Name = "Title2";
            Title2.Size = new Size(107, 20);
            Title2.TabIndex = 26;
            Title2.Text = "Tên Sản phẩm";
            // 
            // roundedPictureBox3
            // 
            roundedPictureBox3.BorderColor = Color.Transparent;
            roundedPictureBox3.BorderRadius = 20;
            roundedPictureBox3.BorderThickness = 2F;
            roundedPictureBox3.Image = Properties.Resources.icons8_plus_24;
            roundedPictureBox3.Location = new Point(142, 55);
            roundedPictureBox3.Name = "roundedPictureBox3";
            roundedPictureBox3.Size = new Size(20, 20);
            roundedPictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            roundedPictureBox3.TabIndex = 25;
            roundedPictureBox3.TabStop = false;
            // 
            // roundedPictureBox2
            // 
            roundedPictureBox2.BorderColor = Color.Transparent;
            roundedPictureBox2.BorderRadius = 20;
            roundedPictureBox2.BorderThickness = 2F;
            roundedPictureBox2.Image = Properties.Resources.icons8_minus_24;
            roundedPictureBox2.Location = new Point(59, 55);
            roundedPictureBox2.Name = "roundedPictureBox2";
            roundedPictureBox2.Size = new Size(20, 20);
            roundedPictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            roundedPictureBox2.TabIndex = 24;
            roundedPictureBox2.TabStop = false;
            // 
            // TbSL
            // 
            TbSL.BackColor = Color.White;
            TbSL.BorderStyle = BorderStyle.None;
            TbSL.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TbSL.ForeColor = Color.Black;
            TbSL.Location = new Point(90, 53);
            TbSL.Multiline = true;
            TbSL.Name = "TbSL";
            TbSL.Size = new Size(43, 24);
            TbSL.TabIndex = 23;
            TbSL.Text = "1";
            TbSL.TextAlign = HorizontalAlignment.Center;
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BorderColor = Color.Transparent;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 2F;
            roundedPictureBox1.Image = Properties.Resources.icons8_trash_242;
            roundedPictureBox1.Location = new Point(30, 15);
            roundedPictureBox1.Name = "roundedPictureBox1";
            roundedPictureBox1.Size = new Size(24, 24);
            roundedPictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox1.TabIndex = 2;
            roundedPictureBox1.TabStop = false;
            // 
            // Wiget2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(roundedPanel1);
            Name = "Wiget2";
            Size = new Size(574, 94);
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private Panel panel1;
        public Label IdSp;
        public Label lblGia;
        public Helpler.RoundedPanel roundedPanel1;
        public Helpler.RoundedPictureBox roundedPictureBox1;
        public TextBox TbSL;
        public Label Title2;
        public Helpler.RoundedPictureBox roundedPictureBox3;
        public Helpler.RoundedPictureBox roundedPictureBox2;
    }
}
