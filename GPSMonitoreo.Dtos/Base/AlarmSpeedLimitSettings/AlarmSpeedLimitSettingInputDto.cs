using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings
{
    public class AlarmSpeedLimitSettingInputDto : CommonBaseInputDto<short>
    {

		public short SpeedLimitRoad { get; set; }

		public short SpeedLimitRoadRestricted { get; set; }


		[Rules("RequiredNumeric")]
		public short SpeedLimitStandard { get; set; }

		public short SpeedLimitAudible { get; set; }


		[Rules("RequiredNumeric")]
		public short SpeedLimitTolerance { get; set; }


		[Rules("MinCount(1)")]
		public List<SpeedLimitRangeInputDto> LowSpeed { get; set; }


		[Rules("MinCount(1)")]
		public List<SpeedLimitRangeInputDto> HiSpeed { get; set; }

	}
}
