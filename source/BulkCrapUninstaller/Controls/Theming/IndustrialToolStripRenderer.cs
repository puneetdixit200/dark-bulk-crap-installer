using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public sealed class IndustrialToolStripRenderer : ToolStripProfessionalRenderer
    {
        public IndustrialToolStripRenderer()
            : base(new IndustrialToolStripColorTable())
        {
            RoundedEdges = true;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.Width <= 0 || e.ToolStrip.Height <= 0)
                return;

            var fill = e.ToolStrip is StatusStrip ? IndustrialTheme.GlassPrimary : IndustrialTheme.GlassElevated;
            using (var brush = new SolidBrush(fill))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.ToolStrip.Size));
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.Width <= 0 || e.ToolStrip.Height <= 0)
                return;

            using (var pen = new Pen(IndustrialTheme.GlassEdge))
            {
                e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var selected = e.Item.Selected;
            var pressed = e.Item.Pressed;

            if (!selected && !pressed)
                return;

            var bounds = new Rectangle(1, 1, e.Item.Width - 3, e.Item.Height - 3);
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var role = GetRole(e.Item);
            var fill = IndustrialTheme.GetButtonBackColor(role, selected, pressed);
            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, IndustrialTheme.GlassEdgeStrong, IndustrialTheme.CornerRadius);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var selected = e.Item.Selected;
            var pressed = e.Item.Pressed;

            if (!selected && !pressed)
                return;

            var bounds = new Rectangle(2, 1, e.Item.Width - 4, e.Item.Height - 2);
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var fill = pressed
                ? Color.FromArgb(72, IndustrialTheme.IndustrialBlue)
                : Color.FromArgb(46, IndustrialTheme.IndustrialBlue);
            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, IndustrialTheme.GlassEdgeStrong, IndustrialTheme.CornerRadius);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? IndustrialTheme.TextHigh : IndustrialTheme.TextMuted;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (var pen = new Pen(IndustrialTheme.GlassEdge))
            {
                if (e.Vertical)
                {
                    var x = e.Item.Width / 2;
                    e.Graphics.DrawLine(pen, x, 4, x, e.Item.Height - 4);
                }
                else
                {
                    e.Graphics.DrawLine(pen, 4, e.Item.Height / 2, e.Item.Width - 4, e.Item.Height / 2);
                }
            }
        }

        private static IndustrialActionRole GetRole(ToolStripItem item)
        {
            var name = item.Name ?? string.Empty;

            if (name.Contains("Uninstall") || name.Contains("Continue") || name.Contains("Next"))
                return IndustrialActionRole.Primary;

            if (name.Contains("Delete"))
                return IndustrialActionRole.Danger;

            return IndustrialActionRole.Neutral;
        }
    }
}
