using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Entities
{
    public class EntityFilterInputDto : CommonBaseFilterInputDto
	{
		public byte IdentificationTypeId { get; set; }

		public string Identification { get; set; }

		public string Names { get; set; }

		public string LastNames { get; set; }

		public string BusinessName { get; set; }

		public byte EntityTypeId { get; set; }

		public List<Int16> CategoryIds { get; set; }

	}
}
