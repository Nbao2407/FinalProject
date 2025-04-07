namespace QLVT.HoaDon
{
    partial class LydoHuy
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
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            txtLyDo = new ReaLTaiizor.Controls.SmallTextBox();
            hopeRoundButton1 = new ReaLTaiizor.Controls.HopeRoundButton();
            hopeRoundButton2 = new ReaLTaiizor.Controls.HopeRoundButton();
            SuspendLayout();
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.Black;
            bigLabel1.Location = new Point(141, 9);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(106, 30);
            bigLabel1.TabIndex = 1;
            bigLabel1.Text = "Lý do hủy";
            // 
            // txtLyDo
            // 
            txtLyDo.BackColor = Color.Transparent;
            txtLyDo.BorderColor = Color.FromArgb(180, 180, 180);
            txtLyDo.CustomBGColor = Color.White;
            txtLyDo.Font = new Font("Tahoma", 11F);
            txtLyDo.ForeColor = Color.DimGray;
            txtLyDo.Location = new Point(12, 54);
            txtLyDo.MaxLength = 32767;
            txtLyDo.Multiline = true;
            txtLyDo.Name = "txtLyDo";
            txtLyDo.ReadOnly = false;
            txtLyDo.Size = new Size(376, 78);
            txtLyDo.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            txtLyDo.TabIndex = 2;
            txtLyDo.Text = "Vui lòng nhập lý do hủy hóa đơn";
            txtLyDo.TextAlignment = HorizontalAlignment.Left;
            txtLyDo.UseSystemPasswordChar = false;
            txtLyDo.Enter += txtLyDo_Enter;
            txtLyDo.Leave += txtLyDo_Leave;
            // 
            // hopeRoundButton1
            // 
            hopeRoundButton1.BorderColor = Color.FromArgb(220, 223, 230);
            hopeRoundButton1.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            hopeRoundButton1.DangerColor = Color.FromArgb(245, 108, 108);
            hopeRoundButton1.DefaultColor = Color.FromArgb(255, 255, 255);
            hopeRoundButton1.Font = new Font("Segoe UI", 12F);
            hopeRoundButton1.HoverTextColor = Color.FromArgb(48, 49, 51);
            hopeRoundButton1.InfoColor = Color.FromArgb(144, 147, 153);
            hopeRoundButton1.Location = new Point(278, 145);
            hopeRoundButton1.Name = "hopeRoundButton1";
            hopeRoundButton1.PrimaryColor = Color.LimeGreen;
            hopeRoundButton1.Size = new Size(110, 27);
            hopeRoundButton1.SuccessColor = Color.FromArgb(103, 194, 58);
            hopeRoundButton1.TabIndex = 3;
            hopeRoundButton1.Text = "Xác nhận";
            hopeRoundButton1.TextColor = Color.White;
            hopeRoundButton1.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton1.Click += hopeRoundButton1_Click;
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
            hopeRoundButton2.Location = new Point(12, 145);
            hopeRoundButton2.Name = "hopeRoundButton2";
            hopeRoundButton2.PrimaryColor = Color.Silver;
            hopeRoundButton2.Size = new Size(110, 27);
            hopeRoundButton2.SuccessColor = Color.FromArgb(255, 128, 128);
            hopeRoundButton2.TabIndex = 4;
            hopeRoundButton2.Text = "Hủy";
            hopeRoundButton2.TextColor = Color.Black;
            hopeRoundButton2.WarningColor = Color.FromArgb(230, 162, 60);
            hopeRoundButton2.Click += hopeRoundButton2_Click;
            // 
            // lblLydo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 184);
            Controls.Add(hopeRoundButton2);
            Controls.Add(hopeRoundButton1);
            Controls.Add(txtLyDo);
            Controls.Add(bigLabel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "lblLydo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "LydoHuy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private ReaLTaiizor.Controls.SmallTextBox txtLyDo;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton1;
        private ReaLTaiizor.Controls.HopeRoundButton hopeRoundButton2;
    }
}