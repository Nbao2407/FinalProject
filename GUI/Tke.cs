using BUS;
using QLVT.Helper;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLVT
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
            ChartHelper.SetupTop5Chart(chartTopProducts, "Top 5 Vật Liệu Bán Chạy");
            SetDataMenuButtonsUI(btnLast7Days);
         
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
                    lblNumOrders.Text = danhSach.Count(o => o.TrangThai == "Đã thanh toán").ToString();
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
            UIHelper.SetupDashboardUI(this, dgvUnderstock,
                tableLayoutPanel4, tableLayoutPanel2,
                panel1, panel2, panel3, panel5,
                btnToday, btnLast7Days, btnLast30Days, btnCustomDate, btnOkCustomDate,
                label1, lblNumOrders, lblTotalRevenue, lblTotalProfit, lblNumCustomers, lblNumSuppliers,
                label2, label4, label5, lblStartDate, lblEndDate,
                dtpStartDate, dtpEndDate);
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