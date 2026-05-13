using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class IndustrialButtonStyler
    {
        private sealed class ButtonStyleState
        {
            public IndustrialActionRole Role { get; set; }
        }

        private static readonly ConditionalWeakTable<Button, ButtonStyleState> ButtonStates =
            new ConditionalWeakTable<Button, ButtonStyleState>();

        public static void Apply(Button button, IndustrialActionRole role)
        {
            if (button == null)
                return;

            SetRole(button, role);

            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = IndustrialTheme.BlendOverBackdrop(IndustrialTheme.GetButtonBackColor(role, false, false));
            button.ForeColor = IndustrialTheme.TextHigh;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = IndustrialTheme.BlendOverBackdrop(IndustrialTheme.GetButtonBackColor(role, true, true));
            button.FlatAppearance.MouseOverBackColor = IndustrialTheme.BlendOverBackdrop(IndustrialTheme.GetButtonBackColor(role, true, false));

            button.Paint -= PaintButton;
            button.Paint += PaintButton;
            button.Resize -= ResizeButton;
            button.Resize += ResizeButton;

            UpdateRegion(button);
            button.Invalidate();
        }

        private static void SetRole(Button button, IndustrialActionRole role)
        {
            ButtonStates.Remove(button);
            ButtonStates.Add(button, new ButtonStyleState { Role = role });
        }

        private static IndustrialActionRole GetRole(Button button)
        {
            ButtonStyleState state;
            return ButtonStates.TryGetValue(button, out state)
                ? state.Role
                : IndustrialStyleManager.GetButtonRole(button.Name, button.Text);
        }

        private static void ResizeButton(object sender, System.EventArgs e)
        {
            UpdateRegion(sender as Button);
        }

        private static void UpdateRegion(Button button)
        {
            if (button == null || button.Width <= 0 || button.Height <= 0)
                return;

            using (var path = GlassDrawing.CreateRoundedRectangle(
                new Rectangle(Point.Empty, button.Size),
                IndustrialTheme.CornerRadius))
            {
                var previousRegion = button.Region;
                button.Region = new Region(path);
                if (previousRegion != null)
                    previousRegion.Dispose();
            }
        }

        private static void PaintButton(object sender, PaintEventArgs e)
        {
            var button = sender as Button;
            if (button == null || button.Width <= 0 || button.Height <= 0)
                return;

            var bounds = new Rectangle(0, 0, button.Width - 1, button.Height - 1);
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var hot = button.ClientRectangle.Contains(button.PointToClient(Control.MousePosition));
            var pressed = hot && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
            var role = GetRole(button);
            var fill = IndustrialTheme.GetButtonBackColor(role, hot, pressed);
            var edge = button.Focused
                ? Color.FromArgb(180, IndustrialTheme.IndustrialBlue)
                : IndustrialTheme.GlassEdgeStrong;

            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, edge, IndustrialTheme.CornerRadius);

            var textColor = button.Enabled ? button.ForeColor : IndustrialTheme.TextMuted;
            TextRenderer.DrawText(
                e.Graphics,
                button.Text,
                button.Font,
                bounds,
                textColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.SingleLine);
        }
    }
}
