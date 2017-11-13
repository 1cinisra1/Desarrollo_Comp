using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using GPSMonitoreo.Services;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers
{
    public class AdminCommonDbEntityController : AdminBaseController
    {
		private CommonDbEntityService _commonDbEntityService;

		public AdminCommonDbEntityController(CommonDbEntityService commonDbEntityService)
		{
			_commonDbEntityService = commonDbEntityService;
		}

		protected async Task<JsonResponse> CommonDbEntitySearch<TId>(Type dbEntityType, CommonDbEntityFilterInputDto input) where TId : struct
		{
			var result = await _commonDbEntityService.GetListAsync<TId>(dbEntityType, input);

			return GetListResultJsonResponse(result);
		}

		protected async Task<JsonResponse> CommonDbEntityCategorySearch<TId>(Type dbEntityType, CommonDbEntityFilterInputDto input) where TId : struct
		{
			var result = await _commonDbEntityService.GetCategoryListAsync<TId>(dbEntityType, input);

			return GetListResultJsonResponse(result);
		}

		public async Task<IActionResult> CommonDbEntityPopupEdit<TId>(Type dbEntityType, TId id) where TId : struct, IComparable<TId>, IEquatable<TId>
		{
			if (id.CompareTo(default(TId)) > 0)
			{
				ViewData["data"] = (await _commonDbEntityService.GetByIdAsync(dbEntityType, id)).Item;
			}

			return View();
		}

		public async Task<IActionResult> CommonDbEntityCategoryPopupEdit<TId>(Type dbEntityType, TId id) where TId : struct, IComparable<TId>, IEquatable<TId>
		{
			if (id.CompareTo(default(TId)) > 0)
			{
				ViewData["data"] = (await _commonDbEntityService.GetCategoryByIdAsync(dbEntityType, id)).Item;
			}

			return View();
		}

		public async Task<JsonResponse> CommonDbEntitySave<TId>(Type dbEntityType, CommonDbEntityInputDto<TId> input) where TId : struct, IEquatable<TId>
		{

			var validationResult = _commonDbEntityService.Validate(input);

			if (validationResult.Succeeded)
			{
				CreateUpdateResult<TId> result = null;

				if (input.Id.Equals(default(TId)))
				{
					result = await _commonDbEntityService.CreateAsync(dbEntityType, input);
				}
				else
				{
					result = await _commonDbEntityService.UpdateAsync(dbEntityType, input);
				}

				if (result.Succeeded)
				{
					return JsonNotification(result.Message, new { id = result.Id });
				}
			}
			else
			{
				return JsonFormErrors(validationResult.ValidationErrors);
			}

			return null;
		}


		public async Task<JsonResponse> CommonDbEntitySave<TId>(Type dbEntityType, CommonDbEntityCategoryInputDto<TId> input) where TId : struct, IComparable<TId>, IEquatable<TId>
		{

			var validationResult = _commonDbEntityService.Validate(dbEntityType, input);

			if (validationResult.Succeeded)
			{
				CreateUpdateResult<TId> result = null;

				if (input.Id.Equals(default(TId)))
				{
					result = await _commonDbEntityService.CreateAsync(dbEntityType, input);
				}
				else
				{
					result = await _commonDbEntityService.UpdateAsync(dbEntityType, input);
				}

				if (result.Succeeded)
				{
					return JsonNotification(result.Message, new { id = result.Id });
				}

				return null;
			}
			else
			{
				return JsonFormErrors(validationResult.ValidationErrors);
			}

			return null;
		}

	}
}
