

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MVCHelpers.ActionResults;

using GPSMonitoreo.Services.Base.AlarmSpeedLimitSettings;
using GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings;
using GPSMonitoreo.Services.Base.AlarmGrades;
using GPSMonitoreo.Dtos.Base.AlarmGrades;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class AlarmSpeedLimitController : AdminBaseController
	{
		private AlarmSpeedLimitSettingService _alarmSpeedLimitSettingService;

		private AlarmGradeService _alarmGradeService;

		public AlarmSpeedLimitController(
			AlarmSpeedLimitSettingService alarmSpeedLimitSettingService,
			AlarmGradeService alarmGradeService
		)
		{
			_alarmSpeedLimitSettingService = alarmSpeedLimitSettingService;
			_alarmGradeService = alarmGradeService;
		}

		public IActionResult Index(int id)
		{
			return View();
		}

		public async Task<JsonResponse> Search(AlarmSpeedLimitSettingFilterInputDto input)
		{
			var result = await _alarmSpeedLimitSettingService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
		}


		public async Task<IActionResult> EditForm()
		{
			ViewData["alarmGrades"] = (await _alarmGradeService.GetListAsync(new AlarmGradeFilterInputDto { Paging = false, OrderBy = FilterInputOrderBy.DefaultOrderByIdAscending })).Items;

			return View();
		}

		public async Task<JsonResponse> EditData(short id)
		{
			var result = await _alarmSpeedLimitSettingService.GetByIdAsync(id);

			if (result.Succeeded)
			{
				return JsonRecord(result.Item);
			}
			else
			{
				return null;
			}
		}

		public async Task<JsonResponse> Save(AlarmSpeedLimitSettingInputDto input)
		{
			//Utils.dump(input);
			var operation = new CreateUpdateOperation<short, AlarmSpeedLimitSettingInputDto>
			{
				InputDto = input,
				Validate = _alarmSpeedLimitSettingService.Validate,
				CreateAsync = _alarmSpeedLimitSettingService.CreateAsync,
				UpdateAsync = _alarmSpeedLimitSettingService.UpdateAsync
			};

			return await CommonSave(operation);
		}
	}
}
