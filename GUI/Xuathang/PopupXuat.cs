using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using System.Globalization;
using DAL;
using static System.Runtime.InteropServices.JavaScript.JSType;
using QLVT.Helper;
using Microsoft.VisualBasic;

namespace QLVT.Order
{
    public partial class PopupXuat : Form
    {
        private readonly int _maDonXuat;
        private readonly BUS_DonXuat busDonXuat = new BUS_DonXuat();
        private DTO_DonXuat _loadedHeader = null;
        private FrmXuat _parrentfrm;
        private List<DTO_ChiTietDonXuat> _loadedDetails = null;
        public bool DataChanged { get; private set; } = false;
        private const string STATUS_PENDING = "Chờ duyệt";
        private const string STATUS_ACTIVE = "Hoàn thành";
        private const string STATUS_REJECTED = "Từ chối";
        public PopupXuat(int maDonXuat, FrmXuat frmXuat)
        {
            InitializeComponent();
            _maDonXuat = maDonXuat;
            _parrentfrm = frmXuat;
            LoadOrderData();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            UpdateButtonStates(tranthai.Text);
            ConfigureDetailsGrid();
        }


        private void TinhTongCongPopup()
        {
            if (_loadedDetails == null) return;
            int tongSL = _loadedDetails.Sum(item => item.SoLuong);
            decimal tongTien = _loadedDetails.Sum(item => item.ThanhTien);
            lblSoLuong.Text = tongSL.ToString("N0");
            lblTongCong.Text = tongTien.ToString("N0") + " VNĐ";
        }

