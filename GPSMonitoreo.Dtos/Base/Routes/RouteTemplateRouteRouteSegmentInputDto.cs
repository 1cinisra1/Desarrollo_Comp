using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Routes
{
    public class RouteTemplateRouteRouteSegmentInputDto : CommonBaseListInputDto<int>
	{

		public List<RouteTemplateGeofenceInputDto> Geofences { get; set; }
	}
}
