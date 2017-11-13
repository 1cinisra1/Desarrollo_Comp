using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Localidades
{
    public class ProvinciasSearch: SearchPostModel
    {
		public string descripcion { get; set; }
		public Int32 paises { get; set; }

	}
}
