using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;

using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels;

namespace GPSMonitoreo.Web.Controllers.General
{

	[Route("/general/[controller]/[action]/{id?}")]
	public abstract class GeneralBaseController : BaseController
    {

		/// <summary>
		/// Método que devuelve una vista de edición.
		/// </summary>
		/// <param name="title">Título de la vista a devolver</param>
		/// <param name="forCats">Parámetro que indica si la entidad es recursiva o no. (si tiene un padreid)</param>
		/// <returns></returns>
		public IActionResult EditView(string title, bool forCats = false)
		{
			var url = Request.Path.Value;
			url = url.Substring(0, url.Length - 4);
			var layoutModel = new ViewComponents.Models.CommonFormModel()
			{
				FormId = "form" + url.Replace("/", "_"),
				Title = title,
				Action = url + "save",
				ForCats = forCats

			};

			//ViewData["ForCats"] = forCats;
			

			return View("~/Views/Shared/GeneralEditView.cshtml", layoutModel);
		}

		public IActionResult EditViewSpecial(string title, bool forCats = false)
		{
			var url = Request.Path.Value;
			url = url.Substring(0, url.Length - 4);
			var layoutModel = new ViewComponents.Models.CommonFormModel()
			{
				FormId = "form" + url.Replace("/", "_"),
				Title = title,
				Action = url + "save",
				ForCats = forCats

			};

			return View(layoutModel);
		}

		public IActionResult EditData<T>(byte id) where T : class, ICommonEntityByte, new()
		{
			var model = PostModelEdit<byte>.Load<T>(DBContext, id);
			return JsonRecord(model);
		}

		public IActionResult EditData<T>(Int16 id) where T : class, ICommonEntityInt16, new()
		{
			var model = PostModelEdit<Int16>.Load<T>(DBContext, id);
			return JsonRecord(model);
		}

		public IActionResult EditData<T>(Int32 id) where T : class, ICommonEntityInt32, new()
		{
			var model = PostModelEdit<Int32>.Load<T>(DBContext, id);
			return JsonRecord(model);
		}

		//public IActionResult EditDataCats<T>(Int16 id) where T : class, IComunCats<T>, new()
		//{
		//	var model = PostModelCats.Load<T>(DBContext, id);
		//	return JsonRecord(model);
		//}

		public ViewResult TreeView(string title, string jsonTree, string editMethod)
		{

			var layoutModel = new ViewModels.AppLayout()
			{
				Title = title
			};

			ViewData["jsonTree"] = "[{\"value\": 0, \"label\": \"RAIZ\", \"items\": " + jsonTree + "}]";
			ViewData["editMethod"] = editMethod;
			return View("~/Views/Shared/GeneralTreeView.cshtml", layoutModel);
		}

		/// <summary>
		/// Método que devuelve una vista.
		/// </summary>
		/// <param name="title">Título de la vista a mostrar</param>
		/// <param name="forCats">Indica si la entidad es una estructura recursiva (si tiene un padreid)</param>
		/// <returns></returns>
		public ViewResult SearchView(string title, bool forCats = false)
		{


			var grid = new jqxGrid();
			var fieldindex = 0;

			grid.AddColumn("Cód. Int.", "60px", "id", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "300px", "descripcion", "string", fieldindex++.ToString());

			if(forCats)
			{
				grid.AddColumn("Padre", "300px", "padre", "string", fieldindex++.ToString());
			}

			var searchUrl = Request.Path.Value + "search";


			Console.WriteLine("************************************");
			Console.WriteLine("************************************");
			Console.WriteLine(this.Request.Path.Value);
			Console.WriteLine("************************************");
			Console.WriteLine("************************************");

			var model = new ViewModels.SearchGridViewModel()
			{
				Grid = grid,
				SearchUrl = searchUrl,
				FormID = "form" + searchUrl.Replace("/", "_"),
				Title = title,
				EditButton = true,
				EditMethod = "App" + this.Request.Path.Value.Replace("/", ".") + "edit(id)",
				AddButton = true,
				AddMethod = "App" + this.Request.Path.Value.Replace("/", ".") + "edit()"
			};

			ViewData["ForCats"] = forCats;
			

			return View("~/Views/Shared/GeneralSearchView.cshtml", model);
		}


