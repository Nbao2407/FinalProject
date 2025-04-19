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
using DAL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using BUS;
using QLVT.Helper;
using Microsoft.VisualBasic;

namespace QLVT.HoaDon
{
    public partial class PopupHoaDon : Form
    {
        private FrmHoaDon _parentForm;
        private DTO_HoaDon _hoaDon;
        private BUS_HoaDon hoaDonBLL = new BUS_HoaDon();
        private int? maHoaDon;
        private DAL_HoaDon dal = new DAL_HoaDon();
        public bool DataChanged { get; private set; } = false;

        public PopupHoaDon(int? maHoaDon, FrmHoaDon frmHoaDon)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.maHoaDon = maHoaDon;
            _parentForm = frmHoaDon;
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
        }


        private void PopupHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDonData();
            Quyen();
            DataGridViewHelper.CustomizeDataGridView(dataGridView);
            DataGridViewHelper.FillColumnsToDgvWidth(dataGridView);
            DataGridViewHelper.DisableColumnSorting(dataGridView);
        }


        private void LoadHoaDonData()
        {
            try
            {
                if (maHoaDon.HasValue)
                {
                    DTO_HoaDon hoaDon = hoaDonBLL.GetHoaDonById(maHoaDon.Value);
                    if (hoaDon != null)
                    {
                        lblID.Text = hoaDon.MaHoaDon.ToString();
                        TenKH.Text = hoaDon.TenKhachHang ?? "Khách lẻ";
                        Ngay.Text = hoaDon.NgayLap.ToString("dd/MM/yyyy");
                        txtNgTao.Text = hoaDon.NguoiLap;
                        lblTrangthai.Text = hoaDon.TrangThai;
                        lblTongCong.Text = hoaDon.TongTien.ToString("N0");
                    }

                    DataTable dtChiTiet = hoaDonBLL.GetChiTietHoaDon(maHoaDon.Value);
                    dataGridView.DataSource = dtChiTiet;

                    dataGridView.Columns["MaVatLieu"].HeaderText = "Mã Vật Liệu";
                    dataGridView.Columns["Ten"].HeaderText = "Tên Vật Liệu";
                    dataGridView.Columns["SoLuong"].HeaderText = "Số Lượng";
                    dataGridView.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dataGridView.Columns.Contains("MaCTHD"))
                    {
                        dataGridView.Columns.Remove("MaCTHD");
                    }
                    if (dataGridView.Columns.Contains("HinhAnh"))
                    {
                        dataGridView.Columns.Remove("HinhAnh");
                    }
                    if (hoaDon.TrangThai == "Đã hủy" || hoaDon.TrangThai == "Đã thanh toán")
                    {
                        BtnDisable.Enabled = false;
                    }
                    if (!dataGridView.Columns.Contains("ThanhTien"))
                    {
                        DataGridViewTextBoxColumn thanhTienColumn = new DataGridViewTextBoxColumn
                        {
                            Name = "ThanhTien",
                            HeaderText = "Thành Tiền",
                            Width = 120
                        };
                        dataGridView.Columns.Add(thanhTienColumn);
                    }
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                        decimal donGia = Convert.ToDecimal(row.Cells["DonGia"].Value);
                        decimal thanhTien = soLuong * donGia;
                        row.Cells["ThanhTien"].Value = thanhTien.ToString("N0");
                    }
                    int tongSoLuong = dtChiTiet.AsEnumerable().Sum(row => row.Field<int>("SoLuong"));
                    lblSoLuong.Text = tongSoLuong.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(lblID.Text);

                if (lblTrangthai.Text == "Chờ duyệt")
                {
                    using (LydoHuy formLyDo = new LydoHuy())
                    {
                        if (formLyDo.ShowDialog() == DialogResult.OK)
                        {
                            string lyDoHuy = formLyDo.LyDoHuy;
                            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn hủy hóa đơn này?\nLý do: {lyDoHuy}", "Xác nhận hủy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                string message = hoaDonBLL.CancelHoaDon((int)maHoaDon, CurrentUser.MaTK, lyDoHuy);
                                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            }
                        }
                    }
                }
                else if (lblTrangthai.Text == "Nháp")
                {
                    bool daXoa = dal.XoaHoaDonVaChiTiet(id);
                    if (daXoa)
                    {
                        MessageBox.Show("Đã xóa hóa đơn và chi tiết thành công!");
                        this.Close();
                        _parentForm.LoadData();
                    }
                    else
                        MessageBox.Show("Xóa thất bại hoặc hóa đơn không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {


            int maHoaDon = Convert.ToInt32(lblID.Text);
            Form1 frm1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (frm1 != null)
            {
                EditHoaDon editForm = new EditHoaDon(maHoaDon);
                frm1.OpenChildForm(editForm);
            }
            else
            {
                MessageBox.Show("Không tìm thấy form cha Frm1!");
                return;
            }
            this.Close();
        }

        private void hopeRoundButton1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblID.Text);
            hoaDonBLL.UpdateTHoaDon(id, "Đã thanh toán");
            MessageBox.Show($"Duyệt đơn thành công{id}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _parentForm.LoadData();

        }
        private void Quyen()
        {
            bool isOwner = CurrentUser.TenDangNhap == txtNgTao.Text;
            bool isStaff = CurrentUser.ChucVu == "Nhân viên";
            bool isManagerOrAdmin = CurrentUser.ChucVu != "Nhân viên";
            bool canEdit = false;
            bool canApprove = false;
            bool canDisable = false;
            bool canDelete = false;

            if (lblTrangthai.Text == "Chờ duyệt")
            {
                if (isStaff && isOwner)
                {
                    canEdit = true;
                    canDelete = true;
                }
                else if (isManagerOrAdmin)
                {
                    canEdit = true;
                    canApprove = true;
                    canDisable = true;
                    canDelete = true;
                }
            }
            else if (lblTrangthai.Text == "Nháp")
            {
                if (isStaff && isOwner)
                {
                    canEdit = true;
                    canDelete = true;
                    canApprove = false;
                    canDisable = false;
                }
                else if (isManagerOrAdmin)
                {
                    canEdit = true;
                    canApprove = false;
                    canDisable = false;
                    canDelete = true;

                }
            }
            else if (lblTrangthai.Text == "Đã huỷ")
            {
                if (isManagerOrAdmin)
                {
                    canEdit = true;
                    canApprove = false;
                    canDisable = true;
                }
            }
            BtnEdit.Enabled = BtnEdit.Visible = canEdit;
            btnDuyet.Visible = canApprove;
            btnDuyet.Enabled = canApprove;
            BtnDisable.Visible = canDisable;
            BtnDisable.Enabled = canDisable;
            btnDecline.Visible = canApprove;
            btnDecline.Enabled = canApprove;
        }

        private async void btnDecline_Click(object sender, EventArgs e)
        {
            int maHoaDonCanTuChoi = int.Parse(lblID.Text);
            

            int maNguoiThucHien = CurrentUser.MaTK; 
            
            DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn Từ chối/Hủy Hóa Đơn có mã {maHoaDonCanTuChoi}?",
                                                   "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor; 
                btnDecline.Enabled = false;

                try
                {
                    var result = await hoaDonBLL.TuChoiHoaDonAsync(maHoaDonCanTuChoi, maNguoiThucHien);

                    if (result.Success)
                    {
                        MessageBox.Show("Từ chối/Hủy Hóa Đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Thao tác thất bại:\n{result.ErrorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Đã xảy ra lỗi trong quá trình xử lý:\n{ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default; 
                    btnDecline.Enabled = true; 
                }
            } 
        }
    }
}