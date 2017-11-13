using GPSMonitoreo.Libraries.Extensions.StringExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewModels
{
	public class AppLayout
	{
		public string Url { get; set; }
		public string Title { get; set; }

		public string SubTitle { get; set; }


		public string BaseUrl { get; }

		public string ControllerNamespace { get; }

		public string ControllerName { get; }

		public string ActionName { get; }

		public string ControllerFolderName { get; }


		public AppLayout()
		{

		}

		public AppLayout(ViewContext viewContext)
		{
			var controllerActionDescriptor = viewContext.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
			ControllerNamespace = controllerActionDescriptor.ControllerTypeInfo.Namespace;
			ControllerName = controllerActionDescriptor.ControllerName;
			ActionName = controllerActionDescriptor.ActionName;
			ControllerFolderName = ControllerNamespace.Split(new[] { "." }, StringSplitOptions.None).Last();
			BaseUrl = "/" + ControllerFolderName.ToLower() + "/" + ControllerName.ToLower();
		}

		public void PrepareUrl(Microsoft.AspNetCore.Http.HttpContext context)
		{
			Url = context.Request.Path.Value;
		}


		//public virtual string RenderInitializers()
		//{
		//	var ret = new System.Text.StringBuilder();

		//	if (!SubTitle.IsNullOrWhiteSpace())
		//	{
		//		ret.Append("layout.setSubTitle('" + SubTitle.EscapeSingleQuotes() + "');");
		//	}

		//	return ret.ToString();

		//}

	}
}
