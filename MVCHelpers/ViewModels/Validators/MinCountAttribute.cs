using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace MVCHelpers.ViewModels.Validators
{
	public class MinCountAttribute : ValidationAttribute
	{
		private Int32 m_min;

		public MinCountAttribute(Int32 min)
		{
			this.ErrorMessageResourceName = "MinCount";
			m_min = min;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing MinCountAttributeXXXX: " + value);

			int count = 0;

			var valueType = value.GetType();

			if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
			{
				count = ((IList)value).Count;
			}
			else if (valueType.IsArray)
			{
				count = ((Array)value).Length;
			}

			if(count >= m_min)
				return ValidationResult.Success;

			
			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_min });
			return new ValidationResult(errorMsg);
		}
	}
}
