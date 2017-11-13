using GPSMonitoreo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Extensions.LinqExtensions
{
    public static class LinqExtensions
    {


		public static Expression<Func<TSource, TRight>> Extend<TSource, TLeft, TRight>(this Expression<Func<TSource, TLeft>> left, Expression<Func<TSource, TRight>> right)
		{
			//Console.WriteLine("EXTENDING.........................");
			var leftBody = ((MemberInitExpression)left.Body);
			var rightBody = ((MemberInitExpression)right.Body);

			Console.WriteLine(left.GetType().ToString());

			var param = left.Parameters[0];

			var replace = new LinqHelpers.ParameterReplaceVisitor(right.Parameters[0], param);

			List<MemberBinding> bindings = new List<MemberBinding>(leftBody.Bindings.OfType<MemberAssignment>());

			foreach (var binding in rightBody.Bindings.OfType<MemberAssignment>())
			{
				bindings.Add(Expression.Bind(binding.Member, replace.VisitAndConvert(binding.Expression, "Combine")));
			}

			return Expression.Lambda<Func<TSource, TRight>>(Expression.MemberInit(rightBody.NewExpression, bindings), param);
		}



		public static Expression<Func<TSource, TRight>> Extend2<TSource, TLeft, TRight>(this Expression<Func<TSource, TLeft>> left, Expression<Func<TSource, TRight>> right)
		{
			//Console.WriteLine("EXTENDING.........................");
			var leftBody = ((MemberInitExpression)left.Body);
			var rightBody = ((MemberInitExpression)right.Body);

			//Console.WriteLine(left.GetType().ToString());

			var param = left.Parameters[0];

			var replace = new LinqHelpers.ParameterReplaceVisitor(left.Parameters[0], right.Parameters[0]);

			//List<MemberBinding> bindings = new List<MemberBinding>(leftBody.Bindings.OfType<MemberAssignment>());
			List<MemberBinding> bindings = new List<MemberBinding>();

			foreach (var binding in leftBody.Bindings.OfType<MemberAssignment>())
			{
				bindings.Add(Expression.Bind(binding.Member, replace.VisitAndConvert(binding.Expression, "Combine")));
			}

			bindings.AddRange(rightBody.Bindings.OfType<MemberAssignment>());

			

			return Expression.Lambda<Func<TSource, TRight>>(Expression.MemberInit(rightBody.NewExpression, bindings), right.Parameters[0]);
		}




		public static Expression<Func<TSource, TRight>> Ext2<TSource, TRight>(this LambdaExpression left, Expression<Func<TSource, TRight>> right)
		{
			//Console.WriteLine("EXTENDING.........................");
			var leftBody = ((MemberInitExpression)left.Body);
			var rightBody = ((MemberInitExpression)right.Body);

			//Console.WriteLine(left.GetType().ToString());

			var param = left.Parameters[0];


			//var replace = new LinqHelpers.ParameterReplaceVisitor(right.Parameters[0], param);

			Console.WriteLine("left parameter: " + left.Parameters[0].Type.ToString());
			Console.WriteLine("right parameter: " + right.Parameters[0].Type.ToString());


			var replace = new LinqHelpers.ParameterReplaceVisitor(left.Parameters[0], right.Parameters[0]);

			//List<MemberBinding> bindings = new List<MemberBinding>(rightBody.Bindings.OfType<MemberAssignment>());

			List<MemberBinding> bindings = new List<MemberBinding>();

			foreach (var binding in leftBody.Bindings.OfType<MemberAssignment>())
			{
				bindings.Add(Expression.Bind(binding.Member, replace.VisitAndConvert(binding.Expression, "Combine")));
				//bindings.Insert()
				//bindings.Add(Expression.Bind(binding.Member, binding.Expression));
				//Expression.Bind()
			}

			bindings.AddRange(rightBody.Bindings.OfType<MemberAssignment>());

			return Expression.Lambda<Func<TSource, TRight>>(Expression.MemberInit(rightBody.NewExpression, bindings), right.Parameters[0]);
		}

	}
}
