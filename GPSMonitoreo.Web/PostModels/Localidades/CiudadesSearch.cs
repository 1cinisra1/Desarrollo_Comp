using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Localidades
{
    public class CiudadesSearch: SearchPostModel
    {
		public string descripcion { get; set; }
		public Int32 paises { get; set; }
		public Int32 provincias { get; set; }

	}
}
