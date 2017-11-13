using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;


namespace GPSMonitoreo.Web.Controllers.General
{
    public class OtrosController: GeneralBaseController
    {

		#region Métodos root getters


		public IActionResult Unidadestree()
		{

			return TreeView(
				"ADMINISTRACION::OTROS::JERARQUIA UNIDADES", 
				DBContext.CategoriesTree<UNIDADES>(null, true).ToJqwidgetsTree(false),
				"App.general.otros.unidadesedit"
			);
		}

		#endregion
	}
}
