using System.Reflection;

namespace QLVT.Helper
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
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ScrollBars = ScrollBars.Vertical;
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
        public static void DisableColumnSorting(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public static void FillColumnsToDgvWidth(DataGridView dgv)
        {
            if (dgv.Columns.Count == 0) return; 
            int totalWidth = dgv.ClientSize.Width;
            int columnWidth = totalWidth / dgv.Columns.Count;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.Width = columnWidth;
            }
            int remainingWidth = totalWidth - (columnWidth * dgv.Columns.Count);
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns[dgv.Columns.Count - 1].Width += remainingWidth;
            }
        }
        public static int TinhTongSoLuongChon(DataGridView dgv, string tenCotSoLuong)
        {
            int tongSoLuong = 0;
            if (!dgv.Columns.Contains(tenCotSoLuong))
            {
                throw new ArgumentException($"Column named {tenCotSoLuong} cannot be found.", nameof(tenCotSoLuong));
            }
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                if (row.Cells[tenCotSoLuong].Value != null)
                {
                    tongSoLuong += Convert.ToInt32(row.Cells[tenCotSoLuong].Value);
                }
            }
            return tongSoLuong;
        }
        public static void AddOrUpdateToTop<T>(List<T> list, T item, DataGridView dgv)
        {
            int existingIndex = list.IndexOf(item);
            if (existingIndex >= 0)
            {
                list.RemoveAt(existingIndex); 
            }
            list.Insert(0, item);
            RefreshDataGridView(dgv, list);
        }

        private static void RefreshDataGridView<T>(DataGridView dgv, List<T> list)
        {
            dgv.DataSource = null; 
            dgv.DataSource = list;
            dgv.Refresh(); 
        }
    }
}