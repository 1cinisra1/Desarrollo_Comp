using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceFilterInputDto : CommonBaseFilterInputDto
    {
		public string AuxiliaryId { get; set; }

		public byte RegionId { get; set; }

		public List<byte> LayerIds { get; set; }


		public string Routes { get; set; }

		public string Sections { get; set; }

		public string Segments { get; set; }

		public bool SearchForMap { get; set; }




	}
}
