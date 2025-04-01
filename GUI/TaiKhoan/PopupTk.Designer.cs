namespace GUI.TaiKhoan
{
    partial class PopupTk
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
            Ngay = new ReaLTaiizor.Controls.SmallTextBox();
            email = new ReaLTaiizor.Controls.SmallTextBox();
            address = new ReaLTaiizor.Controls.SmallTextBox();
            TbSdt = new ReaLTaiizor.Controls.SmallTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblID = new Label();
            lblName = new ReaLTaiizor.Controls.BigLabel();
            label1 = new Label();
            dtpNgaySinh = new ReaLTaiizor.Controls.PoisonDateTime();
            roundedPictureBox1 = new Helpler.RoundedPictureBox();
            panel5 = new Panel();
            BtnDisable = new ReaLTaiizor.Controls.AloneButton();
            BtnDelete = new ReaLTaiizor.Controls.AloneButton();
            label4 = new Label();
            txtEmail = new Label();
            dtpNgayTao = new ReaLTaiizor.Controls.PoisonDateTime();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            label2 = new Label();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            txtNgTao = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            label8 = new Label();
            TgTrangthai = new ReaLTaiizor.Controls.ToggleEdit();
            lblChucvu = new Label();
            groupBox2 = new GroupBox();
            TbNote = new ReaLTaiizor.Controls.DungeonRichTextBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            panel5.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // Ngay
            // 
            Ngay.BackColor = Color.Transparent;
            Ngay.BorderColor = Color.FromArgb(180, 180, 180);
            Ngay.CustomBGColor = Color.Transparent;
            Ngay.Font = new Font("Tahoma", 11F);
            Ngay.ForeColor = Color.Black;
            Ngay.Location = new Point(289, 123);
            Ngay.MaxLength = 32767;
            Ngay.Multiline = false;
            Ngay.Name = "Ngay";
            Ngay.ReadOnly = true;
            Ngay.Size = new Size(202, 28);
            Ngay.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Ngay.TabIndex = 73;
            Ngay.TextAlignment = HorizontalAlignment.Left;
            Ngay.UseSystemPasswordChar = false;
            // 
            // email
            // 
            email.BackColor = Color.Transparent;
            email.BorderColor = Color.FromArgb(180, 180, 180);
            email.CustomBGColor = Color.Transparent;
            email.Font = new Font("Tahoma", 11F);
            email.ForeColor = Color.Black;
            email.Location = new Point(33, 282);
            email.MaxLength = 32767;
            email.Multiline = false;
            email.Name = "email";
            email.ReadOnly = false;
            email.Size = new Size(387, 28);
            email.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            email.TabIndex = 71;
            email.TextAlignment = HorizontalAlignment.Left;
            email.UseSystemPasswordChar = false;
            // 
            // address
            // 
            address.BackColor = Color.Transparent;
            address.BorderColor = Color.FromArgb(180, 180, 180);
            address.CustomBGColor = Color.Transparent;
            address.Font = new Font("Tahoma", 11F);
            address.ForeColor = Color.Black;
            address.Location = new Point(32, 363);
            address.MaxLength = 32767;
            address.Multiline = false;
            address.Name = "address";
            address.ReadOnly = false;
            address.Size = new Size(809, 28);
            address.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            address.TabIndex = 70;
            address.TextAlignment = HorizontalAlignment.Left;
            address.UseSystemPasswordChar = false;
            // 
            // TbSdt
            // 
            TbSdt.BackColor = Color.Transparent;
            TbSdt.BorderColor = Color.FromArgb(180, 180, 180);
            TbSdt.CustomBGColor = Color.Transparent;
            TbSdt.Font = new Font("Tahoma", 11F);
            TbSdt.ForeColor = Color.DimGray;
            TbSdt.Location = new Point(33, 201);
            TbSdt.MaxLength = 32767;
            TbSdt.Multiline = false;
            TbSdt.Name = "TbSdt";
            TbSdt.ReadOnly = false;
            TbSdt.Size = new Size(370, 28);
            TbSdt.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbSdt.TabIndex = 74;
            TbSdt.TextAlignment = HorizontalAlignment.Left;
            TbSdt.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.97938F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.02062F));
            tableLayoutPanel1.Controls.Add(lblID, 1, 0);
            tableLayoutPanel1.Controls.Add(lblName, 0, 0);
            tableLayoutPanel1.Location = new Point(37, 72);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(291, 41);
            tableLayoutPanel1.TabIndex = 69;
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Dock = DockStyle.Left;
            lblID.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblID.ImageAlign = ContentAlignment.MiddleRight;
            lblID.Location = new Point(194, 0);
            lblID.Name = "lblID";
            lblID.Size = new Size(22, 41);
            lblID.TabIndex = 45;
            lblID.Text = "ID";
            lblID.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = Color.Transparent;
            lblName.Dock = DockStyle.Left;
            lblName.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblName.ForeColor = Color.FromArgb(80, 80, 80);
            lblName.Location = new Point(3, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(52, 41);
            lblName.TabIndex = 9;
            lblName.Text = "Tên";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(491, 164);
            label1.Name = "label1";
            label1.Size = new Size(82, 21);
            label1.TabIndex = 67;
            label1.Text = "Ngày Sinh";
            label1.Click += label1_Click;
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.CalendarTrailingForeColor = Color.Black;
            dtpNgaySinh.FontWeight = ReaLTaiizor.Extension.Poison.PoisonDateTimeWeight.Bold;
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(491, 200);
            dtpNgaySinh.MinimumSize = new Size(0, 29);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.RightToLeft = RightToLeft.No;
            dtpNgaySinh.Size = new Size(344, 29);
            dtpNgaySinh.TabIndex = 65;
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BorderColor = Color.Transparent;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 0F;
            roundedPictureBox1.ErrorImage = null;
            roundedPictureBox1.Image = Properties.Resources.icons8_close_26;
            roundedPictureBox1.InitialImage = null;
            roundedPictureBox1.Location = new Point(830, 7);
            roundedPictureBox1.Name = "roundedPictureBox1";
            roundedPictureBox1.Size = new Size(26, 26);
            roundedPictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox1.TabIndex = 41;
            roundedPictureBox1.TabStop = false;
            roundedPictureBox1.Click += roundedPictureBox1_Click;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(44, 62, 80);
            panel5.Controls.Add(roundedPictureBox1);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(862, 38);
            panel5.TabIndex = 64;
            // 
            // BtnDisable
            // 
            BtnDisable.BackColor = Color.Transparent;
            BtnDisable.EnabledCalc = true;
            BtnDisable.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDisable.ForeColor = Color.Black;
            BtnDisable.Location = new Point(724, 508);
            BtnDisable.Name = "BtnDisable";
            BtnDisable.Size = new Size(120, 30);
            BtnDisable.TabIndex = 62;
            BtnDisable.Text = "Khóa tài khoản";
            // 
            // BtnDelete
            // 
            BtnDelete.BackColor = Color.Transparent;
            BtnDelete.EnabledCalc = true;
            BtnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDelete.ForeColor = Color.Black;
            BtnDelete.Location = new Point(32, 508);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(67, 30);
            BtnDelete.TabIndex = 61;
            BtnDelete.Text = "Xóa";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(33, 326);
            label4.Name = "label4";
            label4.Size = new Size(60, 21);
            label4.TabIndex = 60;
            label4.Text = "Địa Chỉ";
            // 
            // txtEmail
            // 
            txtEmail.AutoSize = true;
            txtEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(32, 246);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(48, 21);
            txtEmail.TabIndex = 58;
            txtEmail.Text = "Email";
            // 
            // dtpNgayTao
            // 
            dtpNgayTao.FontWeight = ReaLTaiizor.Extension.Poison.PoisonDateTimeWeight.Bold;
            dtpNgayTao.Location = new Point(289, 123);
            dtpNgayTao.MinimumSize = new Size(0, 29);
            dtpNgayTao.Name = "dtpNgayTao";
            dtpNgayTao.RightToLeft = RightToLeft.No;
            dtpNgayTao.Size = new Size(202, 29);
            dtpNgayTao.TabIndex = 68;
            // 
            // BtnEdit
            // 
            BtnEdit.BorderColor = Color.FromArgb(220, 223, 230);
            BtnEdit.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnEdit.DangerColor = Color.FromArgb(245, 108, 108);
            BtnEdit.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnEdit.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnEdit.InfoColor = Color.FromArgb(144, 147, 153);
            BtnEdit.Location = new Point(612, 508);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.PrimaryColor = Color.FromArgb(39, 174, 97);
            BtnEdit.Size = new Size(106, 30);
            BtnEdit.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnEdit.TabIndex = 63;
            BtnEdit.Text = "Chỉnh sửa";
            BtnEdit.TextColor = Color.White;
            BtnEdit.WarningColor = Color.FromArgb(230, 162, 60);
            BtnEdit.Click += BtnEdit_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(33, 172);
            label2.Name = "label2";
            label2.Size = new Size(84, 21);
            label2.TabIndex = 57;
            label2.Text = "Điện Thoại";
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(194, 123);
            bigLabel2.Name = "bigLabel2";
            bigLabel2.Size = new Size(80, 21);
            bigLabel2.TabIndex = 56;
            bigLabel2.Text = "Ngày tạo :";
            // 
            // txtNgTao
            // 
            txtNgTao.AutoSize = true;
            txtNgTao.BackColor = Color.Transparent;
            txtNgTao.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtNgTao.ForeColor = Color.FromArgb(80, 80, 80);
            txtNgTao.Location = new Point(126, 123);
            txtNgTao.Name = "txtNgTao";
            txtNgTao.Size = new Size(20, 21);
            txtNgTao.TabIndex = 55;
            txtNgTao.Text = "X";
            // 
            // bigLabel5
            // 
            bigLabel5.AutoSize = true;
            bigLabel5.BackColor = Color.Transparent;
            bigLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel5.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel5.Location = new Point(33, 123);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(87, 21);
            bigLabel5.TabIndex = 54;
            bigLabel5.Text = "Người tạo :";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(553, 130);
            label8.Name = "label8";
            label8.Size = new Size(69, 17);
            label8.TabIndex = 78;
            label8.Text = "Trạng thái";
            // 
            // TgTrangthai
            // 
            TgTrangthai.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TgTrangthai.Location = new Point(633, 130);
            TgTrangthai.Name = "TgTrangthai";
            TgTrangthai.Size = new Size(41, 23);
            TgTrangthai.TabIndex = 77;
            TgTrangthai.Text = "toggleEdit1";
            TgTrangthai.Toggled = false;
            TgTrangthai.Type = ReaLTaiizor.Controls.ToggleEdit._Type.OnOff;
            // 
            // lblChucvu
            // 
            lblChucvu.AutoSize = true;
            lblChucvu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblChucvu.ForeColor = SystemColors.ActiveCaption;
            lblChucvu.Location = new Point(374, 85);
            lblChucvu.Name = "lblChucvu";
            lblChucvu.Size = new Size(72, 21);
            lblChucvu.TabIndex = 76;
            lblChucvu.Text = "Chức vụ";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(TbNote);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(29, 412);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(812, 74);
            groupBox2.TabIndex = 79;
            groupBox2.TabStop = false;
            groupBox2.Text = "Ghi chú";
            // 
            // TbNote
            // 
            TbNote.AutoWordSelection = false;
            TbNote.BackColor = Color.Transparent;
            TbNote.BorderColor = Color.FromArgb(180, 180, 180);
            TbNote.EdgeColor = Color.White;
            TbNote.Font = new Font("Tahoma", 10F);
            TbNote.ForeColor = Color.Black;
            TbNote.Location = new Point(6, 26);
            TbNote.Name = "TbNote";
            TbNote.ReadOnly = false;
            TbNote.Size = new Size(800, 24);
            TbNote.TabIndex = 42;
            TbNote.TextBackColor = Color.White;
            TbNote.WordWrap = true;
            // 
            // PopupTk
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 559);
            Controls.Add(groupBox2);
            Controls.Add(label8);
            Controls.Add(TgTrangthai);
            Controls.Add(lblChucvu);
            Controls.Add(Ngay);
            Controls.Add(email);
            Controls.Add(address);
            Controls.Add(TbSdt);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label1);
            Controls.Add(dtpNgaySinh);
            Controls.Add(panel5);
            Controls.Add(BtnDisable);
            Controls.Add(BtnDelete);
            Controls.Add(label4);
            Controls.Add(txtEmail);
            Controls.Add(dtpNgayTao);
            Controls.Add(BtnEdit);
            Controls.Add(label2);
            Controls.Add(bigLabel2);
            Controls.Add(txtNgTao);
            Controls.Add(bigLabel5);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopupTk";
            Text = "PopupTk";
            Load += PopupTk_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            panel5.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.SmallTextBox Ngay;
        private ReaLTaiizor.Controls.SmallTextBox email;
        private ReaLTaiizor.Controls.SmallTextBox address;
        private ReaLTaiizor.Controls.SmallTextBox TbSdt;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblID;
        private ReaLTaiizor.Controls.BigLabel lblName;
        private Label label1;
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgaySinh;
        private Helpler.RoundedPictureBox roundedPictureBox1;
        private Panel panel5;
        private ReaLTaiizor.Controls.AloneButton BtnDisable;
        private ReaLTaiizor.Controls.AloneButton BtnDelete;
        private Label label4;
        private Label txtEmail;
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgayTao;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private Label label2;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel txtNgTao;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private Label label8;
        private ReaLTaiizor.Controls.ToggleEdit TgTrangthai;
        private Label lblChucvu;
        private GroupBox groupBox2;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbNote;
    }
}