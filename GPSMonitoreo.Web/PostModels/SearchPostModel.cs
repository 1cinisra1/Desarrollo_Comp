using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels
{
    public class SearchPostModel : PostModel
	{
		public int pagenum { get; set; }
		public int recordcount { get; set; }
		public int pagesize { get; set; }

    }
}
