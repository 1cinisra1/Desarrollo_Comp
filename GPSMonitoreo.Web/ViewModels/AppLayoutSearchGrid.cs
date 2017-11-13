using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewModels
{
	public class AppLayoutSearchGrid : AppLayout
	{
		public string FormId { get; set; }
		public string PostUrl { get; set; }

		public string EditDataUrl { get; set; }

		public bool ShowRowEditButton { get; set; }

		public string RowEditMethod { get; set; }

		public AppLayoutSearchGrid()
		{

		}

		public AppLayoutSearchGrid(ViewContext viewContext) : base(viewContext)
		{
			Url = BaseUrl;
			PostUrl = BaseUrl + "/search";
		}


		public string RenderOptions()
		{
			var ret = new System.Text.StringBuilder();

			ret.Append($"options.searchUrl = '{PostUrl}';");

			if (ShowRowEditButton && RowEditMethod != null)
			{
				ret.Append($"options.showRowEditButton = true;");
				ret.Append($"options.rowEditMethod = function(id, row){{{RowEditMethod}}};");

				/*
		options.searchUrl = '@Model.PostUrl';
		options.showRowEditButton = @Model.ShowRowEditButton.ToString().ToLower();
				*/
			}



			return ret.ToString();
		}
	}
}
