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
using GPSMonitoreo.Web.PostModels.Entidades;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Data.Models;

namespace GPSMonitoreo.Web.Controllers.Entidades
{
	public class UbicacionController : BaseController
	{

		//
		//PARA INGRESO DE UNA NUEVA ENTIDAD:


		/// <summary>
		/// 
		/// </summary>
		/// <param name="tiporeg">si es 1, entonces va debajo de una dirección, de lo contrario, debajo de otra ubicación</param>
		/// <param name="id">el id de la dirección o ubicación padre</param>
		/// <returns></returns>


		[HttpGet("/entidades/[controller]/[action]/{tiporeg}/{id}")]
		public IActionResult EditForm(int tiporeg, int id)
		{


			ViewData["tipos"] = DBContext.ENTIDADES_DIRS_UBICAS_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			//ViewData["ubicaciones"] = DBContext.ENTIDADES_DIRS_UBICAS.ToJqwidgets().ToJsonString();


			int entidadId = 0;
			int direccionId = 0;
			int ubicacionId = 0;

			ENTIDADES_DIRS entityDireccion;




			if(tiporeg == 1) // va debajo de una dirección
			{
				direccionId = id;
				entityDireccion = DBContext.ENTIDADES_DIRS.First(item => item.ID == id);
			}
			else //va debajo de una ubicación padre
			{
				ubicacionId = id;
				entityDireccion = DBContext.ENTIDADES_DIRS_UBICAS.First(item => item.ID == id).DIRECCION;
				direccionId = entityDireccion.ID;
			}

			entidadId = entityDireccion.ENTIDAD_ID;

			ViewData["entidad"] = entidadId;
			ViewData["direccion"] = direccionId;
			ViewData["ubicacion"] = ubicacionId;



			var layoutModel = new ViewModels.AppLayoutForm()
			{
				FormId = "form_ubicacion",
				Title = "ENTIDADES::ENTIDAD::UBICACIONES EDIT",
			};

			layoutModel.PrepareEditUrls(HttpContext, 3);

			return View(layoutModel);
		}


		public IActionResult Save(UbicacionEdit editModel)
		{
			Utils.dump(editModel);

			var errors = ModelState.ToJsonObject();



			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.descripcion_larga), new { id = id, description = editModel.descripcion_larga });
			}
		}

		public IActionResult EditData(int id)
		{
			var model = UbicacionEdit.Load(DBContext, id);
			return JsonRecord(model, new { cerca_label = DBContext.CERCAS.Find(model.cerca)?.DESCRIPCION_LARGA ?? "" }, new { description = model.descripcion_larga });
		}



		public IActionResult Telefonos(int id)
		{

			var query = DBContext.ENTIDADES_TELFS.OrderBy(item => item.ENTIDAD.APELLIDOS).Where(item => item.UBICACION_ID == id);

			var telefonos = query.Select(item => new {
				id = item.ID,
				tipo = item.TIPO.DESCRIPCION_LARGA,
				codigo_pais = item.CODIGO_PAIS,
				codigo_area = item.CODIGO_AREA,
				telefono = item.TELEFONO
			}).ToList();
			return JsonRecords(telefonos);
		}

		public IActionResult Ubicaciones(int id)
		{
			var query = DBContext.ENTIDADES_DIRS_UBICAS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.UBICACION_ID == id)
				.OrderBy(item => item.DIRECCION.DESCRIPCION_LARGA)
				.Select(item => new {
					id = item.ID,
					descripcion = item.DESCRIPCION_LARGA,
					tipos = item.TIPOS.Select(item2 => item2.TIPO.DESCRIPCION_LARGA)
				});


			var records = query.ToList().Select(item => new { item.id, item.descripcion, tipos = string.Join(", ", item.tipos) });

			return JsonRecords(records);
		}


		public IActionResult QuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE UBICACION",
				Details = ViewModels.Entidades.UbicacionDetails.Load(DBContext, id, true),
				ElementsIdPrefix = $"quickview_ubicacion{id}"
			};

			return View(quickViewModel);
		}


	}
}
