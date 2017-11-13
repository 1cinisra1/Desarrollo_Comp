using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using GPSMonitoreo.Web.Classes;

using GPSMonitoreo.Core.Enums;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Libraries.Extensions.EnumExtensions;

namespace GPSMonitoreo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
			Console.WriteLine("Main");


			//Utils.dump(menu.Items);



			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				//.UseConfiguration()
				//.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			

			Console.WriteLine("Run()");
			var enumList = GPSMonitoreo.Libraries.Helpers.EnumHelper.GetLocalizedNames(GPSMonitoreo.Core.Resources.Entities.ResourceManager, typeof(GPSMonitoreo.Core.Enums.Entity));
			Utils.dump(enumList);

			//var xx = Enums.Entity.Geofences;


			//Console.WriteLine(((Enums.Entity)1).ToLocalizedString());
			//Console.WriteLine("::" + Resources.Entities.ResourceManager.GetString("Geofences"));

			//var inst = new Enums.EntityClass();

			//Console.WriteLine("attributes count:" + typeof(Enums.Entity).CustomAttributes.Count());
			//typeof(Enums.Entity).GetCustomAttributes(typeof(GPSMonitoreo.Libraries.DataAnnotations.ResourceManagerAttribute), false);
			//typeof(Enums.Entity).GetCustomAttributes(typeof(GPSMonitoreo.Libraries.DataAnnotations.ResourceManagerAttribute), false);

			//var attrs = typeof(Enums.Entity).GetCustomAttributes(false);
			//var attrs2 = typeof(Enums.Entity).GetCustomAttributes(false);
			//var attrs2 = inst.GetType().GetCustomAttributes(false);

			//Console.WriteLine("count: " + GPSMonitoreo.Libraries.DataAnnotations.ResourceManagerAttribute.ResourceManagers.Count);

			//Console.WriteLine(xx.ToLocalizedString());



			//Console.WriteLine("attrs count: " + attrs.Length);

			host.Run();



		}
    }
}
