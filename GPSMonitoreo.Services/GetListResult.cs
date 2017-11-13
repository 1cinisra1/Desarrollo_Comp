using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{
	public interface IGetListResult<out TItems>
	{
		TItems Items { get; }

		int RecordCount { get; set; }

	}


	public class GetListResult<TItems> : OperationResult, IGetListResult<TItems>
	{
		public TItems Items { get; set; }

		public int RecordCount { get; set; }
	}
}
