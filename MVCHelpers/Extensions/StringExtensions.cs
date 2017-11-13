using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCHelpers.Extensions
{
    public static class StringExtensions
    {
		public static string ReplaceArgs(this string str, params object[] args)
		{
			return string.Format(str, args);
		}
	}
}
