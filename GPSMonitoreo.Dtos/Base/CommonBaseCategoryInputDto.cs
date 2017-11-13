using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseCategoryInputDto<TId> : CommonBaseInputDto<TId>
	{
		public virtual TId ParentId { get; set; }

		[Rules("MaxLength(5)")]
		public string Order { get; set; }

    }
}
