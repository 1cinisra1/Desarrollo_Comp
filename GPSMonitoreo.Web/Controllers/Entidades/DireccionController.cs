using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels.Entidades;
using MVCHelpers.ActionResults;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Web.PostModels;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;

namespace GPSMonitoreo.Web.Controllers.Entidades
{
	public class DireccionController : BaseController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entidadId">El id de la entidad con la que tiene relación la dirección</param>
		/// <returns></returns>
		///
		[HttpGet("/entidades/[controller]/[action]/{entidadId}")]
		public IActionResult EditForm(int entidadId)
		{
			//var query = DBContext.ENTIDADES_DIRS.Where(item => item.ID == entidadId).FirstOrDefault();

			ViewData["paises"] = DBContext.PAISES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["provincias"] = DBContext.PROVINCIAS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["ciudades"] = DBContext.CIUDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["entidad_id"] = entidadId;
			ViewData["regiones"] = DBContext.REGIONES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["tipos"] = DBContext.ENTIDADES_DIRS_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			//ViewData["telefono_tipos"] = DBContext.TELEFONO_TIPOS.ToJqwidgets().ToJsonString();


			var layoutModel = new ViewModels.AppLayoutForm()
			{
				FormId = "form_entidades_direccion_edit",
				Title = "ENTIDADES::ENTIDAD::DIRECCIONES EDIT"
			};

			layoutModel.PrepareEditUrls(HttpContext, true);
			
			return View(layoutModel);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">El id de la dirección para la cual se quieren obtener los valores</param>
		/// <returns></returns>
		public IActionResult EditData(int id)
		{
			var model = DireccionEdit.Load(DBContext, id);
			return JsonRecord(model, new { cerca_label = DBContext.CERCAS.Find(model.cerca)?.DESCRIPCION_LARGA ?? "" }, new { description = model.descripcion_larga });
		}

		public IActionResult Save(DireccionEdit editModel)
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

		/// <summary>
		/// Recibe el id de la direccion a editar
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Telefonos(int id)
		{

			var query = DBContext.ENTIDADES_TELFS.OrderBy(item => item.ENTIDAD.APELLIDOS).Where(item => item.DIRECCION_ID == id && item.UBICACION_ID == null);

			var telefonos = query.Select(item => new {
				id = item.ID,
				tipo = item.TIPO.DESCRIPCION_LARGA,
				codigo_pais = item.CODIGO_PAIS,
				codigo_area = item.CODIGO_AREA,
				telefono = item.TELEFONO
			}).ToList();


			return JsonRecords(telefonos);

		}

		//public IActionResult TelefonoEditForm(int id)
		//{
		//	ViewData["direccion_id"] = id;
		//	ViewData["tipos"] = DBContext.TELEFONO_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
		//	return View();
		//}

		//public IActionResult TelefonoEditData(int id)
		//{
		//	var model = TelefonoEdit.Load(DBContext, id);
		//	return JsonRecord(model);
		//}


		//public IActionResult TelefonoSave(TelefonoEdit editModel)
		//{
		//	var errors = ModelState.ToJsonObject();

		//	if (errors.Count > 0)
		//	{
		//		return JsonFormErrors(errors);
		//	}
		//	else
		//	{
		//		var id = editModel.Save(DBContext);
		//		return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.telefono), new { id = id });
		//	}
		//}


		public IActionResult EntidadDirecciones(int id)
		{
			var query = DBContext.ENTIDADES_DIRS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.ENTIDAD_ID == id);

			var direcciones = query.Select(item => new
			{
				id = item.ID,
				descripcion = item.DESCRIPCION_LARGA,
				ciudadela = item.CIUDADELA,
				calle_principal = item.CALLE_PRINCIPAL,
				calle_transversal = item.CALLE_TRANSVERSAL,
				numeracion = item.NUMERACION,
				codigo_postal = item.CODIGOPOSTAL,
				pais = item.PAIS.DESCRIPCION_LARGA,
				provincia = item.PROVINCIA.DESCRIPCION_LARGA,
				ciudad = item.CIUDAD.DESCRIPCION_LARGA
			}).ToList();

			return JsonRecords(direcciones);
		}



