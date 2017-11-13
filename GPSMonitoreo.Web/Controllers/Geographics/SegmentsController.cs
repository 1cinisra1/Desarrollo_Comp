using Microsoft.AspNetCore.Mvc;
using MVCHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Services;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;

using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Data.Extensions;
using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json.Converters;
using GPSMonitoreo.Web.Models;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.Segments;
using GPSMonitoreo.Dtos.Base.Segments;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.Geographics
{

	
    public class SegmentsController : AdminBaseController
	{

		private CommonDbEntityService _commonDbEntityService;

		private SegmentService _segmentService;

		public SegmentsController(CommonDbEntityService commonDbEntityService, SegmentService segmentService)
		{
			_commonDbEntityService = commonDbEntityService;
			_segmentService = segmentService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> WindowSearch()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "GEOGRAFICO::SEGMENTOS",
				FormId = "GeographicsSegmentsWindowSearch",
				PostUrl = "/geographics/segments/search"
			};

			model.PrepareUrl(this.HttpContext);

			return View(model);
		}

		public async Task<JsonResponse> Search(SegmentFilterInputDto input)
		{
			var result = await _segmentService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}

		public IActionResult EditForm()
		{
			ViewData["categories"] = DBContext.CategoriesTree<SEGMENTOS_CATS>().ToJqwidgetsTree();

			return View();
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _segmentService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}

		public async Task<JsonResponse> Save(SegmentInputDto input)
		{
			var operation = new CreateUpdateOperation<int, SegmentInputDto>
			{
				InputDto = input,
				Validate = _segmentService.Validate,
				CreateAsync = _segmentService.CreateAsync,
				UpdateAsync = _segmentService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public JsonResponse GeofencesForMap(int id)
		{
			var allGeofencesResult = _segmentService.GetAllGeofencesForMap(id);
			var allOtherGeofencesResult = _segmentService.GetAllOtherGeofencesForMap(id);

			return JsonResponse("OK", new { Geofences = allGeofencesResult.Items, OtherGeofences = allOtherGeofencesResult.Items });
		}
	}
}
