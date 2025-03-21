namespace GUI
{
    partial class AddHoaDon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddHoaDon));
            panel1 = new Panel();
            Tong = new Label();
            button1 = new ReaLTaiizor.Controls.Button();
            pictureBox1 = new PictureBox();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            splitContainer1 = new SplitContainer();
            button2 = new ReaLTaiizor.Controls.Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            dungeonTextBox1 = new ReaLTaiizor.Controls.DungeonTextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            wiget22 = new Wiget2();
            wiget21 = new Wiget2();
            wiget23 = new Wiget2();
            roundedPanel6 = new GUI.Helpler.RoundedPanel();
            label10 = new Label();
            lblTong = new Label();
            lblSL = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            roundedPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.Controls.Add(Tong);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(splitContainer1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1090, 561);
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
            button1.Location = new Point(2724, 31);
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
            pictureBox1.Location = new Point(319, 17);
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
            txtSearch.Location = new Point(7, 17);
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
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(flowLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dungeonTextBox1);
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel2.Controls.Add(roundedPanel6);
            splitContainer1.Size = new Size(1088, 541);
            splitContainer1.SplitterDistance = 561;
            splitContainer1.TabIndex = 9;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.BackColor = Color.Transparent;
            button2.BorderColor = Color.Transparent;
            button2.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button2.EnteredColor = Color.FromArgb(32, 34, 37);
            button2.Font = new Font("Microsoft Sans Serif", 12F);
            button2.Image = null;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.InactiveColor = Color.FromArgb(39, 174, 97);
            button2.Location = new Point(9, 498);
            button2.Name = "button2";
            button2.PressedBorderColor = Color.FromArgb(165, 37, 37);
            button2.PressedColor = Color.FromArgb(165, 37, 37);
            button2.Size = new Size(549, 40);
            button2.TabIndex = 12;
            button2.Text = "Thanh Toán";
            button2.TextAlignment = StringAlignment.Center;
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
            // dungeonTextBox1
            // 
            dungeonTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dungeonTextBox1.BackColor = Color.FromArgb(236, 240, 241);
            dungeonTextBox1.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonTextBox1.EdgeColor = SystemColors.ActiveBorder;
            dungeonTextBox1.Font = new Font("Tahoma", 11F);
            dungeonTextBox1.ForeColor = Color.DimGray;
            dungeonTextBox1.Location = new Point(0, 3);
            dungeonTextBox1.MaxLength = 32767;
            dungeonTextBox1.Multiline = false;
            dungeonTextBox1.Name = "dungeonTextBox1";
            dungeonTextBox1.ReadOnly = false;
            dungeonTextBox1.Size = new Size(290, 28);
            dungeonTextBox1.TabIndex = 10;
            dungeonTextBox1.Text = "Tìm Khách Hàng";
            dungeonTextBox1.TextAlignment = HorizontalAlignment.Left;
            dungeonTextBox1.UseSystemPasswordChar = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.BackColor = Color.FromArgb(236, 240, 241);
            flowLayoutPanel1.Controls.Add(wiget22);
            flowLayoutPanel1.Controls.Add(wiget21);
            flowLayoutPanel1.Controls.Add(wiget23);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 34);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(523, 459);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // wiget22
            // 
            wiget22.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            wiget22.Location = new Point(3, 3);
            wiget22.Name = "wiget22";
            wiget22.Size = new Size(516, 94);
            wiget22.TabIndex = 2;
            // 
            // wiget21
            // 
            wiget21.Location = new Point(3, 103);
            wiget21.Name = "wiget21";
            wiget21.Size = new Size(516, 94);
            wiget21.TabIndex = 3;
            // 
            // wiget23
            // 
            wiget23.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            wiget23.Location = new Point(3, 203);
            wiget23.Name = "wiget23";
            wiget23.Size = new Size(516, 94);
            wiget23.TabIndex = 4;
            // 
            // roundedPanel6
            // 
            roundedPanel6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            roundedPanel6.BackColor = Color.White;
            roundedPanel6.BorderRadius = 20;
            roundedPanel6.Controls.Add(label10);
            roundedPanel6.Controls.Add(lblTong);
            roundedPanel6.Controls.Add(lblSL);
            roundedPanel6.Location = new Point(3, 497);
            roundedPanel6.Name = "roundedPanel6";
            roundedPanel6.Size = new Size(516, 41);
            roundedPanel6.TabIndex = 16;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label10.AutoSize = true;
            label10.BackColor = Color.White;
            label10.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(22, 10);
            label10.Name = "label10";
            label10.Size = new Size(120, 21);
            label10.TabIndex = 13;
            label10.Text = "Tổng tiền hàng";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTong
            // 
            lblTong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTong.AutoSize = true;
            lblTong.BackColor = Color.White;
            lblTong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTong.ForeColor = Color.LimeGreen;
            lblTong.Location = new Point(391, 10);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(68, 21);
            lblTong.TabIndex = 15;
            lblTong.Text = "500,000";
            // 
            // lblSL
            // 
            lblSL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSL.AutoSize = true;
            lblSL.BackColor = Color.White;
            lblSL.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSL.Location = new Point(148, 10);
            lblSL.Name = "lblSL";
            lblSL.Size = new Size(19, 21);
            lblSL.TabIndex = 14;
            lblSL.Text = "3";
            lblSL.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AddHoaDon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1090, 561);
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
            flowLayoutPanel1.ResumeLayout(false);
            roundedPanel6.ResumeLayout(false);
            roundedPanel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label Tong;
        private ReaLTaiizor.Controls.Button button1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.DungeonTextBox txtSearch;
        private SplitContainer splitContainer1;
        private ReaLTaiizor.Controls.DungeonTextBox dungeonTextBox1;
        private ReaLTaiizor.Controls.Button button2;
        private Label lblTong;
        private Label lblSL;
        private Label label10;
        private Helpler.RoundedPanel roundedPanel6;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Wiget2 wiget22;
        private Wiget2 wiget21;
        private Wiget2 wiget23;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}