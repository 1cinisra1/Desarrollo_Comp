using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Brands
{
    public class BrandInputDto : CommonBaseInputDto<short>
    {
		[Rules("RequiredNumeric", "DBKeyExists('MARCAS_CATS')")]
		public short CategoryId { get; set; }
    }
}
