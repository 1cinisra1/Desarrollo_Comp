using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
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
    public class GeograficoController : GeneralBaseController
    {
		public IActionResult CercasTree()
		{

			return TreeView(
				"ADMINISTRACION::GEOGRAFICO::JERARQUIA CERCAS",
				DBContext.CategoriesTree<CERCAS_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.geografico.cercasedit"
			);
		}

		public IActionResult SegmentosTree()
		{

			return TreeView(
				"ADMINISTRACION::GEOGRAFICO::JERARQUIA SEGMENTOS",
				DBContext.CategoriesTree<SEGMENTOS_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.geografico.segmentosedit"
			);
		}

		public IActionResult TramosTree()
		{

			return TreeView(
				"ADMINISTRACION::GEOGRAFICO::JERARQUIA TRAMOS",
				DBContext.CategoriesTree<TRAMOS_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.geografico.tramosedit"
			);
		}

		public IActionResult RutasTree()
		{

			return TreeView(
				"ADMINISTRACION::GEOGRAFICO::JERARQUIA RUTAS",
				DBContext.CategoriesTree<RUTAS_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.geografico.rutasedit"
			);
		}
	}
}
