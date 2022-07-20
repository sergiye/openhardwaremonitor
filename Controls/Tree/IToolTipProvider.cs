using OpenHardwareMonitor.Controls.Tree.NodeControls;

namespace OpenHardwareMonitor.Controls.Tree
{
	public interface IToolTipProvider
	{
		string GetToolTip(TreeNodeAdv node, NodeControl nodeControl);
	}
}
