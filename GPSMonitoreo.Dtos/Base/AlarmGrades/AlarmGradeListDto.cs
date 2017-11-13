using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.AlarmGrades
{
    public class AlarmGradeListDto: CommonBaseListDto<byte>
	{

		[Column("COLOR")]
		public string Color { get; set; }

		[Column("PARPADEANTE")]
		public bool Blinking { get; set; }
    }
}
