using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVCHelpers.ViewModels.Validators
{
    public class GreaterThanAttribute : ValidationAttribute
	{
		private decimal m_greaterThan;

		public GreaterThanAttribute(decimal greaterThan)
		{
			m_greaterThan = greaterThan;
		}

		public GreaterThanAttribute(Int32 greaterThan)
		{
			m_greaterThan = greaterThan;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{

			Console.WriteLine("executing GreaterThanAttribute: " + value);

			decimal parsedVal = 0;


			var isOk = true;


			//Console.WriteLine("parsed: " + parsedVal.ToString());

			if (value != null)
			{
				switch (Type.GetTypeCode(value.GetType()))
				{
					case TypeCode.Decimal:
					case TypeCode.Double:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
					case TypeCode.Byte:
					case TypeCode.SByte:
						//Console.WriteLine("converting:" + value.ToString());
						parsedVal = Convert.ToDecimal(value);
						break;

					case TypeCode.String:
						//Console.WriteLine("converting string:" + value.ToString());
						decimal.TryParse((string)value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo, out parsedVal);
						break;

					default:
						isOk = false;
						break;
				}
			}
			else
				isOk = false;

			Console.WriteLine("parsed: " + parsedVal.ToString());

			if(isOk && parsedVal > m_greaterThan)
				return ValidationResult.Success;
			else
			{
				var template = this.ErrorMessage ?? this.ErrorMessageString;
				var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_greaterThan});
				return new ValidationResult(errorMsg);
			}
		}
	}
}
