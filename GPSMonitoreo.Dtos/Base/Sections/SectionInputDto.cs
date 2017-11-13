using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Sections
{
    public class SectionInputDto: CommonBaseInputDto<int>
    {

		[Rules("MaxLength(10)")]
		public string AuxiliaryId { get; set; }


		[Rules("Required", "DBKeyExists('TRAMOS_CATS')")]
		public short CategoryId { get; set; }

		[Rules("MinCount(2)", "ValidateCollection")]
		public List<CommonBaseWithAuxiliarListInputDto<int>> Segments { get; set; }
	}
}
