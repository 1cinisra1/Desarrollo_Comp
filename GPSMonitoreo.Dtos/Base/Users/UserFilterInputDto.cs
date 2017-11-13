using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Users
{
    public class UserFilterInputDto : FilterInputDto
    {
		public string Username { get; set; }
    }
}
