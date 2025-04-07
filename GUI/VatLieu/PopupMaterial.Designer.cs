namespace QLVT.VatLieu
{
    partial class PopupMaterial
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
            TbGiaNhap = new ReaLTaiizor.Controls.SmallTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblID = new Label();
            lblName = new ReaLTaiizor.Controls.BigLabel();
            panel5 = new Panel();
            roundedPictureBox2 = new QLVT.Helper.RoundedPictureBox();
            BtnDelete = new ReaLTaiizor.Controls.AloneButton();
            txtEmail = new Label();
            dtpNgayTao = new ReaLTaiizor.Controls.PoisonDateTime();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            label2 = new Label();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            txtNgTao = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            TbGiaBan = new ReaLTaiizor.Controls.SmallTextBox();
            label1 = new Label();
            lblType = new Label();
            lblKho = new Label();
            TbNote = new ReaLTaiizor.Controls.DungeonRichTextBox();
            groupBox1 = new GroupBox();
            pictureBox2 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // Ngay
            // 
            Ngay.BackColor = Color.Transparent;
            Ngay.BorderColor = Color.FromArgb(180, 180, 180);
            Ngay.CustomBGColor = Color.Transparent;
            Ngay.Font = new Font("Tahoma", 11F);
            Ngay.ForeColor = Color.Black;
            Ngay.Location = new Point(286, 97);
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
            // TbGiaNhap
            // 
            TbGiaNhap.BackColor = Color.Transparent;
            TbGiaNhap.BorderColor = Color.FromArgb(180, 180, 180);
            TbGiaNhap.CustomBGColor = Color.Transparent;
            TbGiaNhap.Font = new Font("Tahoma", 11F);
            TbGiaNhap.ForeColor = Color.DimGray;
            TbGiaNhap.Location = new Point(32, 212);
            TbGiaNhap.MaxLength = 32767;
            TbGiaNhap.Multiline = false;
            TbGiaNhap.Name = "TbGiaNhap";
            TbGiaNhap.ReadOnly = true;
            TbGiaNhap.Size = new Size(219, 28);
            TbGiaNhap.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbGiaNhap.TabIndex = 74;
            TbGiaNhap.TextAlignment = HorizontalAlignment.Left;
            TbGiaNhap.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.97938F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.02062F));
            tableLayoutPanel1.Controls.Add(lblID, 1, 0);
            tableLayoutPanel1.Controls.Add(lblName, 0, 0);
            tableLayoutPanel1.Location = new Point(191, 44);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(407, 41);
            tableLayoutPanel1.TabIndex = 69;
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Dock = DockStyle.Left;
            lblID.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblID.ForeColor = Color.Cyan;
            lblID.ImageAlign = ContentAlignment.MiddleRight;
            lblID.Location = new Point(271, 0);
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
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(30, 58, 138);
            panel5.Controls.Add(roundedPictureBox2);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(815, 38);
            panel5.TabIndex = 64;
            // 
            // roundedPictureBox2
            // 
            roundedPictureBox2.BorderColor = Color.Transparent;
            roundedPictureBox2.BorderRadius = 20;
            roundedPictureBox2.BorderThickness = 0F;
            roundedPictureBox2.ErrorImage = null;
            roundedPictureBox2.Image = GUI.Properties.Resources.icons8_close_26;
            roundedPictureBox2.InitialImage = null;
            roundedPictureBox2.Location = new Point(778, 3);
            roundedPictureBox2.Name = "roundedPictureBox2";
            roundedPictureBox2.Size = new Size(26, 26);
            roundedPictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox2.TabIndex = 42;
            roundedPictureBox2.TabStop = false;
            roundedPictureBox2.Click += roundedPictureBox2_Click;
            // 
            // BtnDelete
            // 
            BtnDelete.BackColor = Color.Transparent;
            BtnDelete.EnabledCalc = true;
            BtnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDelete.ForeColor = Color.Black;
            BtnDelete.Location = new Point(22, 339);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(67, 30);
            BtnDelete.TabIndex = 61;
            BtnDelete.Text = "Xóa";
            // 
            // txtEmail
            // 
            txtEmail.AutoSize = true;
            txtEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(308, 183);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(63, 21);
            txtEmail.TabIndex = 58;
            txtEmail.Text = "Giá bán";
            // 
            // dtpNgayTao
            // 
            dtpNgayTao.FontWeight = ReaLTaiizor.Extension.Poison.PoisonDateTimeWeight.Bold;
            dtpNgayTao.Location = new Point(286, 99);
            dtpNgayTao.MinimumSize = new Size(0, 29);
            dtpNgayTao.Name = "dtpNgayTao";
            dtpNgayTao.RightToLeft = RightToLeft.No;
            dtpNgayTao.Size = new Size(202, 29);
            dtpNgayTao.TabIndex = 68;
            dtpNgayTao.Visible = false;
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
            BtnEdit.Location = new Point(687, 339);
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
            label2.Location = new Point(33, 182);
            label2.Name = "label2";
            label2.Size = new Size(72, 21);
            label2.TabIndex = 57;
            label2.Text = "Giá nhập";
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(198, 101);
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
            txtNgTao.Location = new Point(620, 103);
            txtNgTao.Name = "txtNgTao";
            txtNgTao.Size = new Size(20, 21);
            txtNgTao.TabIndex = 55;
            txtNgTao.Text = "X";
            txtNgTao.Click += txtNgTao_Click;
            // 
            // bigLabel5
            // 
            bigLabel5.AutoSize = true;
            bigLabel5.BackColor = Color.Transparent;
            bigLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel5.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel5.Location = new Point(527, 103);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(87, 21);
            bigLabel5.TabIndex = 54;
            bigLabel5.Text = "Người tạo :";
            // 
            // TbGiaBan
            // 
            TbGiaBan.BackColor = Color.Transparent;
            TbGiaBan.BorderColor = Color.FromArgb(180, 180, 180);
            TbGiaBan.CustomBGColor = Color.Transparent;
            TbGiaBan.Font = new Font("Tahoma", 11F);
            TbGiaBan.ForeColor = Color.DimGray;
            TbGiaBan.Location = new Point(308, 212);
            TbGiaBan.MaxLength = 32767;
            TbGiaBan.Multiline = false;
            TbGiaBan.Name = "TbGiaBan";
            TbGiaBan.ReadOnly = true;
            TbGiaBan.Size = new Size(219, 28);
            TbGiaBan.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            TbGiaBan.TabIndex = 76;
            TbGiaBan.TextAlignment = HorizontalAlignment.Left;
            TbGiaBan.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(198, 141);
            label1.Name = "label1";
            label1.Size = new Size(100, 21);
            label1.TabIndex = 77;
            label1.Text = "Loại vật liệu :";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblType.Location = new Point(317, 141);
            lblType.Name = "lblType";
            lblType.Size = new Size(46, 21);
            lblType.TabIndex = 78;
            lblType.Text = "Gạch";
            // 
            // lblKho
            // 
            lblKho.AutoSize = true;
            lblKho.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblKho.Location = new Point(527, 141);
            lblKho.Name = "lblKho";
            lblKho.Size = new Size(37, 21);
            lblKho.TabIndex = 81;
            lblKho.Text = "Kho";
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
            TbNote.ReadOnly = true;
            TbNote.Size = new Size(765, 32);
            TbNote.TabIndex = 42;
            TbNote.TextBackColor = Color.White;
            TbNote.WordWrap = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(TbNote);
            groupBox1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(22, 256);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(782, 69);
            groupBox1.TabIndex = 43;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ghi chú";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.LightGray;
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Location = new Point(33, 44);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 130);
            pictureBox2.TabIndex = 82;
            pictureBox2.TabStop = false;
            // 
            // PopupMaterial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(815, 385);
            Controls.Add(pictureBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblKho);
            Controls.Add(lblType);
            Controls.Add(label1);
            Controls.Add(TbGiaBan);
            Controls.Add(Ngay);
            Controls.Add(TbGiaNhap);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel5);
            Controls.Add(BtnDelete);
            Controls.Add(txtEmail);
            Controls.Add(dtpNgayTao);
            Controls.Add(BtnEdit);
            Controls.Add(label2);
            Controls.Add(bigLabel2);
            Controls.Add(txtNgTao);
            Controls.Add(bigLabel5);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopupMaterial";
            Text = "PopupMaterial";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.SmallTextBox Ngay;
        private ReaLTaiizor.Controls.SmallTextBox TbGiaNhap;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblID;
        private ReaLTaiizor.Controls.BigLabel lblName;
        private Panel panel5;
        private ReaLTaiizor.Controls.AloneButton BtnDelete;
        private Label txtEmail;
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgayTao;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private Label label2;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel txtNgTao;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.SmallTextBox TbGiaBan;
        private Label label1;
        private Label lblType;
        private ReaLTaiizor.Controls.SmallTextBox smallTextBox1;
        private Label lblKho;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbNote;
        private GroupBox groupBox1;
        private Helper.RoundedPictureBox roundedPictureBox2;
        private PictureBox pictureBox2;
    }
}