using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using GUI.NCC;
using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GUI
{

    public partial class PopNcc : Form
    {
        private FrmNCc _parentForm;
        private DTO_Ncap ncc;
        private BUS_Ncc busNcc = new BUS_Ncc();
        private DAL_NCcap DAL_NCcap = new DAL_NCcap();
        public PopNcc(FrmNCc parentForm, DTO_Ncap Ncc)
        {
            InitializeComponent();
            _parentForm = parentForm;
            ncc = Ncc;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.Load += PopupForm_Load;
            this.Resize += PopupForm_Resize;
            LoadDataToControls(ncc);
            Ngay.ReadOnly = true;
            TbEmail.ReadOnly = true;
            TbPhone.ReadOnly = true;
            TbAddress.ReadOnly = true;
        }


        public void LoadDataToControls(DTO_Ncap Ncc)
        {
            if (Ncc != null)
            {
                ID.Text = Ncc.MaNCC.ToString();
                lblName.Text = Ncc.TenNCC;
                Ngay.Text = Ncc.NgayTao.ToString("dd/MM/yyyy");
                lblNgTao.Text = DAL_NCcap.GetCreatorNameById(Ncc.NguoiTao);
                TbPhone.Text = Ncc.SDT;
                TbEmail.Text = Ncc.Email.ToString();
                TbAddress.Text = Ncc.DiaChi.ToString();
            }
        }



        private void PopupForm_Load(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.MakeTextBoxesTransparent(this);

        }

        private void PopupForm_Resize(object sender, EventArgs e)
        {
            PopupHelper.RoundCorners(this, 10);
        }

        private void roundedPictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            using (var edit = new EditNcc(_parentForm, ncc, this))
            {
                edit.Deactivate += (s, e) => edit.TopMost = true;

                edit.StartPosition = FormStartPosition.CenterParent;

                edit.ShowDialog();
            }
        }

        private void PopNcc_Load(object sender, EventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int Id = int.Parse(ID.Text);
                int nguoiCapNhat =CurrentUser.MaTK; 
                busNcc.XoaNCC(Id, nguoiCapNhat);
                MessageBox.Show("Xóa nhà cung cấp thành công!");
                _parentForm.Loaddata();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
