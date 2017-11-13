using GPSMonitoreo.Services.Base.CommonDbEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Libraries.Extensions.TaskExtensions;
using GPSMonitoreo.Services.Base.EntityAddresses;
using GPSMonitoreo.Dtos.Base.EntityAddress;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Dtos.Base.EntityAlarmNotifications;
using GPSMonitoreo.Services.Base.EntityAlarmNotifications;
using GPSMonitoreo.Services;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.Entities
{

	//[Route("/entities/[controller]")]
	//[Route("/entities/[controller]/[action]")]
	public class EntityAlarmNotificationsController: AdminBaseController
	{

		private CommonDbEntityService _commonDbEntityService;

		private EntityAddressService _entityAddressService;

		private EntityAlarmNotificationService _entityAlarmNotificationService;

		public EntityAlarmNotificationsController(
			CommonDbEntityService commonDbEntityService, 
			EntityAddressService entityAddressService,
			EntityAlarmNotificationService entityAlarmNotificationService
		)
		{
			_commonDbEntityService = commonDbEntityService;
			_entityAddressService = entityAddressService;
			_entityAlarmNotificationService = entityAlarmNotificationService;
		}



		//[HttpGet("/entities/[controller]/[action]/{id:int?}")]
		//[HttpGet("{id:int}")]


		//[Route("/entities/[controller]/[action]/{id:int}")]
		[Route("/entities/[controller]/{id:int}")]
		//[HttpGet("{id:int}")]
		public async Task<IActionResult> Index(int id)
		{

			var model = new ViewModels.AppLayout()
			{
				Title = "ENTIDADES::NOTIFICACIONES ALARMAS",
				SubTitle = (await _commonDbEntityService.GetDescriptionAsync<int>(typeof(Data.Models.ENTIDADES), id)).Result
			};

			model.PrepareUrl(this.HttpContext);

			ViewData["entityId"] = id;

			return View(model);
		}

		public async Task<JsonResponse> Search(EntityAlarmNotificationFilterInputDto input)
		{
			var result = await _entityAlarmNotificationService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
			//return JsonRecords(null, 0);
		}

		private string GetEntityDescription(int id)
		{
			var result = _commonDbEntityService.GetDescriptionAsync<int>(typeof(Data.Models.ENTIDADES), id).RunSync();

			//task.RunSynchronously();

			Console.WriteLine("::::" + result.Result);

			return result.Result;
		}


		public async Task<IActionResult> EditForm(int id)
		{
			ViewData["roles"] = (await _commonDbEntityService.GetSimpleListAsync<Int16>(typeof(Data.Models.ROLES))).Items;

			ViewData["places"] = (await _entityAddressService.GetListAsync(new EntityAddressFilterInputDto { EntityId = id, TypeIds = new List<byte> { 3, 4, 6 }, Paging = false })).Items;

			//var alarmsDataTable = DBContext.CategoriesTwoTablesTreeOrdenador<GPSMonitoreo.Data.Models.ALARMAS_CATS, GPSMonitoreo.Data.Models.ALARMAS>();

			//var mapper = new GPSMonitoreo.Libraries.Utils.Data.JsonTreeMapper
			//{
			//	id = "ID",
			//	description = "DESCRIPCION_LARGA",
			//	level = "LEV",
			//	jsonNodeId = "Id",
			//	jsonNodeDescription = "Description",
			//	jsonNodeChildren = "children"
			//};

			//ViewData["alarmsJson"] = alarmsDataTable.ToJqwidgetsTree(mapper, false);

			var model = new ViewModels.AppLayoutFreeForm
			{
				FormId = "formEntityAlarmNotifications_" + id,
				Title = "ENTIDADES::NOTIFICACIONES ALARMAS::" + GetEntityDescription(id)
			};

			model.PrepareEditUrls(this.HttpContext, true);

			ViewData["entityId"] = id;

			return View(model);
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _entityAlarmNotificationService.GetByIdAsync(id);

			if (result.Succeeded)
			{
				return JsonRecord(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<JsonResponse> Save(EntityAlarmNotificationInputDto input)
		{
			var operation = new CreateUpdateOperation<int, EntityAlarmNotificationInputDto>
			{
				InputDto = input,
				Validate = _entityAlarmNotificationService.Validate,
				CreateAsync = _entityAlarmNotificationService.CreateAsync,
				UpdateAsync = _entityAlarmNotificationService.UpdateAsync
			};

			return await CommonSave(operation);

			//Console.WriteLine("before RunAsync");
			//operation.RunAsync();
			//Console.WriteLine("after RunAsync");

			//Utils.dump(operation.ValidationResult);



			//var validationResult = _entityAlarmNotificationService.Validate(input);


			//if (validationResult.Succeeded)
			//{
			//	CreateUpdateResult<int> result = null;

			//	if (input.Id == 0)
			//	{
			//		result = await _entityAlarmNotificationService.CreateAsync(input);
			//	}
			//	else
			//	{
			//		result = await _entityAlarmNotificationService.UpdateAsync(input);
			//	}

			//	if (result.Succeeded)
			//	{
			//		return JsonNotification(result.Message, new { id = result.Id });

			//	}
			//}
			//else
			//{
			//	return JsonFormErrors(validationResult.ValidationErrors);
			//}

			//return null;
		}
	}
}
