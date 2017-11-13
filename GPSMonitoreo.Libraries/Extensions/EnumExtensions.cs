using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Libraries.DataAnnotations;

namespace GPSMonitoreo.Libraries.Extensions.EnumExtensions
{
    public static class EnumExtensions
    {
		public static string ToLocalizedString(this Enum enumValue, System.Globalization.CultureInfo cultureInfo = null)
		{
			var attrs = enumValue.GetType().GetCustomAttributes(typeof(ResourceManagerAttribute), false);

			if(attrs.Length > 0)
			{
				var resourceManagerAttribute = attrs[0] as ResourceManagerAttribute;

				var ret = resourceManagerAttribute.ResourceManager.GetString(enumValue.ToString(), cultureInfo);

				if(string.IsNullOrEmpty(ret))
				{
					return enumValue.ToString();
				}
				else
				{
					return ret;
				}
			}
			return enumValue.ToString();
		}
    }
}
