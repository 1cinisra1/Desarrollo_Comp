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



	[HtmlTargetElement("form")]
	[HtmlTargetElement("formsection")]
	public class FormManagerTagHelper : TagHelper
	{


		[ViewContext]
		public ViewContext ViewContext { get; set; }


		[HtmlAttributeName("form-manager")]
		public string ManagerId { get; set; }

		[HtmlAttributeName("form-manager-rendersection")]
		public string RenderSection { get; set; }

		[HtmlAttributeName("form-manager-section")]
		public string AddToSection { get; set; }

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (ManagerId != null)
			{
				//Console.WriteLine("Rendering");
				var managerId = "form-manager-" + ManagerId;
				var manager = FormManager.GetInstance(ViewContext, managerId);

				
				if(AddToSection != null)
				{
					await AddSection(manager, output);
				}
				else if(RenderSection != null)
				{
					await manager.RenderSectionAsync(RenderSection, context, output);
				}


				
				//await manager.RenderAsync(context, output);

				
				




				//await RenderAsync(context, output);
			}
			//else
			//{
			//	if (AddTo != null)
			//	{
			//		await AddAsync(context, output);
			//	}
			//}
		}
		private async Task AddSection(FormManager manager, TagHelperOutput output)
		{
			var contents = await output.GetChildContentAsync();
			var scriptContent = contents.GetContent();
			manager.Add(AddToSection, scriptContent);
			output.SuppressOutput();
		}

	}
}
