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
using GPSMonitoreo.Web.Controllers.General;

namespace GPSMonitoreo.Web.Controllers.General
{
    public class ViajesController : GeneralBaseController
    {

		//VIAJES ESTADO
		public IActionResult Estado()
		{
			return SearchView("ADMINISTRACION::VIAJES::ESTADO");
		}

		public IActionResult EstadoSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.VIAJES_ESTADO);
		}

		public IActionResult EstadoEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::VIAJES ESTADO") : EditData<VIAJES_ESTADO>(id);
		}

		public IActionResult EstadoSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<VIAJES_ESTADO>(postModel);
		}

		//VIAJES FASES
		public IActionResult Geografico()
		{
			return SearchView("ADMINISTRACION::VIAJES::GEOGRAFICO");
		}

		//public IActionResult GeograficoSearch(PostModels.GeneralSearch searchModel)
		//{
		//	return Search(searchModel, DBContext.VIAJES_FASES);
		//}

		//public IActionResult GeograficoEdit(byte id)
		//{
		//	return id == 0 ? EditView("GENERAL::VIAJES GEOGRAFICO") : EditData<VIAJES_FASES>(id);
		//}

		//public IActionResult GeograficoSave(PostModels.PostModelEdit<byte> postModel)
		//{
		//	return Save<VIAJES_FASES>(postModel);
		//}

		//OPERACIONES RUTAS
		public IActionResult Operacionruta()
		{
			return SearchView("ADMINISTRACION::FASES::OPERACIONES RUTAS");
		}

		public IActionResult OperacionrutaSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.RUTAS_OPERAS);
		}

		public IActionResult OperacionrutaEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::FASES OPERACIONES RUTAS") : EditData<RUTAS_OPERAS>(id);
		}

		public IActionResult OperacionrutaSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<RUTAS_OPERAS>(postModel);
		}



		public IActionResult Test()
		{

			return View("~/Views/Test.cshtml");
		}


	}
}
