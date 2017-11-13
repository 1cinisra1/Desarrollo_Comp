using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GPSMonitoreo.Web.ViewComponents
{
    public class FormRelationalGrid : ViewComponent
	{

		//public FormRelationalGrid(object pars)
		//{


		//}

		public IViewComponentResult Invoke(Models.FormRelationalGridModel model)
		{
			//var items = await GetItemsAsync(maxPriority, isDone);

			//Utils.dump(ViewContext.ViewData);

			//return View("~/ViewComponents/Views/FormRelationalGrid.cshtml");
			//return View("~/ViewComponents/Views/FormRelationalGrid.cshtml", model);
			//return View("~/Views/Shared/Components/FormRelationalGrid.cshtml");
			return View("~/Views/Shared/Components/FormRelationalGrid.cshtml", model);
		}


		//public async Task<IViewComponentResult> InvokeAsync(Models.FormRelationalGridModel model)
		//{
		//	var items = await GetItemsAsync(maxPriority, isDone);

		//	return View("~/ViewComponents/Views/FormRelationalGrid.cshtml", model);
		//	return View("~/Views/Shared/Components/FormRelationalGrid.cshtml");
		//}

	}
}
