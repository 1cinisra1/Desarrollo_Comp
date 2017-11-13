using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVCHelpers.ViewModels.Validators
{
    public abstract class DBContextAttribute : ValidationAttribute
    {
		public static Type DBContextServiceType;
		public static string DefaultPrimaryKEYName = "id";

		private System.Data.Entity.Database m_database;

		
		public void GetDBConnection(ValidationContext validationContext)
		{
			//var db = validationContext.ObjectInstance
			//Utils.ObjectJsonDumper.Dump(, 1);

			var ctx = (System.Data.Entity.DbContext)validationContext.GetService(DBContextServiceType);
			
			m_database = ctx.Database;
		}

		public string GetPreparedValue(object value)
		{

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.String:
					return "'" + ((string)value).Replace("'", "''") + "'";

				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.UInt64:
				case TypeCode.Byte:
					return value.ToString();

				case TypeCode.Boolean:
					return (bool)value ? "1" : "0";

					
				case TypeCode.Char:
					break;
			}

			bool x = true;

			x.ToString();


			return "null";
		}

		public int Count(string stmt)
		{
			var qry = m_database.SqlQuery<Int32>(stmt);
			return qry.FirstOrDefault();
		}

		public bool Exists(string tablename, object value, string keyField = "id", string extraWhere = "")
		{
			//Console.WriteLine("value type: " + value.GetType().ToString());
			//CTX_DESCRIPCION_MED
			//var qry = m_database.SqlQuery<Int32>($"SELECT COUNT(1) FROM {tablename} WHERE {keyField} = :param1", new object[] { null });
			//var qry = m_database.SqlQuery<Int32>($"SELECT COUNT(1) FROM {tablename} WHERE CTX_DESCRIPCION_MED = :param1", new object[] { null });
			var stmt = $"SELECT COUNT(1) FROM {tablename} WHERE {keyField} ";

			if (value == null)
			{
				stmt += "IS NULL";
			}
			else
				stmt += "= " + GetPreparedValue(value);


			var qry = m_database.SqlQuery<Int32>(stmt);
			var count = qry.FirstOrDefault();
			Console.WriteLine("Count: " + count);
			if (count > 0)
				return true;
			else
				return false;
		}
    }
}
