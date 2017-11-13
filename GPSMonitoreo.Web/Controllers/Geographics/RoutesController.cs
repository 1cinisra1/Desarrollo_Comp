using Microsoft.AspNetCore.Mvc;
using MVCHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Services;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;

using GPSMonitoreo.Services.Base.Routes;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Data.Extensions;
using Newtonsoft.Json.Linq;
using GPSMonitoreo.Dtos.Base.Routes;
using System.Data;
using Newtonsoft.Json.Converters;
using GPSMonitoreo.Web.Models;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Dtos.Base.Geographics.Routes;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.Geographics
{

	
    public class RoutesController : AdminBaseController
	{

		private CommonDbEntityService _commonDbEntityService;

		private RouteService _routeService;

		public RoutesController(CommonDbEntityService commonDbEntityService, RouteService routeService)
		{
			_commonDbEntityService = commonDbEntityService;
			_routeService = routeService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> WindowSearch()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "GEOGRAFICO::RUTAS",
				FormId = "GeographicsRoutesWindowSearch",
				PostUrl = "/geographics/routes/search"
			};

			model.PrepareUrl(this.HttpContext);

			return View(model);
		}

		public async Task<JsonResponse> Search(RouteFilterInputDto input)
		{
			var result = await _routeService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
		}

		public async Task<IActionResult> EditForm()
		{
			ViewData["categories"] = DBContext.CategoriesTree<RUTAS_CATS>().ToJqwidgetsTree();

			return View();
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _routeService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}

		public async Task<JsonResponse> Save(RouteInputDto input)
		{
			var operation = new CreateUpdateOperation<int, RouteInputDto>
			{
				InputDto = input,
				Validate = _routeService.Validate,
				CreateAsync = _routeService.CreateAsync,
				UpdateAsync = _routeService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public IActionResult RouteTemplate()
		{

			ViewData["categories"] = DBContext.CategoriesTree<RUTAS_CATS>().ToJqwidgetsTree();

			var layoutModel = new ViewModels.AppLayoutForm
			{
				FormId = "formReturningRoutes",
				Title = "GEOGRAFICO::PLANTILLA RUTAS",
				PostUrl = "/geographics/routes/routetemplatesave"
			};

			layoutModel.PrepareUrl(this.HttpContext);

			//layoutModel.PrepareEditUrls(this.HttpContext);

			Console.WriteLine("showing view");



			return View(layoutModel);
		}

		public async Task<JsonResponse> RouteTemplateSave(RouteTemplateInputDto input)
		{

			//return JsonResponseActions(new List<ResponseAction>
			//{
			//	new ResponseAction(ResponseAction.ResponseActionName.Notification, "Resgistro guardado exitosamente" ),
			//	new ResponseAction(ResponseAction.ResponseActionName.Warning, "Los siguientes tramos no fueron creado" )
			//}, new { id = 500 });


			if (input.Id > 0)
			{
				return JsonError("No puede editar la ruta. La misma ya fue creada con sus respectivos tramos y segmentos. Debe editar la ruta desde la opción de Rutas");
			}
			else
			{
				var validationResult = await _routeService.ValidateTemplateAsync(input);

				if (validationResult.Succeeded)
				{
					var result = await _routeService.CreateFromTemplateAsync(input);

					if (result.Succeeded)
					{
						var actionsList = new ResponseActionList();

						actionsList.AddNotification(result.Message);

						if(result.Status == Services.Enums.OperationStatus.SuccessAndWarning)
						{
							var warningMessage = result.WarningMessage + "<br/><br/><ul>";

							foreach(var skippedSection in result.SkippedSections)
							{
								warningMessage += "<li>" + skippedSection.Description + "</li>";

							}

							warningMessage += "</ul>";

							actionsList.AddWarning(warningMessage);
						}

						return JsonResponseActions(actionsList, new { id = result.Id });

						//return JsonNotification(result.Message, new { id = result.Id });

					}
				}
				else
				{
					return JsonFormErrors(validationResult.ValidationErrors);
				}
			}

			return null;
		}


		[Route("/geographics/[controller]/[action]/{routeId}/{type}/{direction}")]
		public IActionResult RouteTemplateData(int routeId, int type, int direction)
		{
			var parameters = new List<KeyValuePair<string, object>>
			{
				{"RUTA_ID", routeId },
				{"P_ASCENDENTE", direction == 1 ? 1 : 0 }
			};

			var dt = DBContext.ProcedureDataTable("SP_RUTA_JERARQUIA", parameters);

			dt.Columns["DESCRIPCION_LARGA"].MaxLength = 500;

			string description;

			foreach(DataRow row in dt.Rows)
			{
				if(Convert.ToInt32(row["LEV"]) < 3)
				{
					description = row["DESCRIPCION_LARGA"] as string;

					if(direction == 1) //same direction
					{
						//no need to change description
					}
					else
					{
						if(type == 1) //new route will be created as going route
						{
							if(description.StartsWith("R-"))
							{
								description = description.Substring(2);
							}
						}
						else //will be created as returning route
						{
							description = "R-" + description;
						}
					}

					row["DESCRIPCION_LARGA"] = description;
				}
			}

			var mapper = new GPSMonitoreo.Libraries.Utils.Data.JsonTreeMapper
			{
				id = "ID",
				description = "DESCRIPCION_LARGA",
				level = "LEV",
				jsonNodeId = "Id",
				jsonNodeDescription = "Description",
				jsonNodeChildren = "children"
			};

			var records = JArray.Parse(GPSMonitoreo.Libraries.Utils.Data.DataTableToJsonTree(dt, mapper));

			//var records = JObject.Parse(dt.ToJqwidgetsTree());


			return JsonRecords(records);

			//return null;

		}

		public JsonResponse GeofencesForMap(int id)
		{
			var allGeofencesResult = _routeService.GetAllGeofencesForMap(id);
			var allOtherGeofencesResult = _routeService.GetAllOtherGeofencesForMap(id);

			return JsonResponse("OK", new { Geofences = allGeofencesResult.Items, OtherGeofences = allOtherGeofencesResult.Items });
		}
	}
}
