using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Entidades
{
    public class ContactoSearch: SearchPostModel
    {
		public int empresa { get; set; }
		public string descripcion { get; set; }

		//public string identificacion { get; set; }

		public string nombres{ get; set; }
		public string apellidos { get; set; }
	}
}
