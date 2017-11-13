using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.CommonDbEntities
{
    public class CommonDbEntityCategoryListDto<TId> : CommonDbEntityListDto<TId>
	{
		public string Hierarchy { get; set; }
    }
}
