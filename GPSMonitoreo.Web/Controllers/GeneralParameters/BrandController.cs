using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Brands;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.EquipmentCapabilities;
using GPSMonitoreo.Services.Base.Brands;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class BrandController : AdminBaseController
    {
		private CommonDbEntityService _commonDbEntityService;

		private BrandService _brandService;

		public BrandController(CommonDbEntityService commonDbEntityService, BrandService brandService)
		{
			_commonDbEntityService = commonDbEntityService;
			_brandService = brandService;
		}


		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["cats"] = DBContext.CategoriesTree<MARCAS_CATS>().ToJqwidgetsTree();

			if (id > 0)
			{
				ViewData["data"] = (await _brandService.GetByIdAsync(id)).Item;
			}

			return View();
		}

		public async Task<JsonResponse> Save(BrandInputDto input)
		{
			var operation = new CreateUpdateOperation<short, BrandInputDto>
			{
				InputDto = input,
				Validate = _brandService.Validate,
				CreateAsync = _brandService.CreateAsync,
				UpdateAsync = _brandService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public async Task<JsonResponse> Search(BrandFilterInputDto input)
		{
			var result = await _brandService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}
	}
}
