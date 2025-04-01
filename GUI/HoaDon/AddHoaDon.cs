using BUS;
using DTO;
using System.Data;

namespace GUI
{
    public partial class AddHoaDon : Form
    {
        private BUS_HoaDon busHoaDon = new BUS_HoaDon();
        private int? _maHoaDon = null;
        private List<ChiTietHoaDonTemp> chiTietHoaDonTemps = new List<ChiTietHoaDonTemp>();
        public AddHoaDon(int? maHoaDon = null)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
            this.DoubleBuffered = true;
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Resize += AddHoaDon_Resize;
            this.FormClosing += AddHoaDon_FormClosing;
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
                dungeonTextBox1.Text = "Khách lẻ";
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

                var chiTiet = chiTietHoaDonTemps.FirstOrDefault(x => x.MaVatLieu == maVatLieu);
                if (chiTiet != null)
                {
                    chiTiet.SoLuong = newSoLuong;
                }
                else
                {
                    chiTietHoaDonTemps.Add(new ChiTietHoaDonTemp
                    {
                        MaVatLieu = maVatLieu,
                        TenVatLieu = name,
                        DonGia = price,
                        SoLuong = newSoLuong,
                        HinhAnh = image
                    });
                }
            }
            else
            {
                chiTietHoaDonTemps.Add(new ChiTietHoaDonTemp
                {
                    MaVatLieu = maVatLieu,
                    TenVatLieu = name,
                    DonGia = price,
                    SoLuong = soLuong,
                    HinhAnh = image
                });

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
                    chiTietHoaDonTemps.RemoveAll(x => x.MaVatLieu == widget2.MaVatLieu);
                    flowLayoutPanel1.Controls.Remove(widget2);
                    UpdateTongSoLuongVaTongTien();
                };

                widget2.OnQuantityChanged += (s, e) =>
                {
                    int newSoLuong = int.Parse(widget2.TbSL.Text);
                    var chiTiet = chiTietHoaDonTemps.FirstOrDefault(x => x.MaVatLieu == widget2.MaVatLieu);
                    if (chiTiet != null)
                    {
                        chiTiet.SoLuong = newSoLuong;
                    }
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
                if (!chiTietHoaDonTemps.Any())
                {
                    MessageBox.Show("Hóa đơn trống! Vui lòng thêm vật liệu trước khi thanh toán.");
                    return;
                }

                foreach (var chiTiet in chiTietHoaDonTemps)
                {
                    DataTable dt = busHoaDon.CheckSoLuongTonKho(chiTiet.MaVatLieu);
                    int soLuongTon = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
                    if (soLuongTon < chiTiet.SoLuong)
                    {
                        MessageBox.Show($"Số lượng tồn kho không đủ cho vật liệu {chiTiet.TenVatLieu}! (Tồn: {soLuongTon}, Yêu cầu: {chiTiet.SoLuong})");
                        return;
                    }
                }
                foreach (var chiTiet in chiTietHoaDonTemps)
                {
                    busHoaDon.AddChiTietHoaDon(_maHoaDon.Value, chiTiet.MaVatLieu, chiTiet.SoLuong);
                }
                busHoaDon.UpdateTHoaDon(_maHoaDon.Value, "Đã thanh toán");

                MessageBox.Show("Thanh toán thành công!");
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
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message);
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
                return busHoaDon.CreateHoaDon(maTKLap, maKhachHang);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể tạo hóa đơn mới: " + ex.Message);
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
            UpdateFlowLayoutPanel1Size();
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
        private void AddHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0 && _maHoaDon.HasValue)
            {
                try
                {
                    DialogResult result = MessageBox.Show(
                        "Hóa đơn trống sẽ bị xóa. Bạn có chắc chắn muốn đóng?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        busHoaDon.DeleteHoaDonTam(_maHoaDon.Value,CurrentUser.MaTK);
                    }
                    else
                    {
                        e.Cancel = true; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn tạm: " + ex.Message);
                }
            }
        }
    }
}