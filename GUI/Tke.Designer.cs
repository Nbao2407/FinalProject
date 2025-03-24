
namespace GUI
{
    partial class Tke
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            label1 = new Label();
            dtpStartDate = new DateTimePicker();
            dtpEndDate = new DateTimePicker();
            btnLast30Days = new Button();
            btnLast7Days = new Button();
            btnToday = new Button();
            btnCustomDate = new Button();
            btnOkCustomDate = new Button();
            panel3 = new Panel();
            label11 = new Label();
            lblTotalProfit = new Label();
            label5 = new Label();
            chartGrossRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panel5 = new Panel();
            tableLayoutPanel5 = new TableLayoutPanel();
            dgvUnderstock = new DataGridView();
            label13 = new Label();
            lblStartDate = new Label();
            lblEndDate = new Label();
            label12 = new Label();
            panel2 = new Panel();
            label10 = new Label();
            lblTotalRevenue = new Label();
            label4 = new Label();
            panel1 = new Panel();
            label3 = new Label();
            lblNumOrders = new Label();
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            chartTopProducts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel2 = new TableLayoutPanel();
            panelTotalCounters = new Panel();
            tableLayoutPanel6 = new TableLayoutPanel();
            label21 = new Label();
            label17 = new Label();
            lblNumCustomers = new Label();
            lblNumSuppliers = new Label();
            label19 = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel4 = new Panel();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartGrossRevenue).BeginInit();
            panel5.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartTopProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelTotalCounters.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.WhiteSmoke;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(127, 31);
            label1.TabIndex = 0;
            label1.Text = "Thống kê";
            label1.Click += label1_Click;
            // 
            // dtpStartDate
            // 
            dtpStartDate.Cursor = Cursors.Hand;
            dtpStartDate.CustomFormat = "dd/MM/yyyy";
            dtpStartDate.Enabled = false;
            dtpStartDate.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(183, 7);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(128, 20);
            dtpStartDate.TabIndex = 1;
            dtpStartDate.ValueChanged += dtpStartDate_ValueChanged;
            // 
            // dtpEndDate
            // 
            dtpEndDate.Cursor = Cursors.Hand;
            dtpEndDate.CustomFormat = "dd/MM/yyyy";
            dtpEndDate.Enabled = false;
            dtpEndDate.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(355, 7);
            dtpEndDate.MaxDate = new DateTime(2025, 12, 31, 0, 0, 0, 0);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(128, 20);
            dtpEndDate.TabIndex = 2;
            dtpEndDate.Value = new DateTime(2025, 3, 15, 0, 0, 0, 0);
            dtpEndDate.ValueChanged += dtpEndDate_ValueChanged;
            // 
            // btnLast30Days
            // 
            btnLast30Days.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnLast30Days.FlatStyle = FlatStyle.Flat;
            btnLast30Days.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLast30Days.ForeColor = Color.FromArgb(124, 141, 181);
            btnLast30Days.Location = new Point(963, 0);
            btnLast30Days.Margin = new Padding(5);
            btnLast30Days.Name = "btnLast30Days";
            btnLast30Days.Size = new Size(130, 35);
            btnLast30Days.TabIndex = 4;
            btnLast30Days.Text = " 30 ngày trước";
            btnLast30Days.UseVisualStyleBackColor = true;
            btnLast30Days.Click += btnLast30Days_Click;
            // 
            // btnLast7Days
            // 
            btnLast7Days.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnLast7Days.FlatStyle = FlatStyle.Flat;
            btnLast7Days.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLast7Days.ForeColor = Color.FromArgb(124, 141, 181);
            btnLast7Days.Location = new Point(834, 0);
            btnLast7Days.Margin = new Padding(5);
            btnLast7Days.Name = "btnLast7Days";
            btnLast7Days.Size = new Size(130, 35);
            btnLast7Days.TabIndex = 5;
            btnLast7Days.Text = "7 ngày trước";
            btnLast7Days.UseVisualStyleBackColor = true;
            btnLast7Days.Click += btnLast7Days_Click;
            // 
            // btnToday
            // 
            btnToday.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnToday.ForeColor = Color.FromArgb(124, 141, 181);
            btnToday.Location = new Point(705, 0);
            btnToday.Margin = new Padding(5);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(130, 35);
            btnToday.TabIndex = 6;
            btnToday.Text = "Hôm nay";
            btnToday.UseVisualStyleBackColor = true;
            btnToday.Click += btnToday_Click;
            // 
            // btnCustomDate
            // 
            btnCustomDate.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnCustomDate.FlatStyle = FlatStyle.Flat;
            btnCustomDate.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCustomDate.ForeColor = Color.FromArgb(124, 141, 181);
            btnCustomDate.Location = new Point(576, 0);
            btnCustomDate.Margin = new Padding(5);
            btnCustomDate.Name = "btnCustomDate";
            btnCustomDate.Size = new Size(130, 35);
            btnCustomDate.TabIndex = 7;
            btnCustomDate.Text = "Tùy chỉnh";
            btnCustomDate.UseVisualStyleBackColor = true;
            btnCustomDate.Click += btnCustomDate_Click;
            // 
            // btnOkCustomDate
            // 
            btnOkCustomDate.BackColor = Color.FromArgb(241, 88, 127);
            btnOkCustomDate.FlatAppearance.BorderSize = 0;
            btnOkCustomDate.FlatStyle = FlatStyle.Flat;
            btnOkCustomDate.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnOkCustomDate.ForeColor = Color.White;
            btnOkCustomDate.Location = new Point(537, 0);
            btnOkCustomDate.Margin = new Padding(5);
            btnOkCustomDate.Name = "btnOkCustomDate";
            btnOkCustomDate.Size = new Size(39, 35);
            btnOkCustomDate.TabIndex = 8;
            btnOkCustomDate.Text = "✔️";
            btnOkCustomDate.UseVisualStyleBackColor = false;
            btnOkCustomDate.Visible = false;
            btnOkCustomDate.Click += btnOkCustomDate_Click;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.BackColor = Color.FromArgb(42, 45, 86);
            panel3.Controls.Add(label11);
            panel3.Controls.Add(lblTotalProfit);
            panel3.Controls.Add(label5);
            panel3.Location = new Point(754, 5);
            panel3.Margin = new Padding(5);
            panel3.Name = "panel3";
            panel3.Size = new Size(347, 73);
            panel3.TabIndex = 11;
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.FromArgb(128, 128, 255);
            label11.Location = new Point(282, 27);
            label11.Name = "label11";
            label11.Size = new Size(50, 20);
            label11.TabIndex = 3;
            label11.Text = "+19%";
            // 
            // lblTotalProfit
            // 
            lblTotalProfit.AutoSize = true;
            lblTotalProfit.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalProfit.ForeColor = Color.WhiteSmoke;
            lblTotalProfit.Location = new Point(28, 34);
            lblTotalProfit.Name = "lblTotalProfit";
            lblTotalProfit.Size = new Size(78, 25);
            lblTotalProfit.TabIndex = 1;
            lblTotalProfit.Text = "100000";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(124, 141, 181);
            label5.Location = new Point(29, 14);
            label5.Name = "label5";
            label5.Size = new Size(113, 20);
            label5.TabIndex = 0;
            label5.Text = "Tổng lợi nhuận";
            // 
            // chartGrossRevenue
            // 
            chartGrossRevenue.BackColor = Color.FromArgb(52, 73, 94);
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.LabelStyle.ForeColor = Color.Silver;
            chartArea1.AxisX.LineColor = Color.White;
            chartArea1.AxisX.LineWidth = 0;
            chartArea1.AxisX.MajorGrid.LineColor = Color.White;
            chartArea1.AxisX.MajorGrid.LineWidth = 0;
            chartArea1.AxisX.MajorTickMark.LineColor = Color.FromArgb(73, 75, 111);
            chartArea1.AxisX.MajorTickMark.Size = 3F;
            chartArea1.AxisY.LabelStyle.ForeColor = Color.Silver;
            chartArea1.AxisY.LabelStyle.Format = "${0:0,}K";
            chartArea1.AxisY.LineColor = Color.White;
            chartArea1.AxisY.LineWidth = 0;
            chartArea1.AxisY.MajorGrid.LineColor = Color.FromArgb(73, 75, 111);
            chartArea1.AxisY.MajorTickMark.LineColor = Color.FromArgb(73, 75, 111);
            chartArea1.AxisY.MajorTickMark.LineWidth = 0;
            chartArea1.BackColor = Color.FromArgb(52, 73, 94);
            chartArea1.BackSecondaryColor = Color.FromArgb(52, 73, 94);
            chartArea1.BorderColor = Color.White;
            chartArea1.Name = "ChartArea1";
            chartGrossRevenue.ChartAreas.Add(chartArea1);
            chartGrossRevenue.Dock = DockStyle.Fill;
            chartGrossRevenue.Enabled = false;
            legend1.BackColor = Color.FromArgb(42, 45, 86);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new Font("Microsoft Sans Serif", 9F);
            legend1.ForeColor = Color.FromArgb(192, 192, 255);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            chartGrossRevenue.Legends.Add(legend1);
            chartGrossRevenue.Location = new Point(5, 5);
            chartGrossRevenue.Margin = new Padding(5);
            chartGrossRevenue.Name = "chartGrossRevenue";
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            series1.BackSecondaryColor = Color.FromArgb(107, 83, 255);
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series1.Color = Color.FromArgb(241, 88, 127);
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = Color.MediumPurple;
            series1.MarkerSize = 10;
            series1.Name = "Series1";
            chartGrossRevenue.Series.Add(series1);
            chartGrossRevenue.Size = new Size(737, 202);
            chartGrossRevenue.TabIndex = 12;
            chartGrossRevenue.Text = "chartGrossRevenue";
            title1.Alignment = ContentAlignment.TopLeft;
            title1.Font = new Font("Microsoft Sans Serif", 15F);
            title1.ForeColor = Color.WhiteSmoke;
            title1.Name = "Title1";
            title1.Text = "Doanh thu (theo ngày)";
            chartGrossRevenue.Titles.Add(title1);
            chartGrossRevenue.Click += chartGrossRevenue_Click;
            // 
            // panel5
            // 
            panel5.BackColor = Color.Black;
            panel5.Controls.Add(tableLayoutPanel5);
            panel5.Dock = DockStyle.Fill;
            panel5.Font = new Font("Microsoft Sans Serif", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel5.Location = new Point(238, 5);
            panel5.Margin = new Padding(5);
            panel5.Name = "panel5";
            panel5.Size = new Size(504, 271);
            panel5.TabIndex = 11;
            panel5.Paint += panel5_Paint;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(dgvUnderstock, 0, 1);
            tableLayoutPanel5.Controls.Add(label13, 0, 0);
            tableLayoutPanel5.Location = new Point(3, 10);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 13.7795277F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 86.2204742F));
            tableLayoutPanel5.Size = new Size(496, 254);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // dgvUnderstock
            // 
            dgvUnderstock.AllowUserToAddRows = false;
            dgvUnderstock.AllowUserToDeleteRows = false;
            dgvUnderstock.AllowUserToResizeColumns = false;
            dgvUnderstock.AllowUserToResizeRows = false;
            dgvUnderstock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.BackgroundColor = Color.FromArgb(52, 73, 94);
            dgvUnderstock.BorderStyle = BorderStyle.None;
            dgvUnderstock.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dgvUnderstock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUnderstock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUnderstock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.Transparent;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = Color.Transparent;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUnderstock.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUnderstock.EnableHeadersVisualStyles = false;
            dgvUnderstock.GridColor = Color.FromArgb(80, 85, 120);
            dgvUnderstock.Location = new Point(3, 38);
            dgvUnderstock.Name = "dgvUnderstock";
            dgvUnderstock.ReadOnly = true;
            dgvUnderstock.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(52, 73, 94);
            dgvUnderstock.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvUnderstock.RowTemplate.Height = 35;
            dgvUnderstock.Size = new Size(490, 213);
            dgvUnderstock.TabIndex = 3;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.ForeColor = Color.WhiteSmoke;
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(171, 25);
            label13.TabIndex = 2;
            label13.Text = "Sản phẩm sắp hết";
            // 
            // lblStartDate
            // 
            lblStartDate.AutoSize = true;
            lblStartDate.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStartDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblStartDate.Location = new Point(181, 7);
            lblStartDate.MinimumSize = new Size(130, 0);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(130, 20);
            lblStartDate.TabIndex = 4;
            lblStartDate.Text = "22/3/2025";
            lblStartDate.TextAlign = ContentAlignment.MiddleRight;
            lblStartDate.Click += lblStartDate_Click;
            // 
            // lblEndDate
            // 
            lblEndDate.AutoSize = true;
            lblEndDate.Cursor = Cursors.Hand;
            lblEndDate.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEndDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblEndDate.Location = new Point(353, 7);
            lblEndDate.MinimumSize = new Size(130, 0);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(130, 20);
            lblEndDate.TabIndex = 14;
            lblEndDate.Text = "22/3/2025";
            lblEndDate.TextAlign = ContentAlignment.MiddleLeft;
            lblEndDate.Click += lblEndDate_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.FromArgb(124, 141, 181);
            label12.Location = new Point(327, 8);
            label12.Name = "label12";
            label12.Size = new Size(14, 20);
            label12.TabIndex = 15;
            label12.Text = "-";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(42, 45, 86);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(lblTotalRevenue);
            panel2.Controls.Add(label4);
            panel2.Location = new Point(310, 5);
            panel2.Margin = new Padding(5);
            panel2.Name = "panel2";
            panel2.Size = new Size(434, 74);
            panel2.TabIndex = 10;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.FromArgb(255, 128, 128);
            label10.Location = new Point(353, 27);
            label10.Name = "label10";
            label10.Size = new Size(50, 20);
            label10.TabIndex = 3;
            label10.Text = "+21%";
            // 
            // lblTotalRevenue
            // 
            lblTotalRevenue.AutoSize = true;
            lblTotalRevenue.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalRevenue.ForeColor = Color.WhiteSmoke;
            lblTotalRevenue.Location = new Point(74, 34);
            lblTotalRevenue.Name = "lblTotalRevenue";
            lblTotalRevenue.Size = new Size(78, 25);
            lblTotalRevenue.TabIndex = 1;
            lblTotalRevenue.Text = "100000";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(124, 141, 181);
            label4.Location = new Point(75, 14);
            label4.Name = "label4";
            label4.Size = new Size(121, 20);
            label4.TabIndex = 0;
            label4.Text = "Tổng doanh thu";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.FromArgb(42, 45, 86);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(lblNumOrders);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(5, 5);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(295, 73);
            panel1.TabIndex = 9;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 192, 192);
            label3.Location = new Point(230, 27);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 2;
            label3.Text = "+15%";
            // 
            // lblNumOrders
            // 
            lblNumOrders.AutoSize = true;
            lblNumOrders.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNumOrders.ForeColor = Color.WhiteSmoke;
            lblNumOrders.Location = new Point(29, 34);
            lblNumOrders.Name = "lblNumOrders";
            lblNumOrders.Size = new Size(78, 25);
            lblNumOrders.TabIndex = 1;
            lblNumOrders.Text = "100000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(124, 141, 181);
            label2.Location = new Point(30, 14);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 0;
            label2.Text = "Số Hóa Đơn";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.6083031F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.23508F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.1880646F));
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 2, 0);
            tableLayoutPanel1.Location = new Point(0, 45);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1106, 84);
            tableLayoutPanel1.TabIndex = 16;
            // 
            // chartTopProducts
            // 
            chartTopProducts.BackColor = Color.FromArgb(42, 45, 86);
            chartArea2.BackColor = Color.FromArgb(42, 45, 86);
            chartArea2.Name = "ChartArea1";
            chartTopProducts.ChartAreas.Add(chartArea2);
            chartTopProducts.Dock = DockStyle.Fill;
            legend2.BackColor = Color.FromArgb(42, 45, 86);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Font = new Font("Microsoft Sans Serif", 10F);
            legend2.ForeColor = Color.Gainsboro;
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            chartTopProducts.Legends.Add(legend2);
            chartTopProducts.Location = new Point(5, 5);
            chartTopProducts.Margin = new Padding(5);
            chartTopProducts.Name = "chartTopProducts";
            chartTopProducts.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            chartTopProducts.PaletteCustomColors = new Color[]
    {
    Color.FromArgb(241, 160, 139),
    Color.FromArgb(239, 188, 0),
    Color.FromArgb(241, 88, 127),
    Color.FromArgb(1, 220, 205),
    Color.FromArgb(107, 83, 255),
    Color.FromArgb(94, 153, 254)
    };
            series2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalRight;
            series2.BackSecondaryColor = Color.FromArgb(255, 159, 255);
            series2.BorderColor = Color.FromArgb(42, 45, 86);
            series2.BorderWidth = 8;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series2.Color = Color.FromArgb(192, 192, 255);
            series2.Font = new Font("Microsoft Sans Serif", 12F);
            series2.IsValueShownAsLabel = true;
            series2.LabelForeColor = Color.White;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chartTopProducts.Series.Add(series2);
            chartTopProducts.Size = new Size(345, 486);
            chartTopProducts.TabIndex = 13;
            chartTopProducts.Text = "chartTopProducts";
            title2.Alignment = ContentAlignment.TopLeft;
            title2.Font = new Font("Microsoft Sans Serif", 15F);
            title2.ForeColor = Color.WhiteSmoke;
            title2.Name = "Title1";
            title2.Text = "Top 5 Sản phẩm bán chạy nhất";
            chartTopProducts.Titles.Add(title2);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 126);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel2);
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel4);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel3);
            splitContainer1.Size = new Size(1106, 496);
            splitContainer1.SplitterDistance = 747;
            splitContainer1.TabIndex = 18;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 514F));
            tableLayoutPanel2.Controls.Add(panel5, 1, 0);
            tableLayoutPanel2.Controls.Add(panelTotalCounters, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 215);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(747, 281);
            tableLayoutPanel2.TabIndex = 14;
            // 
            // panelTotalCounters
            // 
            panelTotalCounters.BackColor = Color.FromArgb(52, 73, 94);
            panelTotalCounters.Controls.Add(tableLayoutPanel6);
            panelTotalCounters.Controls.Add(label19);
            panelTotalCounters.Dock = DockStyle.Fill;
            panelTotalCounters.Location = new Point(5, 5);
            panelTotalCounters.Margin = new Padding(5);
            panelTotalCounters.MinimumSize = new Size(0, 200);
            panelTotalCounters.Name = "panelTotalCounters";
            panelTotalCounters.Size = new Size(223, 271);
            panelTotalCounters.TabIndex = 14;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(label21, 0, 0);
            tableLayoutPanel6.Controls.Add(label17, 0, 2);
            tableLayoutPanel6.Controls.Add(lblNumCustomers, 0, 1);
            tableLayoutPanel6.Controls.Add(lblNumSuppliers, 0, 3);
            tableLayoutPanel6.Location = new Point(41, 40);
            tableLayoutPanel6.Margin = new Padding(3, 15, 3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 5;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50.7042236F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 49.2957764F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tableLayoutPanel6.Size = new Size(124, 174);
            tableLayoutPanel6.TabIndex = 5;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Dock = DockStyle.Left;
            label21.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label21.ForeColor = Color.FromArgb(124, 141, 181);
            label21.Location = new Point(3, 0);
            label21.Name = "label21";
            label21.Size = new Size(115, 33);
            label21.TabIndex = 0;
            label21.Text = "Khách Hàng";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Dock = DockStyle.Left;
            label17.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label17.ForeColor = Color.FromArgb(124, 141, 181);
            label17.ImageAlign = ContentAlignment.MiddleLeft;
            label17.Location = new Point(3, 66);
            label17.Name = "label17";
            label17.Size = new Size(101, 34);
            label17.TabIndex = 3;
            label17.Text = "Nhà Cung cấp";
            // 
            // lblNumCustomers
            // 
            lblNumCustomers.AutoSize = true;
            lblNumCustomers.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumCustomers.ForeColor = Color.WhiteSmoke;
            lblNumCustomers.Location = new Point(3, 33);
            lblNumCustomers.Name = "lblNumCustomers";
            lblNumCustomers.RightToLeft = RightToLeft.No;
            lblNumCustomers.Size = new Size(90, 25);
            lblNumCustomers.TabIndex = 1;
            lblNumCustomers.Text = "100000";
            lblNumCustomers.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNumSuppliers
            // 
            lblNumSuppliers.AutoSize = true;
            lblNumSuppliers.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumSuppliers.ForeColor = Color.WhiteSmoke;
            lblNumSuppliers.Location = new Point(3, 100);
            lblNumSuppliers.Name = "lblNumSuppliers";
            lblNumSuppliers.RightToLeft = RightToLeft.No;
            lblNumSuppliers.Size = new Size(90, 25);
            lblNumSuppliers.TabIndex = 5;
            lblNumSuppliers.Text = "100000";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Dock = DockStyle.Left;
            label19.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label19.ForeColor = Color.WhiteSmoke;
            label19.Location = new Point(0, 0);
            label19.Name = "label19";
            label19.Size = new Size(63, 25);
            label19.TabIndex = 2;
            label19.Text = "Tổng ";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(chartGrossRevenue, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Top;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(747, 212);
            tableLayoutPanel4.TabIndex = 15;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(chartTopProducts, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(355, 496);
            tableLayoutPanel3.TabIndex = 14;
            // 
            // panel4
            // 
            panel4.Controls.Add(tableLayoutPanel1);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(btnOkCustomDate);
            panel4.Controls.Add(label12);
            panel4.Controls.Add(btnLast30Days);
            panel4.Controls.Add(btnLast7Days);
            panel4.Controls.Add(btnToday);
            panel4.Controls.Add(btnCustomDate);
            panel4.Controls.Add(lblStartDate);
            panel4.Controls.Add(dtpStartDate);
            panel4.Controls.Add(lblEndDate);
            panel4.Controls.Add(dtpEndDate);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1106, 126);
            panel4.TabIndex = 19;
            // 
            // Tke
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(1106, 622);
            Controls.Add(splitContainer1);
            Controls.Add(panel4);
            Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "Tke";
            Load += Form1_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chartGrossRevenue).EndInit();
            panel5.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartTopProducts).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panelTotalCounters.ResumeLayout(false);
            panelTotalCounters.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnLast30Days;
        private System.Windows.Forms.Button btnLast7Days;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnCustomDate;
        private System.Windows.Forms.Button btnOkCustomDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTotalProfit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGrossRevenue;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label label12;
        private Panel panel2;
        private Label label10;
        private Label lblTotalRevenue;
        private Label label4;
        private Panel panel1;
        private Label label3;
        private Label lblNumOrders;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dgvUnderstock;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopProducts;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panelTotalCounters;
        private Label label17;
        private Label label19;
        private Label label21;
        private Label lblNumCustomers;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private Label lblNumSuppliers;
    }
}