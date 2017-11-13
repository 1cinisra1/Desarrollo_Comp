using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Entidades
{
    public class EntidadSearch: SearchPostModel
    {
		public string descripcion { get; set; }
		public string identificacion { get; set; }

		public byte tipoidentificacion { get; set; }

		public string nombres{ get; set; }
		public string apellidos { get; set; }

		public string razon_social { get; set; }

		public byte tipoentidad { get; set; }

		public Int16[] categoria { get; set; }

		public List<byte> relaciones { get; set; }

	}
}
