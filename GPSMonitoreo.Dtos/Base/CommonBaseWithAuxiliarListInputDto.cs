using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseWithAuxiliarListInputDto<TId> : CommonBaseListInputDto<TId>
	{
		public string AuxiliaryId { get; set; }
	}
}
