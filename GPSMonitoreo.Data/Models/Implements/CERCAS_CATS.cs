using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{
	public partial class CERCAS_CATS : IComunCats<CERCAS_CATS>, ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				return this.PADRE;
			}

			set
			{
				this.PADRE = (CERCAS_CATS)value;
			}
		}
	}
}
