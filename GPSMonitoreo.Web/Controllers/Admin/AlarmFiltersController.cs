using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using GPSMonitoreo.Libraries.Extensions.EnumExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Core.Authorization;
using GPSMonitoreo.Services.Base.Roles;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Dtos.Misc.AlarmFilters;
using GPSMonitoreo.Services.Misc.AlarmFilters;
using static GPSMonitoreo.Libraries.Utils.Data;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using MVCHelpers.ActionResults;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers.Admin
{
    public class AlarmFiltersController : BaseController
    {

		private RoleService _roleService;
		private AlarmFilterService _alarmFilterService;


		public AlarmFiltersController(RoleService roleService, AlarmFilterService alarmFilterService)
		{
			_roleService = roleService;
			_alarmFilterService = alarmFilterService;
		}

		// GET: /<controller>/
		public async Task<IActionResult> Index()
        {




			var alarmsDataTable = DBContext.CategoriesTwoTablesTreeOrdenador<GPSMonitoreo.Data.Models.ALARMAS_CATS, GPSMonitoreo.Data.Models.ALARMAS>();

			var mapper = new JsonTreeMapper
			{
				id = "ID",
				description = "DESCRIPCION_LARGA",
				level = "LEV",
				jsonNodeId = "Id",
				jsonNodeDescription = "Description",
				jsonNodeChildren = "children"
			};

			ViewData["alarmsJson"] = alarmsDataTable.ToJqwidgetsTree(mapper, false);


			var applicableActions = new PermissionAction[]
			{
				PermissionAction.View
			};

			var actions = applicableActions.Select(x => new
			{
				Id = (int)x,
				Description = x.ToLocalizedString()
			});


			ViewData["actions"] = actions.ToJsonString();


			var rolesResult = await _roleService.GetListAsync();

			var roles = rolesResult.Items.OrderBy(x => x.Description).ToList();

			var superAdminRole = roles.First(x => x.Id == 1);

			roles.Remove(superAdminRole);
			roles.Insert(0, superAdminRole);

			


			ViewData["roles"] = roles.ToJsonString();

			var currentRequiredPermissionsResult = await _alarmFilterService.GetAsync();

			ViewData["currentRequiredPermissions"] = currentRequiredPermissionsResult.Item.RequiredPermissions.ToJsonString();


			var model = new GPSMonitoreo.Web.ViewModels.AppLayoutFreeForm()
			{
				Title = "FILTROS ALARMAS",
				FormId = "alarmFiltersForm",
				MainTabTitle = "Permisos Requeridos",
				MainTabStyle = "",
				PostUrl = "/admin/alarmfilters/save",
				UseFreeFormTable = false,
				FormStyle = "display:block;position:relative;width:100%;height:100%;"
			};

			model.PrepareUrl(this.HttpContext);


			return View(model);
        }

		public async Task<IActionResult> Save(AlarmFilterInputDto input)
		{
			Utils.dump(input);
			var result = await _alarmFilterService.UpdateAsync(input);

			return JsonNotification(result.Message);

		}

		public async Task<JsonResponse> Get()
		{

			var ll = await _alarmFilterService.GetAsync();

			return JsonRecord(ll.Item);
		}

	}
}
