using System.Drawing;
using System.Drawing.Text;
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
            Assert.AreEqual(Color.FromArgb(255, 127, 36), IndustrialTheme.PrimaryActionHot);
            Assert.AreEqual(Color.FromArgb(62, 146, 255), IndustrialTheme.IndustrialBlue);
            Assert.AreEqual(Color.FromArgb(76, 175, 80), IndustrialTheme.Success);
            Assert.AreEqual(Color.FromArgb(255, 77, 77), IndustrialTheme.Critical);
            Assert.AreEqual(Color.White, IndustrialTheme.TextHigh);
            Assert.AreEqual(Color.FromArgb(153, 248, 249, 255), IndustrialTheme.TextMuted);
            Assert.AreEqual(Color.FromArgb(24, 255, 255, 255), IndustrialTheme.ControlFill);
            Assert.AreEqual(Color.FromArgb(40, 255, 255, 255), IndustrialTheme.ControlFillHot);
            Assert.AreEqual(Color.FromArgb(12, 255, 255, 255), IndustrialTheme.RowAlternate);
            Assert.AreEqual(Color.FromArgb(42, 255, 107, 0), IndustrialTheme.RowSelected);
            Assert.AreEqual(8, IndustrialTheme.CornerRadius);
        }

        [TestMethod]
        public void AlphaColorsRepresentGlassLayers()
        {
            Assert.AreEqual(166, IndustrialTheme.GlassPrimary.A);
            Assert.AreEqual(Color.FromArgb(166, 22, 32, 44), IndustrialTheme.GlassPrimary);
            Assert.AreEqual(Color.FromArgb(204, 28, 41, 56), IndustrialTheme.GlassElevated);
            Assert.AreEqual(Color.FromArgb(31, 255, 255, 255), IndustrialTheme.GlassEdge);
            Assert.AreEqual(Color.FromArgb(51, 255, 255, 255), IndustrialTheme.GlassEdgeStrong);
        }

        [TestMethod]
        public void PreferredFontFallsBackToExistingFont()
        {
            using (var fallback = new Font("Arial", 9f))
            using (var font = IndustrialTheme.CreateUiFont(fallback, FontStyle.Bold, 11f))
            {
                var expectedFamilyName = IsFontInstalled("Hanken Grotesk") ? "Hanken Grotesk" : fallback.FontFamily.Name;

                Assert.AreEqual(expectedFamilyName, font.FontFamily.Name);
                Assert.AreEqual(FontStyle.Bold, font.Style);
                Assert.AreEqual(11f, font.Size, 0.1f);
            }
        }

        [TestMethod]
        public void ButtonBackColorsMatchRoleAndInteractionState()
        {
            Assert.AreEqual(IndustrialTheme.ControlFill, IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Neutral, false, false));
            Assert.AreEqual(IndustrialTheme.ControlFillHot, IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Neutral, true, false));
            Assert.AreEqual(Color.FromArgb(52, 255, 255, 255), IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Neutral, true, true));

            Assert.AreEqual(IndustrialTheme.PrimaryAction, IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Primary, false, false));
            Assert.AreEqual(IndustrialTheme.PrimaryActionHot, IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Primary, true, false));
            Assert.AreEqual(IndustrialTheme.PrimaryAction, IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Primary, true, true));

            Assert.AreEqual(Color.FromArgb(46, IndustrialTheme.Critical), IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Danger, false, false));
            Assert.AreEqual(Color.FromArgb(64, IndustrialTheme.Critical), IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Danger, true, false));
            Assert.AreEqual(Color.FromArgb(82, IndustrialTheme.Critical), IndustrialTheme.GetButtonBackColor(IndustrialActionRole.Danger, true, true));
        }

        private static bool IsFontInstalled(string fontFamilyName)
        {
            using (var fonts = new InstalledFontCollection())
            {
                foreach (var family in fonts.Families)
                {
                    if (family.Name == fontFamilyName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
