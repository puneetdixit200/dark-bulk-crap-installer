# BCUninstaller Industrial Glass UI Refresh Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a full dark Industrial Precision Glass UI refresh for BCUninstaller and publish a Release output containing `BCUninstaller.exe`.

**Architecture:** Add a centralized WinForms theming layer under `source/BulkCrapUninstaller/Controls/Theming`, then apply it from existing form constructors after `InitializeComponent()`. Keep event handlers, control names, list behavior, uninstall logic, cleanup logic, and settings semantics unchanged.

**Tech Stack:** C#/.NET 8 WinForms, BrightIdeasSoftware ObjectListView, MSTest, Visual Studio 2022 MSBuild.

---

## File Structure

- Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialTheme.cs`: color, typography, spacing, and role constants.
- Create `source/BulkCrapUninstaller/Controls/Theming/GlassDrawing.cs`: rounded rectangle and glass-surface drawing helpers.
- Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripColorTable.cs`: dark color table for menu/toolbar/status renderers.
- Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripRenderer.cs`: owner-drawn menu, toolbar, dropdown, separator, and button backgrounds.
- Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialButtonStyler.cs`: paint and role styling for standard WinForms buttons.
- Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialStyleManager.cs`: recursive runtime styling for forms, controls, components, and ObjectListView.
- Create `source/BulkCrapUninstallerTests/Ui/IndustrialThemeTests.cs`: deterministic tests for palette and role mapping.
- Create `source/BulkCrapUninstallerTests/Ui/GlassDrawingTests.cs`: deterministic tests for drawing helper geometry.
- Modify `source/BulkCrapUninstaller/Forms/Windows/MainWindow.cs`: install renderer, apply theme, and style the main ObjectListView without touching behavior handlers.
- Modify these constructors to call the style manager after `InitializeComponent()`:
  - `source/BulkCrapUninstaller/Forms/Windows/SettingsWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Wizards/FirstStartBox.cs`
  - `source/BulkCrapUninstaller/Forms/Wizards/BeginUninstallTaskWizard.cs`
  - `source/BulkCrapUninstaller/Forms/Windows/UninstallProgressWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Windows/JunkRemoveWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/PropertiesWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/ListLegendWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/TargetWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/AdvancedClipboardCopyWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/FeedbackWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/FeedbackBox.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/RatingPopup.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/NewsPopup.cs`
  - `source/BulkCrapUninstaller/Forms/Helpers/DebugWindow.cs`
  - `source/BulkCrapUninstaller/Forms/Windows/AboutBox.cs`

## Commands

- MSBuild: `"C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe"`
- Restore: `& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Restore /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal`
- Tests: `& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal`
- Publish: `& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Publish /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal /p:filealignment=512 /p:DeployOnBuild=true /p:PublishSingleFile=False /p:SelfContained=False /p:PublishReadyToRun=false /p:PublishTrimmed=False /p:PublishProtocol=FileSystem /p:PublishDir="$PWD\bin\publish"`

---

### Task 1: Baseline Restore And Test Check

**Files:**
- Read: `source/BulkCrapUninstaller.sln`
- Read: `source/BulkCrapUninstallerTests/BulkCrapUninstallerTests.csproj`

- [ ] **Step 1: Restore the solution before UI edits**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Restore /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 2: Run the existing tests**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`, or a documented existing failure before any UI code changes.

- [ ] **Step 3: Commit only if the baseline required documentation changes**

If Step 2 exposes an existing failure, document it in `docs/superpowers/plans/2026-05-13-bcuninstaller-glass-ui-implementation.md` under a new `Baseline Notes` section and commit that note:

```powershell
git add docs\superpowers\plans\2026-05-13-bcuninstaller-glass-ui-implementation.md
git commit -m "Document baseline UI refresh verification"
```

Expected: no commit if restore and tests already pass.

**Baseline Notes (2026-05-13):**

- Restore command failed before UI edits with exit code `1`: several legacy projects report `MSB4057: The target "Restore" does not exist in the project`.
- Test command failed before UI edits with exit code `1`: `source/BulkCrapUninstallerTests/BulkCrapUninstallerTests.csproj` could not resolve SDK `Microsoft.NET.Sdk` from the Visual Studio MSBuild SDK path.

