using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseSimpleListDto<TId> : BaseListDto<TId>
    {
		public string Description { get; set; }
    }
}
