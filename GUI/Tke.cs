using BUS;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class Tke : Form
    {
        private BUS_TKe thongKeBUS;
        private Button currentButton;

        public Tke()
        {
            InitializeComponent();
            thongKeBUS = new BUS_TKe();
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;
            btnLast7Days.Select();
            ChartHelper.SetupSplineAreaChart(chartGrossRevenue, "Tổng Doanh Thu");
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableLayoutPanel4.Size = new Size(0, 0);
            tableLayoutPanel2.Size = new Size(0, 0);
            SetDataMenuButtonsUI(btnLast7Days);
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            BackColor = Color.FromArgb(44, 62, 80);
            dgvUnderstock.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            panel1.BackColor = Color.FromArgb(52, 73, 94);
            panel2.BackColor = Color.FromArgb(52, 73, 94);
            panel3.BackColor = Color.FromArgb(52, 73, 94);
            panel5.BackColor = Color.FromArgb(52, 73, 94);
            btnToday.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
            btnLast7Days.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
            btnLast30Days.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
            btnCustomDate.FlatAppearance.BorderColor = Color.FromArgb(52, 152, 219);
            btnOkCustomDate.BackColor = Color.FromArgb(52, 152, 219);

            chartTopProducts.Series.Clear();
            chartTopProducts.ChartAreas.Clear();
            chartTopProducts.Legends.Clear();
            chartTopProducts.Titles.Clear();

            ChartArea chartArea2 = new ChartArea
            {
                Name = "ChartArea1",
                BackColor = Color.FromArgb(52, 73, 94)
            };
            chartTopProducts.ChartAreas.Add(chartArea2);

            Legend legend2 = new Legend
            {
                Name = "Legend1",
                BackColor = Color.FromArgb(52, 73, 94),
                Docking = Docking.Bottom,
                Font = new Font("Microsoft Sans Serif", 10F),
                ForeColor = Color.FromArgb(189, 195, 199),
                IsTextAutoFit = false
            };
            chartTopProducts.Legends.Add(legend2);

            Series series2 = new Series
            {
                Name = "TopProducts",
                ChartArea = "ChartArea1",
                Legend = "Legend1",
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut,
                BackSecondaryColor = Color.FromArgb(255, 159, 255),
                BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalRight,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                LegendText = "#LEGENDTEXT"
            };
            series2["DoughnutHoleSize"] = "40";
            series2.BorderColor = Color.FromArgb(52, 73, 94);
            series2.BorderWidth = 8;
            chartTopProducts.Series.Add(series2);
            chartTopProducts.Palette = ChartColorPalette.None;
            chartTopProducts.PaletteCustomColors = new Color[]
            {
    Color.FromArgb(239, 188, 0),
    Color.FromArgb(241, 88, 127),
    Color.FromArgb(1, 220, 205),
    Color.FromArgb(107, 83, 255),
    Color.FromArgb(94, 153, 254)
            };

            Title title2 = new Title
            {
                Name = "Title1",
                Text = "5 Best Selling Products",
                Alignment = ContentAlignment.TopLeft,
                Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke
            };
            chartTopProducts.Titles.Add(title2);
            panelTotalCounters.Width = this.ClientSize.Width / 3;
            panelTotalCounters.Height = this.ClientSize.Height - 50;
            chartTopProducts.BackColor = Color.FromArgb(52, 73, 94);
            chartTopProducts.Location = new Point(751, 138);
            chartTopProducts.Size = new Size(351, 415);
            panelTotalCounters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            label1.ForeColor = Color.WhiteSmoke;
            lblNumOrders.ForeColor = Color.WhiteSmoke;
            lblTotalRevenue.ForeColor = Color.WhiteSmoke;
            lblTotalProfit.ForeColor = Color.WhiteSmoke;
            lblNumCustomers.ForeColor = Color.WhiteSmoke;
            lblNumSuppliers.ForeColor = Color.WhiteSmoke;

            label2.ForeColor = Color.FromArgb(189, 195, 199);
            label4.ForeColor = Color.FromArgb(189, 195, 199);
            label5.ForeColor = Color.FromArgb(189, 195, 199);
            lblStartDate.ForeColor = Color.FromArgb(189, 195, 199);
            lblEndDate.ForeColor = Color.FromArgb(189, 195, 199);
            dgvUnderstock.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvUnderstock.GridColor = Color.Gray;
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.EnableHeadersVisualStyles = false;
            dgvUnderstock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUnderstock.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Viền ngang
            dgvUnderstock.DefaultCellStyle.SelectionBackColor = dgvUnderstock.DefaultCellStyle.BackColor;
            dgvUnderstock.DefaultCellStyle.SelectionForeColor = dgvUnderstock.DefaultCellStyle.ForeColor;
            AdjustDataGridViewColumns();
            this.Resize += new EventHandler(Tke_Resize);
        }

        private void LoadData()
        {
            try
            {
                var danhSach = thongKeBUS.GetThongKeDoanhThu(dtpStartDate.Value, dtpEndDate.Value);
                if (danhSach != null && danhSach.Count > 0)
                {
                    lblNumOrders.Text = danhSach.Count.ToString();
                    lblTotalRevenue.Text = thongKeBUS.TinhTongDoanhThu(dtpStartDate.Value, dtpEndDate.Value).ToString("N0") + " VNĐ";
                    lblTotalProfit.Text = (thongKeBUS.TinhTongDoanhThu(dtpStartDate.Value, dtpEndDate.Value) * 0.2m).ToString("N0") + " VNĐ";
                    lblNumCustomers.Text = danhSach.Select(x => x.KhachHang).Distinct().Count().ToString();

                    var grossRevenueData = danhSach.GroupBy(x => x.NgayLap.Date)
                        .Select(g => new { Date = g.Key, TotalAmount = g.Sum(x => x.TongTien) / 1000 }).ToList();
                    chartGrossRevenue.DataSource = grossRevenueData;
                    chartGrossRevenue.Series["DoanhThu"].XValueMember = "Date";
                    chartGrossRevenue.Series["DoanhThu"].YValueMembers = "TotalAmount";
                    chartGrossRevenue.DataBind();
                }
                else
                {
                    chartGrossRevenue.Series["DoanhThu"].Points.Clear();
                    chartGrossRevenue.Series["DoanhThu"].Points.AddXY("Không có dữ liệu", 0);
                    chartGrossRevenue.Series["DoanhThu"].Points[0].Label = "Không có dữ liệu";
                    chartGrossRevenue.Series["DoanhThu"].Points[0].LabelForeColor = Color.White;
                }

                var topProducts = thongKeBUS.GetTopProducts(dtpStartDate.Value, dtpEndDate.Value);
                if (topProducts != null && topProducts.Count > 0)
                {
                    chartTopProducts.Series["TopProducts"].Points.Clear();
                    foreach (var product in topProducts)
                    {
                        DataPoint point = new DataPoint();
                        point.SetValueXY(product.TenVatLieu, product.SoLuongBan);
                        point.LegendText = product.TenVatLieu;
                        chartTopProducts.Series["TopProducts"].Points.Add(point);
                    }
                }
                else
                {
                    chartTopProducts.Series["TopProducts"].Points.Clear();
                    chartTopProducts.Series["TopProducts"].Points.AddXY("No Data", 0);
                }

                var understock = thongKeBUS.GetUnderstock();
                dgvUnderstock.DataSource = understock;
                if (understock != null && understock.Count > 0)
                {
                    dgvUnderstock.Columns[0].HeaderText = "Tên Vật Liệu";
                    dgvUnderstock.Columns[1].HeaderText = "Số Lượng Tồn";
                }

                lblNumSuppliers.Text = thongKeBUS.GetSoNhaCungCap().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void SetDataMenuButtonsUI(object button)
        {
            var btn = (Button)button;
            btn.BackColor = Color.FromArgb(52, 152, 219);
            btn.ForeColor = Color.White;
            if (currentButton != null && currentButton != btn)
            {
                currentButton.BackColor = this.BackColor;
                currentButton.ForeColor = Color.FromArgb(189, 195, 199);
            }
            currentButton = btn;

            if (btn == btnCustomDate)
            {
                dtpStartDate.Enabled = true;
                dtpEndDate.Enabled = true;
                btnOkCustomDate.Visible = true;
                lblStartDate.Cursor = Cursors.Hand;
                lblEndDate.Cursor = Cursors.Hand;
            }
            else
            {
                dtpStartDate.Enabled = false;
                dtpEndDate.Enabled = false;
                btnOkCustomDate.Visible = false;
                lblStartDate.Cursor = Cursors.Default;
                lblEndDate.Cursor = Cursors.Default;
            }
        }

        private void Tke_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            splitContainer1.SplitterDistance = (int)(this.ClientSize.Width * 0.68);

            int panel1Height = splitContainer1.Panel1.ClientSize.Height;
            tableLayoutPanel4.Height = (int)(panel1Height * 0.6);
            tableLayoutPanel2.Height = (int)(panel1Height * 0.4);
            panelTotalCounters.Dock = DockStyle.Fill;
            panel5.Dock = DockStyle.Fill;
            dgvUnderstock.Dock = DockStyle.Fill;
            tableLayoutPanel2.ColumnStyles.Clear();
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            dgvUnderstock.Dock = DockStyle.Fill;
            dgvUnderstock.ColumnHeadersVisible = true;
            tableLayoutPanel5.Dock = DockStyle.Top;
        }

        private void AdjustDataGridViewColumns()
        {
            if (dgvUnderstock.Columns.Count > 0)
            {
                int totalWidth = dgvUnderstock.ClientSize.Width;

                dgvUnderstock.Columns["MaterialName"].Width = (int)(totalWidth * 0.6);

                dgvUnderstock.Columns["Quantity"].Width = (int)(totalWidth * 0.4);
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        { dtpStartDate.Value = DateTime.Today; dtpEndDate.Value = DateTime.Now; LoadData(); SetDataMenuButtonsUI(sender); }

        private void btnLast7Days_Click(object sender, EventArgs e)
        { dtpStartDate.Value = DateTime.Today.AddDays(-7); dtpEndDate.Value = DateTime.Now; LoadData(); SetDataMenuButtonsUI(sender); }

        private void btnLast30Days_Click(object sender, EventArgs e)
        { dtpStartDate.Value = DateTime.Today.AddDays(-30); dtpEndDate.Value = DateTime.Now; LoadData(); SetDataMenuButtonsUI(sender); }

        private void btnThisMonth_Click(object sender, EventArgs e)
        { dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); dtpEndDate.Value = DateTime.Now; LoadData(); SetDataMenuButtonsUI(sender); }

        private void btnCustomDate_Click(object sender, EventArgs e)
        { SetDataMenuButtonsUI(sender); }

        private void btnOkCustomDate_Click(object sender, EventArgs e)
        { LoadData(); }

        private void lblStartDate_Click(object sender, EventArgs e)
        { if (currentButton == btnCustomDate) { dtpStartDate.Select(); SendKeys.Send("%{DOWN}"); } }

        private void lblEndDate_Click(object sender, EventArgs e)
        { if (currentButton == btnCustomDate) { dtpEndDate.Select(); SendKeys.Send("%{DOWN}"); } }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        { lblStartDate.Text = dtpStartDate.Text; }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        { lblEndDate.Text = dtpEndDate.Text; }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblStartDate.Text = dtpStartDate.Text;
            lblEndDate.Text = dtpEndDate.Text;
            LoadData();
        }

        private void chartGrossRevenue_Click(object sender, EventArgs e)
        {
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}