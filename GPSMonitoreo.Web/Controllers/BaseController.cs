using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using GPSMonitoreo.Web.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers
{

	//[Authorize]
	public class BaseController : Controller
    {
		protected GPSMonitoreo.Data.Models.EntitiesContext DBContext;

		protected AppUser CurrentUser { get; set; }


		public static IServiceProvider ServiceProvider;

		public BaseController()
		{

		}




		public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if(this.User != null && this.User.Identity != null && this.User.Identity.IsAuthenticated)
			{
				CurrentUser = new AppUser(this.User);
			}
			
			

			

			DBContext = (GPSMonitoreo.Data.Models.EntitiesContext)HttpContext.RequestServices.GetService(typeof(GPSMonitoreo.Data.Models.EntitiesContext));
			return base.OnActionExecutionAsync(context, next);
		}

		private string GetViewPath()
        {
            var fullName = this.ControllerContext.ActionDescriptor.ControllerTypeInfo.FullName;
            var pos = fullName.IndexOf(".Controllers.");
            fullName = fullName.Substring(pos + 13);

            //Console.WriteLine(fullName + "ll");

            //return null;

            var splitted = fullName.Split(new[] { "." }, StringSplitOptions.None);
            var path = "Views/";
            for (var x = 0; x < splitted.Length - 1; x++)
            {
                path += splitted[x] + "/";
            }


            path += this.ControllerContext.ActionDescriptor.ControllerName + "/" + this.ControllerContext.ActionDescriptor.ActionName + ".cshtml";

            return path;

        }
		public override ViewResult View()
		{
			return base.View(GetViewPath());
		}

        public override ViewResult View(object model)
        {
            return base.View(GetViewPath(), model);
        }



        protected JsonResponse JsonResponse(string jsonString)
		{
			return new JsonResponse(jsonString);
		}

		protected JsonResponse JsonResponse(string status, string jsonTokens)
		{
			//return new JsonStringResult($"{{\"status\": \"{status}\", {jsonTokens}}}");
			return new JsonResponse($"{{\"status\": \"{status}\", {jsonTokens}}}");
			//var response = new Newtonsoft.Json.Linq.JObject();
			//response["status"] = status;
			//var obj = JObject.Parse(jsonTokens);
			//response.Merge(obj);
			//return Json(response);
		}

		protected JsonResponse JsonResponse(string status, object obj)
		{
			var response = new JObject();
			response["status"] = status;
			response.Merge(JObject.FromObject(obj));
			return JsonResponse(response);
		}

		protected JsonResponse JsonResponse(string status, object obj, object additionalTokens)
		{
			var response = new JObject();
			response["status"] = status;
			response.Merge(JObject.FromObject(obj));
			response.Merge(JObject.FromObject(additionalTokens));
			return JsonResponse(response);
		}

		protected JsonResponse JsonResponse(string status, JObject jObject)
		{
			var response = new JObject();
			response["status"] = status;
			response.Merge(jObject);
			return JsonResponse(response);
		}

		protected JsonResponse JsonResponse(string status, JObject jObject, JObject jObject2, bool mergeObjects = true)
		{
			var response = new JObject()
			{
				{"status", status}
			};



			if (mergeObjects)
			{
				jObject.Merge(jObject2);
				response.Merge(jObject);
			}
			else
			{
				response.Merge(jObject);
				response.Merge(jObject2);
			}





			//response.Merge(jObject);

			//response.Values().add

			//if (mergeObjects)
			//	response.Merge(jObject2);
			//else
			//	response.Add(jObject2);

			//response["status"] = status;
			//response.Merge(jObject);
			return JsonResponse(response);
		}


		protected JsonResponse JsonResponse(JObject jObject)
		{
			return new JsonResponse(jObject);
		}

		protected JsonResponse JsonOk(string msg)
		{
			return JsonResponse("OK", new { message = msg });
			//Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.FromObject(obj);
		}

		protected JsonResponse JsonError(string msg)
		{
			return JsonResponse("ERROR", new { message = msg });
			//Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.FromObject(obj);
		}

		protected JsonResponse JsonNotification(string msg)
		{
			return JsonResponse("OK", new { notification = msg });
		}

		protected JsonResponse JsonNotification(string msg, object additionalTokens)
		{
			return JsonResponse("OK", new { notification = msg }, additionalTokens);
		}


		protected JsonResponse JsonResponseActions(ResponseActionList actions)
		{
			return JsonResponse("OK", new { responseActions = actions });
		}

		protected JsonResponse JsonResponseActions(ResponseActionList actions, object additionalTokens)
		{
			return JsonResponse("OK", new { responseActions = actions }, additionalTokens);
		}





		//public JsonStringResult JsonResponse(JObject jObject)
		//{
		//	return new JsonStringResult(jObject.ToString(Newtonsoft.Json.Formatting.None));
		//	//var response = new Newtonsoft.Json.Linq.JObject();
		//	//response["status"] = status;
		//	//var obj = JObject.Parse(jsonTokens);
		//	//response.Merge(obj);
		//	//return Json(response);
		//}



		protected JsonResponse JsonRecords(IEnumerable<object> records, int recordcount)
		{



			//m_data["status"] = "OK";
			//m_data["records"] = (records != null) ? Newtonsoft.Json.Linq.JArray.FromObject(records) : new Newtonsoft.Json.Linq.JArray();
			//m_data["recordcount"] = recordcount;

			if(records == null)
				return JsonResponse("OK", new { records = new object[] { }, recordcount = 0 });
			else
				return JsonResponse("OK", new { records = records, recordcount = recordcount });




		}

		protected JsonResponse JsonRecords(IEnumerable<object> records)
		{
			//return JsonResponse("OK", new { records = records});

			if (records == null)
				return JsonResponse("OK", new { records = new object[] { } });
			else
				return JsonResponse("OK", new { records = records });

		}

		protected JsonResponse JsonRecord(object record)
		{
			return JsonResponse("OK", new { record = record });
		}

		protected JsonResponse JsonRecord(object record, object additionalTokens, bool mergeAdditionalTokens = true)
		{

			var merged = new JObject();
			var recordJObject = JObject.FromObject(record);
			merged.Add("record", recordJObject);

			if(mergeAdditionalTokens)
				recordJObject.Merge(JObject.FromObject(additionalTokens));
			else
				merged.Merge(JObject.FromObject(additionalTokens));

			return JsonResponse("OK", merged);
		}

		protected JsonResponse JsonRecord(object record, object additionalRecordTokens, object additionalResponseTokens)
		{

			var merged = new JObject();
			var recordJObject = JObject.FromObject(record);
			merged.Add("record", recordJObject);

			recordJObject.Merge(JObject.FromObject(additionalRecordTokens));

			merged.Merge(JObject.FromObject(additionalResponseTokens));

			return JsonResponse("OK", merged);
		}


		protected JsonResponse JsonFormErrors(JObject errors, string generalErrorMessage = null)
		{
			var jObject = new Newtonsoft.Json.Linq.JObject();
			jObject.Add("status", "ERRORFORM");
			jObject.Add("errors", errors);
			jObject.Add("errorMessage", "El formulario contiene 1 o más errores, favor verificar");
			return JsonResponse(jObject);
		}

		protected JsonResponse JsonFormErrors(Services.Validation.InputValidationErrors errors, string generalErrorMessage = null)
		{
			var jObject = new Newtonsoft.Json.Linq.JObject();
			jObject.Add("status", "ERRORFORM");
			jObject.Add("errors", JObject.FromObject(errors));
			jObject.Add("errorMessage", "El formulario contiene 1 o más errores, favor verificar");
			return JsonResponse(jObject);
		}

		protected JsonResponse JsonFormErrors(ModelStateDictionary modelState, string generalErrorMessage = null)
		{
			//var errors = ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
			//List<object> errorslist = new List<object>();
			//List<string> suberrors;

			//foreach (var err in errors)
			//{
			//	suberrors = new List<string>();
			//	foreach (var valueerror in err.Value.Errors)
			//	{
			//		suberrors.Add(valueerror.ErrorMessage);
			//		Console.WriteLine(valueerror.ErrorMessage);
			//		System.Diagnostics.Trace.WriteLine(valueerror.ErrorMessage);
			//	}
			//	errorslist.Add(new { field = err.Key, errors = suberrors });
			//}


			var errors = new Dictionary<string, object>();

			foreach (var keypair in ModelState.Where(n => n.Value.Errors.Count > 0).ToList())
			{
				errors.Add(keypair.Key, new { error = keypair.Value.Errors[0].ErrorMessage });
				//errors.Add(keypair.Key, (IReadOnlyDictionary<string, ModelBinding.ModelStateEntry>)keypair.Value);
			}

			var jObject = new Newtonsoft.Json.Linq.JObject();
			jObject.Add("status", "ERRORFORM");
			jObject.Add("errors", JToken.FromObject(errors));
			jObject.Add("errorMessage", "El formulario contiene 1 o más errores, favor verificar");


			return JsonResponse(jObject);
		}
	}
}
