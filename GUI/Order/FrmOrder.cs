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
            debounceTimer.Tick += DebounceTimer_Tick; ;
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
            this.Resize += new EventHandler(Frm_Resize);

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
            this.Resize += new EventHandler(Frm_Resize);
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
                dataGridView1.Width = 1100;
                dataGridView1.Height = 642;
                dataGridView1.Left = (this.ClientSize.Width) / 2;
                dataGridView1.Top = (this.ClientSize.Height - 642) / 2;
            }
            else if(this.WindowState == FormWindowState.Maximized)
            {
                dataGridView1.Width = this.ClientSize.Width-20;
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

            int variableColumnWidth = 0; 
            if (variableColumnCount > 0) 
            {
                variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;
            }


            if (variableColumnWidth > 0)
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.Width = variableColumnWidth;
                }
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
            string keyword = txtSearch.Text.Trim();
            string selectedTrangThai = cboTrangThai.SelectedItem?.ToString();
            bool isStatusFilterActive = selectedTrangThai != null && selectedTrangThai != "-- Tất cả TT --";

            try
            {
                this.Cursor = Cursors.WaitCursor;
                result.Visible = false;

                List<DTO_DonNhapSearchResult> results = null; 

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    results = new List<DTO_DonNhapSearchResult>();
                }
                else
                {
                    var orderResults = _busOrder.TimKiemDonNhap(keyword);


                    if (orderResults != null && orderResults.Count > 0)
                    {
                        try
                        {
                            results = orderResults.Select(order => new DTO_DonNhapSearchResult
                            {
                                MaDonNhap = order.MaDonNhap,
                                TenNhaCungCap = order.TenNhaCungCap,
                                NgayNhap = order.NgayNhap,
                                TrangThai = order.TrangThai
                            }).ToList();
                        }
                        catch (Exception mapEx)
                        {
                            MessageBox.Show($"Lỗi khi map dữ liệu: {mapEx.Message}\nKiểm tra lại tên thuộc tính trong LINQ Select.", "Lỗi Mapping", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            results = new List<DTO_DonNhapSearchResult>(); 
                        }
                    }
                    else
                    {
                        results = new List<DTO_DonNhapSearchResult>();
                    }
                }

                if (results != null && results.Any()) 
                {
                    Console.WriteLine($"Mẫu sau map: ID={results[0].MaDonNhap} | NCC='{results[0].TenNhaCungCap}' | Status='{results[0].TrangThai}'"); // **GHI LẠI GIÁ TRỊ NÀY**
                }

                if (isStatusFilterActive && results != null)
                {
                    int countBeforeFilter = results.Count;
                    results = results.Where(r => r.TrangThai != null && r.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)).ToList();
                    int countAfterFilter = results.Count; 
                    MessageBox.Show($"Lọc theo trạng thái '{selectedTrangThai}': Trước={countBeforeFilter}, Sau={countAfterFilter}"); // **GHI LẠI GIÁ TRỊ NÀY**
                }

                currentSearchResults = results ?? new List<DTO_DonNhapSearchResult>();

                UpdateSearchSuggestions(currentSearchResults);
                UpdateDataGridView(currentSearchResults);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void UpdateDataGridView(List<DTO_DonNhapSearchResult> results)
        {
            dataGridView1.DataSource = null;
            if (results != null && results.Count > 0)
            {
                dataGridView1.DataSource = results;
                CustomizeSearchResultColumns();

            }
            else
            {
                dataGridView1.DataSource = null;

            }
        }


        private void UpdateSearchSuggestions(List<DTO_DonNhapSearchResult> results)
        {
            result.Controls.Clear();
            if (results != null && results.Any() && txtSearch.Text.Trim().Length > 0)
            {
                result.Height = Math.Min(results.Count * 35, 180); 
                result.BackColor = Color.White;


                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = $"{item.MaDonNhap}-{item.TenNhaCungCap ?? "N/A"}", 
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
                        var selectedItem = (DTO_DonNhapSearchResult)lbl.Tag;
                        txtSearch.TextChanged -= txtSearch_TextChanged;
                        txtSearch.Text = selectedItem.MaDonNhap.ToString();
                        result.Visible = false; 

                        UpdateDataGridView(new List<DTO_DonNhapSearchResult> { selectedItem });
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
            PerformSearch();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

    }
}