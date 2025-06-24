using System.Drawing;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OpenHardwareMonitor.UI
{
    internal static class TreeViewAdvThemeExtender
    {
        private static Brush BackgroundBrush;
        private static Brush SelectedBackBrush;
        private static Pen ForegroundPen;
        private static Pen LinePen;

        internal static void SubscribeToThemes()
        {
            Theme.OnCurrentChanged += () => { AssignRendersTheme(); };
            Theme.OnApplyToControl += (control, theme) => {
                if (control is TreeViewAdv treeView)
                {
                    treeView.BackColor = theme.BackgroundColor;
                    treeView.ForeColor = theme.ForegroundColor;
                    treeView.LineColor = theme.ForegroundColor;
                    return true;
                }
                return false;
            };
        }

        internal static void AssignRendersTheme()
        {
            BackgroundBrush?.Dispose();
            BackgroundBrush = new SolidBrush(Theme.Current.BackgroundColor);
            SelectedBackBrush?.Dispose();
            SelectedBackBrush = new SolidBrush(Theme.Current.SelectedBackgroundColor);
            ForegroundPen?.Dispose();
            ForegroundPen = new Pen(Theme.Current.ForegroundColor);
            LinePen?.Dispose();
            LinePen = new Pen(Theme.Current.LineColor);

            TreeViewAdv.CustomPlusMinusRenderFunc = (g, rect, isExpanded) =>
            {
                int x = rect.Left;
                int y = rect.Top + 5;
                int size = 8;
                g.FillRectangle(BackgroundBrush, x - 1, y - 1, size + 4, size + 4);
                g.DrawRectangle(ForegroundPen, x, y, size, size);
                g.DrawLine(ForegroundPen, x + 2, y + size / 2, x + size - 2, y + size / 2);
                if (!isExpanded)
                {
                    g.DrawLine(ForegroundPen, x + size / 2, y + 2, x + size / 2, y + size - 2);
                }
            };

            TreeViewAdv.CustomCheckRenderFunc = (g, rect, isChecked) =>
            {
                int x = rect.Left;
                int y = rect.Top + 1;
                int size = 12;
                g.FillRectangle(BackgroundBrush, x - 1, y - 1, 12, 12);
                g.DrawRectangle(ForegroundPen, x, y, size, size);
                if (isChecked)
                {
                    x += 3;
                    y += 3;
                    g.DrawLine(ForegroundPen, x, y + 3, x + 2, y + 5);
                    g.DrawLine(ForegroundPen, x + 2, y + 5, x + 6, y + 1);
                    g.DrawLine(ForegroundPen, x, y + 4, x + 2, y + 6);
                    g.DrawLine(ForegroundPen, x + 2, y + 6, x + 6, y + 2);
                }
            };

            TreeViewAdv.CustomColumnBackgroundRenderFunc = (g, rect, isPressed, isHot) =>
            {
                g.FillRectangle(BackgroundBrush, rect);
                g.DrawLine(LinePen, rect.Left, rect.Top, rect.Right, rect.Top);
                g.DrawLine(LinePen, rect.Left, rect.Top + 1, rect.Right, rect.Top + 1);
            };

            TreeViewAdv.CustomColumnTextRenderFunc = (g, rect, font, text) =>
            {
                TextRenderer.DrawText(g, text, font, rect, Theme.Current.ForegroundColor, TextFormatFlags.Left);
            };

            TreeViewAdv.CustomHorizontalLinePen = LinePen;
            TreeViewAdv.CustomSelectedRowBrush = SelectedBackBrush;
            TreeViewAdv.CustomSelectedTextColor = Theme.Current.SelectedForegroundColor;
        }
    }
}
