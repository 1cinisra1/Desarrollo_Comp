using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Web.ViewComponents;

namespace GPSMonitoreo.Web.ViewComponents.Models
{
    public class FormEditableGridModel
    {
		public string ManagerId { get; set; } = "formgrid";

		public string Title { get; set; }
		public string FormId { get; set; }
		public string FieldName { get; set; }

		public short TabIndex { get; set; } = -1;

		public bool ShowEditButton { get; set; }

		public bool ShowReloadButton { get; set; }

		public bool ShowAddButton { get; set; } = true;

		public bool ShowRemoveButton { get; set; } = true;

		public bool Swapable { get; set; }

		public bool AddToForm { get; set; } = true;

		public bool ShowToolBar { get; set; } = true;




	}
}
