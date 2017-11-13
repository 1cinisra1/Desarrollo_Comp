using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using GPSMonitoreo.Web.ViewModels;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Services.Base.Users;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Dtos.Base.Users;
using GPSMonitoreo.Services.Base.Roles;

using GPSMonitoreo.Libraries.Extensions.StringExtensions;

namespace GPSMonitoreo.Web.Controllers.Authorization
{

	public class UsersController : BaseController
	{
		private UserService _userService;

		private RoleService _roleService;

		public UsersController
		(
			UserService userService,
			RoleService roleService
		)
		{
			_userService = userService;
			_roleService = roleService;
		}

		public IActionResult Index()
		{

			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "GEOGRAFICO::TRAYECTOS",
				FormId = "GeographicsCoursesSearch",
				PostUrl = "/authorization/users/search",
				ShowRowEditButton = true,
				RowEditMethod = "App.authorization.users.edit(id);"
			};

			model.PrepareUrl(this.HttpContext);

			

			return View(model);

		}

		public async Task<IActionResult> EditForm()
		{
			var model = new AppLayoutFreeForm()
			{
				FormId = "form_users",
				Title = "USUARIOS::USUARIO",
				//PostUrl = "/authorization/users/save"
			};
			model.PrepareEditUrls(this.HttpContext);

			var roles = (await _roleService.GetListAsync()).Items;

			ViewData["roles"] = roles;

			return View(model);
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _userService.GetByIdAsync(id);

			if (result.Succeeded)
			{
				//return JsonRecord(model, new { entidad_label = DBContext.ENTIDADES.FirstOrDefault(x => x.ID == model.entidad).DESCRIPCION_LARGA });
				return JsonRecord(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<JsonResponse> Save(UserInputDto input)
		{

			var validationResult = await _userService.ValidateAsync(input);

			if (validationResult.Succeeded)
			{
				var result = await _userService.CreateOrUpdateAsync(input);

				if (result.Succeeded)
				{
					return JsonNotification(result.Message, new { id = result.Id });

				}
				else
				{
					return null;
				}
			}
			else
			{
				return JsonFormErrors(validationResult.ValidationErrors);
			}
		}

		public async Task<JsonResponse> Search(UserFilterInputDto input)
		{
			var result = await _userService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
			//return JsonRecords(null, 0);
		}

	}
}
