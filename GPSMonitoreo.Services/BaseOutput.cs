using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{
    public class BaseOutput
    {
		public Enums.OutputStatus Status;

		public string ErrorMessage { get; set; }

		public string[] ErrorMessages { get; set; }
    }
}
