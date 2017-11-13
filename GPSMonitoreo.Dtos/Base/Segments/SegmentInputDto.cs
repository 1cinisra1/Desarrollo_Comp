using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Segments
{
    public class SegmentInputDto : CommonBaseInputDto<int>
    {

		[Rules("MaxLength(10)")]
		public string AuxiliaryId { get; set; }


		[Rules("Required", "DBKeyExists('SEGMENTOS_CATS')")]
		public Int16 CategoryId { get; set; }

		[Rules("MinCount(2)", "ValidateCollection")]
		public List<CommonBaseWithAuxiliarListInputDto<int>> Geofences { get; set; }
	}
}
