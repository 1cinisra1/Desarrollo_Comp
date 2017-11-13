using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GPSMonitoreo.Dtos.Base
{
    public class BaseListDto<TId>
	{
		public TId Id { get; set; }
	}
}
