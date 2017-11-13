using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Geografico
{
    public class RutasFasesCercaFaseInputDto : PostModel
	{
		public int cerca { get; set; }

		public byte fase { get; set; }
    }
}
