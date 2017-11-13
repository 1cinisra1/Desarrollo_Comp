using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{

	public partial class RUTAS_CATS: IComunCats<RUTAS_CATS>, ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				return this.PADRE;
			}

			set
			{
				this.PADRE = (RUTAS_CATS)value;
			}
		}

	}
}
