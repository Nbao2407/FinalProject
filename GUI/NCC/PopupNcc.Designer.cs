using GUI.Helpler;

namespace GUI
{
    partial class PopNcc
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
            label2 = new Label();
            bigLabel2 = new ReaLTaiizor.Controls.BigLabel();
            lblNgTao = new ReaLTaiizor.Controls.BigLabel();
            bigLabel5 = new ReaLTaiizor.Controls.BigLabel();
            lblName = new ReaLTaiizor.Controls.BigLabel();
            TbPhone = new TextBox();
            panel2 = new Panel();
            TbEmail = new TextBox();
            panel1 = new Panel();
            txtEmail = new Label();
            TbAddress = new TextBox();
            panel4 = new Panel();
            label4 = new Label();
            BtnDelete = new ReaLTaiizor.Controls.AloneButton();
            BtnEdit = new ReaLTaiizor.Controls.HopeRoundButton();
            panel5 = new Panel();
            roundedPictureBox1 = new RoundedPictureBox();
            ID = new ReaLTaiizor.Controls.BigLabel();
            panel3 = new Panel();
            Ngay = new TextBox();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(32, 187);
            label2.Name = "label2";
            label2.Size = new Size(84, 21);
            label2.TabIndex = 15;
            label2.Text = "Điện Thoại";
            // 
            // bigLabel2
            // 
            bigLabel2.AutoSize = true;
            bigLabel2.BackColor = Color.Transparent;
            bigLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel2.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel2.Location = new Point(221, 101);
            bigLabel2.Name = "bigLabel2";
            bigLabel2.Size = new Size(80, 21);
            bigLabel2.TabIndex = 12;
            bigLabel2.Text = "Ngày tạo :";
            // 
            // lblNgTao
            // 
            lblNgTao.AutoSize = true;
            lblNgTao.BackColor = Color.Transparent;
            lblNgTao.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNgTao.ForeColor = Color.FromArgb(80, 80, 80);
            lblNgTao.Location = new Point(125, 101);
            lblNgTao.Name = "lblNgTao";
            lblNgTao.Size = new Size(19, 21);
            lblNgTao.TabIndex = 11;
            lblNgTao.Text = "X";
            // 
            // bigLabel5
            // 
            bigLabel5.AutoSize = true;
            bigLabel5.BackColor = Color.Transparent;
            bigLabel5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bigLabel5.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel5.Location = new Point(32, 101);
            bigLabel5.Name = "bigLabel5";
            bigLabel5.Size = new Size(87, 21);
            bigLabel5.TabIndex = 10;
            bigLabel5.Text = "Người tạo :";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.BackColor = Color.Transparent;
            lblName.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblName.ForeColor = Color.FromArgb(80, 80, 80);
            lblName.Location = new Point(28, 59);
            lblName.Name = "lblName";
            lblName.Size = new Size(57, 37);
            lblName.TabIndex = 9;
            lblName.Text = "Tên";
            // 
            // TbPhone
            // 
            TbPhone.BackColor = SystemColors.Control;
            TbPhone.BorderStyle = BorderStyle.None;
            TbPhone.Font = new Font("Microsoft Sans Serif", 9.75F);
            TbPhone.ForeColor = Color.Black;
            TbPhone.Location = new Point(32, 215);
            TbPhone.Multiline = true;
            TbPhone.Name = "TbPhone";
            TbPhone.Size = new Size(374, 24);
            TbPhone.TabIndex = 18;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Black;
            panel2.Location = new Point(32, 241);
            panel2.Name = "panel2";
            panel2.Size = new Size(377, 1);
            panel2.TabIndex = 17;
            // 
            // TbEmail
            // 
            TbEmail.BackColor = SystemColors.Control;
            TbEmail.BorderStyle = BorderStyle.None;
            TbEmail.Font = new Font("Microsoft Sans Serif", 9.75F);
            TbEmail.ForeColor = Color.Black;
            TbEmail.Location = new Point(466, 218);
            TbEmail.Multiline = true;
            TbEmail.Name = "TbEmail";
            TbEmail.Size = new Size(374, 24);
            TbEmail.TabIndex = 21;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Location = new Point(466, 244);
            panel1.Name = "panel1";
            panel1.Size = new Size(377, 1);
            panel1.TabIndex = 20;
            // 
            // txtEmail
            // 
            txtEmail.AutoSize = true;
            txtEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(466, 190);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(48, 21);
            txtEmail.TabIndex = 19;
            txtEmail.Text = "Email";
            // 
            // TbAddress
            // 
            TbAddress.BackColor = SystemColors.Control;
            TbAddress.BorderStyle = BorderStyle.None;
            TbAddress.Font = new Font("Microsoft Sans Serif", 9.75F);
            TbAddress.ForeColor = Color.Black;
            TbAddress.Location = new Point(32, 302);
            TbAddress.Multiline = true;
            TbAddress.Name = "TbAddress";
            TbAddress.Size = new Size(808, 24);
            TbAddress.TabIndex = 27;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Black;
            panel4.Location = new Point(32, 328);
            panel4.Name = "panel4";
            panel4.Size = new Size(811, 1);
            panel4.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(35, 274);
            label4.Name = "label4";
            label4.Size = new Size(60, 21);
            label4.TabIndex = 25;
            label4.Text = "Địa Chỉ";
            // 
            // BtnDelete
            // 
            BtnDelete.BackColor = Color.Transparent;
            BtnDelete.EnabledCalc = true;
            BtnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDelete.ForeColor = Color.Black;
            BtnDelete.Location = new Point(32, 386);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(67, 30);
            BtnDelete.TabIndex = 34;
            BtnDelete.Text = "Xóa";
            BtnDelete.Click += BtnDelete_Click;
            // 
            // BtnEdit
            // 
            BtnEdit.BorderColor = Color.FromArgb(220, 223, 230);
            BtnEdit.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            BtnEdit.DangerColor = Color.FromArgb(245, 108, 108);
            BtnEdit.DefaultColor = Color.FromArgb(255, 255, 255);
            BtnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnEdit.HoverTextColor = Color.FromArgb(48, 49, 51);
            BtnEdit.InfoColor = Color.FromArgb(144, 147, 153);
            BtnEdit.Location = new Point(737, 386);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.PrimaryColor = Color.FromArgb(39, 174, 97);
            BtnEdit.Size = new Size(106, 30);
            BtnEdit.SuccessColor = Color.FromArgb(103, 194, 58);
            BtnEdit.TabIndex = 38;
            BtnEdit.Text = "Chỉnh sửa";
            BtnEdit.TextColor = Color.White;
            BtnEdit.WarningColor = Color.FromArgb(230, 162, 60);
            BtnEdit.Click += BtnEdit_Click;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(44, 62, 80);
            panel5.Controls.Add(roundedPictureBox1);
            panel5.Location = new Point(-2, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(871, 38);
            panel5.TabIndex = 40;
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
            roundedPictureBox1.Click += roundedPictureBox1_Click_1;
            // 
            // ID
            // 
            ID.AutoSize = true;
            ID.BackColor = Color.Transparent;
            ID.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ID.ForeColor = Color.FromArgb(80, 80, 80);
            ID.Location = new Point(221, 72);
            ID.Name = "ID";
            ID.Size = new Size(19, 21);
            ID.TabIndex = 42;
            ID.Text = "2";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ActiveBorder;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Location = new Point(187, 104);
            panel3.Name = "panel3";
            panel3.Size = new Size(1, 20);
            panel3.TabIndex = 44;
            // 
            // Ngay
            // 
            Ngay.BackColor = SystemColors.Control;
            Ngay.BorderStyle = BorderStyle.None;
            Ngay.Font = new Font("Microsoft Sans Serif", 9.75F);
            Ngay.ForeColor = Color.Black;
            Ngay.Location = new Point(307, 104);
            Ngay.Multiline = true;
            Ngay.Name = "Ngay";
            Ngay.Size = new Size(149, 24);
            Ngay.TabIndex = 45;
            // 
            // PopNcc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(869, 428);
            Controls.Add(Ngay);
            Controls.Add(panel3);
            Controls.Add(ID);
            Controls.Add(panel5);
            Controls.Add(BtnEdit);
            Controls.Add(BtnDelete);
            Controls.Add(TbAddress);
            Controls.Add(panel4);
            Controls.Add(label4);
            Controls.Add(TbEmail);
            Controls.Add(panel1);
            Controls.Add(txtEmail);
            Controls.Add(TbPhone);
            Controls.Add(panel2);
            Controls.Add(label2);
            Controls.Add(bigLabel2);
            Controls.Add(lblNgTao);
            Controls.Add(bigLabel5);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PopNcc";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Form2";
            Load += PopNcc_Load;
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)roundedPictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private ReaLTaiizor.Controls.BigLabel bigLabel2;
        private ReaLTaiizor.Controls.BigLabel lblNgTao;
        private ReaLTaiizor.Controls.BigLabel bigLabel5;
        private ReaLTaiizor.Controls.BigLabel lblName;
        private TextBox TbPhone;
        private Panel panel2;
        private TextBox TbEmail;
        private Panel panel1;
        private Label txtEmail;
        private TextBox TbAddress;
        private Panel panel4;
        private Label label4;
        private ReaLTaiizor.Controls.AloneButton BtnDelete;
        private ReaLTaiizor.Controls.HopeRoundButton BtnEdit;
        private Panel panel5;
        private RoundedPictureBox roundedPictureBox1;
        private ReaLTaiizor.Controls.BigLabel ID;
        private Panel panel3;
        private TextBox Ngay;
    }
}
