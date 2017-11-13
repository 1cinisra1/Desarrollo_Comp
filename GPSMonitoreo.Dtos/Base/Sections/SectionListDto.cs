using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Sections
{
    public class SectionListDto: CommonBaseWithAuxiliarSimpleListDto<int>
    {
		public string CategoryDescription { get; set; }
	}
}
