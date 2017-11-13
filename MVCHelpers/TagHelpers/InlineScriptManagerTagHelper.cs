using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCHelpers.TagHelpers
{


	[HtmlTargetElement("script")]
	public class InlineScriptManagerTagHelper : TagHelper
	{

		[ViewContext]
		public ViewContext ViewContext { get; set; }


		[HtmlAttributeName("script-manager-add")]
		public string AddTo { get; set; }


		[HtmlAttributeName("script-manager-key")]
		public string Key { get; set; }

		[HtmlAttributeName("script-manager-render")]
		public string RenderId { get; set; }


		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if(RenderId != null)
			{
				//Console.WriteLine("Rendering");
				var managerId = "script-manager-" + RenderId;
				var manager = InlineScriptManager.GetInstance(ViewContext, managerId);
				await manager.RenderAsync(context, output);


				//await RenderAsync(context, output);
			}
			else
			{
				if(AddTo != null)
				{
					await AddAsync(context, output);
				}
			}
		}
		private async Task AddAsync(TagHelperContext context, TagHelperOutput output)
		{
			var managerId = "script-manager-" + AddTo;
			//Console.WriteLine("Script manager add: " + managerId);
			var manager = InlineScriptManager.GetInstance(ViewContext, managerId);

			var contents = await output.GetChildContentAsync();
			var scriptContent = contents.GetContent();

			//Console.WriteLine("content: " + scriptContent);

			//Console.WriteLine("Adding: " + managerId);

			//Console.WriteLine("content: " + scriptContent);

			if (Key == null)
				manager.Add(scriptContent);
			else
				manager.Add(Key, scriptContent);

			output.SuppressOutput();
		}
	}
}
