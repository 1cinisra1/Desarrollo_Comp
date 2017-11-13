using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos
{
    public class InputDto
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

	}
}
