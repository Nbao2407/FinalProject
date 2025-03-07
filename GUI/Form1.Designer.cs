namespace GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            nightControlBox1 = new ReaLTaiizor.Controls.NightControlBox();
            btnSidebar = new PictureBox();
            panel2 = new Panel();
            panel10 = new Panel();
            BtnNV = new Button();
            button11 = new Button();
            panel8 = new Panel();
            BtnTke = new Button();
            button9 = new Button();
            panel6 = new Panel();
            BtnOrder = new Button();
            button5 = new Button();
            panel5 = new Panel();
            BtnType = new Button();
            panel3 = new Panel();
            btnMaterial = new Button();
            panel4 = new Panel();
            button2 = new Button();
            panel7 = new Panel();
            btnCustomer = new Button();
            panel9 = new Panel();
            BtnUser = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnSidebar).BeginInit();
            panel2.SuspendLayout();
            panel10.SuspendLayout();
            panel8.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel7.SuspendLayout();
            panel9.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(nightControlBox1);
            panel1.Controls.Add(btnSidebar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1150, 45);
            panel1.TabIndex = 0;
            // 
            // nightControlBox1
            // 
            nightControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nightControlBox1.BackColor = Color.Transparent;
            nightControlBox1.CloseHoverColor = Color.FromArgb(199, 80, 80);
            nightControlBox1.CloseHoverForeColor = Color.White;
            nightControlBox1.DefaultLocation = true;
            nightControlBox1.DisableMaximizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.DisableMinimizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.EnableCloseColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMaximizeButton = true;
            nightControlBox1.EnableMaximizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMinimizeButton = true;
            nightControlBox1.EnableMinimizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.Location = new Point(1011, 0);
            nightControlBox1.MaximizeHoverColor = Color.FromArgb(15, 255, 255, 255);
            nightControlBox1.MaximizeHoverForeColor = Color.White;
            nightControlBox1.MinimizeHoverColor = Color.FromArgb(15, 255, 255, 255);
            nightControlBox1.MinimizeHoverForeColor = Color.White;
            nightControlBox1.Name = "nightControlBox1";
            nightControlBox1.Size = new Size(139, 31);
            nightControlBox1.TabIndex = 3;
            // 
            // btnSidebar
            // 
            btnSidebar.BackgroundImageLayout = ImageLayout.None;
            btnSidebar.Image = (Image)resources.GetObject("btnSidebar.Image");
            btnSidebar.Location = new Point(12, 7);
            btnSidebar.Name = "btnSidebar";
            btnSidebar.Size = new Size(32, 32);
            btnSidebar.SizeMode = PictureBoxSizeMode.CenterImage;
            btnSidebar.TabIndex = 2;
            btnSidebar.TabStop = false;
            btnSidebar.Click += ToggleButton_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Black;
            panel2.Controls.Add(panel10);
            panel2.Controls.Add(panel8);
            panel2.Controls.Add(panel6);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel7);
            panel2.Controls.Add(panel9);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 45);
            panel2.Name = "panel2";
            panel2.Size = new Size(223, 555);
            panel2.TabIndex = 1;
            // 
            // panel10
            // 
            panel10.Controls.Add(BtnNV);
            panel10.Controls.Add(button11);
            panel10.Location = new Point(3, 312);
            panel10.Name = "panel10";
            panel10.Size = new Size(215, 45);
            panel10.TabIndex = 11;
            // 
            // BtnNV
            // 
            BtnNV.BackColor = Color.Black;
            BtnNV.Cursor = Cursors.Hand;
            BtnNV.ForeColor = Color.White;
            BtnNV.Image = (Image)resources.GetObject("BtnNV.Image");
            BtnNV.ImageAlign = ContentAlignment.MiddleLeft;
            BtnNV.Location = new Point(-9, -7);
            BtnNV.Name = "BtnNV";
            BtnNV.Padding = new Padding(15, 0, 0, 0);
            BtnNV.Size = new Size(228, 59);
            BtnNV.TabIndex = 4;
            BtnNV.Text = "Nhân sự";
            BtnNV.UseVisualStyleBackColor = false;
            BtnNV.Click += BtnNV_Click;
            // 
            // button11
            // 
            button11.BackColor = Color.Black;
            button11.Cursor = Cursors.Hand;
            button11.ForeColor = Color.White;
            button11.Image = (Image)resources.GetObject("button11.Image");
            button11.ImageAlign = ContentAlignment.MiddleLeft;
            button11.Location = new Point(-9, -6);
            button11.Name = "button11";
            button11.Padding = new Padding(15, 0, 0, 0);
            button11.Size = new Size(228, 59);
            button11.TabIndex = 3;
            button11.Text = "Thống kê ";
            button11.UseVisualStyleBackColor = false;
            // 
            // panel8
            // 
            panel8.Controls.Add(BtnTke);
            panel8.Controls.Add(button9);
            panel8.Location = new Point(5, 261);
            panel8.Name = "panel8";
            panel8.Size = new Size(215, 45);
            panel8.TabIndex = 10;
            // 
            // BtnTke
            // 
            BtnTke.BackColor = Color.Black;
            BtnTke.Cursor = Cursors.Hand;
            BtnTke.ForeColor = Color.White;
            BtnTke.Image = (Image)resources.GetObject("BtnTke.Image");
            BtnTke.ImageAlign = ContentAlignment.MiddleLeft;
            BtnTke.Location = new Point(-9, -7);
            BtnTke.Name = "BtnTke";
            BtnTke.Padding = new Padding(15, 0, 0, 0);
            BtnTke.Size = new Size(228, 59);
            BtnTke.TabIndex = 4;
            BtnTke.Text = "Thống kê ";
            BtnTke.UseVisualStyleBackColor = false;
            BtnTke.Click += BtnTke_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.Black;
            button9.Cursor = Cursors.Hand;
            button9.ForeColor = Color.White;
            button9.Image = (Image)resources.GetObject("button9.Image");
            button9.ImageAlign = ContentAlignment.MiddleLeft;
            button9.Location = new Point(-9, -6);
            button9.Name = "button9";
            button9.Padding = new Padding(15, 0, 0, 0);
            button9.Size = new Size(228, 59);
            button9.TabIndex = 3;
            button9.Text = "Thống kê ";
            button9.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            panel6.Controls.Add(BtnOrder);
            panel6.Controls.Add(button5);
            panel6.Location = new Point(3, 210);
            panel6.Name = "panel6";
            panel6.Size = new Size(215, 45);
            panel6.TabIndex = 9;
            // 
            // BtnOrder
            // 
            BtnOrder.BackColor = Color.Black;
            BtnOrder.Cursor = Cursors.Hand;
            BtnOrder.ForeColor = Color.White;
            BtnOrder.Image = (Image)resources.GetObject("BtnOrder.Image");
            BtnOrder.ImageAlign = ContentAlignment.MiddleLeft;
            BtnOrder.Location = new Point(-7, -7);
            BtnOrder.Name = "BtnOrder";
            BtnOrder.Padding = new Padding(15, 0, 0, 0);
            BtnOrder.Size = new Size(228, 59);
            BtnOrder.TabIndex = 4;
            BtnOrder.Text = "Đơn hàng";
            BtnOrder.UseVisualStyleBackColor = false;
            BtnOrder.Click += BtnOrder_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.Black;
            button5.Cursor = Cursors.Hand;
            button5.ForeColor = Color.White;
            button5.Image = (Image)resources.GetObject("button5.Image");
            button5.ImageAlign = ContentAlignment.MiddleLeft;
            button5.Location = new Point(-9, -6);
            button5.Name = "button5";
            button5.Padding = new Padding(15, 0, 0, 0);
            button5.Size = new Size(228, 59);
            button5.TabIndex = 3;
            button5.Text = "Thống kê ";
            button5.UseVisualStyleBackColor = false;
            // 
            // panel5
            // 
            panel5.Controls.Add(BtnType);
            panel5.Location = new Point(3, 159);
            panel5.Name = "panel5";
            panel5.Size = new Size(215, 45);
            panel5.TabIndex = 8;
            // 
            // BtnType
            // 
            BtnType.BackColor = Color.Black;
            BtnType.Cursor = Cursors.Hand;
            BtnType.ForeColor = Color.White;
            BtnType.Image = (Image)resources.GetObject("BtnType.Image");
            BtnType.ImageAlign = ContentAlignment.MiddleLeft;
            BtnType.Location = new Point(-9, -6);
            BtnType.Name = "BtnType";
            BtnType.Padding = new Padding(15, 0, 0, 0);
            BtnType.Size = new Size(228, 59);
            BtnType.TabIndex = 3;
            BtnType.Text = "Loại Vật liệu";
            BtnType.UseVisualStyleBackColor = false;
            BtnType.Click += BtnType_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnMaterial);
            panel3.Location = new Point(5, 108);
            panel3.Name = "panel3";
            panel3.Size = new Size(215, 45);
            panel3.TabIndex = 7;
            // 
            // btnMaterial
            // 
            btnMaterial.BackColor = Color.Black;
            btnMaterial.Cursor = Cursors.Hand;
            btnMaterial.ForeColor = Color.White;
            btnMaterial.Image = (Image)resources.GetObject("btnMaterial.Image");
            btnMaterial.ImageAlign = ContentAlignment.MiddleLeft;
            btnMaterial.Location = new Point(-9, -6);
            btnMaterial.Name = "btnMaterial";
            btnMaterial.Padding = new Padding(15, 0, 0, 0);
            btnMaterial.Size = new Size(228, 59);
            btnMaterial.TabIndex = 3;
            btnMaterial.Text = "Vật tư";
            btnMaterial.UseVisualStyleBackColor = false;
            btnMaterial.Click += btnMaterial_Click;
            // 
            // panel4
            // 
            panel4.Controls.Add(button2);
            panel4.Location = new Point(3, 6);
            panel4.Name = "panel4";
            panel4.Size = new Size(215, 45);
            panel4.TabIndex = 7;
            // 
            // button2
            // 
            button2.BackColor = Color.Black;
            button2.Cursor = Cursors.Hand;
            button2.ForeColor = Color.White;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(-8, -12);
            button2.Name = "button2";
            button2.Padding = new Padding(15, 0, 0, 0);
            button2.Size = new Size(235, 69);
            button2.TabIndex = 3;
            button2.Text = "Home";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(btnCustomer);
            panel7.Location = new Point(3, 57);
            panel7.Name = "panel7";
            panel7.Size = new Size(224, 45);
            panel7.TabIndex = 6;
            // 
            // btnCustomer
            // 
            btnCustomer.BackColor = Color.Black;
            btnCustomer.Cursor = Cursors.Hand;
            btnCustomer.ForeColor = Color.White;
            btnCustomer.Image = Properties.Resources.icons8_user_24;
            btnCustomer.ImageAlign = ContentAlignment.MiddleLeft;
            btnCustomer.Location = new Point(-8, -12);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Padding = new Padding(15, 0, 0, 0);
            btnCustomer.Size = new Size(235, 69);
            btnCustomer.TabIndex = 3;
            btnCustomer.Text = "Khách hàng";
            btnCustomer.UseVisualStyleBackColor = false;
            btnCustomer.Click += btnCustomer_Click;
            // 
            // panel9
            // 
            panel9.Controls.Add(BtnUser);
            panel9.Dock = DockStyle.Bottom;
            panel9.Location = new Point(0, 519);
            panel9.Name = "panel9";
            panel9.Size = new Size(223, 36);
            panel9.TabIndex = 8;
            // 
            // BtnUser
            // 
            BtnUser.BackColor = Color.Black;
            BtnUser.ForeColor = Color.White;
            BtnUser.Image = (Image)resources.GetObject("BtnUser.Image");
            BtnUser.ImageAlign = ContentAlignment.MiddleLeft;
            BtnUser.Location = new Point(-16, -18);
            BtnUser.Name = "BtnUser";
            BtnUser.Padding = new Padding(25, 0, 0, 0);
            BtnUser.Size = new Size(243, 61);
            BtnUser.TabIndex = 3;
            BtnUser.Text = "Tài khoản";
            BtnUser.UseVisualStyleBackColor = false;
            BtnUser.Click += BtnUser_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1150, 600);
            Controls.Add(panel2);
            Controls.Add(panel1);
            IsMdiContainer = true;
            Name = "Form1";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Minimized;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnSidebar).EndInit();
            panel2.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel9.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox btnSidebar;
        private ReaLTaiizor.Controls.NightControlBox nightControlBox1;
        private Panel panel2;
        private Panel panel7;
        private Button btnCustomer;
        private Panel panel9;
        private Button BtnUser;
        private Panel panel3;
        private Button btnMaterial;
        private Panel panel4;
        private Button button2;
        private Panel panel6;
        private Button button5;
        private Panel panel5;
        private Button BtnType;
        private Panel panel8;
        private Button BtnTke;
        private Button button9;
        private Button BtnOrder;
        private Panel panel10;
        private Button BtnNV;
        private Button button11;
    }
}
