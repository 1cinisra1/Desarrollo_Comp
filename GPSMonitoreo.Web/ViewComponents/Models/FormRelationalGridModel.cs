using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewComponents.Models
{
    public class FormRelationalGridModel
    {

		public string Title { get; set; }
		public string FormId { get; set; }
		public string FieldName { get; set; }

		public string AddMethod { get; set; }

		public bool IncludeAuxiliaryIdColumn { get; set; }
    }
}
