using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class CommonCalendarioForm : ViewComponent
	{

		public IViewComponentResult Invoke(Models.CommonFormModel model)
		{
			return View("~/Views/Shared/Components/CommonCalendarioForm.cshtml", model);
		}

	}
}
