using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceForMapDto
    {
		public int Id { get; set; }

		public string Description { get; set; }

		public byte ShapeId { get; set; }

		public byte LayerId { get; set; }

		public GeofenceCoordExtended Coords { get; set; }

	}
}
