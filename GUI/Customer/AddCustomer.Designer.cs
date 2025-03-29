namespace GUI
{
    partial class AddCustomer
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
            label1 = new Label();
            groupBox2 = new GroupBox();
            TbAddress = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label2 = new Label();
            TbSdt = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label3 = new Label();
            label4 = new Label();
            TbEmail = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label5 = new Label();
            dungeonRichTextBox5 = new ReaLTaiizor.Controls.DungeonRichTextBox();
            CbGender = new ReaLTaiizor.Controls.ComboBoxEdit();
            label6 = new Label();
            DateTime = new ReaLTaiizor.Controls.PoisonDateTime();
            label7 = new Label();
            button1 = new ReaLTaiizor.Controls.Button();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(134, 21);
            label1.TabIndex = 0;
            label1.Text = "Tạo Khách Hàng";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(TbAddress);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 261);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(776, 74);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Địa Chỉ";
            // 
            // TbAddress
            // 
            TbAddress.AutoWordSelection = false;
            TbAddress.BackColor = Color.Transparent;
            TbAddress.BorderColor = Color.FromArgb(180, 180, 180);
            TbAddress.EdgeColor = Color.White;
            TbAddress.Font = new Font("Tahoma", 10F);
            TbAddress.ForeColor = Color.Black;
            TbAddress.Location = new Point(6, 26);
            TbAddress.Name = "TbAddress";
            TbAddress.ReadOnly = false;
            TbAddress.Size = new Size(764, 32);
            TbAddress.TabIndex = 0;
            TbAddress.TextBackColor = Color.White;
            TbAddress.WordWrap = true;
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
            btnSave.Location = new Point(729, 360);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 10;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            btnSave.Click += btnSave_Click;
            // 
            // TbName
            // 
            TbName.AutoWordSelection = false;
            TbName.BackColor = Color.Transparent;
            TbName.BorderColor = Color.FromArgb(180, 180, 180);
            TbName.EdgeColor = Color.White;
            TbName.Font = new Font("Tahoma", 10F);
            TbName.ForeColor = Color.Black;
            TbName.Location = new Point(12, 77);
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(344, 32);
            TbName.TabIndex = 1;
            TbName.TextBackColor = Color.White;
            TbName.WordWrap = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(104, 17);
            label2.TabIndex = 2;
            label2.Text = "Tên khách hàng";
            // 
            // TbSdt
            // 
            TbSdt.AutoWordSelection = false;
            TbSdt.BackColor = Color.Transparent;
            TbSdt.BorderColor = Color.FromArgb(180, 180, 180);
            TbSdt.EdgeColor = Color.White;
            TbSdt.Font = new Font("Tahoma", 10F);
            TbSdt.ForeColor = Color.Black;
            TbSdt.Location = new Point(12, 144);
            TbSdt.Name = "TbSdt";
            TbSdt.ReadOnly = false;
            TbSdt.Size = new Size(344, 32);
            TbSdt.TabIndex = 3;
            TbSdt.TextBackColor = Color.White;
            TbSdt.WordWrap = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 123);
            label3.Name = "label3";
            label3.Size = new Size(70, 17);
            label3.TabIndex = 4;
            label3.Text = "Điện thoại";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 192);
            label4.Name = "label4";
            label4.Size = new Size(40, 17);
            label4.TabIndex = 13;
            label4.Text = "Email";
            // 
            // TbEmail
            // 
            TbEmail.AutoWordSelection = false;
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(180, 180, 180);
            TbEmail.EdgeColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 10F);
            TbEmail.ForeColor = Color.Black;
            TbEmail.Location = new Point(12, 213);
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(344, 32);
            TbEmail.TabIndex = 12;
            TbEmail.TextBackColor = Color.White;
            TbEmail.WordWrap = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(408, 55);
            label5.Name = "label5";
            label5.Size = new Size(102, 17);
            label5.TabIndex = 15;
            label5.Text = "Mã khách hàng";
            // 
            // dungeonRichTextBox5
            // 
            dungeonRichTextBox5.AutoWordSelection = false;
            dungeonRichTextBox5.BackColor = Color.Transparent;
            dungeonRichTextBox5.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonRichTextBox5.EdgeColor = Color.White;
            dungeonRichTextBox5.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dungeonRichTextBox5.ForeColor = Color.Gray;
            dungeonRichTextBox5.Location = new Point(408, 77);
            dungeonRichTextBox5.Name = "dungeonRichTextBox5";
            dungeonRichTextBox5.ReadOnly = true;
            dungeonRichTextBox5.Size = new Size(380, 32);
            dungeonRichTextBox5.TabIndex = 14;
            dungeonRichTextBox5.Text = "Tự động";
            dungeonRichTextBox5.TextBackColor = Color.White;
            dungeonRichTextBox5.WordWrap = true;
            // 
            // CbGender
            // 
            CbGender.BackColor = Color.FromArgb(246, 246, 246);
            CbGender.DrawMode = DrawMode.OwnerDrawFixed;
            CbGender.DropDownHeight = 100;
            CbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            CbGender.Font = new Font("Segoe UI", 10F);
            CbGender.ForeColor = Color.Black;
            CbGender.FormattingEnabled = true;
            CbGender.HoverSelectionColor = Color.FromArgb(241, 241, 241);
            CbGender.IntegralHeight = false;
            CbGender.ItemHeight = 20;
            CbGender.Items.AddRange(new object[] { "Chọn Giới tính", "Nam", "Nữ" });
            CbGender.Location = new Point(408, 144);
            CbGender.Name = "CbGender";
            CbGender.Size = new Size(380, 26);
            CbGender.StartIndex = 0;
            CbGender.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(408, 123);
            label6.Name = "label6";
            label6.Size = new Size(59, 17);
            label6.TabIndex = 17;
            label6.Text = "Giới tính";
            // 
            // DateTime
            // 
            DateTime.Location = new Point(408, 216);
            DateTime.MinimumSize = new Size(0, 29);
            DateTime.Name = "DateTime";
            DateTime.Size = new Size(380, 29);
            DateTime.TabIndex = 18;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(408, 192);
            label7.Name = "label7";
            label7.Size = new Size(66, 17);
            label7.TabIndex = 19;
            label7.Text = "Sinh nhật";
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BorderColor = Color.FromArgb(52, 73, 94);
            button1.Cursor = Cursors.Hand;
            button1.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button1.EnteredColor = Color.FromArgb(32, 34, 37);
            button1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Image = null;
            button1.ImageAlign = ContentAlignment.MiddleRight;
            button1.InactiveColor = Color.LightGray;
            button1.Location = new Point(636, 360);
            button1.Margin = new Padding(3, 3, 160, 3);
            button1.Name = "button1";
            button1.Padding = new Padding(0, 0, 100, 0);
            button1.PressedBorderColor = Color.Green;
            button1.PressedColor = Color.Green;
            button1.RightToLeft = RightToLeft.Yes;
            button1.Size = new Size(73, 28);
            button1.TabIndex = 20;
            button1.Text = "Bỏ qua";
            button1.TextAlignment = StringAlignment.Center;
            button1.Click += btncancel_Click;
            // 
            // AddCustomer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 400);
            Controls.Add(button1);
            Controls.Add(label7);
            Controls.Add(DateTime);
            Controls.Add(label6);
            Controls.Add(CbGender);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(TbEmail);
            Controls.Add(dungeonRichTextBox5);
            Controls.Add(label3);
            Controls.Add(TbSdt);
            Controls.Add(TbName);
            Controls.Add(label2);
            Controls.Add(btnSave);
            Controls.Add(groupBox2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddCustomer";
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private GroupBox groupBox2;
        private ReaLTaiizor.Controls.Button btnSave;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbAddress;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox dungeonRichTextBox5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private Label label2;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbSdt;
        private Label label3;
        private Label label4;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbEmail;
        private ReaLTaiizor.Controls.ComboBoxEdit CbGender;
        private Label label6;
        private ReaLTaiizor.Controls.PoisonDateTime DateTime;
        private Label label7;
        private ReaLTaiizor.Controls.Button button1;
    }
}