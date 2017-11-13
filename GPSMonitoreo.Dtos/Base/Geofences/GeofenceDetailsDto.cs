using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceDetailsDto
    {
		
		public int Id { get; set; }

		public string AuxiliaryId { get; set; }

		public string Description { get; set; }

		public string Notes { get; set; }


		public byte LayerId { get; set; }

		public string LayerDescription { get; set; }

		public string CategoryDescription { get; set; }

		public string StatusDescription { get; set; }

		public string RegionDescription { get; set; }




		public byte RoadShapeId { get; set; }

		public string RoadShapeDescription { get; set; }

		public string CurveTypeDescription { get; set; }

		public string CurveGradeDescription { get; set; }

		public byte CurvesQty { get; set; }

		public double Distance { get; set; }

		public byte RoadSurfaceId { get; set; }

		public string RoadSurfaceDescription { get; set; }

		public string RoadSurfaceStateDescription { get; set; }

		public byte RoadLanesGoing { get; set; }

		public byte RoadLanesReturning { get; set; }

		public string RoadTrafficDescription { get; set; }

		public string HillGoingTypeDescription { get; set; }
		public string HillGoingGradeDescription { get; set; }

		public string HillReturningTypeDescription { get; set; }

		public string HillReturningGradeDescription { get; set; }



		public byte SpeedGoingMop { get; set; }

		public byte SpeedReturningMop { get; set; }

		public byte SpeedGoingMopRest { get; set; }

		public byte SpeedReturningMopRest { get; set; }

		public byte SpeedGoingCustomRest { get; set; }

		public byte SpeedReturningCustomRest { get; set; }


		public byte OptimalSpeed { get; set; }

		public double OptimalTiming { get; set; }

	}
}
