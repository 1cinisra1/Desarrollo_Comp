using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GPSMonitoreo.Web
{
    public class QueryValueService
    {
		//SAAM aja
		//Confirmado
        private readonly IHttpContextAccessor _accessor;

        public QueryValueService(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
        }



        public string GetValue()
        {
            return _accessor.HttpContext.Request.Query["value"];
        }
    }

    public class MyApplicationModelConvention : Microsoft.AspNetCore.Mvc.ApplicationModels.IApplicationModelConvention
	{
		

		public void Apply(Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel application)
		{
			//Console.WriteLine("apply");
			Console.WriteLine("Filters -------------------");
			foreach (var filter in application.Filters)
			{
				Console.WriteLine(filter.ToString());

			}

			

			Console.WriteLine("Controllers -------------------");


			foreach (var controller in application.Controllers)
			{
				
				Console.WriteLine(controller.ControllerType.ToString());
				//if (Routes.ContainsKey(controller.ControllerType))
				//{
				//	var typedRoutes = Routes[controller.ControllerType];
				//	foreach (var route in typedRoutes)
				//	{
				//		var action = controller.Actions.FirstOrDefault(x => x.ActionMethod == route.ActionMember);
				//		if (action != null)
				//		{
				//			action.AttributeRouteModel = route;
				//			foreach (var method in route.HttpMethods)
				//			{
				//				action.HttpMethods.Add(method);
				//			}
				//		}
				//	}
				//}
			}
		}
	}

	public class Startup
    {

		public IConfigurationRoot Configuration { get; set; }

		public Startup(IHostingEnvironment env)
		{
			Console.WriteLine("Startup");
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			if (env.IsDevelopment())
			{
				// This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
				//builder.AddApplicationInsightsSettings(developerMode: true);
			}
			Configuration = builder.Build();

			//var xx = new Serilog.Formatting.Json.JsonFormatter();
			
			
			//var formatter2 = new Serilog.Formatting.Compact.RenderedCompactJsonFormatter();
			
			


			Log.Logger = new LoggerConfiguration()
					.MinimumLevel.Debug()
					.Enrich.FromLogContext()
					//.WriteTo.Seq("http://localhost:5341")
					//.WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
					//.WriteTo.JsonConsole(Serilog.Events.LogEventLevel.Debug)
					
					.WriteTo.LiterateConsole(Serilog.Events.LogEventLevel.Debug)
					//.WriteTo.JsonConsole(Serilog.Events.LogEventLevel.Debug)
					
					.CreateLogger();

			//GPSMonitoreo.Dtos.AutoMapperConfiguration.Configure();


			//Console.WriteLine(Configuration.GetConnectionString("DefaultConnection"));


			Libraries.DataAnnotations.ResourceManagerAttribute.ResourceManagers.Add(typeof(GPSMonitoreo.Core.Resources.Entities), GPSMonitoreo.Core.Resources.Entities.ResourceManager);
			Libraries.DataAnnotations.ResourceManagerAttribute.ResourceManagers.Add(typeof(GPSMonitoreo.Core.Resources.PermissionActions), GPSMonitoreo.Core.Resources.PermissionActions.ResourceManager);
			Libraries.DataAnnotations.ResourceManagerAttribute.ResourceManagers.Add(typeof(Resources.Messages), Resources.Messages.ResourceManager);

		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {
			Console.WriteLine("ConfigureServices");
			//System.cu
			//CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-AU");
			var culture = new System.Globalization.CultureInfo("es");
			System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
			System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
			
			///CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-AU");

			//services.AddApplicationInsightsTelemetry(Configuration);

			//services.AddMvc();


			

			//IList<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IModelValidatorProvider> providers = null;



			var mvc = services.AddMvc(options =>
			{
				//options.ModelValidatorProviders.Clear();

				//foreach (var p in options.ModelValidatorProviders)
				//{
				//	Console.WriteLine("ModelValidator Provider: " + p.ToString());
				//}



				//var provider = options.ModelValidatorProviders.OfType<Microsoft.AspNetCore.Mvc.DataAnnotations.Internal.DataAnnotationsModelValidatorProvider>().FirstOrDefault();



				//if (provider != null)
				//	options.ModelValidatorProviders.Remove(provider);


				//var opt = new Microsoft.AspNetCore.Mvc.DataAnnotations.MvcDataAnnotationsLocalizationOptions();

				//var kkk = Microsoft.Extensions.Options.Options.Create(opt);
				//options.ModelValidatorProviders.Add(new Validators.ModelValidatorProvider(new Microsoft.AspNetCore.Mvc.DataAnnotations.Internal.ValidationAttributeAdapterProvider(), kkk, null));

				options.ModelMetadataDetailsProviders.Add(
					new Microsoft.AspNetCore.Mvc.ModelBinding.SuppressChildValidationMetadataProvider(typeof(GPSMonitoreo.Dtos.InputDto))
				);

			});


			

			//services.AddLocalization(options =>
			//{
			//	options.ResourcesPath = "Resources";
			//});

			//mvc.AddDataAnnotationsLocalization();
			
			//LanguageViewLocationExpanderFormat.
			//Console.WriteLine("locale");
			//Console.WriteLine(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.SubFolder);
			//Console.WriteLine(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);



			//Utils.dump(services);
			//foreach (var ser in services)
			//{

			//	Console.WriteLine(ser.ServiceType.ToString());
			//}

			services.Configure< Microsoft.AspNetCore.Mvc.MvcOptions> (options =>
			{
				options.Conventions.Add(new MyApplicationModelConvention());
				Console.WriteLine("Binder Providers---------------------");
				//foreach(var binder in opt.ModelBinderProviders)
				//{
				//	Console.WriteLine(binder.ToString());

				//}

				//foreach(var f in opt.InputFormatters)
				//{
				//	Console.WriteLine(f.ToString());
				//}

				var readerFactory = services.BuildServiceProvider().GetRequiredService<Microsoft.AspNetCore.Mvc.Internal.IHttpRequestStreamReaderFactory>();
				Console.WriteLine(readerFactory);

				//opt.ModelBinderProviders.Add(new ModelBinders.ViewModelBinderProvider());
				options.ModelBinderProviders.Insert(0, new ModelBinders.ViewModelBinderProvider(options.InputFormatters, readerFactory));
				Console.WriteLine("Binder Providers END---------------------");

				//opts.ApplicationModelConventions.Add(new MyApplicationModelConvention());
			});




			//var entitiesSchemaMapping = new System.Collections.Specialized.StringDictionary() { { "MONITOREO", null } };
			//var methodsSchemaMapping = new System.Collections.Specialized.StringDictionary() { { "MONITOREO", "MONITOREO2" } };

			//Console.WriteLine("config: " + Configuration["Entities:EntitiesSchemaMapping"]);

			//Console.WriteLine(Configuration.GetSection("Entities").GetValue(.GetValue<object>("EntitiesSchemaMapping").ToString());
			//Console.WriteLine(Configuration.GetValue<int[]>("Entities:EntitiesSchemaMapping").ToString());
			//var xx = services.Configure<List<string>>(Configuration.GetSection("Entities:EntitiesSchemaMappingss"));
			

			//List<string> result = app.ApplicationServices.GetRequiredService<IOptions<List<ExtraData>>>().Value;

			var configurationSection = Configuration.GetSection("Entities");
			

			Console.WriteLine("-*-------");
			//Console.WriteLine(configurationSection.GetValue<Newtonsoft.Json.Linq.JObject>("EntitiesSchemaMapping").ToString());
			var entitiesSchemaMapping = Configuration.GetSection("Entities:EntitiesSchemaMapping").GetChildren().ToDictionary(a => a.Key, b => b.Value);
			var methodsSchemaMapping = Configuration.GetSection("Entities:MethodsSchemaMapping").GetChildren().ToDictionary(a => a.Key, b => b.Value);

			//Console.WriteLine(xx.Count());
			//Console.WriteLine(Configuration.GetValue<int[]>("Entities:EntitiesSchemaMapping").ToString());

			services.AddScoped((IServiceProvider serviceProvider) => new GPSMonitoreo.Data.Models.EntitiesContext(Configuration["ConnectionStrings:DefaultConnection"], "Models.EntitiesModel", entitiesSchemaMapping, methodsSchemaMapping, true));


			//services.AddTransient<Services.Base.Users.UserService>();
			//services.AddTransient<Services.Geographics.GeoFences.GeoFenceService>();


			//services.AddScoped((_) => new GPSMonitoreo.Data.Models.EntitiesContext(Configuration["ConnectionStrings:DefaultConnection"], true));

			//services.AddTransient<QueryValueService>();
			services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

			


			var baseServiceType = typeof(GPSMonitoreo.Services.BaseService);

			var servicesAssembly = baseServiceType.Assembly;

			foreach(var serviceType in servicesAssembly.GetTypes().Where(x => x.IsClass && x.IsSubclassOf(baseServiceType)))
			{
				Console.WriteLine("adding transient:" + serviceType.ToString());
				services.AddTransient(serviceType);
			}

			//var coreBaseServiceType = typeof(GPSMonitoreo.Core.CoreBaseService);

			//var coreAssembly = coreBaseServiceType.Assembly;

			//foreach (var serviceType in coreAssembly.GetTypes().Where(x => x.IsClass && x.IsSubclassOf(coreBaseServiceType)))
			//{
			//	Console.WriteLine("adding transient:" + serviceType.ToString());
			//	services.AddTransient(serviceType);
			//}





			//services.AddAuthorization(options =>
			//{




			//};





			Console.WriteLine("ConfigureServices ended");
		}

		

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			Console.WriteLine("Configure");
			//var ss = loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			//loggerFactory.AddConsole(LogLevel.Debug);
			//loggerFactory.AddDebug(LogLevel.Debug);

			//loggerFactory.AddDebug();
			loggerFactory.AddSerilog();
			
			

			GPSMonitoreo.Services.BaseService.ServiceProvider = app.ApplicationServices;
			Controllers.BaseController.ServiceProvider = app.ApplicationServices;
			


			//loggerFactory.AddConsole()

			//loggerFactory.AddDebug();
			//var yy = new Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Mvc.DataAnnotations.MvcDataAnnotationsLocalizationOptions>();

			//app.UseApplicationInsightsRequestTelemetry();

			//if (env.IsDevelopment())
			//         {
			//             app.UseDeveloperExceptionPage();
			//         }
			//else
			//{
			//	app.UseExceptionHandler("/Home/Error");
			//}

			//app.
			app.UseDeveloperExceptionPage();
			

			//app.UseApplicationInsightsExceptionTelemetry();

			app.UseStaticFiles(new StaticFileOptions()
			{
				OnPrepareResponse = context =>
				{
					context.Context.Response.Headers.Append("Cache-Control", "no-cache");
				}
			});

			var serverAddresses = (List<string>)app.ServerFeatures.Get<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>().Addresses;


			serverAddresses.Clear();
			serverAddresses.AddRange(Configuration["Server:Urls"].Split(';'));

			Console.WriteLine(Configuration["Server:Urls"]);

			//Utils.dump(serverAddressesFeature.Addresses);


			//serverAddressesFeature.Addresses.Add("http://172.30.1.63:5050");
			//serverAddressesFeature.Addresses.Add("http://192.168.10.63:5050");
			//serverAddressesFeature.Addresses



			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationScheme = "Cookies",
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,
				LoginPath = new PathString("/account/login"),
				Events = new CookieAuthenticationEvents
				{
					OnValidatePrincipal = GPSMonitoreo.Web.Authorization.UpdateValidator.ValidateAsync,

				}

			});





			app.UseMvc(routes =>
			{



				//routes.MapRoute(
				//	name: "areaRoute",
				//	template: "{area:exists}/{controller}/{action=Index}"
				//	);




				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");


				routes.MapRoute(
					name: "geografico",
					template: "geografico/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "geographics",
					template: "geographics/{controller}/{action=Index}/{id?}"
				);


				routes.MapRoute(
					name: "localidades",
					template: "localidades/{controller}/{action=Index}/{id?}"
				);



				routes.MapRoute(
					name: "general",
					template: "general/{controller}/{action=Index}/{id?}"
				);

                routes.MapRoute(
                    name: "equipos",
                    template: "equipos/{controller}/{action=Index}/{id?}"
                );

				routes.MapRoute(
					name: "equipments",
					template: "equipments/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "productos",
					template: "productos/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
                    name: "entidades",
                    template: "entidades/{controller}/{action=Index}/{id?}"
                );

				routes.MapRoute(
					name: "entities",
					template: "entities/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "simulation",
					template: "simulation/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "authorization",
					template: "authorization/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "generalparameters",
					template: "generalparameters/{controller}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "admin",
					template: "admin/{controller}/{action=Index}/{id?}"
				);




				/*
								routes.MapRoute(
									name: "subs",
									template: "sub/{controller=Home}/{action=Index}/{id?}");
								*/

				//routes.MapRoute(
				//	name: "1",
				//	template: "sub/{controller}/{action=Index}/{id?}");

				//routes.MapRoute(
				//	name: "2",
				//	template: "subss/{controller}/{action=Index}/{id?}");


				//var xx = new Microsoft.AspNetCore.Routing.Route()

				foreach (var item in routes.Routes)
				{

					Console.WriteLine(item.ToString());
				}

			});




			MVCHelpers.ViewModels.Validators.Configuration.Configure(options =>
			{
				options.DBContextServiceType = typeof(GPSMonitoreo.Data.Models.EntitiesContext);
				options.DefaultPrimaryKEYName = "ID";
			});

			GPSMonitoreo.Dtos.Validation.Configuration.Configure(options =>
			{
				options.DBContextServiceType = typeof(GPSMonitoreo.Data.Models.EntitiesContext);
				options.DefaultPrimaryKEYName = "ID";
				options.DbContextGetter = () => {
					var ret = app.ApplicationServices.GetService<GPSMonitoreo.Data.Models.EntitiesContext>();
					return ret;
				};
			});


			//app.Run(async (context) =>
			//{
			//    await context.Response.WriteAsync("Hello Worldsss!");
			//});

			//app.Use(next => context => 
			//{
			//	var input = new System.IO.StreamReader(context.Request.Body).ReadToEnd();
			//	Console.WriteLine(input);
			//	return next(context);
			//});
			

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				//Formatting = Formatting.Indented,
				//TypeNameHandling = TypeNameHandling.Objects,
				//DateFormatString = "dd/MM/yyyy HH:mm:ss",
				//ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				//DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
				//DateParseHandling = DateParseHandling.None
				//DateParseHandling = DateParseHandling.None,
				//DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				//Converters.ad
				//ContractResolver = new Newtonsoft.Json.Serialization.


				Converters =
				{
					new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" }
				}
			};

			//Console.WriteLine(JsonConvert.DefaultSettings.ToString());


		}
    }
}
