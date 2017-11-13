using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Trees
{
    public class RoledTree<TId, TRoles, TPermissions>: RoledNode<TId, TRoles, TPermissions>
    {
		public bool AllocateAllNodes;

		public Dictionary<TId, RoledNode<TId, TRoles, TPermissions>> AllNodes;

		public RoledTree()
		{
			this.Tree = this;
		}

		public RoledTree(bool allocateAllNodes, int capacity = 0) : this()
		{
			AllocateAllNodes = allocateAllNodes;
			if (capacity > 0)
			{
				AllNodes = new Dictionary<TId, RoledNode<TId, TRoles, TPermissions>>(capacity);
			}
			else
			{
				AllNodes = new Dictionary<TId, RoledNode<TId, TRoles, TPermissions>>();
			}
		}
	}
}
