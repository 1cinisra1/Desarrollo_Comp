using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Equipments;
using GPSMonitoreo.Dtos.Base.Models;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.Equipments;
using GPSMonitoreo.Services.Base.Models;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.Equipments
{
    public class EquipmentController : AdminBaseController
    {
		private CommonDbEntityService _commonDbEntityService;
		private EquipmentService _equipmentService;
		private ModelService _modelService;

		public EquipmentController(
			CommonDbEntityService commonDbEntityService,
			EquipmentService equipmentService, 
			ModelService modelService
		)
		{
			_equipmentService = equipmentService;
			_commonDbEntityService = commonDbEntityService;
			_modelService = modelService;
		}


		public IActionResult Index()
		{
			ViewData["categories"] = DBContext.CategoriesTree<EQUIPOS_CATS>().ToJqwidgetsTree();
			return View();
		}

		public async Task<JsonResponse> Search(EquipmentFilterInputDto input)
		{
            Utils.dump(input);

            Console.WriteLine("----------------------------------------------------------------------");

			var result = await _equipmentService.GetListAsync(input);

			return GetListResultJsonResponse(result);
		}

		public async Task<IActionResult> EditForm()
		{
            //forma antigua
			ViewData["categories"] = DBContext.CategoriesTree<EQUIPOS_CATS>().ToJqwidgetsTree();
			ViewData["productCategories"] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();
			ViewData["equipmentCapabilities"] = DBContext.CategoriesTree<EQUIPOS_CAPS>().ToJqwidgetsTree();
			ViewData["arreglo_unidades"] = DBContext.UNIDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToDictionary(a => a.ID, b => b.DESCRIPCION_LARGA).ToList().ToJsonString();
			ViewData["arreglo_caps_unidades"] = DBContext.EQUIPOS_CAPS.OrderBy(item => item.DESCRIPCION_LARGA).ToDictionary(a => a.ID, b => b.UNIDAD_ID).ToList().ToJsonString();
            //forma antigua - end


            ViewData["groups"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(EQUIPOS_GRUPOS), null)).Items;
			ViewData["brands"] = (await _commonDbEntityService.GetSimpleListAsync<short>(typeof(MARCAS), null)).Items;
			ViewData["operationStatus"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(EQUIPOS_ESTADOS_OPERA), null)).Items;

			return View();
		}



		public async Task<JsonResponse> EditData(int id)
		{
			var result = await _equipmentService.GetByIdAsync(id);

			return GetEditDataResultJsonResponse(result);
		}



		public async Task<JsonResponse> Models(short id)
		{
			var result = await _modelService.GetSimpleListAsync(new ModelFilterInputDto { Paging = false, BrandId = id, OrderBy = FilterInputOrderBy.DefaultOrderByDescriptionAscending });

			return GetListResultJsonResponse(result);
		}

		public async Task<JsonResponse> Save(EquipmentInputDto input)
		{
			var operation = new CreateUpdateOperation<int, EquipmentInputDto>
			{
				InputDto = input,
				Validate = _equipmentService.Validate,
				CreateAsync = _equipmentService.CreateAsync,
				UpdateAsync = _equipmentService.UpdateAsync
			};

			return await CommonSave(operation);
		}


	}
}
