using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Extensions.ObjectExtensions
{
    public static class ObjectExtensions
    {
		public static string PropertyName<TModel, TProperty>(this TModel obj, Expression<Func<TModel, TProperty>> propertyExpression) where TModel : class
		{
			var expression = propertyExpression.Body as MemberExpression;
			if (expression == null)
			{
				throw new ArgumentException("Expression is not a property.");
			}

			return expression.Member.Name;
		}

		//public string PropertyName<TSource, TField>(Expression<Func<TSource, TField>> Field)
		//{
		//	if (object.Equals(Field, null))
		//	{
		//		throw new NullReferenceException("Field is required");
		//	}

		//	MemberExpression expr = null;

		//	if (Field.Body is MemberExpression)
		//	{
		//		expr = (MemberExpression)Field.Body;
		//	}
		//	else if (Field.Body is UnaryExpression)
		//	{
		//		expr = (MemberExpression)((UnaryExpression)Field.Body).Operand;
		//	}
		//	else
		//	{
		//		const string Format = "Expression '{0}' not supported.";
		//		string message = string.Format(Format, Field);

		//		throw new ArgumentException(message, "Field");
		//	}

		//	return expr.Member.Name;
		//}

	}
}