		public ViewResult SearchView(string title, jqxGrid grid)
		{

			var searchUrl = Request.Path.Value + "search";

			var model = new ViewModels.SearchGridViewModel()
			{
				Grid = grid,
				SearchUrl = searchUrl,
				FormID = "form" + searchUrl.Replace("/", "_"),
				Title = title,
				EditButton = true,
				EditMethod = "App" + this.Request.Path.Value.Replace("/", ".") + "edit(id)",
				AddButton = true,
				AddMethod = "App" + this.Request.Path.Value.Replace("/", ".") + "edit()"
			};


			return View("~/Views/Shared/GeneralSearchView.cshtml", model);
		}

		public IActionResult Save<T, TR>(PostModels.PostModelEdit<TR> postModel) where TR: struct, IEquatable<TR> where T : class, ICommonEntity<TR>, new()
		{

			var errors = ModelState.ToJsonObject();

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = postModel.Save<T>(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(postModel.descripcion_mediana), new { id = id });
			}
		}

		public IActionResult Save<T>(PostModels.PostModelEdit<byte> postModel) where T : class, ICommonEntityByte, new()
		{
			return Save<T, byte>(postModel);
		}

		public IActionResult Save<T>(PostModels.PostModelEdit<Int16> postModel) where T : class, ICommonEntityInt16, new()
		{
			return Save<T, Int16>(postModel);
		}

		public IActionResult Save<T>(PostModels.PostModelEdit<Int32> postModel) where T : class, ICommonEntityInt32, new()
		{
			return Save<T, Int32>(postModel);
		}

		//public IActionResult Save<T>(PostModels.PostModelCats postModel) where T : class, IComunCats<T>, new()
		//{

		//	var errors = ModelState.ToJsonObject();

			
			
		//	if(postModel.padre > 0)
		//	{
		//		if (postModel.id == postModel.padre)
		//		{
		//			errors.Add("padre", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "No se puede seleccionar asi mismo como padre" }));
		//		}
		//		else
		//		{

		//			var entityPadre = DBContext.Set<T>().Where(item => item.ID == postModel.padre).FirstOrDefault();
		//			if(entityPadre == null)
		//			{
		//				errors.Add("padre", Newtonsoft.Json.Linq.JToken.FromObject(new { error = "Item seleccionado no existe" }));
		//			}
		//		}
		//	}


		//	if (errors.Count > 0)
		//	{
		//		return JsonFormErrors(errors);
		//	}
		//	else
		//	{
		//		var id = postModel.Save<T>(DBContext);
		//		return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(postModel.descripcion_mediana), new { id = id });
		//	}
		//}

