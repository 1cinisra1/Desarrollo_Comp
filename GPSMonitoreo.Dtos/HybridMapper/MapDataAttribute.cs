using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.HybridMapper
{
    public class MapDataAttribute : Attribute
    {
		private Dictionary<string, object> _data;

		public MapDataAttribute()
		{
			//typeof(Attribute).GetCustomAttributes()

		}
    }
}
