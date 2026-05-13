using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Klocman.Extensions;
using SimpleTreeMap;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class IndustrialStyleManager
    {
        public static void Apply(Form form)
        {
            if (form == null)
                return;

            form.SuspendLayout();
            try
            {
                form.BackColor = IndustrialTheme.Backdrop;
                form.ForeColor = IndustrialTheme.TextHigh;
                form.Font = IndustrialTheme.CreateUiFont(form.Font);
                NativeWindowTheme.EnableDarkTitleBar(form);

                ToolStripManager.Renderer = new IndustrialToolStripRenderer();

                foreach (var control in form.GetAllChildren())
                {
                    ApplyControl(control);
                }

                foreach (var component in form.GetComponents())
                {
                    ApplyComponent(component);
                }
            }
            finally
            {
                form.ResumeLayout();
            }
        }

        public static IndustrialActionRole GetButtonRole(string name, string text)
        {
            var value = ((name ?? string.Empty) + " " + (text ?? string.Empty)).ToLowerInvariant();

            if (value.Contains("delete") || value.Contains("remove"))
                return IndustrialActionRole.Danger;

            if (value.Contains("uninstall") ||
                value.Contains("next") ||
                value.Contains("finish") ||
                value.Contains("continue") ||
                value.Contains("ok") ||
                value.Contains("apply"))
            {
                return IndustrialActionRole.Primary;
            }

            return IndustrialActionRole.Neutral;
        }

        public static void ApplyObjectListView(ObjectListView listView)
        {
            if (listView == null)
                return;

            listView.BackColor = IndustrialTheme.Backdrop;
            listView.ForeColor = IndustrialTheme.TextHigh;
            listView.AlternateRowBackColor = IndustrialTheme.RowAlternate;
            listView.SelectedBackColor = IndustrialTheme.RowSelected;
            listView.SelectedForeColor = IndustrialTheme.RowSelectedText;
            listView.UnfocusedSelectedBackColor = IndustrialTheme.RowSelected;
            listView.UnfocusedSelectedForeColor = IndustrialTheme.RowSelectedText;
            listView.GridLines = false;
            listView.HeaderUsesThemes = false;
            listView.HeaderFormatStyle = new HeaderFormatStyle();
            listView.HeaderFormatStyle.SetBackColor(IndustrialTheme.GlassElevated);
            listView.HeaderFormatStyle.SetForeColor(IndustrialTheme.TextHigh);
            listView.BorderStyle = BorderStyle.None;
        }

        private static void ApplyLinkLabel(LinkLabel linkLabel)
        {
            linkLabel.BackColor = IndustrialTheme.Backdrop;
            linkLabel.ForeColor = IndustrialTheme.TextHigh;
            linkLabel.LinkColor = IndustrialTheme.LinkText;
            linkLabel.ActiveLinkColor = IndustrialTheme.LinkTextHot;
            linkLabel.VisitedLinkColor = IndustrialTheme.TextMuted;
            linkLabel.DisabledLinkColor = IndustrialTheme.TextMuted;
        }

        private static void ApplyComponent(Component component)
        {
            var toolStrip = component as ToolStrip;
            if (toolStrip == null)
                return;

            ApplyToolStrip(toolStrip);
        }

        private static void ApplyToolStrip(ToolStrip toolStrip)
        {
            toolStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            toolStrip.BackColor = IndustrialTheme.GlassPrimarySolid;
            toolStrip.ForeColor = IndustrialTheme.TextHigh;
            ApplyToolStripItems(toolStrip.Items);
        }

        private static void ApplyToolStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                item.ForeColor = IndustrialTheme.TextHigh;
                item.Image = CreateWhiteToolStripImage(item.Image);

                var dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                    ApplyToolStripItems(dropDownItem.DropDownItems);
            }
        }

        private static Image CreateWhiteToolStripImage(Image source)
        {
            if (source == null)
                return null;

            var result = new Bitmap(source.Width, source.Height);

            using (var sourceBitmap = new Bitmap(source))
            {
                for (var x = 0; x < source.Width; x++)
                {
                    for (var y = 0; y < source.Height; y++)
                    {
                        var pixel = sourceBitmap.GetPixel(x, y);
                        result.SetPixel(x, y, Color.FromArgb(pixel.A, IndustrialTheme.TextHigh));
                    }
                }
            }

            return result;
        }

        private static void ApplyControl(Control control)
        {
            if (control == null)
                return;

            control.ForeColor = IndustrialTheme.TextHigh;
            ApplyDarkScrollableTheme(control);

            var toolStrip = control as ToolStrip;
            if (toolStrip != null)
            {
                ApplyToolStrip(toolStrip);
                return;
            }

            var listView = control as ObjectListView;
            if (listView != null)
            {
                ApplyObjectListView(listView);
                return;
            }

            var treeMap = control as TreeMap;
            if (treeMap != null)
            {
                ApplyTreeMap(treeMap);
                return;
            }

            var linkLabel = control as LinkLabel;
            if (linkLabel != null)
            {
                ApplyLinkLabel(linkLabel);
                return;
            }

            var button = control as Button;
            if (button != null)
            {
                IndustrialButtonStyler.Apply(button, GetButtonRole(button.Name, button.Text));
                return;
            }

            var textBoxBase = control as TextBoxBase;
            if (textBoxBase != null)
            {
                textBoxBase.BackColor = IndustrialTheme.GlassElevatedSolid;
                textBoxBase.ForeColor = IndustrialTheme.TextHigh;
                textBoxBase.BorderStyle = BorderStyle.FixedSingle;
                return;
            }

            var comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = IndustrialTheme.GlassElevatedSolid;
                comboBox.ForeColor = IndustrialTheme.TextHigh;
                comboBox.FlatStyle = FlatStyle.Flat;
                return;
            }

            var tabPage = control as TabPage;
            if (tabPage != null)
            {
                tabPage.BackColor = IndustrialTheme.Backdrop;
                tabPage.ForeColor = IndustrialTheme.TextHigh;
                tabPage.UseVisualStyleBackColor = false;
                return;
            }

            var splitContainer = control as SplitContainer;
            if (splitContainer != null)
            {
                splitContainer.BackColor = IndustrialTheme.Backdrop;
                splitContainer.Panel1.BackColor = IndustrialTheme.Backdrop;
                splitContainer.Panel2.BackColor = IndustrialTheme.Backdrop;
                return;
            }

            if (control is GroupBox)
            {
                control.BackColor = IndustrialTheme.GlassPrimarySolid;
                return;
            }

            if (control is Panel || control is FlowLayoutPanel || control is TableLayoutPanel)
            {
                control.BackColor = IndustrialTheme.GlassPrimarySolid;
                return;
            }

            if (control.BackColor == SystemColors.Control || control.BackColor == SystemColors.ControlLightLight)
                control.BackColor = IndustrialTheme.Backdrop;
        }

        private static void ApplyDarkScrollableTheme(Control control)
        {
            if (control is ScrollableControl || control is ListView || control is TreeView || control is ListBox || control is ComboBox)
                NativeWindowTheme.EnableDarkScrollbars(control);
        }

        public static void ApplyTreeMap(TreeMap treeMap)
        {
            if (treeMap == null)
                return;

            treeMap.BackColor = IndustrialTheme.StorageMapBack;
            treeMap.ForeColor = IndustrialTheme.TextHigh;
            treeMap.CellBorderColor = IndustrialTheme.StorageMapBack;
            treeMap.SelectedBackColor = IndustrialTheme.StorageMapSelected;
        }
    }
}
