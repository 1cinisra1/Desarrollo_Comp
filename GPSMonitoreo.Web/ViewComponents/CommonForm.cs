using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class CommonForm : ViewComponent
	{
		//public IViewComponentResult Invoke(Models.FormRelationalGridModel model)
		//{
		//	return View("~/Views/Shared/Components/CommonForm.cshtml", model);
		//}

		public IViewComponentResult Invoke(Models.CommonFormModel model)
		{
			return View("~/Views/Shared/Components/CommonForm.cshtml", model);
		}
	}
}
