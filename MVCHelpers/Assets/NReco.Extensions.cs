using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace NReco.Linq
{
	public static class Extensions
	{

		public static string ToJsonString<T>(this IQueryable<T> entity)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(entity);
		}


		public static LambdaParser.CompiledExpression CompileExpression(this NReco.Linq.LambdaParser parser, string stringExpression)
		{
			
			var expression = parser.Parse(stringExpression);

			var compiledExpr = new LambdaParser.CompiledExpression() { Parameters = LambdaParser.GetExpressionParameters(expression) };

			var lambdaExpr = Expression.Lambda(expression, compiledExpr.Parameters);
			compiledExpr.Lambda = lambdaExpr.Compile();

			return compiledExpr;
		}

		public static object Evaluate (this LambdaParser.CompiledExpression compiledExpression, Dictionary<string, object> values)
		{
			var valuesList = new List<object>();

			foreach (var paramExpr in compiledExpression.Parameters)
			{
				valuesList.Add(new LambdaParameterWrapper(values[paramExpr.Name]));
			}

			var lambdaRes = compiledExpression.Lambda.DynamicInvoke(valuesList.ToArray());

			if (lambdaRes is NReco.Linq.LambdaParameterWrapper)
				return ((NReco.Linq.LambdaParameterWrapper)lambdaRes).Value;

			return lambdaRes;
		}
	}
}
