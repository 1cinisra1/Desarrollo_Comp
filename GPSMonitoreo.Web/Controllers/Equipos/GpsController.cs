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

namespace GPSMonitoreo.Web.Controllers.Equipos
{
	public class GpsController : BaseController
	{

		private const string _ID_LISTA_EQUIPOS = "lista_equipo";
		private const string _ID_LISTA_GPS_MARCAS = "lista_gps_marca";
		private const string _ID_LISTA_GPS_MODELOS = "lista_gps_modelo";

		public IActionResult Index()
		{
			int fieldindex = 0;


			var grid = new jqxGrid();

			grid.AddColumn("Id", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción Larga", "250px", "descripcion_larga", "string", fieldindex++.ToString());
			grid.AddColumn("Estado", "100px", "estado_id", "string", fieldindex++.ToString());
			grid.AddColumn("IMEI", "100px", "imei", "string", fieldindex++.ToString());
			grid.AddColumn("Marca", "100px", "marca", "string", fieldindex++.ToString());
			grid.AddColumn("Modelo", "100px", "modelo", "string", fieldindex++.ToString());
			grid.AddColumn("Equipo", "100px", "equipo", "string", fieldindex++.ToString());



			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/equipos/gps/search";
			model.Title = "EQUIPOS::GPS::BUSQUEDA";
			model.EditButton = true;

			////ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.GPS_MARCAS.ToJqwidgets().ToJsonString();
			////ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.GPS_MODELOS.ToJqwidgets().ToJsonString();

			ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.MARCAS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.MODELOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();

			ViewData[_ID_LISTA_EQUIPOS] = DBContext.EQUIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			

			return View(model);
		}




		public IActionResult Edit(int id)
		{
			if (id > 0)
			{
				var model = GpsEdit.Load(DBContext, id);

				if (model != null)
				{
					return JsonRecord(model);
				}

				return null;
			}
			else
			{

				////ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.GPS_MARCAS.ToJqwidgets().ToJsonString();
				////ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.GPS_MODELOS.ToJqwidgets().ToJsonString();
				ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.MARCAS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
				ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.MODELOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();

				ViewData[_ID_LISTA_EQUIPOS] = DBContext.EQUIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().AddBlankItem().ToJsonString();

				var layoutModel = new ViewModels.TabbedLayoutForm()
				{
					FormId = "form_gps",
					Title = "EQUIPOS::GPS"
				};


				return View(layoutModel);

			}
		}



		public IActionResult Save(GpsEdit editModel)
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


		public IActionResult Menu()
		{
			return View();
		}


		public IActionResult Search(GpsSearch searchModel)
		{

			var query = DBContext.GPS.AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));

			if (searchModel.marca > 0)
				query = query.Where(item => item.MARCA_ID == searchModel.marca);

			if (searchModel.modelo > 0)
				query = query.Where(item => item.MODELO_ID == searchModel.modelo);

			if (searchModel.equipo > 0)
				query = query.Where(item => item.EQUIPO_ID == searchModel.equipo);

			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID
								   , descripcion_larga = item.DESCRIPCION_LARGA
								   , estado = item.ESTADO_ID
								   , imei = item.IMEI
								   , marca = item.MARCA.DESCRIPCION_LARGA
								   , modelo = item.MODELO.DESCRIPCION_LARGA
								   , equipo = item.EQUIPO.DESCRIPCION_LARGA
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{

				return new object[]
				{
						item.id
						, item.descripcion_larga
						, item.estado
						, item.imei
						, item.marca
						, item.modelo
						, item.equipo
				};

			});


			return JsonRecords(records, searchModel.recordcount);
		}

		public IActionResult WindowSearch()
		{
			

			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Id", "150px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción Larga", "250px", "descripcion_larga", "string", fieldindex++.ToString());
			grid.AddColumn("Estado", "100px", "estado_id", "string", fieldindex++.ToString());
			grid.AddColumn("IMEI", "100px", "imei", "string", fieldindex++.ToString());
			grid.AddColumn("Marca", "100px", "marca", "string", fieldindex++.ToString());
			grid.AddColumn("Modelo", "100px", "modelo", "string", fieldindex++.ToString());
			grid.AddColumn("Equipo", "100px", "equipo", "string", fieldindex++.ToString());


			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/equipos/gps/search";
			model.Title = "BUSQUEDA DE GPS";

			////ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.GPS_MARCAS.ToJqwidgets().ToJsonString();
			////ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.GPS_MODELOS.ToJqwidgets().ToJsonString();

			ViewData[_ID_LISTA_GPS_MARCAS] = DBContext.MARCAS.ToJqwidgets().ToJsonString();
			ViewData[_ID_LISTA_GPS_MODELOS] = DBContext.MODELOS.ToJqwidgets().ToJsonString();

			ViewData[_ID_LISTA_EQUIPOS] = DBContext.EQUIPOS.ToJqwidgets().ToJsonString();


			return View(model);

		}





	}
}
