using System.Drawing.Drawing2D;

namespace GUI
{
    public partial class Form1 : Form
    {
      
        private bool isSidebarExpanded = true; // Trạng thái mặc định là mở rộng
        private const int SidebarExpandedWidth = 223;
        private const int SidebarCollapsedWidth = 60;
        private System.Windows.Forms.Timer sidebarTimer;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeSidebarToggle();
            this.IsMdiContainer = true;
            OpenChildForm(new FrmHome());
        }

        private void InitializeSidebarToggle()
        {
            sidebarTimer = new System.Windows.Forms.Timer();
            sidebarTimer.Interval = 10; // Tốc độ trượt
            sidebarTimer.Tick += SidebarTimer_Tick;
            ApplyButtonHoverEffect();
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            if (isSidebarExpanded)
            {
                if (panel2.Width > SidebarCollapsedWidth)
                    panel2.Width -= 10;
                else
                {
                    isSidebarExpanded = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                if (panel2.Width < SidebarExpandedWidth)
                    panel2.Width += 10;
                else
                {
                    isSidebarExpanded = true;
                    sidebarTimer.Stop();
                }
            }
        }
        private void ApplyButtonHoverEffect()
        {
            foreach (Control control in panel2.Controls)
            {
                if (control is Panel panel && panel.Controls.Count > 0)
                {
                    Button button = (Button)panel.Controls[0];
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(50, 50, 50);
                    button.MouseLeave += (s, e) => button.BackColor = Color.Black;
                    btnSidebar.MouseEnter += (s, e) => btnSidebar.BackColor = Color.FromArgb(50, 50, 50);
                    btnSidebar.MouseLeave += (s, e) => btnSidebar.BackColor = Color.Transparent;
                }
            }
        }

        private void CloseAllChildForms()
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close(); // Đóng tất cả form con đang mở
            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmHome());

        }

        private void OpenChildForm(Form childForm)
        {
            // Đóng form con hiện tại nếu có
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }

            // Mở form con mới
            childForm.MdiParent = this;
            childForm.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền form
            childForm.Dock = DockStyle.Fill; // Để form con lấp đầy Form1
            childForm.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCustomer());
        }

        private void btnMaterial_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmMaterial());
        }

        private void BtnType_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmType());
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmOrder());
        }

        private void BtnTke_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTke());
        }

        private void BtnNV_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNV());
        }


        private void BtnUser_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmUser());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
