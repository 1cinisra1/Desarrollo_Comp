using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Routes
{
    public class RouteListDto: CommonBaseWithAuxiliarSimpleListDto<int>
    {
		public string CategoryDescription { get; set; }
	}
}
