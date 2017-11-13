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
    public class EmailTypeController : AdminCommonDbEntityController
    {
		public EmailTypeController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntitySearch<byte>(typeof(EMAIL_TIPOS), input);
		}

		public async Task<IActionResult> PopupEdit(byte id)
		{
			return await CommonDbEntityPopupEdit(typeof(EMAIL_TIPOS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityInputDto<byte> input)
		{
			return await CommonDbEntitySave(typeof(EMAIL_TIPOS), input);
		}
	}
}