        private void PopupOrder_Load(object sender, EventArgs e)
        {
            dgvPopupChiTiet.AutoGenerateColumns = false;
            dgvPopupChiTiet.Columns.Clear();

            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaVatLieu",
                HeaderText = "Mã",
                DataPropertyName = "MaVatLieu",
                ReadOnly = true,
                ValueType = typeof(int),
                Width = 80
            });

            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenVatLieu",
                HeaderText = "Tên",
                DataPropertyName = "TenVatLieu",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Kho",
                HeaderText = "Kho",
                DataPropertyName = "Kho",
                ReadOnly = true,
                Visible = false
            });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonViTinh",
                HeaderText = "Đơn Vị",
                DataPropertyName = "DonViTinh",
                ReadOnly = true,
                Width = 80
            });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong",
                HeaderText = "Số Lượng",
                DataPropertyName = "SoLuong",
                ReadOnly = true,
                ValueType = typeof(int),
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaBan",
                HeaderText = "Giá bán",
                DataPropertyName = "GiaBan",
                ReadOnly = true,
                ValueType = typeof(decimal),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 110
            });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành Tiền",
                DataPropertyName = "ThanhTien",
                ReadOnly = true,
                ValueType = typeof(decimal),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                Width = 120
            });

            ConfigureDgvChiTietStyles();
        }

        private void ConfigureDgvChiTietStyles()
        {
            dgvPopupChiTiet.BorderStyle = BorderStyle.None;
            dgvPopupChiTiet.GridColor = Color.Gainsboro;
            dgvPopupChiTiet.RowHeadersVisible = false;
            dgvPopupChiTiet.BackgroundColor = Color.White;
            dgvPopupChiTiet.EnableHeadersVisualStyles = false;
            dgvPopupChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvPopupChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(66, 139, 202);
            dgvPopupChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPopupChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPopupChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPopupChiTiet.ColumnHeadersHeight = 40;
            dgvPopupChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPopupChiTiet.DefaultCellStyle.BackColor = Color.FromArgb(217, 237, 247);
            dgvPopupChiTiet.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvPopupChiTiet.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvPopupChiTiet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPopupChiTiet.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            dgvPopupChiTiet.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvPopupChiTiet.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvPopupChiTiet.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgvPopupChiTiet.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dgvPopupChiTiet.DefaultCellStyle.SelectionForeColor = Color.White;
            if (dgvPopupChiTiet.Columns.Contains("MaVatLieu"))
            {
                dgvPopupChiTiet.Columns["MaVatLieu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvPopupChiTiet.Columns.Contains("SoLuong"))
            {
                dgvPopupChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPopupChiTiet.Columns["SoLuong"].DefaultCellStyle.Format = "N0";
            }
            if (dgvPopupChiTiet.Columns.Contains("GiaBan"))
            {
                dgvPopupChiTiet.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvPopupChiTiet.RowTemplate.Height = 35;
        }

        private void LoadOrderData() // Đổi tên từ LoadOrderDataForEditing nếu cần
        {
            if (busDonXuat == null)
            {
                MessageBox.Show("Lỗi: Chưa khởi tạo đối tượng BUS.", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_maDonXuat <= 0) // Giả sử _maDonXuat đã được gán giá trị trước khi gọi hàm này
            {
                MessageBox.Show("Mã đơn xuất không hợp lệ.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            this.Cursor = Cursors.WaitCursor;
            try
            {
                _loadedHeader = busDonXuat.GetDonXuatById(_maDonXuat);
                if (_loadedHeader != null)
                {
                    lbld.Text = _loadedHeader.MaDonXuat.ToString();
                    lblNgayXuat.Text = _loadedHeader.NgayXuat.ToString("dd/MM/yyyy"); // Giả sử NgayXuat là DateOnly
                    lblLoaiXuat.Text = _loadedHeader.LoaiXuat;
                    txtNgTao.Text = _loadedHeader.TenNguoiLap ?? "N/A";
                    tranthai.Text = _loadedHeader.TrangThai;
                    txtGhiChuPopup.Text = _loadedHeader.GhiChu ?? "";

                    if (_loadedHeader.LoaiXuat == "Xuất hàng")
                    {
                        lblKh.Text = !string.IsNullOrEmpty(_loadedHeader.TenKhachHang)
                                               ? $"{_loadedHeader.TenKhachHang} (ID: {_loadedHeader.MaKhachHang?.ToString() ?? "N/A"})"
                                               : "[Không có]";
                        lblHoaDonValue.Text = _loadedHeader.MaHoaDon?.ToString() ?? "[Không có]";
                    }
                    else if (_loadedHeader.LoaiXuat == "Chuyển kho")
                    {
                        string tenKhoDich = "[Chưa xác định]";
                        if (_loadedHeader.MaKhoDich.HasValue)
                        {
                            tenKhoDich = $"ID: {_loadedHeader.MaKhoDich.Value}"; // Tạm hiển thị ID
                        }
                        lblKh.Text = $"Kho Đích: {tenKhoDich}";
                        lblHoaDonValue.Text = "[Không áp dụng]";
                    }
                    else
                    {
                        lblKh.Text = "[Không xác định]";
                        lblHoaDonValue.Text = "[Không xác định]";
                    }

                    dgvPopupChiTiet.DataSource = null; // Xóa nguồn cũ

                    if (_loadedHeader.ChitietDonXuat != null && _loadedHeader.ChitietDonXuat.Any())
                    {
                        var bindingSource = new BindingSource { DataSource = _loadedHeader.ChitietDonXuat };
                        dgvPopupChiTiet.DataSource = bindingSource;
                        Console.WriteLine($"[LoadOrderData] Loaded {_loadedHeader.ChitietDonXuat.Count} detail items to DGV.");
                    }
                    else
                    {
                        dgvPopupChiTiet.DataSource = null;
                        Console.WriteLine("[LoadOrderData] No detail items found to load to DGV.");
                    }

                    TinhTongCongPopup();

                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn xuất.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn xuất: {ex.Message}\n{ex.ToString()}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error); // Thêm ex.ToString()
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void ConfigureDetailsGrid()
        {
            if (dgvPopupChiTiet == null) return;
            dgvPopupChiTiet.AutoGenerateColumns = false;
            dgvPopupChiTiet.Columns.Clear();
            dgvPopupChiTiet.ReadOnly = true;
            dgvPopupChiTiet.AllowUserToAddRows = false;

            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_MaVL", HeaderText = "Mã VL", DataPropertyName = "MaVatLieu", Width = 60 });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_TenVL", HeaderText = "Tên Vật Liệu", DataPropertyName = "TenVatLieu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_DVT", HeaderText = "ĐVT", DataPropertyName = "DonViTinh", Width = 70 });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_SoLuong", HeaderText = "Số Lượng", DataPropertyName = "SoLuong", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_DonGia", HeaderText = "Đơn Giá", DataPropertyName = "DonGia", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvPopupChiTiet.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCT_ThanhTien", HeaderText = "Thành Tiền", DataPropertyName = "ThanhTien", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });

            if (typeof(DataGridViewHelper) != null)
            {
                DataGridViewHelper.CustomizeDataGridView(dgvPopupChiTiet);
            }
        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int id = int.Parse(lbld.Text);
            Form1 frm1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (frm1 != null)
            {
                EditXuat editForm = new EditXuat(this, id);
                frm1.OpenChildForm(editForm);
                this.Close();
            }
            else
            {
                MessageBox.Show("Không tìm thấy form cha Frm1!");
                return;
            }
            this.Close();
        }

        private void UpdateButtonStates(string currentStatus)
        {
            bool isManagerOrAdmin = CurrentUser.ChucVu == "Admin" || CurrentUser.ChucVu == "Quản lý";

            bool canEdit = false;
            bool canApprove = false;
            bool canReject = false;
            bool canToggleLock = false;
            string toggleButtonText = "Huỷ";

            if (isManagerOrAdmin)
            {
                switch (currentStatus)
                {
                    case STATUS_PENDING:
                        canApprove = true;
                        canReject = true;
                        canEdit = true;
                        toggleButtonText = "Huỷ";
                        break;

                    case STATUS_ACTIVE:
                        canApprove = false;
                        canReject = false;
                        canEdit = false;
                        break;
                    case STATUS_REJECTED:
                        canApprove = false;
                        canReject = false;
                        canEdit = true;
                        toggleButtonText = "Huỷ";
                        break;

                    default:
                        canEdit = false;
                        canApprove = false;
                        canReject = false;
                        toggleButtonText = "Huỷ";
                        break;
                }
            }
            else
            {
                canEdit = false;
                canApprove = false;
                canReject = false;
                canToggleLock = false;
            }

            BtnEdit.Enabled = canEdit;
            BtnEdit.Visible = canEdit;
            btnDuyet.Enabled = canApprove;
            btnDuyet.Visible = canApprove;
            btnDecine.Enabled = canReject;
            btnDecine.Visible = canReject;
            BtnDisable.Enabled = canToggleLock;
            BtnDisable.Visible = canToggleLock;
            BtnDisable.Text = toggleButtonText;
        }
        private void btnDuyet_Click(object sender, EventArgs e)
        {
            int id = int.Parse(lbld.Text);
            try
            {
                bool da = busDonXuat.DuyeDonXuat(_maDonXuat, CurrentUser.MaTK, txtGhiChuPopup.Text);
                if (da)
                {
                    MessageBox.Show("Duyệt đơn thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    _parrentfrm.LoadAllDonXuat();
                }
                else { MessageBox.Show("Duyệt đơn thất bại"); }
            }
            catch
            {
                MessageBox.Show("Lỗi", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async void btnDecine_Click(object sender, EventArgs e)
        {
            int maDonXuatCanTuChoi = Convert.ToInt32(lbld.Text);
            int maNguoiTuChoi = CurrentUser.MaTK;
            string errorMessage;
            bool success = await busDonXuat.TuChoiDonXuatAsync(maDonXuatCanTuChoi, maNguoiTuChoi);

            if (success)
            {
                MessageBox.Show("Từ chối Đơn Xuất Hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _parrentfrm.LoadAllDonXuat();
            }
            else
            {
                MessageBox.Show($"Từ chối Đơn Xuất Hàng thất bại:\n", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}