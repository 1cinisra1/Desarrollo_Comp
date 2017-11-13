using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web
{
	public class MailerOptions
	{

		public MailerOptions()
		{

		}


		public string SenderEmailAddress { get; set; }

		public List<string> To { get; set; }

		public string Subject { get; set; }

		public string BodyHtml { get; set; }

		public string BodyText { get; set; }

		public void AddTo(string emailAddress)
		{
			if(To == null)
			{
				To = new List<string>();
			}

			To.Add(emailAddress);
		}
	}
}
