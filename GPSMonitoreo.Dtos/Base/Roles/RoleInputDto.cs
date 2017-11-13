using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Roles
{
    public class RoleInputDto: CommonBaseInputDto<Int16>
    {

		public List<PermissionListInputDto<int>> EntitiesPermissions { get; set; }

		public List<PermissionListInputDto<string>> AlarmsFiltersPermissions { get; set; }


	}
}
