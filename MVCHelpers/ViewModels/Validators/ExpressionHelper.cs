using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NReco.Linq;

namespace MVCHelpers.ViewModels.Validators
{


    public class ExpressionHelper
    {

		public class ArrayHelper
		{


			//public object GetValue(object value)
			//{

			//	switch(Type.GetTypeCode(value.GetType()))
			//	{
			//		case TypeCode.Byte:
			//		case TypeCode.Decimal:
			//		case TypeCode.Double:
			//		case TypeCode.Int16:
			//		case TypeCode.Int32:
			//		case TypeCode.Int64:
			//			Console.WriteLine("returning converted");
			//			return Convert.ToDecimal(value);

			//		default:
			//			return value;
			//	}
			//}

			public bool Contains(Array arr, object value)
			{
				//Console.WriteLine("Evaluating CONTAINS");
				//Console.WriteLine("Value Type: " + value.GetType().ToString());
				//Utils.ObjectJsonDumper.Dump(arr, 1);
				

				if(arr.Length > 0 )
				{
					//Console.WriteLine("first value: " + ((object[])arr)[0]);
					//Console.WriteLine("second value: " + ((object[])arr)[1]);

					//var elementType = Type.GetTypeCode(arr.GetType().GetElementType().GetElementType());
					var elementType = Type.GetTypeCode(((object[])arr)[0].GetType());
					switch (elementType)
					{
						case TypeCode.Decimal:
							//Console.WriteLine("IsArray of Decimals: " + GetValue(value));

							//return Array.IndexOf(arr, GetValue(value)) > -1;
							return Array.IndexOf(arr, Convert.ToDecimal(value)) > -1;


						case TypeCode.String:
							return Array.IndexOf<string>((string[])arr, (string)value) > -1;

						default:
							break;

					}
				}
				return false;
			}
		}

		private LambdaParser.CompiledExpression m_compiledExpression;

		public ExpressionHelper(string expression)
		{
			var parser = new LambdaParser();
			m_compiledExpression = parser.CompileExpression(expression);


		}



		public int ParametersLength
		{
			get
			{
				return m_compiledExpression.Parameters.Length;
			}
		}

		public bool Evaluate(object thisValue)
		{
			var values = new Dictionary<string, object>() { { "this", thisValue } };
			return Evaluate(values);
		}

		public bool Evaluate(Dictionary<string, object> values = null)
		{

			foreach (var parameter in m_compiledExpression.Parameters)
			{
				switch (parameter.Name)
				{
					case "Array":
						if (values == null)
							values = new Dictionary<string, object>();

						values.Add("Array", new ArrayHelper());

						break;
				}
			}

			return (bool)m_compiledExpression.Evaluate(values);
		}
	}
}
