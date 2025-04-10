using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.TaiKhoan;
using QLVT.Type;
using QLVT.VatLieu;

namespace QLVT
{
    public partial class FrmNV : Form
    {
        private List<DTO_TK> danhSach;
        private System.Windows.Forms.Timer debounceTimer;
        public FrmNV(DTO_TK loggedInUser = null)
        {
            InitializeComponent();
            this.Resize += new EventHandler(Frm_Resize);
        }

        private void FrmNV_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
            SetupDebounceTimer();

        }
        private void SetupDebounceTimer()
        {
            debounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop();
                PerformSearch();
            };
        }
        public void LoadData()
        {
            try
            {
                QLTK busNhanVien = new QLTK();
                List<DTO_TK> danhSach = busNhanVien.GetAllTk();

                if (danhSach == null || danhSach.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (dataGridView1 != null)
                        dataGridView1.DataSource = null;
                    return;
                }

                if (CurrentUser.ChucVu == "Quản lý")
                {
                    danhSach = danhSach.Where(nv => nv.quyen == "Nhân viên").ToList();
                }
                if (danhSach.Count == 0)
                {
                    MessageBox.Show("Không có nhân viên nào để hiển thị!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.DataSource = null;
                    return;
                }

                var data = danhSach.Select(nv => new
                {
                    MaTK = nv.maTK,
                    TenDangNhap = nv.tenDangNhap,
                    SDT = nv.sdt,
                    Email = nv.email,
                    Quyen = nv.quyen,
                    TrangThai = nv.trangThai
                }).ToList();

                dataGridView1.DataSource = data;
                dataGridView1.Columns["MaTK"].HeaderText = "Mã Tài Khoản";
                dataGridView1.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
                dataGridView1.Columns["SDT"].HeaderText = "Số Điện Thoại";
                dataGridView1.Columns["Email"].HeaderText = "Email";
                dataGridView1.Columns["Quyen"].HeaderText = "Quyền";
                dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";
                DataGridViewHelper.CustomizeDataGridView(dataGridView1);
                ResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                dataGridView1.Width = 1150;
                dataGridView1.Height = 642;
                dataGridView1.Left = (this.ClientSize.Width) / 2;
                dataGridView1.Top = (this.ClientSize.Height - 642) / 2;
            }
            else
            {
                dataGridView1.Width = this.ClientSize.Width;
                dataGridView1.Height = this.ClientSize.Height - 80;
                dataGridView1.Left = 25;
                dataGridView1.Top = 80;
            }
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            if (dataGridView1.Columns.Count == 0) return;

            int totalWidth = dataGridView1.ClientSize.Width;
            int fixedColumnWidth = 50;
            int variableColumnCount = dataGridView1.Columns.Count;
            int variableColumnWidth = (totalWidth - fixedColumnWidth) / variableColumnCount;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Width = variableColumnWidth;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var add = new AddTk(this))
            {
                add.Deactivate += (s, e) => add.TopMost = true;
                add.StartPosition = FormStartPosition.CenterParent;
                add.ShowDialog();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int tk = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MaTK"].Value);
                int maTk = Convert.ToInt32(dataGridView1.CurrentRow.Cells["MaTK"].Value);
                PopupTk popup = new PopupTk(this, maTk);
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }

        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
            try
            {
                IEnumerable<DTO_TK> suggestionSource = danhSach;

                List<DTO_TK> suggestionResults = new List<DTO_TK>(); // Khởi tạo danh sách rỗng cho gợi ý
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        bool match = false;
                        if (order.maTK.ToString().Contains(searchQuery)) match = true;
                        if (!match && order.tenDangNhap.ToLowerInvariant().Contains(searchQuery)) match = true;
                        return match;
                    }).ToList();
                }

                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");

                Func<DTO_TK, string> displayFunc = item => $"{item.maTK} - {item.tenDangNhap ?? "N/A"}";

                Action<DTO_TK> clickAction = selectedItem =>
                {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.maTK.ToString(); // Điền Mã ĐN vào ô search
                    txtSearch.TextChanged += txtSearch_TextChanged;

                    var itemToShow = new List<DTO_TK> { selectedItem };
                    dataGridView1.DataSource = itemToShow;
                    ResizeColumns();
                };

                SearchHelper.UpdateSearchSuggestions(
                    result,
                    suggestionResults,
                    txtSearch,
                    38,
                    190,
                    displayFunc,
                    clickAction
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm gợi ý: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
    }
}