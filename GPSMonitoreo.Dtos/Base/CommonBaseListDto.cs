using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseListDto<TId> : BaseListDto<TId>
    {
		public string Description { get; set; }
    }
}
