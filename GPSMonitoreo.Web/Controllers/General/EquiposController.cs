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
using GPSMonitoreo.Web.PostModels.Geografico;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;

using GPSMonitoreo.Data.Models;
using System.Data.Entity;
using MVCHelpers.ActionResults;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using System.Collections;
using GPSMonitoreo.Web.QueryModels;
using GPSMonitoreo.Web.PostModels.Misc;

namespace GPSMonitoreo.Web.Controllers.General
{
    public class EquiposController : GeneralBaseController
    {
		public IActionResult CatsTree()
		{
			int fieldindex = 0;

			var grid = new jqxGrid();

			grid.AddColumn("Código", "50px", "id", "string", fieldindex++.ToString());
			////grid.AddColumn("Alterno", "50px", "alterno", "string", fieldindex++.ToString());
			grid.AddColumn("Descripción", "250px", "descripcion", "string", fieldindex++.ToString());
			grid.AddColumn("Estado", "100px", "estado", "string", fieldindex++.ToString());
			ViewData["objGrid"] = grid;
			ViewData["objGridFields"] = grid.Fields;
			ViewData["objGridColumns"] = grid.Columns;


			var arregloColecciones = generarCollection();

			var rels = new List<object>()
			{
				new {value = "EQUIPOS", label = "EQUIPOS"}
			};

			

			ViewData["lista_tablas"] = rels.ToJsonString();

			return TreeView(
				"ADMINISTRACION::EQUIPOS::JERARQUIA CATEGORIAS", 
				DBContext.CategoriesTree<EQUIPOS_CATS>(null, true).ToJqwidgetsTree(false),
				"App.general.equipos.catsedit"
			);


		}

		public IActionResult CatsTreeRels(TreeRelsSearch searchModel)
		{
			var entity = DBContext.EQUIPOS_CATS.FirstOrDefault(item => item.ID == searchModel.id);


			IQueryable<ICommonEntityInt32> query = null;

			if (entity != null)
			{

				switch (searchModel.rel)
				{

					case "EQUIPOS":
						query = DBContext.EQUIPOS.Where(item => item.CATEGORIA_ID == searchModel.id);
						break;

				}
			}

			if (query == null)
			{
				return JsonRecords(null);
			}
			else
			{

				if (searchModel.recordcount <= 0)
					searchModel.recordcount = query.Count();

				var query_select = query.OrderBy(item => item.DESCRIPCION_LARGA)
					.Select(item => new
					{
						id = item.ID,
						descripcion = item.DESCRIPCION_LARGA,
						estado = item.ESTADO_ID
					})
					.Skip(searchModel.pagenum * searchModel.pagesize)
					.Take(searchModel.pagesize);

				var records = query_select.ToList().Select(item =>
				{
					return new object[] { item.id, item.descripcion, item.estado };
				});

				return JsonRecords(records, searchModel.recordcount);
			}
		}


		public static List<JqwidgetsItem> ToJqwidgetsArray(List<string> entity)
		{
			List<JqwidgetsItem> lista = new List<JqwidgetsItem>();
			int i = 0;
			foreach (string obj in entity)
			{
				i = i + 1;
				lista.Add(new JqwidgetsItem { label = obj, value = i });
			}

			return lista;
		}

		private List<string> generarCollection()
		{

			List<string> arreglo = new List<string>();

			arreglo.Add("EQUIPOS");

			return arreglo;
		}

		private List<string> generarCollectionPorPropiedades()
		{

			var arregloTodosLosCampos = typeof(EQUIPOS_CATS).GetProperties()
						.Select(property => property.Name)
						.ToArray();

			var arregloSoloCampos = new List<string>();
			var obj = DBContext.EQUIPOS_CATS.First();
			var entity = DBContext.Entry(obj);
				foreach (var propertyName in entity.CurrentValues.PropertyNames)
				{
					arregloSoloCampos.Add(propertyName);
				}
			return obtenerNombresCollection(arregloTodosLosCampos, arregloSoloCampos.ToArray());
		}

		private List<string> obtenerNombresCollection(string[] arreglo_1, string[] arreglo_2)
		{
			List<string> arreglo = new List<string>();



			foreach (string obj1 in arreglo_1)
			{

				if (!obj1.Equals("HIJOS") && !obj1.Equals("PADRE"))
				{
					var encontro = arreglo_2.Where(x => x.Equals(obj1)).FirstOrDefault();
					if (encontro == null)
					{
						arreglo.Add(obj1);

					}
				}


			}

			return arreglo;

		}
	}
}
