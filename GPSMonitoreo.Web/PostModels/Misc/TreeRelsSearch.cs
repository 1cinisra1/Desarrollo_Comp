using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.Misc
{
    public class TreeRelsSearch :  SearchPostModel
    {
		public int id { get; set; }

		public string rel { get; set; }
    }
}
