using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVCHelpers.ViewModels.Validators
{
    public class RequiredNumericAttribute : ValidationAttribute
    {

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing RequiredNumericAttribute: " + value);

			Int32 parsedVal = 0;

			Convert.ToString(null);

			Int32.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out parsedVal);




			if (parsedVal <= 0)
			{
				var template = this.ErrorMessage ?? this.ErrorMessageString;
				var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value});
				return new ValidationResult(errorMsg);
			}
			else
				return ValidationResult.Success;



		}
	}
}
