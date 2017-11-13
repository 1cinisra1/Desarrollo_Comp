using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{

    public class RolePermissions: Dictionary<PermissionElementType, PermissionElements>
	{
		//private Dictionary<PermissionType, PermissionElements> _permissions;

		public Role Role { get; set; }

		public RolePermissions()
		{
			//_permissions = new Dictionary<PermissionType, PermissionElements>();
		}

		public bool IsGranted(RestrictedElement restrictedElement)
		{
			if(restrictedElement.RequiredRoles != null && IsGranted(restrictedElement.RequiredRoles))
			{
				return true;
			}

			if(restrictedElement.RequiredPermissions != null && IsGranted(restrictedElement.RequiredPermissions))
			{
				return true;
			}

			if(restrictedElement.RequiredRoles == null && restrictedElement.RequiredPermissions == null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool IsGranted(Role[] requiredRoles)
		{
			if(Array.IndexOf<Role>(requiredRoles, Role) > -1)
			{
				return true;
			}

			return false;
		}

		public bool IsGranted(RequiredPermission requiredPermission)
		{
			PermissionElements permissionElements;

			if(TryGetValue(requiredPermission.Type, out permissionElements))
			{
				return permissionElements.IsGranted(requiredPermission);
			}

			return false;
		}

		public bool IsGranted(RequiredPermission[] requiredPermissions)
		{
			foreach(var requiredPermission in requiredPermissions )
			{
				if(IsGranted(requiredPermission))
				{
					return true;
				}
			}
			return false;
		}

		//public void Add(PermissionType type, PermissionElements permissionElements)
		//{
		//	_permissions.Add(type, permissionElements);
		//}


		//public void Add(PermissionType type, int elementId, List<PermissionAction> actions)
		//{
		//	//_permissions.Add(type, new Dictionary<int, List<PermissionAction>>());
		//}
	}
}
