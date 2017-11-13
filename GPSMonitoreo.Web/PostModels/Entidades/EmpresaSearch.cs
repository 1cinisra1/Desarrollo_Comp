using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Entidades
{
    public class EmpresaSearch: SearchPostModel
    {
		public string descripcion { get; set; }
		public string ruc { get; set; }
		public string razon_social { get; set; }
	}
}
