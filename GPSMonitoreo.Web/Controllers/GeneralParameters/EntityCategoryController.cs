using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers.GeneralParameters
{
    public class EntityCategoryController : AdminCommonDbEntityController
	{
		public EntityCategoryController(CommonDbEntityService commonDbEntityService) : base(commonDbEntityService)
		{

		}


		/// <summary>
		/// Método que devuelve la vista genérica de Categorias. Aplicable para entidades generadas del entity framework que mantengan la estructura estándar
		/// </summary>
		/// <returns>Microsoft.AspNetCore.Mvc.ViewResult</returns>
		/// 
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Método que devuelve una categoría de entidad. Recibe un PostModels.GeneralSearch y devuelve un ENTIDADES_CATS
		/// </summary>

		public async Task<JsonResponse> Search(CommonDbEntityFilterInputDto input)
		{
			return await CommonDbEntityCategorySearch<short>(typeof(ENTIDADES_CATS), input);
			//return await CommonDbEntitySearch<byte>(typeof(Data.Models.CERCAS_TRAFICO), input);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">El id a editar. En caso de ser 0 define un ViewData estándar</param>
		/// <returns>La data a editar o la vista de edición</returns>

		public async Task<IActionResult> PopupEdit(short id)
		{
			ViewData["cats"] = DBContext.CategoriesTree<ENTIDADES_CATS>().ToJqwidgetsTree();

			return await CommonDbEntityCategoryPopupEdit(typeof(ENTIDADES_CATS), id);
		}

		public async Task<JsonResponse> Save(CommonDbEntityCategoryInputDto<short> input)
		{
			return await CommonDbEntitySave(typeof(ENTIDADES_CATS), input);
		}



	}
}
