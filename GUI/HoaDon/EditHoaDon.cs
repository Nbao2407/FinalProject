using BUS;
using DTO;
using QLVT.HoaDon;
using System.Data;

namespace QLVT
{
    public partial class EditHoaDon : Form
    {
        private BUS_HoaDon busHoaDon = new BUS_HoaDon();
        private int? _maHoaDon = null;
        private List<ChiTietHoaDonTemp> chiTietHoaDonTemps = new List<ChiTietHoaDonTemp>();

        public EditHoaDon(int? maHoaDon = null)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
            if (_maHoaDon == null)
            {
                MessageBox.Show("MaHoaDon truyền vào là null!");
            }
            this.DoubleBuffered = true;
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Resize += EditHoaDon_Resize;
            flowLayoutPanel1.Resize += (s, e) =>
            {
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    ctrl.Width = flowLayoutPanel1.Width - 20;
                }
            };
        }

        private void EditHoaDon_Load(object sender, EventArgs e)
        {
            LoadVatLieu();
            if (_maHoaDon.HasValue)
            {
                LoadChiTietHoaDon(_maHoaDon.Value);
                LoadThongTinHoaDon(_maHoaDon.Value);
            }
            else
            {
                _maHoaDon = CreateNewHoaDon();
                dungeonTextBox1.Text = "Khách lẻ";
                lblTong.Text = "0";
                lblTongSoMatHang.Text = "Tổng số lượng: 0";
            }
            EditHoaDon_Resize(this, EventArgs.Empty);
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

        private void LoadChiTietHoaDon(int maHoaDon)
        {
            try
            {
                DataTable dt = busHoaDon.LoadChiTietHoaDon(maHoaDon);
                if (dt == null || dt.Rows.Count == 0)
                {
                    lblTongSoMatHang.Text = "Tổng số lượng: 0";
                    lblTong.Text = "0";
                }
                flowLayoutPanel1.Controls.Clear();
                int tongSoLuong = 0;
                double tongTien = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string ten = row["Ten"] != DBNull.Value ? row["Ten"].ToString() : "Không có tên";
                    double gia = row["DonGia"] != DBNull.Value ? Convert.ToDouble(row["DonGia"]) : 0;
                    int soLuong = row["SoLuong"] != DBNull.Value ? Convert.ToInt32(row["SoLuong"]) : 0;
                    int maVatLieu = row["MaVatLieu"] != DBNull.Value ? Convert.ToInt32(row["MaVatLieu"]) : 0;
                    byte[] hinhAnhBytes = row["HinhAnh"] as byte[];
                    Image hinhAnh = hinhAnhBytes != null ? busHoaDon.ConvertByteArrayToImage(hinhAnhBytes) : null;
                    tongSoLuong += soLuong;
                    tongTien += gia * soLuong;

                    var widget2 = new Wiget2()
                    {
                        IdSp = { Text = $"SP{maVatLieu:D4}" },
                        Title2 = { Text = ten },
                        DonGia = gia,
                        TbSL = { Text = soLuong.ToString() },
                        lblGia = { Text = (gia * soLuong).ToString("#,##0") },
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
                        busHoaDon.UpdateChiTietHoaDon(widget2.MaHoaDon, widget2.MaVatLieu, newSoLuong);
                        widget2.UpdateGia(newSoLuong);
                        UpdateTongSoLuongVaTongTien();
                    };

                    flowLayoutPanel1.Controls.Add(widget2);
                }
                lblTongSoMatHang.Text = $"Tổng số lượng: {tongSoLuong}";
                lblTong.Text = tongTien.ToString("#,##0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load chi tiết hóa đơn: " + ex.Message);
            }
        }
        private void UpdateTongSoLuongVaTongTien()
        {
            int tongSoLuong = flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => int.Parse(w.TbSL.Text));
            double tongTien = flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => w.DonGia * int.Parse(w.TbSL.Text));
            lblTongSoMatHang.Text = $"Tổng số lượng: {tongSoLuong}";
            lblTong.Text = tongTien.ToString("#,##0");
        }
        private void LoadThongTinHoaDon(int maHoaDon)
        {
            try
            {
                DataTable dt = busHoaDon.LoadThongTinHoaDon(maHoaDon);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    dungeonTextBox1.Text = row["MaKhachHang"].ToString();
                    lblTong.Text = Convert.ToDouble(row["TongTien"]).ToString("#,##0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load thông tin hóa đơn: " + ex.Message);
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
                    lblTongSoMatHang.Text = $"Tổng số lượng: {flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => int.Parse(w.TbSL.Text))}";
                };

                widget2.OnQuantityChanged += (s, e) =>
                {
                    int newSoLuong = int.Parse(widget2.TbSL.Text);
                    var chiTiet = chiTietHoaDonTemps.FirstOrDefault(x => x.MaVatLieu == widget2.MaVatLieu);
                    if (chiTiet != null)
                    {
                        chiTiet.SoLuong = newSoLuong;
                    }
                    busHoaDon.UpdateChiTietHoaDon(widget2.MaHoaDon, widget2.MaVatLieu, newSoLuong);
                    widget2.UpdateGia(newSoLuong);
                    lblTongSoMatHang.Text = $"Tổng số lượng: {flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => int.Parse(w.TbSL.Text))}";
                };

                flowLayoutPanel1.Controls.Add(widget2);
            }

            lblTongSoMatHang.Text = $"Tổng số lượng: {flowLayoutPanel1.Controls.OfType<Wiget2>().Sum(w => int.Parse(w.TbSL.Text))}";
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
        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!flowLayoutPanel1.Controls.OfType<Wiget2>().Any())
                {
                    MessageBox.Show("Hóa đơn trống! Vui lòng thêm vật liệu trước khi thanh toán.");
                    return;
                }

                foreach (Wiget2 widget in flowLayoutPanel1.Controls.OfType<Wiget2>())
                {
                    DataTable dt = busHoaDon.CheckSoLuongTonKho(widget.MaVatLieu);
                    int soLuongTon = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
                    int soLuong = int.Parse(widget.TbSL.Text);
                    if (soLuongTon < soLuong)
                    {
                        MessageBox.Show($"Số lượng tồn kho không đủ cho vật liệu {widget.Title2.Text}! (Tồn: {soLuongTon}, Yêu cầu: {soLuong})");
                        return;
                    }
                }

                foreach (var chiTiet in chiTietHoaDonTemps)
                {
                    busHoaDon.AddChiTietHoaDon(_maHoaDon.Value, chiTiet.MaVatLieu, chiTiet.SoLuong);
                }
                busHoaDon.UpdateTHoaDon(_maHoaDon.Value, "Đã thanh toán");

                MessageBox.Show("Thanh toán thành công!");
                Form1 frm1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                if (frm1 != null)
                {
                    frm1.Activate();
                    frm1.OpenChildForm(new FrmHoaDon());
                }
                else
                {
                    frm1 = new Form1();
                    frm1.Show(); 
                    frm1.OpenChildForm(new FrmHoaDon());
                    frm1.BringToFront();
                }

                
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (form != frm1 && (form is PopupHoaDon || form is EditHoaDon))
                    {
                        form.Close();
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message);
            }
        }

        private int CreateNewHoaDon()
        {
            try
            {
                int maTKLap = 1;
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

        private void EditHoaDon_Resize(object sender, EventArgs e)
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
    }
}