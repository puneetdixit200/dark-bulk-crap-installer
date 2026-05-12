using System.Drawing;
using BulkCrapUninstaller.Controls.Theming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkCrapUninstallerTests.Ui
{
    [TestClass]
    public class IndustrialThemeTests
    {
        [TestMethod]
        public void PaletteMatchesApprovedIndustrialGlassSpec()
        {
            Assert.AreEqual(Color.FromArgb(5, 10, 16), IndustrialTheme.Backdrop);
            Assert.AreEqual(Color.FromArgb(11, 27, 50), IndustrialTheme.BackdropAccent);
            Assert.AreEqual(Color.FromArgb(255, 107, 0), IndustrialTheme.PrimaryAction);
            Assert.AreEqual(Color.FromArgb(62, 146, 255), IndustrialTheme.IndustrialBlue);
            Assert.AreEqual(Color.FromArgb(76, 175, 80), IndustrialTheme.Success);
            Assert.AreEqual(Color.FromArgb(255, 77, 77), IndustrialTheme.Critical);
            Assert.AreEqual(8, IndustrialTheme.CornerRadius);
        }

        [TestMethod]
        public void AlphaColorsRepresentGlassLayers()
        {
            Assert.AreEqual(166, IndustrialTheme.GlassPrimary.A);
            Assert.AreEqual(Color.FromArgb(166, 22, 32, 44), IndustrialTheme.GlassPrimary);
            Assert.AreEqual(Color.FromArgb(204, 28, 41, 56), IndustrialTheme.GlassElevated);
            Assert.AreEqual(Color.FromArgb(31, 255, 255, 255), IndustrialTheme.GlassEdge);
        }

        [TestMethod]
        public void PreferredFontFallsBackToExistingFont()
        {
            using (var fallback = new Font("Arial", 9f))
            using (var font = IndustrialTheme.CreateUiFont(fallback, FontStyle.Bold, 11f))
            {
                Assert.AreEqual(FontStyle.Bold, font.Style);
                Assert.AreEqual(11f, font.Size, 0.1f);
            }
        }
    }
}
