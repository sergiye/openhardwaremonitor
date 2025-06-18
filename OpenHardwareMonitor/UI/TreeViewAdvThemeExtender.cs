using System.Drawing;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OpenHardwareMonitor.UI
{
    internal static class TreeViewAdvThemeExtender
    {
        internal static void SubscribeToThemes()
        {
            Theme.OnCurrentChanged += () => { AssignRendersTheme(Theme.Current); };
            Theme.OnApplyToControl += (control, theme) => {
                if (control is TreeViewAdv treeView)
                {
                    treeView.BackColor = theme.TreeBackgroundColor;
                    treeView.ForeColor = theme.TreeTextColor;
                    treeView.LineColor = theme.TreeOutlineColor;
                    return true;
                }
                return false;
            };
        }

        internal static void AssignRendersTheme(Theme current)
        {
            TreeViewAdv.CustomPlusMinusRenderFunc = (g, rect, isExpanded) =>
            {
                int x = rect.Left;
                int y = rect.Top + 5;
                int size = 8;
                using (Brush brush = new SolidBrush(current.BackgroundColor))
                {
                    g.FillRectangle(brush, x - 1, y - 1, size + 4, size + 4);
                }
                using (Pen pen = new Pen(current.TreeOutlineColor))
                {

                    g.DrawRectangle(pen, x, y, size, size);
                    g.DrawLine(pen, x + 2, y + size / 2, x + size - 2, y + size / 2);
                    if (!isExpanded)
                    {
                        g.DrawLine(pen, x + size / 2, y + 2, x + size / 2, y + size - 2);
                    }
                }
            };

            TreeViewAdv.CustomCheckRenderFunc = (g, rect, isChecked) =>
            {
                int x = rect.Left;
                int y = rect.Top + 1;
                int size = 12;
                using (Brush brush = new SolidBrush(current.BackgroundColor))
                {
                    g.FillRectangle(brush, x - 1, y - 1, 12, 12);
                }
                using (Pen pen = new Pen(current.TreeOutlineColor))
                {
                    g.DrawRectangle(pen, x, y, size, size);
                    if (isChecked)
                    {
                        x += 3;
                        y += 3;
                        g.DrawLine(pen, x, y + 3, x + 2, y + 5);
                        g.DrawLine(pen, x + 2, y + 5, x + 6, y + 1);
                        g.DrawLine(pen, x, y + 4, x + 2, y + 6);
                        g.DrawLine(pen, x + 2, y + 6, x + 6, y + 2);
                    }
                }
            };

            TreeViewAdv.CustomColumnBackgroundRenderFunc = (g, rect, isPressed, isHot) =>
            {
                using (Brush brush = new SolidBrush(current.TreeBackgroundColor))
                {
                    g.FillRectangle(brush, rect);
                }
                using (Pen pen = new Pen(current.TreeRowSepearatorColor))
                {
                    g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                    g.DrawLine(pen, rect.Left, rect.Top + 1, rect.Right, rect.Top + 1);
                }
            };

            TreeViewAdv.CustomColumnTextRenderFunc = (g, rect, font, text) =>
            {
                TextRenderer.DrawText(g, text, font, rect, current.TreeTextColor, TextFormatFlags.Left);
            };

            TreeViewAdv.CustomHorizontalLinePen = new Pen(current.TreeRowSepearatorColor);
            TreeViewAdv.CustomSelectedRowBrush = new SolidBrush(current.TreeSelectedBackgroundColor);
            TreeViewAdv.CustomSelectedTextColor = current.TreeSelectedTextColor;
        }
    }
}
