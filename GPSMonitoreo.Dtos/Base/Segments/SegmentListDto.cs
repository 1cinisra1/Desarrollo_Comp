using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Segments
{
    public class SegmentListDto: CommonBaseWithAuxiliarSimpleListDto<int>
    {
		public string CategoryDescription { get; set; }
	}
}
