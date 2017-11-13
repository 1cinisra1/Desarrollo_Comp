using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{
    public class CreateUpdateResult<TId> : OperationResult
    {
		public TId Id;
		public string Description;
    }
}
