using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewModels
{
	public class AppLayoutForm: AppLayout
	{
		public string FormId { get; set; }
		public string PostUrl { get; set; }

		public string EditDataUrl { get; set; }

		public string MainTabTitle { get; set; } = "Principal";

		public string MainTabStyle { get; set; } = "padding:5px";


		public AppLayoutForm()
		{

		}

		public AppLayoutForm(ViewContext viewContext) : base(viewContext)
		{
			Url = BaseUrl + "/editform" ;
			PostUrl = BaseUrl + "/save";
			EditDataUrl = BaseUrl + "/editdata";
			FormId = "form" + ControllerFolderName + ControllerName;
		}

		public void PrepareEditUrls(Microsoft.AspNetCore.Http.HttpContext context,  bool baseUrlIncludesId = false)
		{
			PrepareUrl(context);
			var urlArr = Url.Split('/');
			var baseUrl = string.Join("/", urlArr.Take(urlArr.Length - (baseUrlIncludesId ? 2 : 1)));
			PostUrl = baseUrl + "/save";
			EditDataUrl = baseUrl + "/editdata";
		}

		public void PrepareEditUrls(Microsoft.AspNetCore.Http.HttpContext context, int extraParameters)
		{
			PrepareUrl(context);
			var urlArr = Url.Split('/');
			var baseUrl = string.Join("/", urlArr.Take(urlArr.Length - extraParameters));
			PostUrl = baseUrl + "/save";
			EditDataUrl = baseUrl + "/editdata";
		}
	}
}
