using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceInputDto : CommonBaseInputDto<int>
	{
		[Rules("MaxLength(10)")]
		public string AuxiliaryId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('CERCAS_CAPAS')")]
		public byte LayerId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('CERCAS_CATS')")]
		public Int16 CategoryId { get; set; }


		public Int32 InRouteGeofenceId { get; set; }

		public string InRouteGeofenceDescription { get; set; }

		public Int32 ParentGeofenceId { get; set; }

		public string ParentGeofenceDescription { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('REGIONES')")]
		public byte RegionId { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "RequiredNumeric", "DBKeyExists('CERCAS_TRAZADO_VIA')")]
		public byte RoadShapeId { get; set; }

		[Rules("ContinueIf('this.RoadShapeId == 2')", "RequiredNumeric", "DBKeyExists('CERCAS_CURVAS_TIPOS')")]
		public byte CurveTypeId { get; set; }

		[Rules("ContinueIf('this.RoadShapeId == 2')", "RequiredNumeric", "DBKeyExists('CERCAS_CURVAS_GRADO')")]
		public byte CurveGradeId { get; set; }

		[Rules("ContinueIf('this.RoadShapeId == 2')", "Min(1)")]
		public byte CurvesQty { get; set; }

		public double Distance { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "RequiredNumeric", "DBKeyExists('CERCAS_CALZADAS')")]
		public byte RoadSurfaceId { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "RequiredNumeric", "DBKeyExists('CERCAS_CALZADAS_ESTADOS')")]
		public byte RoadSurfaceStateId { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "Min(0)")]
		public byte RoadLanesGoing { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "Min(0)")]
		public byte RoadLanesReturning { get; set; }

		[Rules("ContinueIf('this.LayerId == 1')", "RequiredNumeric", "DBKeyExists('CERCAS_TRAFICO')")]
		public byte RoadTrafficId { get; set; }

		[Rules("ContinueIf('Array.Contains(new []{1, 2, 3}, this.RoadShapeId)')", "RequiredNumeric", "DBKeyExists('CERCAS_PENDIENTES')")]
		public byte HillGoingTypeId { get; set; }

		[Rules("ContinueIf('this.HillGoingGradeId != 0')", "DBKeyExists('CERCAS_PENDIENTES_GRADO')")]
		public byte HillGoingGradeId { get; set; }

		[Rules("ContinueIf('Array.Contains(new []{1, 2, 3}, this.RoadShapeId)')", "RequiredNumeric", "DBKeyExists('CERCAS_PENDIENTES')")]
		public byte HillReturningTypeId { get; set; }

		[Rules("ContinueIf('this.HillReturningGradeId != 0')", "DBKeyExists('CERCAS_PENDIENTES_GRADO')")]
		public byte HillReturningGradeId { get; set; }

		public byte SpeedGoingMop { get; set; }

		public byte SpeedReturningMop { get; set; }

		public byte SpeedGoingMopRest { get; set; }

		public byte SpeedReturningMopRest { get; set; }

		public byte SpeedGoingCustomRest { get; set; }

		public byte SpeedReturningCustomRest { get; set; }


		public byte OptimalSpeed { get; set; }

		public double OptimalTiming { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('CERCAS_FORMAS')")]
		public byte ShapeId { get; set; }

		[Rules("ContinueIf('this.ShapeId == 1')", "GreaterThan(0)")]
		public double Radius { get; set; }

		public List<GeofenceCoord> Coords { get; set; }

	}
}
