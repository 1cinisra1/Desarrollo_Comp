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
    public class FormManager
    {

		private Dictionary<string, System.Text.StringBuilder> m_dict;


		public void Add(string key, string content)
		{
			if (m_dict == null)
				m_dict = new Dictionary<string, System.Text.StringBuilder>();


			System.Text.StringBuilder builder;

			if (!m_dict.TryGetValue(key, out builder))
			{
				builder = new System.Text.StringBuilder();
				m_dict.Add(key, builder);
			}

			builder.Append(content);
		}


		public static FormManager GetInstance(ViewContext context, string managerId)
		{

			object instance;

			if (context.ViewData.TryGetValue(managerId, out instance))
			{
				return (FormManager)instance;
			}
			else
			{
				var inst = new FormManager();
				context.ViewData.Add(managerId, inst);
				return inst;
			}
		}

		public async Task RenderSectionAsync(string sectionKey, TagHelperContext context, TagHelperOutput output)
		{
			output.SuppressOutput();

			System.Text.StringBuilder content;

			if(m_dict != null && m_dict.TryGetValue(sectionKey, out content))
			{
				output.Content.SetHtmlContent(content.ToString());
			}
		}
	}
}
