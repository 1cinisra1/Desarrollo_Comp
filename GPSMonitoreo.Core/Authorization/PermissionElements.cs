using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{
	public class PermissionElements : Dictionary<int, List<PermissionAction>>
	{
		//Dictionary<int, List<PermissionAction>> _elements;


		//public PermissionElements()
		//{
		//	_elements = new Dictionary<int, List<PermissionAction>>();
		//}

		//public void Add(int elementId, List<PermissionAction> actions)
		//{
		//	_elements.Add(elementId, actions);
		//}

		public bool IsGranted(RequiredPermission[] requiredPermissions)
		{
			foreach(var requiredPermission in requiredPermissions)
			{
				if(IsGranted(requiredPermission))
				{
					return true;
				}
			}

			return false;

		}

		public bool IsGranted(RequiredPermission requiredPermission)
		{
			List<PermissionAction> actions;

			foreach(var elementId in requiredPermission.Elements)
			{
				if (TryGetValue(elementId, out actions))
				{

					if(requiredPermission.Actions == null)
					{
						return true;
					}

					foreach(var requiredAction in requiredPermission.Actions)
					{
						if(actions.Contains(requiredAction))
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
