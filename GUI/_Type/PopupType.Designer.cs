namespace GUI.Type
{
    partial class PopupType
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
            btnDelete = new ReaLTaiizor.Controls.Button();
            label5 = new Label();
            TbID = new ReaLTaiizor.Controls.DungeonRichTextBox();
            TbName = new ReaLTaiizor.Controls.DungeonRichTextBox();
            btnSave = new ReaLTaiizor.Controls.Button();
            label2 = new Label();
            panel5 = new Panel();
            roundedPictureBox1 = new GUI.Helpler.RoundedPictureBox();
            roundedPictureBox2 = new GUI.Helpler.RoundedPictureBox();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Transparent;
            btnDelete.BorderColor = Color.FromArgb(52, 73, 94);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            btnDelete.EnteredColor = Color.FromArgb(32, 34, 37);
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.Image = null;
            btnDelete.ImageAlign = ContentAlignment.MiddleRight;
            btnDelete.InactiveColor = Color.LightGray;
            btnDelete.Location = new Point(12, 155);
            btnDelete.Margin = new Padding(3, 3, 160, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Padding = new Padding(0, 0, 100, 0);
            btnDelete.PressedBorderColor = Color.Green;
            btnDelete.PressedColor = Color.Green;
            btnDelete.RightToLeft = RightToLeft.Yes;
            btnDelete.Size = new Size(73, 28);
            btnDelete.TabIndex = 67;
            btnDelete.Text = "Xóa";
            btnDelete.TextAlignment = StringAlignment.Center;
            btnDelete.Click += btnCancel_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(408, 65);
            label5.Name = "label5";
            label5.Size = new Size(67, 17);
            label5.TabIndex = 66;
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
            TbID.TabIndex = 65;
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
            TbName.TabIndex = 62;
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
            btnSave.InactiveColor = Color.Green;
            btnSave.Location = new Point(679, 155);
            btnSave.Margin = new Padding(3, 3, 160, 3);
            btnSave.Name = "btnSave";
            btnSave.Padding = new Padding(0, 0, 100, 0);
            btnSave.PressedBorderColor = Color.Green;
            btnSave.PressedColor = Color.Green;
            btnSave.RightToLeft = RightToLeft.Yes;
            btnSave.Size = new Size(109, 28);
            btnSave.TabIndex = 64;
            btnSave.Text = "Chỉnh sửa";
            btnSave.TextAlignment = StringAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(69, 17);
            label2.TabIndex = 63;
            label2.Text = "Tên nhóm";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(44, 62, 80);
            panel5.Controls.Add(roundedPictureBox2);
            panel5.Controls.Add(roundedPictureBox1);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(800, 38);
            panel5.TabIndex = 68;
            // 
            // roundedPictureBox1
            // 
            roundedPictureBox1.BorderColor = Color.Transparent;
            roundedPictureBox1.BorderRadius = 20;
            roundedPictureBox1.BorderThickness = 0F;
            roundedPictureBox1.ErrorImage = null;
            roundedPictureBox1.Image = Properties.Resources.icons8_close_26;
            roundedPictureBox1.InitialImage = null;
            roundedPictureBox1.Location = new Point(830, 7);
            roundedPictureBox1.Name = "roundedPictureBox1";
            roundedPictureBox1.Size = new Size(26, 26);
            roundedPictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox1.TabIndex = 41;
            roundedPictureBox1.TabStop = false;
            // 
            // roundedPictureBox2
            // 
            roundedPictureBox2.BorderColor = Color.Transparent;
            roundedPictureBox2.BorderRadius = 20;
            roundedPictureBox2.BorderThickness = 0F;
            roundedPictureBox2.ErrorImage = null;
            roundedPictureBox2.Image = Properties.Resources.icons8_close_26;
            roundedPictureBox2.InitialImage = null;
            roundedPictureBox2.Location = new Point(762, 9);
            roundedPictureBox2.Name = "roundedPictureBox2";
            roundedPictureBox2.Size = new Size(26, 26);
            roundedPictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            roundedPictureBox2.TabIndex = 42;
            roundedPictureBox2.TabStop = false;
            // 
            // PopupType
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 202);
            Controls.Add(panel5);
            Controls.Add(btnDelete);
            Controls.Add(label5);
            Controls.Add(TbID);
            Controls.Add(TbName);
            Controls.Add(btnSave);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopupType";
            Text = "PopupType";
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.Button btnDelete;
        private Label label5;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbID;
        private ReaLTaiizor.Controls.DungeonRichTextBox TbName;
        private ReaLTaiizor.Controls.Button btnSave;
        private Label label2;
        private Panel panel5;
        private Helpler.RoundedPictureBox roundedPictureBox1;
        private Helpler.RoundedPictureBox roundedPictureBox2;
    }
}