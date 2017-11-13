using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Entities
{
    public class EntityListDto: CommonBaseListDto<int>
    {

		public byte EntityTypeId { get; set; }

		public string EntityTypeDescription { get; set; }


		public string IdentificationTypeDescription { get; set; }

		public string Identification { get; set; }

		public string BusinessName { get; set; }
	
		public string Names { get; set; }

		public string LastNames { get; set; }
	}
}
