using System.Drawing;

namespace OpenHardwareMonitor.UI.Themes;

internal struct ThemeDto
{
    public string DisplayName { get; set; }
    public string ForegroundColor { get; set; }
    public string BackgroundColor { get; set; }
    public string HyperlinkColor { get; set; }
    public string SelectedForegroundColor { get; set; }
    public string SelectedBackgroundColor { get; set; }
    public string LineColor { get; set; }
    public string StrongLineColor { get; set; }
    public bool DarkMode { get; set; }
}

internal class CustomTheme : Theme
{
    public override Color ForegroundColor { get; }
    public override Color BackgroundColor { get; }
    public override Color HyperlinkColor { get; }
    public override Color SelectedForegroundColor { get; }
    public override Color SelectedBackgroundColor { get; }
    public override Color LineColor { get; }
    public override Color StrongLineColor { get; }
    public override bool WindowTitlebarFallbackToImmersiveDarkMode { get; }

    public CustomTheme(string id, ThemeDto theme) : base(id, theme.DisplayName)
    {
        ForegroundColor = ColorTranslator.FromHtml(theme.ForegroundColor);
        BackgroundColor = ColorTranslator.FromHtml(theme.BackgroundColor);
        HyperlinkColor = ColorTranslator.FromHtml(theme.HyperlinkColor);
        SelectedForegroundColor = ColorTranslator.FromHtml(theme.SelectedForegroundColor);
        SelectedBackgroundColor = ColorTranslator.FromHtml(theme.SelectedBackgroundColor);
        LineColor = ColorTranslator.FromHtml(theme.LineColor);
        StrongLineColor = ColorTranslator.FromHtml(theme.StrongLineColor);
        WindowTitlebarFallbackToImmersiveDarkMode = theme.DarkMode;
    }

    public static ThemeDto GetThemeDto(Theme theme)
    {
        return new ThemeDto
        {
            DisplayName = string.IsNullOrEmpty(theme.DisplayName) ? theme.Id : theme.DisplayName,
            ForegroundColor = ColorTranslator.ToHtml(theme.ForegroundColor),
            BackgroundColor = ColorTranslator.ToHtml(theme.BackgroundColor),
            HyperlinkColor = ColorTranslator.ToHtml(theme.HyperlinkColor),
            SelectedForegroundColor = ColorTranslator.ToHtml(theme.SelectedForegroundColor),
            SelectedBackgroundColor = ColorTranslator.ToHtml(theme.SelectedBackgroundColor),
            LineColor = ColorTranslator.ToHtml(theme.LineColor),
            StrongLineColor = ColorTranslator.ToHtml(theme.StrongLineColor),
            DarkMode = theme.WindowTitlebarFallbackToImmersiveDarkMode,
        };
    }
}
