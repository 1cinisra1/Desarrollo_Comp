using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web
{
    public class Utils
    {
		public static void dump(object obj)
		{
			var settings = new Newtonsoft.Json.JsonSerializerSettings();
			settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings));
		}

		public static void dump(string label, object obj)
		{
			Console.WriteLine(label + ":");
			dump(obj);
		}

		public static Newtonsoft.Json.Linq.JObject toJsonObject(object obj)
		{
			return Newtonsoft.Json.Linq.JObject.FromObject(obj);
		}

		//public static string formatearMonto(decimal val)
		//{

		//	return val.ToString("$ #,##0.0000", Globals.s_numberFormat);

		//}

		//public static string formatearCantidad(decimal val)
		//{

		//	return val.ToString("#,##0.0000", Globals.s_numberFormat);

		//}
	}
}