---

### Task 2: Theme Constants And Palette Tests

**Files:**
- Create: `source/BulkCrapUninstaller/Controls/Theming/IndustrialTheme.cs`
- Create: `source/BulkCrapUninstallerTests/Ui/IndustrialThemeTests.cs`

- [ ] **Step 1: Write palette tests**

Create `source/BulkCrapUninstallerTests/Ui/IndustrialThemeTests.cs`:

```csharp
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
            using var fallback = new Font("Arial", 9f);
            using var selected = IndustrialTheme.CreateUiFont(fallback, FontStyle.Bold, 11f);

            Assert.AreEqual(FontStyle.Bold, selected.Style);
            Assert.AreEqual(11f, selected.Size, 0.1f);
        }
    }
}
```

- [ ] **Step 2: Run the new tests to verify they fail**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: FAIL because `BulkCrapUninstaller.Controls.Theming.IndustrialTheme` does not exist.

- [ ] **Step 3: Implement theme constants**

Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialTheme.cs`:

```csharp
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
            var preferredSize = size ?? fallback.Size;
            try
            {
                return new Font("Hanken Grotesk", preferredSize, style, GraphicsUnit.Point);
            }
            catch
            {
                return new Font(fallback.FontFamily, preferredSize, style, GraphicsUnit.Point);
            }
        }

        public static Color GetButtonBackColor(IndustrialActionRole role, bool hot, bool pressed)
        {
            if (role == IndustrialActionRole.Primary)
                return pressed ? PrimaryAction : hot ? PrimaryActionHot : PrimaryAction;

            if (role == IndustrialActionRole.Danger)
                return pressed ? Color.FromArgb(82, Critical) : hot ? Color.FromArgb(64, Critical) : Color.FromArgb(46, Critical);

            return pressed ? Color.FromArgb(52, 255, 255, 255) : hot ? ControlFillHot : ControlFill;
        }
    }
}
```

- [ ] **Step 4: Run the palette tests**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: PASS for `IndustrialThemeTests`.

- [ ] **Step 5: Commit theme constants**

```powershell
git add source\BulkCrapUninstaller\Controls\Theming\IndustrialTheme.cs source\BulkCrapUninstallerTests\Ui\IndustrialThemeTests.cs
git commit -m "Add industrial glass theme palette"
```

Expected: commit contains only the theme constants and tests.

---

### Task 3: Rounded Drawing Helpers

**Files:**
- Create: `source/BulkCrapUninstaller/Controls/Theming/GlassDrawing.cs`
- Create: `source/BulkCrapUninstallerTests/Ui/GlassDrawingTests.cs`

- [ ] **Step 1: Write drawing helper tests**

Create `source/BulkCrapUninstallerTests/Ui/GlassDrawingTests.cs`:

```csharp
using System.Drawing;
using BulkCrapUninstaller.Controls.Theming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkCrapUninstallerTests.Ui
{
    [TestClass]
    public class GlassDrawingTests
    {
        [TestMethod]
        public void CreateRoundedRectangleKeepsInputBounds()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(4, 5, 120, 40), 8);

            Assert.AreEqual(new RectangleF(4, 5, 120, 40), path.GetBounds());
        }

        [TestMethod]
        public void CreateRoundedRectangleFallsBackToRectangleForZeroRadius()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(0, 0, 20, 10), 0);

            Assert.AreEqual(new RectangleF(0, 0, 20, 10), path.GetBounds());
            Assert.AreEqual(4, path.PointCount);
        }
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: FAIL because `GlassDrawing` does not exist.

- [ ] **Step 3: Implement drawing helpers**

Create `source/BulkCrapUninstaller/Controls/Theming/GlassDrawing.cs`:

```csharp
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
            using var path = CreateRoundedRectangle(bounds, radius);
            using var brush = new SolidBrush(fill);
            using var pen = new Pen(edge);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.DrawPath(pen, path);
        }

        public static void FillGlassSurface(Graphics graphics, Rectangle bounds, Color fill, bool elevated)
        {
            var top = elevated ? Color.FromArgb(46, Color.White) : Color.FromArgb(28, Color.White);
            using var path = CreateRoundedRectangle(bounds, IndustrialTheme.CornerRadius);
            using var baseBrush = new SolidBrush(fill);
            using var sheenBrush = new LinearGradientBrush(bounds, top, Color.Transparent, LinearGradientMode.Vertical);
            using var edgePen = new Pen(elevated ? IndustrialTheme.GlassEdgeStrong : IndustrialTheme.GlassEdge);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(baseBrush, path);
            graphics.FillPath(sheenBrush, path);
            graphics.DrawPath(edgePen, path);
        }
    }
}
```

