using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceCoordExtended
    {

		public GeofenceCoord Vertex1 { get; set; }

		public GeofenceCoord Vertex2 { get; set; }

		public GeofenceCoord CircleCoord { get; set; }

		public double CircleRadius { get; set; }

		public List<GeofenceCoord> Coords { get; set; }

	}
}
