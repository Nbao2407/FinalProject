using BUS; 
using DAL; 
using DTO;
using Microsoft.Data.SqlClient; 
using QLVT.Helper;
using QLVT.Order;
using System;
using System.Collections.Generic; 
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration; 
namespace QLVT
{
    public partial class FrmOrder : Form
    {
        private System.Windows.Forms.Timer debounceTimer;
        private readonly Color defaultLabelColor = Color.White;
        private readonly Color hoverLabelColor = Color.FromArgb(230, 240, 255);
        private  BUS_Order _busOrder = new BUS_Order();
        private  DAL_Order _dalOrder = new DAL_Order();
        private List<DTO_DonNhapSearchResult> currentSearchResults;
        private List<DTO_Order> danhSach;
        private BindingSource orderBindingSource = new BindingSource();
        private DataTable initialOrderData;
        public FrmOrder()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Frm_Resize);
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadComboBoxes();
            LoadInitialData();
            ResizeColumns();
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

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            PerformSearch();     
        }


        private void FrmOrder_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadComboBoxes();
            LoadInitialData(); 
            ResizeColumns();
        }

        private void LoadInitialData()
        {
            try
            {
                DataTable orders = _dalOrder.GetAllOrder();
                dataGridView1.DataSource = orders;

                if (orders != null && orders.Rows.Count > 0)
                {
                    this.Resize += new EventHandler(Frm_Resize);

                    CustomizeInitialColumns(); 
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu ban đầu để hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomizeInitialColumns()
        {

            if (dataGridView1.Columns["MaDonNhap"] != null)
                dataGridView1.Columns["MaDonNhap"].HeaderText = "Mã ĐN";
            if (dataGridView1.Columns["NgayNhap"] != null)
            {
                dataGridView1.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                dataGridView1.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dataGridView1.Columns["TenNCC"] != null)
                dataGridView1.Columns["TenNCC"].HeaderText = "Nhà Cung Cấp";
            if (dataGridView1.Columns["TrangThai"] != null)
                dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void CustomizeSearchResultColumns()
        {
            if (dataGridView1.Columns["MaDonNhap"] != null)
                dataGridView1.Columns["MaDonNhap"].HeaderText = "Mã ĐN";
            if (dataGridView1.Columns["NgayNhap"] != null)
            {
                dataGridView1.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                dataGridView1.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dataGridView1.Columns["TenNhaCungCap"] != null)
                dataGridView1.Columns["TenNhaCungCap"].HeaderText = "Nhà Cung Cấp";
            if (dataGridView1.Columns["TrangThai"] != null)
                dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                object dataItem = dataGridView1.Rows[e.RowIndex].DataBoundItem;

                int? maDonNhap = null;

                if (dataItem is DataRowView rowView) 
                {
                    if (rowView.Row.Table.Columns.Contains("MaDonNhap") && rowView.Row["MaDonNhap"] != DBNull.Value)
                    {
                        maDonNhap = Convert.ToInt32(rowView.Row["MaDonNhap"]);
                    }
                }
                else if (dataItem is DTO_DonNhapSearchResult searchResult) 
                {
                    maDonNhap = searchResult.MaDonNhap;
                }


                if (maDonNhap.HasValue)
                {
                    ShowPopup(maDonNhap.Value);
                }
                else
                {
                    MessageBox.Show("Không thể lấy Mã Đơn Nhập từ dòng được chọn.", "Lỗi", MessageBoxButtons.OK);
                }
            }
        }

        private void ShowPopup(int? orderId = null)
        {
            using (var popup = new PopupOrder()) 
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog(this);
                                        
                                        
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            NhapHang nhap = new NhapHang()
            {
                TopLevel = false,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(nhap);
            nhap.Show();
        }

        private void LoadComboBoxes()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("-- Tất cả TT --");
            cboTrangThai.Items.Add("Hoàn thành");
            cboTrangThai.Items.Add("Đang xử lý");
            cboTrangThai.SelectedIndex = 0;
        }


        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim();
            string selectedTrangThai = cboTrangThai.SelectedItem?.ToString();

            try
            {
                DAL_Order da = new DAL_Order();
                List<DTO_Order> results = da.SearchOrder(searchQuery);

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
                dataGridView1.DataSource = null;
            }
        }



        private void UpdateDataGridView(string searchQuery, List<DTO_Order> results)
        {
            BUS_Order bus = new BUS_Order();
            if (searchQuery.Length > 0)
            {
                dataGridView1.DataSource = results;
            }
            else
            {
                var all = bus.GetAllOrder();
                if (!string.IsNullOrEmpty(cboTrangThai.SelectedItem?.ToString()))
                {
                    all = all.Where(kh => kh.TrangThai == cboTrangThai.SelectedItem.ToString()).ToList();
                }
                dataGridView1.DataSource = all;
            }
        }


        private void UpdateSearchSuggestions(List<DTO_Order> results)
        {
            result.Controls.Clear(); // Panel holding suggestion labels
            if (results != null && results.Any() && txtSearch.Text.Trim().Length > 0)
            {
                result.Height = Math.Min(results.Count * 35, 180); // Limit height
                result.BackColor = Color.White;


                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = $"{item.MaDonNhap} - {item.TenNhaCungCap ?? "N/A"}", // Use DTO properties
                        AutoSize = false,
                        Dock = DockStyle.Top,
                        Height = 35,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Padding = new Padding(10, 0, 0, 0),
                        Font = new Font("Segoe UI", 10f),
                        BackColor = defaultLabelColor,
                        ForeColor = Color.FromArgb(33, 37, 41),
                        Cursor = Cursors.Hand,
                        Margin = new Padding(0, 0, 0, 1),
                        Tag = item 
                    };

                    lbl.MouseEnter += (s, e) => lbl.BackColor = hoverLabelColor;
                    lbl.MouseLeave += (s, e) => lbl.BackColor = defaultLabelColor;

                    lbl.Click += (s, e) =>
                    {
                        var selectedItem = (DTO_Order)lbl.Tag;
                        txtSearch.TextChanged -= txtSearch_TextChanged;
                        txtSearch.Text = selectedItem.MaDonNhap.ToString();
                        txtSearch.TextChanged += txtSearch_TextChanged;

                        result.Visible = false; // Hide suggestions

                        orderBindingSource.DataSource = new List<DTO_Order> { selectedItem };
                        CustomizeSearchResultColumns(); 
                        ResizeColumns();
                    };

                    result.Controls.Add(lbl);
                    lbl.BringToFront();
                }
                result.Visible = true;
            }
            else
            {
                result.Visible = false;
            }
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = cboTrangThai.SelectedItem.ToString();
            if (filter == "-- Tất cả TT --")
            {
                dataGridView1.DataSource = danhSach;
            }
            else
            {
                var filteredList = danhSach.Where(k => k.TrangThai == filter).ToList();
                dataGridView1.DataSource = filteredList;
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

    }
}