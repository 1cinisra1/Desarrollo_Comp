using GPSMonitoreo.Dtos.Base.Geographics.Routes;
using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geographics.Courses
{
    public class CourseInputDto: CommonBaseInputDto<int>
    {

		[Rules("Required", "MaxLength(10)")]
		public string AlternateId { get; set; }

		[Rules("MinCount(2)", "ValidateCollection")]

		public List<RouteListItemInputDto> Routes { get; set; }
	}
}
