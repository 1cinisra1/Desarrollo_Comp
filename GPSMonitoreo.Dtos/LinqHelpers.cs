using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos
{
    public class LinqHelpers
    {

		public class ParameterReplaceVisitor : ExpressionVisitor
		{
			private readonly ParameterExpression from, to;
			public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
			{
				this.from = from;
				this.to = to;
			}
			protected override Expression VisitParameter(ParameterExpression node)
			{
				Console.WriteLine("node:" + node.ToString());
				Console.WriteLine("from:" + from.ToString());
				Console.WriteLine("to:" + to.ToString());

				return node == from ? to : base.VisitParameter(node);
			}
		}

		public class ParameterReplaceVisitor2 : ExpressionVisitor
		{
			private readonly ParameterExpression from, to;
			public ParameterReplaceVisitor2(ParameterExpression from, ParameterExpression to)
			{
				this.from = from;
				this.to = to;
			}
			protected override Expression VisitParameter(ParameterExpression node)
			{
				Console.WriteLine("node:" + node.ToString());
				Console.WriteLine("from:" + from.ToString());
				Console.WriteLine("to:" + to.ToString());

				return node == to ? base.VisitParameter(node) :  from;
				//return from;
			}
		}

		public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(params Expression<Func<TSource, TDestination>>[] selectors)
		{

			var zeroth = ((MemberInitExpression)selectors[0].Body);
			var param = selectors[0].Parameters[0];
			List<MemberBinding> bindings = new List<MemberBinding>(zeroth.Bindings.OfType<MemberAssignment>());
			for (int i = 1; i < selectors.Length; i++)
			{
				var memberInit = (MemberInitExpression)selectors[i].Body;
				var replace = new ParameterReplaceVisitor(selectors[i].Parameters[0], param);
				foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
				{
					bindings.Add(Expression.Bind(binding.Member,
						replace.VisitAndConvert(binding.Expression, "Combine")));
				}
			}

			return Expression.Lambda<Func<TSource, TDestination>>(
				Expression.MemberInit(zeroth.NewExpression, bindings), param);
		}

		public static Expression<Func<TSource, TLeft>> Combine2<TSource, TLeft, TRight>(Expression<Func<TSource, TLeft>> left, Expression<Func<TSource, TRight>> right)
		{


			var zeroth = ((MemberInitExpression)left.Body);

			var leftBody = ((MemberInitExpression)left.Body);
			var rightBody = ((MemberInitExpression)right.Body);

			var param = right.Parameters[0];

			
			

			var memberInit = (MemberInitExpression)right.Body;
			var replace = new ParameterReplaceVisitor(left.Parameters[0], param);

			//List<MemberBinding> bindings = new List<MemberBinding>(zeroth.Bindings.OfType<MemberAssignment>());

			//foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
			//{
			//	bindings.Add(Expression.Bind(binding.Member,
			//		replace.VisitAndConvert(binding.Expression, "Combine")));
			//}


			List<MemberBinding> bindings = new List<MemberBinding>(rightBody.Bindings.OfType<MemberAssignment>());

			foreach (var binding in leftBody.Bindings.OfType<MemberAssignment>())
			{
				//Console.WriteLine("binding member name: " + binding.Member.Name);
				//Console.WriteLine("binding expression: " + binding.Expression);

				//var meth = (MethodCallExpression)binding.Expression;
				bindings.Add(Expression.Bind(binding.Member, replace.VisitAndConvert(binding.Expression, "Combine")));
			}

			
			


			return Expression.Lambda<Func<TSource, TLeft>>(
				Expression.MemberInit(zeroth.NewExpression, bindings), param);
		}

	}
}
