using System.Windows.Forms.DataVisualization.Charting;

public class ChartHelper
{
    public static void SetupSplineAreaChart(Chart chart, string titleText = "Gross Revenue")
    {
        chart.BackColor = Color.FromArgb(52, 73, 94);
        chart.ChartAreas.Clear();
        chart.Series.Clear();
        chart.Legends.Clear();
        chart.Titles.Clear();

        ChartArea chartArea = new ChartArea
        {
            Name = "ChartArea1",
            BackColor = Color.FromArgb(52, 73, 94)
        };
        chart.ChartAreas.Add(chartArea);
        chartArea.AxisY.LabelStyle.Format = "#,##0 VNĐ"; // Hiển thị dấu phân cách và "VNĐ"
        chartArea.AxisY.LabelStyle.ForeColor = Color.White; // Màu chữ trục

        Legend legend = new Legend
        {
            Name = "Legend1",
            BackColor = Color.FromArgb(52, 73, 94),
            Docking = Docking.Top,
            Font = new Font("Microsoft Sans Serif", 9F),
            ForeColor = Color.FromArgb(192, 192, 255),
            IsTextAutoFit = false
        };
        chart.Legends.Add(legend);

        // Series
        Series series = new Series
        {
            Name = "DoanhThu",
            ChartArea = "ChartArea1",
            ChartType = SeriesChartType.SplineArea,
            Color = Color.FromArgb(52, 152, 219),
            BackSecondaryColor = Color.FromArgb(255, 159, 255),
            BackGradientStyle = GradientStyle.DiagonalRight
        };
        chart.Series.Add(series);

        // Title
        Title title = new Title
        {
            Name = "DoanhThu",
            Text = "Tổng Doanh Thu",
            Alignment = ContentAlignment.TopLeft,
            Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold),
            ForeColor = Color.WhiteSmoke
        };
        chart.Titles.Add(title);

        chart.BackColor = Color.FromArgb(52, 73, 94);
    }


        public static void SetupTop5Chart(Chart chart, string titleText = "Gross Revenue")
         {


            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Titles.Clear();

            ChartArea chartArea2 = new ChartArea
            {
                Name = "ChartArea1",
                BackColor = Color.FromArgb(52, 73, 94)
            };
            chart.ChartAreas.Add(chartArea2);

            Legend legend2 = new Legend
            {
                Name = "Legend1",
                BackColor = Color.FromArgb(52, 73, 94),
                Docking = Docking.Bottom,
                Font = new Font("Microsoft Sans Serif", 10F),
                ForeColor = Color.FromArgb(189, 195, 199),
                IsTextAutoFit = false
            };
            chart.Legends.Add(legend2);

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
            chart.Series.Add(series2);
            chart.Palette = ChartColorPalette.None;
            chart.PaletteCustomColors = new Color[]
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
                Text = "Top 5 sản phẩm bán chạy nhất",
                Alignment = ContentAlignment.TopLeft,
                Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke
            };
            chart.Titles.Add(title2);
            chart.BackColor = Color.FromArgb(52, 73, 94);
            chart.Location = new Point(751, 138);
            chart.Size = new Size(351, 415);
        }
}