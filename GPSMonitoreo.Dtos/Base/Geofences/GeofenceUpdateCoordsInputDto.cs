using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceUpdateCoordsInputDto: BaseInputDto<int>
    {
		public byte ShapeId { get; set; }

		public double CurveRadius { get; set; }

		public List<GeofenceCoord> Coords { get; set; }

    }
}
