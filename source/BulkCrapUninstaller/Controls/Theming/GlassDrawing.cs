using System.Drawing;
using System.Drawing.Drawing2D;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class GlassDrawing
    {
        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            var diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void FillRoundedRectangle(Graphics graphics, Rectangle bounds, Color fill, Color edge, int radius)
        {
            using (var path = CreateRoundedRectangle(bounds, radius))
            using (var fillBrush = new SolidBrush(fill))
            using (var edgePen = new Pen(edge))
            {
                var previousSmoothingMode = graphics.SmoothingMode;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    graphics.FillPath(fillBrush, path);
                    graphics.DrawPath(edgePen, path);
                }
                finally
                {
                    graphics.SmoothingMode = previousSmoothingMode;
                }
            }
        }

        public static void FillGlassSurface(Graphics graphics, Rectangle bounds, Color fill, bool elevated)
        {
            var top = elevated ? Color.FromArgb(46, Color.White) : Color.FromArgb(28, Color.White);
            var edge = elevated ? IndustrialTheme.GlassEdgeStrong : IndustrialTheme.GlassEdge;

            using (var path = CreateRoundedRectangle(bounds, IndustrialTheme.CornerRadius))
            using (var baseBrush = new SolidBrush(fill))
            using (var sheenBrush = new LinearGradientBrush(bounds, top, Color.Transparent, LinearGradientMode.Vertical))
            using (var edgePen = new Pen(edge))
            {
                var previousSmoothingMode = graphics.SmoothingMode;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    graphics.FillPath(baseBrush, path);
                    graphics.FillPath(sheenBrush, path);
                    graphics.DrawPath(edgePen, path);
                }
                finally
                {
                    graphics.SmoothingMode = previousSmoothingMode;
                }
            }
        }
    }
}
