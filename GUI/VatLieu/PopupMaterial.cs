using BUS;
using DAL;
using DTO;
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
using static IronPython.Modules._ast;
using System.Globalization;
namespace GUI.VatLieu
{
    public partial class PopupMaterial : Form
    {
        FrmMaterial frmMaterial;
        private int NguoiCapNhat = CurrentUser.MaTK;
        private byte[] hinhAnh;
        private int? maVatLieu;
        private BUS_VatLieu bus = new BUS_VatLieu();

        public PopupMaterial(FrmMaterial parent, int maVatLieu)
        {
            InitializeComponent();
            frmMaterial = parent;
            this.maVatLieu = maVatLieu;
            LoadDataFromDatabase();
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

                    TbGiaNhap.Text = giaNhap.ToString("N0", new CultureInfo("vi-VN")) + " ₫";

                    int maLoai = Convert.ToInt32(row["Loai"]);
                    lblType.Text = bus.LayTenLoaiTheoMa(maLoai);

                    decimal giaban = Convert.ToDecimal(row["DonGiaBan"]);
                    TbGiaBan.Text = Convert.ToDecimal(row["DonGiaBan"]).ToString("N0", new CultureInfo("vi-VN")) + " ₫";

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
            this.Close();
        }

        private void txtNgTao_Click(object sender, EventArgs e)
        {

        }
    }
}
