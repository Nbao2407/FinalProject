using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using QLVT.Helper;
using QLVT.Order;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace QLVT
{
    public partial class FrmOrder : Form
    {
        private System.Windows.Forms.Timer debounceTimer;
        private readonly Color defaultLabelColor = Color.White;
        private readonly Color hoverLabelColor = Color.FromArgb(230, 240, 255);
        private readonly BUS_Order _busOrder = new BUS_Order();
        private readonly DAL_Order _dalOrder = new DAL_Order(); 
        private List<DTO_Order> danhSach = new List<DTO_Order>();
        
        private List<DTO_DonNhapSearchResult> _allLoadedOrders = new List<DTO_DonNhapSearchResult>();

        public FrmOrder()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadComboBoxes();
            LoadInitialData();
            SetupDebounceTimer();
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMaDonNhap",
                HeaderText = "Mã ĐN",
                DataPropertyName = "MaDonNhap",
                Width = 80,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNgayNhap",
                HeaderText = "Ngày Nhập",
                DataPropertyName = "NgayNhap", 
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } 
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTenNCC",
                HeaderText = "Nhà Cung Cấp",
                DataPropertyName = "TenNCC",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTrangThai",
                HeaderText = "Trạng Thái",
                DataPropertyName = "TrangThai", 
                Width = 100,
                ReadOnly = true
            });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);

            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            this.Resize += new EventHandler(Frm_Resize);
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

        private void LoadInitialData()
        {
            try
            {
                danhSach = _dalOrder.GetAllOrder(); 
                danhSach = danhSach.AsEnumerable().Reverse().ToList();

                if (danhSach != null && danhSach.Any())
                {
                    dataGridView1.DataSource = danhSach;
                    ResizeColumns();
                }
                else
                {
                    danhSach = new List<DTO_Order>();
                    dataGridView1.DataSource = null;
                    MessageBox.Show("Không có dữ liệu ban đầu để hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                danhSach = new List<DTO_Order>();
                dataGridView1.DataSource = null;
            }
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
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];

                if (clickedRow.DataBoundItem is DTO_Order selectedOrder)
                {
                    int orderId = selectedOrder.MaDonNhap;
                    ShowPopup(orderId); 
                }
                else
                {
                    MessageBox.Show("Không thể xác định dữ liệu đơn hàng từ dòng được chọn.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Console.WriteLine($"Double click on row {e.RowIndex}, but DataBoundItem is not DTO_Order or is null. Actual Type: {clickedRow.DataBoundItem?.GetType().Name ?? "null"}");
                }
            }
        }

        private void ShowPopup(int? orderId)
        {
            try
            {
                using (var popup = new PopupOrder(orderId))
                {

                    popup.StartPosition = FormStartPosition.CenterParent;

                    DialogResult result = popup.ShowDialog();

                    bool dataMayHaveChanged = popup.DataChanged;
                    if (dataMayHaveChanged)
                    {
                        Console.WriteLine("Data may have changed, reloading initial data...");
                        LoadInitialData();
                    }
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open or process the order details.\nError: {ex.Message}", "Popup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
   
       
        private void button2_Click(object sender, EventArgs e) 
        {
            Control parentControl = this.Parent; 
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this);
                this.Dispose();

                AddXuat nhap = new AddXuat()
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                    FormBorderStyle = FormBorderStyle.None
                };
                parentControl.Controls.Add(nhap);
                nhap.Show();
            }
            else
            {
                AddXuat nhap = new AddXuat();
                nhap.Show(); 
            }
        }

        private void LoadComboBoxes()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("-- Tất cả TT --");
            cboTrangThai.Items.Add("Hoàn thành");
            cboTrangThai.Items.Add("Đang xử lý"); 
            cboTrangThai.SelectedIndex = -1;
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
            if (cboTrangThai.SelectedIndex > 0) 
            {
                selectedTrangThai = cboTrangThai.Text; 
            }

            try
            {
                if (danhSach == null)
                {
                    MessageBox.Show("Danh sách đơn nhập chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result != null) result.Visible = false;
                    dataGridView1.DataSource = null; 
                    return;
                }

                IEnumerable<DTO_Order> suggestionSource = danhSach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    suggestionSource = suggestionSource.Where(order =>
                        order != null && 
                        order.TrangThai != null && 
                        order.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }

                List<DTO_Order> suggestionResults;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        if (order == null) return false;

                        bool match = false;
                        if (order.MaDonNhap.ToString().ToLowerInvariant().Contains(searchQuery))
                        {
                            match = true;
                        }

                        if (!match && order.TenNCC != null &&
                            order.TenNCC.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
                        {
                            match = true;
                        }
                        return match;
                    }).ToList();
                }
                else
                {
                    suggestionResults = suggestionSource.Where(order => order != null).ToList();
                }


                dataGridView1.DataSource = null;
                if (suggestionResults.Any())
                {
                    dataGridView1.DataSource = suggestionResults;
                }

                Func<DTO_Order, string> displayFunc = item => $"{item.MaDonNhap} - {item.TenNCC}";

                Action<DTO_Order> clickAction = selectedItem => {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.TenNCC;
                    txtSearch.TextChanged += txtSearch_TextChanged;
                    var itemToShow = new List<DTO_Order> { selectedItem };
                    dataGridView1.DataSource = itemToShow; 
                    ResizeColumns();
                    if (result != null) result.Visible = false; 
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
            catch (NullReferenceException nre) 
            {
                MessageBox.Show($"Lỗi tham chiếu null khi tìm kiếm: {nre.Message}\nKiểm tra xem có đối tượng 'order' nào bị null trong danh sách không.\n{nre.StackTrace}", "Lỗi Null Reference", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khác khi tìm kiếm: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
    }
}