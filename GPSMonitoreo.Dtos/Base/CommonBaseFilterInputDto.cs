using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{
    public class CommonBaseFilterInputDto: FilterInputDto
    {
		public string Description { get; set; }
    }
}
