using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings
{
    public class SpeedLimitRangeInputDto : InputDto
    {
		public short From { get; set; }

		public short To { get; set; }

		public List<TimeRangeSettingInputDto> TimeRangeSettings { get; set; }
    }
}
