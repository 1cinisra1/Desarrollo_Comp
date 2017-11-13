using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class Mes
    {

		public int idAnio { get; set; }

		public int idMes { get; set; }

		public int numMes { get; set; }

		public string abv { get; set; }//3 caracteres

		public string nombre { get; set; }


		public int cantidadDias { get; set; }

		public int numPrimeraSemana { get; set; }

		public int numUltimaSemana { get; set; }

		public int cantidadSemanas { get; set; }


		public List<Dia> arregloDias { get; set; }

		public Mes() {

			this.idAnio = 0;
			this.idMes = 0;
			this.arregloDias = new List<Dia>();
		}

	}
}
