using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Extensions.CollectionExtensions
{
    public static class CollectionExtensions
    {

		//public static string ToJsonString(this IEnumerable<object> list)
		//public static string ToJsonString(this IEnumerable<object> list)
		//{
		//	return Newtonsoft.Json.JsonConvert.SerializeObject(list);
		//}

		public static string ToJsonString<T>(this IEnumerable<T> list)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(list);
		}


		public static void Add<T1, T2>(this ICollection<KeyValuePair<T1, T2>> target, T1 item1, T2 item2)
		{
			if (target == null)
				throw new ArgumentNullException(nameof(target));

			target.Add(new KeyValuePair<T1, T2>(item1, item2));
		}
	}
}
