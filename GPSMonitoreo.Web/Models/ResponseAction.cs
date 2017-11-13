using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Models
{
    public class ResponseAction
    {
		[JsonConverter(typeof(StringEnumConverter), true )]
		public enum ResponseActionName
		{
			Notification,
			Warning
		}

		[JsonProperty(PropertyName = "action")]
		public ResponseActionName ActionName;

		[JsonProperty(PropertyName = "parameters")]
		public List<object> Parameters;

		public ResponseAction(ResponseActionName actionName, object parameter)
		{
			ActionName = actionName;
			Parameters = new List<object> { parameter };
		}
	}
}
