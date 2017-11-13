using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EquipmentCapabilities
{
    public class EquipmentCapabilityListDto : CommonBaseWithAuxiliarSimpleListDto<short>
    {
		public string MeasureUnitDescription { get; set; }

		public string Hierarchy { get; set; }
	}
}
