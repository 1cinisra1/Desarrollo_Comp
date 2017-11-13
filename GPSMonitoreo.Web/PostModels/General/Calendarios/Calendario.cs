using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class Calendario
    {

		public List<CalendarioOperativo> arregloCalendarioOperativo { get; set; }


		public Calendario() {

			this.arregloCalendarioOperativo = new List<CalendarioOperativo>();

		}

	}

}
