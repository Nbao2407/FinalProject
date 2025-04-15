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

namespace QLVT.Order
{
    public partial class PopupXuat : Form
    {
        private readonly int _maDonXuat;
        private readonly BUS_DonXuat busDonXuat = new BUS_DonXuat();
        private DTO_DonXuat _loadedHeader = null;
        private List<DTO_ChiTietDonXuat> _loadedDetails = null;
        public bool DataChanged { get; private set; } = false;

        public PopupXuat(int maDonXuat)
        {
            InitializeComponent();
            _maDonXuat = maDonXuat;
            LoadOrderData();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            Quyen();
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

        private void LoadOrderData()
        {
            if (busDonXuat == null) return; 

            this.Cursor = Cursors.WaitCursor;
            try
            {
                _loadedHeader = busDonXuat.GetDonXuatById(_maDonXuat);

                _loadedDetails = busDonXuat.GetChiTietDonXuat(_maDonXuat);

                if (_loadedHeader != null)
                {
                    lbld.Text = _loadedHeader.MaDonXuat.ToString();
                    lblNgayXuat.Text = _loadedHeader.NgayXuat.ToString("dd/MM/yyyy");
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
                    else 
                    {
                        string khoDichInfo = "";
                        if (!string.IsNullOrEmpty(_loadedHeader.GhiChu) && _loadedHeader.GhiChu.Contains("đến kho:"))
                        {
                            khoDichInfo = _loadedHeader.GhiChu.Substring(_loadedHeader.GhiChu.IndexOf("đến kho:") + 8).Split('.')[0].Trim();
                        }
                        lblKh.Text = $"Kho Đích: {khoDichInfo}"; 
                        lblHoaDonValue.Text = "[Không áp dụng]";
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn xuất.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                dgvPopupChiTiet.DataSource = null;
                if (_loadedDetails != null && _loadedDetails.Any())
                {
                    var bindingSource = new BindingSource { DataSource = _loadedDetails };
                    dgvPopupChiTiet.DataSource = bindingSource;
                }
                else
                {
                    dgvPopupChiTiet.DataSource = null;
                }

                TinhTongCongPopup();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn xuất: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Không tìm thấy form cha Frm1!");
                return;
            }
            this.Dispose();
        }

        private void Quyen()
        {
            bool isOwner = CurrentUser.MaTK == int.Parse(lbld.Text);
            bool isStaff = CurrentUser.ChucVu == "Nhân viên";
            bool isManagerOrAdmin = CurrentUser.ChucVu == "Quản lý" || CurrentUser.ChucVu == "Admin";
            bool canEdit = false;
            bool canApprove = false;
            bool canDisable = false;

            if (tranthai.Text == "Đang xử lý")
            {
                if (isStaff && isOwner)
                {
                    canEdit = true;
                }
                else if (isManagerOrAdmin)
                {
                    canEdit = true;
                    canApprove = true;
                    canDisable = true;
                }
            }
            BtnEdit.Enabled = BtnEdit.Visible = canEdit;
            btnDuyet.Visible = canApprove;
            btnDuyet.Enabled = canApprove;
            BtnDisable.Visible = canDisable;
            BtnDisable.Enabled = canDisable;
        }
    }
}