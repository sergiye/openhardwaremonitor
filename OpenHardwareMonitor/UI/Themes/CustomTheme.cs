using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenHardwareMonitor.Utilities;

namespace OpenHardwareMonitor.UI;

#region predefined themes

internal class LightTheme : Theme
{
    public LightTheme() : base("light", "Light") {
        ForegroundColor = Color.FromArgb(0, 0, 0);
        BackgroundColor = Color.FromArgb(255, 255, 255);
        HyperlinkColor = Color.FromArgb(0, 0, 255);
        SelectedForegroundColor = ForegroundColor;
        SelectedBackgroundColor = Color.FromArgb(240, 240, 240);
        LineColor = Color.FromArgb(247, 247, 247);
        StrongLineColor = Color.FromArgb(209, 209, 209);
        WindowTitlebarFallbackToImmersiveDarkMode = false;
    }
}

internal class DarkTheme : Theme
{
    public DarkTheme() : base("dark", "Dark") {
        ForegroundColor = Color.FromArgb(233, 233, 233);
        BackgroundColor = Color.FromArgb(30, 30, 30);
        HyperlinkColor = Color.FromArgb(144, 220, 232);
        SelectedForegroundColor = Color.Black;
        SelectedBackgroundColor = ColorTranslator.FromHtml("#009687");
        LineColor = Color.FromArgb(38, 38, 38);
        StrongLineColor = Color.FromArgb(53, 53, 53);
        WindowTitlebarFallbackToImmersiveDarkMode = true;
    }
}

internal class BlackTheme : Theme
{
    public BlackTheme() : base("black", "Black") {
        ForegroundColor = Color.FromArgb(218, 218, 218);
        BackgroundColor = Color.FromArgb(0, 0, 0);
        HyperlinkColor = Color.FromArgb(144, 220, 232);
        SelectedForegroundColor = ForegroundColor;
        SelectedBackgroundColor = ColorTranslator.FromHtml("#2B5278");
        LineColor = ColorTranslator.FromHtml("#070A12");
        StrongLineColor = ColorTranslator.FromHtml("#091217");
        WindowTitlebarFallbackToImmersiveDarkMode = true;
    }
}

#endregion

internal class CustomTheme : Theme
{
    private class ThemeDto
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

    private CustomTheme(string id, ThemeDto theme) : base(id, theme.DisplayName)
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

    public static IEnumerable<Theme> GetAllThemes()
    {
        foreach (Type type in typeof(Theme).Assembly.GetTypes())
        {
            if (type == typeof(Theme) || !typeof(Theme).IsAssignableFrom(type))
                continue;
            var theme = (Theme)type.GetConstructor(new Type[] { })?.Invoke(new object[] { });
            if (theme != null)
                yield return theme;
        }

        var appPath = Path.GetDirectoryName(Updater.CurrentFileLocation);
        var themesPath = Path.Combine(appPath, "Themes");
        var di = new DirectoryInfo(themesPath);
        //var dt = CustomTheme.ToDto(new DarkTheme());
        //Directory.CreateDirectory(themesPath);
        //dt.ToJsonFile(Path.Combine(themesPath, "custom.json"));
        if (!di.Exists)
        {
            yield break;
        }
        foreach (FileInfo fi in di.GetFiles("*.json", SearchOption.TopDirectoryOnly))
        {
            ThemeDto dto = null;
            try
            {
                string json = File.ReadAllText(fi.FullName);
                dto = json.FromJson<ThemeDto>();
            }
            catch (Exception)
            {
                //ignore
            }

            if (dto != null)
                yield return new CustomTheme(fi.Name, dto);
        }
    }

    private static ThemeDto ToDto(Theme theme)
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
