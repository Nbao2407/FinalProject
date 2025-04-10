using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace QLVT
{
    public partial class FrmType : Form
    {
        private BUS_LVL lVL = new BUS_LVL();
        private DAL_LVL dal = new DAL_LVL();
        private List<DTO_LVL> danhSach;
        private BindingSource bindingSource = new BindingSource();
        private System.Windows.Forms.Timer debounceTimer;
        private Color defaultLabelColor = Color.White;
        private Color hoverLabelColor = Color.FromArgb(230, 240, 255);

        public FrmType()
        {
            InitializeComponent();
            LoadData();
            ConfigureDataGridView();
            this.Resize += new EventHandler(Frm_Resize);
            txtSearch.KeyDown += txtSearch_KeyDown;
            debounceTimer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop();
                PerformSearch();
            };
        }
        private void ConfigureDataGridView()
        {
            dataGridView.Columns["MaLoaiVatLieu"].HeaderText= "Mã loại";
            dataGridView.Columns["TenLoai"].HeaderText = "Tên loại";
            dataGridView.Columns["TrangThai"].HeaderText = "Trạng thái";
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
            ResizeColumns();
        }
        public void LoadData()
        {
            danhSach = lVL.GetAllLoaiVatLieu();
            dataGridView.DataSource = danhSach;
        }

        private void FrmType_Load(object sender, EventArgs e)
        {
            LoadData();
            CbTrangThai.Items.Add("-- Tất cả TT --");
            CbTrangThai.Items.Add("Hoạt động");
            CbTrangThai.Items.Add("Ngừng sử dụng");
            CbTrangThai.SelectedIndex = 1;
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
        }

        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView.Width = 1150;
                dataGridView.Height = 642;
                dataGridView.Left = (this.ClientSize.Width) / 2;
                dataGridView.Top = (this.ClientSize.Height - 642) / 2;
            }
            else
            {
                dataGridView.Width = this.ClientSize.Width;
                dataGridView.Height = this.ClientSize.Height - 80;
                dataGridView.Left = 25;
                dataGridView.Top = 80;
            }

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            if (dataGridView.Columns.Count == 0) return;

            int totalWidth = dataGridView.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Width = variableColumnWidth;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
            string selectedTrangThai = CbTrangThai.SelectedItem?.ToString();
            if (selectedTrangThai == "-- Tất cả TT --") { selectedTrangThai = null; }
            try
            {
                IEnumerable<DTO_LVL> suggestionSource = danhSach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    suggestionSource = suggestionSource.Where(order =>
                        order.TrangThai != null &&
                        order.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }

                List<DTO_LVL> suggestionResults = new List<DTO_LVL>(); 
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        bool match = false;
                        if (order.MaLoaiVatLieu.ToString().Contains(searchQuery)) match = true;
                        if (!match && order.TenLoai != null && order.TenLoai.ToLowerInvariant().Contains(searchQuery)) match = true;
                        return match;
                    }).ToList();
                }

                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");

                Func<DTO_LVL, string> displayFunc = item => $"{item.MaLoaiVatLieu} - {item.TenLoai ?? "N/A"}";

                Action<DTO_LVL> clickAction = selectedItem => {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.MaLoaiVatLieu.ToString(); 
                    txtSearch.TextChanged += txtSearch_TextChanged;

                    var itemToShow = new List<DTO_LVL> { selectedItem };
                    dataGridView.DataSource = itemToShow;
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

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                e.SuppressKeyPress = true;
                txtSearch.Focus();
            }
        }

        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var lvatLieu = (DTO_LVL)dataGridView.Rows[e.RowIndex].DataBoundItem;
                using (var Pop = new PopupType(this, lvatLieu))
                {
                    Pop.Deactivate += (s, e) => Pop.TopMost = true;
                    Pop.StartPosition = FormStartPosition.CenterParent;
                    Pop.ShowDialog();
                }
            }
        }

        private void ShowPopup()
        {
            using (var popup = new AddType(this))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }

        private void CbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = CbTrangThai.SelectedItem?.ToString();

            if (danhSach == null) { return; }

            IEnumerable<DTO_LVL> listToShow = danhSach; 

            if (filter != "-- Tất cả TT --" && !string.IsNullOrEmpty(filter))
            {
                listToShow = danhSach.Where(k => k.TrangThai != null && k.TrangThai.Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            var filteredByStatus = listToShow.ToList();
            dataGridView.DataSource = filteredByStatus.Any() ? filteredByStatus : null;
            ResizeColumns();

            if (txtSearch.Text.Length > 0)
            {
                txtSearch.Text = string.Empty;
            }
            else
            {
                if (result != null) result.Visible = false;
            }
        }
    }
}