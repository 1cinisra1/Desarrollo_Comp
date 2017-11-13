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
using GPSMonitoreo.Web.PostModels.General.ControlVelocidad;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;
using MVCHelpers.ActionResults;

namespace GPSMonitoreo.Web.Controllers.General
{
	public class ControlVelocidadController : BaseController
	{

		private const string _ID_LISTA_ENTIDADES = "lista_entidades";
		private const string _ID_LISTA_CATEGORIA_PRODUCTOS = "lista_categoria_productos";
		private const string _ID_LISTA_PRODUCTOS = "lista_productos";

		public IActionResult Index()
		{
			int fieldindex = 0;

			var grid = new jqxGrid();

			grid.AddColumn("Código", "50px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "250px", "descripcion_larga", "string", fieldindex++.ToString());
			grid.AddColumn("Estado", "100px", "estado_id", "string", fieldindex++.ToString());


			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/general/controlvelocidad/search";
			model.Title = "GENERAL::MATRICES CONTROL VELOCIDAD::BUSQUEDA";
			model.EditButton = true;
			model.QuickViewButton = true;

			ViewData[_ID_LISTA_ENTIDADES] = DBContext.ENTIDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData[_ID_LISTA_CATEGORIA_PRODUCTOS] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();
			ViewData[_ID_LISTA_PRODUCTOS] = DBContext.PRODUCTOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			

			return View(model);
		}

		public IActionResult QuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE CONTROL VELOCIDAD",
				Details = ViewModels.Equipos.EquipoDetails.Load(DBContext, id)
			};

			return View(quickViewModel);
		}


		public IActionResult Edit(int id)
		{
			if (id > 0)
			{
				var model = ControlVelocidadEdit.Load(DBContext, id);

				if (model != null)
				{
					return JsonRecord(model);
				}

				return null;
			}
			else
			{

				ViewData[_ID_LISTA_ENTIDADES] = DBContext.ENTIDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
				ViewData[_ID_LISTA_CATEGORIA_PRODUCTOS] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();
				ViewData[_ID_LISTA_PRODUCTOS] = DBContext.PRODUCTOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();


				var layoutModel = new ViewModels.TabbedLayoutForm()
				{
					FormId = "form_control_velocidad",
					Title = "GENERAL::MATRICES CONTROL VELOCIDAD"
				};


				return View(layoutModel);

			}
		}



		public IActionResult Save(ControlVelocidadEdit editModel)
		{
			var errors = ModelState.ToJsonObject();	

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var reglasCount = editModel.reglas?.Length ?? 0;

				Console.WriteLine("reglas de evaluacion de velocidades count: " + reglasCount);

				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.descripcion_larga), new { id = id });
			}
		}


		public IActionResult Search(ControlVelocidadSearch searchModel)
		{

			var query = DBContext.EQUIPOS.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));

			if (searchModel.categoria > 0)
				query = query.Where(item => item.CATEGORIA_ID == searchModel.categoria);


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID
								   ,
								   alterno = item.ALTERNO_ID
								   ,
								   descripcion_larga = item.DESCRIPCION_LARGA
								   ,
								   estado = item.ESTADO_ID
								   ,
								   estado_opera = item.ESTADO_OPERACION.DESCRIPCION_LARGA
								   ,
								   grupo = item.GRUPO.DESCRIPCION_LARGA
								   ,
								   categoria = item.CATEGORIA.DESCRIPCION_LARGA
								   ,
								   placa = item.PLACA
								   ,
								   marca = item.MARCA.DESCRIPCION_LARGA
								   ,
								   modelo = item.MODELO.DESCRIPCION_LARGA
								   ,
								   modelo_ano = item.MODELO_ANO
								   ,
								   serial = item.SERIAL
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{

				return new object[]
				{
						item.id
						, item.alterno
						, item.descripcion_larga
						, item.estado
						, item.estado_opera
						, item.grupo
						, item.categoria
						, item.placa
						, item.marca
						, item.modelo
						, item.modelo_ano
						, item.serial
				};

			});


			return JsonRecords(records, searchModel.recordcount);
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
			grid.AddColumn("Estado", "100px", "estado_id", "string", fieldindex++.ToString());


			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/general/controlvelocidad/search";
			model.Title = "BUSQUEDA DE MATRICES CONTROL VELOCIDAD::BUSQUEDA";

			ViewData[_ID_LISTA_ENTIDADES] = DBContext.ENTIDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData[_ID_LISTA_CATEGORIA_PRODUCTOS] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();
			ViewData[_ID_LISTA_PRODUCTOS] = DBContext.PRODUCTOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();


			return View(model);

		}





	}


}
