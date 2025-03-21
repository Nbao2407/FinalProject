using System.Drawing.Drawing2D;

namespace GUI
{
    public partial class Form1 : Form
    {
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
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeSidebarToggle();
            this.IsMdiContainer = true;
            panelMain.Dock = DockStyle.Fill;
            OpenChildForm(new FrmHome());
            this.Resize += Form1_Resize;
            SetDefaultFont(this, new Font("Segoe UI", 10, FontStyle.Regular));
            panel1.MouseDown += Form1_MouseDown;
            panel1.MouseMove += Form1_MouseMove;
            panel1.MouseUp += Form1_MouseUp;
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

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                this.Left = Cursor.Position.X - startPoint.X;
                this.Top = Cursor.Position.Y - startPoint.Y;
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
        private void RoundCorners(int radius)
        {
            if (WindowState == FormWindowState.Normal)
            {
                var path = new GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
                path.AddArc(0, Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                this.Region = null;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                RoundCorners(20); // 20 là bán kính bo góc
            else
                RoundCorners(0); // Hủy bo góc khi fullscreen
        }


        private void InitializeSidebarToggle()
        {
            sidebarTimer = new System.Windows.Forms.Timer();
            sidebarTimer.Interval = 10; // Tốc độ trượt
            sidebarTimer.Tick += SidebarTimer_Tick;
            ApplyButtonHoverEffect();
        }
        private async Task ToggleSidebar()
        {
            int step = 10;
            int delay = 5; // Giảm độ trễ để nhanh hơn

            this.SuspendLayout();

            if (isSidebarExpanded)
            {
                for (int i = panel2.Width; i >= SidebarCollapsedWidth; i -= step)
                {
                    panel2.Width = i;
                    AdjustPanelMain();
                    await Task.Delay(delay);
                }
                isSidebarExpanded = false;
            }
            else
            {
                for (int i = panel2.Width; i <= SidebarExpandedWidth; i += step)
                {
                    panel2.Width = i;
                    AdjustPanelMain();
                    await Task.Delay(delay);
                }
                isSidebarExpanded = true;
            }

            this.ResumeLayout();
        }

        private async void ToggleButton_Click(object sender, EventArgs e)
        {
            await ToggleSidebar();
        }

        private void SidebarTimer_Tick(object? sender, EventArgs e)
        {
            if (sender == null) return;

            this.SuspendLayout(); // Dừng cập nhật UI

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

            AdjustPanelMain(); // Cập nhật panelMain ngay sau khi thay đổi panel2.Width
            this.ResumeLayout(); // Tiếp tục cập nhật UI
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
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(50, 50, 50);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(44, 62, 80);
                    btnSidebar.MouseEnter += (s, e) => btnSidebar.BackColor = Color.FromArgb(50, 50, 50);
                    btnSidebar.MouseLeave += (s, e) => btnSidebar.BackColor = Color.Transparent;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild is not FrmHome)
            {
                OpenChildForm(new FrmHome());
            }
        }
        private void OpenChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(childForm);
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
            ShowPopup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AdjustPanelMain();
            panelMain.Left = panel2.Width;
            this.Resize += Form1_Resize;
        }
        private void ShowPopup()
        {
            Panel overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(50, 0, 0, 0),
                Parent = this,
                Visible = true
            };
            this.Controls.Add(overlay);

            overlay.BringToFront();
            this.Resize += (s, e) =>
            {
                overlay.Size = this.ClientSize;
            };
            using (var popup = new FrmUser())
            {
                popup.Deactivate += (s, e) => popup.TopMost = true;

                popup.StartPosition = FormStartPosition.CenterParent;

                popup.ShowDialog();
            }
            overlay.Dispose();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmHoaDon());
        }

        private void Ncc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNCc());
        }
    }
}
