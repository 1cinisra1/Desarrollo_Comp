using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Validation
{
    public class InputValidationErrors: Dictionary<string, InputValidationError>
    {
		public void AddError(string propertyName, string errorMessage)
		{
			this.Add(propertyName, new InputValidationError()
			{
				Error = errorMessage
			});
		}

		public void AddCollectionError(string propertyName, InputCollectionValidationError collectioniValidationError)
		{
			this.Add(propertyName, collectioniValidationError);
		}
    }


	public class InputValidationError
	{
		public string Error;
	}
}
