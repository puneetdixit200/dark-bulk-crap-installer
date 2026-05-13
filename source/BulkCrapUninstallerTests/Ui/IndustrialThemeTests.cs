using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using BulkCrapUninstaller.Controls.Theming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTreeMap;

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
            Assert.AreEqual(Color.FromArgb(255, 176, 74), IndustrialTheme.RowSelected);
            Assert.AreEqual(Color.Black, IndustrialTheme.RowSelectedText);
            Assert.AreEqual(Color.FromArgb(8, 17, 31), IndustrialTheme.StorageMapBack);
            Assert.AreEqual(Color.FromArgb(255, 176, 74), IndustrialTheme.StorageMapSelected);
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
        public void SolidControlBackColorsAreOpaque()
        {
            Assert.AreEqual(255, IndustrialTheme.GlassPrimarySolid.A);
            Assert.AreEqual(255, IndustrialTheme.GlassElevatedSolid.A);
            Assert.AreEqual(255, IndustrialTheme.ControlFillSolid.A);
            Assert.AreEqual(255, IndustrialTheme.ControlFillHotSolid.A);
            Assert.AreEqual(255, IndustrialTheme.BlendOverBackdrop(Color.FromArgb(64, Color.White)).A);
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

        [TestMethod]
        public void ButtonStylerUsesSupportedWinFormsBackColor()
        {
            using (var button = new Button { Name = "buttonNext", Text = "Next", Size = new Size(120, 32) })
            {
                IndustrialButtonStyler.Apply(button, IndustrialActionRole.Primary);

                Assert.AreNotEqual(Color.Transparent, button.BackColor);
                Assert.AreEqual(FlatStyle.Flat, button.FlatStyle);
                Assert.AreEqual(0, button.FlatAppearance.BorderSize);
            }
        }

        [TestMethod]
        public void ObjectListViewSelectionUsesReadableBlackText()
        {
            using (var listView = new BrightIdeasSoftware.ObjectListView())
            {
                IndustrialStyleManager.ApplyObjectListView(listView);

                Assert.AreEqual(IndustrialTheme.RowSelected, listView.SelectedBackColor);
                Assert.AreEqual(Color.Black, listView.SelectedForeColor);
                Assert.AreEqual(Color.Black, listView.UnfocusedSelectedForeColor);
            }
        }

        [TestMethod]
        public void LinkLabelsReceiveIndustrialReadableColors()
        {
            using (var form = new Form())
            using (var linkLabel = new LinkLabel())
            {
                form.Controls.Add(linkLabel);

                IndustrialStyleManager.Apply(form);

                Assert.AreEqual(IndustrialTheme.LinkText, linkLabel.LinkColor);
                Assert.AreEqual(IndustrialTheme.LinkTextHot, linkLabel.ActiveLinkColor);
                Assert.AreEqual(IndustrialTheme.TextMuted, linkLabel.VisitedLinkColor);
                Assert.AreEqual(IndustrialTheme.TextMuted, linkLabel.DisabledLinkColor);
            }
        }

        [TestMethod]
        public void StorageMapColorsStayDarkAgainstTheGlassShell()
        {
            var lightSource = Color.FromArgb(235, 248, 235);
            var darkened = IndustrialTheme.GetStorageMapColor(lightSource);

            Assert.IsTrue(darkened.R < 100);
            Assert.IsTrue(darkened.G < 120);
            Assert.IsTrue(darkened.B < 130);
            Assert.AreEqual(255, darkened.A);
        }

        [TestMethod]
        public void TreeMapReceivesIndustrialStorageStyling()
        {
            using (var form = new Form())
            using (var treeMap = new TreeMap())
            {
                form.Controls.Add(treeMap);

                IndustrialStyleManager.Apply(form);

                Assert.AreEqual(IndustrialTheme.StorageMapBack, treeMap.BackColor);
                Assert.AreEqual(IndustrialTheme.StorageMapBack, treeMap.CellBorderColor);
                Assert.AreEqual(IndustrialTheme.StorageMapSelected, treeMap.SelectedBackColor);
            }
        }

        [TestMethod]
        public void NativeDarkThemeHelpersAreSafeBeforeHandleCreation()
        {
            Assert.AreEqual("DarkMode_Explorer", NativeWindowTheme.DarkScrollbarThemeName);
            Assert.IsFalse(NativeWindowTheme.TryApplyDarkTitleBar(System.IntPtr.Zero));
            Assert.IsFalse(NativeWindowTheme.TryApplyDarkScrollbars(System.IntPtr.Zero));
        }

        [TestMethod]
        public void ButtonRoleMappingClassifiesDangerNames()
        {
            Assert.AreEqual(IndustrialActionRole.Danger, IndustrialStyleManager.GetButtonRole("buttonDelete", "Delete"));
            Assert.AreEqual(IndustrialActionRole.Danger, IndustrialStyleManager.GetButtonRole("deleteToolStripMenuItem", "Delete entry"));
        }

        [TestMethod]
        public void ButtonRoleMappingClassifiesPrimaryNames()
        {
            Assert.AreEqual(IndustrialActionRole.Primary, IndustrialStyleManager.GetButtonRole("buttonNext", "Next"));
            Assert.AreEqual(IndustrialActionRole.Primary, IndustrialStyleManager.GetButtonRole("toolStripButtonUninstall", "Uninstall"));
            Assert.AreEqual(IndustrialActionRole.Primary, IndustrialStyleManager.GetButtonRole("buttonFinish", "Finish"));
        }

        [TestMethod]
        public void ButtonRoleMappingDefaultsToNeutral()
        {
            Assert.AreEqual(IndustrialActionRole.Neutral, IndustrialStyleManager.GetButtonRole("buttonCancel", "Cancel"));
            Assert.AreEqual(IndustrialActionRole.Neutral, IndustrialStyleManager.GetButtonRole("buttonProperties", "Properties"));
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
