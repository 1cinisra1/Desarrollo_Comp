using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GPSMonitoreo.Web.QueryModels
{
	public class JqwidgetsItem
	{
		public Int32 value;
		public string label;

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string html;
	}
}
