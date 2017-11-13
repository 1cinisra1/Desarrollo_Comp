using GPSMonitoreo.Libraries.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{

	[ResourceManager(typeof(Resources.PermissionActions))]
	public enum PermissionAction : byte
    {
		Create = 1,
		Edit = 2,
		View = 3,
		Delete = 4
	}
}
