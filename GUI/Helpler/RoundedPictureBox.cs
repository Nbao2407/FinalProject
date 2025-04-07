using System.Drawing.Drawing2D;

namespace QLVT.Helper
{
    public class RoundedPictureBox : PictureBox
    {
        public int BorderRadius { get; set; } = 20;
        public Color BorderColor { get; set; } = Color.Black;
        public float BorderThickness { get; set; } = 2f;

        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);

            Region = new Region(grPath);

            base.OnPaint(pe);
            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                RectangleF rect = new RectangleF(
                    BorderThickness / 2,
                    BorderThickness / 2,
                    ClientSize.Width - BorderThickness,
                    ClientSize.Height - BorderThickness);

                g.DrawEllipse(pen, rect);
            }

            base.OnPaint(pe);
        }
    }
}