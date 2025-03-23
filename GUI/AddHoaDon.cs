namespace GUI
{
    public partial class AddHoaDon : Form
    {
        public AddHoaDon()
        {
            InitializeComponent();
      
            this.DoubleBuffered = true;
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Resize += AddHoaDon_Resize;
            flowLayoutPanel1.Resize += (s, e) =>
            {
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    ctrl.Width = flowLayoutPanel1.Width - 20;
                }
            };
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
        public void AddItem(string name, double price, Image image)
        {
            var widget = new Wiget()
            {
                Title = name,
                Gia = price,
                Images = image,
                Margin = new Padding(5)
            };
            flowLayoutPanel2.Controls.Add(widget);
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
            int maxSplitterDistance = splitContainer1.Width - 300; // Leave at least 300px for the right panel

            productListWidth = Math.Max(minSplitterDistance, Math.Min(productListWidth, maxSplitterDistance));

            // Set the splitter distance
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

        private void AddHoaDon_Load(object sender, EventArgs e)
        {
            AddItem("Bàn gỗ", 1500000, Properties.Resources.freepik__background__67436);
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddHoaDon_Resize(this, EventArgs.Empty);
        }
    }
}