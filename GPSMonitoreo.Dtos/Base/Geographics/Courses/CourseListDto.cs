using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Geographics.Courses
{
    public class CourseListDto: CommonBaseSimpleListDto<int>
    {
		public string AlternateId { get; set; }
    }
}
