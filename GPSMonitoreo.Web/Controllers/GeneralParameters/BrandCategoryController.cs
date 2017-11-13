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
    public class BrandCategoryController : AdminCommonDbEntityController
	{
		public BrandCategoryController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntityCategorySearch<short>(typeof(MARCAS_CATS), input);
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["cats"] = DBContext.CategoriesTree<MARCAS_CATS>().ToJqwidgetsTree();

			return await CommonDbEntityCategoryPopupEdit(typeof(MARCAS_CATS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityCategoryInputDto<short> input)
		{
			return await CommonDbEntitySave(typeof(MARCAS_CATS), input);
		}
	}
}
