namespace GUI.Type
{
    partial class AddType
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
            label5 = new Label();
            TbID = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            label2 = new Label();
            label1 = new Label();
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
            btnCancel.Location = new Point(635, 155);
            btnCancel.Margin = new Padding(3, 3, 160, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(0, 0, 100, 0);
            btnCancel.PressedBorderColor = Color.Green;
            btnCancel.PressedColor = Color.Green;
            btnCancel.RightToLeft = RightToLeft.Yes;
            btnCancel.Size = new Size(73, 28);
            btnCancel.TabIndex = 60;
            btnCancel.Text = "Bỏ qua";
            btnCancel.TextAlignment = StringAlignment.Center;
            btnCancel.Click += btnCancel_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(408, 65);
            label5.Name = "label5";
            label5.Size = new Size(67, 17);
            label5.TabIndex = 59;
            label5.Text = "Mã nhóm";
            // 
            // TbID
            // 
            TbID.AutoWordSelection = false;
            TbID.BackColor = Color.Transparent;
            TbID.BorderColor = Color.FromArgb(180, 180, 180);
            TbID.EdgeColor = Color.White;
            TbID.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TbID.ForeColor = Color.Gray;
            TbID.Location = new Point(408, 87);
            TbID.Name = "TbID";
            TbID.ReadOnly = true;
            TbID.Size = new Size(380, 32);
            TbID.TabIndex = 58;
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
            TbName.Location = new Point(12, 87);
            TbName.Name = "TbName";
            TbName.ReadOnly = false;
            TbName.Size = new Size(344, 32);
            TbName.TabIndex = 50;
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
            btnSave.Location = new Point(728, 155);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(60, 28);
            btnSave.TabIndex = 55;
            btnSave.Text = "Lưu";
            btnSave.TextAlignment = StringAlignment.Center;
            btnSave.Click += btnSave_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(69, 17);
            label2.TabIndex = 51;
            label2.Text = "Tên nhóm";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(130, 21);
            label1.TabIndex = 49;
            label1.Text = "Tạo nhóm hàng";
            // 
            // AddType
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 202);
            Controls.Add(btnCancel);
            Controls.Add(label5);
            Controls.Add(TbID);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddType";
            Text = "AddType";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.Button btnCancel;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbID;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private Label label2;
        private Label label1;
    }
}