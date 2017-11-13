using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class DBListExistAttribute: DBContextAttribute
	{
		private string m_tablename;
		private string m_keyFieldName;

		public DBListExistAttribute(string tablename)
		{
			this.ErrorMessageResourceName = "DBListExist";
			Console.WriteLine("DBKeyExists(string tablename)");
			m_tablename = tablename;
			m_keyFieldName = DefaultPrimaryKEYName;
		}



		public DBListExistAttribute(string tablename, string keyFieldName)
		{
			Console.WriteLine("DBListExistAttribute(string tablename, string keyFieldName)");
			m_tablename = tablename;
			m_keyFieldName = keyFieldName;
		}



		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			this.GetDBConnection(validationContext);

			var valueType = value.GetType();
			var values_str = "";
			int count = 0;

			if(valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
			{
				var list = (IList)value;
				foreach (var item in list)
				{
					values_str += GetPreparedValue(item) + ", ";
				}
				count = list.Count;
			}
			else if(valueType.IsArray)
			{
				var values = (Array)value;
				count = values.Length;

				//Utils.ObjectJsonDumper.Dump(values, 1);
				

				foreach (var val in (Array)value)
				{
					values_str += GetPreparedValue(val) + ", ";
				}

			}

			

			if (values_str != "")
			{
				values_str = values_str.Substring(0, values_str.Length - 2);
				var stmt = $"SELECT COUNT(1) FROM {m_tablename} WHERE {m_keyFieldName} IN ({values_str})";

				if (Count(stmt) == count)
				{
					return ValidationResult.Success;
				}

			}


			var template = this.ErrorMessage ?? this.ErrorMessageString;
			var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_tablename, m_keyFieldName });
			return new ValidationResult(errorMsg);
		}
	}
}
