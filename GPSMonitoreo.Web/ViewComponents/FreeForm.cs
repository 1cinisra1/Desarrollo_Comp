using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class FreeForm : ViewComponent
	{

		public IViewComponentResult Invoke(Models.FormModel model)
		{
			return View("~/Views/Shared/Components/FreeForm.cshtml", model);
		}
	}
}
