using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels.Entidades;
using MVCHelpers.ActionResults;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;
using System.Data.Entity;
using Newtonsoft.Json.Linq;

namespace GPSMonitoreo.Web.Controllers.Entidades
{
    public class EntidadController : BaseController
	{
		//public IActionResult Index()
		//{
		//	int fieldindex = 0;

		//	var grid = new jqxGrid();

		//	grid.AddColumn("Código interno", "100px", "id", "string", fieldindex++.ToString());
		//	grid.AddColumn("Tipo", "150px", "tipo", "string", fieldindex++.ToString());
		//	grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());
		//	grid.AddColumn("Identificación", "100px","identificacion","string", fieldindex++.ToString());
		//	grid.AddColumn("Razón Social", "250px", "razon_social", "string", fieldindex++.ToString());
		//	grid.AddColumn("Apellidos", "250px", "apellidos", "string", fieldindex++.ToString());
		//	grid.AddColumn("Nombres", "250px", "nombres", "string", fieldindex++.ToString());

			


		//	var model = new ViewModels.SearchGridViewModel();
		//	model.Grid = grid;
		//	model.TargetContainer = "App.$splittedBody.$right";
		//	model.SearchUrl = "/entidades/entidad/search";
		//	model.Title = "ENTIDADES::ENTIDAD::BUSQUEDA";
		//	model.EditButton = true;
		//	model.QuickViewButton = true;

		//	ViewData["tipoidentificacion"] = DBContext.ENTIDADES_IDENT_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
		//	ViewData["tipoentidad"] = DBContext.ENTIDADES_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
		//	ViewData["categoriaentidad"] = DBContext.CategoriesTree<ENTIDADES_CATS>().ToJqwidgetsTree(false);
		//	ViewData["relaciones"] = DBContext.ENTIDADES_RELS.ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
			

		//	return View(model);
		//}

