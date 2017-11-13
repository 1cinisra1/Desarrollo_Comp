using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels.Geografico;

using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Data.Models;

using Newtonsoft.Json.Linq;
using GPSMonitoreo.Web.ViewModels;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using System.Data;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;

namespace GPSMonitoreo.Web.Controllers.Geografico
{
    public class RutasController : BaseController
	{
		public IActionResult Fases()
		{
			var model = new AppLayoutFreeForm()
			{
				FormId = "rutas_fases",
				Title = "GEOGRAFICO::RUTAS::FASES",
				PostUrl = "/geografico/rutas/rutafasessave"
			};
			model.PrepareUrl(this.HttpContext);
			//model.PrepareEditUrls(this.HttpContext);


			ViewData["fases"] = DBContext.RUTAS_FASES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().PrependBlankItem().ToJsonString();

			return View(model);
		}

		[HttpGet("/geografico/[controller]/[action]/{id}/{entidadId}")]
		public IActionResult RutaFases(int id, int entidadId)
		{

			var parameters = new List<KeyValuePair<string, object>>
			{
				{"RUTA_ID", id },
				{"ENTIDAD_ID", entidadId }
			};

			var dt = DBContext.ProcedureDataTable("SP_RUTA_FASES", parameters);

			dt.Columns.Add("expanded", typeof(bool));
			//

			dt.Columns["expanded"].DefaultValue = true;
			//dt.AcceptChanges();

			foreach (DataRow row in dt.Rows)
			{
				row["expanded"] = true;

			}


			var mapper = new GPSMonitoreo.Libraries.Utils.Data.JsonTreeMapper
			{
				id = "ID",
				description = "DESCRIPCION_LARGA",
				level = "LEV",
				jsonNodeId = "id",
				jsonNodeDescription = "description",
				jsonNodeChildren = "children",
				extraFields = new Dictionary<string, string>()
				{
					//{"fase", "FASE_ID"}
					{ "fase", "FASE_ID"},
					{ "tipo", "TIPO_ID" },
					{ "expanded", "expanded" }
				}
			};

			var records = JArray.Parse(GPSMonitoreo.Libraries.Utils.Data.DataTableToJsonTree(dt, mapper));

			//var records = JObject.Parse(dt.ToJqwidgetsTree());


			return JsonRecords(records);
			//return JsonResponse("OK", new { ruta = records, fases = new { } });

			//Utils.dump(records);
			
		}

		public IActionResult RutaFasesSave(RutaFasesInputDto input)
		{
			var errors = ModelState.ToJsonObject();

			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				input.Save(DBContext);
				//return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.descripcion_larga), new { id = id });
				return JsonNotification(Resources.Messages.RecordSavedGeneral);
			}
		}

		[HttpGet("/geografico/[controller]/[action]/{id}/{entidadId}")]
		public IActionResult CercasFases(int id, int entidadId)
		{
			Console.WriteLine("public IActionResult CercasFases(int id, int entidadId)");
			var reader = DBContext.ProcedureDataReader("SP_RUTAS_CERCAS_FASES", new List<KeyValuePair<string, object>>() { { "P_ID", id }, { "P_ENTIDAD_ID", entidadId } });
			Console.WriteLine(reader.HasRows);

			var records = new List<object>();

			while(reader.Read())
			{
				records.Add(new
				{
					cerca = (int)reader["ID"],
					cerca_descripcion = (string)reader["DESCRIPCION_LARGA"],
					//entidad = (int)reader["ENTIDAD_ID"],
					fase = reader["FASE_ID"]


				});
			}

			return JsonRecords(records);
		}
	}
}
