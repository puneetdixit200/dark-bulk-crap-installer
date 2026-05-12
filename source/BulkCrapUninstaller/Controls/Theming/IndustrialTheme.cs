using System.Drawing;

namespace BulkCrapUninstaller.Controls.Theming
{
    public enum IndustrialActionRole
    {
        Neutral,
        Primary,
        Danger
    }

    public static class IndustrialTheme
    {
        public static readonly Color Backdrop = Color.FromArgb(5, 10, 16);
        public static readonly Color BackdropAccent = Color.FromArgb(11, 27, 50);
        public static readonly Color GlassPrimary = Color.FromArgb(166, 22, 32, 44);
        public static readonly Color GlassElevated = Color.FromArgb(204, 28, 41, 56);
        public static readonly Color GlassEdge = Color.FromArgb(31, 255, 255, 255);
        public static readonly Color GlassEdgeStrong = Color.FromArgb(51, 255, 255, 255);
        public static readonly Color PrimaryAction = Color.FromArgb(255, 107, 0);
        public static readonly Color PrimaryActionHot = Color.FromArgb(255, 127, 36);
        public static readonly Color IndustrialBlue = Color.FromArgb(62, 146, 255);
        public static readonly Color Success = Color.FromArgb(76, 175, 80);
        public static readonly Color Critical = Color.FromArgb(255, 77, 77);
        public static readonly Color TextHigh = Color.White;
        public static readonly Color TextMuted = Color.FromArgb(153, 248, 249, 255);
        public static readonly Color ControlFill = Color.FromArgb(24, 255, 255, 255);
        public static readonly Color ControlFillHot = Color.FromArgb(40, 255, 255, 255);
        public static readonly Color RowAlternate = Color.FromArgb(12, 255, 255, 255);
        public static readonly Color RowSelected = Color.FromArgb(42, 255, 107, 0);

        public const int CornerRadius = 8;
        public const int FocusGlowWidth = 4;

        public static Font CreateUiFont(Font fallback, FontStyle style = FontStyle.Regular, float? size = null)
        {
            var fontSize = size ?? fallback.Size;

            try
            {
                return new Font("Hanken Grotesk", fontSize, style);
            }
            catch
            {
                return new Font(fallback.FontFamily, fontSize, style);
            }
        }

        public static Color GetButtonBackColor(IndustrialActionRole role, bool hot, bool pressed)
        {
            switch (role)
            {
                case IndustrialActionRole.Primary:
                    return pressed ? PrimaryAction : hot ? PrimaryActionHot : PrimaryAction;
                case IndustrialActionRole.Danger:
                    return pressed ? Color.FromArgb(82, Critical) : hot ? Color.FromArgb(64, Critical) : Color.FromArgb(46, Critical);
                default:
                    return pressed ? Color.FromArgb(52, 255, 255, 255) : hot ? ControlFillHot : ControlFill;
            }
        }
    }
}
