using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Models;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.EquipmentCapabilities;
using GPSMonitoreo.Services.Base.Models;
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
    public class ModelController : AdminBaseController
    {
		private CommonDbEntityService _commonDbEntityService;

		private ModelService _modelService;

		public ModelController(CommonDbEntityService commonDbEntityService, ModelService modelService)
		{
			_commonDbEntityService = commonDbEntityService;
			_modelService = modelService;
		}


		public IActionResult Index(int id)
		{
			return View();
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["brands"] = (await _commonDbEntityService.GetSimpleListAsync<short>(typeof(MARCAS), new Dtos.Base.CommonDbEntities.CommonDbEntityFilterInputDto { Paging = false, OrderBy = FilterInputOrderBy.DefaultOrderByDescriptionAscending })).Items;

			if (id > 0)
			{
				ViewData["data"] = (await _modelService.GetByIdAsync(id)).Item;
			}

			return View();
		}

		public async Task<JsonResponse> Save(ModelInputDto input)
		{
			var operation = new CreateUpdateOperation<short, ModelInputDto>
			{
				InputDto = input,
				Validate = _modelService.Validate,
				CreateAsync = _modelService.CreateAsync,
				UpdateAsync = _modelService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public async Task<JsonResponse> Search(ModelFilterInputDto input)
		{
			var result = await _modelService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}
	}
}
