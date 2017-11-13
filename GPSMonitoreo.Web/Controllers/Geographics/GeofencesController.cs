using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Services.Base.Geofences;
using GPSMonitoreo.Dtos.Base.Geofences;
using GPSMonitoreo.Services;
using GPSMonitoreo.Web.Infrastructure;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers.Geographics
{
    public class GeofencesController : AdminBaseController
	{

		private CommonDbEntityService _commonDbEntityService;

		private GeofenceService _geofenceService;

		public GeofencesController(CommonDbEntityService commonDbEntityService, GeofenceService geofenceService)
		{
			_commonDbEntityService = commonDbEntityService;
			_geofenceService = geofenceService;
		}


		public async Task<IActionResult> Index()
		{
			ViewData["regions"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.REGIONES), null)).Items;

			ViewData["layers"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CAPAS), null)).Items;

			return View();
		}

		public async Task<IActionResult> WindowSearch()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "GEOGRAFICO::CERCAS",
				FormId = "GeographicsGeofencesWindowSearch",
				PostUrl = "/geographics/geofences/search"
			};

			model.PrepareUrl(this.HttpContext);

			ViewData["regions"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.REGIONES), null)).Items;

			ViewData["layers"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CAPAS), null)).Items;

			return View(model);
		}

		public async Task<JsonResponse> Search(GeofenceFilterInputDto input)
		{
			var result = await _geofenceService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}


		public async Task<IActionResult> EditForm()
		{
			ViewData["regions"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.REGIONES), null)).Items;

			ViewData["layers"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CAPAS), null)).Items;

			ViewData["roadShapes"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_TRAZADO_VIA), null)).Items;

			ViewData["roadSurfaces"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CALZADAS), null)).Items;

			ViewData["roadSurfaceStates"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CALZADAS_ESTADOS), null)).Items;

			ViewData["curveTypes"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CURVAS_TIPOS), null)).Items;

			ViewData["curveGrades"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_CURVAS_GRADO), null)).Items;

			ViewData["hillTypes"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_PENDIENTES), null)).Items;

			ViewData["hillGrades"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_PENDIENTES_GRADO), null)).Items;

			ViewData["roadTraffics"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.CERCAS_TRAFICO), null)).Items;

			return View();
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _geofenceService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}

		public async Task<IActionResult> Details(int id)
		{
			var result = await _geofenceService.GetDetailsByIdAsync(id);

			if (result.Succeeded)
			{
				return View(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<IActionResult> Quickview(int id)
		{
			var result = await _geofenceService.GetDetailsByIdAsync(id);

			if (result.Succeeded)
			{
				return View(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<JsonResponse> Coords(int id)
		{
			var result = await _geofenceService.GetCoordsByIdAsync(id);

			return JsonRecord(result.Item);
		}

		public async Task<JsonResponse> UpdateCoords(GeofenceUpdateCoordsInputDto input)
		{
			await _geofenceService.UpdateCoordsAsync(input);

			return JsonNotification(Resources.Messages.CoordsSaved);
		}

		public async Task<JsonResponse> Save(GeofenceInputDto input)
		{
			var operation = new CreateUpdateOperation<int, GeofenceInputDto>
			{
				InputDto = input,
				Validate = _geofenceService.Validate,
				CreateAsync = _geofenceService.CreateAsync,
				UpdateAsync = _geofenceService.UpdateAsync
			};

			return await CommonSave(operation);
		}
	}
}
