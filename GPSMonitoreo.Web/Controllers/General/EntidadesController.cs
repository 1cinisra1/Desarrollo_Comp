using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels.Geografico;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;

using GPSMonitoreo.Data.Models;
using System.Data.Entity;

namespace GPSMonitoreo.Web.Controllers.General
{
	/// <summary>
	/// Clase controladora para administración de Entidades. Hereda de GeneralBaseController que hereda de BaseController y este a su vez de  Microsoft.AspNetCore.Mvc.Controller
	/// </summary>
    public class EntidadesController: GeneralBaseController
    {
		public IActionResult CategoriasTree()
		{

			return TreeView(
				"ADMINISTRACION::ENTIDADES::JERARQUIA ENTIDADES",
				DBContext.CategoriesTree<ENTIDADES_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.entidades.categoriasedit"
			);
		}
	}
}
