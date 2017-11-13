using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{
    public class RestrictedElement
    {
		public Role[] RequiredRoles { get; set; }

		public RequiredPermission[] RequiredPermissions { get; set; }
	}
}