		public IActionResult QuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE ENTIDAD",
				Details = ViewModels.Entidades.EntidadDetails.Load(DBContext, id),
				ElementsIdPrefix = $"quickview_entidad{id}"
			};
			return View(quickViewModel);
		}

		public IActionResult EditForm(int id)
		{
			ViewData["paises"] = DBContext.PAISES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["provincias"] = DBContext.PROVINCIAS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["ciudades"] = DBContext.CIUDADES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();

			ViewData["tipos"] = DBContext.ENTIDADES_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData["saludos"] = DBContext.PERSONA_SALUDOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData["profesiones"] = DBContext.PERSONA_PROFESIONES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();				
			ViewData["categorias"] = DBContext.CategoriesTree<ENTIDADES_CATS>().ToJqwidgetsTree(false);
			//Defininendo la data para el control de tipos de identificación que la vista muestra
			ViewData["tipoiden"] = DBContext.ENTIDADES_IDENT_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData["relaciones"] = DBContext.ENTIDADES_RELS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			ViewData["telefono_tipos"] = DBContext.TELEFONO_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();

			//Componentes del tab de calendarios:
			
			

			
			var query = DBContext.ENTIDADES_DIRS;
			query.Where(item => item.ENTIDAD_ID == 0);
								
			ViewData["direcciones"] = query.Select(item => new
			{
				ciudadela = "",
				calle = "",
				calle_trans = "",
				ciudad = ""
			}).OrderBy(item => item.calle).ToJsonString();

			
			var layoutModel = new ViewModels.AppLayoutForm
			{
				FormId = "form_entidades",
				Title = "ENTIDADES::ENTIDAD"
			};

			layoutModel.PrepareEditUrls(this.HttpContext);

			return View(layoutModel);
		}


		public IActionResult EditData(int id)
		{
			var model = EntidadEdit.Load(DBContext, id);
			//ViewData["horarios_anios"] = DBContext.ENTIDADES_CALENDS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.ENTIDAD_ID == id).ToJqwidgets().ToJsonString();
			if (model != null)
			{

				return JsonRecord(model, new {
					description = model.descripcion_larga,
					calendario_anos = DBContext.ENTIDADES_CALENDS.OrderBy(item => item.DESCRIPCION_LARGA).Where(item => item.ENTIDAD_ID == id).ToJqwidgets(),
					//Todos los días tipo:
					calendario_tipo_dias = DBContext.DIAS_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets()
				}, false);
			}
			return null;
		}

		public IActionResult Save(EntidadEdit editModel)
		{
			Utils.dump(editModel);
			
			var errors = ModelState.ToJsonObject();
			int entityId = 0;
			
			//Si no hubo errores en tipo identificación
			if(errors["tipoiden"] == null)
			{
				switch(editModel.tipo)
				{
					//Si el tipo persona es jurídico o natural, se espera RUC en tipo identificación:
					case 1:// juridico
					case 2: //natural
						if(editModel.tipoiden != 2) // RUC
						{
							errors.Add("tipoiden", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Se espera RUC" }));
						}
						else
						{
							if (editModel.identificacion.Length != 13)
							{
								if (errors["identificacion"] == null)
									errors.Add("identificacion", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "RUC debe tener 13 caracteres" }));								
							}
						}
						break;
					case 4: //persona
					case 5: //empleado
						//if (editModel.tipoiden != 1 && editModel.tipoiden != 3)//solo cédula o pasaporte
						if (editModel.tipoiden == 2)//solo cédula o pasaporte
						{
							errors.Add("tipoiden", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Se espera cédula o pasaporte" }));
						}
						else
						{
							//Si es empleado
							if (editModel.tipoiden == 1)
							{
								if (editModel.identificacion.Length != 10)
								{
									if (errors["identificacion"] == null)
										errors.Add("identificacion", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Cédula debe tener 10 caracteres" }));
								}
							}
						}
						break;
				}
			}



			////Validación de identificaciones repetidas en nuevas entidades. El ToLower es por temas de pasaportes.
			////Se agrega la función AsNoTracking() debido a que se cae el save cuando se editan entidades existentes...
			////Esto se debe exclusivamente a que se está cargando la entidad para realizar las validaciones respectivas de indentificación existente
			////antes de actualizar el estado del objeto... esto causa conflictos en la entidad y cambiaba el estado a Detached. 
			//var results = (from b in DBContext.ENTIDADES.AsNoTracking()
			//			   where b.IDENTIFICACION.ToLower().Equals(editModel.identificacion.ToLower())
			//			   select b);

			//if (results.Count() > 0)
			//{
			//	entityId = results.FirstOrDefault().ID;
			//}

			////Si es una edición de una entidad ya existente:
			//if (editModel.id > 0)
			//{
			//	//Si existe la identificación que se está editando... debe ser el único caso... está de más la validación?
			//	//Misma identificación para la misma persona: ok
			//	if (entityId == editModel.id){}
			//	//Si la identificación ya existe y el id de la entidad es distinto a la entidad que se está editando (se intenta grabar una cédula repetida)
			//	else
			//	{
			//		errors.Add("identificacion", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Identificación de la entidad en edición ya existe" }));
			//	}
			//}
			////Si se está creando una nueva entidad y ya existe la cédula:
			//else
			//{
			//	if(entityId > 0) {
			//			errors.Add("identificacion", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Identificación ya existe" }));
			//	}
			//}

			if (DBContext.ENTIDADES.Any(item => item.IDENTIFICACION.ToLower() == editModel.identificacion.ToLower() && item.ID != editModel.id))
			{
				errors.Add("identificacion", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Identificación ya existe" }));
			}



			if (editModel.tipo == 4)
			{

			}


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

		public IActionResult Search(GPSMonitoreo.Web.PostModels.Entidades.EntidadSearch searchModel)
		{

			//NO SE VALIDA "NEGLIGENCIA" DE USUARIO EN FILTROS DE BÚSQUEDA			
			var query = DBContext.ENTIDADES.AsQueryable();			

			//Indistintamente si es o no pasaporte... se hace el tolower a pesar que en numéricos (cedula, etc) no aplica.
			//Se lo hace así para no validar el tipoidentificación
			if (!string.IsNullOrWhiteSpace(searchModel.identificacion))
				query = query.Where(item => item.IDENTIFICACION.ToLower().Contains(searchModel.identificacion.ToLower()));

			if (searchModel.tipoidentificacion > 0)
				query = query.Where(item => item.IDENT_TIPO_ID == searchModel.tipoidentificacion);

			if (!string.IsNullOrWhiteSpace(searchModel.nombres))
				query = query.Where(item => item.NOMBRES == searchModel.nombres);

			if (!string.IsNullOrWhiteSpace(searchModel.apellidos))
				query = query.Where(item => item.APELLIDOS == searchModel.apellidos);

			if (!string.IsNullOrWhiteSpace(searchModel.razon_social))
				query = query.Where(item => item.RAZON_SOCIAL == searchModel.razon_social);

			//Lugares:
			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(searchModel.descripcion.ToLower()));

			if (searchModel.tipoentidad > 0)
				query = query.Where(item => item.TIPO_ID == searchModel.tipoentidad);

			//Categorías pueden ser varias para una entidad:
			if (searchModel.categoria.Length > 0)
				query = query.Where(item => item.CATS_RELS.Any(item2 => searchModel.categoria.Contains(item2.CATEGORIA_ID)));
				
			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   tipo = item.TIPO.DESCRIPCION_LARGA,
								   descripcion = item.DESCRIPCION_LARGA,
								   identificacion = item.IDENTIFICACION,
								   razon_social = item.RAZON_SOCIAL,
								   apellidos = item.APELLIDOS,
								   nombres = item.NOMBRES
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.tipo, item.descripcion, item.identificacion, item.razon_social, item.apellidos, item.nombres};
			});
			
			//Utils.dump(DBContext.ENTIDADES_CATS.Where(item => searchModel.categoria.Contains(item.ID)).Count());

			return JsonRecords(records, searchModel.recordcount);
		}


		/*MANEJO DE DIRECCIONES*/
		public IActionResult DireccionIndex()
		{
			//int fieldindex = 0;

			//var grid = new jqxGrid();

			//grid.AddColumn("Código interno", "150px", "id", "string", fieldindex++.ToString());
			//grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());

			//var model = new ViewModels.SearchGridViewModel();
			//model.Grid = grid;
			//model.TargetContainer = "App.$splittedBody.$right";
			//model.SearchUrl = "/entidades/entidad/direccion_search";
			//model.Title = "ENTIDADES::ENTIDAD::DIRECCION BUSQUEDA";
			//model.EditButton = true;

			var model = new ViewModels.TabbedLayoutForm()
			{
				FormId = "form_entidades_direccion_edit",
				Title = "ENTIDADES::ENTIDAD::DIRECCIONES EDIT"
			};

			ViewData["entidades"] = DBContext.ENTIDADES.ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["paises"] = DBContext.PAISES.ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["provincias"] = DBContext.PROVINCIAS.ToJqwidgets().PrependBlankItem().ToJsonString();
			ViewData["ciudades"] = DBContext.CIUDADES.ToJqwidgets().PrependBlankItem().ToJsonString();
			//REGIONID TBD
			return View(model);
		}



		public IActionResult Contactos(int id)
		{
			var query = DBContext.ENTIDADES_CTOS
				.Include(item => item.PERSONA)
				.Include(item => item.CARGO)
				.Where(item => item.EMPRESA_ID == id)
				.Select(item => new
				{
					id = item.ID,
					persona_id = item.PERSONA_ID,
					apellidos = item.PERSONA.APELLIDOS,
					nombres = item.PERSONA.NOMBRES,
					cargo = item.CARGO.DESCRIPCION_LARGA
				});

			return JsonRecords(query.ToList());
		}

		public IActionResult ContactoDe(int id)
		{
			var query = DBContext.ENTIDADES_CTOS
				.Include(item => item.EMPRESA)
				.Where(item => item.PERSONA_ID == id)
				.OrderBy(item => item.EMPRESA.DESCRIPCION_LARGA)
				.Select(item => new
				{
					id = item.EMPRESA_ID,
					descripcion = item.EMPRESA.DESCRIPCION_LARGA,
					razon_social = item.EMPRESA.RAZON_SOCIAL
				});


			var query2 = DBContext.EQUIPOS_CATS.Where(item => item.ID == 3);

			var entity = query2.FirstOrDefault();


			

			if (entity != null)
			{

				var collection = DBContext.Entry(entity).Collection("EQUIPOS");
				//var collection = DBContext.Entry<EQUIPOS_CATS>(entity).Collection<EQUIPOS>(x => x.EQUIPOS);

				//collection.Load();




				IEnumerable<ICommonEntityInt32> list = (IEnumerable<ICommonEntityInt32>)collection.CurrentValue;

				var listaFinal = list.Select(item => new
				{
					id = item.ID,
					descripcion = item.DESCRIPCION_LARGA,
					estado = item.ESTADO_ID
				});

				Utils.dump(listaFinal);

				//HashSet<EQUIPOS> xx;
				//xx.Add()

				//Console.WriteLine(list);

			}
			else
				Console.WriteLine("es nulo");
			
			//DBContext.Entry(DBContext.EQUIPOS_CATS).co

			return JsonRecords(query.ToList());
		}




		public IActionResult Horarios_Atencion(int id)
		{
			var query = DBContext.ENTIDADES_CALENDS
				.Include(item => item.HORARIOS)
				.Where(item => item.ENTIDAD_ID == id)
				.OrderBy(item => item.ANO)
				.Select(item => new
				{
					id = item.ID,
					descripcion = item.DESCRIPCION_LARGA,
					anio = item.ANO
				});
			return JsonRecords(query.ToList());
		}










		[Route("/entidades/[controller]/[action]/{empresaId}/{contactoId?}")]
		public IActionResult ContactoEditForm(int empresaId, int contactoId)
		{
			ViewData["empresa"] = empresaId;
			ViewData["cargos"] = DBContext.ENTIDADES_CARGOS.ToJqwidgets().ToJsonString();
			ViewData["areas"] = DBContext.ENTIDADES_AREAS.ToJqwidgets().ToJsonString();

			var query = DBContext.ENTIDADES_DIRS.Where(item => item.ENTIDAD_ID == empresaId);


			ViewData["direcciones"] = query.Select(item => new
			{
				id = item.ID,
				descripcion = item.DESCRIPCION_LARGA,
				ciudadela = item.CIUDADELA,
				calle_principal = item.CALLE_PRINCIPAL,
				ciudad = item.CIUDAD.DESCRIPCION_LARGA
			}).ToJsonString();

			ContactoEdit postModel = null;

			if (contactoId > 0)
			{
				postModel = ContactoEdit.Load(DBContext, contactoId);
			}

			if (postModel == null)
			{
				ViewData["data"] = "null";
			}
			else
			{
				var persona_descripcion = DBContext.ENTIDADES.First(item => item.ID == postModel.persona).DESCRIPCION_LARGA;
				ViewData["data"] = postModel.ToJson(new { persona_label = persona_descripcion });
			}

			Console.WriteLine("emprea: " + empresaId);


			var entity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == empresaId);


			var viewModel = new ViewModels.PopupEdit()
			{
				Title = entity.DESCRIPCION_LARGA + " / CONTACTO",
				FormId = "form_contacto",
				PostUrl = "/entidades/entidad/contactosave"
			};

			return View(viewModel);
		}

		//public IActionResult ContactoEditData(int id)
		//{
		//	var model = ContactoEdit.Load(DBContext, id);
		//	return JsonRecord(model, new { persona_descripcion = DBContext.ENTIDADES.First(item => item.ID == model.persona).DESCRIPCION_LARGA });
		//	//return JsonRecord(model);
		//}

		public IActionResult ContactoSave(ContactoEdit postModel)
		{
			var errors = ModelState.ToJsonObject();

			//postModel.


			if (errors["persona"] == null)
			{
				if (DBContext.ENTIDADES_CTOS.Any(item => item.EMPRESA_ID == postModel.empresa && item.PERSONA_ID == postModel.persona && item.ID != postModel.id))
				{
					errors.Add("persona", JObject.FromObject(new { error = "La persona ya tiene relación con la empresa" }));
				}
				//PENDIENTE: VALIDAR SI CAMIA DE EMPRESA, HAY ELIMINAR LAS RELACIONES CON ESA DIRECCION DE SER EL CASO
			}

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = postModel.Save(DBContext);
				Console.WriteLine("saved id: " + id);
				var contacto = DBContext.ENTIDADES_CTOS.AsNoTracking()
					.Include(item => item.EMPRESA)
					.Include(item => item.PERSONA)
					.Where(item => item.ID == id).FirstOrDefault();
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(contacto.EMPRESA.DESCRIPCION_LARGA + "->" + contacto.PERSONA.DESCRIPCION_LARGA), new { id = id });

			}
		}


		//[HttpGet("{!id}/{entidadId}/{contactoId}")]
		[Route("/entidades/[controller]/[action]/{personaId}/{contactoId?}")]

		public IActionResult ContactoDeEditForm(int personaId, int contactoId)
		{
			ViewData["personaId"] = personaId;
			ViewData["cargos"] = DBContext.ENTIDADES_CARGOS.ToJqwidgets().ToJsonString();
			ViewData["areas"] = DBContext.ENTIDADES_AREAS.ToJqwidgets().ToJsonString();

			var entity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == personaId);


			ContactoDeEdit postModel = null;

			if (contactoId > 0)
			{
				postModel = ContactoDeEdit.Load(DBContext, contactoId);
			}

			if (postModel == null)
			{
				ViewData["data"] = "null";
			}
			else
			{
				var empresa_descripcion = DBContext.ENTIDADES.First(item => item.ID == postModel.empresa).DESCRIPCION_LARGA;
				ViewData["data"] = postModel.ToJson(new { empresa_label = empresa_descripcion });
			}





			var viewModel = new ViewModels.PopupEdit()
			{
				Title = entity.DESCRIPCION_LARGA + " / CONTACTO DE",
				FormId = "form_contactode",
				PostUrl = "/entidades/entidad/contactodesave"
			};

			return View(viewModel);
		}

		public IActionResult ContactoDeSave(ContactoDeEdit postModel)
		{

			var errors = ModelState.ToJsonObject();


			if (errors["empresa"] == null)
			{
				if (DBContext.ENTIDADES_CTOS.Any(item => item.EMPRESA_ID == postModel.empresa && item.PERSONA_ID == postModel.persona && item.ID != postModel.id))
				{
					//errors.Add(new { empresa = new { error = "Ya existe relación con la empresa seleccionada" } });
					errors.Add("empresa", JObject.FromObject(new { error = "Ya existe relación con la empresa seleccionada" }));
				}

				//PENDIENTE: VALIDAR SI CAMIA DE EMPRESA, HAY ELIMINAR LAS RELACIONES CON ESA DIRECCION DE SER EL CASO
			}


			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{


				var id = postModel.Save(DBContext);
				var contacto = DBContext.ENTIDADES_CTOS.AsNoTracking()
					.Include(item => item.EMPRESA)
					.Include(item => item.PERSONA)
					.Where(item => item.ID == id).FirstOrDefault();
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(contacto.EMPRESA.DESCRIPCION_LARGA + "->" + contacto.PERSONA.DESCRIPCION_LARGA), new { id = id });

			}

		}

		/*public IActionResult ContactoDeQuickView(int id)
		{
			var quickViewModel = new ViewModels.QuickView()
			{
				Title = "DETALLE CONTACTO",
				//Details = ViewModels.Entidades.ContactoDetails.Load(DBContext, id, true),
				//Details = ViewModels.Entidades.EntidadDetails.LoadPersona(DBContext, id),
				ElementsIdPrefix = $"quickview_contacto{id}"
			};
			return View(quickViewModel);
		}*/


		public IActionResult Telefonos(int id)
		{

			var query = DBContext.ENTIDADES_TELFS.Where(item => item.ENTIDAD_ID == id && item.DIRECCION_ID == null && item.UBICACION_ID == null);

			var telefonos = query.Select(item => new {
				id = item.ID,
				tipo = item.TIPO.DESCRIPCION_LARGA,
				codigo_pais = item.CODIGO_PAIS,
				codigo_area = item.CODIGO_AREA,
				telefono = item.TELEFONO
			}).ToList();
			return JsonRecords(telefonos);
		}


		[HttpGet("/entidades/[controller]/[action]/{entidadId}/{direccionId}/{ubicacionId}/{id?}")]
		public IActionResult TelefonoEditForm(int entidadId, int direccionId, int ubicacionId, int id)
		{
			ViewData["entidadId"] = entidadId;
			ViewData["direccionId"] = direccionId;
			ViewData["ubicacionId"] = ubicacionId;
			ViewData["tipos"] = DBContext.TELEFONO_TIPOS.ToJqwidgets().ToJsonString();


			string title = "";

			var entidadEntity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == entidadId);

			title = entidadEntity.DESCRIPCION_LARGA;

			if(direccionId > 0)
			{
				var direccionEntity = DBContext.ENTIDADES_DIRS.FirstOrDefault(item => item.ID == direccionId);
				title += " / " + direccionEntity.DESCRIPCION_LARGA;

				if(ubicacionId > 0)
				{
					var ubicacionEntity = DBContext.ENTIDADES_DIRS_UBICAS.FirstOrDefault(item => item.ID == ubicacionId);
					title += " / " + ubicacionEntity.DESCRIPCION_LARGA;
				}
			}

			//var direcci


			TelefonoEdit postModel = null;

			if (id > 0)
			{
				postModel = TelefonoEdit.Load(DBContext, id);
			}

			if (postModel == null)
			{
				ViewData["data"] = "null";
			}
			else
			{
				ViewData["data"] = postModel.ToJson();
			}


			var viewModel = new ViewModels.PopupEdit()
			{
				Title = "TELEFONO: " +  title,
				FormId = "form_entidad_telefono",
				PostUrl = "/entidades/entidad/telefonosave"
			};

			return View(viewModel);
		}

		//public IActionResult TelefonoEditData(int id)
		//{
		//	var model = TelefonoEdit.Load(DBContext, id);
		//	return JsonRecord(model);
		//}


		public IActionResult TelefonoSave(TelefonoEdit editModel)
		{
			var errors = ModelState.ToJsonObject();

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.telefono), new { id = id });
			}
		}

		public IActionResult Emails(int id)
		{

			var query = DBContext.ENTIDADES_EMAILS.Where(item => item.ENTIDAD_ID == id);

			var emails = query.Select(item => new {
				id = item.ID,
				email = item.EMAIL,
				tipo = item.TIPO.DESCRIPCION_LARGA
			}).ToList();
			return JsonRecords(emails);
		}


		[HttpGet("/entidades/[controller]/[action]/{entidadId}/{id?}")]
		public IActionResult EmailEditForm(int entidadId, int id)
		{
			ViewData["entidadId"] = entidadId;
			ViewData["tipos"] = DBContext.EMAIL_TIPOS.ToJqwidgets().ToJsonString();
			ViewData["propositos"] = DBContext.EMAIL_PROPS.ToJqwidgets().ToJsonString();


			var entidadEntity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == entidadId);
			var title = entidadEntity.DESCRIPCION_LARGA;



			EmailEdit postModel = null;

			if (id > 0)
			{
				postModel = EmailEdit.Load(DBContext, id);
			}

			if (postModel == null)
			{
				ViewData["data"] = "null";
			}
			else
			{
				ViewData["data"] = postModel.ToJson();
			}




			var viewModel = new ViewModels.PopupEdit()
			{
				Title = "EMAIL: " + title,
				FormId = "form_entidad_email",
				PostUrl = "/entidades/entidad/emailsave"
			};

			return View(viewModel);
		}

		//public IActionResult EmailEditData(int id)
		//{

		//	var model = EmailEdit.Load(DBContext, id);
		//	return JsonRecord(model);
		//}

		public IActionResult EmailSave(EmailEdit editModel)
		{
			var errors = ModelState.ToJsonObject();

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.email), new { id = id });
			}
		}

		public IActionResult ContactosWindowSearch(int id)
		{
			ViewData["empresa"] = id;

			var entity = DBContext.ENTIDADES.FirstOrDefault(item => item.ID == id);

			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Cód. Int.", "70px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Cód. Pers.", "70px", "persona_id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "150px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Apellidos", "150px", "apellidos", "string", fieldindex++.ToString());
			grid.AddColumn("Nombres", "150px", "nombres", "string", fieldindex++.ToString());
			grid.AddColumn("Cargo", "150px", "identificacion", "string", fieldindex++.ToString());

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.SearchUrl = "/entidades/entidad/contactossearch";
			model.Title = "BUSQUEDA DE CONTACTOS: " + entity.DESCRIPCION_LARGA;
			return View(model);
		}


		public IActionResult ContactosSearch(ContactoSearch searchModel)
		{

			var query = DBContext.ENTIDADES_CTOS.Where(item => item.EMPRESA_ID == searchModel.empresa);

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.PERSONA.DESCRIPCION_LARGA.ToLower().Contains(searchModel.descripcion.ToLower()));

			if (!string.IsNullOrWhiteSpace(searchModel.nombres))
				query = query.Where(item => item.PERSONA.NOMBRES.ToLower().Contains(searchModel.nombres.ToLower()));


			if (!string.IsNullOrWhiteSpace(searchModel.apellidos))
				query = query.Where(item => item.PERSONA.APELLIDOS.ToLower().Contains(searchModel.apellidos.ToLower()));

			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = query.OrderBy(item => item.PERSONA.DESCRIPCION_LARGA)
				.Select(item => new
				{
					id = item.ID,
					persona_id = item.PERSONA_ID,
					descripcion = item.PERSONA.DESCRIPCION_LARGA,
					apellidos = item.PERSONA.APELLIDOS,
					nombres = item.PERSONA.NOMBRES,
					cargo = item.CARGO.DESCRIPCION_LARGA

				})
				.Skip(searchModel.pagenum * searchModel.pagesize)
				.Take(searchModel.pagesize);



			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.persona_id, item.descripcion, item.apellidos, item.nombres, item.cargo };
			});
			return JsonRecords(records, searchModel.recordcount);
		}


		public IActionResult EmpresasWindowSearch()
		{
			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Cód. Int.", "50px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "150px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Razón Social", "150px", "razon_social", "string", fieldindex++.ToString());
			grid.AddColumn("RUC", "150px", "ruc", "string", fieldindex++.ToString());

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			//model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/entidades/entidad/empresassearch";
			model.Title = "BUSQUEDA DE EMPRESAS";
			return View(model);
		}


		public IActionResult EmpresasSearch(EmpresaSearch searchModel)
		{

			var query = DBContext.ENTIDADES.Where(item => item.TIPO_ID == 1);

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(searchModel.descripcion.ToLower()));

			if (!string.IsNullOrWhiteSpace(searchModel.razon_social))
				query = query.Where(item => item.RAZON_SOCIAL.ToLower().Contains(searchModel.razon_social.ToLower()));

			if (!string.IsNullOrWhiteSpace(searchModel.ruc))
				query = query.Where(item => item.IDENTIFICACION.Contains(searchModel.ruc));

			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();

			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
								   razon_social = item.RAZON_SOCIAL,
								   ruc = item.IDENTIFICACION
							   };

			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion, item.razon_social, item.ruc };
			});

			//Utils.dump(DBContext.ENTIDADES_CATS.Where(item => searchModel.categoria.Contains(item.ID)).Count());

			return JsonRecords(records, searchModel.recordcount);
		}



		public IActionResult WindowSearch()
		{
			var grid = new jqxGrid();
			var fieldindex = 0;
			grid.AddColumn("Cód. Int.", "50px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "150px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Razón Social", "150px", "razon_social", "string", fieldindex++.ToString());
			grid.AddColumn("RUC", "150px", "ruc", "string", fieldindex++.ToString());

			var model = new ViewModels.SearchGridViewModel();
			model.Grid = grid;
			model.TargetContainer = "App.$splittedBody.$right";
			model.SearchUrl = "/entidades/entidad/search";
			model.Title = "BUSQUEDA DE EMPRESAS";

			ViewData["tipoidentificacion"] = DBContext.ENTIDADES_IDENT_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
			ViewData["tipoentidad"] = DBContext.ENTIDADES_TIPOS.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();
			ViewData["categoriaentidad"] = DBContext.CategoriesTree<ENTIDADES_CATS>().ToJqwidgetsTree(false);
			ViewData["relaciones"] = DBContext.ENTIDADES_RELS.ToJqwidgets().PrependItem(0, "TODOS").ToJsonString();

			return View(model);

		}

	}
}