- [ ] **Step 4: Run tests**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: PASS for `GlassDrawingTests`.

- [ ] **Step 5: Commit drawing helpers**

```powershell
git add source\BulkCrapUninstaller\Controls\Theming\GlassDrawing.cs source\BulkCrapUninstallerTests\Ui\GlassDrawingTests.cs
git commit -m "Add industrial glass drawing helpers"
```

Expected: commit contains drawing helpers and tests.

---

### Task 4: ToolStrip And Menu Renderer

**Files:**
- Create: `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripColorTable.cs`
- Create: `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripRenderer.cs`
- Modify: `source/BulkCrapUninstaller/Forms/Windows/MainWindow.cs`

- [ ] **Step 1: Add the color table**

Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripColorTable.cs`:

```csharp
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
```

- [ ] **Step 2: Add the renderer**

Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialToolStripRenderer.cs`:

```csharp
using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public sealed class IndustrialToolStripRenderer : ToolStripProfessionalRenderer
    {
        public IndustrialToolStripRenderer() : base(new IndustrialToolStripColorTable())
        {
            RoundedEdges = true;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var bounds = new Rectangle(Point.Empty, e.ToolStrip.Size);
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            using var brush = new SolidBrush(e.ToolStrip is StatusStrip ? IndustrialTheme.GlassPrimary : IndustrialTheme.GlassElevated);
            e.Graphics.FillRectangle(brush, bounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using var pen = new Pen(IndustrialTheme.GlassEdge);
            e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed) return;

            var role = GetRole(e.Item);
            var fill = IndustrialTheme.GetButtonBackColor(role, e.Item.Selected, e.Item.Pressed);
            var bounds = new Rectangle(1, 1, e.Item.Width - 3, e.Item.Height - 3);
            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, IndustrialTheme.GlassEdgeStrong, IndustrialTheme.CornerRadius);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed) return;

            var fill = e.Item.Pressed ? Color.FromArgb(72, IndustrialTheme.IndustrialBlue) : Color.FromArgb(46, IndustrialTheme.IndustrialBlue);
            var bounds = new Rectangle(2, 1, e.Item.Width - 4, e.Item.Height - 2);
            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, IndustrialTheme.GlassEdgeStrong, IndustrialTheme.CornerRadius);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? IndustrialTheme.TextHigh : IndustrialTheme.TextMuted;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using var pen = new Pen(IndustrialTheme.GlassEdge);
            var x = e.Vertical ? e.Item.Width / 2 : 2;
            if (e.Vertical)
                e.Graphics.DrawLine(pen, x, 4, x, e.Item.Height - 4);
            else
                e.Graphics.DrawLine(pen, 4, e.Item.Height / 2, e.Item.Width - 4, e.Item.Height / 2);
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
```

- [ ] **Step 3: Install the renderer in the main window**

Modify `source/BulkCrapUninstaller/Forms/Windows/MainWindow.cs`.

Add this using near the existing `using BulkCrapUninstaller.Controls;` line:

```csharp
using BulkCrapUninstaller.Controls.Theming;
```

Replace this block:

```csharp
ToolStripManager.Renderer = new ToolStripProfessionalRenderer(new StandardSystemColorTable())
{
    RoundedEdges = true
};
```

with:

```csharp
ToolStripManager.Renderer = new IndustrialToolStripRenderer();
```

- [ ] **Step 4: Build**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 5: Commit renderer**

```powershell
git add source\BulkCrapUninstaller\Controls\Theming\IndustrialToolStripColorTable.cs source\BulkCrapUninstaller\Controls\Theming\IndustrialToolStripRenderer.cs source\BulkCrapUninstaller\Forms\Windows\MainWindow.cs
git commit -m "Add industrial glass toolstrip renderer"
```

