namespace GUI.TaiKhoan
{
    partial class AddTk
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
            btnCancel = new ReaLTaiizor.Controls.Button();
            label4 = new Label();
            label5 = new Label();
            TbEmail = new ReaLTaiizor.Controls.DungeonRichTextBox();
            dungeonRichTextBox5 = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label3 = new Label();
            TbSdt = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            TbAddress = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label2 = new Label();
            groupBox2 = new GroupBox();
            TbNote = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label1 = new Label();
            CbChucVu = new ReaLTaiizor.Controls.AloneComboBox();
            label6 = new Label();
            label7 = new Label();
            Tbpass = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label8 = new Label();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Transparent;
            btnCancel.BorderColor = Color.FromArgb(52, 73, 94);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            btnCancel.EnteredColor = Color.FromArgb(32, 34, 37);
            btnCancel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Image = null;
            btnCancel.ImageAlign = ContentAlignment.MiddleRight;
            btnCancel.InactiveColor = Color.LightGray;
            btnCancel.Location = new Point(669, 442);
            btnCancel.Margin = new Padding(3, 3, 160, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(0, 0, 100, 0);
            btnCancel.PressedBorderColor = Color.Green;
            btnCancel.PressedColor = Color.Green;
            btnCancel.RightToLeft = RightToLeft.Yes;
            btnCancel.Size = new Size(73, 28);
            btnCancel.TabIndex = 36;
            btnCancel.Text = "Bỏ qua";
            btnCancel.TextAlignment = StringAlignment.Center;
            btnCancel.Click += btnCancel_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(442, 125);
            label4.Name = "label4";
            label4.Size = new Size(40, 17);
            label4.TabIndex = 29;
            label4.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(442, 57);
            label5.Name = "label5";
            label5.Size = new Size(88, 17);
            label5.TabIndex = 31;
            label5.Text = "Mã tài khoản";
            // 
            // TbEmail
            // 
            TbEmail.AutoWordSelection = false;
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(180, 180, 180);
            TbEmail.EdgeColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 10F);
            TbEmail.ForeColor = Color.Black;
            TbEmail.Location = new Point(442, 146);
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(380, 32);
            TbEmail.TabIndex = 28;
            TbEmail.TextBackColor = Color.White;
            TbEmail.WordWrap = true;
            // 
            // dungeonRichTextBox5
            // 
            dungeonRichTextBox5.AutoWordSelection = false;
            dungeonRichTextBox5.BackColor = Color.Transparent;
            dungeonRichTextBox5.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonRichTextBox5.EdgeColor = Color.White;
            dungeonRichTextBox5.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dungeonRichTextBox5.ForeColor = Color.Gray;
            dungeonRichTextBox5.Location = new Point(442, 79);
            dungeonRichTextBox5.Name = "dungeonRichTextBox5";
            dungeonRichTextBox5.ReadOnly = true;
            dungeonRichTextBox5.Size = new Size(380, 32);
            dungeonRichTextBox5.TabIndex = 30;
            dungeonRichTextBox5.Text = "Tự động";
            dungeonRichTextBox5.TextBackColor = Color.White;
            dungeonRichTextBox5.WordWrap = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 125);
            label3.Name = "label3";
            label3.Size = new Size(70, 17);
            label3.TabIndex = 26;
            label3.Text = "Điện thoại";
            // 
            // TbSdt
            // 
            TbSdt.AutoWordSelection = false;
            TbSdt.BackColor = Color.Transparent;
            TbSdt.BorderColor = Color.FromArgb(180, 180, 180);
            TbSdt.EdgeColor = Color.White;
            TbSdt.Font = new Font("Tahoma", 10F);
            TbSdt.ForeColor = Color.Black;
            TbSdt.Location = new Point(12, 146);
            TbSdt.Name = "TbSdt";
            TbSdt.ReadOnly = false;
            TbSdt.Size = new Size(344, 32);
            TbSdt.TabIndex = 25;
            TbSdt.TextBackColor = Color.White;
            TbSdt.WordWrap = true;
            // 
            // TbName
            // 
            TbName.AutoWordSelection = false;
            TbName.BackColor = Color.Transparent;
            TbName.BorderColor = Color.FromArgb(180, 180, 180);
            TbName.EdgeColor = Color.White;
            TbName.Font = new Font("Tahoma", 10F);
            TbName.ForeColor = Color.Black;
            TbName.Location = new Point(12, 79);
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(344, 32);
            TbName.TabIndex = 22;
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
            btnSave.Location = new Point(762, 442);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 27;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            // 
            // TbAddress
            // 
            TbAddress.AutoWordSelection = false;
            TbAddress.BackColor = Color.Transparent;
            TbAddress.BorderColor = Color.FromArgb(180, 180, 180);
            TbAddress.EdgeColor = Color.White;
            TbAddress.Font = new Font("Tahoma", 10F);
            TbAddress.ForeColor = Color.Black;
            TbAddress.Location = new Point(12, 297);
            TbAddress.Name = "TbAddress";
            TbAddress.ReadOnly = false;
            TbAddress.Size = new Size(816, 32);
            TbAddress.TabIndex = 0;
            TbAddress.TextBackColor = Color.White;
            TbAddress.WordWrap = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(99, 17);
            label2.TabIndex = 23;
            label2.Text = "Tên đăng nhập";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(TbNote);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(6, 355);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(827, 74);
            groupBox2.TabIndex = 24;
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
            TbNote.Size = new Size(816, 32);
            TbNote.TabIndex = 42;
            TbNote.TextBackColor = Color.White;
            TbNote.WordWrap = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(114, 21);
            label1.TabIndex = 21;
            label1.Text = "Tạo tài khoản";
            // 
            // CbChucVu
            // 
            CbChucVu.DrawMode = DrawMode.OwnerDrawFixed;
            CbChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
            CbChucVu.EnabledCalc = true;
            CbChucVu.FormattingEnabled = true;
            CbChucVu.ItemHeight = 20;
            CbChucVu.Location = new Point(12, 223);
            CbChucVu.Name = "CbChucVu";
            CbChucVu.Size = new Size(344, 26);
            CbChucVu.TabIndex = 37;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 194);
            label6.Name = "label6";
            label6.Size = new Size(57, 17);
            label6.TabIndex = 38;
            label6.Text = "Chức vụ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 266);
            label7.Name = "label7";
            label7.Size = new Size(48, 17);
            label7.TabIndex = 39;
            label7.Text = "Địa chỉ";
            label7.Click += label7_Click;
            // 
            // Tbpass
            // 
            Tbpass.AutoWordSelection = false;
            Tbpass.BackColor = Color.Transparent;
            Tbpass.BorderColor = Color.FromArgb(180, 180, 180);
            Tbpass.EdgeColor = Color.White;
            Tbpass.Font = new Font("Tahoma", 10F);
            Tbpass.ForeColor = Color.Black;
            Tbpass.Location = new Point(442, 217);
            Tbpass.Name = "Tbpass";
            Tbpass.ReadOnly = false;
            Tbpass.Size = new Size(380, 32);
            Tbpass.TabIndex = 40;
            Tbpass.TextBackColor = Color.White;
            Tbpass.WordWrap = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(442, 194);
            label8.Name = "label8";
            label8.Size = new Size(66, 17);
            label8.TabIndex = 41;
            label8.Text = "Mật khẩu";
            // 
            // AddTk
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(843, 482);
            Controls.Add(TbAddress);
            Controls.Add(label8);
            Controls.Add(Tbpass);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(CbChucVu);
            Controls.Add(btnCancel);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(TbEmail);
            Controls.Add(dungeonRichTextBox5);
            Controls.Add(label3);
            Controls.Add(TbSdt);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            Controls.Add(groupBox2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddTk";
            Text = "AddTk";
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.Button btnCancel;
        private Label label4;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbEmail;
        private ReaLTaiizor.Controls.DungeonRichTextBox dungeonRichTextBox5;
        private Label label3;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbSdt;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbAddress;
        private Label label2;
        private GroupBox groupBox2;
        private Label label1;
        private ReaLTaiizor.Controls.AloneComboBox CbChucVu;
        private Label label6;
        private Label label7;
        private ReaLTaiizor.Controls.DungeonRichTextBox Tbpass;
        private Label label8;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbNote;
    }
}