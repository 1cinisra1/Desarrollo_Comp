using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Helpers
{
    public static class EnumHelper
    {
		public static List<string> GetLocalizedNames(ResourceManager resourceManager, Type enumType)
		{
			var names = Enum.GetNames(enumType);

			var ret = new List<string>(names.Length);

			foreach(var name in names)
			{
				ret.Add(resourceManager.GetString(name) ?? name);
			}

			return ret;
		}

		public static List<KeyValuePair<int, string>> GetLocalizedPairs(ResourceManager resourceManager, Type enumType)
		{
			var names = Enum.GetNames(enumType);
			var values = Enum.GetValues(enumType);

			

			var ret = new List<KeyValuePair<int, string>>(names.Length);

			int x = 0;

			string resourceValue;

			foreach(var val in values)
			{
				resourceValue = resourceManager.GetString(names[x]);

				ret.Add(new KeyValuePair<int, string>((int)val, string.IsNullOrEmpty(resourceValue) ? names[x] : resourceValue));
				x++;
			}



			return ret;
		}
	}
}
