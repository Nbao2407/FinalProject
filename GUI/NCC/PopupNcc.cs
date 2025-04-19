using BUS;
using DAL;
using DTO;
using Microsoft.VisualBasic;
using QLVT.Helper;
using QLVT.NCC;
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

namespace QLVT
{
    public partial class PopNcc : Form
    {
        private FrmNCc _parentForm;
        private DTO_Ncap ncc;
        private BUS_Ncc busNcc = new BUS_Ncc();
        private DAL_NCcap DAL_NCcap = new DAL_NCcap();
        private const string STATUS_PENDING = "Chờ duyệt";
        private const string STATUS_ACTIVE = "Hoạt động";
        private const string STATUS_LOCKED = "Bị khóa";
        private const string STATUS_REJECTED = "Từ chối";

        public PopNcc(FrmNCc parentForm, DTO_Ncap Ncc)
        {
            InitializeComponent();
            _parentForm = parentForm;
            ncc = Ncc;
            this.FormBorderStyle = FormBorderStyle.None;
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            this.Load += PopupForm_Load;
            this.Resize += PopupForm_Resize;
            LoadDataToControls(ncc);
            UpdateButtonStates(lblTrangThai.Text);
            Ngay.ReadOnly = true;
            TbEmail.ReadOnly = true;
            TbPhone.ReadOnly = true;
            TbAddress.ReadOnly = true;
        }

        private void UpdateButtonStates(string currentStatus)
        {
            bool isManagerOrAdmin = CurrentUser.ChucVu == "Admin" || CurrentUser.ChucVu == "Quản lý";

            bool canEdit = false;
            bool canApprove = false;
            bool canReject = false;
            bool canToggleLock = false;
            string toggleButtonText = "Khóa/Mở";

            if (isManagerOrAdmin)
            {
                switch (currentStatus)
                {
                    case STATUS_PENDING:
                        canApprove = true;
                        canReject = true;
                        canEdit = true;
                        canToggleLock = false;
                        toggleButtonText = "Khóa";
                        break;

                    case STATUS_ACTIVE:
                        canApprove = false;
                        canReject = false;
                        canEdit = true;
                        canToggleLock = true;
                        toggleButtonText = "Khóa";
                        break;

                    case STATUS_LOCKED:
                        canApprove = false;
                        canReject = false;
                        canEdit = true;
                        canToggleLock = true;
                        toggleButtonText = "Mở khóa";
                        break;

                    case STATUS_REJECTED:
                        canApprove = false;
                        canReject = false;
                        canEdit = false;
                        canToggleLock = false;
                        toggleButtonText = "Khóa";
                        break;

                    default:
                        canEdit = false;
                        canApprove = false;
                        canReject = false;
                        canToggleLock = false;
                        toggleButtonText = "Khóa/Mở";
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

            btnDecline.Enabled = canReject;
            btnDecline.Visible = canReject;

            BtnDisable.Enabled = canToggleLock;
            BtnDisable.Visible = canToggleLock;
            BtnDisable.Text = toggleButtonText;
        }

        public void LoadDataToControls(DTO_Ncap Ncc)
        {
            if (ncc != null)
            {
                lblTrangThai.Text = ncc.TrangThai;

                UpdateButtonStates(ncc.TrangThai);
            }
            else
            {
                UpdateButtonStates(string.Empty);
            }
            lblTrangThai.Text = Ncc.TrangThai;
            ID.Text = Ncc.MaNCC.ToString();
            lblName.Text = Ncc.TenNCC ?? string.Empty;
            TbAddress.Text = Ncc.DiaChi ?? string.Empty;
            TbPhone.Text = Ncc.SDT ?? string.Empty;
            TbEmail.Text = Ncc.Email ?? string.Empty;
            Ngay.Text = Ncc.NgayTao.ToString("dd/MM/yyyy");
            BUS_TK bUS_ = new BUS_TK();
            DataTable dt = bUS_.GetTkByMa(ncc.NguoiTao);
            DataRow row = dt.Rows[0];
            lblNgTao.Text = row["TenDangNhap"].ToString();
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
            int selectedMaNcc = int.Parse(ID.Text);
            int maNguoiThucHien = CurrentUser.MaTK;

            if (busNcc.ToggleKhoaNcc(selectedMaNcc, maNguoiThucHien, out string errorMessage))
            {
                MessageBox.Show("Thay đổi trạng thái Khóa/Mở khóa NCC thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm.Loaddata();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Thao tác thất bại:\n{errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            int id = int.Parse(ID.Text);
            int maNguoiDuyet = CurrentUser.MaTK;

            if (busNcc.DuyetNcc(id, maNguoiDuyet, out string errorMessage))
            {
                MessageBox.Show("Duyệt Nhà Cung Cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                _parentForm.Loaddata();
            }
            else
            {
                MessageBox.Show($"Duyệt Nhà Cung Cấp thất bại:\n{errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            int id = int.Parse(ID.Text);
            int maNguoiTuChoi = CurrentUser.MaTK;
            string lyDo = Interaction.InputBox("Nhập lý do từ chối (bỏ trống nếu không có):", "Lý do từ chối", "");

            if (busNcc.TuChoiNcc(id, maNguoiTuChoi, out string errorMessage))
            {
                MessageBox.Show("Từ chối Nhà Cung Cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm.Loaddata();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Từ chối Nhà Cung Cấp thất bại:\n{errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}