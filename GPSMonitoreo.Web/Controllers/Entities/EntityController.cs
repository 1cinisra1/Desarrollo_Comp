using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Services.Base.Entities;
using GPSMonitoreo.Dtos.Base.Entities;

namespace GPSMonitoreo.Web.Controllers.Entities
{
    public class EntityController : AdminBaseController
    {
		private CommonDbEntityService _commonDbEntityService;

		private EntityService _entityService;

	

		public EntityController(CommonDbEntityService commonDbEntityService, EntityService entityService)
		{
			_commonDbEntityService = commonDbEntityService;
			_entityService = entityService;
		}

		// GET: /<controller>/
		public async Task<IActionResult> Index()
        {
			ViewData["entityTypes"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.ENTIDADES_TIPOS), null)).Items;

			ViewData["identificationTypes"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(GPSMonitoreo.Data.Models.ENTIDADES_IDENT_TIPOS), null)).Items;

			ViewData["categories"] = DBContext.CategoriesTree<Data.Models.ENTIDADES_CATS>().ToJqwidgetsTree();

			return View();
        }

		public IActionResult PersonWindowSearch()
		{
			var model = new ViewModels.AppLayoutSearchGrid()
			{
				Title = "ENTIDADES::PERSONAS",
				FormId = "EntitiesPersonsWindowSearch",
				PostUrl = "/entities/entity/personsearch"
			};

			model.PrepareUrl(this.HttpContext);

			return View(model);
		}

		public async Task<JsonResponse> Search(EntityFilterInputDto input)
		{
			var result = await _entityService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}

		public async Task<JsonResponse> PersonSearch(EntityFilterInputDto input)
		{
			input.EntityTypeId = 4;

			var result = await _entityService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}

		public async Task<JsonResponse> EmailList(int id)
		{
			var result = await _entityService.GetEmailListAsync(new EntityEmailFilterInputDto { Paging = false, EntityId = id });

			return GetListResultJsonResponse(result);
		}
	}
}
