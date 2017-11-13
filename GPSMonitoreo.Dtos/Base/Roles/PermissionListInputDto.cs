using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Roles
{
    public class PermissionListInputDto<TId> : InputDto
    {
		public TId ElementId { get; set; }

		public string ElementDescription { get; set; }

		public List<Core.Authorization.PermissionAction> Actions { get; set; }
    }
}
