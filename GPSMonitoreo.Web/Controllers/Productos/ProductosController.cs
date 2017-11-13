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
using GPSMonitoreo.Web.PostModels.Productos;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;

namespace GPSMonitoreo.Web.Controllers.Productos
{
    public class ProductosController : BaseController
	{
		public IActionResult Index()
		{
			int fieldindex = 0;

			var grid = new jqxGrid();

			grid.AddColumn("Código interno", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Categoria", "250px", "categoria", "string", fieldindex++.ToString());

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/productos/productos/search";
			model.Title = "PRODUCTOS::PRODUCTOS::BUSQUEDA";
			model.EditButton = true;


			
			ViewData["categorias"] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();

			

			return View(model);
		}

		public IActionResult Edit(int id)
		{
			if (id > 0)
			{
				var model = ProductosEdit.Load(DBContext, id);

				if (model != null)
				{
					return JsonRecord(model);
				}

				return null;
			}
			else
			{



				ViewData["categorias"] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();

				var layoutModel = new ViewModels.TabbedLayoutForm()
				{
					FormId = "form_marcas",
					Title = "PRODUCTOS::PRODUCTOS"
				};


				return View(layoutModel);
			}
		}


		public IActionResult Save(ProductosEdit editModel)
		{
			var errors = ModelState.ToJsonObject();

			////var segmentosCount = editModel.segmentos?.Length ?? 0;

			////if (segmentosCount == 0)
			////{
			////	errors.Add("cercastransito", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Se requieren 2 o más segmentos para armar un tramo" }));
			////}

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

		public IActionResult Menu()
		{
			return View();
		}


		public IActionResult Search(ProductosSearch searchModel)
		{

			var query = DBContext.PRODUCTOS.AsQueryable();
			int dummy1 = 1;
			query = query.Where(item => 1 == dummy1);

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));

			if (searchModel.categoria > 0)
				query = query.Where(item => item.CATEGORIA_ID == searchModel.categoria);


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = query.OrderByDescending(item => item.ID)
				.Select(item => new { id = item.ID })
				.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize)
				.Join(DBContext.PRODUCTOS, item => item.id, item2 => item2.ID, (a, b) => new {
					id = b.ID,
					descripcion = b.DESCRIPCION_LARGA,
					categoria = DBContext.FN_HIERARCHY_PATH(typeof(PRODUCTOS_CATS).Name, b.CATEGORIA_ID)
				})
				.OrderByDescending(item => item.id)
				;

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion, item.categoria };
			});


			return JsonRecords(records, searchModel.recordcount);
		}

		public IActionResult WindowSearch()
		{

			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Código interno", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Categoria", "250px", "categoria", "string", fieldindex++.ToString());

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/productos/productos/search";
			model.Title = "BUSQUEDA DE PRODUCTOS";

			ViewData["categorias"] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();

			return View(model);
		}


	}

}
