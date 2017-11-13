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
using GPSMonitoreo.Web.PostModels.Equipos;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;
using MVCHelpers.ActionResults;

namespace GPSMonitoreo.Web.Controllers.Equipos
{
	public class EquipoController : BaseController
	{
		public IActionResult QuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE EQUIPO",
				Details = ViewModels.Equipos.EquipoDetails.Load(DBContext, id)
			};

			return View(quickViewModel);
		}

		public IActionResult WindowSearch()
		{

			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Código interno", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción Larga", "250px", "descripcion_larga", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción Media", "250px", "descripcion_media", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción Corta", "250px", "descripcion_corta", "string", fieldindex++.ToString());
			grid.AddColumn("Abreviación", "250px", "abreviacion", "string", fieldindex++.ToString());
			


			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/equipos/equipos/search";
			model.Title = "BUSQUEDA DE EQUIPOS";

			//ViewData[_ID_LISTA_EQUIPOS_GRUPOS] = DBContext.EQUIPOS_GRUPOS.ToJqwidgets().ToJsonString();
			//ViewData[_ID_LISTA_EQUIPOS_MARCAS] = DBContext.MARCAS.ToJqwidgets().ToJsonString();
			//ViewData[_ID_LISTA_EQUIPOS_MODELOS] = DBContext.MODELOS.ToJqwidgets().ToJsonString();
			//ViewData[_ID_LISTA_EQUIPOS_EOPERA] = DBContext.EQUIPOS_ESTADOS_OPERA.ToJqwidgets().ToJsonString();
			//ViewData[_ID_LISTA_TREE_EQUIPOS_CATEGORIA] = DBContext.CategoriesTree<EQUIPOS_CATS>().ToJqwidgetsTree();


			return View(model);

		}
	}
}
