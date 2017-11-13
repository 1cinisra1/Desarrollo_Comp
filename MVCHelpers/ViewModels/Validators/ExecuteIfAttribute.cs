using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NReco.Linq;


namespace MVCHelpers.ViewModels.Validators
{
    public class ExecuteIfAttribute: RulesContainerAttribute
    {
		private LambdaParser.CompiledExpression m_expression;
		private Rule m_rule;
		public ExecuteIfAttribute(string expression, string rule)
		{
			var parser = new LambdaParser();
			m_expression = parser.CompileExpression(expression);
			m_rule = AddRuleFromString(rule);
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			Console.WriteLine("executing ExecuteIfAttribute");

			var values = new Dictionary<string, object>();

			Utils.ObjectJsonDumper.Dump(m_expression.Parameters, 1);

			Console.WriteLine("parameters:");

			Utils.ObjectJsonDumper.Dump(m_expression.Parameters, 2);

			

			if(m_expression.Parameters.Count() > 0)
			{
				values["this"] = validationContext.ObjectInstance;
			}


			if((bool)m_expression.Evaluate(values))
			{
				Console.WriteLine("executing ExecuteIfAttribute: true");
				return m_rule.GetValidationResult(value, validationContext);
			}

			return ValidationResult.Success;
		}

	}
}
