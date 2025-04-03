using BUS;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using ReaLTaiizor.Controls;

namespace GUI.VatLieu
{
    public partial class EditMaterial : Form
    {
        private byte[] hinhAnh;
        private int nguoiCapNhat = CurrentUser.MaTK;
        private BUS_VatLieu bus = new BUS_VatLieu();
        private int? maVatLieu;
        private PopupMaterial popupMaterial;
        public EditMaterial(PopupMaterial parent, int maVatLieu)
        {
            InitializeComponent();
            popupMaterial = parent;
            this.maVatLieu = maVatLieu;
            LoadDataFromDatabase();
            LoadComboBoxDonViTinh();
            LoadComboBoxLoaiVatLieu();
        }

        private void EditMaterial_Load(object sender, EventArgs e)
        {

        }
        private void LoadDataFromDatabase()
        {
            if (maVatLieu.HasValue)
            {
                DataTable dt = bus.LayVatLieuTheoMa(maVatLieu.Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    TbID.Text = row["MaVatLieu"].ToString();
                    TbName.Text = row["Ten"].ToString();

                    decimal giaNhap = Convert.ToDecimal(row["DonGiaNhap"]);

                    TbGiaNhap.Text = giaNhap.ToString("N0", new CultureInfo("vi-VN")) + " ₫";

                    int maLoai = Convert.ToInt32(row["Loai"]);
                    CbType.SelectedValue = bus.LayTenLoaiTheoMa(maLoai);

                    decimal giaban = Convert.ToDecimal(row["DonGiaBan"]);
                    TbGiaBan.Text = Convert.ToDecimal(row["DonGiaBan"]).ToString("N0", new CultureInfo("vi-VN")) + " ₫";
                    TbNote.Text = row["GhiChu"]?.ToString();
                    int maTk = Convert.ToInt32(row["NguoiTao"]);
                    if (row["HinhAnh"] != DBNull.Value)
                    {
                        byte[] hinhAnh = (byte[])row["HinhAnh"];
                        using (MemoryStream ms = new MemoryStream(hinhAnh))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vật liệu với mã này!");
                    this.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            popupMaterial.LoadDataFromDatabase();
            this.Close();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (CbType.SelectedIndex == -1 || CbDVT.SelectedIndex == -1)
                    throw new Exception("Vui lòng chọn loại vật liệu và đơn vị tính!");

                string ten = TbName.Text;
                int loai = (int)CbType.SelectedValue;
                decimal donGiaNhap = decimal.Parse(TbGiaNhap.Text);
                decimal donGiaBan = decimal.Parse(TbGiaBan.Text);
                string donViTinh = (string)CbDVT.SelectedValue;
                string ghiChu = TbNote.Text;
                byte[] hinhAnh = pictureBox1.Image != null ? ImageToByteArray(pictureBox1.Image) : null;

                bus.CapNhatVatLieu(maVatLieu.Value, ten, loai, donGiaNhap, donGiaBan, donViTinh, hinhAnh, ghiChu, 1);
                MessageBox.Show("Cập nhật vật tư thành công!");
                popupMaterial.LoadDataFromDatabase();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void LoadComboBoxLoaiVatLieu()
        {
            DataTable dt = bus.LayDanhSachLoaiVatLieu();
            CbType.DataSource = dt;
            CbType.DisplayMember = "TenLoai";
            CbType.ValueMember = "MaLoaiVatLieu";
        }
        private void LoadComboBoxDonViTinh()
        {
            DataTable dt = bus.LayDanhSachDVT();
            CbDVT.DataSource = dt;
            CbDVT.DisplayMember = "DonViTinh";
            CbDVT.ValueMember = "DonViTinh";
            TbDonvi.Text = CbDVT.Text;
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
        private void btnTaiHinhAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    hinhAnh = File.ReadAllBytes(ofd.FileName);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
