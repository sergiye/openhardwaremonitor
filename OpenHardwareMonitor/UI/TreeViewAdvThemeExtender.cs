using System.Drawing;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OpenHardwareMonitor.UI
{
    internal static class TreeViewAdvThemeExtender
    {
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
            TreeViewAdv.CustomPlusMinusRenderFunc = (g, rect, isExpanded) =>
            {
                int x = rect.Left;
                int y = rect.Top + 5;
                int size = 8;
                g.FillRectangle(Theme.BackgroundBrush, x - 1, y - 1, size + 4, size + 4);
                g.DrawRectangle(Theme.ForegroundPen, x, y, size, size);
                g.DrawLine(Theme.ForegroundPen, x + 2, y + size / 2, x + size - 2, y + size / 2);
                if (!isExpanded)
                {
                    g.DrawLine(Theme.ForegroundPen, x + size / 2, y + 2, x + size / 2, y + size - 2);
                }
            };

            TreeViewAdv.CustomCheckRenderFunc = (g, rect, isChecked) =>
            {
                int x = rect.Left;
                int y = rect.Top + 1;
                int size = 12;
                g.FillRectangle(Theme.BackgroundBrush, x - 1, y - 1, 12, 12);
                g.DrawRectangle(Theme.ForegroundPen, x, y, size, size);
                if (isChecked)
                {
                    x += 3;
                    y += 3;
                    g.DrawLine(Theme.ForegroundPen, x, y + 3, x + 2, y + 5);
                    g.DrawLine(Theme.ForegroundPen, x + 2, y + 5, x + 6, y + 1);
                    g.DrawLine(Theme.ForegroundPen, x, y + 4, x + 2, y + 6);
                    g.DrawLine(Theme.ForegroundPen, x + 2, y + 6, x + 6, y + 2);
                }
            };

            TreeViewAdv.CustomColumnBackgroundRenderFunc = (g, rect, isPressed, isHot) =>
            {
                g.FillRectangle(Theme.BackgroundBrush, rect);
                g.DrawLine(Theme.LinePen, rect.Left, rect.Top, rect.Right, rect.Top);
                g.DrawLine(Theme.LinePen, rect.Left, rect.Top + 1, rect.Right, rect.Top + 1);
            };

            TreeViewAdv.CustomColumnTextRenderFunc = (g, rect, font, text) =>
            {
                TextRenderer.DrawText(g, text, font, rect, Theme.Current.ForegroundColor, TextFormatFlags.Left);
            };

            TreeViewAdv.CustomHorizontalLinePen = Theme.LinePen;
            TreeViewAdv.CustomSelectedRowBrush = Theme.SelectedBackBrush;
            TreeViewAdv.CustomSelectedTextColor = Theme.Current.SelectedForegroundColor;
        }
    }
}
