using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class BaseValidationAttribute: ValidationAttribute
    {

		public string[] GetMemberNames(ValidationContext validationContext)
		{
			return validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
		}

		public string GetErrorMessage(ValidationContext validationContext)
		{
			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName});
			return errorMsg;
		}

		public string GetErrorMessage(ValidationContext validationContext, object value)
		{
			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value });
			return errorMsg;
		}

		public string GetErrorMessage(ValidationContext validationContext, object value, object[] additionalArguments)
		{
			var template = this.ErrorMessage ?? this.ErrorMessageString;

			var arguments = new[] { validationContext.DisplayName, value };
			Array.Resize(ref arguments, arguments.Length + additionalArguments.Length);
			
			additionalArguments.CopyTo(arguments, arguments.Length);

			var errorMsg = string.Format(template, arguments);
			return errorMsg;
		}

		public ValidationResult GetErrorValidationResult(ValidationContext validationContext)
		{
			return new ValidationResult(GetErrorMessage(validationContext));

		}

		public ValidationResult GetErrorValidationResult(ValidationContext validationContext, object value)
		{
			return new ValidationResult(GetErrorMessage(validationContext, value), GetMemberNames(validationContext));

		}

		public ValidationResult GetErrorValidationResult(ValidationContext validationContext, object value, object[] additionalArguments)
		{
			return new ValidationResult(GetErrorMessage(validationContext, value, additionalArguments), GetMemberNames(validationContext));

		}
	}
}
