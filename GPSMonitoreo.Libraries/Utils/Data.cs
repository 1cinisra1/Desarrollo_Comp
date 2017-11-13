using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Data.Common;

namespace GPSMonitoreo.Libraries.Utils
{
    public class Data
    {
		public class JsonTreeMapper
		{
			public string id;
			public string description;
			public string level;
			public string jsonNodeId;
			public string jsonNodeDescription;
			public string jsonNodeChildren;
			public Dictionary<string, string> extraFields;

		}
		public static string DataTableToJsonTree(DataTable dt, JsonTreeMapper mapping, bool addBlankItem = false)
		{
			if (dt.Rows.Count == 0)
				return "[]";

			int level = 0;
			int nextLevel;
			var json = new System.Text.StringBuilder();
			json.Append("[");


			if (addBlankItem)
				json.Append("{\"value\": 0, \"html\": \"<div style=\\\"width:100px;\\\">&nbsp;</div>\"}, ");

			int x = 0;

			System.Data.DataRow next;

			foreach (System.Data.DataRow row in dt.Rows)
			{
				level = (int)((decimal)row[mapping.level]);
				
				json.Append("{\"" + mapping.jsonNodeId + "\": " + JsonConvert.SerializeObject(row[mapping.id]) + ", \"" + mapping.jsonNodeDescription + "\": " + JsonConvert.ToString(row[mapping.description]));

				if(mapping.extraFields != null)
				{
					foreach(var keypair in mapping.extraFields)
					{
						Console.WriteLine(keypair.Value);
						json.Append(", \"" + keypair.Key + "\": " + JsonConvert.ToString(row[keypair.Value]));
					}
				}

				if ((x + 1) < dt.Rows.Count)
				{
					next = dt.Rows[x + 1];

					nextLevel = (int)((decimal)next[mapping.level]);

					if (nextLevel > level)
					{
						json.Append(", \"" + mapping.jsonNodeChildren + "\": [");
					}
					else if (nextLevel < level)
					{
						json.Append(string.Join("", Enumerable.Repeat("}]", level - nextLevel)) + "}, ");
					}
					else
						json.Append("}, ");
				}
				x++;
			}

			

			//if(level > 0)
				json.Append( string.Join("", Enumerable.Repeat("}]", level)) + "}");


			

			json.Append("]");

			Console.WriteLine(json.ToString());

			return json.ToString();
		}
	}
}
