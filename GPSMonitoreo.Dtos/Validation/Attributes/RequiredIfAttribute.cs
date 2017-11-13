using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NReco.Linq;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class RequiredIfAttribute: RequiredAttribute
	{

		private ExpressionHelper m_expressionHelper;

		public RequiredIfAttribute(string expression)
		{
			m_expressionHelper = new ExpressionHelper(expression);
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Console.WriteLine("executing RequiredIf");

			bool validate;

			if (m_expressionHelper.ParametersLength > 0)
				validate = m_expressionHelper.Evaluate(validationContext.ObjectInstance);
			else
				validate = m_expressionHelper.Evaluate();


			if (validate)
				return base.IsValid(value, validationContext);


			//var values = new Dictionary<string, object>();

			//Utils.ObjectJsonDumper.Dump(m_expression.Parameters, 1);

			//if (m_expression.Parameters.Count() > 0)
			//	values["this"] = validationContext.ObjectInstance;


			//if ((bool)m_expression.Evaluate(values))
			//	return base.IsValid(value, validationContext);

			return ValidationResult.Success;
		}
	}
}
