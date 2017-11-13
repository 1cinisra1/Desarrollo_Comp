using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MVCHelpers.ActionResults
{
	public class JsonStringResult : Microsoft.AspNetCore.Mvc.ContentResult
	{
		public JsonStringResult(string jsonString)
		{
			this.StatusCode = 200;
			this.ContentType = "application/json; charset=utf-8";
			this.Content = jsonString;

			//Console.WriteLine(this.Value.GetType().ToString());
		}
	}

	public class JsonResponse : ActionResult
	{
		private byte[] _content;

		public JsonResponse(object value)
		{
			_content = Encoding.UTF8.GetBytes(JObject.FromObject(value).ToString(Newtonsoft.Json.Formatting.None));
		}

		public JsonResponse(string value)
		{
			_content = Encoding.UTF8.GetBytes(value);
		}

		public JsonResponse(JObject value)
		{
			//_content = Encoding.ASCII.GetBytes(stringToWrite);
			_content = Encoding.UTF8.GetBytes(value.ToString(Newtonsoft.Json.Formatting.None));
		}

		public override Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.StatusCode = 200;
			context.HttpContext.Response.ContentType = "application/json; charset=utf-8";

			return context.HttpContext.Response.Body.WriteAsync(_content, 0, _content.Length);

			//return base.ExecuteResultAsync(context);
		}


	}
}
