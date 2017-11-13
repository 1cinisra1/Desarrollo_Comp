using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{
	public partial class EQUIPOS_CAPS : ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				
				return this.PADRE;
			}

			set
			{
				this.PADRE = (EQUIPOS_CAPS)value;
			}
		}
	}
}
