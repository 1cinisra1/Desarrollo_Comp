using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public class DBKeyExistsAttribute: DBContextAttribute
	{
		private string m_tablename;
		private string m_keyFieldName;

		public DBKeyExistsAttribute(string tablename)
		{
			this.ErrorMessageResourceName = "DBKeyExists";
			Console.WriteLine("DBKeyExists(string tablename)");
			m_tablename = tablename;
			m_keyFieldName = DefaultPrimaryKEYName;
		}


		public DBKeyExistsAttribute(string tablename, string keyFieldName)
		{
			Console.WriteLine("DBKeyExists(string tablename, string keyFieldName)");
			m_tablename = tablename;
			m_keyFieldName = keyFieldName;
		}



		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			this.GetDBConnection(validationContext);

			Console.WriteLine("executing DBKeyExists: " + value);

			if (Exists(m_tablename, value, m_keyFieldName))
				return ValidationResult.Success;
			else
			{
				string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;

				var template = this.ErrorMessage?? this.ErrorMessageString;
				var errorMsg = string.Format(template, new[] { validationContext.DisplayName, value, m_tablename, m_keyFieldName });
				
				//Console.WriteLine(this.ErrorMessageResourceType.ToString());
				return new ValidationResult(errorMsg, memberNames);
				//return new ValidationResult("Record not found");
			}
		}
	}
}
