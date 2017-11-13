using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCHelpers.Extensions
{
	public static class HtmlExtensions
	{
		public static HtmlString ValueOrSpace(this IHtmlHelper html, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new HtmlString("&nbsp;");
			}
			return new HtmlString(html.Encode(value));
		}
	}
}
