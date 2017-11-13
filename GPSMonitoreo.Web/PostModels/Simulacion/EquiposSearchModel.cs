using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Simulacion
{
    public class EquiposSearchModel : PostModel
	{
		public List<byte> region;

		public List<string> equipo;

		public List<string> alarma;

		public List<string> producto;

		public List<byte> fase;
	}
}
