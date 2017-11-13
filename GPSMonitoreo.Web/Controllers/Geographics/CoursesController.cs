using Microsoft.AspNetCore.Mvc;
using MVCHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Services.Base.Geographics.Courses;
using GPSMonitoreo.Services;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Dtos.Base.Geographics.Courses;
using GPSMonitoreo.Web.Infrastructure;

namespace GPSMonitoreo.Web.Controllers.Geographics
{
    public class CoursesController : AdminBaseController
	{

		CourseService _courseAppService;

		public CoursesController(CourseService courseAppService)
		{
			_courseAppService = courseAppService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult EditForm()
		{
			return View();
		}

		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _courseAppService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}

		public async Task<JsonResponse> Search(CourseFilterInputDto input)
		{
			var result = await _courseAppService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}


		public async Task<JsonResponse> Save(CourseInputDto input)
		{
			var operation = new CreateUpdateOperation<int, CourseInputDto>
			{
				InputDto = input,
				Validate = _courseAppService.Validate,
				CreateAsync = _courseAppService.CreateAsync,
				UpdateAsync = _courseAppService.UpdateAsync
			};

			return await CommonSave(operation);
		}
	}
}
