using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Core.Authorization
{
    public enum PermissionElementType : Int16
    {
		None = 0,
		

		AlarmFiltersCategories = 2,

		AlarmFiltersAlarms = 3,

		Entity = 1,

		Section = 4
	}
}
