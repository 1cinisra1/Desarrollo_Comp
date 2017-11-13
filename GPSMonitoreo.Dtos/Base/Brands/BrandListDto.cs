using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Brands
{
    public class BrandListDto : CommonBaseListDto<int>
    {
		public string CategoryDescription { get; set; }
	}
}
