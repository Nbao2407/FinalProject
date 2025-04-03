using BUS;
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

namespace GUI.VatLieu
{
    public partial class AddMaterial : Form
    {
        private byte[] hinhAnh;
        private int nguoiCapNhat = CurrentUser.MaTK;
        private BUS_VatLieu bus = new BUS_VatLieu();
        private FrmMaterial frmMaterial;
        public AddMaterial(FrmMaterial parent)
        {
            InitializeComponent();
            frmMaterial = parent;
            LoadComboBoxDonViTinh();
            LoadComboBoxLoaiVatLieu();
            LoadComboBoxKho();
        }

        private void AddMaterial_Load(object sender, EventArgs e)
        {
    

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            int nguoiTao = CurrentUser.MaTK;

            try
            {
                string ten = TbName.Text;
                int loai = CbType.SelectedValue != null ? (int)CbType.SelectedValue : -1;
                decimal donGiaNhap = decimal.Parse(TbGiaNhap.Text);
                decimal donGiaBan = decimal.Parse(TbGiaBan.Text);
                string donViTinh = (string)aloneComboBox1.SelectedValue;
                TbDonvi.Text = donViTinh;
                int maKho = (int)CbKho.SelectedValue;
                string ghiChu = TbAddress.Text;
                byte[] hinhAnh = this.hinhAnh;
                if (string.IsNullOrEmpty(TbName.Text) || string.IsNullOrEmpty(TbGiaNhap.Text) || string.IsNullOrEmpty(TbGiaBan.Text) || string.IsNullOrEmpty(TbDonvi.Text) || string.IsNullOrEmpty(TbAddress.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }
                if (CbType.SelectedIndex == -1 || aloneComboBox1.SelectedIndex == -1 || CbKho.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ thông tin!");
                    return;
                }
                if (hinhAnh == null)
                {
                    MessageBox.Show("Vui lòng chọn hình ảnh Vật liệu!");
                    return;
                }
                

                bus.ThemVatLieu(ten, loai, donGiaNhap, donGiaBan, donViTinh, maKho, hinhAnh, ghiChu,nguoiTao);
                MessageBox.Show("Thêm vật tư thành công!");
                this.Close();
               frmMaterial.LoadData();
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
            CbType.SelectedIndex = -1;
        }
        private void LoadComboBoxDonViTinh()
        {
            DataTable dt = bus.LayDanhSachDVT();
            aloneComboBox1.DataSource = dt;
            aloneComboBox1.DisplayMember = "DonViTinh";
            aloneComboBox1.ValueMember = "DonViTinh";
            aloneComboBox1.SelectedIndex = -1;
        }
        private void LoadComboBoxKho()
        {
            DataTable dt = bus.LayDanhSachKho();
            CbKho.DataSource = dt;
            CbKho.DisplayMember = "TenKho";
            CbKho.ValueMember = "MaKho";
            aloneComboBox1.SelectedIndex = -1;
        }

        private void aloneComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