		[HttpGet("/entidades/[controller]/[action]/{entidadId}/{direccionId?}")]
		public IActionResult Contactos(int entidadId, int direccionId)
		{
			//var query = DBContext.ENTIDADES_CTOS.OrderBy(item => item.PERSONA.APELLIDOS).Where(item => item.EMPRESA_ID == entidadId)
			//	.OrderBy(item => item.EMPRESA.DESCRIPCION_LARGA)
			//	.Select(item => new {
			//		id = item.ID,
			//		persona_id = item.PERSONA_ID,
			//		persona = item.PERSONA.DESCRIPCION_LARGA,
			//		cargo = item.CARGO.DESCRIPCION_LARGA,
			//		asignado = item.DIRECCIONES_REL.Any(item2 => item2.CONTACTO_ID == item.ID && item2.DIRECCION_ID == direccionId)
			//	});

			var query = DBContext.ENTIDADES_DIRS_CTOS_REL.Where(item => item.DIRECCION_ID == direccionId)
				.OrderBy(item => item.CONTACTO.PERSONA.DESCRIPCION_LARGA)
				.Select(item => new
				{
					id = item.CONTACTO.ID,
					persona_id = item.CONTACTO.PERSONA_ID,
					persona = item.CONTACTO.PERSONA.DESCRIPCION_LARGA,
					cargo = item.CONTACTO.CARGO.DESCRIPCION_LARGA
				});

			return JsonRecords(query.ToList());
		}

		[HttpGet("/entidades/[controller]/[action]/{entidadId}/{direccionId}/{contactoId}")]
		public IActionResult ContactosAdd(int entidadId, int direccionId, int contactoId)
		{

			var entidadEntity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == entidadId);

			if (entidadEntity == null)
				return JsonError("Entidad no existe");

			var direccionEntity = DBContext.ENTIDADES_DIRS.FirstOrDefault(item => item.ENTIDAD_ID == entidadId && item.ID == direccionId);

			if (direccionEntity == null)
				return JsonError("Dirección no existe o no pertence a la entidad");


			var contactoEntity = DBContext.ENTIDADES_CTOS.FirstOrDefault(item => item.EMPRESA_ID == entidadId && item.ID == contactoId);

			if(contactoEntity == null)
				return JsonError("Contacto no existe o no está relacionado a la empresa");

			if(DBContext.ENTIDADES_DIRS_CTOS_REL.Any(item => item.DIRECCION_ID == direccionId && item.CONTACTO_ID == contactoId))
				return JsonError("Contacto ya está relacionado a la dirección");



			DBContext.ENTIDADES_DIRS_CTOS_REL.Add(new ENTIDADES_DIRS_CTOS_REL() { DIRECCION_ID = direccionId, CONTACTO_ID = contactoId });
			DBContext.SaveChanges();

			return JsonNotification("Contacto agregado con exito<br/>" + entidadEntity.DESCRIPCION_LARGA + "->" + direccionEntity.DESCRIPCION_LARGA + "->" + contactoEntity.PERSONA.DESCRIPCION_LARGA);

		}


		public IActionResult Ubicaciones(int id)
		{
			var query = DBContext.ENTIDADES_DIRS_UBICAS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.DIRECCION_ID == id && item.UBICACION_ID == null)
				.OrderBy(item => item.DIRECCION.DESCRIPCION_LARGA)
				.Select(item => new {
					id = item.ID,
					descripcion = item.DESCRIPCION_LARGA,
					//tipo = item.TIPOS.Aggregate((a, b) => a.TIPO.DESCRIPCION_LARGA + " / " + b.TIPO.DESCRIPCION_LARGA)
					tipos = item.TIPOS.Select(item2 => item2.TIPO.DESCRIPCION_LARGA)
					//tipos = DBContext.ENTIDADES_DIRS_UBICAS_TIPOSREL.Where(item2 => item2.UBICACION_ID == item.ID).Select(item2 => item2.TIPO.DESCRIPCION_LARGA)
				});


			var records = query.ToList().Select(item => new { item.id, item.descripcion, tipos = string.Join(", ", item.tipos) });

			return JsonRecords(records);
		}

		public IActionResult QuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE DIRECCION",
				Details = ViewModels.Entidades.DireccionDetails.Load(DBContext, id, true),
				ElementsIdPrefix = $"quickview_direccion{id}"

			};

			return View(quickViewModel);
		}
	}
}
