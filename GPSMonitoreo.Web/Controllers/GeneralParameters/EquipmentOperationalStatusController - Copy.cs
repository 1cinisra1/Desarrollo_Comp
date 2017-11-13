using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class EquipmentOperationalStatusController : AdminCommonDbEntityController
    {
		public EquipmentOperationalStatusController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntitySearch<byte>(typeof(EQUIPOS_ESTADOS_OPERA), input);
		}

		public async Task<IActionResult> PopupEdit(byte id)
		{
			return await CommonDbEntityPopupEdit(typeof(EQUIPOS_ESTADOS_OPERA), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityInputDto<byte> input)
		{
			return await CommonDbEntitySave(typeof(EQUIPOS_ESTADOS_OPERA), input);
		}
	}
}
