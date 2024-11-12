using System.Drawing;

namespace OpenHardwareMonitor.UI.Themes
{
    public class DarkTheme : LightTheme
    {
        public override Color ForegroundColor => Color.FromArgb(233, 233, 233);
        public override Color BackgroundColor => Color.FromArgb(30, 30, 30);
        public override Color HyperlinkColor => Color.FromArgb(144, 220, 232);
        public override Color SelectedForegroundColor => ForegroundColor;
        public override Color SelectedBackgroundColor => Color.FromArgb(45, 45, 45);
        public override Color LineColor => Color.FromArgb(38, 38, 38);
        public override Color StrongLineColor => Color.FromArgb(53, 53, 53);
        public override bool WindowTitlebarFallbackToImmersiveDarkMode => true;

        public DarkTheme() : base("dark", "Dark")
        {
        }
    }
}
