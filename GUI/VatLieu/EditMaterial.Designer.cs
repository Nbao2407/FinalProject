namespace GUI.VatLieu
{
    partial class EditMaterial
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
            groupBox1 = new GroupBox();
            TbNote = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnTaiHinhAnh = new ReaLTaiizor.Controls.Button();
            CbType = new ReaLTaiizor.Controls.AloneComboBox();
            pictureBox1 = new PictureBox();
            label6 = new Label();
            label5 = new Label();
            TbID = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            label2 = new Label();
            groupBox2 = new GroupBox();
            CbDVT = new ReaLTaiizor.Controls.AloneComboBox();
            label10 = new Label();
            TbDonvi = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label9 = new Label();
            TbGiaBan = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label8 = new Label();
            TbGiaNhap = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label1 = new Label();
            btnCancel = new ReaLTaiizor.Controls.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(TbNote);
            groupBox1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(21, 322);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(806, 73);
            groupBox1.TabIndex = 55;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ghi Chú";
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
            TbNote.Size = new Size(794, 32);
            TbNote.TabIndex = 0;
            TbNote.TextBackColor = Color.White;
            TbNote.WordWrap = true;
            // 
            // btnTaiHinhAnh
            // 
            btnTaiHinhAnh.BackColor = Color.Transparent;
            btnTaiHinhAnh.BorderColor = Color.FromArgb(52, 73, 94);
            btnTaiHinhAnh.Cursor = Cursors.Hand;
            btnTaiHinhAnh.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            btnTaiHinhAnh.EnteredColor = Color.FromArgb(32, 34, 37);
            btnTaiHinhAnh.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTaiHinhAnh.Image = null;
            btnTaiHinhAnh.ImageAlign = ContentAlignment.MiddleRight;
            btnTaiHinhAnh.InactiveColor = SystemColors.ActiveBorder;
            btnTaiHinhAnh.Location = new Point(733, 135);
            btnTaiHinhAnh.Margin = new Padding(3, 3, 160, 3);
            btnTaiHinhAnh.Name = "btnTaiHinhAnh";
            btnTaiHinhAnh.Padding = new Padding(0, 0, 100, 0);
            btnTaiHinhAnh.PressedBorderColor = Color.Green;
            btnTaiHinhAnh.PressedColor = Color.Green;
            btnTaiHinhAnh.RightToLeft = RightToLeft.Yes;
            btnTaiHinhAnh.Size = new Size(51, 20);
            btnTaiHinhAnh.TabIndex = 54;
            btnTaiHinhAnh.Text = "Thêm";
            btnTaiHinhAnh.TextAlignment = StringAlignment.Center;
            btnTaiHinhAnh.Click += btnTaiHinhAnh_Click;
            // 
            // CbType
            // 
            CbType.DrawMode = DrawMode.OwnerDrawFixed;
            CbType.DropDownStyle = ComboBoxStyle.DropDownList;
            CbType.EnabledCalc = true;
            CbType.FormattingEnabled = true;
            CbType.ItemHeight = 20;
            CbType.Items.AddRange(new object[] { "Chọn Loại vật liệu" });
            CbType.Location = new Point(362, 179);
            CbType.Name = "CbType";
            CbType.Size = new Size(300, 26);
            CbType.TabIndex = 53;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.LightGray;
            pictureBox1.Location = new Point(693, 75);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(134, 130);
            pictureBox1.TabIndex = 52;
            pictureBox1.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(369, 155);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 50;
            label6.Text = "Loại vật liệu";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(369, 78);
            label5.Name = "label5";
            label5.Size = new Size(66, 17);
            label5.TabIndex = 49;
            label5.Text = "Mã  hàng";
            // 
            // TbID
            // 
            TbID.AutoWordSelection = false;
            TbID.BackColor = Color.Transparent;
            TbID.BorderColor = Color.FromArgb(180, 180, 180);
            TbID.EdgeColor = Color.White;
            TbID.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TbID.ForeColor = Color.Gray;
            TbID.Location = new Point(369, 100);
            TbID.Name = "TbID";
            TbID.ReadOnly = true;
            TbID.Size = new Size(293, 32);
            TbID.TabIndex = 48;
            TbID.Text = "Tự động";
            TbID.TextBackColor = Color.White;
            TbID.WordWrap = true;
            // 
            // TbName
            // 
            TbName.AutoWordSelection = false;
            TbName.BackColor = Color.Transparent;
            TbName.BorderColor = Color.FromArgb(180, 180, 180);
            TbName.EdgeColor = Color.White;
            TbName.Font = new Font("Tahoma", 10F);
            TbName.ForeColor = Color.Black;
            TbName.Location = new Point(21, 100);
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(295, 32);
            TbName.TabIndex = 43;
            TbName.TextBackColor = Color.White;
            TbName.WordWrap = true;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Transparent;
            btnSave.BorderColor = Color.FromArgb(52, 73, 94);
            btnSave.Cursor = Cursors.Hand;
            btnSave.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            btnSave.EnteredColor = Color.FromArgb(32, 34, 37);
            btnSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Image = null;
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.InactiveColor = Color.DodgerBlue;
            btnSave.Location = new Point(767, 414);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 46;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            btnSave.Click += btnLuu_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(21, 78);
            label2.Name = "label2";
            label2.Size = new Size(64, 17);
            label2.TabIndex = 44;
            label2.Text = "Tên hàng";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(CbDVT);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(TbDonvi);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(TbGiaBan);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(TbGiaNhap);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(21, 227);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(806, 89);
            groupBox2.TabIndex = 45;
            groupBox2.TabStop = false;
            groupBox2.Text = "Giá nhập, giá bán";
            // 
            // CbDVT
            // 
            CbDVT.DrawMode = DrawMode.OwnerDrawFixed;
            CbDVT.DropDownStyle = ComboBoxStyle.DropDownList;
            CbDVT.EnabledCalc = true;
            CbDVT.FormattingEnabled = true;
            CbDVT.ItemHeight = 20;
            CbDVT.Items.AddRange(new object[] { "Chọn Đơn vị" });
            CbDVT.Location = new Point(595, 43);
            CbDVT.Name = "CbDVT";
            CbDVT.Size = new Size(179, 26);
            CbDVT.TabIndex = 41;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(419, 23);
            label10.Name = "label10";
            label10.Size = new Size(75, 17);
            label10.TabIndex = 41;
            label10.Text = "Đơn vị tính";
            // 
            // TbDonvi
            // 
            TbDonvi.AutoWordSelection = false;
            TbDonvi.BackColor = Color.Transparent;
            TbDonvi.BorderColor = Color.FromArgb(180, 180, 180);
            TbDonvi.EdgeColor = Color.White;
            TbDonvi.Font = new Font("Tahoma", 10F);
            TbDonvi.ForeColor = Color.Black;
            TbDonvi.Location = new Point(419, 43);
            TbDonvi.Name = "TbDonvi";
            TbDonvi.ReadOnly = false;
            TbDonvi.Size = new Size(161, 32);
            TbDonvi.TabIndex = 40;
            TbDonvi.TextBackColor = Color.White;
            TbDonvi.WordWrap = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(218, 23);
            label9.Name = "label9";
            label9.Size = new Size(54, 17);
            label9.TabIndex = 39;
            label9.Text = "Giá bán";
            // 
            // TbGiaBan
            // 
            TbGiaBan.AutoWordSelection = false;
            TbGiaBan.BackColor = Color.Transparent;
            TbGiaBan.BorderColor = Color.FromArgb(180, 180, 180);
            TbGiaBan.EdgeColor = Color.White;
            TbGiaBan.Font = new Font("Tahoma", 10F);
            TbGiaBan.ForeColor = Color.Black;
            TbGiaBan.Location = new Point(218, 43);
            TbGiaBan.Name = "TbGiaBan";
            TbGiaBan.ReadOnly = false;
            TbGiaBan.Size = new Size(161, 32);
            TbGiaBan.TabIndex = 38;
            TbGiaBan.TextBackColor = Color.White;
            TbGiaBan.WordWrap = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(6, 23);
            label8.Name = "label8";
            label8.Size = new Size(62, 17);
            label8.TabIndex = 37;
            label8.Text = "Giá nhập";
            // 
            // TbGiaNhap
            // 
            TbGiaNhap.AutoWordSelection = false;
            TbGiaNhap.BackColor = Color.Transparent;
            TbGiaNhap.BorderColor = Color.FromArgb(180, 180, 180);
            TbGiaNhap.EdgeColor = Color.White;
            TbGiaNhap.Font = new Font("Tahoma", 10F);
            TbGiaNhap.ForeColor = Color.Black;
            TbGiaNhap.Location = new Point(6, 43);
            TbGiaNhap.Name = "TbGiaNhap";
            TbGiaNhap.ReadOnly = false;
            TbGiaNhap.Size = new Size(161, 32);
            TbGiaNhap.TabIndex = 0;
            TbGiaNhap.TextBackColor = Color.White;
            TbGiaNhap.WordWrap = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(21, 24);
            label1.Name = "label1";
            label1.Size = new Size(149, 30);
            label1.TabIndex = 57;
            label1.Text = "Sửa hàng hóa";
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Transparent;
            btnCancel.BorderColor = Color.FromArgb(52, 73, 94);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            btnCancel.EnteredColor = Color.FromArgb(32, 34, 37);
            btnCancel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Image = null;
            btnCancel.ImageAlign = ContentAlignment.MiddleRight;
            btnCancel.InactiveColor = SystemColors.ActiveBorder;
            btnCancel.Location = new Point(680, 414);
            btnCancel.Margin = new Padding(3, 3, 160, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(0, 0, 100, 0);
            btnCancel.PressedBorderColor = Color.Green;
            btnCancel.PressedColor = Color.Green;
            btnCancel.RightToLeft = RightToLeft.Yes;
            btnCancel.Size = new Size(64, 28);
            btnCancel.TabIndex = 58;
            btnCancel.Text = "Bỏ qua";
            btnCancel.TextAlignment = StringAlignment.Center;
            btnCancel.Click += btnCancel_Click;
            // 
            // EditMaterial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(840, 457);
            Controls.Add(btnCancel);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(btnTaiHinhAnh);
            Controls.Add(CbType);
            Controls.Add(pictureBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(TbID);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            Controls.Add(groupBox2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EditMaterial";
            Text = "EditMaterial";
            Load += EditMaterial_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBox1;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbNote;
        private ReaLTaiizor.Controls.Button btnTaiHinhAnh;
        private ReaLTaiizor.Controls.AloneComboBox CbType;
        private PictureBox pictureBox1;
        private Label label6;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbID;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private Label label2;
        private GroupBox groupBox2;
        private ReaLTaiizor.Controls.AloneComboBox CbDVT;
        private Label label10;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbDonvi;
        private Label label9;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbGiaBan;
        private Label label8;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbGiaNhap;
        private Label label1;
        private ReaLTaiizor.Controls.Button btnCancel;
    }
}