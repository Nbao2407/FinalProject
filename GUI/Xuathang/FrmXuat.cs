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
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using GUI.Helpler;

namespace QLVT
{
    public partial class FrmXuat : Form
    {
        private List<DTO_DonXuat> currentDataSource = new List<DTO_DonXuat>();
        private System.Windows.Forms.Timer debounceTimer;
        private BUS_DonXuat busDonXuat = new BUS_DonXuat();

        public FrmXuat()
        {
            InitializeComponent();
            LoadAllDonXuat();
            LoadComboBoxes();
            SetupDebounceTimer();
            ConfigureDataGridView();
            SetupDebounceTimer();
            LoadStatusComboBox();
            this.Resize += Frm_Resize;
        }
        private void ConfigureDataGridView()
        {
            dgvDonXuat.AutoGenerateColumns = false;
            dgvDonXuat.Columns.Clear();

            dgvDonXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColMaDonXuat", HeaderText = "Mã ĐX", DataPropertyName = "MaDonXuat", Width = 60 });
            dgvDonXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColNgayXuat", HeaderText = "Ngày Xuất", DataPropertyName = "NgayXuat", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }, Width = 90 });
            dgvDonXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColLoaiXuat", HeaderText = "Loại Xuất ", DataPropertyName = "LoaiXuat", Width = 120 });
            dgvDonXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColKhachHang", HeaderText = "Khách Hàng", DataPropertyName = "TenKhachHang", Width = 150 });
            dgvDonXuat.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTrangThai", HeaderText = "Trạng Thái", DataPropertyName = "TrangThai", Width = 150 });
            dgvDonXuat.AllowUserToAddRows = false;
            dgvDonXuat.ReadOnly = true;
            DataGridViewHelper.CustomizeDataGridView(dgvDonXuat);
            dgvDonXuat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDonXuat.MultiSelect = false;
            dgvDonXuat.RowHeadersVisible = false;
        }
        private void LoadStatusComboBox()
        {
            cboTrangThaiFilter.Items.Clear();
            cboTrangThaiFilter.Items.Add("-- Tất cả TT --");
            cboTrangThaiFilter.Items.Add("Chờ duyệt");
            cboTrangThaiFilter.Items.Add("Hoàn thành");
            cboTrangThaiFilter.Items.Add("Đã hủy");
            cboTrangThaiFilter.SelectedIndex = 0;
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
        public void LoadAllDonXuat()
        {
            try
            {
                currentDataSource = busDonXuat.GetAllDonXuat();
                BindDataToGrid(currentDataSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn xuất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BindDataToGrid(new List<DTO_DonXuat>());
            }
        }


        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dgvDonXuat.Width = 1150;
                dgvDonXuat.Height = 642;
                dgvDonXuat.Left = (this.ClientSize.Width) / 2;
                dgvDonXuat.Top = (this.ClientSize.Height - 642) / 2;
            }
            else
            {
                int leftMargin = 25;
                int rightMargin = 20;
                int topMargin = 80;
                int bottomMargin = 20;

                if (dgvDonXuat != null)
                {
                    dgvDonXuat.Left = leftMargin;
                    dgvDonXuat.Top = topMargin;

                    int calculatedWidth = this.ClientSize.Width - leftMargin - rightMargin;
                    dgvDonXuat.Width = Math.Max(50, calculatedWidth);

                    int calculatedHeight = this.ClientSize.Height - topMargin - bottomMargin;
                    dgvDonXuat.Height = Math.Max(50, calculatedHeight);
                }

                ResizeColumns();
            }
        }
        private void ResizeColumns()
        {
            if (dgvDonXuat.Columns.Count == 0) return;

            int totalWidth = dgvDonXuat.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dgvDonXuat.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dgvDonXuat.Columns)
            {
                column.Width = variableColumnWidth;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDonXuat.Rows.Count && !dgvDonXuat.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow clickedRow = dgvDonXuat.Rows[e.RowIndex];

                if (clickedRow.DataBoundItem is DTO_DonXuat selected)
                {
                    int orderId = selected.MaDonXuat;

                    if (CurrentUser.ChucVu == "Nhân viên")
                    {
                        if (selected.TenNguoiLap == CurrentUser.TenDangNhap)
                        {
                            Console.WriteLine($"User {CurrentUser.MaTK} (Role: Nhân viên) is the creator ({selected.TenNguoiLap}). Opening order {orderId}.");
                            ShowPopup(orderId);
                        }
                        else
                        {
                            Console.WriteLine($"User {CurrentUser.MaTK} (Role: Nhân viên) is NOT the creator ({selected.TenNguoiLap}). Denying access to order {orderId}.");
                            MessageBox.Show("Bạn là Nhân viên nhưng không phải người tạo đơn này nên không thể mở.", "Không có quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {

                        ShowPopup(orderId);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể xác định dữ liệu đơn hàng từ dòng được chọn.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Console.WriteLine($"Double click on row {e.RowIndex}, but DataBoundItem is not DTO_Order or is null. Actual Type: {clickedRow.DataBoundItem?.GetType().Name ?? "null"}");
                }
            }
        }
        private void ShowPopup(int orderId)
        {
            try
            {
                using (var popup = new PopupXuat(orderId,this))
                {
                    popup.StartPosition = FormStartPosition.CenterParent;

                    DialogResult result = popup.ShowDialog();

                    bool dataMayHaveChanged = popup.DataChanged; 

                    if (dataMayHaveChanged)
                    {
                        Console.WriteLine("Data may have changed, reloading initial data...");
                        LoadAllDonXuat();
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
            cboTrangThaiFilter.Items.Clear();
            cboTrangThaiFilter.Items.Add("-- Tất cả TT --");
            cboTrangThaiFilter.Items.Add("Hoàn thành");
            cboTrangThaiFilter.Items.Add("Chờ duyệt");
            cboTrangThaiFilter.SelectedIndex = -1;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        public void PerformSearch()
        {
            string keyword = txtSearch.Text.Trim();
            string selectedTrangThai = null;

            if (cboTrangThaiFilter.SelectedIndex > 0)
            {
                selectedTrangThai = cboTrangThaiFilter.SelectedItem.ToString();
            }

            try
            {
                if (string.IsNullOrEmpty(keyword) && selectedTrangThai == null)
                {
                    LoadAllDonXuat();
                    SearchHelper.UpdateSearchSuggestions(result, new List<DTO_DonXuat>(), txtSearch, 0, 0, null, null);
                    return;
                }


                IEnumerable<DTO_DonXuat> filteredList = currentDataSource;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    filteredList = filteredList.Where(dx => dx.TrangThai != null && dx.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    string lowerKeyword = keyword.ToLowerInvariant();
                    filteredList = filteredList.Where(dx =>
                        (dx.MaDonXuat.ToString().Contains(lowerKeyword)) ||
                        (dx.TenNguoiLap != null && dx.TenNguoiLap.ToLowerInvariant().Contains(lowerKeyword)) ||
                        (dx.TenKhachHang != null && dx.TenKhachHang.ToLowerInvariant().Contains(lowerKeyword)) ||
                         (dx.GhiChu != null && dx.GhiChu.ToLowerInvariant().Contains(lowerKeyword))
                    );
                }

                var finalResults = filteredList.ToList();
                BindDataToGrid(finalResults);

                // Ví dụ đơn giản là hiển thị các kết quả grid làm gợi ý
                Func<DTO_DonXuat, string> displayFunc = item => $"{item.MaDonXuat} - {item.TenKhachHang ?? item.TenNguoiLap}";
                Action<DTO_DonXuat> clickAction = selectedItem =>
                {
                    txtSearch.Text = selectedItem.MaDonXuat.ToString(); // Hiện mã đơn
                    if (result != null) result.Visible = false;
                    foreach (DataGridViewRow row in dgvDonXuat.Rows)
                    {
                        if (row.DataBoundItem is DTO_DonXuat dto && dto.MaDonXuat == selectedItem.MaDonXuat)
                        {
                            row.Selected = true;
                            dgvDonXuat.CurrentCell = row.Cells[0]; // Focus vào cell đầu tiên
                            break;
                        }
                    }
                };
                SearchHelper.UpdateSearchSuggestions(result, finalResults, txtSearch, 38, 190, displayFunc, clickAction);


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm đơn xuất: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BindDataToGrid(new List<DTO_DonXuat>());
            }
        }

        private void BindDataToGrid(List<DTO_DonXuat> data)
        {
            dgvDonXuat.DataSource = null;
            if (data != null && data.Any())
            {
                var bindingSource = new BindingSource { DataSource = data };
                dgvDonXuat.DataSource = bindingSource;
            }
            else
            {
                dgvDonXuat.DataSource = null;
                ResizeColumns();
            }
        }
        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

    }
}