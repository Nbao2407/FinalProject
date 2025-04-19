using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.HoaDon;
using QLVT.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace QLVT
{
    public partial class FrmHoaDon : Form
    {
        private BUS_HoaDon busHoaDon = new BUS_HoaDon();
        private DTO_HoaDon dalhoaDon = new DTO_HoaDon();
        private DAL_HoaDon dalHoaDon = new DAL_HoaDon();
        private FrmHoaDon _parentForm;
        private System.Windows.Forms.Timer debounceTimer;
        private List<DTO_HoaDon> hoaDons = new List<DTO_HoaDon>();
        private List<DTO_HoaDon> danhSach = new List<DTO_HoaDon>();
        private List<ChiTietHoaDon> chiTietHoaDon = new List<ChiTietHoaDon>();
        private int? maHoaDon;

        public FrmHoaDon()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Resize += new EventHandler(Frm_Resize);
            LoadData();
            SetupDataGridView();
            SetupDebounceTimer();
            LoadComboBoxes();
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            LoadData();
            ResizeColumns();
        }

        public void LoadData()
        {
            try
            {
                danhSach = busHoaDon.LayTatcaHoaDon2(CurrentUser.MaTK);
                danhSach = danhSach.AsEnumerable().Reverse().ToList();

                if (danhSach != null && danhSach.Any())
                {
                    dataGridView1.DataSource = danhSach;
                    ResizeColumns();
                }
                else
                {
                    danhSach = new List<DTO_HoaDon>();
                    dataGridView1.DataSource = null;
                    MessageBox.Show("Không có dữ liệu ban đầu để hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ban đầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                danhSach = new List<DTO_HoaDon>();
                dataGridView1.DataSource = null;
            }
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMaHoaDon",
                HeaderText = "Mã",
                DataPropertyName = "MaHoaDon",
                Width = 80,
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNguoiTao",
                HeaderText = "Người tạo",
                DataPropertyName = "NguoiLap",
                Width = 100,
                ReadOnly = true,
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNhapLap",
                HeaderText = "Ngày lập",
                DataPropertyName = "Ngaylap",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTenKh",
                HeaderText = "Tên khách hàng",
                DataPropertyName = "TenKhachHang",
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTrangThai",
                HeaderText = "Trạng Thái",
                DataPropertyName = "TrangThai",
                Width = 100,
                ReadOnly = true
            });

            DataGridViewHelper.CustomizeDataGridView(dataGridView1);
            this.Resize += new EventHandler(Frm_Resize);
        }



        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];

                string maHoaDonColumnName = "colMaHoaDon"; // Ví dụ tên cột
                int hoaDonId = -1;

                try
                {
                    if (dataGridView1.Columns.Contains(maHoaDonColumnName) && clickedRow.Cells[maHoaDonColumnName].Value != null &&
                        int.TryParse(clickedRow.Cells[maHoaDonColumnName].Value.ToString(), out hoaDonId))
                    {
                        DTO_HoaDon selected = busHoaDon.GetHoaDonById(hoaDonId);

                        if (selected == null)
                        {
                            MessageBox.Show("Không tìm thấy chi tiết hóa đơn với mã được chọn.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (CurrentUser.ChucVu == "Nhân viên")
                        {
                            // Dùng MaTK từ DT vừa lấy được
                            if (selected.MaTK == CurrentUser.MaTK)
                            {
                                ShowPopup(selected.MaHoaDon);
                            }
                            else
                            {
                                MessageBox.Show("Bạn là Nhân viên nhưng không phải người tạo hóa đơn này nên không thể mở.", "Không có quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            ShowPopup(selected.MaHoaDon);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể lấy Mã Hóa Đơn hợp lệ từ dòng được chọn.", "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy hoặc xử lý chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowPopup(int orderId)
        {
            try
            {
                using (var popup = new PopupHoaDon(orderId, this))
                {
                    popup.StartPosition = FormStartPosition.CenterParent;

                    DialogResult result = popup.ShowDialog();

                    bool dataMayHaveChanged = popup.DataChanged;

                    if (dataMayHaveChanged)
                    {
                        Console.WriteLine("Data may have changed, reloading initial data...");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open or process the order details.\nError: {ex.Message}", "Popup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        public void Openadd()
        {
            this.Controls.Clear();
            AddHoaDon addHoaDon = new AddHoaDon(maHoaDon)
            {
                TopLevel = false,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(addHoaDon);
            addHoaDon.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Openadd();
        }

        private void CbTrangthai_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();

        }

        private void LoadComboBoxes()
        {
            CbTrangthai.Items.Clear();
            CbTrangthai.Items.Add("Tất cả");
            CbTrangthai.Items.Add("Đã thanh toán");
            CbTrangthai.Items.Add("Chờ duyệt");
            CbTrangthai.Items.Add("Nháp");
            CbTrangthai.Items.Add("Từ chối");
            CbTrangthai.Items.Add("Đã hủy");
            CbTrangthai.SelectedIndex = 0;
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

        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();

            string selectedTrangThai = null;
            if (CbTrangthai.SelectedIndex > 0)
            {
                selectedTrangThai = CbTrangthai.Text;
            }

            try
            {
                if (danhSach == null)
                {
                    MessageBox.Show("Danh sách đơn nhập chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result != null) result.Visible = false;
                    dataGridView1.DataSource = null;
                    return;
                }

                IEnumerable<DTO_HoaDon> suggestionSource = danhSach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    suggestionSource = suggestionSource.Where(order =>
                        order != null &&
                        order.TrangThai != null &&
                        order.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }

                List<DTO_HoaDon> suggestionResults;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        if (order == null) return false;

                        bool match = false;
                        if (order.MaHoaDon.ToString().ToLowerInvariant().Contains(searchQuery))
                        {
                            match = true;
                        }

                        if (!match && order.TenKhachHang != null &&
                            order.TenKhachHang.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase))
                        {
                            match = true;
                        }
                        return match;
                    }).ToList();
                }
                else
                {
                    suggestionResults = suggestionSource.Where(order => order != null).ToList();
                }

                dataGridView1.DataSource = null;
                if (suggestionResults.Any())
                {
                    dataGridView1.DataSource = suggestionResults;
                }

                Func<DTO_HoaDon, string> displayFunc = item => $"{item.MaHoaDon} - {item.TenKhachHang}";

                Action<DTO_HoaDon> clickAction = selectedItem =>
                {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.MaHoaDon.ToString();
                    txtSearch.TextChanged += txtSearch_TextChanged;
                    var itemToShow = new List<DTO_HoaDon> { selectedItem };
                    dataGridView1.DataSource = itemToShow;
                    ResizeColumns();
                    if (result != null) result.Visible = false;
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
            catch (NullReferenceException nre)
            {
                MessageBox.Show($"Lỗi tham chiếu null khi tìm kiếm: {nre.Message}\nKiểm tra xem có đối tượng 'order' nào bị null trong danh sách không.\n{nre.StackTrace}", "Lỗi Null Reference", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khác khi tìm kiếm: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result != null) result.Visible = false;
                dataGridView1.DataSource = null;
            }
        }
    }
}