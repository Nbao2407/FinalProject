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
}