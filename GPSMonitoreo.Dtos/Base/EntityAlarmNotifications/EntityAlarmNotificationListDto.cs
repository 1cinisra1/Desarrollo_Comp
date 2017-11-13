using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.EntityAlarmNotifications
{
    public class EntityAlarmNotificationListDto : BaseListDto<int>
    {

		public string RecipientLastNames { get; set; }

		public string RecipientNames { get; set; }

	}
}
