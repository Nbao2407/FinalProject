namespace QLVT.HoaDon
{
    partial class PopupHoaDon
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
            roundedPictureBox1 = new QLVT.Helper.RoundedPictureBox();
            panel5 = new Panel();
            lblTongCong = new Label();
            lblSoLuong = new Label();
            panel1 = new Panel();
            label2 = new Label();
            label1 = new Label();
            Ngay = new ReaLTaiizor.Controls.SmallTextBox();
            BtnHuy = new ReaLTaiizor.Controls.AloneButton();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            TenKH = new ReaLTaiizor.Controls.BigLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblID = new Label();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            txtNgTao = new ReaLTaiizor.Controls.BigLabel();
            lblTrangthai = new ReaLTaiizor.Controls.BigLabel();
            badge1 = new ReaLTaiizor.Controls.Badge();
            dataGridView = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            panel5.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BorderColor = Color.Transparent;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 0F;
            roundedPictureBox1.ErrorImage = null;
            roundedPictureBox1.Image = GUI.Properties.Resources.icons8_close_26;
            roundedPictureBox1.InitialImage = null;
            roundedPictureBox1.Location = new Point(847, 6);
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
            panel5.Size = new Size(885, 38);
            panel5.TabIndex = 64;
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongCong.Location = new Point(819, 412);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(21, 15);
            lblTongCong.TabIndex = 84;
            lblTongCong.Text = "21";
            // 
            // lblSoLuong
            // 
            lblSoLuong.AutoSize = true;
            lblSoLuong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSoLuong.Location = new Point(819, 376);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(21, 15);
            lblSoLuong.TabIndex = 83;
            lblSoLuong.Text = "21";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaptionText;
            panel1.Location = new Point(12, 448);
            panel1.Name = "panel1";
            panel1.Size = new Size(1050, 1);
            panel1.TabIndex = 100;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(646, 410);
            label2.Name = "label2";
            label2.Size = new Size(73, 17);
            label2.TabIndex = 82;
            label2.Text = "Tổng cộng";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(646, 374);
            label1.Name = "label1";
            label1.Size = new Size(125, 17);
            label1.TabIndex = 81;
            label1.Text = "Số lượng mặt hàng";
            // 
            // Ngay
            // 
            Ngay.BackColor = Color.Transparent;
            Ngay.BorderColor = Color.FromArgb(180, 180, 180);
            Ngay.CustomBGColor = Color.Transparent;
            Ngay.Font = new Font("Tahoma", 11F);
            Ngay.ForeColor = Color.Black;
            Ngay.Location = new Point(422, 113);
            Ngay.MaxLength = 32767;
            Ngay.Multiline = false;
            Ngay.Name = "Ngay";
            Ngay.ReadOnly = true;
            Ngay.Size = new Size(202, 28);
            Ngay.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Ngay.TabIndex = 94;
            Ngay.TextAlignment = HorizontalAlignment.Left;
            Ngay.UseSystemPasswordChar = false;
            // 
            // BtnHuy
            // 
            BtnHuy.BackColor = Color.Transparent;
            BtnHuy.EnabledCalc = true;
            BtnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnHuy.ForeColor = Color.Black;
            BtnHuy.Location = new Point(21, 455);
            BtnHuy.Name = "BtnHuy";
            BtnHuy.Size = new Size(67, 30);
            BtnHuy.TabIndex = 88;
            BtnHuy.Text = "Hủy";
            BtnHuy.Click += BtnHuy_Click;
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
            BtnEdit.Location = new Point(742, 455);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.PrimaryColor = Color.FromArgb(39, 174, 97);
            BtnEdit.Size = new Size(114, 30);
            BtnEdit.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnEdit.TabIndex = 90;
            BtnEdit.Text = "Mở phiếu";
            BtnEdit.TextColor = Color.White;
            BtnEdit.WarningColor = Color.FromArgb(230, 162, 60);
            BtnEdit.Click += BtnEdit_Click;
            // 
            // bigLabel5
            // 
            bigLabel5.AutoSize = true;
            bigLabel5.BackColor = Color.Transparent;
            bigLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel5.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel5.Location = new Point(18, 113);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(87, 21);
            bigLabel5.TabIndex = 85;
            bigLabel5.Text = "Người tạo :";
            // 
            // TenKH
            // 
            TenKH.AutoSize = true;
            TenKH.BackColor = Color.Transparent;
            TenKH.Dock = DockStyle.Left;
            TenKH.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TenKH.ForeColor = Color.FromArgb(80, 80, 80);
            TenKH.Location = new Point(3, 0);
            TenKH.Name = "TenKH";
            TenKH.Size = new Size(87, 41);
            TenKH.TabIndex = 9;
            TenKH.Text = "Tên Kh";
            TenKH.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.97938F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.02062F));
            tableLayoutPanel1.Controls.Add(lblID, 1, 0);
            tableLayoutPanel1.Controls.Add(TenKH, 0, 0);
            tableLayoutPanel1.Location = new Point(18, 50);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(291, 41);
            tableLayoutPanel1.TabIndex = 93;
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
            lblID.TabIndex = 46;
            lblID.Text = "ID";
            lblID.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(332, 113);
            bigLabel2.Name = "bigLabel2";
            bigLabel2.Size = new Size(84, 21);
            bigLabel2.TabIndex = 87;
            bigLabel2.Text = "Ngày bán :";
            // 
            // txtNgTao
            // 
            txtNgTao.AutoSize = true;
            txtNgTao.BackColor = Color.Transparent;
            txtNgTao.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtNgTao.ForeColor = Color.FromArgb(80, 80, 80);
            txtNgTao.Location = new Point(111, 113);
            txtNgTao.Name = "txtNgTao";
            txtNgTao.Size = new Size(20, 21);
            txtNgTao.TabIndex = 86;
            txtNgTao.Text = "X";
            // 
            // lblTrangthai
            // 
            lblTrangthai.AutoSize = true;
            lblTrangthai.BackColor = Color.Transparent;
            lblTrangthai.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTrangthai.ForeColor = Color.FromArgb(80, 80, 80);
            lblTrangthai.Location = new Point(332, 56);
            lblTrangthai.Name = "lblTrangthai";
            lblTrangthai.Size = new Size(109, 30);
            lblTrangthai.TabIndex = 101;
            lblTrangthai.Text = "Trạng thái";
            // 
            // badge1
            // 
            badge1.BGColorA = Color.FromArgb(197, 69, 68);
            badge1.BGColorB = Color.FromArgb(176, 52, 52);
            badge1.BorderColor = Color.FromArgb(205, 70, 66);
            badge1.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            badge1.ForeColor = Color.FromArgb(255, 255, 253);
            badge1.Location = new Point(798, 325);
            badge1.Maximum = 9;
            badge1.Name = "badge1";
            badge1.Size = new Size(8, 8);
            badge1.TabIndex = 102;
            badge1.Text = "badge1";
            badge1.Value = 0;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.BackgroundColor = Color.FromArgb(236, 240, 241);
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(21, 161);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.Size = new Size(835, 199);
            dataGridView.TabIndex = 103;
            // 
            // PopupHoaDon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(885, 498);
            Controls.Add(dataGridView);
            Controls.Add(badge1);
            Controls.Add(lblTrangthai);
            Controls.Add(lblTongCong);
            Controls.Add(lblSoLuong);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Ngay);
            Controls.Add(BtnHuy);
            Controls.Add(BtnEdit);
            Controls.Add(bigLabel5);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(bigLabel2);
            Controls.Add(txtNgTao);
            Controls.Add(panel5);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopupHoaDon";
            StartPosition = FormStartPosition.CenterParent;
            Text = "PopupHoaDon";
            Load += PopupHoaDon_Load;
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            panel5.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Helper.RoundedPictureBox roundedPictureBox1;
        private Panel panel5;
        private Label lblTongCong;
        private Label lblSoLuong;
        private Panel panel1;
        private Label label2;
        private Label label1;
        private ReaLTaiizor.Controls.SmallTextBox Ngay;
        private ReaLTaiizor.Controls.AloneButton BtnHuy;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.BigLabel TenKH;
        private TableLayoutPanel tableLayoutPanel1;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel txtNgTao;
        private Label lblID;
        private ReaLTaiizor.Controls.BigLabel lblTrangthai;
        private ReaLTaiizor.Controls.Badge badge1;
        private DataGridView dataGridView;
    }
}