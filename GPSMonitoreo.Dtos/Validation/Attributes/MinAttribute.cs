using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
	public class MinAttribute : ValidationAttribute
	{
		private Int32 m_min;

		public MinAttribute(Int32 min)
		{
			m_min = min;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing MinAttribute: " + value);

			decimal retNum;

			

			if (Decimal.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum))
			{
				if (retNum >= m_min)
					return ValidationResult.Success;
			}


			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_min });
			return new ValidationResult(errorMsg);
		}
	}
}
