using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class CommonForm3 : ViewComponent
	{

		public IViewComponentResult Invoke(Models.CommonFormModel model)
		{
			return View("~/Views/Shared/Components/CommonForm3.cshtml", model);
		}
	}
}
