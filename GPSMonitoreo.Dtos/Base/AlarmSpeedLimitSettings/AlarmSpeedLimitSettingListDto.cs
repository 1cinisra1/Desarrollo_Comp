using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings
{
    public class AlarmSpeedLimitSettingListDto : CommonBaseListDto<short>
	{
		public short SpeedLimitStandard { get; set; }

		public short SpeedLimitTolerance { get; set; }

    }
}
