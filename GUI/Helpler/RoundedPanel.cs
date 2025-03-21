using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpler
{
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 20;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectF = new RectangleF(0, 0, Width, Height);

            using (GraphicsPath path = new GraphicsPath())
            {
                float radius = BorderRadius;
                path.AddArc(rectF.X, rectF.Y, radius, radius, 180, 90);
                path.AddArc(rectF.Right - radius, rectF.Y, radius, radius, 270, 90);
                path.AddArc(rectF.Right - radius, rectF.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rectF.X, rectF.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                Region = new Region(path);
            }

            base.OnPaint(e);
        }
    }
}
