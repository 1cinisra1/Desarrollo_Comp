using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Classes
{
    public class FilaQuery
    {

		public short IDAC { get; set; }

		public string DLAC { get; set; }
		
		public short IDA { get; set; }

		public string DLA { get; set; }

		public bool viaje { get; set; }

		public bool ruta { get; set; }
		
		public bool tramo { get; set; }
		
		public bool segmento { get; set; }
		
		public bool cerca { get; set; }
	}
}
