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
    [HtmlTargetElement("script", Attributes = "inline-script-render")]
    public class InlineScriptTagHelper : TagHelper
    {

        [ViewContext]
        public ViewContext ViewContext { get; set; }


        //private IHttpContextAccessor httpContextAccessor;

        //public InlineScriptTagHelper(IHttpContextAccessor httpContextAccessor)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //    Console.WriteLine("InlineScriptTagHelper constructed");
        //}

		public InlineScriptTagHelper()
		{
			Console.WriteLine("InlineScriptTagHelper()");

		}

        [HtmlAttributeName("inline-script-render")]
        public string BundleName { get; set; }

        [HtmlAttributeName("inline-script-wrap")]
        public string Wrap { get; set; }

		
		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			//if no scripts were added, suppress the contents

			var key = "inline-script-" + BundleName;

			if (ViewContext.ViewData.ContainsKey(key))
			{
				
				
				var contents = await output.GetChildContentAsync();
				var scriptContent = contents.GetContent();

				var list = (List<string>)ViewContext.ViewData[key];

				output.Content.SetHtmlContent(scriptContent.Replace("//content", String.Join("", list)));
				//output.Content.SetHtmlContent("asdf");


				//var list = (List<string>)ViewContext.ViewData[key];
				//if (Wrap == null)
				//	output.Content.SetHtmlContent(String.Join("", list) + scriptContent);
				//else
				//{
				//	output.Content.SetHtmlContent(string.Format(Wrap, String.Join("", list) + scriptContent));
				//}
			}
			else
			{
				output.SuppressOutput();
			}

			/*

			if (!httpContextAccessor.HttpContext.Items.ContainsKey(BundleName))
			{
				Console.WriteLine("NO ADDED");
				output.SuppressOutput();
				return;
			}

			//Otherwise get all the scripts for the bundle 

			var scripts = httpContextAccessor.HttpContext.Items[BundleName] as ICollection<string>;

			//Concatenate all of them as set them as the contents of this tag
			//output.Content.SetContentEncoded(String.Join("", scripts));
			//output.Content.SetContent(String.Join("", scripts));
			output.Content.SetHtmlContent(String.Join("", scripts));
			*/




			//return base.ProcessAsync(context, output);
		}

		//public override void Process(TagHelperContext context, TagHelperOutput output)
		//      {
		//          //if no scripts were added, suppress the contents

		//          var key = "inline-script-" + BundleName;

		//          if(ViewContext.ViewData.ContainsKey(key))
		//          {
		//		output.GetChildContentAsync
		//		var contents = await output.GetChildContentAsync();
		//		var scriptContent = contents.GetContent();


		//		var list = (List<string>)ViewContext.ViewData[key];
		//		if (Wrap == null)
		//			output.Content.SetHtmlContent(String.Join("", list));
		//		else
		//		{
		//			output.Content.SetHtmlContent(string.Format(Wrap, String.Join("", list)));
		//		}
		//	}
		//          else
		//          {
		//              output.SuppressOutput();
		//          }

		//          /*

		//          if (!httpContextAccessor.HttpContext.Items.ContainsKey(BundleName))
		//          {
		//              Console.WriteLine("NO ADDED");
		//              output.SuppressOutput();
		//              return;
		//          }

		//          //Otherwise get all the scripts for the bundle 

		//          var scripts = httpContextAccessor.HttpContext.Items[BundleName] as ICollection<string>;

		//          //Concatenate all of them as set them as the contents of this tag
		//          //output.Content.SetContentEncoded(String.Join("", scripts));
		//          //output.Content.SetContent(String.Join("", scripts));
		//          output.Content.SetHtmlContent(String.Join("", scripts));
		//          */
		//      }
	}
}
