using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Extensions.Options;
using System.Reflection;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System.Data.Entity;

namespace GPSMonitoreo.Dtos.Validation
{

	public class ConfigurationOptions
	{

		public Type DBContextServiceType
		{
			get { return DBContextAttribute.DBContextServiceType; }
			set { DBContextAttribute.DBContextServiceType = value; }
		}

		public Func<DbContext> DbContextGetter
		{
			get
			{
				return DBContextAttribute.DbContextGetter;
			}
			set
			{
				DBContextAttribute.DbContextGetter = value;
			}

		}

		public String DefaultPrimaryKEYName
		{
			get { return DBContextAttribute.DefaultPrimaryKEYName; }
			set { DBContextAttribute.DefaultPrimaryKEYName = value; }
		}

		public Type ErrorMessageResourceType
		{

			get { return RulesAttribute.DefaultErrorMessageResourceType; }
			set { RulesAttribute.DefaultErrorMessageResourceType = value; }
		}

		public void AddValidationAttributesAssembly(Assembly assembly, string @namespace)
		{
			Rule.AddRuleAssembly(assembly, @namespace);

		}

		public void AddValidationAttributesAssembly(Type type)
		{
			Rule.AddRuleAssembly(type.GetTypeInfo());
		}

		public ConfigurationOptions()
		{

			AddValidationAttributesAssembly(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute));
			AddValidationAttributesAssembly(typeof(RulesAttribute));
		}



	}


	public class Configuration
    {


		//public static void Configure(ConfigurationOptions options)
		//{
		//	//Console.WriteLine("now do the configuration");
		//}

		public static void Configure(Action<ConfigurationOptions> options)
		{
			var configOptions = new ConfigurationOptions();
			options.Invoke(configOptions);
			//Configure(configOptions);
			
		}
	}
}
