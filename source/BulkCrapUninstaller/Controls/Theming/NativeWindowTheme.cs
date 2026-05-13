using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BulkCrapUninstaller.Controls.Theming
{
    public static class NativeWindowTheme
    {
        public const string DarkScrollbarThemeName = "DarkMode_Explorer";

        private const int DwmwaUseImmersiveDarkMode = 20;
        private const int DwmwaUseImmersiveDarkModeBefore20H1 = 19;
        private const int DwmwaCaptionColor = 35;
        private const int DwmwaTextColor = 36;

        private static readonly EventHandler DarkTitleBarHandleCreated = (sender, args) =>
        {
            if (sender is Form form)
                TryApplyDarkTitleBar(form.Handle);
        };

        private static readonly EventHandler DarkScrollbarsHandleCreated = (sender, args) =>
        {
            if (sender is Control control)
                TryApplyDarkScrollbars(control.Handle);
        };

        public static void EnableDarkTitleBar(Form form)
        {
            if (form == null)
                return;

            if (form.IsHandleCreated)
                TryApplyDarkTitleBar(form.Handle);

            form.HandleCreated -= DarkTitleBarHandleCreated;
            form.HandleCreated += DarkTitleBarHandleCreated;
        }

        public static void EnableDarkScrollbars(Control control)
        {
            if (control == null)
                return;

            if (control.IsHandleCreated)
                TryApplyDarkScrollbars(control.Handle);

            control.HandleCreated -= DarkScrollbarsHandleCreated;
            control.HandleCreated += DarkScrollbarsHandleCreated;
        }

        public static bool TryApplyDarkTitleBar(IntPtr handle)
        {
            if (handle == IntPtr.Zero || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;

            var darkMode = 1;
            var result = TrySetDwmAttribute(handle, DwmwaUseImmersiveDarkMode, darkMode) ||
                         TrySetDwmAttribute(handle, DwmwaUseImmersiveDarkModeBefore20H1, darkMode);

            TrySetDwmAttribute(handle, DwmwaCaptionColor, ToColorRef(IndustrialTheme.Backdrop));
            TrySetDwmAttribute(handle, DwmwaTextColor, ToColorRef(IndustrialTheme.TextHigh));

            return result;
        }

        public static bool TryApplyDarkScrollbars(IntPtr handle)
        {
            if (handle == IntPtr.Zero || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;

            try
            {
                return SetWindowTheme(handle, DarkScrollbarThemeName, null) == 0;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
            catch (EntryPointNotFoundException)
            {
                return false;
            }
        }

        private static bool TrySetDwmAttribute(IntPtr handle, int attribute, int value)
        {
            try
            {
                return DwmSetWindowAttribute(handle, attribute, ref value, Marshal.SizeOf<int>()) == 0;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
            catch (EntryPointNotFoundException)
            {
                return false;
            }
        }

        private static int ToColorRef(System.Drawing.Color color)
        {
            return color.R | (color.G << 8) | (color.B << 16);
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }
}
