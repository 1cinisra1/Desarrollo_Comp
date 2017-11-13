using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using MVCHelpers.ActionResults;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers.Geografico
{

	[Route("geografico/capas/[action]")]
	public class CapasController : BaseController
    {



		[HttpGet("{codigo}")]
		public JsonResponse Categorias(short codigo)
        {

			var data = from cat in DBContext.CERCAS_CATS.Where(x => x.CAPA_ID == codigo && x.PADRE_ID == null)
					   select new { value = cat.ID, label = cat.DESCRIPCION_LARGA };





			var pars = new[]
			{
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CAPA", Oracle.ManagedDataAccess.Client.OracleDbType.Int32) {Value = codigo},
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) {Direction = System.Data.ParameterDirection.Output }
			};


			

			var dt = DBContext.Database.SqlQuery("SP_CERCAS_CATEGORIAS", System.Data.CommandType.StoredProcedure, pars);

			var json = GPSMonitoreo.Libraries.Utils.Data.DataTableToJsonTree(dt, new GPSMonitoreo.Libraries.Utils.Data.JsonTreeMapper { id = "ID", description = "DESCRIPCION_LARGA", level = "LEV", jsonNodeId = "value", jsonNodeDescription = "label", jsonNodeChildren = "items" });

			return JsonResponse("OK", "\"records\": " + json);

			//return Json()
			//return JsonOk("positivo");
			//return JsonRecords(data);

			//return Content(GPSMonitoreo.Core.Utils.ObjectJsonDumper.ToJsonString(data));
        }
    }
}