		public IActionResult Search(PostModels.GeneralSearch searchModel, System.Linq.IQueryable<GPSMonitoreo.Data.Models.ICommonEntityInt16> entity)
		{

			var query = entity;

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
							   };



			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion };
			});

			return JsonRecords(records, searchModel.recordcount);
		}


		public IActionResult Search(PostModels.GeneralSearch searchModel, System.Linq.IQueryable<GPSMonitoreo.Data.Models.ICommonEntityByte> entity)
		{

			var query = entity;

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
							   };



			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion };
			});

			return JsonRecords(records, searchModel.recordcount);
		}

		public IActionResult Search<T>(PostModels.GeneralSearch searchModel) where T : class, GPSMonitoreo.Data.Models.IComunCats<T>, new()
		{
			var query = DBContext.Set<T>().AsQueryable();
			var dummy1 = 1;			
			query = query.Where(item => 1 == dummy1);


			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));

			//query = query.Where(item => item.DESCRIPCION_LARGA.Contains("b"));

			


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = query.OrderByDescending(item => item.ID)
				.Select(item => new { id = item.ID })
				.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize)
				.Join(DBContext.Set<T>(), item => item.id, item2 => item2.ID, (a, b) => new {
					id = b.ID,
					descripcion = b.DESCRIPCION_LARGA,
					padre = DBContext.FN_HIERARCHY_PATH(typeof(T).Name, b.PADRE_ID)
				})
				.OrderByDescending(item => item.id)
				;

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion, item.padre };
			});

			return JsonRecords(records, searchModel.recordcount);



			//query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize)
			//	;

			//Utils.dump(query_select);

			//var query_select3 = from item in query_select
			//					select new { item.id };


			//var query_into = from item in query_select


			////var query_select3 = query_select2.Join(DBContext.Set<T>(), item1 => item1.id, item2 => item2.ID, (a, b) => new { b })
			////	.Select(item => new { item.b.ID, item.b.DESCRIPCION_LARGA })
			////	;

			//var query_select2 = from item in DBContext.Set<T>()
			//					let t2s = from item2 in query_select.Select(item3 => new { item3.id }) select item2.id
			//					where t2s.Contains(item.ID)
			//					select item.DESCRIPCION_LARGA

			//var query_select2 = query_select.Select(x => new { id = x.id }).GroupJoin(DBContext.Set<T>(), item1 => item1.id, item2 => item2.ID, (a, b) => new { b })
			//	.SelectMany(item => item.b.DefaultIfEmpty(), (a, b) => new { b.DESCRIPCION_LARGA });





			//var query_select2 = from item in query_select
			//					from item2 in DBContext.Set<T>()
			//					where item.id == item2.ID
			//					orderby item.id descending
			//					select new { id = item2.ID, descripcion = item2.DESCRIPCION_LARGA, padre = DBContext.FN_HIERARCHY_PATH(typeof(T).Name, item2.PADRE_ID ?? 0)};

			//Utils.dump(query_select2.ToList());

			//item.padre_id ?? 0

			//var records2 = from item in query_select.AsQueryable()
			//			   select item into r
			//			   select new
			//			   {
			//				   r.id,
			//				   r.descripcion,
			//				   //padre = DBContext.FN_PARENTS_PATH(typeof(T).Name, 0)
			//				   padre = r.id + r.descripcion
			//			   };

			//return Content("");


		}

		/// <summary>
		/// Método de búsqueda implementado para IComunInt32. 
		/// Se podía crear una clase en postmodels para aquellos que no puedan usar el generalsearch pero las entidades de localidades cumplen la estructura estandar
		/// </summary>
		/// <param name="searchModel"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public IActionResult Search(PostModels.GeneralSearch searchModel, System.Linq.IQueryable<GPSMonitoreo.Data.Models.ICommonEntityInt32> entity)
		{
			var query = entity;

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
							   };



			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion };
			});

			return JsonRecords(records, searchModel.recordcount);

		}

		
		/*public IActionResult Search<T>(PostModels.GeneralSearch searchModel, ref T entity)
		{
			if (entity is IComunInt32)
			{
				...
			}
			var query = (System.Linq.IQueryable<IComunInt32>)entity;

			if (!string.IsNullOrWhiteSpace(searchModel.descripcion))
				query = query.Where(item => item.DESCRIPCION_LARGA.Contains(searchModel.descripcion));


			if (searchModel.recordcount <= 0)
				searchModel.recordcount = query.Count();


			var query_select = from item in query
							   orderby item.ID descending
							   select new
							   {
								   id = item.ID,
								   descripcion = item.DESCRIPCION_LARGA,
							   };



			query_select = query_select.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize);

			var records = query_select.ToList().Select(item =>
			{
				return new object[] { item.id, item.descripcion };
			});

			return JsonRecords(records, searchModel.recordcount);
		}*/

	}
}
