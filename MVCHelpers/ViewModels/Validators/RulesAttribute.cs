using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MVCHelpers.ViewModels.Validators
{
	public class RulesAttribute : RulesContainerAttribute
	{

		public static Type DefaultErrorMessageResourceType = typeof(Resources.ValidationErrors);


		
		//public Rules(Rule[] rules)

		//public new Type ErrorMessageResourceType { get; set; }





		public RulesAttribute(params string[] rules)
		{
		
			Console.WriteLine("Rules constructor");

			foreach (var rule in rules)
				AddRuleFromString(rule);

			//Console.WriteLine("element: " + this.GetType().get);
			Console.WriteLine("Rules constructor End");
			
		}



		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			//if (this.ErrorMessageResourceType == null)
			//	this.ErrorMessageResourceType = DefaultErrorMessageResourceType;

			ValidationResult result;

			if (ErrorMessageResourceType == null && ErrorMessage == null)
				ErrorMessageResourceType = DefaultErrorMessageResourceType;



			foreach (var rule in m_rules)
			{

				Console.WriteLine("Executing rule: " + rule.attributeType.ToString());
				//Console.WriteLine(this.ErrorMessageResourceType.ToString());
				

				if (typeof(ContinueIfAttribute).IsAssignableFrom(rule.attributeType))
				{
					if (((ContinueIfAttribute)rule.attribute).Continue(value, validationContext))
						continue;
					else
						break;
				}

				result = rule.GetValidationResult(value, validationContext);

				if (result != null)
					return result;
			}

			//object[] parameters;
			//foreach (var rule in m_rules)
			//{
			//	parameters = rule.GetParameters(validationContext.ObjectInstance);
			//	//Utils.dump(parameters);
			//	attribute = Activator.CreateInstance(rule.type, parameters) as ValidationAttribute;

			//	//validationContext.
			//	//attribute.Validate(value, validationContext);
			//	//attribute.IsValid(value);
			//	//Console.WriteLine("error: " + attribute.FormatErrorMessage("descripcion_larga"));
			//	attribute.ErrorMessageResourceType = ErrorMessageResourceType;
			//	attribute.ErrorMessageResourceName = "NotExists";


			//	//attribute.ErrorMessage = "NotExists";
			//	var result = attribute.GetValidationResult(value, validationContext);
			//	Console.WriteLine("error: " + result.ErrorMessage);
			//	return result;

			//}

			return ValidationResult.Success;
		}
	}

	public class NotExistsAttribute : ValidationAttribute
	{
		string m_tableName;
		string m_fieldName;
		public NotExistsAttribute(string tableName, string fieldName)
		{
			Console.WriteLine("validating notexists");
			m_tableName = tableName;
			m_fieldName = fieldName;




		}

		/*
		public new ValidationResult GetValidationResult(object value, ValidationContext validationContext)
		{
			Console.WriteLine("GetValidationResult");
			return null;
		}
		*/


		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Console.WriteLine("validating noexists IsValid: " + value.ToString());



			//Utils.dump(validationContext.ObjectInstance);

			//return new ValidationResult("custom no valid");

			//Utils.dump(validationContext.Items);

			

			//return new ValidationResult("posi error", new[] { validationContext.MemberName });
			//return new ValidationResult(this.ErrorMessageString);
			//String.Format()
			//return new ValidationResult(FormatErrorMessage("error:::: {0}"), new[] { validationContext.MemberName });
			//return new ValidationResult(this.ErrorMessageString);
			return new ValidationResult(string.Format(this.ErrorMessageString, new[] { validationContext.DisplayName, value, m_tableName, m_fieldName }));
			//return new ValidationResult(FormatErrorMessage(validationContext.MemberName), new[] { validationContext.MemberName });
			//return base.IsValid(value, validationContext);

			//return null;
		}
	}


	public class RulesAttributeException : Exception
	{
		public RulesAttributeException() : base()

		{

		}

		public RulesAttributeException(String message) : base(message)

		{

		}


		public RulesAttributeException(String message, Exception innerException) : base(message, innerException)

		{

		}
	}
}
