using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{
    public class RequiredPermission
    {
		public PermissionElementType Type;
		public int[] Elements;
		public PermissionAction[] Actions;

		public RequiredPermission(PermissionElementType type, int elementId)
		{
			Type = type;
			Elements = new int[] { elementId };
		}

		public RequiredPermission(PermissionElementType type, int[] elements)
		{
			Type = type;
			Elements = elements;
		}

		public RequiredPermission(PermissionElementType type, int elementId, PermissionAction[] actions) : this(type, elementId)
		{
			Actions = actions;
		}

		public RequiredPermission(PermissionElementType type, int elementId, PermissionAction action) : this(type, elementId)
		{
			Actions = new PermissionAction[] { action };
		}
    }
}
