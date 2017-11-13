using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{


	public class Rule
	{
		private string m_namespace;
		public string name;
		public object[] m_parameters;
		public Type attributeType;
		public ValidationAttribute attribute;

		private static List<RuleAssembly> m_assemblies = new List<RuleAssembly>();
		private bool m_hasSpecialParameters;

		private ValidationAttribute _parent;


		

		public static void AddRuleAssembly(TypeInfo typeInfo)
		{
			m_assemblies.Add(new RuleAssembly() { assembly = typeInfo.Assembly, assemblyNamespace = typeInfo.Namespace });
		}

		public static void AddRuleAssembly(Assembly assembly, string @namespace)
		{
			m_assemblies.Add(new RuleAssembly() { assembly = assembly, assemblyNamespace = @namespace});
		}


		public Rule(string rule, ValidationAttribute parent)
		{
			string splittable;
			string remaining = "";
			int pos;

			if ((pos = rule.IndexOf("(")) > 0)
			{
				splittable = rule.Substring(0, pos);
				remaining = rule.Substring(pos);
			}
			else
				splittable = rule;



			var splitted = splittable.Split('.');

			//asdfadf.adfadsf(asdfasdf.asdfadsf.);

			if (splitted.Length > 1)
			{
				m_namespace = string.Join(".", splitted, 0, splitted.Length - 1);
			}

			var attributeName = splitted[splitted.Length - 1];
			var match = Regex.Match(attributeName + remaining, @"^([a-zA-Z0-9_]+)(\((.*)\))?$");

			//Console.WriteLine("count: " + match.Groups.Count.ToString());
			foreach (var str in match.Groups)
			{
				Console.WriteLine("match: " + str);
			}

			if (match.Groups.Count == 4)
			{
				name = match.Groups[1].Value;
				var pars = match.Groups[3].Value;

				if (pars.Length > 0)
				{
					try
					{
						m_parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>($"[{pars}]");
					}
					catch
					{
						throw new RulesAttributeException($"Parameters ({pars}) of Rule({rule}) could not be parsed");
					}
				}
			}
			else
			{
				throw new RulesAttributeException($"Rule ({rule}) could not be parsed");
			}

			//PrepareParameters();



			if (m_namespace == null)
			{
				Console.WriteLine("A");
				attributeType = GetRuleType(name);
			}
			else
			{
				Console.WriteLine("B: " + m_namespace);
				attributeType = GetRuleType(m_namespace, name);
			}

			if (attributeType == null)
			{
				throw new RulesAttributeException($"Rule ({name}) does not exist.");
			}

			Utils.ObjectJsonDumper.Dump(m_parameters, 1);


			//switch(name)
			//{

			//	case "MinLength":
			//	case "MaxLength":
			//	case "Min":
			//	case "GreatherThan"
			//		var i = 0;
			//		foreach (var val in m_parameters)
			//		{
			//			switch (Type.GetTypeCode(val.GetType()))
			//			{
			//				case TypeCode.Int64:
			//					m_parameters[i] = (Int32)(Int64)val;
			//					break;
			//			}
			//			i++;
			//		}
			//		break;
			//}

			if (m_parameters != null)
			{
				var i = 0;
				foreach (var val in m_parameters)
				{

					switch (Type.GetTypeCode(val.GetType()))
					{
						case TypeCode.Int64:
							m_parameters[i] = (Int32)(Int64)val;
							break;
					}
					i++;
				}
			}


			Console.WriteLine("parsed type: ");
			Console.WriteLine(attributeType.ToString());

			attribute = (ValidationAttribute)Activator.CreateInstance(attributeType, m_parameters);

			_parent = parent;
		}

		private void PrepareParameters()
		{
			var x = 0;
			foreach (var parameter in m_parameters)
			{
				if (parameter is string)
				{
					if ((parameter as string).StartsWith("this."))
					{
						var propertyName = (parameter as string).Substring(5);

						Func<object, object> f = (object obj) => {
							var property = obj.GetType().GetProperty(propertyName);
							if (property != null)
							{
								return property.GetValue(obj);
							}

							return null;
						};

						m_parameters[x] = f;
						m_hasSpecialParameters = true;
						//Console.WriteLine("dumping");
						//Utils.dump(m_parameters);
					}
				}
				x++;
			}
		}

		public object[] GetParameters(object instance)
		{
			if (!m_hasSpecialParameters)
				return m_parameters;
			else
			{
				var pars = new object[m_parameters.Length];
				int x = 0;
				foreach (var parameter in m_parameters)
				{
					if (parameter is Func<object, object>)
					{
						pars[x] = (parameter as Func<object, object>).Invoke(instance);
					}
					else
						pars[x] = parameter;

					x++;

				}

				return pars;
			}
		}

		//public Type GetRuleType(string qualifiedName, string namespaceSuffix = "")
		//{
		//	Type type;

		//	if(!qualifiedName.EndsWith("Attribute"))
		//	{
		//		type = Type.GetType(qualifiedName + "Attribute" + namespaceSuffix);
		//		if (type != null && typeof(ValidationAttribute).IsAssignableFrom(type))
		//			return type;
		//	}


		//	type = Type.GetType(qualifiedName + namespaceSuffix);
		//	if (type != null && typeof(ValidationAttribute).IsAssignableFrom(type))
		//		return type;

		//	return null;
		//}

		public Type GetRuleType(string assemblyName, string name)
		{
			var assembly_name = new AssemblyName(assemblyName);
			var assembly = assembly_name.GetType().GetTypeInfo().Assembly;

			Type type = null;

			if (!name.EndsWith("Attribute"))
			{
				type = GetRuleType(assembly.GetType(name + "Attribute"));
				if (type != null)
					return type;
			}

			type = GetRuleType(assembly.GetType(name));

			return type;
		}

		public Type GetRuleType(Type type)
		{
			Console.WriteLine(type == null ? "type es null" : "type no es null");
			if (type != null && typeof(ValidationAttribute).IsAssignableFrom(type))
				return type;
			return null;
		}

		public Type GetRuleType(string name)
		{
			Console.WriteLine("count: " + m_assemblies.Count);

			Console.WriteLine("getting type for: " + name);

			Type type = null;
			foreach (var assembly in m_assemblies)
			{
				Console.WriteLine("Probando assembly: " + assembly.assembly.FullName);
				if (!name.EndsWith("Attribute"))
				{
					Console.WriteLine("probando con attribute: " + name + "Attribute");
					type = assembly.GetRuleType(name + "Attribute");
					if (type != null)
						return type;
				}

				type = assembly.GetRuleType(name);
				if (type != null)
					return type;
			}

			if (type == null)
			{
				//Console.WriteLine("name: " + name);
			}

			return type;
		}
		public ValidationResult GetValidationResult(object value, ValidationContext validationContext)
		{
			if(attribute.ErrorMessageResourceType == null && attribute.ErrorMessage == null)
			{
				attribute.ErrorMessageResourceType = _parent.ErrorMessageResourceType;
				Console.WriteLine("setting resourcetype from parent");
			}
			return attribute.GetValidationResult(value, validationContext);
		}
	}
}
