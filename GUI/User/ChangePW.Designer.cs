namespace QLVT
{
    partial class ChangePW
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
            if (disposing)
            {
                if (slideTimer != null)
                {
                    slideTimer.Stop();
                    slideTimer.Dispose();
                }
                if (components != null)
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
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            splitter1 = new Splitter();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel5 = new Panel();
            hopeRoundButton2 = new ReaLTaiizor.Controls.HopeRoundButton();
            TbEmail = new ReaLTaiizor.Controls.SmallTextBox();
            panel1 = new Panel();
            hopeRoundButton1 = new ReaLTaiizor.Controls.HopeRoundButton();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            BtnchangePw = new ReaLTaiizor.Controls.HopeRoundButton();
            TbNewPass = new ReaLTaiizor.Controls.SmallTextBox();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            TbToken = new ReaLTaiizor.Controls.SmallTextBox();
            panel2 = new Panel();
            BtnBack = new ReaLTaiizor.Controls.HopeRoundButton();
            paneloverlay = new QLVT.Helper.RoundedPanel();
            Tieude = new ReaLTaiizor.Controls.BigLabel();
            Title = new ReaLTaiizor.Controls.BigLabel();
            panel4 = new Panel();
            bigLabel3 = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel1.SuspendLayout();
            panel5.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            paneloverlay.SuspendLayout();
            SuspendLayout();
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Dock = DockStyle.Fill;
            bigLabel1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(3, 0);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(357, 32);
            bigLabel1.TabIndex = 0;
            bigLabel1.Text = "Cập nhật mật khẩu của bạn";
            bigLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 450);
            splitter1.TabIndex = 1;
            splitter1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(236, 240, 241);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel5, 0, 3);
            tableLayoutPanel1.Controls.Add(TbEmail, 0, 1);
            tableLayoutPanel1.Controls.Add(bigLabel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Location = new Point(9, 106);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(363, 189);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.Controls.Add(hopeRoundButton2);
            panel5.Location = new Point(3, 130);
            panel5.Name = "panel5";
            panel5.Size = new Size(357, 51);
            panel5.TabIndex = 5;
            // 
            // hopeRoundButton2
            // 
            hopeRoundButton2.BorderColor = Color.FromArgb(220, 223, 230);
            hopeRoundButton2.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            hopeRoundButton2.DangerColor = Color.FromArgb(245, 108, 108);
            hopeRoundButton2.DefaultColor = Color.FromArgb(255, 255, 255);
            hopeRoundButton2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            hopeRoundButton2.ForeColor = Color.White;
            hopeRoundButton2.HoverTextColor = Color.FromArgb(48, 49, 51);
            hopeRoundButton2.InfoColor = Color.FromArgb(144, 147, 153);
            hopeRoundButton2.Location = new Point(85, 17);
            hopeRoundButton2.Name = "hopeRoundButton2";
            hopeRoundButton2.PrimaryColor = Color.DarkRed;
            hopeRoundButton2.Size = new Size(177, 31);
            hopeRoundButton2.SuccessColor = Color.FromArgb(103, 194, 58);
            hopeRoundButton2.TabIndex = 5;
            hopeRoundButton2.Text = "Exit";
            hopeRoundButton2.TextColor = Color.White;
            hopeRoundButton2.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton2.Click += parrotPictureBox2_Click;
            // 
            // TbEmail
            // 
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(180, 180, 180);
            TbEmail.CustomBGColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 11F);
            TbEmail.ForeColor = Color.DimGray;
            TbEmail.Location = new Point(3, 35);
            TbEmail.MaxLength = 32767;
            TbEmail.Multiline = false;
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(353, 28);
            TbEmail.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbEmail.TabIndex = 1;
            TbEmail.TextAlignment = HorizontalAlignment.Left;
            TbEmail.UseSystemPasswordChar = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(hopeRoundButton1);
            panel1.Location = new Point(3, 69);
            panel1.Name = "panel1";
            panel1.Size = new Size(357, 55);
            panel1.TabIndex = 0;
            // 
            // hopeRoundButton1
            // 
            hopeRoundButton1.BorderColor = Color.FromArgb(220, 223, 230);
            hopeRoundButton1.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            hopeRoundButton1.DangerColor = Color.FromArgb(245, 108, 108);
            hopeRoundButton1.DefaultColor = Color.FromArgb(255, 255, 255);
            hopeRoundButton1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            hopeRoundButton1.ForeColor = Color.White;
            hopeRoundButton1.HoverTextColor = Color.FromArgb(48, 49, 51);
            hopeRoundButton1.InfoColor = Color.FromArgb(144, 147, 153);
            hopeRoundButton1.Location = new Point(85, 13);
            hopeRoundButton1.Name = "hopeRoundButton1";
            hopeRoundButton1.PrimaryColor = Color.FromArgb(30, 58, 138);
            hopeRoundButton1.Size = new Size(177, 31);
            hopeRoundButton1.SuccessColor = Color.FromArgb(103, 194, 58);
            hopeRoundButton1.TabIndex = 4;
            hopeRoundButton1.Text = "Gửi mã";
            hopeRoundButton1.TextColor = Color.White;
            hopeRoundButton1.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton1.Click += hopeRoundButton1_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 3);
            tableLayoutPanel2.Controls.Add(TbNewPass, 0, 2);
            tableLayoutPanel2.Controls.Add(bigLabel2, 0, 0);
            tableLayoutPanel2.Controls.Add(TbToken, 0, 1);
            tableLayoutPanel2.Controls.Add(panel2, 0, 4);
            tableLayoutPanel2.Enabled = false;
            tableLayoutPanel2.Location = new Point(386, 106);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(359, 225);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(BtnchangePw);
            panel3.Location = new Point(3, 138);
            panel3.Name = "panel3";
            panel3.Size = new Size(353, 38);
            panel3.TabIndex = 3;
            // 
            // BtnchangePw
            // 
            BtnchangePw.BorderColor = Color.FromArgb(220, 223, 230);
            BtnchangePw.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnchangePw.DangerColor = Color.FromArgb(245, 108, 108);
            BtnchangePw.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnchangePw.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnchangePw.ForeColor = Color.White;
            BtnchangePw.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnchangePw.InfoColor = Color.FromArgb(144, 147, 153);
            BtnchangePw.Location = new Point(85, 4);
            BtnchangePw.Name = "BtnchangePw";
            BtnchangePw.PrimaryColor = Color.FromArgb(30, 58, 138);
            BtnchangePw.Size = new Size(180, 31);
            BtnchangePw.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnchangePw.TabIndex = 5;
            BtnchangePw.Text = "Đổi mật khẩu";
            BtnchangePw.TextColor = Color.White;
            BtnchangePw.WarningColor = Color.FromArgb(230, 162, 60);
            // 
            // TbNewPass
            // 
            TbNewPass.BackColor = Color.Transparent;
            TbNewPass.BorderColor = Color.FromArgb(180, 180, 180);
            TbNewPass.CustomBGColor = Color.White;
            TbNewPass.Font = new Font("Tahoma", 11F);
            TbNewPass.ForeColor = Color.DimGray;
            TbNewPass.Location = new Point(3, 92);
            TbNewPass.MaxLength = 32767;
            TbNewPass.Multiline = false;
            TbNewPass.Name = "TbNewPass";
            TbNewPass.ReadOnly = false;
            TbNewPass.Size = new Size(353, 28);
            TbNewPass.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbNewPass.TabIndex = 2;
            TbNewPass.TextAlignment = HorizontalAlignment.Left;
            TbNewPass.UseSystemPasswordChar = false;
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Dock = DockStyle.Fill;
            bigLabel2.Font = new Font("Segoe UI Semibold", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(3, 0);
            bigLabel2.Name = "bigLabel2";
            bigLabel2.Size = new Size(353, 45);
            bigLabel2.TabIndex = 0;
            bigLabel2.Text = "Đổi mật khẩu";
            bigLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TbToken
            // 
            TbToken.BackColor = Color.Transparent;
            TbToken.BorderColor = Color.FromArgb(180, 180, 180);
            TbToken.CustomBGColor = Color.White;
            TbToken.Font = new Font("Tahoma", 11F);
            TbToken.ForeColor = Color.DimGray;
            TbToken.Location = new Point(3, 48);
            TbToken.MaxLength = 32767;
            TbToken.Multiline = false;
            TbToken.Name = "TbToken";
            TbToken.ReadOnly = false;
            TbToken.Size = new Size(353, 28);
            TbToken.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbToken.TabIndex = 1;
            TbToken.TextAlignment = HorizontalAlignment.Left;
            TbToken.UseSystemPasswordChar = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(BtnBack);
            panel2.Location = new Point(3, 183);
            panel2.Name = "panel2";
            panel2.Size = new Size(353, 38);
            panel2.TabIndex = 0;
            // 
            // BtnBack
            // 
            BtnBack.BorderColor = Color.FromArgb(220, 223, 230);
            BtnBack.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnBack.DangerColor = Color.FromArgb(245, 108, 108);
            BtnBack.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnBack.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnBack.ForeColor = Color.White;
            BtnBack.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnBack.InfoColor = Color.FromArgb(144, 147, 153);
            BtnBack.Location = new Point(85, 4);
            BtnBack.Name = "BtnBack";
            BtnBack.PrimaryColor = Color.FromArgb(30, 58, 138);
            BtnBack.Size = new Size(180, 31);
            BtnBack.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnBack.TabIndex = 5;
            BtnBack.Text = "Quay lại";
            BtnBack.TextColor = Color.White;
            BtnBack.WarningColor = Color.FromArgb(230, 162, 60);
            // 
            // paneloverlay
            // 
            paneloverlay.BackColor = Color.FromArgb(30, 58, 138);
            paneloverlay.BorderRadius = 20;
            paneloverlay.Controls.Add(bigLabel3);
            paneloverlay.Controls.Add(Tieude);
            paneloverlay.Controls.Add(Title);
            paneloverlay.Location = new Point(381, 0);
            paneloverlay.Name = "paneloverlay";
            paneloverlay.Size = new Size(369, 450);
            paneloverlay.TabIndex = 4;
            // 
            // Tieude
            // 
            Tieude.AutoSize = true;
            Tieude.BackColor = Color.Transparent;
            Tieude.Font = new Font("Segoe UI", 25F);
            Tieude.ForeColor = Color.White;
            Tieude.Location = new Point(94, 211);
            Tieude.Name = "Tieude";
            Tieude.Size = new Size(0, 46);
            Tieude.TabIndex = 1;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.BackColor = Color.Transparent;
            Title.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Title.ForeColor = Color.White;
            Title.Location = new Point(-66, 310);
            Title.Name = "Title";
            Title.Size = new Size(0, 30);
            Title.TabIndex = 0;
            Title.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Transparent;
            panel4.Location = new Point(12, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(710, 34);
            panel4.TabIndex = 6;
            // 
            // bigLabel3
            // 
            bigLabel3.AutoSize = true;
            bigLabel3.BackColor = Color.Transparent;
            bigLabel3.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel3.ForeColor = Color.White;
            bigLabel3.Location = new Point(81, 188);
            bigLabel3.Name = "bigLabel3";
            bigLabel3.Size = new Size(232, 64);
            bigLabel3.TabIndex = 2;
            bigLabel3.Text = "Nhập email của bạn\r\n để đổi mật khẩu";
            // 
            // ChangePW
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(750, 450);
            Controls.Add(panel4);
            Controls.Add(paneloverlay);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(splitter1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ChangePW";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            Load += ChangePW_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel5.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            paneloverlay.ResumeLayout(false);
            paneloverlay.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private Splitter splitter1;
        private TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.SmallTextBox TbEmail;
        private Panel panel1;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton1;
        private TableLayoutPanel tableLayoutPanel2;
        private ReaLTaiizor.Controls.SmallTextBox TbNewPass;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.SmallTextBox TbToken;
        private Panel panel2;
        private Panel panel3;
        private ReaLTaiizor.Controls.HopeRoundButton BtnchangePw;
        private ReaLTaiizor.Controls.HopeRoundButton BtnBack;
        private Helper.RoundedPanel paneloverlay;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton2;
        private Panel panel4;
        private Panel panel5;
        private ReaLTaiizor.Controls.BigLabel Title;
        private ReaLTaiizor.Controls.BigLabel Tieude;
        private ReaLTaiizor.Controls.BigLabel bigLabel3;
    }
}