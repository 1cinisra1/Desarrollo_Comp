using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.EquipmentCapabilities;
using GPSMonitoreo.Services.Base.EquipmentCapabilities;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class EquipmentCapabilityController : AdminBaseController
    {
		private EquipmentCapabilityService _equipmentCapabilityService;

		public EquipmentCapabilityController(EquipmentCapabilityService equipmentCapabilityService)
		{
			_equipmentCapabilityService = equipmentCapabilityService;
		}


		public IActionResult Index(int id)
		{
			return View();
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["cats"] = DBContext.CategoriesTree<EQUIPOS_CAPS>().ToJqwidgetsTree();

			ViewData["measureUnits"] = DBContext.CategoriesTree<UNIDADES>().ToJqwidgetsTree();

			if (id > 0)
			{
				ViewData["data"] = (await _equipmentCapabilityService.GetByIdAsync(id)).Item;
			}

			return View();
		}

		public async Task<JsonResponse> Save(EquipmentCapabilityInputDto input)
		{
			var operation = new CreateUpdateOperation<short, EquipmentCapabilityInputDto>
			{
				InputDto = input,
				Validate = _equipmentCapabilityService.Validate,
				CreateAsync = _equipmentCapabilityService.CreateAsync,
				UpdateAsync = _equipmentCapabilityService.UpdateAsync
			};

			return await CommonSave(operation);
		}

		public async Task<JsonResponse> Search(EquipmentCapabilityFilterInputDto input)
		{
			var result = await _equipmentCapabilityService.GetListAsync(input);

			return JsonRecords(result.Items, result.RecordCount);
		}
	}
}
