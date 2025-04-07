using QLVT.Helper;

namespace QLVT
{
    partial class FrmUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUser));
            Avatar = new ReaLTaiizor.Controls.ParrotPictureBox();
            button3 = new ReaLTaiizor.Controls.Button();
            TbName = new ReaLTaiizor.Controls.DungeonTextBox();
            TbEmail = new ReaLTaiizor.Controls.DungeonTextBox();
            TbNote = new ReaLTaiizor.Controls.DungeonTextBox();
            TbPhone = new ReaLTaiizor.Controls.DungeonTextBox();
            roundedPanel1 = new RoundedPanel();
            bigLabel9 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel4 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel3 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            BtnChangePass = new ReaLTaiizor.Controls.Button();
            btnSave = new ReaLTaiizor.Controls.Button();
            btnCancel = new ReaLTaiizor.Controls.Button();
            panel1 = new Panel();
            parrotPictureBox2 = new ReaLTaiizor.Controls.ParrotPictureBox();
            UserName = new ReaLTaiizor.Controls.BigLabel();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            TxtVaitro = new ReaLTaiizor.Controls.BigLabel();
            LblID = new ReaLTaiizor.Controls.BigLabel();
            roundedPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Avatar
            // 
            Avatar.BackColor = Color.Transparent;
            Avatar.BackgroundImageLayout = ImageLayout.Center;
            Avatar.ColorLeft = Color.Transparent;
            Avatar.ColorRight = Color.Transparent;
            Avatar.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Avatar.Cursor = Cursors.No;
            Avatar.FilterAlpha = 200;
            Avatar.FilterEnabled = false;
            Avatar.ForeColor = Color.Transparent;
            Avatar.Image = (Image)resources.GetObject("Avatar.Image");
            Avatar.ImeMode = ImeMode.KatakanaHalf;
            Avatar.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            Avatar.IsElipse = true;
            Avatar.IsParallax = false;
            Avatar.Location = new Point(9, 64);
            Avatar.Name = "Avatar";
            Avatar.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Avatar.Size = new Size(100, 100);
            Avatar.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Avatar.TabIndex = 29;
            Avatar.Text = "parrotPictureBox1";
            Avatar.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.BackColor = Color.Transparent;
            button3.BorderColor = Color.FromArgb(52, 73, 94);
            button3.Cursor = Cursors.Hand;
            button3.EnteredBorderColor = Color.Transparent;
            button3.EnteredColor = Color.FromArgb(32, 34, 37);
            button3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Image = null;
            button3.ImageAlign = ContentAlignment.MiddleRight;
            button3.InactiveColor = Color.FromArgb(44, 62, 80);
            button3.Location = new Point(551, 111);
            button3.Margin = new Padding(3, 3, 160, 3);
            button3.Name = "button3";
            button3.Padding = new Padding(0, 0, 100, 0);
            button3.PressedBorderColor = Color.Green;
            button3.PressedColor = Color.Green;
            button3.RightToLeft = RightToLeft.Yes;
            button3.Size = new Size(162, 28);
            button3.TabIndex = 27;
            button3.Text = "Đổi  avatar";
            button3.TextAlignment = StringAlignment.Center;
            button3.Click += button3_Click;
            // 
            // TbName
            // 
            TbName.BackColor = Color.Transparent;
            TbName.BorderColor = Color.FromArgb(204, 204, 204);
            TbName.EdgeColor = Color.White;
            TbName.Font = new Font("Tahoma", 11F);
            TbName.ForeColor = Color.FromArgb(85, 85, 85);
            TbName.Location = new Point(17, 64);
            TbName.MaxLength = 32767;
            TbName.Multiline = false;
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(300, 28);
            TbName.TabIndex = 27;
            TbName.TextAlignment = HorizontalAlignment.Left;
            TbName.UseSystemPasswordChar = false;
            // 
            // TbEmail
            // 
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(204, 204, 204);
            TbEmail.EdgeColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 11F);
            TbEmail.ForeColor = Color.FromArgb(85, 85, 85);
            TbEmail.Location = new Point(17, 152);
            TbEmail.MaxLength = 32767;
            TbEmail.Multiline = false;
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(300, 28);
            TbEmail.TabIndex = 29;
            TbEmail.TextAlignment = HorizontalAlignment.Left;
            TbEmail.UseSystemPasswordChar = false;
            // 
            // TbNote
            // 
            TbNote.BackColor = Color.Transparent;
            TbNote.BorderColor = Color.FromArgb(204, 204, 204);
            TbNote.EdgeColor = Color.White;
            TbNote.Font = new Font("Tahoma", 11F);
            TbNote.ForeColor = Color.FromArgb(85, 85, 85);
            TbNote.Location = new Point(17, 234);
            TbNote.MaxLength = 32767;
            TbNote.Multiline = false;
            TbNote.Name = "TbNote";
            TbNote.ReadOnly = false;
            TbNote.Size = new Size(666, 28);
            TbNote.TabIndex = 30;
            TbNote.TextAlignment = HorizontalAlignment.Left;
            TbNote.UseSystemPasswordChar = false;
            // 
            // TbPhone
            // 
            TbPhone.BackColor = Color.Transparent;
            TbPhone.BorderColor = Color.FromArgb(204, 204, 204);
            TbPhone.EdgeColor = Color.White;
            TbPhone.Font = new Font("Tahoma", 11F);
            TbPhone.ForeColor = Color.FromArgb(85, 85, 85);
            TbPhone.Location = new Point(383, 152);
            TbPhone.MaxLength = 32767;
            TbPhone.Multiline = false;
            TbPhone.Name = "TbPhone";
            TbPhone.ReadOnly = false;
            TbPhone.Size = new Size(300, 28);
            TbPhone.TabIndex = 32;
            TbPhone.TextAlignment = HorizontalAlignment.Left;
            TbPhone.UseSystemPasswordChar = false;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.FromArgb(217, 217, 217);
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.Controls.Add(TbPhone);
            roundedPanel1.Controls.Add(TbNote);
            roundedPanel1.Controls.Add(TbEmail);
            roundedPanel1.Controls.Add(TbName);
            roundedPanel1.Controls.Add(bigLabel9);
            roundedPanel1.Controls.Add(bigLabel4);
            roundedPanel1.Controls.Add(bigLabel3);
            roundedPanel1.Controls.Add(bigLabel5);
            roundedPanel1.Location = new Point(9, 191);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(704, 280);
            roundedPanel1.TabIndex = 27;
            // 
            // bigLabel9
            // 
            bigLabel9.AccessibleRole = AccessibleRole.None;
            bigLabel9.Anchor = AnchorStyles.None;
            bigLabel9.AutoSize = true;
            bigLabel9.BackColor = Color.Transparent;
            bigLabel9.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel9.ForeColor = Color.FromArgb(30, 42, 56);
            bigLabel9.Location = new Point(17, 197);
            bigLabel9.Name = "bigLabel9";
            bigLabel9.Size = new Size(70, 21);
            bigLabel9.TabIndex = 23;
            bigLabel9.Text = "Ghi Chú";
            // 
            // bigLabel4
            // 
            bigLabel4.AccessibleRole = AccessibleRole.None;
            bigLabel4.Anchor = AnchorStyles.None;
            bigLabel4.AutoSize = true;
            bigLabel4.BackColor = Color.Transparent;
            bigLabel4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel4.ForeColor = Color.FromArgb(30, 42, 56);
            bigLabel4.Location = new Point(17, 116);
            bigLabel4.Name = "bigLabel4";
            bigLabel4.Size = new Size(53, 21);
            bigLabel4.TabIndex = 14;
            bigLabel4.Text = "Email";
            // 
            // bigLabel3
            // 
            bigLabel3.AccessibleRole = AccessibleRole.None;
            bigLabel3.Anchor = AnchorStyles.None;
            bigLabel3.AutoSize = true;
            bigLabel3.BackColor = Color.Transparent;
            bigLabel3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel3.ForeColor = Color.FromArgb(30, 42, 56);
            bigLabel3.Location = new Point(15, 27);
            bigLabel3.Name = "bigLabel3";
            bigLabel3.Size = new Size(125, 21);
            bigLabel3.TabIndex = 6;
            bigLabel3.Text = "Tên Đăng nhập";
            // 
            // bigLabel5
            // 
            bigLabel5.AccessibleRole = AccessibleRole.None;
            bigLabel5.Anchor = AnchorStyles.None;
            bigLabel5.AutoSize = true;
            bigLabel5.BackColor = Color.Transparent;
            bigLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel5.ForeColor = Color.FromArgb(30, 42, 56);
            bigLabel5.Location = new Point(392, 116);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(40, 21);
            bigLabel5.TabIndex = 16;
            bigLabel5.Text = "SĐT";
            // 
            // BtnChangePass
            // 
            BtnChangePass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnChangePass.BackColor = Color.Transparent;
            BtnChangePass.BorderColor = Color.FromArgb(52, 73, 94);
            BtnChangePass.Cursor = Cursors.Hand;
            BtnChangePass.EnteredBorderColor = Color.Transparent;
            BtnChangePass.EnteredColor = Color.FromArgb(32, 34, 37);
            BtnChangePass.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnChangePass.Image = null;
            BtnChangePass.ImageAlign = ContentAlignment.MiddleRight;
            BtnChangePass.InactiveColor = Color.FromArgb(0, 131, 143);
            BtnChangePass.Location = new Point(12, 496);
            BtnChangePass.Margin = new Padding(3, 3, 160, 3);
            BtnChangePass.Name = "BtnChangePass";
            BtnChangePass.Padding = new Padding(0, 0, 100, 0);
            BtnChangePass.PressedBorderColor = Color.Green;
            BtnChangePass.PressedColor = Color.Green;
            BtnChangePass.RightToLeft = RightToLeft.Yes;
            BtnChangePass.Size = new Size(123, 28);
            BtnChangePass.TabIndex = 25;
            BtnChangePass.Text = "Đổi mật khẩu";
            BtnChangePass.TextAlignment = StringAlignment.Center;
            BtnChangePass.Click += BtnChangePass_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.Transparent;
            btnSave.BorderColor = Color.FromArgb(52, 73, 94);
            btnSave.Cursor = Cursors.Hand;
            btnSave.EnteredBorderColor = Color.White;
            btnSave.EnteredColor = Color.FromArgb(32, 34, 37);
            btnSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Image = null;
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.InactiveColor = Color.FromArgb(76, 175, 80);
            btnSave.Location = new Point(590, 496);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Transparent;
            btnSave.PressedColor = Color.Transparent;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(123, 28);
            btnSave.TabIndex = 10;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.BackColor = Color.Transparent;
            btnCancel.BorderColor = Color.FromArgb(52, 73, 94);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.EnteredBorderColor = Color.Transparent;
            btnCancel.EnteredColor = Color.FromArgb(32, 34, 37);
            btnCancel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Image = null;
            btnCancel.ImageAlign = ContentAlignment.MiddleRight;
            btnCancel.InactiveColor = Color.FromArgb(161, 161, 161);
            btnCancel.Location = new Point(448, 496);
            btnCancel.Margin = new Padding(3, 3, 160, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(0, 0, 100, 0);
            btnCancel.PressedBorderColor = Color.Green;
            btnCancel.PressedColor = Color.Green;
            btnCancel.RightToLeft = RightToLeft.Yes;
            btnCancel.Size = new Size(123, 28);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Bỏ qua";
            btnCancel.TextAlignment = StringAlignment.Center;
            btnCancel.Click += btnCancel_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(30, 58, 138);
            panel1.Controls.Add(parrotPictureBox2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(725, 58);
            panel1.TabIndex = 0;
            // 
            // parrotPictureBox2
            // 
            parrotPictureBox2.BackColor = Color.Transparent;
            parrotPictureBox2.BackgroundImageLayout = ImageLayout.Center;
            parrotPictureBox2.ColorLeft = Color.Transparent;
            parrotPictureBox2.ColorRight = Color.Transparent;
            parrotPictureBox2.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            parrotPictureBox2.Cursor = Cursors.Hand;
            parrotPictureBox2.FilterAlpha = 200;
            parrotPictureBox2.FilterEnabled = false;
            parrotPictureBox2.ForeColor = Color.Transparent;
            parrotPictureBox2.Image = (Image)resources.GetObject("parrotPictureBox2.Image");
            parrotPictureBox2.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            parrotPictureBox2.IsElipse = true;
            parrotPictureBox2.IsParallax = false;
            parrotPictureBox2.Location = new Point(689, 12);
            parrotPictureBox2.Name = "parrotPictureBox2";
            parrotPictureBox2.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            parrotPictureBox2.Size = new Size(24, 24);
            parrotPictureBox2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            parrotPictureBox2.TabIndex = 30;
            parrotPictureBox2.Text = "parrotPictureBox2";
            parrotPictureBox2.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            parrotPictureBox2.Click += parrotPictureBox2_Click;
            // 
            // UserName
            // 
            UserName.AutoSize = true;
            UserName.BackColor = Color.Transparent;
            UserName.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UserName.ForeColor = Color.FromArgb(30, 42, 56);
            UserName.Location = new Point(124, 77);
            UserName.Name = "UserName";
            UserName.Size = new Size(132, 32);
            UserName.TabIndex = 2;
            UserName.Text = "UserName";
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(30, 42, 56);
            bigLabel1.Location = new Point(124, 109);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(93, 30);
            bigLabel1.TabIndex = 3;
            bigLabel1.Text = "Chức vụ:";
            // 
            // TxtVaitro
            // 
            TxtVaitro.AutoSize = true;
            TxtVaitro.BackColor = Color.Transparent;
            TxtVaitro.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtVaitro.ForeColor = Color.Coral;
            TxtVaitro.Location = new Point(223, 109);
            TxtVaitro.Name = "TxtVaitro";
            TxtVaitro.Size = new Size(27, 30);
            TxtVaitro.TabIndex = 4;
            TxtVaitro.Text = "X";
            // 
            // LblID
            // 
            LblID.AutoSize = true;
            LblID.BackColor = Color.Transparent;
            LblID.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblID.ForeColor = Color.FromArgb(30, 42, 56);
            LblID.Location = new Point(299, 79);
            LblID.Name = "LblID";
            LblID.Size = new Size(27, 30);
            LblID.TabIndex = 30;
            LblID.Text = "X";
            // 
            // FrmUser
            // 
            AccessibleRole = AccessibleRole.None;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(725, 536);
            Controls.Add(LblID);
            Controls.Add(Avatar);
            Controls.Add(panel1);
            Controls.Add(button3);
            Controls.Add(roundedPanel1);
            Controls.Add(UserName);
            Controls.Add(TxtVaitro);
            Controls.Add(bigLabel1);
            Controls.Add(BtnChangePass);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUser";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmUser";
            TopMost = true;
            Load += FrmUser_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.ParrotPictureBox Avatar;
        private ReaLTaiizor.Controls.Button button3;
        private ReaLTaiizor.Controls.DungeonTextBox TbName;
        private ReaLTaiizor.Controls.DungeonTextBox TbEmail;
        private ReaLTaiizor.Controls.DungeonTextBox TbNote;
        private ReaLTaiizor.Controls.DungeonTextBox TbPhone;
        private RoundedPanel roundedPanel1;
        private ReaLTaiizor.Controls.BigLabel bigLabel9;
        private ReaLTaiizor.Controls.BigLabel bigLabel4;
        private ReaLTaiizor.Controls.BigLabel bigLabel3;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.Button BtnChangePass;
        private ReaLTaiizor.Controls.Button btnSave;
        private ReaLTaiizor.Controls.Button btnCancel;
        private Panel panel1;
        private ReaLTaiizor.Controls.BigLabel UserName;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private ReaLTaiizor.Controls.BigLabel TxtVaitro;
        private ReaLTaiizor.Controls.ParrotPictureBox parrotPictureBox2;
        private ReaLTaiizor.Controls.BigLabel LblID;
    }
}