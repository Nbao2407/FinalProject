namespace QLVT
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;

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

        private void InitializeComponent()
        {
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            splitter1 = new Splitter();
            tableLayoutPanel1 = new TableLayoutPanel();
            Tbpass = new ReaLTaiizor.Controls.SmallTextBox();
            TbEmail = new ReaLTaiizor.Controls.SmallTextBox();
            splitContainer2 = new SplitContainer();
            RdRemenber = new ReaLTaiizor.Controls.AloneCheckBox();
            ForgetPassLink = new ReaLTaiizor.Controls.SmallLabel();
            panel1 = new Panel();
            btnLogin = new ReaLTaiizor.Controls.HopeRoundButton();
            Title = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            BtnchangePw = new ReaLTaiizor.Controls.HopeRoundButton();
            TbNewPass = new ReaLTaiizor.Controls.SmallTextBox();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            TbToken = new ReaLTaiizor.Controls.SmallTextBox();
            panel2 = new Panel();
            BtnBack = new ReaLTaiizor.Controls.HopeRoundButton();
            paneloverlay = new QLVT.Helper.RoundedPanel();
            hopeRoundButton2 = new ReaLTaiizor.Controls.HopeRoundButton();
            panel4 = new Panel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            paneloverlay.SuspendLayout();
            SuspendLayout();

            // bigLabel1
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Dock = DockStyle.Fill;
            bigLabel1.Font = new Font("Segoe UI Semibold", 24.75F, FontStyle.Bold);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(3, 0);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(357, 45);
            bigLabel1.TabIndex = 0; // Đặt TabIndex = 0
            bigLabel1.Text = "Đăng Nhập";
            bigLabel1.TextAlign = ContentAlignment.MiddleCenter;

            // splitter1
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 450);
            splitter1.TabIndex = 0; // Đặt TabIndex = 0
            splitter1.TabStop = false;

            // tableLayoutPanel1
            tableLayoutPanel1.BackColor = Color.FromArgb(236, 240, 241);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(Tbpass, 0, 2);
            tableLayoutPanel1.Controls.Add(TbEmail, 0, 1);
            tableLayoutPanel1.Controls.Add(splitContainer2, 0, 3);
            tableLayoutPanel1.Controls.Add(panel1, 0, 4);
            tableLayoutPanel1.Controls.Add(bigLabel1, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 106);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(363, 264);
            tableLayoutPanel1.TabIndex = 0; // Đặt TabIndex = 0

            // Tbpass
            Tbpass.BackColor = Color.Transparent;
            Tbpass.BorderColor = Color.FromArgb(180, 180, 180);
            Tbpass.CustomBGColor = Color.White;
            Tbpass.Font = new Font("Tahoma", 11F);
            Tbpass.ForeColor = Color.DimGray;
            Tbpass.Location = new Point(3, 92);
            Tbpass.MaxLength = 32767;
            Tbpass.Multiline = false;
            Tbpass.Name = "Tbpass";
            Tbpass.ReadOnly = false;
            Tbpass.Size = new Size(353, 28);
            Tbpass.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Tbpass.TabIndex = 2; // Giữ TabIndex = 2 để ưu tiên sau TbEmail
            Tbpass.Text = "Password";
            Tbpass.TextAlignment = HorizontalAlignment.Left;
            Tbpass.UseSystemPasswordChar = false;
            Tbpass.Enter += Tbpass_Enter;
            Tbpass.Leave += Tbpass_Leave;

            // TbEmail
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(180, 180, 180);
            TbEmail.CustomBGColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 11F);
            TbEmail.ForeColor = Color.DimGray;
            TbEmail.Location = new Point(3, 48);
            TbEmail.MaxLength = 32767;
            TbEmail.Multiline = false;
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(353, 28);
            TbEmail.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbEmail.TabIndex = 1; // Giữ TabIndex = 1 để ưu tiên đầu tiên
            TbEmail.Text = "Email/Username";
            TbEmail.TextAlignment = HorizontalAlignment.Left;
            TbEmail.UseSystemPasswordChar = false;
            TbEmail.Enter += TbEmail_Enter;
            TbEmail.Leave += TbEmail_Leave_1;

            // splitContainer2
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new Point(3, 138);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Size = new Size(353, 24);
            splitContainer2.SplitterDistance = 178;
            splitContainer2.TabIndex = 0; // Đặt TabIndex = 0
            splitContainer2.Panel1.Controls.Add(RdRemenber);
            splitContainer2.Panel2.Controls.Add(ForgetPassLink);

            // RdRemenber
            RdRemenber.BackColor = Color.Transparent;
            RdRemenber.Checked = false;
            RdRemenber.EnabledCalc = true;
            RdRemenber.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            RdRemenber.ForeColor = Color.Black;
            RdRemenber.Location = new Point(4, 3);
            RdRemenber.Name = "RdRemenber";
            RdRemenber.Size = new Size(146, 17);
            RdRemenber.TabIndex = 0; // Đặt TabIndex = 0
            RdRemenber.Text = "Ghi nhớ đăng nhập";

            // ForgetPassLink
            ForgetPassLink.AutoSize = true;
            ForgetPassLink.BackColor = Color.Transparent;
            ForgetPassLink.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            ForgetPassLink.ForeColor = Color.Black;
            ForgetPassLink.Location = new Point(59, 4);
            ForgetPassLink.Name = "ForgetPassLink";
            ForgetPassLink.Size = new Size(109, 17);
            ForgetPassLink.TabIndex = 0; // Đặt TabIndex = 0
            ForgetPassLink.Text = "Quên mật khẩu?";
            ForgetPassLink.Click += ForgetPassLink_Click;

            // panel1
            panel1.Controls.Add(btnLogin);
            panel1.Location = new Point(3, 168);
            panel1.Name = "panel1";
            panel1.Size = new Size(353, 50);
            panel1.TabIndex = 0; // Đặt TabIndex = 0

            // btnLogin
            btnLogin.BorderColor = Color.FromArgb(220, 223, 230);
            btnLogin.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            btnLogin.DangerColor = Color.FromArgb(245, 108, 108);
            btnLogin.DefaultColor = Color.FromArgb(255, 255, 255);
            btnLogin.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.HoverTextColor = Color.FromArgb(48, 49, 51);
            btnLogin.InfoColor = Color.FromArgb(144, 147, 153);
            btnLogin.Location = new Point(85, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.PrimaryColor = Color.FromArgb(30, 58, 138);
            btnLogin.Size = new Size(177, 31);
            btnLogin.SuccessColor = Color.FromArgb(103, 194, 58);
            btnLogin.TabIndex = 0; // Đặt TabIndex = 0
            btnLogin.Text = "Đăng nhập";
            btnLogin.TextColor = Color.White;
            btnLogin.WarningColor = Color.FromArgb(230, 162, 60);
            btnLogin.Click += hopeRoundButton1_Click;
            btnLogin.KeyDown += hopeRoundButton1_KeyDown;

            // Title
            Title.AutoSize = true;
            Title.BackColor = Color.Transparent;
            Title.Font = new Font("Segoe UI", 25F);
            Title.ForeColor = Color.White;
            Title.Location = new Point(85, 196);
            Title.Name = "Title";
            Title.Size = new Size(220, 46);
            Title.TabIndex = 0; // Đặt TabIndex = 0
            Title.Text = "Xin chào ,bạn";

            // tableLayoutPanel2
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 3);
            tableLayoutPanel2.Controls.Add(TbNewPass, 0, 2);
            tableLayoutPanel2.Controls.Add(bigLabel2, 0, 0);
            tableLayoutPanel2.Controls.Add(TbToken, 0, 1);
            tableLayoutPanel2.Controls.Add(panel2, 0, 4);
            tableLayoutPanel2.Location = new Point(386, 106);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(359, 225);
            tableLayoutPanel2.TabIndex = 0; // Đặt TabIndex = 0

            // panel3
            panel3.Controls.Add(BtnchangePw);
            panel3.Location = new Point(3, 138);
            panel3.Name = "panel3";
            panel3.Size = new Size(353, 38);
            panel3.TabIndex = 0; // Đặt TabIndex = 0

            // BtnchangePw
            BtnchangePw.BorderColor = Color.FromArgb(220, 223, 230);
            BtnchangePw.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnchangePw.DangerColor = Color.FromArgb(245, 108, 108);
            BtnchangePw.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnchangePw.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            BtnchangePw.ForeColor = Color.White;
            BtnchangePw.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnchangePw.InfoColor = Color.FromArgb(144, 147, 153);
            BtnchangePw.Location = new Point(85, 4);
            BtnchangePw.Name = "BtnchangePw";
            BtnchangePw.PrimaryColor = Color.FromArgb(30, 58, 138);
            BtnchangePw.Size = new Size(180, 31);
            BtnchangePw.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnchangePw.TabIndex = 0; // Đặt TabIndex = 0
            BtnchangePw.Text = "Đổi mật khẩu";
            BtnchangePw.TextColor = Color.White;
            BtnchangePw.WarningColor = Color.FromArgb(230, 162, 60);

            // TbNewPass
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
            TbNewPass.TabIndex = 0; // Đặt TabIndex = 0 (không cần nhập liệu ở đây)
            TbNewPass.Text = "Mật khẩu mới";
            TbNewPass.TextAlignment = HorizontalAlignment.Left;
            TbNewPass.UseSystemPasswordChar = false;
            TbNewPass.Enter += TbNewPass_Enter;
            TbNewPass.Leave += TbNewpass_Leave;

            // bigLabel2
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Dock = DockStyle.Fill;
            bigLabel2.Font = new Font("Segoe UI Semibold", 24.75F, FontStyle.Bold);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(3, 0);
            bigLabel2.Name = "bigLabel2";
            bigLabel2.Size = new Size(353, 45);
            bigLabel2.TabIndex = 0; // Đặt TabIndex = 0
            bigLabel2.Text = "Đổi mật khẩu";
            bigLabel2.TextAlign = ContentAlignment.MiddleCenter;

            // TbToken
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
            TbToken.TabIndex = 0; // Đặt TabIndex = 0 (không cần nhập liệu ở đây)
            TbToken.Text = "Token";
            TbToken.TextAlignment = HorizontalAlignment.Left;
            TbToken.UseSystemPasswordChar = false;
            TbToken.Enter += TbToken_Enter;
            TbToken.Leave += TbToken_Leave;

            // panel2
            panel2.Controls.Add(BtnBack);
            panel2.Location = new Point(3, 183);
            panel2.Name = "panel2";
            panel2.Size = new Size(353, 38);
            panel2.TabIndex = 0; // Đặt TabIndex = 0

            // BtnBack
            BtnBack.BorderColor = Color.FromArgb(220, 223, 230);
            BtnBack.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnBack.DangerColor = Color.FromArgb(245, 108, 108);
            BtnBack.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnBack.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            BtnBack.ForeColor = Color.White;
            BtnBack.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnBack.InfoColor = Color.DarkGray;
            BtnBack.Location = new Point(85, 4);
            BtnBack.Name = "BtnBack";
            BtnBack.PrimaryColor = Color.Silver;
            BtnBack.Size = new Size(180, 31);
            BtnBack.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnBack.TabIndex = 0; // Đặt TabIndex = 0
            BtnBack.Text = "Quay lại";
            BtnBack.TextColor = Color.White;
            BtnBack.WarningColor = Color.FromArgb(230, 162, 60);

            // paneloverlay
            paneloverlay.BackColor = Color.FromArgb(30, 58, 138);
            paneloverlay.BorderRadius = 20;
            paneloverlay.Controls.Add(Title);
            paneloverlay.Location = new Point(381, 0);
            paneloverlay.Name = "paneloverlay";
            paneloverlay.Size = new Size(369, 450);
            paneloverlay.TabIndex = 0; // Đặt TabIndex = 0

            // hopeRoundButton2
            hopeRoundButton2.BorderColor = Color.FromArgb(220, 223, 230);
            hopeRoundButton2.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            hopeRoundButton2.DangerColor = Color.FromArgb(245, 108, 108);
            hopeRoundButton2.DefaultColor = Color.FromArgb(255, 255, 255);
            hopeRoundButton2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            hopeRoundButton2.ForeColor = Color.White;
            hopeRoundButton2.HoverTextColor = Color.FromArgb(48, 49, 51);
            hopeRoundButton2.InfoColor = Color.FromArgb(144, 147, 153);
            hopeRoundButton2.Location = new Point(100, 330);
            hopeRoundButton2.Name = "hopeRoundButton2";
            hopeRoundButton2.PrimaryColor = Color.Crimson;
            hopeRoundButton2.Size = new Size(177, 31);
            hopeRoundButton2.SuccessColor = Color.FromArgb(103, 194, 58);
            hopeRoundButton2.TabIndex = 0; // Đặt TabIndex = 0
            hopeRoundButton2.Text = "Exit";
            hopeRoundButton2.TextColor = Color.White;
            hopeRoundButton2.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton2.Click += parrotPictureBox2_Click;

            // panel4
            panel4.BackColor = Color.Transparent;
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(3, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(747, 65);
            panel4.TabIndex = 0; // Đặt TabIndex = 0

            // Login
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(750, 450);
            Controls.Add(panel4);
            Controls.Add(hopeRoundButton2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(paneloverlay);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(splitter1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";

            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            paneloverlay.ResumeLayout(false);
            paneloverlay.PerformLayout();
            ResumeLayout(false);
        }

        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private Splitter splitter1;
        private TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.SmallTextBox Tbpass;
        private ReaLTaiizor.Controls.SmallTextBox TbEmail;
        private SplitContainer splitContainer2;
        private ReaLTaiizor.Controls.AloneCheckBox RdRemenber;
        private ReaLTaiizor.Controls.SmallLabel ForgetPassLink;
        private Panel panel1;
        private ReaLTaiizor.Controls.HopeRoundButton btnLogin;
        private TableLayoutPanel tableLayoutPanel2;
        private ReaLTaiizor.Controls.SmallTextBox TbNewPass;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.SmallTextBox TbToken;
        private Panel panel2;
        private Panel panel3;
        private ReaLTaiizor.Controls.HopeRoundButton BtnchangePw;
        private ReaLTaiizor.Controls.HopeRoundButton BtnBack;
        private ReaLTaiizor.Controls.BigLabel Title;
        private Helper.RoundedPanel paneloverlay;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton2;
        private Panel panel4;
    }
}