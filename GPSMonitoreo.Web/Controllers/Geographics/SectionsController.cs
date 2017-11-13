using Microsoft.AspNetCore.Mvc;
using MVCHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Services;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;

using GPSMonitoreo.Services.Base.Sections;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Data.Extensions;
using Newtonsoft.Json.Linq;
using GPSMonitoreo.Dtos.Base.Sections;
using System.Data;
using Newtonsoft.Json.Converters;
using GPSMonitoreo.Web.Models;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.Geographics
{

	
    public class SectionsController : AdminBaseController
	{
		private CommonDbEntityService _commonDbEntityService;

		private SectionService _sectionService;

		public SectionsController(CommonDbEntityService commonDbEntityService, SectionService sectionservice)
		{
			_commonDbEntityService = commonDbEntityService;
			_sectionService = sectionservice;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> WindowSearch()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "GEOGRAFICO::TRAMOS",
				FormId = "GeographicsSectionsWindowSearch",
				PostUrl = "/geographics/sections/search"
			};

			model.PrepareUrl(this.HttpContext);

			return View(model);
		}

		public async Task<JsonResponse> Search(SectionFilterInputDto input)
		{
			var result = await _sectionService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}

		public IActionResult EditForm()
		{
			ViewData["categories"] = DBContext.CategoriesTree<TRAMOS_CATS>().ToJqwidgetsTree();

			return View();
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _sectionService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}

		public async Task<JsonResponse> Save(SectionInputDto input)
		{
			var operation = new CreateUpdateOperation<int, SectionInputDto>
			{
				InputDto = input,
				Validate = _sectionService.Validate,
				CreateAsync = _sectionService.CreateAsync,
				UpdateAsync = _sectionService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public JsonResponse GeofencesForMap(int id)
		{
			var allGeofencesResult = _sectionService.GetAllGeofencesForMap(id);
			var allOtherGeofencesResult = _sectionService.GetAllOtherGeofencesForMap(id);

			return JsonResponse("OK", new { Geofences = allGeofencesResult.Items, OtherGeofences = allOtherGeofencesResult.Items });
		}
	}
}
