using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers
{
    public class AjaxController : Controller
    {
		// GET: /<controller>/
		//public JsonResult JsonRedirect(string url)
		//{

		//	m_data["status"] = "REDIRECT";
		//	m_data["url"] = url;
		//	return Json(m_data);
		//}

		//public JsonResult JsonObject(object obj)
		//{
		//	Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.FromObject(obj);
		//	jsonObject["status"] = "OK";
		//	return Json(jsonObject);
		//}

		//public JsonResult JsonOk(String message, object extra = null)
		//{
		//	m_data["status"] = "OK";
		//	m_data["message"] = message;

		//	if (extra != null)
		//	{
		//		Console.WriteLine("si ha extra");
		//		Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.FromObject(extra);

		//		m_data.Merge(obj);
		//	}

		//	return Json(m_data);
		//}

		//public JsonResult JsonError(String message)
		//{
		//	m_data["status"] = "ERROR";
		//	m_data["message"] = message;
		//	return Json(m_data);
		//}

		//public JsonResult JsonErrors(List<object> errors)
		//{
		//	m_data["status"] = "ERROR";
		//	m_data["errors"] = Newtonsoft.Json.Linq.JArray.FromObject(errors); ;
		//	return Json(m_data);
		//}

		//public JsonResult JsonErrorsForm(List<object> errors)
		//{
		//	m_data["status"] = "ERRORFORM";
		//	m_data["message"] = "Se encontraron los siguientes errores:";
		//	m_data["errors"] = Newtonsoft.Json.Linq.JArray.FromObject(errors); ;
		//	return Json(m_data);
		//}

		//public JsonResult JsonOkInserted(object extra = null)
		//{
		//	return JsonOk("Se ingresó el registro con éxito", extra);
		//}

		//public JsonResult JsonOkUpdated(object extra = null)
		//{
		//	return JsonOk("Se actualizó el registro con éxito", extra);
		//}

		//public JsonResult JsonRecordSet(IEnumerable<object[]> records, int recordcount)
		//{
		//	m_data["status"] = "OK";
		//	m_data["records"] = (records != null) ? Newtonsoft.Json.Linq.JArray.FromObject(records) : new Newtonsoft.Json.Linq.JArray();
		//	m_data["recordcount"] = recordcount;
		//	return Json(m_data);
		//}
	}
}
