﻿using System;
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
using QLVT.Helper;
using Microsoft.VisualBasic.ApplicationServices;
using DTO;
using Mono.Unix.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using BUS;
namespace QLVT
{
    public partial class Login : Form
    {
        private System.Windows.Forms.Timer slideTimer = new System.Windows.Forms.Timer();
        private int targetX;
        private int labelTargetX;
        private bool isOverlayVisible = false;
        private BUS_Login bus_Login = new BUS_Login();
        public Login()
        {
            InitializeComponent();
            SetRoundedRegion();
            ForgetPassLink.Click += ForgetPassLink_Click;
            BtnBack.Click += BtnBack_Click;
            BtnchangePw.Click += ChangePW_Click;
            this.MouseDown += Form1_MouseDown;
            TbEmail.Focus();
            var savedCredentials = AuthStorage.LoadCredentials();
            if (savedCredentials != null)
            {
                TbEmail.Text = savedCredentials.Email;
                TbEmail.Text = savedCredentials.Username;
                Tbpass.Text = savedCredentials.Password;
                TbNewPass.UseSystemPasswordChar = true;
                Tbpass.UseSystemPasswordChar = true;
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
                        try
                        {
                            SendResetEmail(email, token);
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
            string username = TbEmail.Text.Trim();
            string email = TbEmail.Text.Trim();
            string password = Tbpass.Text;

            try
            {
                if (string.IsNullOrWhiteSpace(password) || (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email)))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu và ít nhất một trong hai: tên đăng nhập hoặc email!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = bus_Login.kiemtradangnhap(username, password, email);
                if (user != null)
                {
                    MessageBox.Show("Đăng nhập thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CurrentUser.SetUser(user.Id, user.Username, user.Role);

                    if (RdRemenber.Checked)
                    {
                        AuthStorage.SaveCredentials(username, email, password);
                    }
                    else
                    {
                        AuthStorage.ClearCredentials();
                    }
                    Form1 form = new Form1();
                    form.Show();
                    this.Hide();
                }
                else
                {
                    if (bus_Login.CheckAccountExists(username, email) && !bus_Login.IsAccountActive(username, email))
                    {
                        MessageBox.Show("Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập, email hoặc mật khẩu không đúng!", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    System.Threading.Thread.Sleep(2000);
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
            string username = TbEmail.Text;
            string password = Tbpass.Text;
            string email = TbEmail.Text;

            if (RdRemenber.Checked)
            {
                AuthStorage.SaveCredentials(username, email, password);
            }
            else
            {
                AuthStorage.ClearCredentials();
            }
            Application.Exit();
        }

        private void TbEmail_Enter(object sender, EventArgs e)
        {
            if (TbEmail.Text == "Email/Username")
            {
                TbEmail.Text = "";
                TbEmail.ForeColor = Color.Black;
            }
        }

        private void TbEmail_Leave_1(object sender, EventArgs e)
        {
            if (TbEmail.Text == "")
            {
                TbEmail.Text = "Email/Username";
                TbEmail.ForeColor = Color.Gray;
            }
        }
        private void Tbpass_Enter(object sender, EventArgs e)
        {
            if (Tbpass.Text == "Password")
            {
                Tbpass.Text = "";
                Tbpass.ForeColor = Color.Black;
                Tbpass.UseSystemPasswordChar = true;
            }
        }
        private void Tbpass_Leave(object sender, EventArgs e)
        {
            if (Tbpass.Text == "")
            {
                Tbpass.Text = "Password";
                Tbpass.ForeColor = Color.Gray;
                Tbpass.UseSystemPasswordChar = false;
            }
        }
        private void TbToken_Enter(object sender, EventArgs e)
        {
            if (TbToken.Text == "Token")
            {
                TbToken.Text = "";
                TbToken.ForeColor = Color.Black;
            }
        }
        private void TbToken_Leave(object sender, EventArgs e)
        {
            if (TbToken.Text == "")
            {
                TbToken.Text = "Token";
                TbToken.ForeColor = Color.Gray;
            }
        }
        private void TbNewPass_Enter(object sender, EventArgs e)
        {
            if (TbNewPass.Text == "Mật khẩu mới")
            {
                TbNewPass.Text = "";
                TbNewPass.ForeColor = Color.Black;
                TbNewPass.UseSystemPasswordChar = true;
            }
        }
        private void TbNewpass_Leave(object sender, EventArgs e)
        {
            if (TbNewPass.Text == "")
            {
                TbNewPass.Text = "Mật khẩu mới";
                TbNewPass.ForeColor = Color.Gray;
                TbNewPass.UseSystemPasswordChar = false;
            }
        }

        private void hopeRoundButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                hopeRoundButton1_Click(sender, e);
            }
        }
    }
}