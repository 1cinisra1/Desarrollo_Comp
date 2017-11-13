using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings
{
    public class TimeRangeSettingInputDto : InputDto
    {
		public short From { get; set; }

		public short To { get; set; }

		public short Points { get; set; }

		public byte GradeId { get; set; }

		public bool Blinking { get; set; }

		public bool Audible { get; set; }


    }
}
