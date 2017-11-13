using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas.Libraries
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
	}
}
