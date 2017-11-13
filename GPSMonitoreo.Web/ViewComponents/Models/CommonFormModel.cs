using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewComponents.Models
{
    public class CommonFormModel : FormModel
    {

		public CommonFormModel()
		{

		}

		public CommonFormModel(ViewContext viewContext)
		{
			var controllerActionDescriptor = viewContext.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
			var controllerNamespace = controllerActionDescriptor.ControllerTypeInfo.Namespace;
			var controllerName = controllerActionDescriptor.ControllerName;
			var actionName = controllerActionDescriptor.ActionName;


			var folder = controllerNamespace.Split(new[] { "." }, StringSplitOptions.None).Last();

			string baseUrl = "/" + folder.ToLower() + "/" + controllerName.ToLower();

			Action = baseUrl + "/save";
			FormId = folder + controllerName + actionName;
		}

		public bool ForCats { get; set; }

		public bool ForPopupEdit { get; set; }


	}
}
