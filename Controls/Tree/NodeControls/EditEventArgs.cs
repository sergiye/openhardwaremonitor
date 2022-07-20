using System.Windows.Forms;

namespace OpenHardwareMonitor.Controls.Tree.NodeControls
{
	public class EditEventArgs : NodeEventArgs
	{
		private Control _control;
		public Control Control
		{
			get { return _control; }
		}

		public EditEventArgs(TreeNodeAdv node, Control control)
			: base(node)
		{
			_control = control;
		}
	}
}
