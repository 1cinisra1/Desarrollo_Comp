using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewModels
{
	public class AppLayoutFreeForm: AppLayoutForm
	{
		public bool UseFreeFormTable { get; set; } = true;

		public string FormStyle { get; set; }
	}
}
