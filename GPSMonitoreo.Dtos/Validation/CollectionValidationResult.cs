using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Validation
{
    public class CollectionValidationResult: ValidationResult
    {
		public List<CollectionItemValidationResult> Results { get; set; }


		public CollectionValidationResult(string errorMessage) : base(errorMessage)
		{

		}

		public CollectionValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
		{

		}
	}

	public class CollectionItemValidationResult
	{
		public int Index { get; set; }

		public List<ValidationResult> Errors { get; set; }

		public CollectionItemValidationResult()
		{
			Errors = new List<ValidationResult>();
		}
	}

}
