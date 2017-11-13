using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web
{
    public static class MailSender
    {
		public static void Send(MailerOptions options)
		{
			var mailMessage = new MailMessage();

			mailMessage.From = new MailAddress(options.SenderEmailAddress);

			mailMessage.To.Add(string.Join(";", options.To));

			mailMessage.Subject = options.Subject;

			mailMessage.Body = options.BodyHtml;

			mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

			mailMessage.IsBodyHtml = true;


			//SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
			//SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
			//172.30.1.48
			SmtpClient SmtpServer = new SmtpClient("190.10.208.138");
			//
			SmtpServer.Port = 587;
			//SmtpServer.EnableSsl = true;
			//SmtpServer.EnableSsl = true;
			//SmtpServer.UseDefaultCredentials = false;
			//SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			SmtpServer.Credentials = new System.Net.NetworkCredential("fvonromberg", "vnE19MXW");
			
			
			SmtpServer.Send(mailMessage);
			//SmtpServer.SendMailAsync(mailMessage);

			//this._emailSender.SendAsync(mailMessage, false);
		}

		public static void SendTemplate(MailerTemplateOptions options)
		{
			var parsed = ParseTemplate(options);

			options.BodyHtml = parsed;

			Send(options);
		}


		public static string ParseTemplate(MailerTemplateOptions options)
		{
			var re = new Regex(@"(\{([a-zA-Z0-9_]+)\})", RegexOptions.Compiled);

			var parsedHtml = re.Replace(options.TemplateHtml, match =>
			{
				return options.TemplateValues[match.Groups[2].Value];
			});

			return parsedHtml;
		}
    }
}
