using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Models
{
    public class ModelListDto : CommonBaseListDto<int>
    {
		public string BrandDescription { get; set; }
	}
}
