namespace GUI
{
    partial class FrmMaterial
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMaterial));
            dataGridView1 = new ReaLTaiizor.Controls.PoisonDataGridView();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            pictureBox1 = new PictureBox();
            button1 = new ReaLTaiizor.Controls.Button();
            result = new Panel();
            panel1 = new Panel();
            Tong = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Anchor = AnchorStyles.None;
            dataGridView1.BackgroundColor = Color.FromArgb(236, 240, 241);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridView1.GridColor = Color.FromArgb(255, 255, 255);
            dataGridView1.Location = new Point(3, 74);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1100, 463);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellClick;
            dataGridView1.SelectionChanged += dgv_SelectionChanged;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(236, 240, 241);
            txtSearch.BorderColor = Color.FromArgb(180, 180, 180);
            txtSearch.EdgeColor = SystemColors.ActiveBorder;
            txtSearch.Font = new Font("Tahoma", 11F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(23, 31);
            txtSearch.MaxLength = 32767;
            txtSearch.Multiline = false;
            txtSearch.Name = "txtSearch";
            txtSearch.ReadOnly = false;
            txtSearch.Size = new Size(223, 28);
            txtSearch.TabIndex = 2;
            txtSearch.TextAlignment = HorizontalAlignment.Left;
            txtSearch.UseSystemPasswordChar = false;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(252, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(27, 28);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
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
            button1.Location = new Point(928, 31);
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
            // result
            // 
            result.Location = new Point(23, 60);
            result.Name = "result";
            result.Size = new Size(223, 75);
            result.TabIndex = 6;
            result.Visible = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(236, 240, 241);
            panel1.Controls.Add(Tong);
            panel1.Controls.Add(result);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1106, 600);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // Tong
            // 
            Tong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Tong.AutoSize = true;
            Tong.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Tong.ForeColor = Color.DarkGray;
            Tong.Location = new Point(12, 584);
            Tong.Name = "Tong";
            Tong.Size = new Size(33, 13);
            Tong.TabIndex = 8;
            Tong.Text = "Tổng";
            // 
            // FrmMaterial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 600);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmMaterial";
            Text = "FrmMaterial";
            Load += FrmMaterial_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ReaLTaiizor.Controls.PoisonDataGridView DataGridView1;
        private ReaLTaiizor.Controls.PoisonDataGridView dataGridView1;
        private ReaLTaiizor.Controls.DungeonTextBox txtSearch;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.Button button1;
        private Panel result;
        private Panel panel1;
        private Label Tong;
    }
}