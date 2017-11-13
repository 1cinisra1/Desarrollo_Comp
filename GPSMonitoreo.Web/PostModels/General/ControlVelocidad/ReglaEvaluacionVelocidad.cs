using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.ControlVelocidad
{
    public class ReglaEvaluacionVelocidad
    {

		public int entidad { get; set; }
		
		public int categoria { get; set; }

		public int producto { get; set; }

		public int tipo_volumen { get; set; }

		public int tipo_direccion { get; set; }
		
		public decimal tiempo { get; set; }
		
		public decimal velocidad { get; set; }
	}
}
