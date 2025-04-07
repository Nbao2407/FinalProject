
namespace QLVT
{
    partial class AddHoaDon
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddHoaDon));
            panel1 = new Panel();
            Tong = new Label();
            button1 = new ReaLTaiizor.Controls.Button();
            pictureBox1 = new PictureBox();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            splitContainer1 = new BufferedSplitContainer();
            panel2 = new Panel();
            BtnThanhToan = new ReaLTaiizor.Controls.Button();
            flowLayoutPanel2 = new BufferedFlowLayoutPanel();
            ketqua = new Panel();
            SeacrhKh = new ReaLTaiizor.Controls.DungeonTextBox();
            flowLayoutPanel1 = new BufferedFlowLayoutPanel();
            roundedPanel6 = new QLVT.Helper.RoundedPanel();
            lblTongSoMatHang = new Label();
            lblTong = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            searchTimer = new System.Windows.Forms.Timer(components);
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            roundedPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.Controls.Add(Tong);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(splitContainer1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1106, 561);
            panel1.TabIndex = 2;
            // 
            // Tong
            // 
            Tong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Tong.AutoSize = true;
            Tong.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Tong.ForeColor = Color.DarkGray;
            Tong.Location = new Point(12, 1545);
            Tong.Name = "Tong";
            Tong.Size = new Size(33, 13);
            Tong.TabIndex = 8;
            Tong.Text = "Tổng";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.BackColor = Color.Transparent;
            button1.BorderColor = Color.FromArgb(52, 73, 94);
            button1.Cursor = Cursors.Hand;
            button1.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button1.EnteredColor = Color.FromArgb(32, 34, 37);
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Image = null;
            button1.ImageAlign = ContentAlignment.MiddleRight;
            button1.InactiveColor = Color.FromArgb(44, 62, 80);
            button1.Location = new Point(2740, 31);
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
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(376, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(30, 28);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(236, 240, 241);
            txtSearch.BorderColor = Color.FromArgb(180, 180, 180);
            txtSearch.EdgeColor = SystemColors.ActiveBorder;
            txtSearch.Font = new Font("Tahoma", 11F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(64, 3);
            txtSearch.MaxLength = 32767;
            txtSearch.Multiline = false;
            txtSearch.Name = "txtSearch";
            txtSearch.ReadOnly = false;
            txtSearch.Size = new Size(312, 28);
            txtSearch.TabIndex = 2;
            txtSearch.Text = "Tìm hàng hóa";
            txtSearch.TextAlignment = HorizontalAlignment.Left;
            txtSearch.UseSystemPasswordChar = false;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(3, 17);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox2);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(BtnThanhToan);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Controls.Add(flowLayoutPanel2);
            splitContainer1.Panel1.Controls.Add(txtSearch);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(ketqua);
            splitContainer1.Panel2.Controls.Add(SeacrhKh);
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2.Controls.Add(roundedPanel6);
            splitContainer1.Size = new Size(1104, 541);
            splitContainer1.SplitterDistance = 569;
            splitContainer1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.Location = new Point(64, 34);
            panel2.Name = "panel2";
            panel2.Size = new Size(312, 82);
            panel2.TabIndex = 14;
            panel2.Visible = false;
            // 
            // BtnThanhToan
            // 
            BtnThanhToan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnThanhToan.BackColor = Color.Transparent;
            BtnThanhToan.BorderColor = Color.Transparent;
            BtnThanhToan.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            BtnThanhToan.EnteredColor = Color.FromArgb(32, 34, 37);
            BtnThanhToan.Font = new Font("Microsoft Sans Serif", 12F);
            BtnThanhToan.Image = null;
            BtnThanhToan.ImageAlign = ContentAlignment.MiddleLeft;
            BtnThanhToan.InactiveColor = Color.FromArgb(39, 174, 97);
            BtnThanhToan.Location = new Point(9, 498);
            BtnThanhToan.Name = "BtnThanhToan";
            BtnThanhToan.PressedBorderColor = Color.FromArgb(165, 37, 37);
            BtnThanhToan.PressedColor = Color.FromArgb(165, 37, 37);
            BtnThanhToan.Size = new Size(549, 40);
            BtnThanhToan.TabIndex = 12;
            BtnThanhToan.Text = "Thanh Toán";
            BtnThanhToan.TextAlignment = StringAlignment.Center;
            BtnThanhToan.Click += BtnThanhToan_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel2.BackColor = Color.White;
            flowLayoutPanel2.Location = new Point(0, 37);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(561, 456);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // ketqua
            // 
            ketqua.Location = new Point(6, 34);
            ketqua.Name = "ketqua";
            ketqua.Size = new Size(312, 82);
            ketqua.TabIndex = 15;
            ketqua.Visible = false;
            // 
            // SeacrhKh
            // 
            SeacrhKh.BackColor = Color.FromArgb(236, 240, 241);
            SeacrhKh.BorderColor = Color.FromArgb(180, 180, 180);
            SeacrhKh.EdgeColor = SystemColors.ActiveBorder;
            SeacrhKh.Font = new Font("Tahoma", 11F);
            SeacrhKh.ForeColor = Color.DimGray;
            SeacrhKh.Location = new Point(3, 3);
            SeacrhKh.MaxLength = 32767;
            SeacrhKh.Multiline = false;
            SeacrhKh.Name = "SeacrhKh";
            SeacrhKh.ReadOnly = false;
            SeacrhKh.Size = new Size(312, 28);
            SeacrhKh.TabIndex = 10;
            SeacrhKh.TextAlignment = HorizontalAlignment.Left;
            SeacrhKh.UseSystemPasswordChar = false;
            SeacrhKh.TextChanged += SeacrhKh_TextChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.BackColor = Color.FromArgb(236, 240, 241);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 37);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(531, 459);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // roundedPanel6
            // 
            roundedPanel6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel6.BackColor = Color.White;
            roundedPanel6.BorderRadius = 20;
            roundedPanel6.Controls.Add(lblTongSoMatHang);
            roundedPanel6.Controls.Add(lblTong);
            roundedPanel6.Location = new Point(3, 497);
            roundedPanel6.Name = "roundedPanel6";
            roundedPanel6.Size = new Size(524, 41);
            roundedPanel6.TabIndex = 16;
            // 
            // lblTongSoMatHang
            // 
            lblTongSoMatHang.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTongSoMatHang.AutoSize = true;
            lblTongSoMatHang.BackColor = Color.White;
            lblTongSoMatHang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongSoMatHang.Location = new Point(22, 10);
            lblTongSoMatHang.Name = "lblTongSoMatHang";
            lblTongSoMatHang.Size = new Size(120, 21);
            lblTongSoMatHang.TabIndex = 13;
            lblTongSoMatHang.Text = "Tổng tiền hàng";
            lblTongSoMatHang.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTong
            // 
            lblTong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTong.AutoSize = true;
            lblTong.BackColor = Color.White;
            lblTong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTong.ForeColor = Color.LimeGreen;
            lblTong.Location = new Point(399, 10);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(68, 21);
            lblTong.TabIndex = 15;
            lblTong.Text = "500,000";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = GUI.Properties.Resources.icons8_back_50;
            pictureBox2.Location = new Point(8, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(50, 36);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // AddHoaDon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 561);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddHoaDon";
            Text = "AddHoaDon";
            Load += AddHoaDon_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            roundedPanel6.ResumeLayout(false);
            roundedPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        private Panel panel1;
        private Label Tong;
        private ReaLTaiizor.Controls.Button button1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.DungeonTextBox txtSearch;
        private BufferedSplitContainer splitContainer1;
        private ReaLTaiizor.Controls.Button BtnThanhToan;
        private Label lblTong;
        private Label lblTongSoMatHang;
        private Helper.RoundedPanel roundedPanel6;
        private BufferedFlowLayoutPanel flowLayoutPanel2;
        private BufferedFlowLayoutPanel flowLayoutPanel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panel2;
        private System.Windows.Forms.Timer searchTimer;
        private ReaLTaiizor.Controls.DungeonTextBox SeacrhKh;
        private Panel ketqua;
        private PictureBox pictureBox2;
    }
}