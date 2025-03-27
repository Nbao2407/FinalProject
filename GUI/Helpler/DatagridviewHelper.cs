using System.Reflection;

namespace GUI.Helpler
{
    public static class DataGridViewHelper
    {
        public static void CustomizeDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(72, 144, 232);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 60;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(112, 185, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;
            dgv.GridColor = Color.LightGray;
            dgv.RowHeadersVisible = false;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DoubleBuffered(true);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ScrollBars = ScrollBars.None;
            dgv.MultiSelect = true;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;

        }
        public static void FormatDateColumns(DataGridView dataGridView, params string[] dateColumnNames)
        {
            foreach (string columnName in dateColumnNames)
            {
                if (dataGridView.Columns[columnName] != null)
                {
                    dataGridView.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi?.SetValue(dgv, setting, null);
        }

        public static int TinhTongSoLuongChon(DataGridView dgv, string tenCotSoLuong)
        {
            int tongSoLuong = 0;
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                if (row.Cells[tenCotSoLuong].Value != null)
                {
                    tongSoLuong += Convert.ToInt32(row.Cells[tenCotSoLuong].Value);
                }
            }
            return tongSoLuong;
        }
    }
}