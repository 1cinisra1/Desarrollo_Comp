using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EquipmentCapabilities
{
    public class EquipmentCapabilityInputDto : CommonBaseCategoryInputDto<short>
    {
		[Rules("ContinueIf('this.ParentId > 0')", "DBKeyExists('EQUIPOS_CAPS')")]
		public override short ParentId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('UNIDADES')")]
		public short MeasureUnitId { get; set; }
    }
}
