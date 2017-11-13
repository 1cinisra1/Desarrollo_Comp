using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Routes
{
    public class RouteTemplateRouteSectionInputDto: CommonBaseListInputDto<int>
	{

		public List<RouteTemplateRouteRouteSegmentInputDto> Segments { get; set; }

	}
}
