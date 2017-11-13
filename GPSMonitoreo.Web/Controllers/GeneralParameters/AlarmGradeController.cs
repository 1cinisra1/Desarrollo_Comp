using GPSMonitoreo.Dtos.Base.AlarmGrades;
using GPSMonitoreo.Services.Base.AlarmGrades;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{

    public class AlarmGradeController : AdminBaseController
    {
		private AlarmGradeService _alarmGradeService;

		public AlarmGradeController(AlarmGradeService alarmGradeService)
		{
			
			_alarmGradeService = alarmGradeService;
		}

		public IActionResult Index(int id)
		{
			return View();
		}

		public async Task<IActionResult> PopupEdit(byte id)
		{
            if(id > 0)
            {
                ViewData["data"] = (await _alarmGradeService.GetByIdAsync(id)).Item;
            }
			
			return View();
		}

		public async Task<JsonResponse> Save(AlarmGradeInputDto input)
		{
			Utils.dump(input);
			var operation = new CreateUpdateOperation<byte, AlarmGradeInputDto>
			{
				InputDto = input,
				Validate = _alarmGradeService.Validate,
				CreateAsync = _alarmGradeService.CreateAsync,
				UpdateAsync = _alarmGradeService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public async Task<JsonResponse> Search(AlarmGradeFilterInputDto input)
		{
			var result = await _alarmGradeService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
		}

	}
}
