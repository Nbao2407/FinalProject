using System.Drawing.Drawing2D;

namespace QLVT.Helper
{
    public static class PopupHelper
    {
        public static void RoundCorners(Form form, int radius)
        {
            if (form.Width < radius * 2 || form.Height < radius * 2)
                return; // Tránh lỗi khi form quá nhỏ

            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(0, 0, diameter, diameter, 180, 90);
            path.AddArc(form.Width - diameter, 0, diameter, diameter, 270, 90);
            path.AddArc(form.Width - diameter, form.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(0, form.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            form.Region = new Region(path);
        }

        public static void MakeTextBoxesTransparent(Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BackColor = container.BackColor;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.Parent = container;
                }
            }
        }
        public static void changecolor(Form form) => form.BackColor = Color.FromArgb(245, 247, 250);
    }
}