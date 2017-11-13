using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class RouteCategoryController : AdminCommonDbEntityController
	{
		public RouteCategoryController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntityCategorySearch<short>(typeof(RUTAS_CATS), input);
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["cats"] = DBContext.CategoriesTree<RUTAS_CATS>().ToJqwidgetsTree();

			return await CommonDbEntityCategoryPopupEdit(typeof(RUTAS_CATS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityCategoryInputDto<short> input)
		{
			return await CommonDbEntitySave(typeof(RUTAS_CATS), input);
		}
	}
}
