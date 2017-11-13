using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using GPSMonitoreo.Web.Infrastructure;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Controllers
{
    public class AdminBaseController : BaseController
    {

		protected async Task<JsonResponse> CommonSave<TId>(BaseCreateUpdateOperation<TId> operation)
		{
			await operation.RunAsync();

			if (operation.ValidationResult.Succeeded)
			{
				if (operation.Result.Succeeded)
				{
					return JsonNotification(operation.Result.Message, new { id = operation.Result.Id });
				}
				else
				{
					Utils.dump(operation);
					return null;
				}
			}
			else
			{
				return JsonFormErrors(operation.ValidationResult.ValidationErrors);
			}
		}

		protected JsonResponse GetEditDataResultJsonResponse(Services.OperationResult result)
		{
			if (result.Succeeded)
			{
				var iResult = result as Services.IGetResult<object>;

				return JsonRecord(iResult.Item);
			}
			else
			{
				return null;
			}
		}

		protected JsonResponse GetListResultJsonResponse(Services.OperationResult result)
		{
			if (result.Succeeded)
			{
				var iResult = result as Services.IGetListResult<IEnumerable<object>>;

				return JsonRecords(iResult.Items, iResult.RecordCount);
			}
			else
			{
				return null;
			}
		}

	}
}
