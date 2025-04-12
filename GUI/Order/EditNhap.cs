using BUS;
using DAL;
using DTO;
using Microsoft.VisualBasic;
using QLVT.HoaDon;
using QLVT.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class EditNhap : Form
    {
        private int? _orderId;
        private PopupOrder _popupOrder;
        private readonly BUS_Order _busOrder = new BUS_Order();
        private readonly BUS_Ncc _busNcc = new BUS_Ncc();
        private readonly BUS_TK _busTk = new BUS_TK();
        private DTO_OrderDetail _originalOrderData;

        public EditNhap(PopupOrder popupOrder, int? orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            _popupOrder = popupOrder;
            LoadComboBoxes();
            LoadOrderDataForEditing();
            SetupEditDataGridView();
            ConfigureDgvChiTietStyles();
            CalculateAndDisplayTotals();
            dgvChiTietEdit.CellClick += DgvChiTiet_CellClick;
        }

        private void LoadComboBoxes()
        {
            try
            {
                if (CurrentUser.ChucVu == "Admin" || CurrentUser.ChucVu == "Quản lý")
                {
                    CbNgNhapEdit.Enabled = true;
                    CbNgNhapEdit.DataSource = _busTk.LayDSNgNhap();
                    CbNgNhapEdit.DisplayMember = "TenDangNhap";
                    CbNgNhapEdit.ValueMember = "MaTK";
                    CbNgNhapEdit.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else
                {
                    CbNgNhapEdit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu cho ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SetupEditDataGridView()
        {
            dgvChiTietEdit.AutoGenerateColumns = false;
            dgvChiTietEdit.Columns.Clear();
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaVatLieu", HeaderText = "Mã", DataPropertyName = "MaVatLieu", ReadOnly = true, Width = 80 });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenVatLieu", HeaderText = "Tên", DataPropertyName = "TenVatLieu", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonViTinh", HeaderText = "ĐVT", DataPropertyName = "DonViTinh", ReadOnly = true, Width = 80 });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng", DataPropertyName = "SoLuong", ReadOnly = false, ValueType = typeof(int), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, Width = 90 });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaNhap", HeaderText = "Giá Nhập", DataPropertyName = "GiaNhap", ReadOnly = false, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, Width = 110 });
            dgvChiTietEdit.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền", DataPropertyName = "ThanhTien", ReadOnly = true, ValueType = typeof(decimal), DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, Width = 120 });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn { Name = "Tang", HeaderText = "Tăng", Text = "+", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTietEdit.Columns.Add(new DataGridViewButtonColumn { Name = "Giam", HeaderText = "Giảm", Text = "-", UseColumnTextForButtonValue = true, Width = 80, FlatStyle = FlatStyle.System });
            dgvChiTietEdit.CellContentClick += DgvChiTietEdit_CellContentClick;
            dgvChiTietEdit.CellValueChanged += DgvChiTietEdit_CellValueChanged;
            dgvChiTietEdit.RowsRemoved += DgvChiTietEdit_RowsRemoved;
            ResizeColumns();
        }

        private void ConfigureDgvChiTietStyles()
        {
            dgvChiTietEdit.BorderStyle = BorderStyle.None;
            dgvChiTietEdit.GridColor = Color.Gainsboro;
            dgvChiTietEdit.RowHeadersVisible = false;
            dgvChiTietEdit.BackgroundColor = Color.White;
            dgvChiTietEdit.EnableHeadersVisualStyles = false;
            dgvChiTietEdit.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(66, 139, 202);
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTietEdit.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTietEdit.ColumnHeadersHeight = 40;
            dgvChiTietEdit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTietEdit.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
            dgvChiTietEdit.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTietEdit.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvChiTietEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvChiTietEdit.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvChiTietEdit.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvChiTietEdit.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvChiTietEdit.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgvChiTietEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dgvChiTietEdit.DefaultCellStyle.SelectionForeColor = Color.White;
            if (dgvChiTietEdit.Columns.Contains("MaVatLieu"))
            {
                dgvChiTietEdit.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvChiTietEdit.Columns.Contains("SoLuong"))
            {
                dgvChiTietEdit.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTietEdit.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
            }
            if (dgvChiTietEdit.Columns.Contains("GiaNhap"))
            {
                dgvChiTietEdit.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvChiTietEdit.RowTemplate.Height = 35;
        }

        private void ResizeColumns()
        {
            if (dgvChiTietEdit.Columns.Count == 0 || dgvChiTietEdit.ClientSize.Width <= 0) return;
            try
            {
                int availableWidth = dgvChiTietEdit.ClientSize.Width;
                if (dgvChiTietEdit.Controls.OfType<VScrollBar>().Any(v => v.Visible))
                {
                    availableWidth -= SystemInformation.VerticalScrollBarWidth;
                }
                int fixedWidthTotal = 0;
                int fillColumnsCount = 0;
                Dictionary<string, int> fixedWidths = new Dictionary<string, int>
         {
             { "MaVatLieu", 80 },
             { "DonViTinh", 80 },
             { "SoLuong", 90 },
             { "GiaNhap", 100 },
             { "ThanhTien", 120 },
             { "Tang", 60 },
             { "Giam", 60 },
         };
                foreach (DataGridViewColumn col in dgvChiTietEdit.Columns)
                {
                    if (!col.Visible) continue;
                    if (fixedWidths.ContainsKey(col.Name))
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        col.Width = Math.Max(20, Math.Min(fixedWidths[col.Name], availableWidth - fixedWidthTotal - 50));
                        fixedWidthTotal += col.Width;
                    }
                    else if (col.Name == "TenVatLieu")
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        fillColumnsCount++;
                    }
                    else
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    }
                }
                if (dgvChiTietEdit.Columns.Contains("TenVatLieu"))
                {
                    dgvChiTietEdit.Columns["TenVatLieu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvChiTietEdit.Columns["TenVatLieu"].MinimumWidth = 150;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during column resize: {ex.Message}");
            }
        }
        private void LoadOrderDataForEditing()
        {
            if (_orderId <= 0) return;
            try
            {
                _originalOrderData = _busOrder.GetOrderDetailById(_orderId);

                if (_originalOrderData != null)
                {
                    TbNcc.Text = _originalOrderData.TenNCC;
                    ID.Text = _originalOrderData.MaDonNhap.ToString();
                    dtpNgayNhap.Value = _originalOrderData.NgayNhap;
                    dtpNgayNhap.MaxDate = DateTime.Now;
                    if (CbNgNhapEdit.Items.Count > 0 && CurrentUser.ChucVu != "Nhân viên")
                    {
                        CbNgNhapEdit.SelectedValue = _originalOrderData.MaTK;
                        if (CbNgNhapEdit.SelectedValue == null || (int)CbNgNhapEdit.SelectedValue != _originalOrderData.MaTK)
                        {
                            CbNgNhapEdit.Text = _originalOrderData.NguoiTao ?? "N/A";
                            CbNgNhapEdit.Enabled = false;
                        }
                    }
                    else
                    {
                        CbNgNhapEdit.Text = _originalOrderData.NguoiTao ?? "N/A";
                        CbNgNhapEdit.Enabled = false;
                    }

                    Tbnote.Text = _originalOrderData.GhiChu;

                    lblNgTao.Text = _originalOrderData.NguoiTao ?? "N/A";

                    dgvChiTietEdit.DataSource = null;
                    dgvChiTietEdit.DataSource = _originalOrderData.ChiTietDonNhap;

                    CalculateAndDisplayTotals();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn hàng để sửa.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn hàng để sửa: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void CalculateAndDisplayTotals()
        {
            int tongSoLuong = 0;
            decimal tongGiaTri = 0;

            if (dgvChiTietEdit.DataSource is List<DTO_ChiTietDonNhap> currentDetails)
            {
                foreach (var item in currentDetails)
                {
                    tongSoLuong += item.SoLuong;
                    tongGiaTri += item.SoLuong * item.GiaNhap; // Tính lại
                }
            }
            // Hoặc lấy từ các ô trong grid (chậm hơn nhưng chính xác nếu chưa cập nhật DataSource)
            /* else {
                foreach (DataGridViewRow row in dgvChiTietEdit.Rows)
                {
                    if (row.IsNewRow) continue;
                    int sl = 0;
                    decimal gn = 0;
                    int.TryParse(row.Cells["SoLuong"].Value?.ToString() ?? "0", out sl);
                    decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString() ?? "0", NumberStyles.Any, CultureInfo.CurrentCulture, out gn);
                    tongSoLuong += sl;
                    tongGiaTri += sl * gn;
                }
            } */
            lblSl.Text = tongSoLuong.ToString("N0");
            lblTongTien.Text = tongGiaTri.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }

        private void DgvChiTietEdit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (dgvChiTietEdit.Columns[e.ColumnIndex].Name == "SoLuong" || dgvChiTietEdit.Columns[e.ColumnIndex].Name == "GiaNhap"))
            {
                DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
                if (row.Cells["SoLuong"].Value != null && row.Cells["GiaNhap"].Value != null)
                {
                    int sl = 0;
                    decimal gn = 0;
                    int.TryParse(row.Cells["SoLuong"].Value.ToString(), out sl);
                    decimal.TryParse(row.Cells["GiaNhap"].Value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out gn);
                    if (dgvChiTietEdit.Columns.Contains("ThanhTien"))
                    {
                        row.Cells["ThanhTien"].Value = sl * gn;
                    }
                }
                CalculateAndDisplayTotals();
            }
        }
        private void DgvChiTietEdit_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalculateAndDisplayTotals();
        }
        private void DgvChiTietEdit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvChiTietEdit.Columns[e.ColumnIndex].Name == "colDeleteDetail")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvChiTietEdit.DataSource is List<DTO_ChiTietDonNhap> detailsList)
                    {
                        if (dgvChiTietEdit.Rows[e.RowIndex].DataBoundItem is DTO_ChiTietDonNhap itemToRemove)
                        {
                            BindingSource bs = new BindingSource();
                            bs.DataSource = detailsList;
                            bs.Remove(itemToRemove);
                            dgvChiTietEdit.DataSource = bs.List;
                        }
                    }
                    else
                    {
                        if (!dgvChiTietEdit.Rows[e.RowIndex].IsNewRow)
                        {
                            dgvChiTietEdit.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Control parentControl = this.Parent;
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this);
                this.Dispose();

                FrmXuat nhap = new FrmXuat()
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
                FrmXuat nhap = new FrmXuat();
                nhap.Show();
            }
        }

        private void dgvChiTietEdit_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void btnNhapHang_Click(object sender, EventArgs e)
        {
            DateTime ngayNhapMoi = dtpNgayNhap.Value.Date;
            string ghiChuMoi = Tbnote.Text.Trim();
            int maTkNguoiNhap = _originalOrderData.MaTK; 
            if (CbNgNhapEdit.Enabled && CbNgNhapEdit.SelectedValue != null && CurrentUser.ChucVu !="Nhân viên")
            {
                maTkNguoiNhap = (int)CbNgNhapEdit.SelectedValue;
            }
            List<DTO_ChiTietDonNhap> chiTietDaSua = new List<DTO_ChiTietDonNhap>();
            foreach (DataGridViewRow row in dgvChiTietEdit.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.DataBoundItem is DTO_ChiTietDonNhap originalDetail)
                {
                    DTO_ChiTietDonNhap editedDetail = new DTO_ChiTietDonNhap
                    {
                        MaDonNhap = int.Parse(ID.Text),
                        MaVatLieu = originalDetail.MaVatLieu,
                        SoLuong = int.TryParse(row.Cells["SoLuong"].Value?.ToString() ?? "0", out int sl) ? sl : 0,
                        GiaNhap = decimal.TryParse(row.Cells["GiaNhap"].Value?.ToString() ?? "0", NumberStyles.Any, CultureInfo.CurrentCulture, out decimal gn) ? gn : 0m
                        // Lấy thêm TenVatLieu, DonViTinh nếu cần truyền đi đâu đó, nhưng không cần cho SP này
                    };
                    chiTietDaSua.Add(editedDetail);
                }
            }

            if (!chiTietDaSua.Any()) { /*...*/ return; }
            foreach (var detail in chiTietDaSua) { if (detail.SoLuong <= 0 || detail.GiaNhap < 0) { /*...*/ return; } }


            bool success = false;
            try
            {
                DTO_Order headerUpdate = new DTO_Order
                {
                    MaDonNhap = int.Parse(ID.Text),
                    NgayNhap = ngayNhapMoi, 
                    GhiChu = ghiChuMoi,
                    NguoiNhap = maTkNguoiNhap, 
                    MaNCC = _originalOrderData.MaNCC 
                };

                btnNhapHang.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                success = await _busOrder.UpdateOrderAsync(headerUpdate, chiTietDaSua, CurrentUser.MaTK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi:\n{ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = false;
            }
            finally
            {
                btnNhapHang.Enabled = true;
                this.Cursor = Cursors.Default;
            }

            if (success)
            {
                MessageBox.Show("Lưu thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Control parentControl = this.Parent;
                if (parentControl != null)
                {
                    parentControl.Controls.Remove(this);
                    this.Dispose();

                    FrmXuat nhap = new FrmXuat()
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
                    FrmXuat nhap = new FrmXuat();
                    nhap.Show();
                }
            }
        }
        private void DgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvChiTietEdit.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DataGridViewRow row = dgvChiTietEdit.Rows[e.RowIndex];
                if (row.IsNewRow) return;
                string columnName = dgvChiTietEdit.Columns[e.ColumnIndex].Name;
                int currentSoLuong = 0;
                int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out currentSoLuong);
                switch (columnName)
                {
                    case "Tang":
                        row.Cells["SoLuong"].Value = Math.Max(1, currentSoLuong + 1);
                        break;
                    case "Giam":
                        if (currentSoLuong > 1)
                        {
                            row.Cells["SoLuong"].Value = currentSoLuong - 1;
                        }
                        break;
                }
                if (columnName == "Tang" || columnName == "Giam")
                {
                    CalculateAndDisplayTotals();
                }
            }
        }


    }
}
