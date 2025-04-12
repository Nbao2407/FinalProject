namespace QLVT
{
    partial class FrmXuat
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Dispose the timer
                if (debounceTimer != null)
                {
                    debounceTimer.Dispose();
                    debounceTimer = null;
                }
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
            button2 = new ReaLTaiizor.Controls.Button();
            result = new Panel();
            txtSearch = new ReaLTaiizor.Controls.DungeonTextBox();
            dgvDonXuat = new ReaLTaiizor.Controls.PoisonDataGridView();
            cboTrangThaiFilter = new ReaLTaiizor.Controls.AloneComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvDonXuat).BeginInit();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.BackColor = Color.Transparent;
            button2.BorderColor = Color.FromArgb(52, 73, 94);
            button2.Cursor = Cursors.Hand;
            button2.EnteredBorderColor = Color.FromArgb(165, 37, 37);
            button2.EnteredColor = Color.FromArgb(32, 34, 37);
            button2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Image = null;
            button2.ImageAlign = ContentAlignment.MiddleRight;
            button2.InactiveColor = Color.FromArgb(30, 58, 138);
            button2.Location = new Point(932, 27);
            button2.Margin = new Padding(3, 3, 160, 3);
            button2.Name = "button2";
            button2.Padding = new Padding(0, 0, 100, 0);
            button2.PressedBorderColor = Color.Green;
            button2.PressedColor = Color.Green;
            button2.RightToLeft = RightToLeft.Yes;
            button2.Size = new Size(129, 28);
            button2.TabIndex = 17;
            button2.Text = "Thêm";
            button2.TextAlignment = StringAlignment.Center;
            button2.Click += button2_Click;
            // 
            // result
            // 
            result.BackColor = Color.White;
            result.BorderStyle = BorderStyle.FixedSingle;
            result.Location = new Point(23, 56);
            result.Name = "result";
            result.Size = new Size(223, 45);
            result.TabIndex = 16;
            result.Visible = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(236, 240, 241);
            txtSearch.BorderColor = Color.FromArgb(180, 180, 180);
            txtSearch.EdgeColor = SystemColors.ActiveBorder;
            txtSearch.Font = new Font("Tahoma", 11F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(23, 27);
            txtSearch.MaxLength = 32767;
            txtSearch.Multiline = false;
            txtSearch.Name = "txtSearch";
            txtSearch.ReadOnly = false;
            txtSearch.Size = new Size(223, 28);
            txtSearch.TabIndex = 15;
            txtSearch.TextAlignment = HorizontalAlignment.Left;
            txtSearch.UseSystemPasswordChar = false;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvDonXuat
            // 
            dgvDonXuat.AllowUserToAddRows = false;
            dgvDonXuat.AllowUserToDeleteRows = false;
            dgvDonXuat.AllowUserToResizeColumns = false;
            dgvDonXuat.AllowUserToResizeRows = false;
            dgvDonXuat.Anchor = AnchorStyles.None;
            dgvDonXuat.BackgroundColor = Color.FromArgb(224, 224, 224);
            dgvDonXuat.BorderStyle = BorderStyle.None;
            dgvDonXuat.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvDonXuat.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDonXuat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDonXuat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDonXuat.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDonXuat.EnableHeadersVisualStyles = false;
            dgvDonXuat.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dgvDonXuat.GridColor = Color.FromArgb(255, 255, 255);
            dgvDonXuat.Location = new Point(3, 90);
            dgvDonXuat.Name = "dgvDonXuat";
            dgvDonXuat.ReadOnly = true;
            dgvDonXuat.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDonXuat.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDonXuat.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvDonXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDonXuat.Size = new Size(1104, 463);
            dgvDonXuat.TabIndex = 14;
            dgvDonXuat.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // cboTrangThaiFilter
            // 
            cboTrangThaiFilter.DrawMode = DrawMode.OwnerDrawFixed;
            cboTrangThaiFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThaiFilter.EnabledCalc = true;
            cboTrangThaiFilter.FormattingEnabled = true;
            cboTrangThaiFilter.ItemHeight = 20;
            cboTrangThaiFilter.Location = new Point(252, 29);
            cboTrangThaiFilter.Name = "cboTrangThaiFilter";
            cboTrangThaiFilter.Size = new Size(204, 26);
            cboTrangThaiFilter.TabIndex = 18;
            cboTrangThaiFilter.SelectedIndexChanged += cboTrangThai_SelectedIndexChanged;
            // 
            // FrmXuat
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 600);
            Controls.Add(cboTrangThaiFilter);
            Controls.Add(button2);
            Controls.Add(result);
            Controls.Add(txtSearch);
            Controls.Add(dgvDonXuat);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmXuat";
            Text = "FrmOrder";
            ((System.ComponentModel.ISupportInitialize)dgvDonXuat).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.Button button2;
        private Panel result;
        private ReaLTaiizor.Controls.DungeonTextBox txtSearch;
        private ReaLTaiizor.Controls.PoisonDataGridView dgvDonXuat;
        private ReaLTaiizor.Controls.AloneComboBox cboTrangThaiFilter;
    }
}