Expected: commit contains renderer files and one renderer replacement in `MainWindow.cs`.

---

### Task 5: Button Styling And Recursive Form Styling

**Files:**
- Create: `source/BulkCrapUninstaller/Controls/Theming/IndustrialButtonStyler.cs`
- Create: `source/BulkCrapUninstaller/Controls/Theming/IndustrialStyleManager.cs`
- Modify: `source/BulkCrapUninstallerTests/Ui/IndustrialThemeTests.cs`

- [ ] **Step 1: Add role mapping tests**

Append these tests to `source/BulkCrapUninstallerTests/Ui/IndustrialThemeTests.cs` inside `IndustrialThemeTests`:

```csharp
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
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: FAIL because `IndustrialStyleManager` does not exist.

- [ ] **Step 3: Add button painter**

Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialButtonStyler.cs`:

```csharp
using System.Drawing;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class IndustrialButtonStyler
    {
        public static void Apply(Button button, IndustrialActionRole role)
        {
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Color.Transparent;
            button.ForeColor = IndustrialTheme.TextHigh;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            button.Paint -= PaintButton;
            button.Paint += PaintButton;
            button.Tag = role;
            button.Resize -= ResizeButton;
            button.Resize += ResizeButton;
            ResizeButton(button, System.EventArgs.Empty);
        }

        private static void ResizeButton(object sender, System.EventArgs e)
        {
            if (sender is not Button button || button.Width <= 0 || button.Height <= 0) return;
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(Point.Empty, button.Size), IndustrialTheme.CornerRadius);
            button.Region = new Region(path);
        }

        private static void PaintButton(object sender, PaintEventArgs e)
        {
            if (sender is not Button button) return;

            var role = button.Tag is IndustrialActionRole typedRole ? typedRole : IndustrialActionRole.Neutral;
            var hot = button.ClientRectangle.Contains(button.PointToClient(Control.MousePosition));
            var pressed = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && hot;
            var bounds = new Rectangle(0, 0, button.Width - 1, button.Height - 1);
            var fill = IndustrialTheme.GetButtonBackColor(role, hot, pressed);
            var edge = button.Focused ? Color.FromArgb(180, IndustrialTheme.IndustrialBlue) : IndustrialTheme.GlassEdgeStrong;

            GlassDrawing.FillRoundedRectangle(e.Graphics, bounds, fill, edge, IndustrialTheme.CornerRadius);
            TextRenderer.DrawText(
                e.Graphics,
                button.Text,
                button.Font,
                bounds,
                button.Enabled ? button.ForeColor : IndustrialTheme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
```

- [ ] **Step 4: Add recursive style manager**

Create `source/BulkCrapUninstaller/Controls/Theming/IndustrialStyleManager.cs`:

