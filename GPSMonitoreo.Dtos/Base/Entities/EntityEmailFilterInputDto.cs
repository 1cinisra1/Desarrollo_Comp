using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Entities
{
    public class EntityEmailFilterInputDto : CommonBaseFilterInputDto
	{
		public int EntityId { get; set; }

	}
}
