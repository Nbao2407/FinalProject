namespace QLVT
{
    partial class NhapHang
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
            panel1 = new Panel();
            ketqua = new FlowLayoutPanel();
            panel4 = new Panel();
            lblNgTao = new Label();
            CbNgNhap = new ReaLTaiizor.Controls.AloneComboBox();
            label2 = new Label();
            Tbnote = new ReaLTaiizor.Controls.SmallTextBox();
            label7 = new Label();
            label6 = new Label();
            CbNcc = new ReaLTaiizor.Controls.AloneComboBox();
            label3 = new Label();
            lblSl = new Label();
            btnNhapHang = new ReaLTaiizor.Controls.HopeRoundButton();
            dtpNgayNhap = new ReaLTaiizor.Controls.PoisonDateTime();
            lblTongTien = new Label();
            label4 = new Label();
            button3 = new ReaLTaiizor.Controls.Button();
            button4 = new ReaLTaiizor.Controls.Button();
            ID = new ReaLTaiizor.Controls.SmallTextBox();
            panelImportExcel = new Panel();
            hopeRoundButton2 = new ReaLTaiizor.Controls.HopeRoundButton();
            smallLabel1 = new ReaLTaiizor.Controls.SmallLabel();
            button2 = new ReaLTaiizor.Controls.Button();
            button1 = new ReaLTaiizor.Controls.Button();
            panel2 = new Panel();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            pictureBox1 = new PictureBox();
            pictureBox3 = new PictureBox();
            dgvChiTiet = new ReaLTaiizor.Controls.PoisonDataGridView();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panelImportExcel.SuspendLayout();
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
            panel1.Controls.Add(panelImportExcel);
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
            panel4.Controls.Add(lblNgTao);
            panel4.Controls.Add(CbNgNhap);
            panel4.Controls.Add(label2);
            panel4.Controls.Add(Tbnote);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(CbNcc);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(lblSl);
            panel4.Controls.Add(btnNhapHang);
            panel4.Controls.Add(dtpNgayNhap);
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
            // lblNgTao
            // 
            lblNgTao.AutoSize = true;
            lblNgTao.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNgTao.Location = new Point(17, 384);
            lblNgTao.Name = "lblNgTao";
            lblNgTao.Size = new Size(71, 17);
            lblNgTao.TabIndex = 37;
            lblNgTao.Text = "Người Tạo";
            // 
            // CbNgNhap
            // 
            CbNgNhap.DrawMode = DrawMode.OwnerDrawFixed;
            CbNgNhap.DropDownStyle = ComboBoxStyle.DropDownList;
            CbNgNhap.EnabledCalc = true;
            CbNgNhap.FormattingEnabled = true;
            CbNgNhap.ItemHeight = 20;
            CbNgNhap.Location = new Point(122, 144);
            CbNgNhap.Name = "CbNgNhap";
            CbNgNhap.Size = new Size(166, 26);
            CbNgNhap.TabIndex = 36;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(28, 144);
            label2.Name = "label2";
            label2.Size = new Size(80, 17);
            label2.TabIndex = 35;
            label2.Text = "Người nhập";
            // 
            // Tbnote
            // 
            Tbnote.BackColor = Color.Transparent;
            Tbnote.BorderColor = Color.FromArgb(180, 180, 180);
            Tbnote.CustomBGColor = Color.White;
            Tbnote.Font = new Font("Tahoma", 11F);
            Tbnote.ForeColor = Color.DimGray;
            Tbnote.Location = new Point(23, 277);
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
            label7.Location = new Point(23, 26);
            label7.Name = "label7";
            label7.Size = new Size(65, 17);
            label7.TabIndex = 27;
            label7.Text = "Mã phiếu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(33, 195);
            label6.Name = "label6";
            label6.Size = new Size(75, 17);
            label6.TabIndex = 26;
            label6.Text = "Ngày nhập";
            // 
            // CbNcc
            // 
            CbNcc.DrawMode = DrawMode.OwnerDrawFixed;
            CbNcc.DropDownStyle = ComboBoxStyle.DropDownList;
            CbNcc.EnabledCalc = true;
            CbNcc.FormattingEnabled = true;
            CbNcc.ItemHeight = 20;
            CbNcc.Location = new Point(122, 93);
            CbNcc.Name = "CbNcc";
            CbNcc.Size = new Size(166, 26);
            CbNcc.TabIndex = 22;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(20, 93);
            label3.Name = "label3";
            label3.Size = new Size(92, 17);
            label3.TabIndex = 21;
            label3.Text = "Nhà cung cấp";
            // 
            // lblSl
            // 
            lblSl.AutoSize = true;
            lblSl.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSl.Location = new Point(129, 415);
            lblSl.Name = "lblSl";
            lblSl.Size = new Size(15, 17);
            lblSl.TabIndex = 19;
            lblSl.Text = "0";
            // 
            // btnNhapHang
            // 
            btnNhapHang.BorderColor = Color.FromArgb(220, 223, 230);
            btnNhapHang.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            btnNhapHang.DangerColor = Color.FromArgb(245, 108, 108);
            btnNhapHang.DefaultColor = Color.FromArgb(255, 255, 255);
            btnNhapHang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNhapHang.HoverTextColor = Color.FromArgb(48, 49, 51);
            btnNhapHang.InfoColor = Color.FromArgb(144, 147, 153);
            btnNhapHang.Location = new Point(59, 476);
            btnNhapHang.Name = "btnNhapHang";
            btnNhapHang.PrimaryColor = Color.LimeGreen;
            btnNhapHang.Size = new Size(190, 40);
            btnNhapHang.SuccessColor = Color.FromArgb(103, 194, 58);
            btnNhapHang.TabIndex = 18;
            btnNhapHang.Text = "Tạo phiếu";
            btnNhapHang.TextColor = Color.White;
            btnNhapHang.WarningColor = Color.FromArgb(230, 162, 60);
            btnNhapHang.Click += btnNhapHang_Click;
            // 
            // dtpNgayNhap
            // 
            dtpNgayNhap.CalendarForeColor = Color.FromArgb(30, 58, 138);
            dtpNgayNhap.CalendarTitleBackColor = Color.FromArgb(30, 58, 138);
            dtpNgayNhap.CalendarTitleForeColor = Color.White;
            dtpNgayNhap.CustomFormat = "dd/MM/yyyy";
            dtpNgayNhap.Format = DateTimePickerFormat.Custom;
            dtpNgayNhap.Location = new Point(122, 195);
            dtpNgayNhap.MinimumSize = new Size(0, 29);
            dtpNgayNhap.Name = "dtpNgayNhap";
            dtpNgayNhap.Size = new Size(166, 29);
            dtpNgayNhap.Style = ReaLTaiizor.Enum.Poison.ColorStyle.White;
            dtpNgayNhap.TabIndex = 15;
            dtpNgayNhap.Value = new DateTime(2025, 4, 7, 8, 15, 57, 0);
            // 
            // lblTongTien
            // 
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongTien.ForeColor = Color.LimeGreen;
            lblTongTien.Location = new Point(197, 415);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Size = new Size(15, 17);
            lblTongTien.TabIndex = 13;
            lblTongTien.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 415);
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
            ID.Location = new Point(123, 20);
            ID.MaxLength = 32767;
            ID.Multiline = false;
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Size = new Size(157, 28);
            ID.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            ID.TabIndex = 33;
            ID.Text = "Tự động";
            ID.TextAlignment = HorizontalAlignment.Left;
            ID.UseSystemPasswordChar = false;
            // 
            // panelImportExcel
            // 
            panelImportExcel.Anchor = AnchorStyles.None;
            panelImportExcel.BackColor = Color.FromArgb(236, 240, 241);
            panelImportExcel.Controls.Add(hopeRoundButton2);
            panelImportExcel.Controls.Add(smallLabel1);
            panelImportExcel.Location = new Point(285, 228);
            panelImportExcel.Name = "panelImportExcel";
            panelImportExcel.Size = new Size(247, 117);
            panelImportExcel.TabIndex = 12;
            // 
            // hopeRoundButton2
            // 
            hopeRoundButton2.BorderColor = Color.FromArgb(220, 223, 230);
            hopeRoundButton2.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            hopeRoundButton2.DangerColor = Color.FromArgb(245, 108, 108);
            hopeRoundButton2.DefaultColor = Color.FromArgb(255, 255, 255);
            hopeRoundButton2.Font = new Font("Segoe UI", 12F);
            hopeRoundButton2.HoverTextColor = Color.FromArgb(48, 49, 51);
            hopeRoundButton2.InfoColor = Color.FromArgb(144, 147, 153);
            hopeRoundButton2.Location = new Point(25, 64);
            hopeRoundButton2.Name = "hopeRoundButton2";
            hopeRoundButton2.PrimaryColor = Color.FromArgb(64, 158, 255);
            hopeRoundButton2.Size = new Size(190, 40);
            hopeRoundButton2.SuccessColor = Color.FromArgb(103, 194, 58);
            hopeRoundButton2.TabIndex = 1;
            hopeRoundButton2.Text = "Chọn file";
            hopeRoundButton2.TextColor = Color.White;
            hopeRoundButton2.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton2.Click += hopeRoundButton2_Click;
            // 
            // smallLabel1
            // 
            smallLabel1.AutoSize = true;
            smallLabel1.BackColor = Color.Transparent;
            smallLabel1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            smallLabel1.ForeColor = Color.Black;
            smallLabel1.Location = new Point(8, 28);
            smallLabel1.Name = "smallLabel1";
            smallLabel1.Size = new Size(225, 21);
            smallLabel1.TabIndex = 0;
            smallLabel1.Text = "Thêm sản phẩm từ file excel";
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
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(58, 1);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(123, 30);
            bigLabel1.TabIndex = 11;
            bigLabel1.Text = "Nhập hàng";
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
            dgvChiTiet.ReadOnly = true;
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
            // 
            // NhapHang
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 561);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "NhapHang";
            Text = "NhapHang";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panelImportExcel.ResumeLayout(false);
            panelImportExcel.PerformLayout();
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
        private ReaLTaiizor.Controls.PoisonDateTime dtpNgayNhap;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private PictureBox pictureBox3;
        private ReaLTaiizor.Controls.HopeRoundButton btnNhapHang;
        private Panel panelImportExcel;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton2;
        private ReaLTaiizor.Controls.SmallLabel smallLabel1;
        private Label lblSl;
        private ReaLTaiizor.Controls.AloneComboBox CbNcc;
        private Label label3;
        private ReaLTaiizor.Controls.SmallTextBox smallTextBox1;
        private Label label6;
        private Panel panel2;
        private Label label7;
        private ReaLTaiizor.Controls.SmallTextBox Tbnote;
        private ReaLTaiizor.Controls.SmallTextBox ID;
        private FlowLayoutPanel ketqua;
        private ReaLTaiizor.Controls.AloneComboBox CbNgNhap;
        private Label label2;
        private Label lblNgTao;
    }
}