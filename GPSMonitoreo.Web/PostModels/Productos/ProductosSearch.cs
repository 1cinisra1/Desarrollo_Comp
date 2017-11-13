using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Productos
{
    public class ProductosSearch: SearchPostModel
    {
		public string descripcion { get; set; }
		public int categoria { get; set; }

    }
}
