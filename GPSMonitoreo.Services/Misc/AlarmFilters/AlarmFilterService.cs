
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using GPSMonitoreo.Services.Base.RequiredPermissions;
using GPSMonitoreo.Dtos.Misc.AlarmFilters;
using GPSMonitoreo.Core.Authorization;
using GPSMonitoreo.Services.Resources;

namespace GPSMonitoreo.Services.Misc.AlarmFilters
{
    public class AlarmFilterService : BaseService
	{

		RequiredPermissionService _requiredPermissionService;

		public AlarmFilterService(RequiredPermissionService requiredPermissionService)
		{
			_requiredPermissionService = requiredPermissionService;
		}

		public async Task<GetResult<AlarmFilterInputDto>> GetAsync()
		{
			var result = new GetResult<AlarmFilterInputDto>();

			var item = new AlarmFilterInputDto();

			var permissionsResult = await _requiredPermissionService.GetListAsync(new List<PermissionElementType> { PermissionElementType.AlarmFiltersCategories, PermissionElementType.AlarmFiltersAlarms });

			item.RequiredPermissions = permissionsResult.Items;

			result.Item = item;

			result.SetSuccess();

			return result;
		}

		public async Task<OperationResult> UpdateAsync(AlarmFilterInputDto input)
		{

			var result = new OperationResult();

			var elementTypes = new List<PermissionElementType>
			{
				PermissionElementType.AlarmFiltersCategories,
				PermissionElementType.AlarmFiltersAlarms
			};

			var requiredPermissionResult = await _requiredPermissionService.UpdateAsync(elementTypes, input.RequiredPermissions);

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

			return result;
		}


	}
}
