using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels
{

	[Microsoft.AspNetCore.Mvc.ModelBinder(BinderType = typeof(ModelBinders.ViewModelBinder))]
	public class PostModel
    {


		public virtual void OnBindingFinished()
		{

		}

		[Newtonsoft.Json.Serialization.OnError]
		void OnError(System.Runtime.Serialization.StreamingContext context, Newtonsoft.Json.Serialization.ErrorContext errorContext)
		{
			Console.WriteLine("PARSING ERROR: " + errorContext.Member.ToString());
			errorContext.Handled = true;
		}


		public JObject ToJObject(object additionalTokens = null)
		{
			var ret = JObject.FromObject(this);
			if(additionalTokens != null)
				ret.Merge(JObject.FromObject(additionalTokens));
			return ret;
		}

		public string ToJson(object additionalTokens = null)
		{
			return ToJObject(additionalTokens).ToString(Newtonsoft.Json.Formatting.None);
		}
	}
}
