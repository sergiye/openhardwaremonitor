using System;

namespace Aga.Controls.Tree
{
	public class TreeViewAdvEventArgs : EventArgs
	{
        public TreeNodeAdv Node { get; }
        public bool RaisedByUser { get; }

        public TreeViewAdvEventArgs(TreeNodeAdv node, bool raisedByUser = false)
		{
			Node = node;
            RaisedByUser = raisedByUser;
        }
	}
}
