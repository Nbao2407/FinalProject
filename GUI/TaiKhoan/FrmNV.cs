using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using Microsoft.VisualBasic;
using QLVT.Helper;
using QLVT.TaiKhoan;
using QLVT.Type;
using QLVT.VatLieu;

namespace QLVT
{
    public partial class FrmNV : Form
    {
        private List<DTO_TK> danhSach = new List<DTO_TK>(); 
        private System.Windows.Forms.Timer debounceTimer;
        private QLTK busTaiKhoan = new QLTK();
        public FrmNV(DTO_TK loggedInUser = null)
        {
            InitializeComponent();
            LoadData();
            this.Resize += new EventHandler(Frm_Resize);
            ConfigureDataGridViewColumns();
        }

        private void FrmNV_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
            SetupDebounceTimer();
            LoadComboBoxes();
            aloneComboBox1.SelectedIndexChanged += aloneComboBox1_SelectedIndexChanged;
            aloneComboBox2.SelectedIndexChanged += aloneComboBox2_SelectedIndexChanged;

        }
        private void ConfigureDataGridViewColumns()
        {
            dataGridView1.AutoGenerateColumns = false; // IMPORTANT
            dataGridView1.Columns.Clear(); // Clear any design-time columns first

            // Define columns, mapping DataPropertyName to DTO_TK properties
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColMaTK", // Control column name
                HeaderText = "Mã Tài Khoản",
                DataPropertyName = "maTK" // Matches DTO_TK.maTK
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColTenDangNhap",
                HeaderText = "Tên Đăng Nhập",
                DataPropertyName = "tenDangNhap" // Matches DTO_TK.tenDangNhap
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColSDT",
                HeaderText = "Số Điện Thoại",
                DataPropertyName = "sdt" // Matches DTO_TK.sdt
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColEmail",
                HeaderText = "Email",
                DataPropertyName = "email" // Matches DTO_TK.email
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColQuyen",
                HeaderText = "Quyền",
                DataPropertyName = "quyen" // Matches DTO_TK.quyen
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColTrangThai",
                HeaderText = "Trạng Thái",
                DataPropertyName = "trangThai" // Matches DTO_TK.trangThai
            });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
        }
        private void SetupDebounceTimer()
        {
            debounceTimer?.Dispose();

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
                List<DTO_TK> tempDanhSach = busTaiKhoan.GetAllTk();

                if (tempDanhSach == null)
                {
                    MessageBox.Show("Không thể tải danh sách tài khoản (null từ BUS/DAL).", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.danhSach = new List<DTO_TK>();
                }
                else
                {
                    this.danhSach = tempDanhSach.AsEnumerable().Reverse().ToList();
                }

                if (CurrentUser.ChucVu == "Quản lý")
                {
                    this.danhSach = this.danhSach.Where(nv => nv != null && nv.quyen == "Nhân viên").ToList();
                }

                dataGridView1.DataSource = null; 
                if (this.danhSach.Any()) 
                {   
                    
                    
                    var bindingSource = new BindingSource { DataSource = this.danhSach };
                    dataGridView1.DataSource = bindingSource;
                }
                else
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show("Không có dữ liệu tài khoản phù hợp để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                ResizeColumns(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nghiêm trọng khi tải dữ liệu tài khoản: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.danhSach = new List<DTO_TK>(); // Ensure danhSach is empty list on error
                if (dataGridView1 != null) dataGridView1.DataSource = null;
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

            string selectedTrangThai = null;
            string selectedChucvu = null;
            if (aloneComboBox1.SelectedIndex > 0) 
            {
                selectedTrangThai = aloneComboBox1.Text;
            }
            if (aloneComboBox2.Visible && aloneComboBox2.SelectedIndex > 0) 
            {
                selectedChucvu = aloneComboBox2.Text;
            }

            try
            {
                if (danhSach == null)
                {
                    MessageBox.Show("Danh sách gốc chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result != null) result.Visible = false;
                    dataGridView1.DataSource = null;
                    return;
                }

                IEnumerable<DTO_TK> filteredSource = danhSach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    filteredSource = filteredSource.Where(tk =>
                        tk != null &&
                        tk.trangThai != null &&
                        tk.trangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }

                if (!string.IsNullOrEmpty(selectedChucvu))
                {
                    filteredSource = filteredSource.Where(tk =>
                        tk != null &&
                        tk.quyen != null &&
                        tk.quyen.Equals(selectedChucvu, StringComparison.OrdinalIgnoreCase)
                    );
                }

                List<DTO_TK> finalResults;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    finalResults = filteredSource.Where(tk =>
                    {
                        if (tk == null) return false;
                        bool match = false;
                        if (tk.maTK.ToString().ToLowerInvariant().Contains(searchQuery))
                            match = true;
                        if (!match && tk.tenDangNhap != null && tk.tenDangNhap.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
                            match = true;
                        return match;
                    }).ToList();
                }
                else
                {
                    finalResults = filteredSource.Where(tk => tk != null).ToList();
                }

                dataGridView1.DataSource = null;
                if (finalResults.Any())
                {
                    var bindingSource = new BindingSource { DataSource = finalResults };
                    dataGridView1.DataSource = bindingSource;
                }
                ResizeColumns(); 

                Func<DTO_TK, string> displayFunc = item => $"{item.maTK} - {item.tenDangNhap}";
                Action<DTO_TK> clickAction = selectedItem =>
                {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.tenDangNhap;
                    txtSearch.TextChanged += txtSearch_TextChanged;
                    var itemToShow = new List<DTO_TK> { selectedItem };
                    var singleBindingSource = new BindingSource { DataSource = itemToShow };
                    dataGridView1.DataSource = singleBindingSource;
                    ResizeColumns();
                    if (result != null) result.Visible = false;
                };

                SearchHelper.UpdateSearchSuggestions(
                    result,
                    finalResults, 
                    txtSearch,
                    38, 190, displayFunc, clickAction);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
        }
        private void LoadComboBoxes()
        {
            if (CurrentUser.ChucVu == "Quản lý")
            {
                aloneComboBox1.Items.Clear();
                aloneComboBox2.Visible =false;
                aloneComboBox1.Items.Add("-- Tất cả TT --");
                aloneComboBox1.Items.Add("Hoạt động");
                aloneComboBox1.Items.Add("Ngừng hoạt động");
                aloneComboBox1.SelectedIndex = -1;
            }
            else
            {
                aloneComboBox1.Items.Clear();
                aloneComboBox1.Items.Add("-- Tất cả TT --");
                aloneComboBox1.Items.Add("Hoạt động");
                aloneComboBox1.Items.Add("Ngừng hoạt động");
                aloneComboBox1.SelectedIndex = -1;
                aloneComboBox2.Items.Clear();
                aloneComboBox2.Items.Add("-- Tất cả TT --");
                aloneComboBox2.Items.Add("Admin");
                aloneComboBox2.Items.Add("Quản lý");
                aloneComboBox2.Items.Add("Nhân viên");
                aloneComboBox2.SelectedIndex = -1;
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void aloneComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
        private void aloneComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }
    }
}