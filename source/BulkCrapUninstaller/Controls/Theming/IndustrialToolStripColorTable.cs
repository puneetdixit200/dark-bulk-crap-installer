using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public sealed class IndustrialToolStripColorTable : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin => IndustrialTheme.GlassElevated;
        public override Color ToolStripGradientMiddle => IndustrialTheme.GlassPrimary;
        public override Color ToolStripGradientEnd => IndustrialTheme.GlassPrimary;
        public override Color MenuStripGradientBegin => IndustrialTheme.GlassElevated;
        public override Color MenuStripGradientEnd => IndustrialTheme.GlassPrimary;
        public override Color StatusStripGradientBegin => IndustrialTheme.GlassPrimary;
        public override Color StatusStripGradientEnd => IndustrialTheme.GlassPrimary;
        public override Color ImageMarginGradientBegin => IndustrialTheme.GlassElevated;
        public override Color ImageMarginGradientMiddle => IndustrialTheme.GlassElevated;
        public override Color ImageMarginGradientEnd => IndustrialTheme.GlassPrimary;
        public override Color MenuItemSelected => Color.FromArgb(46, IndustrialTheme.IndustrialBlue);
        public override Color MenuItemBorder => IndustrialTheme.GlassEdgeStrong;
        public override Color ButtonSelectedBorder => IndustrialTheme.GlassEdgeStrong;
        public override Color ButtonPressedBorder => Color.FromArgb(128, IndustrialTheme.PrimaryAction);
        public override Color SeparatorDark => Color.FromArgb(42, Color.White);
        public override Color SeparatorLight => Color.FromArgb(12, Color.White);
    }
}
