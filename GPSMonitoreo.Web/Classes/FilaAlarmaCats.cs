using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Classes
{
    public class FilaAlarmaCats
    {

		public int id { get; set; }

		public string descripcion { get; set; }

		public FilaAlarma [] arregloFilaAlarma { get; set; }

		public string viaje { get; set; }

		public string ruta { get; set; }

		public string tramo { get; set; }

		public string segmento { get; set; }

		public string cerca { get; set; }

	}
}
