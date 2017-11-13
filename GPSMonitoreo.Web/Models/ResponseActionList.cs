using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Models
{
    public class ResponseActionList: List<ResponseAction>
    {
		public void AddNotification(string message)
		{
			Add(new ResponseAction(ResponseAction.ResponseActionName.Notification, message));
		}

		public void AddWarning(string message)
		{
			Add(new ResponseAction(ResponseAction.ResponseActionName.Warning, message));
		}
    }
}
