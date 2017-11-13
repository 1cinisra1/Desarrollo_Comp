using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Services;
using GPSMonitoreo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GPSMonitoreo.Web.Infrastructure
{

	public class BaseCreateUpdateOperation<TPrimaryKey> /*where TPrimaryKey : struct, IEquatable<TPrimaryKey>, IComparable<TPrimaryKey>, IComparable*/
	{
		public InputValidationResult ValidationResult { get; set; }

		public CreateUpdateResult<TPrimaryKey> Result { get; set; }

		public async virtual Task RunAsync()
		{
			

		}

	}

	public class CreateUpdateOperation<TPrimaryKey, TInputDto>: BaseCreateUpdateOperation<TPrimaryKey> /*where TPrimaryKey : struct, IEquatable<TPrimaryKey>, IComparable<TPrimaryKey>, IComparable*/ where TInputDto : BaseInputDto<TPrimaryKey>
	{

		//public delegate object MyDelegate(MyInput par1);
		//public delegate object MyDelegate2(MyInterface par1);

		//public delegate Task<CreateUpdateResult<int>> MyDelegate3<out T>(BaseInputDto<int> input);

		public delegate Task<CreateUpdateResult<TPrimaryKey>> CreateOrUpdateAsyncDelegate(TInputDto input);

		public delegate Task<InputValidationResult> ValidateAsyncDelegate(TInputDto input);

		public delegate InputValidationResult ValidateDelegate(TInputDto input);

		public ValidateAsyncDelegate ValidateAsync { get; set; }

		public ValidateDelegate Validate { get; set; }

		public CreateOrUpdateAsyncDelegate CreateAsync { get; set; }

		public CreateOrUpdateAsyncDelegate UpdateAsync { get; set; }

		public TInputDto InputDto { get; set; }

		public async override Task RunAsync()
		{
			if (ValidateAsync != null)
			{
				ValidationResult = await ValidateAsync(InputDto);
			}
			else if(Validate != null)
			{
				ValidationResult = Validate(InputDto);
			}

			if (ValidationResult.Succeeded)
			{
				//Console.WriteLine("id: " + InputDto.Id.GetType().ToString());
				//Console.WriteLine("id: " + InputDto.Id);
				//Console.WriteLine("id: " + InputDto.Id.Equals(0).ToString());
				if (InputDto.Id.Equals(default(TPrimaryKey)))
				{
					Console.WriteLine("Running CreateAsync");
					Result = await CreateAsync(InputDto);
				}
				else
				{
					Console.WriteLine("Running UpdateAsync");
					Result = await UpdateAsync(InputDto);
				}
			}
		}



		//public Task CreateAsync { get; set;}

		//public Task<CreateUpdateResult<TPrimaryKey>> CreateAsync { get; set; }



		//public static CreateUpdateOperation<TPrimaryKey> RunAsync()
		//{
		//	Services.Base.Geofences.GeofenceService service;

		//	dynamic aa = service.CreateAsync2;




		//}
	}
}
