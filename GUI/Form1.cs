using BUS;
using DTO;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace QLVT
{
    public partial class Form1 : Form
    {
        private GraphicsPath roundedPath;
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        private FormWindowState lastWindowState = FormWindowState.Normal;
        private bool isSidebarExpanded = true;
        private const int SidebarExpandedWidth = 223;
        private const int SidebarCollapsedWidth = 60;
        private System.Windows.Forms.Timer sidebarTimer = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeSidebarToggle();
            this.IsMdiContainer = true;
            panelMain.Dock = DockStyle.Fill;
            OpenChildForm(new FrmHome());
            roundedPath = CreateRoundedPath(20);
            this.Region = new Region(roundedPath);
            SetDefaultFont(this, new Font("Segoe UI", 10, FontStyle.Regular));
            panel1.MouseDown += Form1_MouseDown;
            panel1.MouseMove += Form1_MouseMove;
            panel1.MouseUp += Form1_MouseUp;
            Quyen();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(Cursor.Position.X - this.Width / 2, Cursor.Position.Y - 10);
            }
        }

        private GraphicsPath CreateRoundedPath(int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
            path.AddArc(0, Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point newPoint = new Point(Cursor.Position.X - startPoint.X, Cursor.Position.Y - startPoint.Y);
                if (Math.Abs(newPoint.X - this.Left) > 5 || Math.Abs(newPoint.Y - this.Top) > 5)
                {
                    this.Left = newPoint.X;
                    this.Top = newPoint.Y;
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void SetDefaultFont(Control parent, Font font)
        {
            parent.Font = font;
            foreach (Control ctrl in parent.Controls)
            {
                SetDefaultFont(ctrl, font);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != lastWindowState)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    roundedPath = CreateRoundedPath(20);
                    this.Region = new Region(roundedPath);
                }
                else
                {
                    this.Region = null;
                }
                lastWindowState = WindowState;
            }
            AdjustPanelMain();
        }

        private void InitializeSidebarToggle()
        {
            sidebarTimer = new System.Windows.Forms.Timer();
            sidebarTimer.Interval = 30;
            sidebarTimer.Tick += SidebarTimer_Tick;
            ApplyButtonHoverEffect();
        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            this.SuspendLayout();
            panelMain.SuspendLayout();
            int step = 10;

            if (isSidebarExpanded)
            {
                if (panel2.Width > SidebarCollapsedWidth)
                {
                    panel2.Width = Math.Max(panel2.Width - step, SidebarCollapsedWidth);
                }
                else
                {
                    isSidebarExpanded = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                if (panel2.Width < SidebarExpandedWidth)
                {
                    panel2.Width = Math.Min(panel2.Width + step, SidebarExpandedWidth);
                }
                else
                {
                    isSidebarExpanded = true;
                    sidebarTimer.Stop();
                }
            }
            AdjustPanelMain();
            panelMain.ResumeLayout();
            this.ResumeLayout();
        }

        private void AdjustPanelMain()
        {
            panelMain.Left = panel2.Width;
            panelMain.Width = this.ClientSize.Width - panel2.Width;
        }

        private void ApplyButtonHoverEffect()
        {
            foreach (Control control in panel2.Controls)
            {
                if (control is Panel panel && panel.Controls.Count > 0)
                {
                    Button button = (Button)panel.Controls[0];
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(147, 197, 253);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(30, 58, 138);
                    btnSidebar.MouseEnter += (s, e) => btnSidebar.BackColor = Color.FromArgb(50, 50, 50);
                    btnSidebar.MouseLeave += (s, e) => btnSidebar.BackColor = Color.Transparent;
                }
            }
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AdjustPanelMain();
            panelMain.Left = panel2.Width;
            this.Resize += Form1_Resize;
        }

        public void OpenChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(childForm);
            childForm.Show();
        }

        private void button2_Click(object sender, EventArgs e) => OpenChildForm(new FrmHome());

        private void btnCustomer_Click(object sender, EventArgs e) => OpenChildForm(new FrmCustomer());

        private void btnMaterial_Click(object sender, EventArgs e) => OpenChildForm(new FrmMaterial());

        private void BtnType_Click(object sender, EventArgs e) => OpenChildForm(new FrmType());

        private void BtnOrder_Click(object sender, EventArgs e) => OpenChildForm(new FrmOrder());

        private void BtnTke_Click(object sender, EventArgs e) => OpenChildForm(new Tke());

        private void BtnNV_Click(object sender, EventArgs e) => OpenChildForm(new FrmNV());

        private void button3_Click(object sender, EventArgs e) => OpenChildForm(new FrmHoaDon());

        private void Ncc_Click(object sender, EventArgs e) => OpenChildForm(new FrmNCc());
        private void BtnPhieu_Click(object sender, EventArgs e) => OpenChildForm(new FrmXuat());
        private void BtnUser_Click(object sender, EventArgs e)
        {
            DTO_TK user = new DTO_TK();
            ShowPopup(user);
        }
        private void Quyen()
        {
            if (CurrentUser.ChucVu == "Nhân viên") 
            {
                button1.Enabled = false;
                BtnTke.Enabled = false;
                panel14.Location = new Point(4, 260);
                panel13.Location = new Point(6, 209);
            }
            else 
            {
                panel14.Location = new Point(4, 465);
                panel13.Location = new Point(4, 414);


            }
        }
        private void ShowPopup(DTO_TK tk)
        {
            using (var popup = new FrmUser(tk))
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }
    }
}