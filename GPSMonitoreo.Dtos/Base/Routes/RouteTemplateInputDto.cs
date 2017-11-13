using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Routes
{
    public class RouteTemplateInputDto: CommonBaseInputDto<int>
	{

		[Rules("Required", "DBKeyExists('RUTAS_CATS')")]
		public Int16 CategoryId { get; set; }

		[Rules("MinCount(1)")]
		public List<RouteTemplateRouteSectionInputDto> Sections { get; set; }


		[Rules("RequiredNumeric")]
		public int Direction { get; set; }


		[Rules("RequiredNumeric")]
		public int RouteType { get; set; }

		
    }
}
