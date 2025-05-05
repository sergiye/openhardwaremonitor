using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace OpenHardwareMonitor.UI;

internal class NodeToolTipProvider : IToolTipProvider
{
    public string GetToolTip(TreeNodeAdv node, NodeControl nodeControl) => (node.Tag as Node)?.ToolTip;
}
