using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geographics.Routes
{
    public class RouteListItemInputDto: InputDto
	{

		[Rules("RequiredNumeric", "DBKeyExists('RUTAS')")]
		public int Id { get; set; }

		public string Description { get; set; }
    }
}
