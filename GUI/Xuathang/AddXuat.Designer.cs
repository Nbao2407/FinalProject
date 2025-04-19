namespace QLVT
{
    partial class AddXuat
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panel1 = new Panel();
            ketqua = new FlowLayoutPanel();
            panel4 = new Panel();
            CbKhoNguon = new ReaLTaiizor.Controls.AloneComboBox();
            label9 = new Label();
            resultKh = new Panel();
            CbKhoDich = new ReaLTaiizor.Controls.AloneComboBox();
            label1 = new Label();
            TgTrangthai = new ReaLTaiizor.Controls.ForeverToggle();
            label8 = new Label();
            label5 = new Label();
            SearchKh = new ReaLTaiizor.Controls.SmallTextBox();
            lblNgTao = new Label();
            Tbnote = new ReaLTaiizor.Controls.SmallTextBox();
            label7 = new Label();
            label6 = new Label();
            label3 = new Label();
            lblSl = new Label();
            btnXuatHang = new ReaLTaiizor.Controls.HopeRoundButton();
            dtpNgayXuat = new ReaLTaiizor.Controls.PoisonDateTime();
            lblTongTien = new Label();
            label4 = new Label();
            button3 = new ReaLTaiizor.Controls.Button();
            button4 = new ReaLTaiizor.Controls.Button();
            ID = new ReaLTaiizor.Controls.SmallTextBox();
            button2 = new ReaLTaiizor.Controls.Button();
            button1 = new ReaLTaiizor.Controls.Button();
            panel2 = new Panel();
            btnCancelImport = new ReaLTaiizor.Controls.HopeRoundButton();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            dgvChiTiet = new ReaLTaiizor.Controls.PoisonDataGridView();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.Controls.Add(ketqua);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(dgvChiTiet);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1106, 561);
            panel1.TabIndex = 2;
            // 
            // ketqua
            // 
            ketqua.Location = new Point(192, 32);
            ketqua.Name = "ketqua";
            ketqua.Size = new Size(340, 100);
            ketqua.TabIndex = 31;
            ketqua.Visible = false;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(CbKhoNguon);
            panel4.Controls.Add(label9);
            panel4.Controls.Add(resultKh);
            panel4.Controls.Add(CbKhoDich);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(TgTrangthai);
            panel4.Controls.Add(label8);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(SearchKh);
            panel4.Controls.Add(lblNgTao);
            panel4.Controls.Add(Tbnote);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(lblSl);
            panel4.Controls.Add(btnXuatHang);
            panel4.Controls.Add(dtpNgayXuat);
            panel4.Controls.Add(lblTongTien);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(button4);
            panel4.Controls.Add(ID);
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(805, 33);
            panel4.Name = "panel4";
            panel4.Size = new Size(301, 528);
            panel4.TabIndex = 2;
            // 
            // CbKhoNguon
            // 
            CbKhoNguon.DrawMode = DrawMode.OwnerDrawFixed;
            CbKhoNguon.DropDownStyle = ComboBoxStyle.DropDownList;
            CbKhoNguon.EnabledCalc = true;
            CbKhoNguon.FormattingEnabled = true;
            CbKhoNguon.ItemHeight = 20;
            CbKhoNguon.Location = new Point(118, 100);
            CbKhoNguon.Name = "CbKhoNguon";
            CbKhoNguon.Size = new Size(166, 26);
            CbKhoNguon.TabIndex = 77;
            CbKhoNguon.SelectedIndexChanged += CbKhoNguon_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(19, 104);
            label9.Name = "label9";
            label9.Size = new Size(76, 17);
            label9.TabIndex = 76;
            label9.Text = "Kho nguồn";
            // 
            // resultKh
            // 
            resultKh.Location = new Point(118, 168);
            resultKh.Name = "resultKh";
            resultKh.Size = new Size(167, 24);
            resultKh.TabIndex = 74;
            resultKh.Visible = false;
            // 
            // CbKhoDich
            // 
            CbKhoDich.DrawMode = DrawMode.OwnerDrawFixed;
            CbKhoDich.DropDownStyle = ComboBoxStyle.DropDownList;
            CbKhoDich.EnabledCalc = true;
            CbKhoDich.FormattingEnabled = true;
            CbKhoDich.ItemHeight = 20;
            CbKhoDich.Location = new Point(117, 140);
            CbKhoDich.Name = "CbKhoDich";
            CbKhoDich.Size = new Size(172, 26);
            CbKhoDich.TabIndex = 73;
            CbKhoDich.SelectedIndexChanged += CbKhoDich_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(30, 141);
            label1.Name = "label1";
            label1.Size = new Size(61, 17);
            label1.TabIndex = 72;
            label1.Text = "Kho đích";
            // 
            // TgTrangthai
            // 
            TgTrangthai.BackColor = Color.Transparent;
            TgTrangthai.BaseColor = Color.FromArgb(35, 168, 109);
            TgTrangthai.BaseColorRed = Color.White;
            TgTrangthai.BGColor = Color.FromArgb(255, 255, 192);
            TgTrangthai.Checked = false;
            TgTrangthai.Font = new Font("Segoe UI", 10F);
            TgTrangthai.Location = new Point(114, 6);
            TgTrangthai.Name = "TgTrangthai";
            TgTrangthai.Options = ReaLTaiizor.Controls.ForeverToggle._Options.Style3;
            TgTrangthai.Size = new Size(76, 33);
            TgTrangthai.TabIndex = 71;
            TgTrangthai.Text = "foreverToggle1";
            TgTrangthai.TextColor = Color.Black;
            TgTrangthai.ToggleColor = Color.LimeGreen;
            TgTrangthai.CheckedChanged += TgTrangthai_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(209, 16);
            label8.Name = "label8";
            label8.Size = new Size(71, 17);
            label8.TabIndex = 42;
            label8.Text = "Xuất hàng";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(32, 16);
            label5.Name = "label5";
            label5.Size = new Size(63, 17);
            label5.TabIndex = 41;
            label5.Text = "Xuất kho";
            // 
            // SearchKh
            // 
            SearchKh.BackColor = Color.Transparent;
            SearchKh.BorderColor = Color.FromArgb(180, 180, 180);
            SearchKh.CustomBGColor = Color.White;
            SearchKh.Font = new Font("Tahoma", 11F);
            SearchKh.ForeColor = Color.DimGray;
            SearchKh.Location = new Point(119, 139);
            SearchKh.MaxLength = 32767;
            SearchKh.Multiline = false;
            SearchKh.Name = "SearchKh";
            SearchKh.ReadOnly = false;
            SearchKh.Size = new Size(166, 28);
            SearchKh.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            SearchKh.TabIndex = 38;
            SearchKh.TextAlignment = HorizontalAlignment.Left;
            SearchKh.UseSystemPasswordChar = false;
            SearchKh.TextChanged += SearchKh_TextChanged;
            // 
            // lblNgTao
            // 
            lblNgTao.AutoSize = true;
            lblNgTao.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNgTao.Location = new Point(17, 408);
            lblNgTao.Name = "lblNgTao";
            lblNgTao.Size = new Size(71, 17);
            lblNgTao.TabIndex = 37;
            lblNgTao.Text = "Người Tạo";
            // 
            // Tbnote
            // 
            Tbnote.BackColor = Color.Transparent;
            Tbnote.BorderColor = Color.FromArgb(180, 180, 180);
            Tbnote.CustomBGColor = Color.White;
            Tbnote.Font = new Font("Tahoma", 11F);
            Tbnote.ForeColor = Color.DimGray;
            Tbnote.Location = new Point(17, 320);
            Tbnote.MaxLength = 32767;
            Tbnote.Multiline = true;
            Tbnote.Name = "Tbnote";
            Tbnote.ReadOnly = false;
            Tbnote.Size = new Size(268, 75);
            Tbnote.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Tbnote.TabIndex = 32;
            Tbnote.Text = "Ghi chú";
            Tbnote.TextAlignment = HorizontalAlignment.Left;
            Tbnote.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(30, 61);
            label7.Name = "label7";
            label7.Size = new Size(65, 17);
            label7.TabIndex = 27;
            label7.Text = "Mã phiếu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(22, 264);
            label6.Name = "label6";
            label6.Size = new Size(71, 17);
            label6.TabIndex = 26;
            label6.Text = "Ngày xuất";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(9, 141);
            label3.Name = "label3";
            label3.Size = new Size(80, 17);
            label3.TabIndex = 21;
            label3.Text = "Khách hàng";
            // 
            // lblSl
            // 
            lblSl.AutoSize = true;
            lblSl.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSl.Location = new Point(129, 439);
            lblSl.Name = "lblSl";
            lblSl.Size = new Size(15, 17);
            lblSl.TabIndex = 19;
            lblSl.Text = "0";
            // 
            // btnXuatHang
            // 
            btnXuatHang.BorderColor = Color.FromArgb(220, 223, 230);
            btnXuatHang.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            btnXuatHang.DangerColor = Color.FromArgb(245, 108, 108);
            btnXuatHang.DefaultColor = Color.FromArgb(255, 255, 255);
            btnXuatHang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuatHang.HoverTextColor = Color.FromArgb(48, 49, 51);
            btnXuatHang.InfoColor = Color.FromArgb(144, 147, 153);
            btnXuatHang.Location = new Point(59, 476);
            btnXuatHang.Name = "btnXuatHang";
            btnXuatHang.PrimaryColor = Color.LimeGreen;
            btnXuatHang.Size = new Size(190, 40);
            btnXuatHang.SuccessColor = Color.FromArgb(103, 194, 58);
            btnXuatHang.TabIndex = 18;
            btnXuatHang.Text = "Tạo phiếu";
            btnXuatHang.TextColor = Color.White;
            btnXuatHang.WarningColor = Color.FromArgb(230, 162, 60);
            btnXuatHang.Click += btnTaoPhieuXuat_Click;
            // 
            // dtpNgayXuat
            // 
            dtpNgayXuat.CalendarForeColor = Color.FromArgb(30, 58, 138);
            dtpNgayXuat.CalendarTitleBackColor = Color.FromArgb(30, 58, 138);
            dtpNgayXuat.CalendarTitleForeColor = Color.White;
            dtpNgayXuat.CustomFormat = "dd/MM/yyyy";
            dtpNgayXuat.Format = DateTimePickerFormat.Custom;
            dtpNgayXuat.Location = new Point(119, 264);
            dtpNgayXuat.MinimumSize = new Size(0, 29);
            dtpNgayXuat.Name = "dtpNgayXuat";
            dtpNgayXuat.Size = new Size(166, 29);
            dtpNgayXuat.Style = ReaLTaiizor.Enum.Poison.ColorStyle.White;
            dtpNgayXuat.TabIndex = 15;
            dtpNgayXuat.Value = new DateTime(2025, 4, 7, 8, 15, 57, 0);
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongTien.ForeColor = Color.LimeGreen;
            lblTongTien.Location = new Point(197, 439);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(15, 17);
            lblTongTien.TabIndex = 13;
            lblTongTien.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 439);
            label4.Name = "label4";
            label4.Size = new Size(101, 17);
            label4.TabIndex = 11;
            label4.Text = "Tổng tiền hàng";
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.BackColor = Color.Transparent;
            button3.BorderColor = Color.FromArgb(52, 73, 94);
            button3.Cursor = Cursors.Hand;
            button3.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button3.EnteredColor = Color.FromArgb(32, 34, 37);
            button3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Image = null;
            button3.ImageAlign = ContentAlignment.MiddleRight;
            button3.InactiveColor = Color.FromArgb(44, 62, 80);
            button3.Location = new Point(1029, 31);
            button3.Margin = new Padding(3, 3, 160, 3);
            button3.Name = "button3";
            button3.Padding = new Padding(0, 0, 100, 0);
            button3.PressedBorderColor = Color.Green;
            button3.PressedColor = Color.Green;
            button3.RightToLeft = RightToLeft.Yes;
            button3.Size = new Size(129, 28);
            button3.TabIndex = 7;
            button3.Text = "Thêm";
            button3.TextAlignment = StringAlignment.Center;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button4.BackColor = Color.Transparent;
            button4.BorderColor = Color.FromArgb(32, 34, 37);
            button4.Cursor = Cursors.Hand;
            button4.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button4.EnteredColor = Color.FromArgb(32, 34, 37);
            button4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Image = null;
            button4.ImageAlign = ContentAlignment.MiddleRight;
            button4.InactiveColor = Color.CornflowerBlue;
            button4.Location = new Point(1930, 31);
            button4.Margin = new Padding(3, 3, 160, 3);
            button4.Name = "button4";
            button4.Padding = new Padding(0, 0, 100, 0);
            button4.PressedBorderColor = Color.Green;
            button4.PressedColor = Color.Green;
            button4.RightToLeft = RightToLeft.Yes;
            button4.Size = new Size(129, 28);
            button4.TabIndex = 5;
            button4.Text = "Thêm";
            button4.TextAlignment = StringAlignment.Center;
            // 
            // ID
            // 
            ID.BackColor = Color.Transparent;
            ID.BorderColor = Color.FromArgb(180, 180, 180);
            ID.CustomBGColor = Color.White;
            ID.Font = new Font("Tahoma", 11F);
            ID.ForeColor = Color.DimGray;
            ID.Location = new Point(119, 61);
            ID.MaxLength = 32767;
            ID.Multiline = false;
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Size = new Size(166, 28);
            ID.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            ID.TabIndex = 33;
            ID.Text = "Tự động";
            ID.TextAlignment = HorizontalAlignment.Left;
            ID.UseSystemPasswordChar = false;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.BackColor = Color.Transparent;
            button2.BorderColor = Color.FromArgb(52, 73, 94);
            button2.Cursor = Cursors.Hand;
            button2.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button2.EnteredColor = Color.FromArgb(32, 34, 37);
            button2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Image = null;
            button2.ImageAlign = ContentAlignment.MiddleRight;
            button2.InactiveColor = Color.FromArgb(44, 62, 80);
            button2.Location = new Point(1834, 31);
            button2.Margin = new Padding(3, 3, 160, 3);
            button2.Name = "button2";
            button2.Padding = new Padding(0, 0, 100, 0);
            button2.PressedBorderColor = Color.Green;
            button2.PressedColor = Color.Green;
            button2.RightToLeft = RightToLeft.Yes;
            button2.Size = new Size(129, 28);
            button2.TabIndex = 7;
            button2.Text = "Thêm";
            button2.TextAlignment = StringAlignment.Center;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.Transparent;
            button1.BorderColor = Color.FromArgb(32, 34, 37);
            button1.Cursor = Cursors.Hand;
            button1.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button1.EnteredColor = Color.FromArgb(32, 34, 37);
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Image = null;
            button1.ImageAlign = ContentAlignment.MiddleRight;
            button1.InactiveColor = Color.CornflowerBlue;
            button1.Location = new Point(2735, 31);
            button1.Margin = new Padding(3, 3, 160, 3);
            button1.Name = "button1";
            button1.Padding = new Padding(0, 0, 100, 0);
            button1.PressedBorderColor = Color.Green;
            button1.PressedColor = Color.Green;
            button1.RightToLeft = RightToLeft.Yes;
            button1.Size = new Size(129, 28);
            button1.TabIndex = 5;
            button1.Text = "Thêm";
            button1.TextAlignment = StringAlignment.Center;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnCancelImport);
            panel2.Controls.Add(bigLabel1);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(pictureBox3);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1106, 33);
            panel2.TabIndex = 13;
            // 
            // btnCancelImport
            // 
            btnCancelImport.BorderColor = Color.FromArgb(220, 223, 230);
            btnCancelImport.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            btnCancelImport.DangerColor = Color.FromArgb(245, 108, 108);
            btnCancelImport.DefaultColor = Color.FromArgb(255, 255, 255);
            btnCancelImport.Enabled = false;
            btnCancelImport.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelImport.HoverTextColor = Color.FromArgb(48, 49, 51);
            btnCancelImport.InfoColor = Color.FromArgb(144, 147, 153);
            btnCancelImport.Location = new Point(934, 4);
            btnCancelImport.Name = "btnCancelImport";
            btnCancelImport.PrimaryColor = Color.Crimson;
            btnCancelImport.Size = new Size(120, 24);
            btnCancelImport.SuccessColor = Color.FromArgb(103, 194, 58);
            btnCancelImport.TabIndex = 38;
            btnCancelImport.Text = "Xoá import";
            btnCancelImport.TextColor = Color.White;
            btnCancelImport.Visible = false;
            btnCancelImport.WarningColor = Color.FromArgb(230, 162, 60);
            btnCancelImport.Click += btnCancelImport_Click;
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(58, 1);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(119, 30);
            bigLabel1.TabIndex = 11;
            bigLabel1.Text = "Phiếu xuất";
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.Transparent;
            txtSearch.BorderColor = Color.FromArgb(180, 180, 180);
            txtSearch.EdgeColor = SystemColors.ActiveBorder;
            txtSearch.Font = new Font("Tahoma", 11F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(192, 1);
            txtSearch.MaxLength = 32767;
            txtSearch.Multiline = false;
            txtSearch.Name = "txtSearch";
            txtSearch.ReadOnly = false;
            txtSearch.Size = new Size(339, 28);
            txtSearch.TabIndex = 2;
            txtSearch.TextAlignment = HorizontalAlignment.Center;
            txtSearch.UseSystemPasswordChar = false;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = GUI.Properties.Resources.icons8_search_24;
            pictureBox1.Location = new Point(201, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = GUI.Properties.Resources.icons8_back_501;
            pictureBox3.Location = new Point(2, -3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(49, 40);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 10;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AllowUserToResizeRows = false;
            dgvChiTiet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChiTiet.BackgroundColor = Color.FromArgb(236, 240, 241);
            dgvChiTiet.BorderStyle = BorderStyle.None;
            dgvChiTiet.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvChiTiet.DefaultCellStyle = dataGridViewCellStyle2;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dgvChiTiet.GridColor = Color.FromArgb(255, 255, 255);
            dgvChiTiet.Location = new Point(3, 31);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvChiTiet.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvChiTiet.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(798, 530);
            dgvChiTiet.TabIndex = 8;
            dgvChiTiet.CellContentClick += dgvChiTiet_CellContentClick;
            dgvChiTiet.RowsAdded += dgvChiTiet_RowsAdded;
            dgvChiTiet.RowsRemoved += dgvChiTiet_RowsRemoved;
            // 
            // AddXuat
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 561);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddXuat";
            Text = "NhapHang";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ReaLTaiizor.Controls.Button button2;
        private ReaLTaiizor.Controls.Button button1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.DungeonTextBox txtSearch;
        private ReaLTaiizor.Controls.PoisonDataGridView dgvChiTiet;
        private Label label1;
        private Panel panel4;
        private ReaLTaiizor.Controls.Button button3;
        private ReaLTaiizor.Controls.Button button4;
        private PictureBox pictureBox2;
        private Label lblTongTien;
        private Label label4;
        private ReaLTaiizor.Controls.SmallTextBox TbId;
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgayXuat;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private PictureBox pictureBox3;
        private ReaLTaiizor.Controls.HopeRoundButton btnXuatHang;
        private Label lblSl;
        private ReaLTaiizor.Controls.AloneComboBox CbKhach;
        private ReaLTaiizor.Controls.SmallTextBox SearchKh;
        private Label label6;
        private Panel panel2;
        private Label label7;
        private ReaLTaiizor.Controls.SmallTextBox Tbnote;
        private ReaLTaiizor.Controls.SmallTextBox ID;
        private FlowLayoutPanel ketqua;
        private ReaLTaiizor.Controls.AloneComboBox CbNgNhap;
        private Label lblNgTao;
        private ReaLTaiizor.Controls.HopeRoundButton btnCancelImport;
        private Label label3;
        private Label label8;
        private Label label5;
        private ReaLTaiizor.Controls.ForeverToggle TgTrangthai;
        private Panel resultKh;
        private System.Windows.Forms.Timer timer1;
        private ReaLTaiizor.Controls.AloneComboBox CbKhoNguon;
        private Label label9;
        private ReaLTaiizor.Controls.AloneComboBox CbKhoDich;
    }
}