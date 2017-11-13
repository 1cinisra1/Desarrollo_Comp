using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewComponents.jqxGrid
{
    public class jqxGrid
    {

		public List<DataField> Fields { get; set; }
		public List<Column> Columns { get; set; }
		public string Url { get; set; }

		public jqxGrid()
		{
			Fields = new List<DataField>();
			Columns = new List<Column>();
		}

		public string RenderColumns()
		{
			//var ret = new System.Text.StringBuilder();
			var cols = new List<string>();

			foreach(var c in Columns)
			{
				cols.Add(c.ToString());
			}


			return "[" + string.Join(", ", cols) + "]";

		}

		public void AddColumn(string text, string width)
		{
			Columns.Add(new ViewComponents.jqxGrid.Column { text = text, width = width });
		}

		public void AddColumn(string text, string width, string dataFieldName, string dataFieldType, string dataFieldIndex, string cellsrenderer = null)
		{
			var c = new ViewComponents.jqxGrid.Column { text = text, width = width, datafield = dataFieldName };
			if (cellsrenderer != null)
				c.cellsrenderer = cellsrenderer;

			Columns.Add(c);
			Fields.Add(new DataField { name = dataFieldName, type = dataFieldType, map = dataFieldIndex });
		}

		public void AddColumn(string text, string width, string dataFieldName, string dataFieldType, string columntype, object[] source, string dataFieldIndex)
		{
			Columns.Add(new ViewComponents.jqxGrid.Column { text = text, width = width, datafield = dataFieldName, columntype = columntype});
			Fields.Add(new DataField { name = dataFieldName, type = dataFieldType, map = dataFieldIndex, values = new { source = source, value = "value", name = "label" } });
		}
	}
}
