using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;

namespace MVCHelpers.TagHelpers
{

    public class InlineScriptManager
    {
		public System.Text.StringBuilder m_content;
		private Dictionary<string, System.Text.StringBuilder> m_dict;


		private string m_ID;
		private ViewContext m_viewContext;


		public InlineScriptManager(string ID, ViewContext viewContext)
		{
			m_ID = ID;
			m_viewContext = viewContext;
		}






		public void Add(string key, string content)
		{
			if (m_dict == null)
				m_dict = new Dictionary<string, System.Text.StringBuilder>();


			System.Text.StringBuilder builder;

			if(!m_dict.TryGetValue(key, out builder))
			{
				builder = new System.Text.StringBuilder();
				m_dict.Add(key, builder);
			}

			builder.Append(content);
		}

		public void Add(string content)
		{
			//Console.WriteLine("adding content to: " + m_ID + ": " + content);
			if (m_content == null)
				m_content = new System.Text.StringBuilder();

			
			m_content.Append(content);
			//Console.WriteLine("-----------------");
			//Utils.ObjectJsonDumper.Dump(m_viewContext.ViewData, 2);
		}

		public async Task<string> GetRenderedContentAsync(string scriptContent)
		{
			//Console.WriteLine("GetRenderedContentAsync: " + m_ID);
			System.Text.StringBuilder builder;
			scriptContent = Regex.Replace(scriptContent, @"\/\/script-manager\[""([^""]+)""\]", delegate (Match match)
			{
				//string v = match.ToString();

				//Console.WriteLine(v);
				//return char.ToUpper(v[0]) + v.Substring(1);
				//return match.Groups[1].Value;

				var key = match.Groups[1].Value;

				//Console.WriteLine("KEY: " + key);

				if (key.StartsWith("manager:"))
				{
					var managerId = "script-manager-" + key.Substring(8);
					//Console.WriteLine("inline manager: " + managerId);
					var inst = GetInstance(m_viewContext, managerId);


					//Console.WriteLine("manager KEY: " + key);
					//Console.WriteLine("manager id: " + managerId);

					////inst.RenderAsync(context, output);

					//Console.WriteLine("content is: " + inst.m_content.ToString());

					if (inst.m_content != null)
					{
						var result = inst.GetRenderedContentAsync(inst.m_content.ToString());
						return result.Result;
					}
					else
					{
						//Console.WriteLine("content of: " + managerId + " IS NULL");
						//Utils.ObjectJsonDumper.Dump(m_viewContext.ViewData, 1);
						return "null";
					}


					//return "ññ";
				}
				else
				{
					if (m_dict != null && m_dict.TryGetValue(key, out builder))
					{
						Console.WriteLine("getting by key: " + key);
						var parsed = GetRenderedContentAsync(builder.ToString());
						//return builder.ToString();
						return parsed.Result;
					}
					else
						return match.Groups[0].Value;
				}


			});
			//}
			//else
			//	Console.WriteLine("dict is null");






			//var inst = GetInstance(m_viewContext, "script-manager-" + "commonform");


			//var result = inst.GetRenderedContentAsync(context, output);
			//Console.WriteLine(inst.m_content);


			//return result.Result;


			return scriptContent;
		}

		public async Task RenderAsync(TagHelperContext context, TagHelperOutput output)
		{

			//Console.WriteLine("RenderAync: " + m_ID);
			var contents = await output.GetChildContentAsync();
			var scriptContent = contents.GetContent();

			////var list = (List<string>)ViewContext.ViewData[key];

			//Console.WriteLine("RenderAsync: " + m_ID);


			////if (m_dict != null)
			////{
			//	System.Text.StringBuilder builder;
			//	scriptContent = Regex.Replace(scriptContent, @"\/\/script-manager\[""([^""]+)""\]", delegate (Match match)
			//	{
			//		//string v = match.ToString();

			//		//Console.WriteLine(v);
			//		//return char.ToUpper(v[0]) + v.Substring(1);
			//		//return match.Groups[1].Value;

			//		var key = match.Groups[1].Value;

			//		Console.WriteLine("KEY: " + key);

			//		if (key.StartsWith("manager:"))
			//		{
			//			var inst = GetInstance(m_viewContext, key.Substring(8));

			//			inst.RenderAsync(context, output);

			//			return "";
			//		}
			//		else
			//		{
			//			if (m_dict != null && m_dict.TryGetValue(key, out builder))
			//			{
			//				return builder.ToString();
			//			}
			//			else
			//				return match.Groups[0].Value;
			//		}


			//	});
			////}
			////else
			////	Console.WriteLine("dict is null");



			//if (m_content != null)
			//	scriptContent += m_content.ToString();


			scriptContent = await GetRenderedContentAsync(scriptContent);

			if (m_content != null)
				scriptContent += m_content.ToString();



			output.Content.SetHtmlContent(scriptContent);


			//Console.WriteLine("final: ");
			//Utils.ObjectJsonDumper.Dump(m_viewContext.ViewData, 2);

			//output.Content.SetHtmlContent(scriptContent.Replace("//content", String.Join("", list)));

		}


		public static InlineScriptManager GetInstance(ViewContext context, string managerId)
		{
			//Console.WriteLine("getting instance for: " + managerId);
			object instance;

			if (context.ViewData.TryGetValue(managerId, out instance))
			{
				return (InlineScriptManager)instance;
			}
			else
			{
				var inst = new InlineScriptManager(managerId, context);
				context.ViewData.Add(managerId, inst);
				return inst;
			}
		}
	}
}
