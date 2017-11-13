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
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.PostModels.Localidades;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;


namespace GPSMonitoreo.Web.Controllers.Localidades
{
    public class CiudadesController: BaseController
    {
		
		public IActionResult Index()
		{
			int fieldindex = 0;

			var grid = new jqxGrid();

			grid.AddColumn("Código interno", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());			

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/localidades/ciudades/search";
			model.Title = "LOCALIDADES::CIUDADES::BUSQUEDA";
			model.EditButton = true;

			ViewData["paises"] = DBContext.PAISES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
			ViewData["provincias"] = DBContext.PROVINCIAS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();

			return View(model);
		}
	
		public IActionResult Edit(int id)
		{
			Console.WriteLine("id: " + id);
			if (id > 0)
			{
				var model = CiudadesEdit.Load(DBContext, id);

				if (model != null)
				{
					return JsonRecord(model);
				}

				return null;
			}
			else
			{
				ViewData["paises"] = DBContext.PAISES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();

				var layoutModel = new ViewModels.TabbedLayoutForm()
				{
					FormId = "form_ciudades",
					Title = "LOCALIDADES::CIUDADES"
				};


				return View(layoutModel);
			}
		}

		public IActionResult Save(CiudadesEdit editModel)
		{
			var errors = ModelState.ToJsonObject();


			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.descripcion_larga), new { id = id });
			}
		}


		public IActionResult Search(GPSMonitoreo.Web.PostModels.Localidades.CiudadesSearch searchModel)
		{

			var query = DBContext.CIUDADES.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(searchModel.descripcion.ToLower()));

			if (searchModel.paises > 0)
				query = query.Where(item => item.PAISES.ID == searchModel.paises);

			if (searchModel.provincias > 0)
				query = query.Where(item => item.PROVINCIAS.ID == searchModel.provincias);

			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
								   pais_id = item.PAIS_ID,
								   provincia_id = item.PROVINCIA_ID								   
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion, item.pais_id, item.provincia_id };
			});


			return JsonRecords(records, searchModel.recordcount);
		}
	}
}
