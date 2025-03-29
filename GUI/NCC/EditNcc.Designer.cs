namespace GUI.NCC
{
    partial class EditNcc
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
            TbID = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label3 = new Label();
            TbSdt = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            TbAddress = new ReaLTaiizor.Controls.DungeonRichTextBox();
            label2 = new Label();
            groupBox2 = new GroupBox();
            label1 = new Label();
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
            btnCancel.Location = new Point(636, 362);
            btnCancel.Margin = new Padding(3, 3, 160, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(0, 0, 100, 0);
            btnCancel.PressedBorderColor = Color.Green;
            btnCancel.PressedColor = Color.Green;
            btnCancel.RightToLeft = RightToLeft.Yes;
            btnCancel.Size = new Size(73, 28);
            btnCancel.TabIndex = 48;
            btnCancel.Text = "Bỏ qua";
            btnCancel.TextAlignment = StringAlignment.Center;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(408, 125);
            label4.Name = "label4";
            label4.Size = new Size(40, 17);
            label4.TabIndex = 45;
            label4.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(408, 57);
            label5.Name = "label5";
            label5.Size = new Size(113, 17);
            label5.TabIndex = 47;
            label5.Text = "Mã nhà cung cấp";
            // 
            // TbEmail
            // 
            TbEmail.AutoWordSelection = false;
            TbEmail.BackColor = Color.Transparent;
            TbEmail.BorderColor = Color.FromArgb(180, 180, 180);
            TbEmail.EdgeColor = Color.White;
            TbEmail.Font = new Font("Tahoma", 10F);
            TbEmail.ForeColor = Color.Black;
            TbEmail.Location = new Point(408, 146);
            TbEmail.Name = "TbEmail";
            TbEmail.ReadOnly = false;
            TbEmail.Size = new Size(380, 32);
            TbEmail.TabIndex = 44;
            TbEmail.TextBackColor = Color.White;
            TbEmail.WordWrap = true;
            // 
            // TbID
            // 
            TbID.AutoWordSelection = false;
            TbID.BackColor = Color.Transparent;
            TbID.BorderColor = Color.FromArgb(180, 180, 180);
            TbID.EdgeColor = Color.White;
            TbID.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TbID.ForeColor = Color.Gray;
            TbID.Location = new Point(408, 79);
            TbID.Name = "TbID";
            TbID.ReadOnly = true;
            TbID.Size = new Size(380, 32);
            TbID.TabIndex = 46;
            TbID.Text = "Tự động";
            TbID.TextBackColor = Color.White;
            TbID.WordWrap = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 125);
            label3.Name = "label3";
            label3.Size = new Size(70, 17);
            label3.TabIndex = 42;
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
            TbSdt.TabIndex = 41;
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
            TbName.TabIndex = 38;
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
            btnSave.Location = new Point(729, 362);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 43;
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
            TbAddress.Location = new Point(6, 26);
            TbAddress.Name = "TbAddress";
            TbAddress.ReadOnly = false;
            TbAddress.Size = new Size(764, 32);
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
            label2.Size = new Size(115, 17);
            label2.TabIndex = 39;
            label2.Text = "Tên nhà cung cấp";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(TbAddress);
            groupBox2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 250);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(776, 74);
            groupBox2.TabIndex = 40;
            groupBox2.TabStop = false;
            groupBox2.Text = "Địa Chỉ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(192, 21);
            label1.TabIndex = 37;
            label1.Text = "Chỉnh sửa nhà cung cấp";
            // 
            // EditNcc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 400);
            Controls.Add(btnCancel);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(TbEmail);
            Controls.Add(TbID);
            Controls.Add(label3);
            Controls.Add(TbSdt);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            Controls.Add(groupBox2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EditNcc";
            Text = "EditNcc";
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.Button btnCancel;
        private Label label4;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbEmail;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbID;
        private Label label3;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbSdt;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbAddress;
        private Label label2;
        private GroupBox groupBox2;
        private Label label1;
    }
}