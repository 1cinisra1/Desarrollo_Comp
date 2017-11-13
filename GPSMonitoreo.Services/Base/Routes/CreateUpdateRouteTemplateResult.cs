using GPSMonitoreo.Dtos.Base.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Base.Routes
{
    public class CreateUpdateRouteTemplateResult : CreateUpdateResult<int>
	{
		public List<RouteTemplateRouteSectionInputDto> SkippedSections;

	}
}
