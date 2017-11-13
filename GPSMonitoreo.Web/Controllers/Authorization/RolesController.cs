using Microsoft.AspNetCore.Mvc;
using MVCHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Services.Base.Roles;
using GPSMonitoreo.Services;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Dtos.Base.Roles;
using GPSMonitoreo.Libraries.Helpers;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;

using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;

namespace GPSMonitoreo.Web.Controllers.Authorization
{

	
    public class RolesController : BaseController
	{

		RoleService _roleAppService;

		public RolesController(RoleService RoleAppService)
		{
			_roleAppService = RoleAppService;
		}

		public IActionResult Index()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "SEGURIDAD::ROLES",
				FormId = "SecurityRolesSearch",
				PostUrl = "/authorization/roles/search",
				ShowRowEditButton = true,
				RowEditMethod = "App.security.roles.edit(id);"
			};

			model.PrepareUrl(this.HttpContext);

			return View(model);
		}

		public IActionResult EditForm(int id)
		{
			var layoutModel = new ViewModels.AppLayoutForm
			{
				FormId = "SecurityRolesForm",
				Title = "AUTORIZACIONES::ROL"
			};

			layoutModel.PrepareEditUrls(this.HttpContext);

			var entitiesEnumList = EnumHelper.GetLocalizedPairs(Core.Resources.Entities.ResourceManager, typeof(Core.Enums.Entity));

			var entitiesPermissions = entitiesEnumList.Select(x => new Dtos.Base.Roles.PermissionListInputDto<int>
			{
				ElementId = x.Key,
				ElementDescription = x.Value
			})
			.OrderBy(x => x.ElementDescription)
			.ToList();
			;

			entitiesPermissions[0].Actions = new List<Core.Authorization.PermissionAction>() { Core.Authorization.PermissionAction.Create, Core.Authorization.PermissionAction.View };

			ViewData["entities"] = entitiesPermissions.ToJsonString();

			var alarmsDataTable = DBContext.CategoriesTwoTablesTreeOrdenador<GPSMonitoreo.Data.Models.ALARMAS_CATS, GPSMonitoreo.Data.Models.ALARMAS>();

			var mapper = new GPSMonitoreo.Libraries.Utils.Data.JsonTreeMapper
			{
				id = "ID",
				description = "DESCRIPCION_LARGA",
				level = "LEV",
				jsonNodeId = "Id",
				jsonNodeDescription = "Description",
				jsonNodeChildren = "children"
			};

			ViewData["alarmsJson"] = alarmsDataTable.ToJqwidgetsTree(mapper, false);


			//Utils.dump(entitiesPermissions);



			//var conveted = Newtonsoft.Json.Linq.JValue.CreateString("propóstios");

			//Console.WriteLine("converted: " + Newtonsoft.Json.JsonConvert.ToString("propósitos"));
			//Console.WriteLine("converted: " + conveted.ToString());


			return View(layoutModel);
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _roleAppService.GetByIdAsync(id);

			if (result.Succeeded)
			{
				return JsonRecord(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<JsonResponse> Search(RoleFilterInputDto input)
		{
			var result = await _roleAppService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
		}


		public async Task<JsonResponse> Save(RoleInputDto input)
		{
			var validationResult = await _roleAppService.ValidateAsync(input);

			if (validationResult.Succeeded)
			{
				CreateUpdateResult<int> result = null;

				if (input.Id == 0)
				{
					result = await _roleAppService.CreateAsync(input);
				}
				else
				{
					result = await _roleAppService.UpdateAsync(input);
				}

				if (result.Succeeded)
				{
					return JsonNotification(result.Message, new { id = result.Id });

				}

				//Utils.dump(result);

			}
			else
			{

				return JsonFormErrors(validationResult.ValidationErrors);
				//return errors;
			}


			return null;
		}

	}
}
