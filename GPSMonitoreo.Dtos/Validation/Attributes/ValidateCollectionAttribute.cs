using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
	public class ValidateCollectionAttribute : BaseValidationAttribute
	{
		public Type ValidationType { get; set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{


			var index = 0;

			var results = new List<CollectionItemValidationResult>();


			var items = value as IEnumerable;

			if(items != null)
			{
				List<ValidationResult> itemResults;
				ValidationContext itemContext;

				foreach (var item in items)
				{
					itemResults = new List<ValidationResult>();
					itemContext = new ValidationContext(item);

					Validator.TryValidateObject(item, itemContext, itemResults, true);

					if (itemResults.Count > 0)
					{
						results.Add(new CollectionItemValidationResult()
						{
							Index = index,
							Errors = itemResults
						});
					}


					index++;
				}

				if(results.Count > 0)
				{
					var errorMsg = this.GetErrorMessage(validationContext);
					var memberNames = this.GetMemberNames(validationContext);

					return new CollectionValidationResult(errorMsg, memberNames)
					{
						Results = results
					};
				}

			}

			return ValidationResult.Success;
			
		}
	}
}
