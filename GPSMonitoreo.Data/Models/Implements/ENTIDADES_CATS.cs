using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{
	/// <summary>
	/// CLASE PARCIAL RECURSIVA SOBRE LA CUAL SE REALIZAN EXTENSIONES SOBRE EL COMPORTAMIENTO DE LA ENTIDAD
	/// 
	/// </summary>
	
	//DEVCOMMENT: NOTESE LA HERENCIA DE LA INTERFAZ IComunCats (genérico) en particular del mismo tipo de la clase actual (entidades_cat) para denotar la recursividad en la relación padre-hijo de la tabla.
	public partial class ENTIDADES_CATS: IComunCats<ENTIDADES_CATS>, ICommonEntityCategory<short>
	{
		ICommonEntityCategory<short> ICommonEntityCategory<short>.PADRE
		{
			get
			{
				return this.PADRE;
			}

			set
			{
				this.PADRE = (ENTIDADES_CATS)value;
			}
		}

	}
}
