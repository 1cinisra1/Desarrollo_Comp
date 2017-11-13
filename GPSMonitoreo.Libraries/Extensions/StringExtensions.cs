using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Extensions.StringExtensions
{
    public static class StringExtensions
    {
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static bool IsNullOrWhiteSpace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		public static string Prefix(this string str, string prefix)
		{
			if (str != null)
			{
				return prefix + str;
			}
			else
			{
				return null;
			}
		}


		public static string EscapeSingleQuotes(this string str)
		{
			if(str != null)
			{
				return str.Replace("'", "\'");
			}
			else
			{
				return null;
			}
		}
	}
}
