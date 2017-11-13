using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class CalendarioOperativo
    {

		public int idCalendarioOperativo { get; set; }

		public int cantidadSemanas { get; set; }

		public bool primeroEneroEnSemanaUno { get; set; }


		public List<Mes> arregloMes { get; set; }

		public List<TipoDia> arregloTipoDia { get; set; }


		public CalendarioOperativo() {

			this.arregloMes = new List<Mes>();

		}

	}
}
