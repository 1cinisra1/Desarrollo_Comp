using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GPSMonitoreo.Web.Validators
{
    public class ModelValidatorProvider : IModelValidatorProvider
	{
		private readonly IOptions<MvcDataAnnotationsLocalizationOptions> _options;
		private readonly IStringLocalizerFactory _stringLocalizerFactory;
		private readonly IValidationAttributeAdapterProvider _validationAttributeAdapterProvider;

		/// <summary>
		/// Create a new instance of <see cref="DataAnnotationsModelValidatorProvider"/>.
		/// </summary>
		/// <param name="validationAttributeAdapterProvider">The <see cref="IValidationAttributeAdapterProvider"/>
		/// that supplies <see cref="IAttributeAdapter"/>s.</param>
		/// <param name="options">The <see cref="IOptions{MvcDataAnnotationsLocalizationOptions}"/>.</param>
		/// <param name="stringLocalizerFactory">The <see cref="IStringLocalizerFactory"/>.</param>
		/// <remarks><paramref name="options"/> and <paramref name="stringLocalizerFactory"/>
		/// are nullable only for testing ease.</remarks>
		public ModelValidatorProvider(
			IValidationAttributeAdapterProvider validationAttributeAdapterProvider,
			IOptions<MvcDataAnnotationsLocalizationOptions> options,
			IStringLocalizerFactory stringLocalizerFactory)
		{
			if (validationAttributeAdapterProvider == null)
			{
				throw new ArgumentNullException(nameof(validationAttributeAdapterProvider));
			}
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			_validationAttributeAdapterProvider = validationAttributeAdapterProvider;
			_options = options;
			_stringLocalizerFactory = stringLocalizerFactory;
		}

		public void CreateValidators(ModelValidatorProviderContext context)
		{
			IStringLocalizer stringLocalizer = null;
			if (_stringLocalizerFactory != null && _options.Value.DataAnnotationLocalizerProvider != null)
			{
				stringLocalizer = _options.Value.DataAnnotationLocalizerProvider(
					context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType,
					_stringLocalizerFactory);
			}

			Console.WriteLine("context: " + context.ModelMetadata.PropertyName);

			for (var i = 0; i < context.Results.Count; i++)
			{
				var validatorItem = context.Results[i];
				
				if (validatorItem.Validator != null)
				{
					
					continue;
				}

				var attribute = validatorItem.ValidatorMetadata as ValidationAttribute;
				if (attribute == null)
				{
					continue;
				}

				//var validator = new Microsoft.AspNetCore.Mvc.DataAnnotations.Internal.DataAnnotationsModelValidator(
				//	_validationAttributeAdapterProvider,
				//	attribute,
				//	stringLocalizer);

				var validator = new ModelValidator(
					_validationAttributeAdapterProvider,
					attribute,
					stringLocalizer);

				Console.WriteLine("ATTRIBUTE: " + attribute.ToString());

				validatorItem.Validator = validator;
				validatorItem.IsReusable = true;
				// Inserts validators based on whether or not they are 'required'. We want to run
				// 'required' validators first so that we get the best possible error message.
				if (attribute is RequiredAttribute)
				{
					context.Results.Remove(validatorItem);
					context.Results.Insert(0, validatorItem);
				}
			}

			//Utils.dump(context.Results);

			

			// Produce a validator if the type supports IValidatableObject
			if (typeof(IValidatableObject).IsAssignableFrom(context.ModelMetadata.ModelType))
			{
				context.Results.Add(new ValidatorItem
				{
					Validator = new Microsoft.AspNetCore.Mvc.DataAnnotations.Internal.ValidatableObjectAdapter(),
					IsReusable = true
				});
			}
		}
	}
}

