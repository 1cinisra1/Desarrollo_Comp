using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EntityAlarmNotifications
{
    public class EntityAlarmNotificationFilterInputDto : CommonBaseFilterInputDto
    {

		public int EntityId { get; set; }

		public string LastNames { get; set; }

		public string Names { get; set; }
	}
}
