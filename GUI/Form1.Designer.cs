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
            panel13 = new Panel();
            button10 = new Button();
            button12 = new Button();
            button13 = new Button();
            panel11 = new Panel();
            button3 = new Button();
            button4 = new Button();
            button6 = new Button();
            panel12 = new Panel();
            button7 = new Button();
            button8 = new Button();
            panel10 = new Panel();
            button1 = new Button();
            BtnNV = new Button();
            button11 = new Button();
            panel8 = new Panel();
            BtnTke = new Button();
            button9 = new Button();
            panel6 = new Panel();
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
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            panelMain = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnSidebar).BeginInit();
            panel2.SuspendLayout();
            panel13.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel10.SuspendLayout();
            panel8.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel7.SuspendLayout();
            panel9.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(nightControlBox1);
            panel1.Controls.Add(btnSidebar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1434, 45);
            panel1.TabIndex = 0;
            // 
            // nightControlBox1
            // 
            nightControlBox1.BackColor = Color.Transparent;
            nightControlBox1.CloseHoverColor = Color.FromArgb(199, 80, 80);
            nightControlBox1.CloseHoverForeColor = Color.White;
            nightControlBox1.DefaultLocation = true;
            nightControlBox1.DisableMaximizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.DisableMinimizeColor = Color.FromArgb(105, 105, 105);
            nightControlBox1.Dock = DockStyle.Right;
            nightControlBox1.EnableCloseColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMaximizeButton = true;
            nightControlBox1.EnableMaximizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.EnableMinimizeButton = true;
            nightControlBox1.EnableMinimizeColor = Color.FromArgb(160, 160, 160);
            nightControlBox1.Location = new Point(1295, 0);
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
            btnSidebar.Image = Properties.Resources.icons8_menu_24;
            btnSidebar.Location = new Point(0, 0);
            btnSidebar.Name = "btnSidebar";
            btnSidebar.Size = new Size(55, 45);
            btnSidebar.SizeMode = PictureBoxSizeMode.CenterImage;
            btnSidebar.TabIndex = 2;
            btnSidebar.TabStop = false;
            btnSidebar.Click += ToggleButton_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(44, 62, 80);
            panel2.Controls.Add(panel13);
            panel2.Controls.Add(panel11);
            panel2.Controls.Add(panel12);
            panel2.Controls.Add(panel10);
            panel2.Controls.Add(panel8);
            panel2.Controls.Add(panel6);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel7);
            panel2.Controls.Add(panel9);
            panel2.Dock = DockStyle.Left;
            panel2.ForeColor = SystemColors.ControlText;
            panel2.Location = new Point(0, 45);
            panel2.Name = "panel2";
            panel2.Size = new Size(223, 641);
            panel2.TabIndex = 1;
            // 
            // panel13
            // 
            panel13.Controls.Add(button10);
            panel13.Controls.Add(button12);
            panel13.Controls.Add(button13);
            panel13.Location = new Point(4, 414);
            panel13.Name = "panel13";
            panel13.Size = new Size(215, 45);
            panel13.TabIndex = 14;
            // 
            // button10
            // 
            button10.BackColor = Color.FromArgb(44, 62, 80);
            button10.Cursor = Cursors.Hand;
            button10.ForeColor = Color.White;
            button10.Image = Properties.Resources.icons8_receipt_white;
            button10.ImageAlign = ContentAlignment.MiddleLeft;
            button10.Location = new Point(-9, -7);
            button10.Name = "button10";
            button10.Padding = new Padding(15, 0, 0, 0);
            button10.Size = new Size(228, 59);
            button10.TabIndex = 5;
            button10.Text = "Nhà cung cấp";
            button10.UseVisualStyleBackColor = false;
            button10.Click += Ncc_Click;
            // 
            // button12
            // 
            button12.BackColor = Color.Black;
            button12.Cursor = Cursors.Hand;
            button12.ForeColor = Color.White;
            button12.Image = (Image)resources.GetObject("button12.Image");
            button12.ImageAlign = ContentAlignment.MiddleLeft;
            button12.Location = new Point(-9, -7);
            button12.Name = "button12";
            button12.Padding = new Padding(15, 0, 0, 0);
            button12.Size = new Size(228, 59);
            button12.TabIndex = 4;
            button12.Text = "Nhân sự";
            button12.UseVisualStyleBackColor = false;
            // 
            // button13
            // 
            button13.BackColor = Color.Black;
            button13.Cursor = Cursors.Hand;
            button13.ForeColor = Color.White;
            button13.Image = (Image)resources.GetObject("button13.Image");
            button13.ImageAlign = ContentAlignment.MiddleLeft;
            button13.Location = new Point(-9, -6);
            button13.Name = "button13";
            button13.Padding = new Padding(15, 0, 0, 0);
            button13.Size = new Size(228, 59);
            button13.TabIndex = 3;
            button13.Text = "Thống kê ";
            button13.UseVisualStyleBackColor = false;
            // 
            // panel11
            // 
            panel11.Controls.Add(button3);
            panel11.Controls.Add(button4);
            panel11.Controls.Add(button6);
            panel11.Location = new Point(4, 360);
            panel11.Name = "panel11";
            panel11.Size = new Size(215, 45);
            panel11.TabIndex = 13;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(44, 62, 80);
            button3.Cursor = Cursors.Hand;
            button3.ForeColor = Color.White;
            button3.Image = Properties.Resources.icons8_receipt_white;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(-9, -5);
            button3.Name = "button3";
            button3.Padding = new Padding(15, 0, 0, 0);
            button3.Size = new Size(228, 59);
            button3.TabIndex = 5;
            button3.Text = "Hóa Dơn";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Black;
            button4.Cursor = Cursors.Hand;
            button4.ForeColor = Color.White;
            button4.Image = (Image)resources.GetObject("button4.Image");
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(-9, -7);
            button4.Name = "button4";
            button4.Padding = new Padding(15, 0, 0, 0);
            button4.Size = new Size(228, 59);
            button4.TabIndex = 4;
            button4.Text = "Nhân sự";
            button4.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            button6.BackColor = Color.Black;
            button6.Cursor = Cursors.Hand;
            button6.ForeColor = Color.White;
            button6.Image = (Image)resources.GetObject("button6.Image");
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(-9, -6);
            button6.Name = "button6";
            button6.Padding = new Padding(15, 0, 0, 0);
            button6.Size = new Size(228, 59);
            button6.TabIndex = 3;
            button6.Text = "Thống kê ";
            button6.UseVisualStyleBackColor = false;
            // 
            // panel12
            // 
            panel12.Controls.Add(button7);
            panel12.Controls.Add(button8);
            panel12.Location = new Point(6, 309);
            panel12.Name = "panel12";
            panel12.Size = new Size(215, 45);
            panel12.TabIndex = 12;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(44, 62, 80);
            button7.Cursor = Cursors.Hand;
            button7.ForeColor = Color.White;
            button7.Image = Properties.Resources.icons8_truck_24;
            button7.ImageAlign = ContentAlignment.MiddleLeft;
            button7.Location = new Point(-9, -7);
            button7.Name = "button7";
            button7.Padding = new Padding(15, 0, 0, 0);
            button7.Size = new Size(228, 59);
            button7.TabIndex = 4;
            button7.Text = "Nhập hàng";
            button7.UseVisualStyleBackColor = false;
            button7.Click += BtnOrder_Click;
            // 
            // button8
            // 
            button8.BackColor = Color.Black;
            button8.Cursor = Cursors.Hand;
            button8.ForeColor = Color.White;
            button8.Image = (Image)resources.GetObject("button8.Image");
            button8.ImageAlign = ContentAlignment.MiddleLeft;
            button8.Location = new Point(-9, -6);
            button8.Name = "button8";
            button8.Padding = new Padding(15, 0, 0, 0);
            button8.Size = new Size(228, 59);
            button8.TabIndex = 3;
            button8.Text = "Thống kê ";
            button8.UseVisualStyleBackColor = false;
            // 
            // panel10
            // 
            panel10.Controls.Add(button1);
            panel10.Controls.Add(BtnNV);
            panel10.Controls.Add(button11);
            panel10.Location = new Point(4, 260);
            panel10.Name = "panel10";
            panel10.Size = new Size(215, 45);
            panel10.TabIndex = 11;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(44, 62, 80);
            button1.Cursor = Cursors.Hand;
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(-7, -7);
            button1.Name = "button1";
            button1.Padding = new Padding(15, 0, 0, 0);
            button1.Size = new Size(228, 59);
            button1.TabIndex = 5;
            button1.Text = "Tài khoản";
            button1.UseVisualStyleBackColor = false;
            button1.Click += BtnNV_Click;
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
            panel8.Location = new Point(6, 209);
            panel8.Name = "panel8";
            panel8.Size = new Size(215, 45);
            panel8.TabIndex = 10;
            // 
            // BtnTke
            // 
            BtnTke.BackColor = Color.FromArgb(44, 62, 80);
            BtnTke.Cursor = Cursors.Hand;
            BtnTke.ForeColor = Color.White;
            BtnTke.Image = (Image)resources.GetObject("BtnTke.Image");
            BtnTke.ImageAlign = ContentAlignment.MiddleLeft;
            BtnTke.Location = new Point(-9, -6);
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
            button9.Location = new Point(-9, -8);
            button9.Name = "button9";
            button9.Padding = new Padding(15, 0, 0, 0);
            button9.Size = new Size(228, 59);
            button9.TabIndex = 3;
            button9.Text = "Thống kê ";
            button9.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            panel6.Location = new Point(3, 210);
            panel6.Name = "panel6";
            panel6.Size = new Size(215, 45);
            panel6.TabIndex = 9;
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
            BtnType.BackColor = Color.FromArgb(44, 62, 80);
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
            btnMaterial.BackColor = Color.FromArgb(44, 62, 80);
            btnMaterial.Cursor = Cursors.Hand;
            btnMaterial.ForeColor = Color.White;
            btnMaterial.Image = Properties.Resources.icons8_block_26;
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
            button2.BackColor = Color.FromArgb(44, 62, 80);
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
            btnCustomer.BackColor = Color.FromArgb(44, 62, 80);
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
            panel9.Location = new Point(0, 605);
            panel9.Name = "panel9";
            panel9.Size = new Size(223, 36);
            panel9.TabIndex = 8;
            // 
            // BtnUser
            // 
            BtnUser.BackColor = Color.FromArgb(44, 62, 80);
            BtnUser.ForeColor = Color.White;
            BtnUser.Image = (Image)resources.GetObject("BtnUser.Image");
            BtnUser.ImageAlign = ContentAlignment.MiddleLeft;
            BtnUser.Location = new Point(-16, -18);
            BtnUser.Name = "BtnUser";
            BtnUser.Padding = new Padding(25, 0, 0, 0);
            BtnUser.Size = new Size(243, 61);
            BtnUser.TabIndex = 3;
            BtnUser.Text = "Người dùng";
            BtnUser.UseVisualStyleBackColor = false;
            BtnUser.Click += BtnUser_Click;
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(42, 42, 42);
            panelMain.Location = new Point(225, 44);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1209, 642);
            panelMain.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1434, 686);
            Controls.Add(panelMain);
            Controls.Add(panel2);
            Controls.Add(panel1);
            IsMdiContainer = true;
            Name = "Form1";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnSidebar).EndInit();
            panel2.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel8.ResumeLayout(false);
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
        private Panel panel5;
        private Button BtnType;
        private Button BtnTke;
        private Panel panel10;
        private Button BtnNV;
        private Button button11;
        private Button button1;
        private Panel panel11;
        private Button button3;
        private Button button4;
        private Button button6;
        private Panel panel12;
        private Button button7;
        private Button button8;
        private Panel panel13;
        private Button button10;
        private Button button12;
        private Button button13;
        private Panel panel8;
        private Button button9;
        private Panel panel6;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private Panel panelMain;
    }
}
