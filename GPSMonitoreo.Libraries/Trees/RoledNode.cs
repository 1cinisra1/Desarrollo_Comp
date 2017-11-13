using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Trees
{
    public class RoledNode<TId, TRoles, TPermissions>: BaseNode<TId, RoledTree<TId, TRoles, TPermissions>, RoledNode<TId, TRoles, TPermissions>>
	{
		public List<TRoles> Roles;
		public List<TPermissions> Permissions;

		public override void Add(RoledNode<TId, TRoles, TPermissions> node)
		{
			base.Add(node);

			if(this.Tree.AllocateAllNodes)
			{
				this.Tree.AllNodes.Add(node.Id, node);
			}
		}

		public bool HasRoleAccess(TRoles role)
		{
			if (Roles != null &&  Roles.IndexOf(role) == -1)
			{
				return false;
			}

			return true;
		}

		public void VisitNodesWithRoleAccess(TRoles role, Action<RoledNode<TId, TRoles, TPermissions>> visitor)
		{
			if(Children != null)
			{
				foreach(var child in Children)
				{
					if(child.HasRoleAccess(role))
					{
						visitor(child);
						child.VisitNodesWithRoleAccess(role, visitor);
					}
				}
			}

		}
	}
}
