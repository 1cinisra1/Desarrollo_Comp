using GPSMonitoreo.Dtos.Base.RequiredPermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Misc.AlarmFilters
{
    public class AlarmFilterInputDto : InputDto
    {
		public List<RequiredPermissionListInputDto> RequiredPermissions { get; set; }
    }
}
