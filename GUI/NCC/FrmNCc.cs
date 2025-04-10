using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.NCC;

namespace QLVT
{
    public partial class FrmNCc : Form
    {
        private Panel overlayPanel;
        private BUS_Ncc busNcc = new BUS_Ncc();
        private DAL_NCcap ncc = new DAL_NCcap();
        private List<DTO_Ncap> danhSach;
        private DTO_Ncap _ncc;
        private System.Windows.Forms.Timer debounceTimer;

        public FrmNCc()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            this.Load += FrmNCc_Load;
            ConfigureDataGridView();
            SetupDebounceTimer();
        }
        private void SetupDebounceTimer()
        {
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop();
                PerformSearch();
            };
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNCC", DataPropertyName = "MaNCC", HeaderText = "Mã NCC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", DataPropertyName = "TenNCC", HeaderText = "Tên NCC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SDT", DataPropertyName = "SDT", HeaderText = "Số Điện Thoại" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email" });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            ResizeColumns();
        }
        public void Loaddata()
        {
            danhSach = busNcc.GetAllNcc();
            dataGridView1.DataSource = danhSach;
        }

        private void FrmNCc_Load(object sender, EventArgs e)
        {
            Loaddata();
        }


        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView1.Width = 1150;
                dataGridView1.Height = 642;
                dataGridView1.Left = (this.ClientSize.Width) / 2;
                dataGridView1.Top = (this.ClientSize.Height - 642) / 2;
            }
            else
            {
                dataGridView1.Width = this.ClientSize.Width;
                dataGridView1.Height = this.ClientSize.Height - 80;
                dataGridView1.Left = 25;
                dataGridView1.Top = 80;
            }

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            if (dataGridView1.Columns.Count == 0) return;

            int totalWidth = dataGridView1.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView1.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Width = fixedColumnWidth;
                column.Width = variableColumnWidth;
            }
        }

        private void PopupFrm_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DTO_Ncap ncc = dataGridView1.Rows[e.RowIndex].DataBoundItem as DTO_Ncap;
                if (ncc != null)
                {
                    ShowPopup(ncc);
                }
            }
        }

        private void ShowPopup(DTO_Ncap ncc)
        {

            using (var popup = new PopNcc(this, ncc))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
                Loaddata();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var popup = new AddNcc(this))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
            try
            {
                IEnumerable<DTO_Ncap> suggestionSource = danhSach;
                List<DTO_Ncap> suggestionResults = new List<DTO_Ncap>();
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        bool match = false;
                        if (order.MaNCC.ToString().Contains(searchQuery)) match = true;
                        if (!match && order.TenNCC.ToLowerInvariant().Contains(searchQuery)) match = true;
                        return match;
                    }).ToList();
                }

                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");

                Func<DTO_Ncap, string> displayFunc = item => $"{item.MaNCC}-{item.TenNCC ?? "N/A"}";

                Action<DTO_Ncap> clickAction = selectedItem => {
                    txtSearch.TextChanged -= txtSearch_TextChanged_1;
                    txtSearch.Text = selectedItem.MaNCC.ToString(); 
                    txtSearch.TextChanged += txtSearch_TextChanged_1;

                    var itemToShow = new List<DTO_Ncap> { selectedItem };
                    dataGridView1.DataSource = itemToShow;
                    Console.WriteLine($"DGV DataSource updated to show only item {selectedItem.MaNCC}");
                    ResizeColumns();
                };

                SearchHelper.UpdateSearchSuggestions(
                    result,
                    suggestionResults,
                    txtSearch,
                    38,
                    190,
                    displayFunc,
                    clickAction
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm gợi ý: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
            }
        }
    }
}