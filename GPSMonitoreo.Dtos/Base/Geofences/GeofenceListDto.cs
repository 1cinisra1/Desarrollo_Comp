using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geofences
{
    public class GeofenceListDto: CommonBaseSimpleListDto<int>
    {
		public string AuxiliaryId { get; set; }

		public string LayerDescription { get; set; }

		public string CategoryDescription { get; set; }

		public List<CommonBaseWithAuxiliarSimpleListDto<int>> Routes { get; set; }

		public List<CommonBaseWithAuxiliarSimpleListDto<int>> Sections { get; set; }

		public List<CommonBaseWithAuxiliarSimpleListDto<int>> Segments { get; set; }
	}
}
