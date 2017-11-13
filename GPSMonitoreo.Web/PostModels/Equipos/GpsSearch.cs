using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Equipos
{
    public class GpsSearch: SearchPostModel
    {
		public string descripcion { get; set; }

		public int marca { get; set; }

		public int modelo { get; set; }
		
		public int equipo { get; set; }
	}
}
