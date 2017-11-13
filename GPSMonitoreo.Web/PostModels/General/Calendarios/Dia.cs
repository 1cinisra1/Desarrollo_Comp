using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class Dia
    {

		public int idMes { get; set; }

		public int idDia { get; set; }

		public int numDiaMes { get; set; }

		public int numDiaSemana { get; set; }

		public string abvDia { get; set; }//3 caracteres

		public int numSemana { get; set; }

		public List<TipoDia> arregloTipoDia { get; set; }

		public Dia()
		{
			////this.idMes = 0;
			////this.idDia = 0;
			////this.arregloTipoDia = TipoDia.ObtenerTiposDias();

		}

		public Dia(int numDiaSemana)
		{
			this.idMes = 0;
			this.idDia = 0;
			this.arregloTipoDia = TipoDia.ObtenerTiposDiasConCheck(numDiaSemana);
			
		}

	}

}
