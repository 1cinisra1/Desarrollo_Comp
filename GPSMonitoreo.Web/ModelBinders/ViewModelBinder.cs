using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using System.IO;
using System.Text;
using System.Reflection;

namespace GPSMonitoreo.Web.ModelBinders
{
	public class ViewModelBinderProvider : IModelBinderProvider
	{
		private readonly IList<IInputFormatter> _formatters;
		private readonly IHttpRequestStreamReaderFactory _readerFactory;

		public ViewModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
		{
			Console.WriteLine("ViewModelBinderProvider constructed");
			if (formatters == null)
			{
				throw new ArgumentNullException(nameof(formatters));
			}

			if (readerFactory == null)
			{
				throw new ArgumentNullException(nameof(readerFactory));
			}

			_formatters = formatters;
			_readerFactory = readerFactory;
		}
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			Console.WriteLine("ViewModelBinderProvider GetBinder");
			if (context == null)
			{
				Console.WriteLine("ArgumentNullException");
				throw new ArgumentNullException(nameof(context));
			}


			//Console.WriteLine("binder type:" + context.BindingInfo.BinderType.ToString());
			//Console.WriteLine("binding source:" + context.BindingInfo.BindingSource.ToString());
			//Console.WriteLine("BinderModelName: " + context.BindingInfo.BinderModelName);
			////Console.WriteLine("Container Type: " + context.Metadata.ContainerType.ToString());
			//Console.WriteLine("Model Type: " + context.Metadata.ModelType.ToString());


			//if (context.BindingInfo.BindingSource != null &&
			//	context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))


			//if (context.BindingInfo.BindingSource != null && typeof(PostModels.PostModel).IsAssignableFrom(context.Metadata.ModelType))

			Console.WriteLine(context.Metadata.IsComplexType);
			Console.WriteLine(context.Metadata.ModelType.ToString());

			//if (context.BindingInfo.BindingSource != null && typeof(GPSMonitoreo.Dtos.InputDto).IsAssignableFrom(context.Metadata.ModelType))
			if (context.Metadata.IsComplexType && (typeof(GPSMonitoreo.Dtos.InputDto).IsAssignableFrom(context.Metadata.ModelType) || typeof(PostModels.PostModel).IsAssignableFrom(context.Metadata.ModelType)))
			{
				
				if (_formatters.Count == 0)
				{
					Console.WriteLine("NO FORMATTERS");
					//throw new InvalidOperationException(Resources.FormatInputFormattersAreRequired(
					//	typeof(MvcOptions).FullName,
					//	nameof(MvcOptions.InputFormatters),
					//	typeof(IInputFormatter).FullName));
				}
				Console.WriteLine("ViewModelBinderProvider returning binder");
				return new ViewModelBinder(_formatters, _readerFactory);
			}
			else
			{
				Console.WriteLine("binding source == null or not assignable");
				//if(context.BindingInfo.BindingSource == null)
				//	Console.WriteLine("context.BindingInfo.BindingSource is null");
				//else
				//{
				//	if(!context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
				//		Console.WriteLine("CanAcceptDataFrom failed");
				//}
			}

