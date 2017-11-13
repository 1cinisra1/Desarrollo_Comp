using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{

	public interface IGetResult<out TDto>
	{
		TDto Item { get; }
	}

    public class GetResult<TDto>: OperationResult, IGetResult<TDto>
	{
		public TDto Item { get; set; }

    }
}
