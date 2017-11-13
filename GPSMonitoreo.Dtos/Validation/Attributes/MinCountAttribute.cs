using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
	public class MinCountAttribute : ValidationAttribute
	{
		private Int32 m_min;

		public MinCountAttribute(Int32 min)
		{
			this.ErrorMessageResourceName = "MinCount";
			m_min = min;
		}

		//public override bool IsValid(object value)
		//{
		//	int count = 0;

		//	//validationContext.

		//	if (value != null)
		//	{
		//		var valueType = value.GetType();

		//		if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
		//		{
		//			count = ((IList)value).Count;
		//		}
		//		else if (valueType.IsArray)
		//		{
		//			count = ((Array)value).Length;
		//		}

		//		Console.WriteLine("MinCountAttribute current count: " + count);
		//	}

		//	return count >= m_min;
		//}

		public override string FormatErrorMessage(string name)
		{
			// An error occurred, so we know the value is less than the minimum
			//return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Length);
			return "Error for: " + name;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing MinCountAttributeSSSS: " + value);
			//Console.WriteLine("displayname: " + validationContext.DisplayName);
			//Console.WriteLine("membername: " + validationContext.MemberName);

			int count = 0;

			//validationContext.

			if (value != null)
			{
				var valueType = value.GetType();

				if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
				{
					count = ((IList)value).Count;
				}
				else if (valueType.IsArray)
				{
					count = ((Array)value).Length;
				}

				Console.WriteLine("MinCountAttribute current count: " + count);
			}

			if (count >= m_min)
				return ValidationResult.Success;



			string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;


			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_min });

			return new ValidationResult(errorMsg, memberNames);
		}
	}
}
