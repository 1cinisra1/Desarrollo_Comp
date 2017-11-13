using GPSMonitoreo.Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Authorization
{
    public class RoleAttribute : AuthorizeAttribute
    {

		public RoleAttribute(params Role[] roles) : base()
		{
			//Roles = String.Join(",", Enum.GetNames(typeof(Role)));
			Roles = string.Join(",", roles.Select(r => r.ToString("D")));
		}
    }
}
