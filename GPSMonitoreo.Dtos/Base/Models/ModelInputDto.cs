using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Models
{
    public class ModelInputDto : CommonBaseInputDto<short>
    {
		[Rules("RequiredNumeric", "DBKeyExists('MARCAS')")]
		public short BrandId { get; set; }
    }
}
