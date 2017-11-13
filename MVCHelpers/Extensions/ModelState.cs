using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace MVCHelpers.Extensions
{
    public static class ModelStateExtensions
    {

		public static JObject ToJsonObject(this ModelStateDictionary modelState)
		{


			//String sourcestring = "telefonos[2].codigo_pais";
			Regex re = new Regex(@"(\w+)\[([0-9]+)\].(\w+)");
			//Match match = re.Match(sourcestring);

			string field;
			string childFild;
			int index;





			//for (int gIdx = 0; gIdx < m.Groups.Count; gIdx++)
			//{
			//	Console.WriteLine("[{0}] = {1}", re.GetGroupNames()[gIdx], m.Groups[gIdx].Value);
			//}

			var ret = new JObject();
			

			Match match;

			//foreach (var key in modelState.Keys)
			//{
			//	match = re.Match(key);





			//}

			JArray fieldToken;
			JObject lastItem;
			JObject lastItemErrors;
			

			foreach (var keypair in modelState.Where(n => n.Value.Errors.Count > 0).ToList())
			{
				Console.WriteLine("key: " + keypair.Key);
				match = re.Match(keypair.Key);

				if (match.Groups.Count == 4)
				{
					field = match.Groups[1].Value;
					index = Int32.Parse(match.Groups[2].Value);
					childFild = match.Groups[3].Value;

					fieldToken = (JArray)ret[field];
					if(fieldToken == null)
					{
						fieldToken = new JArray();
						ret.Add(field, fieldToken);
					}

					lastItem = (JObject)fieldToken.Last;

					if(lastItem == null || lastItem["rowIndex"].Value<int>() != index)
					{
						lastItem = new JObject();
						lastItem.Add("rowIndex", index);
						lastItemErrors = new JObject();
						lastItem.Add("errors", lastItemErrors);
						fieldToken.Add(lastItem);
					}
					else
					{
						lastItemErrors = (JObject)lastItem["errors"];
					}

					lastItemErrors.Add(childFild, new JObject(new JProperty("error", keypair.Value.Errors[0].ErrorMessage)));
				}
				else
				{
					ret.Add(keypair.Key, JToken.FromObject(new { error = keypair.Value.Errors[0].ErrorMessage }));
				}





				//errors.Add(keypair.Key, new { error = keypair.Value.Errors[0].ErrorMessage });
				//ret.Add(keypair.Key, JToken.FromObject(new { error = keypair.Value.Errors[0].ErrorMessage }));
				
				//errors.Add(keypair.Key, (IReadOnlyDictionary<string, ModelBinding.ModelStateEntry>)keypair.Value);
			}

			return ret;
		}

	}
}
