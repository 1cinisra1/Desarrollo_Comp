using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseInputDto<TId>: BaseInputDto<TId>/*, ICommonBaseInputDto<TId>*/
	{

		[Rules("Required", "MaxLength(52)")]
		public string DescriptionLong { get; set; }


		[Rules("MaxLength(30)")]
		public string DescriptionMedium { get; set; }


		[Rules("MaxLength(15)")]
		public string DescriptionShort { get; set; }


		[Rules("MaxLength(5)")]
		public string Abbreviation { get; set; }

		public byte StatusId { get; set; }


		[Rules("MaxLength(500)")]
		public string Notes { get; set; }

		public CommonBaseInputDto()
		{
			
		}
    }
}
