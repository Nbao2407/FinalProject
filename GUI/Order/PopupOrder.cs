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
using QLVT.Helper;
namespace QLVT.Order
{
    public partial class PopupOrder : Form
    {
        private int? _orderId;
        private BUS_Order _busOrder = new BUS_Order();
        public bool DataChanged { get; private set; } = false;
        private int tongSoLuong = 0;
        private decimal tongGiaTri = 0;
        public PopupOrder(int? orderid)
        {
            InitializeComponent();
            _orderId = orderid;
            LoadOrderDetails();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            Quyen();
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
                Name = "GiaNhap",
                HeaderText = "Giá Nhập",
                DataPropertyName = "GiaNhap",
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
            if (dgvPopupChiTiet.Columns.Contains("GiaNhap"))
            {
                dgvPopupChiTiet.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvPopupChiTiet.RowTemplate.Height = 35;
        }
        private void LoadOrderDetails()
        {
            if (_orderId <= 0)
            {
                MessageBox.Show("Mã đơn hàng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            try
            {
                var orderDetails = _busOrder.GetOrderDetailById(_orderId);

                if (orderDetails != null)
                {
                    lbld.Text = orderDetails.MaDonNhap.ToString();
                    NgayTao.Text = orderDetails.NgayTao.ToString("dd/MM/yyyy");
                    NgNhap.Text = orderDetails.NguoiCapNhatTen;
                    lblNCC.Text = orderDetails.TenNCC;
                    lblNgayNhap.Text = orderDetails.NgayCapNhat.ToString("dd/MM/yyyy");
                    tranthai.Text = orderDetails.TrangThai;
                    txtGhiChuPopup.Text = orderDetails.GhiChu;
                    txtNgTao.Text = orderDetails.NguoiTao;
                    dgvPopupChiTiet.AutoGenerateColumns = false;
                    dgvPopupChiTiet.DataSource = null;
                    dgvPopupChiTiet.DataSource = orderDetails.ChiTietDonNhap;
                    if (orderDetails.ChiTietDonNhap != null)
                    {
                        foreach (var itemChiTiet in orderDetails.ChiTietDonNhap)
                        {
                            tongSoLuong += itemChiTiet.SoLuong;
                            tongGiaTri += itemChiTiet.ThanhTien;
                        }
                    }
                    lblSoLuong.Text = tongSoLuong.ToString("N0");
                    lblTongCong.Text = tongGiaTri.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
                }
                else
                {
                    MessageBox.Show("Không tìm thấy chi tiết đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết đơn hàng: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
                EditNhap editForm = new EditNhap(this, id);
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
