using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FrmCustomer : Form
    {
        private BUS_Khach busKhach = new BUS_Khach();
        private QLKH kh = new QLKH();
        private List<DTO_Khach> danhSachKhach;
        private System.Windows.Forms.Timer debounceTimer;
        private Color defaultLabelColor = Color.White;
        private Color hoverLabelColor = Color.FromArgb(230, 240, 255);

        public FrmCustomer()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            this.Load += new EventHandler(FrmCustomer_Load);
            ConfigureDataGridView();
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

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
            CbTrangThai.Items.Add("Tất cả");
            CbTrangThai.Items.Add("Hoạt động");
            CbTrangThai.Items.Add("Ngừng sử dụng");
            CbTrangThai.SelectedIndex = 1;
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaKhachHang", DataPropertyName = "MaKhachHang", HeaderText = "Mã Khách Hàng" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", DataPropertyName = "Ten", HeaderText = "Tên" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgaySinh", DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SDT", DataPropertyName = "SDT", HeaderText = "Số Điện Thoại" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", DataPropertyName = "Email", HeaderText = "Email" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng Thái" });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            ResizeColumns();
        }

        public void LoadKhachHang()
        {
            danhSachKhach = busKhach.LayDanhSachKhach();
            danhSachKhach = danhSachKhach.AsEnumerable().Reverse().ToList();
            dataGridView1.DataSource = danhSachKhach;
            DataGridViewHelper.FormatDateColumns(dataGridView1, "NgaySinh");
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

            string selectedTrangThai = null;
            if (CbTrangThai.SelectedIndex > 0)
            {
                selectedTrangThai = CbTrangThai.Text;
            }

            try
            {
                if (danhSachKhach == null)
                {
                    MessageBox.Show("Danh sách gốc chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result != null) result.Visible = false;
                    dataGridView1.DataSource = null;
                    return;
                }

                IEnumerable<DTO_Khach> filteredSource = danhSachKhach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    filteredSource = filteredSource.Where(tk =>
                        tk != null &&
                        tk.TrangThai != null &&
                        tk.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }
                List<DTO_Khach> finalResults;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    finalResults = filteredSource.Where(tk =>
                    {
                        if (tk == null) return false;
                        bool match = false;
                        if (tk.MaKhachHang.ToString().ToLowerInvariant().Contains(searchQuery))
                            match = true;
                        if (!match && tk.Ten != null && tk.Ten.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
                            match = true;
                        return match;
                    }).ToList();
                }
                else
                {
                    finalResults = filteredSource.Where(tk => tk != null).ToList();
                }

                dataGridView1.DataSource = null;
                if (finalResults.Any())
                {
                    var bindingSource = new BindingSource { DataSource = finalResults };
                    dataGridView1.DataSource = bindingSource;
                }
                ResizeColumns();

                Func<DTO_Khach, string> displayFunc = item => $"{item.MaKhachHang} - {item.Ten}";
                Action<DTO_Khach> clickAction = selectedItem =>
                {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.Ten;
                    txtSearch.TextChanged += txtSearch_TextChanged;
                    var itemToShow = new List<DTO_Khach> { selectedItem };
                    var singleBindingSource = new BindingSource { DataSource = itemToShow };
                    dataGridView1.DataSource = singleBindingSource;
                    ResizeColumns();
                    if (result != null) result.Visible = false;
                };

                SearchHelper.UpdateSearchSuggestions(
                    result,
                    finalResults,
                    txtSearch,
                    38, 190, displayFunc, clickAction);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
        }

        private void CbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
          PerformSearch();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DTO_Khach khach = dataGridView1.Rows[e.RowIndex].DataBoundItem as DTO_Khach;
                if (khach != null)
                {
                    ShowPopupWithData(khach);
                }
            }
        }

        private void ShowPopupWithData(DTO_Khach khach)
        {
            using (var popup = new PopupCmer(this, khach))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
                LoadKhachHang();
            }
        }

        private void ShowPopup()
        {
            using (var popup = new AddCustomer(this))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
                LoadKhachHang();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowPopup();
        }
    }
}