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
    [HtmlTargetElement("script", Attributes = "inline-script-add")]
    public class InlineScriptConcatenatorTagHelper : TagHelper
    {
        //private IHttpContextAccessor httpContextAccessor;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        //public InlineScriptConcatenatorTagHelper(IHttpContextAccessor httpContextAccessor)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //    Console.WriteLine("InlineScriptConcatenatorTagHelper constructed");
            
        //}

		public InlineScriptConcatenatorTagHelper()
		{
			Console.WriteLine("InlineScriptConcatenatorTagHelper()");
		}


        [HtmlAttributeName("inline-script-add")]
        public string BundleName { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Get the script contents


            //context.Items.Add("mykey", "asdfasdf");

            var key = "inline-script-" + BundleName;


            //var contents = await context.GetChildContentAsync();
            var contents = await output.GetChildContentAsync();
            var scriptContent = contents.GetContent();
            //Console.WriteLine("processing:" + scriptContent);

            //ViewContext.ViewData.

            List<string> list;

            if(ViewContext.ViewData.ContainsKey(key))
            {
                list = (List<string>)ViewContext.ViewData[key];
            }
            else
            {
                list = new List<string>();
                ViewContext.ViewData.Add(key, list);
            }

            list.Add(scriptContent);

            //Utils.dump(list);
            

            //Save them into the http Context
            //if (httpContextAccessor.HttpContext.Items.ContainsKey(BundleName))
            //{
            //    var scripts = httpContextAccessor.HttpContext.Items[BundleName] as ICollection<string>;
            //    scripts.Add(scriptContent);
            //}
            //else
            //{
            //    httpContextAccessor.HttpContext.Items[BundleName] = new List<string> { scriptContent };
            //}

            //suppress any output
            output.SuppressOutput();
        }
    }
}
