using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmGrades
{
    public class AlarmGradeInputDto : CommonBaseInputDto<byte>
    {


		[Rules("Required", "MaxLength(7)")]
		public string Color { get; set; }

		public bool Blinking { get; set; }

	}
}
