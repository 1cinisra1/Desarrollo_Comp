using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NReco.Linq;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class ContinueIfAttribute: ValidationAttribute
    {
		//private LambdaParser.CompiledExpression m_expression;

		private ExpressionHelper m_expressionHelper;

		public ContinueIfAttribute(string expression)
		{
			//var parser = new LambdaParser();
			//m_expression = parser.CompileExpression(expression);
			m_expressionHelper = new ExpressionHelper(expression);
		}

		public bool Continue(object value, ValidationContext validationContext)
		{
			Console.WriteLine("executing ContinueIfAttribute");


			//Utils.ObjectJsonDumper.Dump(m_expression.Parameters, 1);


			if (m_expressionHelper.ParametersLength > 0)
				return m_expressionHelper.Evaluate(validationContext.ObjectInstance);
			else
				return m_expressionHelper.Evaluate();


			//if ((bool)m_expression.Evaluate(values))
			//	return true;
			//else
			//	return false;
		}
	}
}
