using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EntityAddress
{
    public class EntityAddressListDto : CommonBaseListDto<int>
    {

		public string EntityAddressTypes { get; set; }

		public string CountryDescription { get; set; }

		public string CityDescription { get; set; }

		public string Neighborhood { get; set; }

		public string Street1 { get; set; }

		public int GeofenceId { get; set; }

		public string GeofenceDescription { get; set; }

	}
}
