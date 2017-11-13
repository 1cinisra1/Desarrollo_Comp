using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewComponents.Models
{
    public class FormModel
    {
		public string FormId { get; set; }

		public string Action { get; set; }

		public string Title { get; set; }

		public bool UseTable { get; set; } = true;

		public string Style { get; set; }
	}
}
