using BUS;
using DTO;
using QLVT.Helper;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
namespace QLVT.VatLieu
{
    public partial class PopupMaterial : Form
    {
        FrmMaterial frmMaterial;
        private int NguoiCapNhat = CurrentUser.MaTK;
        private byte[] hinhAnh;
        private int? maVatLieu;
        private BUS_VatLieu bus = new BUS_VatLieu();
        private FrmMaterial FrmMaterial;

        public PopupMaterial(FrmMaterial parent, int maVatLieu)
        {
            InitializeComponent();
            frmMaterial = parent;
            this.maVatLieu = maVatLieu;
            LoadDataFromDatabase();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            Quyen();
        }
        public void LoadDataFromDatabase()
        {
            if (maVatLieu.HasValue)
            {
                DataTable dt = bus.LayVatLieuTheoMa(maVatLieu.Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblID.Text = row["MaVatLieu"].ToString();
                    lblName.Text = row["Ten"].ToString();

                    decimal giaNhap = Convert.ToDecimal(row["DonGiaNhap"]);

                    TbGiaNhap.Text = giaNhap.ToString("N0", new CultureInfo("vi-VN"));

                    int maLoai = Convert.ToInt32(row["Loai"]);
                    lblType.Text = bus.LayTenLoaiTheoMa(maLoai);

                    decimal giaban = Convert.ToDecimal(row["DonGiaBan"]);
                    TbGiaBan.Text = Convert.ToDecimal(row["DonGiaBan"]).ToString("N0", new CultureInfo("vi-VN"));

                    int maKho = Convert.ToInt32(row["MaKho"]);
                    lblKho.Text = bus.LayTenKhoTheoMa(maKho);
                    TbNote.Text = row["GhiChu"]?.ToString();
                    int maTk = Convert.ToInt32(row["NguoiTao"]);
                    txtNgTao.Text = bus.LayTenNgTaoTheoMa(maTk);
                    dtpNgayTao.Text = row["NgayTao"].ToString();
                    Ngay.Text = dtpNgayTao.Value.ToString("dd/MM/yyyy");
                    if (row["HinhAnh"] != DBNull.Value)
                    {
                        byte[] hinhAnh = (byte[])row["HinhAnh"];
                        using (MemoryStream ms = new MemoryStream(hinhAnh))
                        {
                            pictureBox2.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBox2.Image = null;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vật liệu với mã này!");
                    this.Close();
                }
            }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            using (var edit = new EditMaterial(this, maVatLieu.Value))
            {

                edit.Deactivate += (s, e) => edit.TopMost = true;
                edit.StartPosition = FormStartPosition.CenterParent;
                edit.ShowDialog();
            }
        }

        private void roundedPictureBox2_Click(object sender, EventArgs e)
        {
            frmMaterial.LoadData();
            this.Close();

        }

        private void txtNgTao_Click(object sender, EventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn xóa vật liệu này không?", "Xóa vật liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bus.XoaVatLieu(maVatLieu.Value,CurrentUser.MaTK);
                    MessageBox.Show("Xóa vật liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmMaterial.LoadData();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa vật liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Quyen()
        {
            bool isOwner = CurrentUser.TenDangNhap == txtNgTao.Text;
            bool isStaff = CurrentUser.ChucVu == "Nhân viên";
            bool isManagerOrAdmin = CurrentUser.ChucVu != "Nhân viên";
            bool canEdit = false;
            bool canDelete = false;
            if (isStaff)
            {
                canDelete = false;
                canEdit = false;
            }
            else 
            {
                canDelete = true;
                canEdit = true;
            }
            BtnEdit.Enabled = BtnEdit.Visible = canEdit;
            BtnDelete.Visible = canDelete;
            BtnDelete.Enabled = canDelete;
        }
    }
}
