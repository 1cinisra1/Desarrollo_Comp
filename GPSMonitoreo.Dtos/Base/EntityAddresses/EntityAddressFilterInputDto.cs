using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EntityAddress
{
    public class EntityAddressFilterInputDto : CommonBaseFilterInputDto
	{

		public int EntityId { get; set; }

		public List<byte> TypeIds { get; set; }

	}
}
