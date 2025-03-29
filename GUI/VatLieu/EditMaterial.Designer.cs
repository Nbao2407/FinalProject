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
            pictureBox1 = new PictureBox();
            label9 = new Label();
            dungeonRichTextBox1 = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label8 = new Label();
            TbAddress = new ReaLTaiizor.Controls.DungeonRichTextBox();
            button1 = new ReaLTaiizor.Controls.Button();
            label6 = new Label();
            CbType = new ReaLTaiizor.Controls.ComboBoxEdit();
            label4 = new Label();
            label5 = new Label();
            TbKho = new ReaLTaiizor.Controls.DungeonRichTextBox();
            dungeonRichTextBox5 = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            label2 = new Label();
            groupBox2 = new GroupBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.AppWorkspace;
            pictureBox1.Location = new Point(689, 60);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(134, 118);
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(241, 30);
            label9.Name = "label9";
            label9.Size = new Size(54, 17);
            label9.TabIndex = 39;
            label9.Text = "Giá bán";
            // 
            // dungeonRichTextBox1
            // 
            dungeonRichTextBox1.AutoWordSelection = false;
            dungeonRichTextBox1.BackColor = Color.Transparent;
            dungeonRichTextBox1.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonRichTextBox1.EdgeColor = Color.White;
            dungeonRichTextBox1.Font = new Font("Tahoma", 10F);
            dungeonRichTextBox1.ForeColor = Color.Black;
            dungeonRichTextBox1.Location = new Point(241, 50);
            dungeonRichTextBox1.Name = "dungeonRichTextBox1";
            dungeonRichTextBox1.ReadOnly = false;
            dungeonRichTextBox1.Size = new Size(161, 32);
            dungeonRichTextBox1.TabIndex = 38;
            dungeonRichTextBox1.TextBackColor = Color.White;
            dungeonRichTextBox1.WordWrap = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(6, 30);
            label8.Name = "label8";
            label8.Size = new Size(62, 17);
            label8.TabIndex = 37;
            label8.Text = "Giá nhập";
            // 
            // TbAddress
            // 
            TbAddress.AutoWordSelection = false;
            TbAddress.BackColor = Color.Transparent;
            TbAddress.BorderColor = Color.FromArgb(180, 180, 180);
            TbAddress.EdgeColor = Color.White;
            TbAddress.Font = new Font("Tahoma", 10F);
            TbAddress.ForeColor = Color.Black;
            TbAddress.Location = new Point(6, 50);
            TbAddress.Name = "TbAddress";
            TbAddress.ReadOnly = false;
            TbAddress.Size = new Size(161, 32);
            TbAddress.TabIndex = 0;
            TbAddress.TextBackColor = Color.White;
            TbAddress.WordWrap = true;
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
            button1.Location = new Point(667, 343);
            button1.Margin = new Padding(3, 3, 160, 3);
            button1.Name = "button1";
            button1.Padding = new Padding(0, 0, 100, 0);
            button1.PressedBorderColor = Color.Green;
            button1.PressedColor = Color.Green;
            button1.RightToLeft = RightToLeft.Yes;
            button1.Size = new Size(73, 28);
            button1.TabIndex = 49;
            button1.Text = "Bỏ qua";
            button1.TextAlignment = StringAlignment.Center;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(365, 128);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 48;
            label6.Text = "Loại vật liệu";
            // 
            // CbType
            // 
            CbType.BackColor = Color.FromArgb(246, 246, 246);
            CbType.DrawMode = DrawMode.OwnerDrawFixed;
            CbType.DropDownHeight = 100;
            CbType.DropDownStyle = ComboBoxStyle.DropDownList;
            CbType.Font = new Font("Segoe UI", 10F);
            CbType.ForeColor = Color.Black;
            CbType.FormattingEnabled = true;
            CbType.HoverSelectionColor = Color.FromArgb(241, 241, 241);
            CbType.IntegralHeight = false;
            CbType.ItemHeight = 20;
            CbType.Items.AddRange(new object[] { "Chọn loại vật liệu(Bắt buộc)", "Nam", "Nữ" });
            CbType.Location = new Point(365, 148);
            CbType.Name = "CbType";
            CbType.Size = new Size(293, 26);
            CbType.StartIndex = 0;
            CbType.TabIndex = 47;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 122);
            label4.Name = "label4";
            label4.Size = new Size(58, 17);
            label4.TabIndex = 44;
            label4.Text = "Tồn kho";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(365, 60);
            label5.Name = "label5";
            label5.Size = new Size(66, 17);
            label5.TabIndex = 46;
            label5.Text = "Mã  hàng";
            // 
            // TbKho
            // 
            TbKho.AutoWordSelection = false;
            TbKho.BackColor = Color.Transparent;
            TbKho.BorderColor = Color.FromArgb(180, 180, 180);
            TbKho.EdgeColor = Color.White;
            TbKho.Font = new Font("Tahoma", 10F);
            TbKho.ForeColor = Color.Black;
            TbKho.Location = new Point(17, 143);
            TbKho.Name = "TbKho";
            TbKho.ReadOnly = false;
            TbKho.Size = new Size(295, 32);
            TbKho.TabIndex = 43;
            TbKho.TextBackColor = Color.White;
            TbKho.WordWrap = true;
            // 
            // dungeonRichTextBox5
            // 
            dungeonRichTextBox5.AutoWordSelection = false;
            dungeonRichTextBox5.BackColor = Color.Transparent;
            dungeonRichTextBox5.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonRichTextBox5.EdgeColor = Color.White;
            dungeonRichTextBox5.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dungeonRichTextBox5.ForeColor = Color.Gray;
            dungeonRichTextBox5.Location = new Point(365, 82);
            dungeonRichTextBox5.Name = "dungeonRichTextBox5";
            dungeonRichTextBox5.ReadOnly = true;
            dungeonRichTextBox5.Size = new Size(293, 32);
            dungeonRichTextBox5.TabIndex = 45;
            dungeonRichTextBox5.Text = "Tự động";
            dungeonRichTextBox5.TextBackColor = Color.White;
            dungeonRichTextBox5.WordWrap = true;
            // 
            // TbName
            // 
            TbName.AutoWordSelection = false;
            TbName.BackColor = Color.Transparent;
            TbName.BorderColor = Color.FromArgb(180, 180, 180);
            TbName.EdgeColor = Color.White;
            TbName.Font = new Font("Tahoma", 10F);
            TbName.ForeColor = Color.Black;
            TbName.Location = new Point(17, 82);
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(295, 32);
            TbName.TabIndex = 39;
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
            btnSave.Location = new Point(763, 343);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 42;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(17, 60);
            label2.Name = "label2";
            label2.Size = new Size(64, 17);
            label2.TabIndex = 40;
            label2.Text = "Tên hàng";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(dungeonRichTextBox1);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(TbAddress);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(17, 200);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(806, 112);
            groupBox2.TabIndex = 41;
            groupBox2.TabStop = false;
            groupBox2.Text = "Giá nhập, giá bán";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(17, 14);
            label1.Name = "label1";
            label1.Size = new Size(115, 21);
            label1.TabIndex = 38;
            label1.Text = "Sửa hàng hóa";
            // 
            // EditMaterial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(840, 384);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(label6);
            Controls.Add(CbType);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(TbKho);
            Controls.Add(dungeonRichTextBox5);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            Controls.Add(groupBox2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EditMaterial";
            Text = "EditMaterial";
            Load += EditMaterial_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label9;
        private ReaLTaiizor.Controls.DungeonRichTextBox dungeonRichTextBox1;
        private Label label8;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbAddress;
        private ReaLTaiizor.Controls.Button button1;
        private Label label6;
        private ReaLTaiizor.Controls.ComboBoxEdit CbType;
        private Label label4;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbKho;
        private ReaLTaiizor.Controls.DungeonRichTextBox dungeonRichTextBox5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private Label label2;
        private GroupBox groupBox2;
        private Label label1;
    }
}