using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Entities
{
    public class EntityEmailListDto : BaseListDto<int>
	{
		public string Email { get; set; }


		public string EmailTypeDescription { get; set; }

		public string EmailPurposes { get; set; }

		//public string EmailPurposeDescription { get; set; }
    }
}
