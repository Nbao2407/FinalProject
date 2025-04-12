using BUS;
using DAL;
using DTO;
using GUI.Helpler;
using QLVT.Helper;
using QLVT.HoaDon;
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
                List<DTO_HoaDon> danhSach = busHoaDon.LayTatCaHoaDon();
                danhSach = danhSach.AsEnumerable().Reverse().ToList();
                if (danhSach == null || danhSach.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (dataGridView1 != null)
                        dataGridView1.DataSource = null;
                    return;
                }
                var data = danhSach.Select(hd => new
                {
                    hd.MaHoaDon,
                    hd.NgayLap,
                    hd.NguoiLap,
                    KhachHang = hd.TenKhachHang,
                    hd.TongTien,
                    hd.TrangThai,
                    hd.HinhThucThanhToan
                }).ToList();

                dataGridView1.DataSource = data;

                if (dataGridView1.Columns.Contains("MaHoaDon"))
                    dataGridView1.Columns["MaHoaDon"].HeaderText = "Mã Hóa Đơn";

                if (dataGridView1.Columns.Contains("NgayLap"))
                    dataGridView1.Columns["NgayLap"].HeaderText = "Ngày Lập";

                if (dataGridView1.Columns.Contains("NguoiLap"))
                    dataGridView1.Columns["NguoiLap"].HeaderText = "Người Lập";

                if (dataGridView1.Columns.Contains("KhachHang"))
                    dataGridView1.Columns["KhachHang"].HeaderText = "Khách Hàng";

                if (dataGridView1.Columns.Contains("TongTien"))
                {
                    dataGridView1.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    dataGridView1.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["TongTien"].DefaultCellStyle.ForeColor = Color.Blue;
                }

                if (dataGridView1.Columns.Contains("TrangThai"))
                    dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";

                if (dataGridView1.Columns.Contains("HinhThucThanhToan"))
                    dataGridView1.Columns["HinhThucThanhToan"].HeaderText = "Hình Thức TT";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int maHoaDon = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                PopupHoaDon popup = new PopupHoaDon(maHoaDon);
                popup.ShowDialog();
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

        public void OpenEdit(int? maHoaDon)
        {
            this.Controls.Clear();
            EditHoaDon editForm = new EditHoaDon(maHoaDon);
            editForm.TopLevel = false;
            editForm.Dock = DockStyle.Fill;
            editForm.FormClosed += (s, e) =>
            {
                LoadData();
            };
            this.Controls.Add(editForm);
            editForm.Show();
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
                string filter = CbTrangthai.SelectedItem.ToString();
                if (filter == "Tất cả")
                {
                    dataGridView1.DataSource = hoaDons;
                }
                else
                {
                    var filteredList = hoaDons.Where(k => k.TrangThai == filter).ToList();
                    dataGridView1.DataSource = filteredList;
                }
        }

        private void LoadComboBoxes()
        {
            CbTrangthai.Items.Clear();
            CbTrangthai.Items.Add("-- Tất cả TT --");
            CbTrangthai.Items.Add("Hoàn thành");
            CbTrangthai.Items.Add("Đang xử lý");
            CbTrangthai.Items.Add("Đã hủy");
            CbTrangthai.SelectedIndex = 0;
        }

        private void PerformSearch()
        {
            string searchQuery = txtSearch.Text.Trim().ToLowerInvariant();
            string selectedTrangThai = CbTrangthai.SelectedItem?.ToString();
            if (selectedTrangThai == "-- Tất cả TT --") { selectedTrangThai = null; }

            Console.WriteLine($"PerformSearch for suggestions. Query: '{searchQuery}', Status: '{selectedTrangThai ?? "All"}'");

            try
            {
                IEnumerable<DTO_HoaDon> suggestionSource = danhSach;

                if (!string.IsNullOrEmpty(selectedTrangThai))
                {
                    suggestionSource = suggestionSource.Where(order =>
                        order.TrangThai != null &&
                        order.TrangThai.Equals(selectedTrangThai, StringComparison.OrdinalIgnoreCase)
                    );
                }

                List<DTO_HoaDon> suggestionResults = new List<DTO_HoaDon>(); 
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    suggestionResults = suggestionSource.Where(order =>
                    {
                        bool match = false;
                        if (order.MaHoaDon.ToString().Contains(searchQuery)) match = true;
                        if (!match && order.TenKhachHang.ToLowerInvariant().Contains(searchQuery)) match = true;
                        return match;
                    }).ToList();
                }

                Console.WriteLine($"Found {suggestionResults.Count} suggestions.");

                Func<DTO_HoaDon, string> displayFunc = item => $"{item.MaHoaDon} - {item.TenKhachHang ?? "N/A"}";

                Action<DTO_HoaDon> clickAction = selectedItem => {
                    txtSearch.TextChanged -= txtSearch_TextChanged;
                    txtSearch.Text = selectedItem.MaHoaDon.ToString(); 
                    txtSearch.TextChanged += txtSearch_TextChanged;

                    var itemToShow = new List<DTO_HoaDon> { selectedItem };
                    dataGridView1.DataSource = itemToShow;
                    Console.WriteLine($"DGV DataSource updated to show only item {selectedItem.MaHoaDon}");
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
        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = CbTrangthai.SelectedItem?.ToString();

            if (danhSach == null) { return; }

            IEnumerable<DTO_HoaDon> listToShow = danhSach;

            if (filter != "-- Tất cả TT --" && !string.IsNullOrEmpty(filter))
            {
                listToShow = danhSach.Where(k => k.TrangThai != null && k.TrangThai.Equals(filter, StringComparison.OrdinalIgnoreCase));
            }

            var filteredByStatus = listToShow.ToList();
            dataGridView1.DataSource = filteredByStatus.Any() ? filteredByStatus : null;
            Console.WriteLine($"DGV updated by status filter. Showing {filteredByStatus.Count} items.");
            ResizeColumns();

            if (txtSearch.Text.Length > 0)
            {
                txtSearch.Text = string.Empty;
            }
            else
            {
                if (result != null) result.Visible = false;
            }
        }

    }
}