```csharp
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Klocman.Extensions;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class IndustrialStyleManager
    {
        public static void Apply(Form form)
        {
            form.SuspendLayout();
            form.BackColor = IndustrialTheme.Backdrop;
            form.ForeColor = IndustrialTheme.TextHigh;
            form.Font = IndustrialTheme.CreateUiFont(form.Font);
            ToolStripManager.Renderer = new IndustrialToolStripRenderer();

            foreach (var control in form.GetAllChildren())
                ApplyControl(control);

            foreach (var component in form.GetComponents())
                ApplyComponent(component);

            form.ResumeLayout();
        }

        public static IndustrialActionRole GetButtonRole(string name, string text)
        {
            var key = string.Concat(name, " ", text).ToLowerInvariant();
            if (key.Contains("delete") || key.Contains("remove"))
                return IndustrialActionRole.Danger;
            if (key.Contains("uninstall") || key.Contains("next") || key.Contains("finish") || key.Contains("continue") || key.Contains("ok") || key.Contains("apply"))
                return IndustrialActionRole.Primary;
            return IndustrialActionRole.Neutral;
        }

        public static void ApplyObjectListView(ObjectListView listView)
        {
            listView.BackColor = IndustrialTheme.Backdrop;
            listView.ForeColor = IndustrialTheme.TextHigh;
            listView.AlternateRowBackColor = IndustrialTheme.RowAlternate;
            listView.SelectedBackColor = IndustrialTheme.RowSelected;
            listView.SelectedForeColor = IndustrialTheme.TextHigh;
            listView.UnfocusedSelectedBackColor = Color.FromArgb(34, IndustrialTheme.IndustrialBlue);
            listView.UnfocusedSelectedForeColor = IndustrialTheme.TextHigh;
            listView.GridLines = false;
            listView.HeaderUsesThemes = false;
            listView.HeaderFormatStyle = new HeaderFormatStyle();
            listView.HeaderFormatStyle.SetBackColor(IndustrialTheme.GlassElevated);
            listView.HeaderFormatStyle.SetForeColor(IndustrialTheme.TextHigh);
            listView.BorderStyle = BorderStyle.None;
        }

        private static void ApplyComponent(Component component)
        {
            if (component is ToolStrip strip)
            {
                strip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                strip.BackColor = IndustrialTheme.GlassPrimary;
                strip.ForeColor = IndustrialTheme.TextHigh;
                foreach (ToolStripItem item in strip.Items)
                    item.ForeColor = item.Enabled ? IndustrialTheme.TextHigh : IndustrialTheme.TextMuted;
            }
        }

        private static void ApplyControl(Control control)
        {
            control.ForeColor = IndustrialTheme.TextHigh;

            switch (control)
            {
                case ObjectListView objectListView:
                    ApplyObjectListView(objectListView);
                    break;
                case Button button:
                    IndustrialButtonStyler.Apply(button, GetButtonRole(button.Name, button.Text));
                    break;
                case TextBoxBase textBox:
                    textBox.BackColor = IndustrialTheme.GlassElevated;
                    textBox.ForeColor = IndustrialTheme.TextHigh;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case ComboBox comboBox:
                    comboBox.BackColor = IndustrialTheme.GlassElevated;
                    comboBox.ForeColor = IndustrialTheme.TextHigh;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;
                case TabPage tabPage:
                    tabPage.BackColor = IndustrialTheme.Backdrop;
                    tabPage.ForeColor = IndustrialTheme.TextHigh;
                    tabPage.UseVisualStyleBackColor = false;
                    break;
                case SplitContainer splitContainer:
                    splitContainer.BackColor = IndustrialTheme.Backdrop;
                    splitContainer.Panel1.BackColor = IndustrialTheme.Backdrop;
                    splitContainer.Panel2.BackColor = IndustrialTheme.Backdrop;
                    break;
                case GroupBox groupBox:
                    groupBox.BackColor = IndustrialTheme.GlassPrimary;
                    break;
                case Panel or FlowLayoutPanel or TableLayoutPanel:
                    control.BackColor = IndustrialTheme.GlassPrimary;
                    break;
                default:
                    if (control.BackColor == SystemColors.Control || control.BackColor == SystemColors.ControlLightLight)
                        control.BackColor = IndustrialTheme.Backdrop;
                    break;
            }
        }
    }
}
```

- [ ] **Step 5: Run tests**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: PASS for theme, role mapping, and drawing tests.

- [ ] **Step 6: Commit recursive styling**

```powershell
git add source\BulkCrapUninstaller\Controls\Theming\IndustrialButtonStyler.cs source\BulkCrapUninstaller\Controls\Theming\IndustrialStyleManager.cs source\BulkCrapUninstallerTests\Ui\IndustrialThemeTests.cs
git commit -m "Add industrial glass form styling"
```

Expected: commit contains styling manager, button painter, and role tests.

---

### Task 6: Main Window Styling

**Files:**
- Modify: `source/BulkCrapUninstaller/Forms/Windows/MainWindow.cs`
- Modify: `source/BulkCrapUninstaller/Forms/Windows/MainWindow.Designer.cs`

- [ ] **Step 1: Apply the theme after control setup**

In `source/BulkCrapUninstaller/Forms/Windows/MainWindow.cs`, keep the using from Task 4:

```csharp
using BulkCrapUninstaller.Controls.Theming;
```

After this line:

```csharp
_styleController = new WindowStyleController(this);
```

add:

```csharp
IndustrialStyleManager.Apply(this);
IndustrialStyleManager.ApplyObjectListView(uninstallerObjectListView);
```

- [ ] **Step 2: Remove the fixed border from the list container**

