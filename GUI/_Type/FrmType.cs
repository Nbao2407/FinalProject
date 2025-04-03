using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using GUI.Type;
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

namespace GUI
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
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaLoaiVatLieu", DataPropertyName = "MaLoaiVatLieu", HeaderText = "Mã Loại" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenLoai", DataPropertyName = "TenLoai", HeaderText = "Tên Loại" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng thái" });


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
            CbTrangThai.Items.Add("Tất cả");
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
            string searchQuery = txtSearch.Text.Trim();
            string selectedTrangThai = CbTrangThai.SelectedItem?.ToString();

            try
            {
                List<DTO_LVL> results = dal.SearchProductTypes(searchQuery);

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    results = results.Where(kh => kh.TrangThai == selectedTrangThai).ToList();
                }

                UpdateSearchSuggestions(results);

                UpdateDataGridView(searchQuery, results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result.Visible = false;
                dataGridView.DataSource = null;
            }
        }
        private void UpdateDataGridView(string searchQuery, List<DTO_LVL> results)
        {
            if (searchQuery.Length > 0)
            {
                dataGridView.DataSource = results;
            }
            else
            {
                var all = lVL.GetAllLoaiVatLieu();
                if (!string.IsNullOrEmpty(CbTrangThai.SelectedItem?.ToString()))
                {
                    all = all.Where(kh => kh.TrangThai == CbTrangThai.SelectedItem.ToString()).ToList();
                }
                dataGridView.DataSource = all;
            }
        }
        private void UpdateSearchSuggestions(List<DTO_LVL> results)
        {
            result.Controls.Clear();
            if (results.Any() && txtSearch.Text.Trim().Length > 0)
            {
                result.Height = Math.Min(results.Count * 40, 200);
                result.BackColor = Color.Transparent;

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = $"👤 {item.TenLoai}-{item.MaLoaiVatLieu}",
                        AutoSize = false,
                        Width = result.Width - 2,
                        Height = 40,
                        Padding = new Padding(10, 5, 5, 5),
                        Font = new Font("Segoe UI", 11, FontStyle.Regular),
                        BackColor = defaultLabelColor,
                        ForeColor = Color.FromArgb(33, 37, 41),
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(1),
                        Tag = item
                    };

                    lbl.MouseEnter += (s, e) =>
                    {
                        lbl.BackColor = hoverLabelColor;
                        lbl.ForeColor = Color.FromArgb(0, 102, 204);
                    };
                    lbl.MouseLeave += (s, e) =>
                    {
                        lbl.BackColor = defaultLabelColor;
                        lbl.ForeColor = Color.FromArgb(33, 37, 41);
                    };

                    lbl.Click += (s, e) =>
                    {
                        var selectedItem = (DTO_LVL)lbl.Tag;
                        txtSearch.Text = selectedItem.TenLoai;
                        result.Visible = false;
                        dataGridView.DataSource = new List<DTO_LVL> { selectedItem };
                    };

                    result.Controls.Add(lbl);
                }
                result.Visible = true;
            }
            else
            {
                result.Visible = false;
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
            string filter = CbTrangThai.SelectedItem.ToString();
            if (filter == "Tất cả")
            {
                dataGridView.DataSource = danhSach;
            }
            else
            {
                var filteredList = danhSach.Where(k => k.TrangThai == filter).ToList();
                dataGridView.DataSource = filteredList;
            }
        }
    }
}