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
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.PostModels.Localidades;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.Controllers.General;


namespace GPSMonitoreo.Web.Controllers.Localidades

{
    public class PaisesController: BaseController
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
			model.SearchUrl = "/localidades/paises/search";
			model.Title = "LOCALIDADES::PAISES::BUSQUEDA";
			model.EditButton = true;

			return View(model);
		}


		public IActionResult Edit(int id)
		{
			Console.WriteLine("id: " + id);
			if (id > 0)
			{
				var model = PaisesEdit.Load(DBContext, id);

				if (model != null)
				{
					return JsonRecord(model);
				}

				return null;
			}
			else
			{
				var layoutModel = new ViewModels.TabbedLayoutForm()
				{
					FormId = "form_paises",
					Title = "LOCALIDADES::PAISES"
				};

				return View(layoutModel);
			}
		}

		public IActionResult Save(PaisesEdit editModel)
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


		public IActionResult Search(GPSMonitoreo.Web.PostModels.Localidades.PaisesSearch searchModel)
		{

			var query = DBContext.PAISES.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(searchModel.descripcion.ToLower()));

			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA								   
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion};
			});


			return JsonRecords(records, searchModel.recordcount);
		}




		#region Otros Métodos

		/// <summary>
		/// Método invocado desde la vista de ciudades para obtener un listado de provincias según la selección del país realizada por el usuario
		/// </summary>
		/// <param name="codigo">Código del país del cual se desean obtener las provincias</param>
		/// <returns></returns>
		public IActionResult Provincias(int id)
		{

			var provincias = DBContext.PROVINCIAS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.PAIS_ID == id)
							.OrderBy(item => item.DESCRIPCION_LARGA);

			return JsonRecords(provincias.ToJqwidgets());
		}
		#endregion


	}
}
