using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.ControlVelocidad
{
    public class ControlVelocidadSearch: SearchPostModel
    {
		public string descripcion { get; set; }

		public int categoria { get; set; }

		public int producto { get; set; }
	}
}
