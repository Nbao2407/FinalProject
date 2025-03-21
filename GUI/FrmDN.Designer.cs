namespace GUI
{
    partial class FrmDN
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDN));
            pictureBox1 = new PictureBox();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox3 = new PictureBox();
            spaceButton1 = new ReaLTaiizor.Controls.SpaceButton();
            btnExit = new Label();
            txtUserName = new TextBox();
            txtPass = new TextBox();
            rdRemenber = new ReaLTaiizor.Controls.AirRadioButton();
            btnQuenMk = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.Sukuna_Manga_black_and_white;
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(95, 47);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(87, 71);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(85, 130);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(103, 40);
            bigLabel1.TabIndex = 1;
            bigLabel1.Text = "Log In";
            // 
            // pictureBox2
            // 
            pictureBox2.ErrorImage = null;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(28, 202);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 29);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Location = new Point(29, 236);
            panel1.Name = "panel1";
            panel1.Size = new Size(236, 1);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Black;
            panel2.Location = new Point(28, 304);
            panel2.Name = "panel2";
            panel2.Size = new Size(236, 1);
            panel2.TabIndex = 5;
            // 
            // pictureBox3
            // 
            pictureBox3.ErrorImage = null;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(27, 270);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(30, 29);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            // 
            // spaceButton1
            // 
            spaceButton1.Customization = "Kioq/zIyMv8yMjL/Kioq/y8vL/8nJyf//v7+/yMjI/8qKir/";
            spaceButton1.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            spaceButton1.Image = null;
            spaceButton1.Location = new Point(27, 360);
            spaceButton1.Name = "spaceButton1";
            spaceButton1.NoRounding = false;
            spaceButton1.Size = new Size(237, 39);
            spaceButton1.TabIndex = 6;
            spaceButton1.Text = "Đăng Nhập";
            spaceButton1.TextAlignment = HorizontalAlignment.Center;
            spaceButton1.Transparent = false;
            spaceButton1.Click += spaceButton1_Click;
            // 
            // btnExit
            // 
            btnExit.AutoSize = true;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(122, 414);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(34, 21);
            btnExit.TabIndex = 7;
            btnExit.Text = "Exit";
            // 
            // txtUserName
            // 
            txtUserName.BackColor = SystemColors.Control;
            txtUserName.BorderStyle = BorderStyle.None;
            txtUserName.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserName.ForeColor = Color.Black;
            txtUserName.Location = new Point(63, 206);
            txtUserName.Multiline = true;
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(200, 24);
            txtUserName.TabIndex = 8;
            // 
            // txtPass
            // 
            txtPass.BackColor = SystemColors.Control;
            txtPass.BorderStyle = BorderStyle.None;
            txtPass.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPass.ForeColor = Color.Black;
            txtPass.Location = new Point(63, 274);
            txtPass.Multiline = true;
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(200, 24);
            txtPass.TabIndex = 9;
            // 
            // rdRemenber
            // 
            rdRemenber.Checked = false;
            rdRemenber.Customization = "PDw8/+3t7f/m5ub/p6en/2RkZP8=";
            rdRemenber.Field = 16;
            rdRemenber.Font = new Font("Segoe UI", 9F);
            rdRemenber.Image = null;
            rdRemenber.Location = new Point(27, 322);
            rdRemenber.Name = "rdRemenber";
            rdRemenber.NoRounding = false;
            rdRemenber.Size = new Size(116, 16);
            rdRemenber.TabIndex = 10;
            rdRemenber.Text = "Ghi nhớ mật khẩu";
            rdRemenber.Transparent = false;
            // 
            // btnQuenMk
            // 
            btnQuenMk.AutoSize = true;
            btnQuenMk.Location = new Point(176, 322);
            btnQuenMk.Name = "btnQuenMk";
            btnQuenMk.Size = new Size(97, 15);
            btnQuenMk.TabIndex = 11;
            btnQuenMk.Text = "Quên mật khẩu ?";
            // 
            // guna2BorderlessForm1
            // 
            // 
            // FrmDN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 447);
            Controls.Add(btnQuenMk);
            Controls.Add(rdRemenber);
            Controls.Add(txtPass);
            Controls.Add(txtUserName);
            Controls.Add(btnExit);
            Controls.Add(spaceButton1);
            Controls.Add(panel2);
            Controls.Add(pictureBox3);
            Controls.Add(panel1);
            Controls.Add(pictureBox2);
            Controls.Add(bigLabel1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmDN";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmDN";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox3;
        private ReaLTaiizor.Controls.SpaceButton spaceButton1;
        private Label btnExit;
        private TextBox txtUserName;
        private TextBox txtPass;
        private ReaLTaiizor.Controls.AirRadioButton rdRemenber;
        private Label btnQuenMk;
    }
}