In `source/BulkCrapUninstaller/Forms/Windows/MainWindow.Designer.cs`, replace:

```csharp
listViewPanel.BorderStyle = BorderStyle.FixedSingle;
```

with:

```csharp
listViewPanel.BorderStyle = BorderStyle.None;
```

- [ ] **Step 3: Build**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 4: Commit main window styling**

```powershell
git add source\BulkCrapUninstaller\Forms\Windows\MainWindow.cs source\BulkCrapUninstaller\Forms\Windows\MainWindow.Designer.cs
git commit -m "Apply industrial glass theme to main window"
```

Expected: commit changes only main window style wiring and list panel border.

---

### Task 7: Dialog And Wizard Styling Sweep

**Files:**
- Modify: all form files listed in the File Structure constructor list.

- [ ] **Step 1: Add the theming using to each target form**

For each target `.cs` form file, add:

```csharp
using BulkCrapUninstaller.Controls.Theming;
```

Expected: the using is added only to files that call `IndustrialStyleManager.Apply(this);`.

- [ ] **Step 2: Apply style after InitializeComponent**

In each constructor, immediately after:

```csharp
InitializeComponent();
```

add:

```csharp
IndustrialStyleManager.Apply(this);
```

Target constructors:

```text
source\BulkCrapUninstaller\Forms\Windows\SettingsWindow.cs
source\BulkCrapUninstaller\Forms\Wizards\FirstStartBox.cs
source\BulkCrapUninstaller\Forms\Wizards\BeginUninstallTaskWizard.cs
source\BulkCrapUninstaller\Forms\Windows\UninstallProgressWindow.cs
source\BulkCrapUninstaller\Forms\Windows\JunkRemoveWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\PropertiesWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\ListLegendWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\TargetWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\AdvancedClipboardCopyWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\FeedbackWindow.cs
source\BulkCrapUninstaller\Forms\Helpers\FeedbackBox.cs
source\BulkCrapUninstaller\Forms\Helpers\RatingPopup.cs
source\BulkCrapUninstaller\Forms\Helpers\NewsPopup.cs
source\BulkCrapUninstaller\Forms\Helpers\DebugWindow.cs
source\BulkCrapUninstaller\Forms\Windows\AboutBox.cs
```

- [ ] **Step 3: Preserve event wiring**

Run this command and confirm that event handler lines still exist:

```powershell
rg -n "Click \\+=|CheckedChanged \\+=|SelectedIndexChanged \\+=|FormClosing \\+=|Load \\+=" source\BulkCrapUninstaller\Forms source\BulkCrapUninstaller\Controls
```

Expected: event wiring remains present. This task must not delete designer event subscriptions.

- [ ] **Step 4: Build**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 5: Commit dialog styling**

```powershell
git add source\BulkCrapUninstaller\Forms\Windows\SettingsWindow.cs source\BulkCrapUninstaller\Forms\Wizards\FirstStartBox.cs source\BulkCrapUninstaller\Forms\Wizards\BeginUninstallTaskWizard.cs source\BulkCrapUninstaller\Forms\Windows\UninstallProgressWindow.cs source\BulkCrapUninstaller\Forms\Windows\JunkRemoveWindow.cs source\BulkCrapUninstaller\Forms\Helpers\PropertiesWindow.cs source\BulkCrapUninstaller\Forms\Helpers\ListLegendWindow.cs source\BulkCrapUninstaller\Forms\Helpers\TargetWindow.cs source\BulkCrapUninstaller\Forms\Helpers\AdvancedClipboardCopyWindow.cs source\BulkCrapUninstaller\Forms\Helpers\FeedbackWindow.cs source\BulkCrapUninstaller\Forms\Helpers\FeedbackBox.cs source\BulkCrapUninstaller\Forms\Helpers\RatingPopup.cs source\BulkCrapUninstaller\Forms\Helpers\NewsPopup.cs source\BulkCrapUninstaller\Forms\Helpers\DebugWindow.cs source\BulkCrapUninstaller\Forms\Windows\AboutBox.cs
git commit -m "Apply industrial glass theme to dialogs"
```

Expected: commit contains constructor style calls and usings only.

---

### Task 8: Safe Visual Smoke Build

