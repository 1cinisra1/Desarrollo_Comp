using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseListInputDto<TId> : InputDto
    {
		public TId Id { get; set; }

		public string Description { get; set; }
    }
}
