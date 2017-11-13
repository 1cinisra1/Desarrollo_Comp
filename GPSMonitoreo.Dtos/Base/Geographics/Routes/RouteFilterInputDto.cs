using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Routes
{
    public class RouteFilterInputDto : CommonBaseFilterInputDto
    {
		public string AuxiliaryId { get; set; }
	}
}
