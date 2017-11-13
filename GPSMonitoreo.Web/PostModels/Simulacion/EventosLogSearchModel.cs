using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Simulacion
{
    public class EventosLogSearchModel: SearchPostModel
    {
		public int viaje { get; set; }
		public int categoria { get; set; }

	}
}
