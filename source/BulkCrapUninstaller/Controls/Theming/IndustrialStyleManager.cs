using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Klocman.Extensions;

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
            var toolStrip = component as ToolStrip;
            if (toolStrip == null)
                return;

            toolStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            toolStrip.BackColor = IndustrialTheme.GlassPrimarySolid;
            toolStrip.ForeColor = IndustrialTheme.TextHigh;
            ApplyToolStripItems(toolStrip.Items);
        }

        private static void ApplyToolStripItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                item.ForeColor = item.Enabled ? IndustrialTheme.TextHigh : IndustrialTheme.TextMuted;

                var dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                    ApplyToolStripItems(dropDownItem.DropDownItems);
            }
        }

        private static void ApplyControl(Control control)
        {
            if (control == null)
                return;

            control.ForeColor = IndustrialTheme.TextHigh;

            var listView = control as ObjectListView;
            if (listView != null)
            {
                ApplyObjectListView(listView);
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
    }
}
