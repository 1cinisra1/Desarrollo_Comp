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
    public class AlarmasController : GeneralBaseController
    {

		
		//ALARMAS UNIDADES
		public IActionResult Unidades()
		{
			return SearchView("ADMINISTRACION::ALARMAS::UNIDADES");
		}

		public IActionResult UnidadesSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.ALARMAS_UNIDADES);
		}

		public IActionResult UnidadesEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::ALARMAS UNIDADES") : EditData<ALARMAS_UNIDADES>(id);
		}

		public IActionResult UnidadesSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<ALARMAS_UNIDADES>(postModel);
		}


		//ALARMAS PERMANS
		public IActionResult Permans()
		{
			return SearchView("ADMINISTRACION::ALARMAS::PERMANS");
		}

		public IActionResult PermansSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.ALARMAS_PERMANS);
		}

		public IActionResult PermansEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::ALARMAS PERMANS") : EditData<ALARMAS_PERMANS>(id);
		}

		public IActionResult PermansSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<ALARMAS_PERMANS>(postModel);
		}


		//ALARMAS PERMANS RESET
		public IActionResult PermansResets()
		{
			return SearchView("ADMINISTRACION::ALARMAS::PERMANS RESETS");
		}

		public IActionResult PermansResetsSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.ALARMAS_PERMANS_RESETS);
		}

		public IActionResult PermansResetsEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::ALARMAS PERMANS RESETS") : EditData<ALARMAS_PERMANS_RESETS>(id);
		}

		public IActionResult PermansResetsSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<ALARMAS_PERMANS_RESETS>(postModel);
		}


		//ALARMAS NIVELES
		public IActionResult Niveles()
		{
			return SearchView("ADMINISTRACION::ALARMAS::NIVELES");
		}

		public IActionResult NivelesSearch(PostModels.GeneralSearch searchModel)
		{
			return Search(searchModel, DBContext.ALARMAS_NIVELES);
		}

		public IActionResult NivelesEdit(byte id)
		{
			return id == 0 ? EditView("GENERAL::ALARMAS NIVELES") : EditData<ALARMAS_NIVELES>(id);
		}

		public IActionResult NivelesSave(PostModels.PostModelEdit<byte> postModel)
		{
			return Save<ALARMAS_NIVELES>(postModel);
		}


		public IActionResult Test()
		{

			return View("~/Views/Test.cshtml");
		}


	}
}
