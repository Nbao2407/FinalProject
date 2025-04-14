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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            lbld = new ReaLTaiizor.Controls.BigLabel();
            tranthai = new Label();
            roundedPictureBox1 = new QLVT.Helper.RoundedPictureBox();
            panel5 = new Panel();
            BtnDisable = new ReaLTaiizor.Controls.AloneButton();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            txtNgTao = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            bigLabel3 = new ReaLTaiizor.Controls.BigLabel();
            dgvPopupChiTiet = new ReaLTaiizor.Controls.PoisonDataGridView();
            lblNCC = new ReaLTaiizor.Controls.BigLabel();
            bigLabel4 = new ReaLTaiizor.Controls.BigLabel();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            lblSoLuong = new Label();
            lblTongCong = new Label();
            btnDuyet = new ReaLTaiizor.Controls.HopeRoundButton();
            txtGhiChuPopup = new ReaLTaiizor.Controls.AloneTextBox();
            NgayTao = new ReaLTaiizor.Controls.BigLabel();
            lblNgayNhap = new ReaLTaiizor.Controls.BigLabel();
            bigLabel6 = new ReaLTaiizor.Controls.BigLabel();
            NgNhap = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPopupChiTiet).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.8771935F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.1228065F));
            tableLayoutPanel1.Controls.Add(lbld, 0, 0);
            tableLayoutPanel1.Controls.Add(tranthai, 1, 0);
            tableLayoutPanel1.Location = new Point(18, 44);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(313, 41);
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
            // tranthai
            // 
            tranthai.AutoSize = true;
            tranthai.Dock = DockStyle.Left;
            tranthai.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tranthai.Location = new Point(162, 0);
            tranthai.Name = "tranthai";
            tranthai.Size = new Size(102, 41);
            tranthai.TabIndex = 84;
            tranthai.Text = "Trạng thái";
            tranthai.TextAlign = ContentAlignment.MiddleCenter;
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
            BtnDisable.Location = new Point(21, 501);
            BtnDisable.Name = "BtnDisable";
            BtnDisable.Size = new Size(120, 30);
            BtnDisable.TabIndex = 62;
            BtnDisable.Text = "Xoá";
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
            BtnEdit.Location = new Point(910, 501);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.PrimaryColor = Color.FromArgb(39, 174, 97);
            BtnEdit.Size = new Size(114, 30);
            BtnEdit.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnEdit.TabIndex = 63;
            BtnEdit.Text = "Mở phiếu";
            BtnEdit.TextColor = Color.White;
            BtnEdit.WarningColor = Color.FromArgb(230, 162, 60);
            BtnEdit.Click += BtnEdit_Click;
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(273, 105);
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
            bigLabel3.Location = new Point(770, 105);
            bigLabel3.Name = "bigLabel3";
            bigLabel3.Size = new Size(96, 21);
            bigLabel3.TabIndex = 74;
            bigLabel3.Text = "Người nhập:";
            // 
            // dgvPopupChiTiet
            // 
            dgvPopupChiTiet.AllowUserToAddRows = false;
            dgvPopupChiTiet.AllowUserToDeleteRows = false;
            dgvPopupChiTiet.AllowUserToResizeColumns = false;
            dgvPopupChiTiet.AllowUserToResizeRows = false;
            dgvPopupChiTiet.Anchor = AnchorStyles.None;
            dgvPopupChiTiet.BackgroundColor = SystemColors.Control;
            dgvPopupChiTiet.BorderStyle = BorderStyle.None;
            dgvPopupChiTiet.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPopupChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvPopupChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvPopupChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvPopupChiTiet.DefaultCellStyle = dataGridViewCellStyle5;
            dgvPopupChiTiet.EnableHeadersVisualStyles = false;
            dgvPopupChiTiet.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dgvPopupChiTiet.GridColor = Color.FromArgb(255, 255, 255);
            dgvPopupChiTiet.Location = new Point(18, 189);
            dgvPopupChiTiet.Name = "dgvPopupChiTiet";
            dgvPopupChiTiet.ReadOnly = true;
            dgvPopupChiTiet.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvPopupChiTiet.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvPopupChiTiet.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvPopupChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPopupChiTiet.Size = new Size(1006, 175);
            dgvPopupChiTiet.TabIndex = 77;
            // 
            // lblNCC
            // 
            lblNCC.AutoSize = true;
            lblNCC.BackColor = Color.Transparent;
            lblNCC.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNCC.ForeColor = Color.SkyBlue;
            lblNCC.Location = new Point(111, 146);
            lblNCC.Name = "lblNCC";
            lblNCC.Size = new Size(20, 21);
            lblNCC.TabIndex = 79;
            lblNCC.Text = "X";
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
            panel1.Location = new Point(14, 480);
            panel1.Name = "panel1";
            panel1.Size = new Size(1050, 1);
            panel1.TabIndex = 80;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(732, 401);
            label1.Name = "label1";
            label1.Size = new Size(125, 17);
            label1.TabIndex = 0;
            label1.Text = "Số lượng mặt hàng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(784, 437);
            label2.Name = "label2";
            label2.Size = new Size(73, 17);
            label2.TabIndex = 1;
            label2.Text = "Tổng cộng";
            // 
            // lblSoLuong
            // 
            lblSoLuong.AutoSize = true;
            lblSoLuong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSoLuong.Location = new Point(929, 403);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(21, 15);
            lblSoLuong.TabIndex = 2;
            lblSoLuong.Text = "21";
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongCong.Location = new Point(929, 439);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(21, 15);
            lblTongCong.TabIndex = 3;
            lblTongCong.Text = "21";
            // 
            // btnDuyet
            // 
            btnDuyet.BorderColor = Color.FromArgb(220, 223, 230);
            btnDuyet.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            btnDuyet.DangerColor = Color.FromArgb(245, 108, 108);
            btnDuyet.DefaultColor = Color.FromArgb(255, 255, 255);
            btnDuyet.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDuyet.HoverTextColor = Color.FromArgb(48, 49, 51);
            btnDuyet.InfoColor = Color.FromArgb(144, 147, 153);
            btnDuyet.Location = new Point(768, 501);
            btnDuyet.Name = "btnDuyet";
            btnDuyet.PrimaryColor = Color.FromArgb(39, 174, 97);
            btnDuyet.Size = new Size(114, 30);
            btnDuyet.SuccessColor = Color.FromArgb(103, 194, 58);
            btnDuyet.TabIndex = 81;
            btnDuyet.Text = "Duyệt";
            btnDuyet.TextColor = Color.White;
            btnDuyet.WarningColor = Color.FromArgb(230, 162, 60);
            // 
            // txtGhiChuPopup
            // 
            txtGhiChuPopup.BackColor = Color.Transparent;
            txtGhiChuPopup.EnabledCalc = true;
            txtGhiChuPopup.Font = new Font("Segoe UI", 9F);
            txtGhiChuPopup.ForeColor = Color.Black;
            txtGhiChuPopup.Location = new Point(18, 370);
            txtGhiChuPopup.MaxLength = 32767;
            txtGhiChuPopup.MultiLine = false;
            txtGhiChuPopup.Name = "txtGhiChuPopup";
            txtGhiChuPopup.ReadOnly = true;
            txtGhiChuPopup.Size = new Size(313, 84);
            txtGhiChuPopup.TabIndex = 83;
            txtGhiChuPopup.TextAlign = HorizontalAlignment.Left;
            txtGhiChuPopup.UseSystemPasswordChar = false;
            // 
            // NgayTao
            // 
            NgayTao.AutoSize = true;
            NgayTao.BackColor = Color.Transparent;
            NgayTao.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NgayTao.ForeColor = Color.FromArgb(80, 80, 80);
            NgayTao.Location = new Point(359, 107);
            NgayTao.Name = "NgayTao";
            NgayTao.Size = new Size(78, 21);
            NgayTao.TabIndex = 85;
            NgayTao.Text = "7/4/2025";
            // 
            // lblNgayNhap
            // 
            lblNgayNhap.AutoSize = true;
            lblNgayNhap.BackColor = Color.Transparent;
            lblNgayNhap.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNgayNhap.ForeColor = Color.FromArgb(80, 80, 80);
            lblNgayNhap.Location = new Point(617, 107);
            lblNgayNhap.Name = "lblNgayNhap";
            lblNgayNhap.Size = new Size(78, 21);
            lblNgayNhap.TabIndex = 87;
            lblNgayNhap.Text = "7/4/2025";
            // 
            // bigLabel6
            // 
            bigLabel6.AutoSize = true;
            bigLabel6.BackColor = Color.Transparent;
            bigLabel6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel6.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel6.Location = new Point(518, 107);
            bigLabel6.Name = "bigLabel6";
            bigLabel6.Size = new Size(93, 21);
            bigLabel6.TabIndex = 86;
            bigLabel6.Text = "Ngày nhập :";
            // 
            // NgNhap
            // 
            NgNhap.AutoSize = true;
            NgNhap.BackColor = Color.Transparent;
            NgNhap.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NgNhap.ForeColor = Color.FromArgb(80, 80, 80);
            NgNhap.Location = new Point(872, 105);
            NgNhap.Name = "NgNhap";
            NgNhap.Size = new Size(78, 21);
            NgNhap.TabIndex = 88;
            NgNhap.Text = "7/4/2025";
            // 
            // PopupOrder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 561);
            Controls.Add(dgvPopupChiTiet);
            Controls.Add(NgNhap);
            Controls.Add(lblNgayNhap);
            Controls.Add(bigLabel6);
            Controls.Add(NgayTao);
            Controls.Add(txtGhiChuPopup);
            Controls.Add(btnDuyet);
            Controls.Add(lblTongCong);
            Controls.Add(lblSoLuong);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(lblNCC);
            Controls.Add(label1);
            Controls.Add(bigLabel4);
            Controls.Add(bigLabel3);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel5);
            Controls.Add(BtnDisable);
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
            ((System.ComponentModel.ISupportInitialize)dgvPopupChiTiet).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.BigLabel lbld;
        private Helper.RoundedPictureBox roundedPictureBox1;
        private Panel panel5;
        private ReaLTaiizor.Controls.AloneButton BtnDisable;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel txtNgTao;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.BigLabel bigLabel3;
        private ReaLTaiizor.Controls.PoisonDataGridView dgvPopupChiTiet;
        private ReaLTaiizor.Controls.BigLabel lblNCC;
        private ReaLTaiizor.Controls.BigLabel bigLabel4;
        private Panel panel1;
        private Label label1;
        private Label label2;
        private Label lblSoLuong;
        private Label lblTongCong;
        private ReaLTaiizor.Controls.HopeRoundButton btnDuyet;
        private ReaLTaiizor.Controls.AloneTextBox txtGhiChuPopup;
        private Label tranthai;
        private ReaLTaiizor.Controls.BigLabel NgayTao;
        private ReaLTaiizor.Controls.BigLabel lblNgayNhap;
        private ReaLTaiizor.Controls.BigLabel bigLabel6;
        private ReaLTaiizor.Controls.BigLabel NgNhap;
    }
}