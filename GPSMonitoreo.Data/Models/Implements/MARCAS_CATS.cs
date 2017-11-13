using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{
	public partial class MARCAS_CATS: IComunCats<MARCAS_CATS>, ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				return this.PADRE;
			}

			set
			{
				this.PADRE = (MARCAS_CATS)value;
			}
		}
	}
}

