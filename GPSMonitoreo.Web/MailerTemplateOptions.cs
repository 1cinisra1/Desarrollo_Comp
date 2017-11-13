using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web
{
	public class MailerTemplateOptions : MailerOptions
	{
		public string TemplateHtml { get; set; }

		public string TemplateText { get; set; }

		public NameValueCollection TemplateValues { get; set; }

	}
}
