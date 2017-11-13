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
    public class EntityPositionController : AdminCommonDbEntityController
    {
		public EntityPositionController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntitySearch<short>(typeof(ENTIDADES_CARGOS), input);
		}

		public async Task<IActionResult> PopupEdit(short id)
		{
			return await CommonDbEntityPopupEdit(typeof(ENTIDADES_CARGOS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityInputDto<short> input)
		{
			return await CommonDbEntitySave(typeof(ENTIDADES_CARGOS), input);
		}
	}
}