			return null;
		}
	}


	public class ViewModelBinder : IModelBinder
	{
		private readonly IList<IInputFormatter> _formatters;
		private readonly Func<Stream, Encoding, TextReader> _readerFactory;
		public ViewModelBinder(IList<IInputFormatter> formatters, Microsoft.AspNetCore.Mvc.Internal.IHttpRequestStreamReaderFactory readerFactory)
		{
			Console.WriteLine("ViewModelBinder constructed");
			if (formatters == null)
			{
				throw new ArgumentNullException(nameof(formatters));
			}

			if (readerFactory == null)
			{
				throw new ArgumentNullException(nameof(readerFactory));
			}

			_formatters = formatters;
			_readerFactory = readerFactory.CreateReader;
			Console.WriteLine("ViewModelBinder constructed finished");
		}


		//public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
		//{
		//	var binder = new BodyModelBinder();
		//	bindingContext.BindingSource = BindingSource.Body;
		//	var result = binder.BindModelAsync(bindingContext);

		//	//Console.WriteLine((result.Result.Model == null).ToString());
		//	//Classes.Utils.dump(result.Result.Model);

		//	result.Wait();

		//	if (result.Result.IsModelSet)
		//	{
		//		var instance = (ViewModels.ViewModelCleanable)result.Result.Model;
		//		instance.clean();

		//	}

		//	//ViewModels.ComprobantesBusqueda busqueda = (ViewModels.ComprobantesBusqueda)result.Result.Model;

		//	//busqueda.dummy = "fabian von romberg romero";


		//	//return result.Result;
		//	//return Task.FromResult<ModelBindingResult>(result.Result);
		//	//return base.BindModelAsync(bindingContext);
		//	return result;
		//}
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			Console.WriteLine("binding");
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			// Special logic for body, treat the model name as string.Empty for the top level
			// object, but allow an override via BinderModelName. The purpose of this is to try
			// and be similar to the behavior for POCOs bound via traditional model binding.
			string modelBindingKey;
			if (bindingContext.IsTopLevelObject)
			{
				modelBindingKey = bindingContext.BinderModelName ?? string.Empty;
			}
			else
			{
				modelBindingKey = bindingContext.ModelName;
			}

			var httpContext = bindingContext.HttpContext;

			var formatterContext = new InputFormatterContext(
				httpContext,
				modelBindingKey,
				bindingContext.ModelState,
				bindingContext.ModelMetadata,
				_readerFactory);

			var formatter = (IInputFormatter)null;
			for (var i = 0; i < _formatters.Count; i++)
			{
				if (_formatters[i].CanRead(formatterContext))
				{
					formatter = _formatters[i];
					break;
				}
			}

			if (formatter == null)
			{
				//var message = Resources.FormatUnsupportedContentType(httpContext.Request.ContentType);
				var message = "Unsupported Content Type";

				var exception = new UnsupportedContentTypeException(message);
				bindingContext.ModelState.AddModelError(modelBindingKey, exception, bindingContext.ModelMetadata);
				return;
			}

			//Console.WriteLine("vamos bien");
			//var m = new System.IO.StreamReader(formatterContext.HttpContext.Request.Body);



			////formatterContext.HttpContext.Request.Body.Position = 0;
			//Console.WriteLine("2,,");
			//Console.WriteLine(m.ReadToEnd());
			//Console.WriteLine("2 end");

			try
			{

				//Console.WriteLine(formatterContext.HttpContext.Request.Body.ToString());
				//Console.WriteLine("1");


				//Console.WriteLine(formatterContext.HttpContext.Request.Body.Length);

				//var input = new StreamReader(formatterContext.HttpContext.Request.Body).ReadToEnd();


				//Console.WriteLine("3");


				//Console.WriteLine("::" + input);


				var previousCount = bindingContext.ModelState.ErrorCount;
				var result = await formatter.ReadAsync(formatterContext);
				var model = result.Model;

				if (result.HasError)
				{
					// Formatter encountered an error. Do not use the model it returned.
					Console.WriteLine("Formatter encountered an error. Do not use the model it returned");
					return;
				}

				if(result.Model is PostModels.PostModel)
				{
					Console.WriteLine("IS PostModel");
					((PostModels.PostModel)result.Model).OnBindingFinished();
				}
				else
				{
					Console.WriteLine("IS INPUTDTO");
					((Dtos.InputDto)result.Model).OnBindingFinished();
				}

				
				
				bindingContext.Result = ModelBindingResult.Success(model);
				return;
			}
			catch (Exception ex)
			{
				bindingContext.ModelState.AddModelError(modelBindingKey, ex, bindingContext.ModelMetadata);
				return;
			}
		}
	}
}
