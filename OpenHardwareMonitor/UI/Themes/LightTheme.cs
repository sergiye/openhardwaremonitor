﻿using System.Drawing;

namespace OpenHardwareMonitor.UI.Themes
{
    public class LightTheme : Theme
    {
        public LightTheme(string id, string displayName) : base(id, displayName) { }
        public override Color ForegroundColor => Color.FromArgb(0, 0, 0);
        public override Color BackgroundColor => Color.FromArgb(255, 255, 255);
        public override Color HyperlinkColor => Color.FromArgb(0, 0, 255);
        public override Color SelectedForegroundColor => ForegroundColor;
        public override Color SelectedBackgroundColor => Color.FromArgb(240, 240, 240);
        public override Color LineColor => Color.FromArgb(247, 247, 247);
        public override Color StrongLineColor => Color.FromArgb(209, 209, 209);
        public override bool WindowTitlebarFallbackToImmersiveDarkMode => false;

        public LightTheme() : base("light", "Light") { }
    }
}
