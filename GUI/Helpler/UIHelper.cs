using System.Windows.Forms;
using System.Drawing;

namespace QLVT.Helper
{
    public static class UIHelper
    {
        private static readonly Color FormBackColor = Color.FromArgb(44, 62, 80);
        private static readonly Color PanelBackColor = Color.FromArgb(52, 73, 94);
        private static readonly Color ButtonBorderColor = Color.FromArgb(52, 152, 219);
        private static readonly Color ButtonBackColor = Color.FromArgb(52, 152, 219);
        private static readonly Color PrimaryTextColor = Color.WhiteSmoke;
        private static readonly Color SecondaryTextColor = Color.FromArgb(189, 195, 199);
        private static readonly Color GridLineColor = Color.Gray;
        private const string VietnameseDateFormat = "dd/MM/yyyy";

        public static void SetupFormUI(Form form)
        {
            form.BackColor = FormBackColor;
        }

        public static void SetupDataGridViewUI(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.GridColor = GridLineColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
            dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;
        }

     
        public static void SetupTableLayoutPanelUI(TableLayoutPanel tlp, float column1Percent = 30F, float column2Percent = 70F)
        {
            tlp.Size = new Size(0, 0);
            tlp.ColumnStyles.Clear();
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, column1Percent));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, column2Percent));
        }

        public static void SetupPanelUI(Panel panel)
        {
            panel.BackColor = PanelBackColor;
        }

    
        public static void SetupButtonUI(Button btn, bool isBorderOnly = true)
        {
            if (isBorderOnly)
            {
                btn.FlatAppearance.BorderColor = ButtonBorderColor;
            }
            else
            {
                btn.BackColor = ButtonBackColor;
            }
            btn.FlatStyle = FlatStyle.Flat; 
        }

        public static void SetupLabelUI(Label label, bool isPrimaryColor = true)
        {
            label.ForeColor = isPrimaryColor ? PrimaryTextColor : SecondaryTextColor;
        }

        public static void SetupDateTimePickerUI(DateTimePicker dtp)
        {
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = VietnameseDateFormat;
        }
        public static void SetDateLabel(Label label, DateTime date, bool isPrimaryColor = true)
        {
            label.Text = date.ToString(VietnameseDateFormat);
            label.ForeColor = isPrimaryColor ? PrimaryTextColor : SecondaryTextColor;
        }
        public static void SetupDashboardUI(Form form, DataGridView dgvUnderstock,
              TableLayoutPanel tlp4, TableLayoutPanel tlp2,
              Panel panel1, Panel panel2, Panel panel3, Panel panel5,
              Button btnToday, Button btnLast7Days, Button btnLast30Days, Button btnCustomDate, Button btnOkCustomDate,
              Label label1, Label lblNumOrders, Label lblTotalRevenue, Label lblTotalProfit,
              Label lblNumCustomers, Label lblNumSuppliers,
              Label label2, Label label4, Label label5, Label lblStartDate, Label lblEndDate,
              DateTimePicker dtpStartDate, DateTimePicker dtpEndDate)
        {
            SetupFormUI(form); 
            SetupDataGridViewUI(dgvUnderstock);
            SetupTableLayoutPanelUI(tlp4);
            SetupTableLayoutPanelUI(tlp2);

            SetupPanelUI(panel1);
            SetupPanelUI(panel2);
            SetupPanelUI(panel3);
            SetupPanelUI(panel5);

            SetupButtonUI(btnToday);
            SetupButtonUI(btnLast7Days);
            SetupButtonUI(btnLast30Days);
            SetupButtonUI(btnCustomDate);
            SetupButtonUI(btnOkCustomDate, false);

            SetupDateTimePickerUI(dtpStartDate);
            SetupDateTimePickerUI(dtpEndDate);

            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpEndDate.Value = DateTime.Now;
            SetDateLabel(lblStartDate, dtpStartDate.Value, false);
            SetDateLabel(lblEndDate, dtpEndDate.Value, false);

            SetupLabelUI(label1);
            SetupLabelUI(lblNumOrders);
            SetupLabelUI(lblTotalRevenue);
            SetupLabelUI(lblTotalProfit);
            SetupLabelUI(lblNumCustomers);
            SetupLabelUI(lblNumSuppliers);

            SetupLabelUI(label2, false);
            SetupLabelUI(label4, false);
            SetupLabelUI(label5, false);
            SetupLabelUI(lblStartDate, false);
            SetupLabelUI(lblEndDate, false);
        }
    }
}