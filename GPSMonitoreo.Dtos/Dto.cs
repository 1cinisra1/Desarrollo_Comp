using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos
{
    public class Dto<TId>
    {
		public TId Id;
		public string Description;
    }
}
