using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EntityAlarmNotifications
{
    public class EntityAlarmNotificationInputDto : BaseInputDto<int>
	{

		
		public int EntityId { get; set; }


		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int RecipientId { get; set; }

		
		public string RecipientDescription { get; set; }

		public Int16 AlarmsRoleId { get; set; }


		[Rules("MinCount(1)")]
		public List<int> PlaceIds { get; set; }

		[Rules("MinCount(1)")]
		public List<int> EmailIds { get; set; }
	}
}
