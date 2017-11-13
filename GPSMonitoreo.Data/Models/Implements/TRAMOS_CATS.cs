using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{

	public partial class TRAMOS_CATS: IComunCats<TRAMOS_CATS>, ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				return this.PADRE;
			}

			set
			{
				this.PADRE = (TRAMOS_CATS)value;
			}
		}

	}
}
