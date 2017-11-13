using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class FormRelationalGrid2 : ViewComponent
	{

		//public FormRelationalGrid(object pars)
		//{


		//}

		public IViewComponentResult Invoke(Models.FormRelationalGridModel model)
		{
			return View("~/Views/Shared/Components/FormRelationalGrid2.cshtml", model);
		}
	}
}
