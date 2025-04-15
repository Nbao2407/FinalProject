using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.VatLieu
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
            LoadDonViTinhComboBox();
            LoadComboBoxLoaiVatLieu();
            LoadComboBoxKho();
            comboBoxDonViTinh.MaxDropDownItems = 12;
            comboBoxDonViTinh.DropDownHeight = 200; 
            CbType.MaxDropDownItems = 10;
            CbKho.MaxDropDownItems = 10;
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
        private void LoadDonViTinhComboBox()
        {
            List<string> donViTinhList = new List<string>
            {
                "m³ (Mét khối)",
                "Xe",
                "Tấn",
                "Bao",
                "Kg (Kilôgam)",
                "Viên",
                "Pallet",
                "Cây / Thanh",
                "Cuộn",
                "Mét (m)",
                "Tấm",
                "Mét dài (md)",
                "Ống",
                "m² (Mét vuông)",
                "Hộp",
                "Thùng",
                "Lon",
                "Xô",
                "Lít",
                "Bộ",
                "Cái",
            };
            List<string> distinctUnits = new List<string>();
            foreach (string unit in donViTinhList)
            {
                if (!distinctUnits.Contains(unit))
                {
                    distinctUnits.Add(unit);
                }
            }
            distinctUnits.Sort();
            comboBoxDonViTinh.Items.Clear();
            comboBoxDonViTinh.Items.AddRange(distinctUnits.ToArray());
            comboBoxDonViTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDonViTinh.SelectedIndex = -1;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int nguoiTao = CurrentUser.MaTK;

            try
            {
                string ten = TbName.Text;
                int loai = CbType.SelectedValue != null ? (int)CbType.SelectedValue : -1;
                decimal donGiaNhap, donGiaBan;
                bool isGiaNhapValid = decimal.TryParse(TbGiaNhap.Text, out donGiaNhap);
                bool isGiaBanValid = decimal.TryParse(TbGiaBan.Text, out donGiaBan);

                if (!isGiaNhapValid || !isGiaBanValid)
                {
                    MessageBox.Show("Giá nhập và giá bán phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isGiaNhapValid) TbGiaNhap.Focus(); else TbGiaBan.Focus();
                    return;
                }
                if (donGiaNhap < 0 || donGiaBan < 0)
                {
                    MessageBox.Show("Giá nhập và giá bán không được là số âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string donViTinh = comboBoxDonViTinh.SelectedItem as string;
                comboBoxDonViTinh.Text = donViTinh;
                int maKho = (int)CbKho.SelectedValue;
                string ghiChu = TbAddress.Text;
                byte[] hinhAnh = this.hinhAnh;
                if (string.IsNullOrEmpty(TbName.Text) || string.IsNullOrEmpty(TbGiaNhap.Text) || string.IsNullOrEmpty(TbGiaBan.Text) ||string.IsNullOrEmpty(TbAddress.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }
                if (CbType.SelectedIndex == -1 || comboBoxDonViTinh.SelectedIndex == -1 || CbKho.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ thông tin!");
                    return;
                }
                if (hinhAnh == null)
                {
                    MessageBox.Show("Vui lòng chọn hình ảnh Vật liệu!");
                    return;
                }

                bus.ThemVatLieu(ten, loai, donGiaNhap, donGiaBan, donViTinh, maKho, hinhAnh, ghiChu, nguoiTao);
                MessageBox.Show("Thêm vật tư thành công!");
                frmMaterial.LoadData();
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
            CbType.SelectedIndex = -1;
        }
        private void LoadComboBoxKho()
        {
            DataTable dt = bus.LayDanhSachKho();
            CbKho.DataSource = dt;
            CbKho.DisplayMember = "TenKho";
            CbKho.ValueMember = "MaKho";
            comboBoxDonViTinh.SelectedIndex = -1;
        }
    }
}
