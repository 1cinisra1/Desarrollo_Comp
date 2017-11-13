using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NReco.Linq;

namespace MVCHelpers.ViewModels.Validators
{
    public class ExecuteIfElseAttribute: RulesContainerAttribute
	{

		private LambdaParser.CompiledExpression m_expression;
		private Rule m_trueRule;
		private Rule m_falseRule;

		public ExecuteIfElseAttribute(string expression, string trueRule, string falseRule)
		{
			var parser = new LambdaParser();
			m_expression = parser.CompileExpression(expression);
			m_trueRule = AddRuleFromString(trueRule);
			m_falseRule = AddRuleFromString(falseRule);
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Console.WriteLine("executing ExecuteIfElseAttribute");

			var values = new Dictionary<string, object>();

			if (m_expression.Parameters.Count() > 0)
			{
				values["this"] = validationContext.ObjectInstance;
			}


			if ((bool)m_expression.Evaluate(values))
			{
				Console.WriteLine("executing ExecuteIfElseAttribute: true case");
				return m_trueRule.GetValidationResult(value, validationContext);
			}
			else
			{
				Console.WriteLine("executing ExecuteIfElseAttribute: false case");
				return m_falseRule.GetValidationResult(value, validationContext);
			}
		}
	}
}
