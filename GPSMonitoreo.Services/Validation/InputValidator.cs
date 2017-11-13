using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Validation;

namespace GPSMonitoreo.Services.Validation
{
    public static class InputValidator
    {



		private static void AddError(InputValidationErrors errors, ValidationResult result)
		{
			var resultType = result.GetType();

			if(resultType == typeof(ValidationResult))
			{
				errors.AddError(result.MemberNames.First(), result.ErrorMessage);
			}
			else if(resultType == typeof(CollectionValidationResult))
			{
				var collectionValidationResult = result as CollectionValidationResult;

				var inputCollectionValidationError = new InputCollectionValidationError();
				inputCollectionValidationError.Error = result.ErrorMessage;

				



				InputValidationErrors itemInputValidationErrors;

				foreach (var collectionItemValidationResult in collectionValidationResult.Results)
				{
					itemInputValidationErrors = new InputValidationErrors();

					foreach(var itemValidationResult in collectionItemValidationResult.Errors)
					{
						AddError(itemInputValidationErrors, itemValidationResult);
					}

					inputCollectionValidationError.AddItemError(collectionItemValidationResult.Index, itemInputValidationErrors);


					//inputCollectionValidationError.ItemsErrors.Add(new InputCollectionItemValidationError()
					//{
					//	Index = collectionItemValidationResult.Index,
					//	Errors = itemInputValidationErrors
					//});



					//inputCollectionValidationError = new InputCollectionValidationError();

					//inputCollectionValidationError.i







				}

				errors.AddCollectionError(result.MemberNames.First(), inputCollectionValidationError);

			}
		}


		public static InputValidationResult Validate(InputDto input, List<Action<InputValidationErrors>> additionalValidators = null)
		{
			var ret = new InputValidationResult();
			var errors = new InputValidationErrors();

			var results = new List<ValidationResult>();


			var validationContext = new ValidationContext(input);
			

			//System.ComponentModel.DataAnnotations.Validator.TryValidateObject()

			Validator.TryValidateObject(input, validationContext, results, true);

			//Console.WriteLine("list errors:");

			//Utils.ObjectJsonDumper.Dump(results, 20);

			foreach(var validationResult in results)
			{
				AddError(errors, validationResult);
			}

			if(additionalValidators != null)
			{
				foreach(var additionalValidator in additionalValidators)
				{
					additionalValidator(errors);
				}
			}

			if(errors.Count == 0)
			{
				ret.Succeeded = true;
			}
			else
			{
				ret.ValidationErrors = errors;
			}

			return ret;

		}

	}
}
