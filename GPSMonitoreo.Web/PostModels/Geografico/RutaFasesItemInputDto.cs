using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Geografico
{
    public class RutaFasesItemInputDto : PostModel
	{
		public byte tipo { get; set; }

		public int id { get; set; }

		public byte fase { get; set; }


    }
}
