using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Devices
{
	public class GpsAlarmDto
    {
		
		public DateTime DateTime;

		public int? GeoFenceId;

		public string GeoFenceDescription;

		public string AlarmDescription;

		public double AlarmValue;

		public double AlarmValueStandard;

		public double AlarmValueDifference
		{
			get
			{
				return AlarmValue - AlarmValueStandard;
			}
		}
	}
}
