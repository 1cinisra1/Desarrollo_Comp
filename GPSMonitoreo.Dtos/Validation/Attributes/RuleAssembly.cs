using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
	public class RuleAssembly
	{
		public Assembly assembly;
		public string assemblyNamespace;

		public Type GetRuleType(string name)
		{
			Type type = assembly.GetType(assemblyNamespace + "." + name);
			if (type != null && typeof(ValidationAttribute).IsAssignableFrom(type))
				return type;
			return null;
		}
	}
}
