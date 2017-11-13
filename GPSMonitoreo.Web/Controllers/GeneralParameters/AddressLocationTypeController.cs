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
    public class AddressLocationTypeController : AdminCommonDbEntityController
    {
		public AddressLocationTypeController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntitySearch<byte>(typeof(ENTIDADES_DIRS_UBICAS_TIPOS), input);
		}

		public async Task<IActionResult> PopupEdit(byte id)
		{
			return await CommonDbEntityPopupEdit(typeof(ENTIDADES_DIRS_UBICAS_TIPOS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityInputDto<byte> input)
		{
			return await CommonDbEntitySave(typeof(ENTIDADES_DIRS_UBICAS_TIPOS), input);
		}
	}
}
