using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Web.QueryModels;
using GPSMonitoreo.Data;
using Newtonsoft.Json.Linq;
using static GPSMonitoreo.Libraries.Utils.Data;
using GPSMonitoreo.Dtos.Base;

namespace GPSMonitoreo.Web.Extensions
{
	public static class Extensions
	{
		public static void AddModelError(this JObject errors, string fieldName, string errorMessage)
		{
			errors.Add(fieldName, JToken.FromObject(new { error = errorMessage }));
		}
	}
}
