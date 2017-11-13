using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewComponents.jqxGrid
{
    public class Column
    {
		//{ text: 'Empresa', datafield: 'empresa', width: 150 },

		public string text { get; set; }
		public string datafield { get; set; }
		public string width { get; set; }

		public string columntype { get; set; }

		public string source { get; set; }

		public string cellsrenderer { get; set; }


		public override string ToString()
		{
			string ret = $"{{text: '{text}', datafield: '{datafield}', width: '{width}'";

			if (columntype != null)
				ret += $", columntype: '{columntype}'";

			if(source != null)
				ret += $", source: {source}";

			if (cellsrenderer != null)
				ret += $", cellsrenderer: {cellsrenderer}";



			ret += "}";
			return ret;
			//return base.ToString();
		}
	}
}
