using BUS;
using DTO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using GUI.Helpler;
namespace QLVT
{
    public partial class AddHoaDon : Form
    {
        private BUS_HoaDon busHoaDon = new BUS_HoaDon();
        private int? _maHoaDon = null;
        private List<ChiTietHoaDonTemp> chiTietHoaDonTemps = new List<ChiTietHoaDonTemp>();
        private BUS_Khach kh = new BUS_Khach();
        private System.Windows.Forms.Timer debounceTimer;
        private Color defaultLabelColor = Color.White;
        private Color hoverLabelColor = Color.FromArgb(230, 240, 255);
        private Form1 form1;
        public AddHoaDon(int? maHoaDon = null)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
            this.DoubleBuffered = true;
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UpdateFlowLayoutPanel1Size();
            this.Resize += AddHoaDon_Resize;
            debounceTimer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop();
                PerformSearch();
            };
            flowLayoutPanel1.Resize += (s, e) =>
            {
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    ctrl.Width = flowLayoutPanel1.Width - 20;
                }
            };
        }

        private void AddHoaDon_Load(object sender, EventArgs e)
        {
            LoadVatLieu();
            _maHoaDon = CreateNewHoaDon();

            lblTong.Text = "0";
            lblTongSoMatHang.Text = "Tổng số lượng: 0";
            AddHoaDon_Resize(this, EventArgs.Empty);
        }


        private void LoadVatLieu()
        {
            try
            {
                DataTable dt = busHoaDon.LoadVatLieu();
                flowLayoutPanel2.Controls.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string ten = row["Ten"].ToString();
                    double gia = Convert.ToDouble(row["DonGiaBan"]);
                    int maVatLieu = Convert.ToInt32(row["MaVatLieu"]);
                    Image hinhAnh = busHoaDon.ConvertByteArrayToImage(row["HinhAnh"] as byte[]);
                    AddItem(ten, gia, hinhAnh, maVatLieu);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load vật liệu: " + ex.Message);
            }
        }

        public void AddItem(string name, double price, Image image, int maVatLieu)
        {
            var widget = new Wiget()
            {
                Title = name,
                Gia = price,
                Images = image,
                Margin = new Padding(5),
                Tag = maVatLieu
            };
            widget.roundedPanel1.Click += (s, e) => Widget_Click(widget);
            flowLayoutPanel2.Controls.Add(widget);
        }
        private void AddItemToHoaDon(string name, double price, int soLuong, Image image, int maVatLieu)
        {
            var existingWidget = flowLayoutPanel1.Controls.OfType<Wiget2>()
                .FirstOrDefault(w => w.MaVatLieu == maVatLieu);

            if (existingWidget != null)
            {
                int newSoLuong = int.Parse(existingWidget.TbSL.Text) + soLuong;
                existingWidget.TbSL.Text = newSoLuong.ToString();
                existingWidget.UpdateGia(newSoLuong);
            }
            else
            {
                var widget2 = new Wiget2()
                {
                    IdSp = { Text = $"SP{maVatLieu:D4}" },
                    Title2 = { Text = name },
                    DonGia = price,
                    TbSL = { Text = soLuong.ToString() },
                    lblGia = { Text = (price * soLuong).ToString("#,##0") },
                    MaVatLieu = maVatLieu,
                    MaHoaDon = _maHoaDon.Value,
                    Margin = new Padding(5)
                };

                widget2.OnDelete += (s, e) =>
                {
                    flowLayoutPanel1.Controls.Remove(widget2);
                    UpdateTongSoLuongVaTongTien();
                };

                widget2.OnQuantityChanged += (s, e) =>
                {
                    int newSoLuong = int.Parse(widget2.TbSL.Text);
                    widget2.UpdateGia(newSoLuong);
                    UpdateTongSoLuongVaTongTien();
                };

                flowLayoutPanel1.Controls.Add(widget2);
            }
            UpdateTongSoLuongVaTongTien();
        }

        private void UpdateTongSoLuongVaTongTien()
        {
            int tongSoLuong = flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => int.Parse(w.TbSL.Text));
            double tongTien = flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => w.DonGia * int.Parse(w.TbSL.Text));
            lblTongSoMatHang.Text = $"Tổng số lượng: {tongSoLuong}";
            lblTong.Text = tongTien.ToString("#,##0");
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (flowLayoutPanel1.Controls.Count == 0)
                {
                    MessageBox.Show("Hóa đơn trống! Vui lòng thêm vật liệu trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? maKhachHang = null;
                if (!string.IsNullOrWhiteSpace(SeacrhKh.Text))
                {
                    string[] parts = SeacrhKh.Text.Split('-');
                    if (parts.Length > 1 && int.TryParse(parts[parts.Length - 1].Trim(), out int parsedMaKhachHang))
                    {
                        maKhachHang = parsedMaKhachHang;
                        Console.WriteLine($"Khách hàng được chọn: {SeacrhKh.Text} (Mã: {maKhachHang})");
                    }
                }

                foreach (var widget in flowLayoutPanel1.Controls.OfType<Wiget2>())
                {
                    int soLuong = int.Parse(widget.TbSL.Text);
                    if (soLuong <= 0)
                    {
                        MessageBox.Show($"Số lượng của {widget.Title2.Text} phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DataTable dt = busHoaDon.CheckSoLuongTonKho(widget.MaVatLieu);
                    int soLuongTon = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
                    if (soLuongTon < soLuong)
                    {
                        MessageBox.Show($"Số lượng tồn kho không đủ cho vật liệu {widget.Title2.Text}! (Tồn: {soLuongTon}, Yêu cầu: {soLuong})", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (maKhachHang.HasValue)
                {
                    busHoaDon.UpdateHoaDon(_maHoaDon.Value, maKhachHang, "Tiền mặt", "Chờ thanh toán", CurrentUser.MaTK);
                }

                foreach (var widget in flowLayoutPanel1.Controls.OfType<Wiget2>())
                {
                    int soLuong = int.Parse(widget.TbSL.Text);
                    busHoaDon.AddChiTietHoaDon(_maHoaDon.Value, widget.MaVatLieu, soLuong);
                }

                busHoaDon.UpdateTHoaDon(_maHoaDon.Value, "Đã thanh toán");

                MessageBox.Show($"Thanh toán thành công! Mã khách hàng: {maKhachHang ?? -1}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FrmHoaDon frmHoaDon = Application.OpenForms.OfType<FrmHoaDon>().FirstOrDefault();
                if (frmHoaDon != null)
                {
                    frmHoaDon.Activate();
                    frmHoaDon.LoadData();
                }
                else
                {
                    frmHoaDon = new FrmHoaDon();
                    frmHoaDon.LoadData();
                    frmHoaDon.Show();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Lỗi thanh toán: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
        private void Widget_Click(Wiget widget)
        {
            try
            {
                int maVatLieu = (int)widget.Tag;
                int soLuong = 1;

                AddItemToHoaDon(widget.Title, widget.Gia, soLuong, widget.Images, maVatLieu);
                Console.WriteLine("add sản phẩm ...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }
        private int CreateNewHoaDon()
        {
            try
            {
                int maTKLap = CurrentUser.MaTK;
                int? maKhachHang = null;

                if (!string.IsNullOrWhiteSpace(SeacrhKh.Text) && int.TryParse(SeacrhKh.Text, out int parsedMaKhachHang))
                {
                    maKhachHang = parsedMaKhachHang;
                }

                return busHoaDon.CreateHoaDon(maTKLap, maKhachHang);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo hóa đơn mới: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw;
            }
        }



        public class BufferedSplitContainer : SplitContainer
        {
            public BufferedSplitContainer()
            {
                this.DoubleBuffered = true;
            }
        }

        public class BufferedFlowLayoutPanel : FlowLayoutPanel
        {
            public BufferedFlowLayoutPanel()
            {
                this.DoubleBuffered = true;
            }
        }

        private void AddHoaDon_Resize(object sender, EventArgs e)
        {
            int maxWidgetWidth = 0;
            foreach (Control ctrl in flowLayoutPanel2.Controls)
            {
                if (ctrl is Wiget widget)
                {
                    maxWidgetWidth = Math.Max(maxWidgetWidth, widget.Width);
                }
            }

            int productListWidth = maxWidgetWidth + 20;
            int minSplitterDistance = 520;
            int maxSplitterDistance = splitContainer1.Width - 300;
            productListWidth = Math.Max(minSplitterDistance, Math.Min(productListWidth, maxSplitterDistance));
            splitContainer1.SplitterDistance = productListWidth;
        }

        private void UpdateFlowLayoutPanel1Size()
        {
            if (flowLayoutPanel1 != null)
            {
                flowLayoutPanel1.Left = 0;
                flowLayoutPanel1.Width = splitContainer1.Panel2.ClientSize.Width - 10;
                flowLayoutPanel1.Height = splitContainer1.Panel2.ClientSize.Height - 85;

                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    ctrl.Width = flowLayoutPanel1.Width - 20;
                }
            }

            if (WindowState == FormWindowState.Maximized)
            {
                flowLayoutPanel1.Width = splitContainer1.Panel2.ClientSize.Width - 50;
                flowLayoutPanel1.Height = splitContainer1.Panel2.ClientSize.Height - 85;

                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    ctrl.Width = flowLayoutPanel1.Width - 20;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text?.ToLower() ?? "";
            foreach (Control item in flowLayoutPanel2.Controls)
            {
                if (item is Wiget wiget)
                {
                    wiget.Visible = wiget.Title.ToLower().Contains(searchText);
                }
            }
        }
        private void SeacrhKh_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void PerformSearch()
        {
            string searchQuery = SeacrhKh.Text.Trim();
            try
            {
                List<DTO_Khach> results = kh.TimKiemKh(searchQuery);
                Console.WriteLine($"Search Query: {searchQuery}, Results: {results.Count}");
                UpdateSearchSuggestions(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ketqua.Visible = false;
            }
        }
        private void UpdateSearchSuggestions(List<DTO_Khach> results)
        {
            ketqua.Controls.Clear();
            if (results.Any() && !string.IsNullOrWhiteSpace(SeacrhKh.Text.Trim()))
            {
                ketqua.Height = Math.Min(results.Count * 40, 200);
                ketqua.BackColor = Color.Transparent;

                foreach (var item in results)
                {
                    Label lbl = new Label
                    {
                        Text = $"👤 {item.Ten} - {item.MaKhachHang}",
                        AutoSize = false,
                        Width = ketqua.Width - 2,
                        Height = 40,
                        Padding = new Padding(10, 5, 5, 5),
                        Font = new Font("Segoe UI", 11, FontStyle.Regular),
                        ForeColor = Color.FromArgb(33, 37, 41),
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(1),
                        Tag = item
                    };

                    lbl.MouseEnter += (s, e) => { lbl.BackColor = hoverLabelColor; lbl.ForeColor = Color.FromArgb(0, 102, 204); };
                    lbl.MouseLeave += (s, e) => { lbl.BackColor = defaultLabelColor; lbl.ForeColor = Color.FromArgb(33, 37, 41); };
                    lbl.Click += (s, e) =>
                    {
                        var selectedItem = (DTO_Khach)lbl.Tag;
                        SeacrhKh.Text = $"{selectedItem.Ten} - {selectedItem.MaKhachHang}";
                        ketqua.Visible = false;
                    };

                    ketqua.Controls.Add(lbl);
                }
                ketqua.Visible = true;
            }
            else
            {
                ketqua.Visible = false;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
                busHoaDon.DeleteHoaDonTam(_maHoaDon.Value, CurrentUser.MaTK);
                this.Controls.Clear();
                FrmHoaDon frm = new FrmHoaDon()
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(frm);
                frm.Show();
        }
    }
}