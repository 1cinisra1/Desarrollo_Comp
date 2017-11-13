using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Validation
{
    public class InputCollectionValidationError: InputValidationError
	{
		public List<InputCollectionItemValidationError> ItemsErrors;

		public InputCollectionValidationError()
		{
			ItemsErrors = new List<InputCollectionItemValidationError>();
		}

		public void AddItemError(int collectionIndex, InputValidationErrors errors)
		{
			ItemsErrors.Add(new InputCollectionItemValidationError()
			{
				Index = collectionIndex,
				Errors = errors
			});
		}

		public void AddItemError(int collectionIndex, string errorMessage)
		{
			ItemsErrors.Add(new InputCollectionItemValidationError()
			{
				Index = collectionIndex,
				Error = errorMessage
			});
		}
	}

	public class InputCollectionItemValidationError: InputValidationError
	{
		public int Index;
		public InputValidationErrors Errors;
	}
}
