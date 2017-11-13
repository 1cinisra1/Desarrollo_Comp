using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Equipments
{
    public class CapabilityListInputDto : InputDto
    {

		[Rules("RequiredNumeric", "DBKeyExists('PRODUCTOS_CATS')")]
		public short ProductCategoryId { get; set; }

		public string ProductCategoryDescription { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('EQUIPOS_CAPS')")]
		public short CapabilityId { get; set; }

		public string CapabilityDescription { get; set; }

		public string MeasureUnitDescription { get; set; }

		public double Value { get; set; }
    }
}
