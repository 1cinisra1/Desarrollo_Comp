using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class RequiredNumericAttribute : ValidationAttribute
    {

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing RequiredNumericAttribute: " + value);

			Int32 parsedVal = 0;

			Int32.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out parsedVal);




			if (parsedVal <= 0)
			{
				string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;

				var template = this.ErrorMessage ?? this.ErrorMessageString;

				var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value});

				return new ValidationResult(errorMsg, memberNames);
			}
			else
				return ValidationResult.Success;



		}
	}
}
