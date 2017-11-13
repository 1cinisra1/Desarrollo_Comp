using GPSMonitoreo.Core.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.RequiredPermissions
{
    public class RequiredPermissionListInputDto : InputDto
    {
		public PermissionElementType ElementType { get; set; }

		public int ElementId { get; set; }

		public string ElementDescription { get; set; }


		public List<Role> Roles { get; set; }

		public List<PermissionAction> Actions { get; set; }
    }
}
