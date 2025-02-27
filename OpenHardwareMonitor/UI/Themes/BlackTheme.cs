using System.Drawing;

namespace OpenHardwareMonitor.UI.Themes
{
    public class BlackTheme : LightTheme
    {
        public override Color ForegroundColor => Color.FromArgb(218, 218, 218);
        public override Color BackgroundColor => Color.FromArgb(0, 0, 0);
        public override Color HyperlinkColor => Color.FromArgb(144, 220, 232);
        public override Color SelectedForegroundColor => ForegroundColor;
        public override Color SelectedBackgroundColor => ColorTranslator.FromHtml("#2B5278");
        public override Color LineColor => ColorTranslator.FromHtml("#070A12");
        public override Color StrongLineColor => ColorTranslator.FromHtml("#091217");
        public override bool WindowTitlebarFallbackToImmersiveDarkMode => true;

        public BlackTheme() : base("black", "Black")
        {
        }
    }
}
