using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using ReaLTaiizor.Controls;
using Microsoft.Data.SqlClient;
using System.Data;
using DAL;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using GUI.Helpler;
using Microsoft.VisualBasic.ApplicationServices;
using DTO;
namespace GUI
{
    public partial class Login : Form
    {
        private System.Windows.Forms.Timer slideTimer = new System.Windows.Forms.Timer();
        private int targetX;
        private int labelTargetX;
        private bool isOverlayVisible = false;
        public Login()
        {
            InitializeComponent();
            SetRoundedRegion();
            ForgetPassLink.Click += ForgetPassLink_Click;
            BtnBack.Click += BtnBack_Click;
            BtnchangePw.Click += ChangePW_Click;
            this.MouseDown += Form1_MouseDown;
            var savedCredentials = AuthStorage.LoadCredentials();
            if (savedCredentials != null)
            {
                TbEmail.Text = savedCredentials.Email;
                Tbpass.Text = savedCredentials.Password;
                RdRemenber.Checked = true;
            }
            paneloverlay.Location = new Point(371, 0);
            tableLayoutPanel2.Location = new Point(12, 106);
            paneloverlay.Size = new Size(380, 450);
            paneloverlay.BringToFront();
            Title.Location = new Point(85, 196);
            Title.Visible = true;
            Title.Text = "Xin chào, bạn";
        }
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, 0x2, 0);
            }
        }


        private void SetupSlideAnimation(bool show)
        {
            slideTimer.Interval = 20;
            slideTimer.Tick -= SlideTimer_Tick;
            slideTimer.Tick += SlideTimer_Tick;

            if (show)
            {
                targetX = 0;
                labelTargetX = 10;
                paneloverlay.Location = new Point(371, 0);
                Title.Location = new Point(85, 196);
            }
            else
            {
                targetX = 371;
                labelTargetX = 85;
                paneloverlay.Location = new Point(0, 0);
                Title.Location = new Point(10, 196);
            }

            isOverlayVisible = show;
            slideTimer.Start();
        }

        private void SetRoundedRegion()
        {
            int radius = 10;
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Width - (radius * 2), rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Width - (radius * 2), rect.Height - (radius * 2), radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Height - (radius * 2), radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }

        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            int currentX = paneloverlay.Location.X;
            int labelX = Title.Location.X;
            int tableX = tableLayoutPanel2.Location.X;
            int speed = 20;

            if (isOverlayVisible)
            {
                if (currentX > targetX)
                {
                    currentX -= speed;
                    labelX -= speed;
                    tableX += speed;
                    if (currentX < targetX) currentX = targetX;
                    if (labelX < labelTargetX) labelX = labelTargetX;
                    if (tableX > 386) tableX = 386;
                    paneloverlay.Location = new Point(currentX, 0);
                    //Title.Location = new Point(labelX, 196);
                    tableLayoutPanel2.Location = new Point(tableX, 106);
                    tableLayoutPanel1.Visible = false;
                    Title.Text = "Đổi mật khẩu";
                }
                else
                {
                    slideTimer.Stop();
                }
            }
            else
            {
                if (currentX < targetX)
                {
                    currentX += speed;
                    labelX += speed;
                    tableX -= speed;
                    if (currentX > targetX) currentX = targetX;
                    if (labelX > labelTargetX) labelX = labelTargetX;
                    if (tableX < 12) tableX = 12;
                    paneloverlay.Location = new Point(currentX, 0);
                    //Title.Location = new Point(labelX, 196);
                    tableLayoutPanel2.Location = new Point(tableX, 106);
                    tableLayoutPanel1.Visible = true;
                    Title.Text = "Xin chào, bạn";
                }
                else
                {
                    slideTimer.Stop();
                }
            }
        }

        private void ForgetPassLink_Click(object sender, EventArgs e)
        {
            string email = TbEmail.Text;
            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    Console.WriteLine("Mở kết nối SQL...");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TaoTokenResetMatKhau", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlParameter tokenParam = new SqlParameter("@Token", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(tokenParam);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Stored procedure đã chạy...");

                    string token = tokenParam.Value == DBNull.Value ? string.Empty : tokenParam.Value.ToString();

                    if (!string.IsNullOrEmpty(token))
                    {
                        Console.WriteLine("Token tạo thành công, gửi email...");
                        MessageBox.Show("Đã gửi mã", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        try
                        {
                            SendResetEmail(email, token);
                            Console.WriteLine("Email đã được gửi!");
                        }
                        catch (Exception emailEx)
                        {
                            MessageBox.Show($"Gửi email thất bại: {emailEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        Task.Delay(500).Wait();
                        SetupSlideAnimation(true);
                    }
                    else
                    {
                        MessageBox.Show("Không thể tạo token. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SendResetEmail(string toEmail, string token)
        {
            string fromEmail = "bgia24071997@gmail.com";
            string fromPassword = "ydgw wqhq ctgx ajvp";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = "Đặt lại mật khẩu - Quản lý Vật tư";
            mail.Body = $"Mã token của bạn là: <b>{token}</b><br>Sử dụng token này để đặt lại mật khẩu. Token có hiệu lực trong 24 giờ.";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi email: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            SetupSlideAnimation(false);
        }
        private void hopeRoundButton1_Click(object sender, EventArgs e)
        {
            string username = TbEmail.Text;
            string password = Tbpass.Text;

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_KiemTraDangNhap", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() )
                        {
                            string role = reader["ChucVu"].ToString();
                            System.Windows.Forms.MessageBox.Show($"Đăng nhập thành công! Vai trò: {role}", "Thông Báo");

                            if (RdRemenber.Checked)
                            {
                                AuthStorage.SaveCredentials(username, password);
                            }
                            else
                            {
                                AuthStorage.ClearCredentials();
                            }
                            Thread.Sleep(500);
                            Form1 f = new Form1();
                            f.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Đăng nhập thất bại! Vui lòng kiểm tra lại.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ChangePW_Click(object sender, EventArgs e)
        {
            string token = TbToken.Text;
            string newPassword = TbNewPass.Text;

            using (SqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_ResetMatKhau", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@MatKhauMoi", newPassword);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Mật khẩu đã được đặt lại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Threading.Thread.Sleep(500);
                    SetupSlideAnimation(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void parrotPictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}