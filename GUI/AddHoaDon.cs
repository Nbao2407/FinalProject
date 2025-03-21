using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class AddHoaDon : Form
    {
        public AddHoaDon()
        {
            InitializeComponent();
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Resize += AddHoaDon_Resize;
        }
        public void AddItem(string name, double price, Image image)
        {
            var widget = new Wiget()
            {
                Title = name,
                Gia = price,
                Images = image
            };
            flowLayoutPanel2.Controls.Add(widget);
        }
        private void AddHoaDon_Resize(object sender, EventArgs e)
        {
            int productListWidth = 550;
            splitContainer1.SplitterDistance = productListWidth;

            UpdateFlowLayoutPanel1Size();
        }

        private void UpdateFlowLayoutPanel1Size()
        {
            if (flowLayoutPanel1 != null)
            {
                flowLayoutPanel1.Left = 0;
                flowLayoutPanel1.Width = splitContainer1.Panel2.ClientSize.Width-10;
                flowLayoutPanel1.Height = splitContainer1.Panel2.ClientSize.Height-85;
            }
            if(WindowState == FormWindowState.Maximized)
            {
                flowLayoutPanel1.Width = splitContainer1.Panel2.ClientSize.Width - 50;
                flowLayoutPanel1.Height = splitContainer1.Panel2.ClientSize.Height - 85;
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
            // Thêm một số sản phẩm giả (dummy items)
            AddItem("Bàn gỗ", 1500000, Properties.Resources.freepik__background__67436);
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);      
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);     
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Bàn gỗ", 1500000, Properties.Resources.freepik__background__67436);
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Bàn gỗ", 1500000, Properties.Resources.freepik__background__67436);
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436); 
            AddItem("Ghế xoay", 750000, Properties.Resources.freepik__background__67436);
            AddItem("Tủ hồ sơ", 2000000, Properties.Resources.freepik__background__67436);
            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);

            AddItem("Đèn bàn", 300000, Properties.Resources.freepik__background__67436);
        }

       
    }
}