**Files:**
- Read: `source/BulkCrapUninstaller.sln`
- Read: `bin\Release\AnyCPU\BCUninstaller.exe`

- [ ] **Step 1: Run all tests**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstallerTests\BulkCrapUninstallerTests.csproj /t:VSTest /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 2: Build the app**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal
```

Expected: exit code `0`.

- [ ] **Step 3: Launch the Release app without running destructive actions**

Run:

```powershell
Start-Process -FilePath ".\bin\Release\AnyCPU\BCUninstaller.exe"
```

Expected: app starts. Inspect the main window, Settings, About, first-run wizard if it appears, and non-destructive menus only. Do not run uninstall, cleanup, registry delete, or ownership actions.

- [ ] **Step 4: Close the app**

Close the UI normally. If a process remains, close it from Task Manager or run:

```powershell
Get-Process BCUninstaller -ErrorAction SilentlyContinue | Stop-Process
```

Expected: no `BCUninstaller` process remains.

- [ ] **Step 5: Commit smoke fixes when smoke launch finds startup or rendering failures**

If smoke launch shows startup or rendering exceptions, fix only the failing UI code, re-run Steps 1-4, and commit:

```powershell
git add source\BulkCrapUninstaller\Controls\Theming source\BulkCrapUninstaller\Forms
git commit -m "Fix industrial glass UI smoke issues"
```

Expected: no commit if smoke launch succeeds without fixes.

---

### Task 9: Publish Final EXE

**Files:**
- Output: `bin\publish\BCUninstaller.exe`
- Output: `bin\publish\*.dll`
- Output: `bin\publish\BCU_manual.html`
- Output: `bin\publish\Licence.txt`
- Output: `bin\publish\PrivacyPolicy.txt`
- Output: `bin\publish\NOTICE`

- [ ] **Step 1: Clean previous publish output**

Run:

```powershell
if (Test-Path ".\bin\publish") { Remove-Item -LiteralPath ".\bin\publish" -Recurse -Force }
```

Expected: `bin\publish` does not exist after the command.

- [ ] **Step 2: Publish Release Any CPU**

Run:

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\amd64\MSBuild.exe" source\BulkCrapUninstaller.sln /t:Publish /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal /p:filealignment=512 /p:DeployOnBuild=true /p:PublishSingleFile=False /p:SelfContained=False /p:PublishReadyToRun=false /p:PublishTrimmed=False /p:PublishProtocol=FileSystem /p:PublishDir="$PWD\bin\publish"
```

Expected: exit code `0`.

- [ ] **Step 3: Verify final executable exists**

Run:

```powershell
Test-Path ".\bin\publish\BCUninstaller.exe"
Get-ChildItem ".\bin\publish" | Select-Object Name,Length | Sort-Object Name
```

Expected: first command prints `True`; file list includes `BCUninstaller.exe`.

- [ ] **Step 4: Attempt published smoke launch**

Run:

```powershell
Start-Process -FilePath ".\bin\publish\BCUninstaller.exe"
```

Expected: published app starts. Inspect only startup and non-destructive windows. Do not run uninstall or cleanup actions.

- [ ] **Step 5: Close published app**

Run only if the app remains open after manual close:

```powershell
Get-Process BCUninstaller -ErrorAction SilentlyContinue | Stop-Process
```

Expected: no `BCUninstaller` process remains.

- [ ] **Step 6: Report final artifact**

Final response must include:

```text
Published executable: C:\Users\mrpun\Documents\Codex\2026-05-13\https-github-com-klocman-bulk-crap\bin\publish\BCUninstaller.exe
```

Expected: user can run the exe from the publish folder.

---

## Self-Review Notes

- Spec coverage: Tasks 2-5 implement shared colors, glass drawing, round controls, toolstrip/menu renderer, focus and role styling. Task 6 implements the main window and ObjectListView. Task 7 implements major dialogs and wizards. Tasks 8-9 implement verification and final exe output.
- Behavior boundary: no task changes uninstall logic, cleanup logic, registry code, settings semantics, filters, update checks, ratings, hotkeys, import/export, or helper process behavior.
- Testing: deterministic tests cover palette, alpha layers, font fallback, role mapping, and rounded path geometry. Build, smoke launch, and publish verify integration.
