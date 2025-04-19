using BUS;
using DAL;
using DTO;
using QLVT.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Runtime.Profiler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLVT.TaiKhoan
{
    public partial class EditTk : Form
    {
        private BUS_TK busTk = new BUS_TK();
        private FrmNV _parentFrm;
        private int? maTK;
        private bool isManualToggle = true;

        public EditTk(FrmNV frmNV, int? maTk)
        {
            InitializeComponent();
            PopupHelper.RoundCorners(this, 10);
            PopupHelper.changecolor(this);
            TgTrangthai.Items.Add("Hoạt động");
            TgTrangthai.Items.Add("khoá");
            _parentFrm = frmNV;
            this.maTK = maTk;
            loadComboBoxChucVu();
            LoadDataFromDatabase();
        }

        private void EditTk_Load(object sender, EventArgs e)
        {
            if (CurrentUser.ChucVu == "Quản lý")
            {
                label7.Visible = false;
                TbPass.Visible = false;
            }
            else
            {
                TbPass.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void LoadDataFromDatabase()
        {
            if (!maTK.HasValue)
            {
                MessageBox.Show("Không có mã tài khoản để tải dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            DataTable dt = busTk.GetTkByMa(maTK.Value);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                PlaceholderHelper.SetDataAsPlaceholder(TbId, maTK.ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbName, row["TenDangNhap"].ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbEmail, row["Email"].ToString());
                string chucVuCanChon = row["ChucVu"]?.ToString().Trim(); 
                string trangThaiCanChon = row["TrangThai"]?.ToString().Trim(); 

                if (chucVuCanChon != null)
                {
                    CbChucVu.SelectedIndex = -1; 
                    foreach (var item in CbChucVu.Items)
                    {
                        if (item?.ToString().Trim().Equals(chucVuCanChon, StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            CbChucVu.SelectedItem = item;
                            Console.WriteLine($"DEBUG: Found and selected ChucVu: {item}"); // Thêm debug
                            break;
                        }
                        // else { Console.WriteLine($"DEBUG: Comparing '{item?.ToString().Trim()}' with '{chucVuCanChon}' -> NO MATCH"); }
                    }
                    if (CbChucVu.SelectedItem == null) { Console.WriteLine($"DEBUG: Could not find item for ChucVu: '{chucVuCanChon}'"); }
                }

                if (trangThaiCanChon != null)
                {
                    TgTrangthai.SelectedIndex = -1; // Reset
                    foreach (var item in TgTrangthai.Items)
                    {
                        if (item?.ToString().Trim().Equals(trangThaiCanChon, StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            TgTrangthai.SelectedItem = item;
                            Console.WriteLine($"DEBUG: Found and selected TrangThai: {item}");
                            break;
                        }
                    }
                    if (TgTrangthai.SelectedItem == null) { Console.WriteLine($"DEBUG: Could not find item for TrangThai: '{trangThaiCanChon}'"); }
                }
                PlaceholderHelper.SetDataAsPlaceholder(TbSdt, row["SDT"].ToString());
                PlaceholderHelper.SetDataAsPlaceholder(TbNote, row["Ghichu"].ToString());
                TbPass.UseSystemPasswordChar = true;
                PlaceholderHelper.SetPlaceholder(TbPass, "Nhập mật khẩu mới");

            }
            else
            {
                MessageBox.Show("Lỗi tải dữ liệu từ cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = TbName.Text == TbName.Tag?.ToString() ? "" : TbName.Text;
                string email = TbEmail.Text == TbEmail.Tag?.ToString() ? "" : TbEmail.Text;
                string sdt = TbSdt.Text == TbSdt.Tag?.ToString() ? "" : TbSdt.Text;
                string ghichu = TbNote.Text == TbNote.Tag?.ToString() ? null : TbNote.Text;
                string chucVu = CbChucVu.SelectedItem?.ToString();
                string hashedMatKhauMoi = TbPass.Text;
                string trangThai = TgTrangthai.SelectedItem?.ToString();
                string errorMessage;
                if(!ValidationHelper.IsValidMatKhau(hashedMatKhauMoi, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }    
                if (!ValidationHelper.IsValidTenDangNhap(tenDangNhap, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!ValidationHelper.IsValidEmail(email, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(sdt) && !ValidationHelper.IsValidSDT(sdt, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(trangThai)) 
                {
                    MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TgTrangthai.Focus(); 
                    return;
                }

                if (!string.IsNullOrEmpty(ghichu) && ghichu.Length > 255)
                {
                    MessageBox.Show("Ghi chú không được vượt quá 255 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!ValidationHelper.IsValidChucVu(chucVu, out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DTO_TK tk = new DTO_TK
                {
                    maTK = int.Parse(TbId.Text),
                    tenDangNhap = tenDangNhap,
                    email = email,
                    sdt = string.IsNullOrEmpty(sdt) ? null : sdt,
                    quyen = chucVu,
                    ghichu = ghichu,
                    trangThai = trangThai
                };

                bool result = busTk.SuaTaiKhoan(tk, hashedMatKhauMoi, CurrentUser.MaTK);
                if (result)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _parentFrm.LoadData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void seePW_Click(object sender, EventArgs e)
        {
            TbPass.UseSystemPasswordChar = !TbPass.UseSystemPasswordChar;
        }

        private void TbPass_TextChanged(object sender, EventArgs e)
        {
            if (TbPass.Text != "Nhập mật khẩu mới")
            {
                TbPass.UseSystemPasswordChar = true;
            }
            else
            {
                TbPass.UseSystemPasswordChar = false;
            }
        }
        private void loadComboBoxChucVu()
        {
            if (CurrentUser.ChucVu == "Admin")
            {
                List<string> chucVuList = new List<string> { "Admin", "Quản lý", "Nhân viên" };
                TbPass.Enabled = true;
                pictureBox1.Visible = true;
                CbChucVu.DataSource = chucVuList;
            }
            else if (CurrentUser.ChucVu == "Quản lý")
            {
                List<string> chucVuList = new List<string> {"Nhân viên"};
                TbPass.Enabled = false;
                CbChucVu.Enabled = false;
                pictureBox1.Visible = false;
                CbChucVu.DataSource = chucVuList;
            }
        }

        private void TgTrangthai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
