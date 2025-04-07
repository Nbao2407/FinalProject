namespace QLVT.Order
{
    partial class PopupOrder
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Ngay = new ReaLTaiizor.Controls.SmallTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lbld = new ReaLTaiizor.Controls.BigLabel();
            roundedPictureBox1 = new QLVT.Helper.RoundedPictureBox();
            panel5 = new Panel();
            BtnDisable = new ReaLTaiizor.Controls.AloneButton();
            BtnHuy = new ReaLTaiizor.Controls.AloneButton();
            dtpNgayTao = new ReaLTaiizor.Controls.PoisonDateTime();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            txtNgTao = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel3 = new ReaLTaiizor.Controls.BigLabel();
            CbNgNhap = new ComboBox();
            dataGridView = new ReaLTaiizor.Controls.PoisonDataGridView();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel4 = new ReaLTaiizor.Controls.BigLabel();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            lblSoLuong = new Label();
            lblTongCong = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // Ngay
            // 
            Ngay.BackColor = Color.Transparent;
            Ngay.BorderColor = Color.FromArgb(180, 180, 180);
            Ngay.CustomBGColor = Color.Transparent;
            Ngay.Font = new Font("Tahoma", 11F);
            Ngay.ForeColor = Color.Black;
            Ngay.Location = new Point(274, 107);
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
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.97938F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.02062F));
            tableLayoutPanel1.Controls.Add(lbld, 0, 0);
            tableLayoutPanel1.Location = new Point(18, 44);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(291, 41);
            tableLayoutPanel1.TabIndex = 69;
            // 
            // lbld
            // 
            lbld.AutoSize = true;
            lbld.BackColor = Color.Transparent;
            lbld.Dock = DockStyle.Left;
            lbld.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbld.ForeColor = Color.FromArgb(80, 80, 80);
            lbld.Location = new Point(3, 0);
            lbld.Name = "lbld";
            lbld.Size = new Size(48, 41);
            lbld.TabIndex = 9;
            lbld.Text = "Mã";
            lbld.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BorderColor = Color.Transparent;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 0F;
            roundedPictureBox1.ErrorImage = null;
            roundedPictureBox1.Image = GUI.Properties.Resources.icons8_close_26;
            roundedPictureBox1.InitialImage = null;
            roundedPictureBox1.Location = new Point(1012, 3);
            roundedPictureBox1.Name = "roundedPictureBox1";
            roundedPictureBox1.Size = new Size(26, 26);
            roundedPictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox1.TabIndex = 41;
            roundedPictureBox1.TabStop = false;
            roundedPictureBox1.Click += roundedPictureBox1_Click;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(30, 58, 138);
            panel5.Controls.Add(roundedPictureBox1);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1050, 38);
            panel5.TabIndex = 64;
            // 
            // BtnDisable
            // 
            BtnDisable.BackColor = Color.Transparent;
            BtnDisable.EnabledCalc = true;
            BtnDisable.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDisable.ForeColor = Color.Black;
            BtnDisable.Location = new Point(918, 582);
            BtnDisable.Name = "BtnDisable";
            BtnDisable.Size = new Size(120, 30);
            BtnDisable.TabIndex = 62;
            BtnDisable.Text = "Ngừng hoạt động";
            // 
            // BtnHuy
            // 
            BtnHuy.BackColor = Color.Transparent;
            BtnHuy.EnabledCalc = true;
            BtnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnHuy.ForeColor = Color.Black;
            BtnHuy.Location = new Point(18, 582);
            BtnHuy.Name = "BtnHuy";
            BtnHuy.Size = new Size(67, 30);
            BtnHuy.TabIndex = 61;
            BtnHuy.Text = "Hủy";
            // 
            // dtpNgayTao
            // 
            dtpNgayTao.FontWeight = ReaLTaiizor.Extension.Poison.PoisonDateTimeWeight.Bold;
            dtpNgayTao.Location = new Point(274, 107);
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
            BtnEdit.Location = new Point(788, 582);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.PrimaryColor = Color.FromArgb(39, 174, 97);
            BtnEdit.Size = new Size(114, 30);
            BtnEdit.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnEdit.TabIndex = 63;
            BtnEdit.Text = "Mở phiếu";
            BtnEdit.TextColor = Color.White;
            BtnEdit.WarningColor = Color.FromArgb(230, 162, 60);
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(179, 107);
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
            txtNgTao.Location = new Point(111, 107);
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
            bigLabel5.Location = new Point(18, 107);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(87, 21);
            bigLabel5.TabIndex = 54;
            bigLabel5.Text = "Người tạo :";
            // 
            // bigLabel3
            // 
            bigLabel3.AutoSize = true;
            bigLabel3.BackColor = Color.Transparent;
            bigLabel3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel3.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel3.Location = new Point(545, 107);
            bigLabel3.Name = "bigLabel3";
            bigLabel3.Size = new Size(96, 21);
            bigLabel3.TabIndex = 74;
            bigLabel3.Text = "Người nhập:";
            // 
            // CbNgNhap
            // 
            CbNgNhap.FormattingEnabled = true;
            CbNgNhap.Location = new Point(647, 109);
            CbNgNhap.Name = "CbNgNhap";
            CbNgNhap.Size = new Size(170, 23);
            CbNgNhap.TabIndex = 76;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Anchor = AnchorStyles.None;
            dataGridView.BackgroundColor = Color.Gray;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridView.GridColor = Color.FromArgb(255, 255, 255);
            dataGridView.Location = new Point(18, 259);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1015, 175);
            dataGridView.TabIndex = 77;
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.SkyBlue;
            bigLabel1.Location = new Point(111, 146);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(20, 21);
            bigLabel1.TabIndex = 79;
            bigLabel1.Text = "X";
            // 
            // bigLabel4
            // 
            bigLabel4.AutoSize = true;
            bigLabel4.BackColor = Color.Transparent;
            bigLabel4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel4.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel4.Location = new Point(21, 146);
            bigLabel4.Name = "bigLabel4";
            bigLabel4.Size = new Size(76, 21);
            bigLabel4.TabIndex = 78;
            bigLabel4.Text = "Tên NCC :";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaptionText;
            panel1.Location = new Point(0, 561);
            panel1.Name = "panel1";
            panel1.Size = new Size(1050, 1);
            panel1.TabIndex = 80;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(829, 473);
            label1.Name = "label1";
            label1.Size = new Size(125, 17);
            label1.TabIndex = 0;
            label1.Text = "Số lượng mặt hàng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(829, 509);
            label2.Name = "label2";
            label2.Size = new Size(73, 17);
            label2.TabIndex = 1;
            label2.Text = "Tổng cộng";
            // 
            // lblSoLuong
            // 
            lblSoLuong.AutoSize = true;
            lblSoLuong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSoLuong.Location = new Point(1002, 475);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(21, 15);
            lblSoLuong.TabIndex = 2;
            lblSoLuong.Text = "21";
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongCong.Location = new Point(1002, 511);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(21, 15);
            lblTongCong.TabIndex = 3;
            lblTongCong.Text = "21";
            // 
            // PopupOrder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 624);
            Controls.Add(lblTongCong);
            Controls.Add(lblSoLuong);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(bigLabel1);
            Controls.Add(label1);
            Controls.Add(bigLabel4);
            Controls.Add(dataGridView);
            Controls.Add(CbNgNhap);
            Controls.Add(bigLabel3);
            Controls.Add(Ngay);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel5);
            Controls.Add(BtnDisable);
            Controls.Add(BtnHuy);
            Controls.Add(dtpNgayTao);
            Controls.Add(BtnEdit);
            Controls.Add(bigLabel2);
            Controls.Add(txtNgTao);
            Controls.Add(bigLabel5);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopupOrder";
            Text = "a";
            Load += PopupOrder_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.SmallTextBox Ngay;
        private TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.BigLabel lbld;
        private Helper.RoundedPictureBox roundedPictureBox1;
        private Panel panel5;
        private ReaLTaiizor.Controls.AloneButton BtnDisable;
        private ReaLTaiizor.Controls.AloneButton BtnHuy;
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgayTao;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel txtNgTao;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.BigLabel bigLabel3;
        private ComboBox CbNgNhap;
        private ReaLTaiizor.Controls.PoisonDataGridView dataGridView;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private ReaLTaiizor.Controls.BigLabel bigLabel4;
        private Panel panel1;
        private Label label1;
        private Label label2;
        private Label lblSoLuong;
        private Label lblTongCong;
    }
}