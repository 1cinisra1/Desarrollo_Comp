using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{
    public class GetStringResult : OperationResult
    {
		public string Result;

		public void SetSuccessResult(string result)
		{
			this.Result = result;
			this.SetSuccess();
		}
    }
}
