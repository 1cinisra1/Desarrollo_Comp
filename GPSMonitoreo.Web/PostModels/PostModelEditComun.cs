using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using MVCHelpers.ViewModels.Validators;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Data.Models;


namespace GPSMonitoreo.Web.PostModels
{
    public class PostModelEditComun: PostModel
    {
		[Rules("Required", "MaxLength(52)")]
		public string descripcion_larga { get; set; }


		[Rules("MaxLength(30)")]
		public string descripcion_mediana { get; set; }

		[Rules("MaxLength(15)")]
		public string descripcion_corta { get; set; }


		[Rules("MaxLength(5)")]
		public string abreviacion { get; set; }

		[Rules("MaxLength(500)")]
		public string observaciones { get; set; }

		//TODO: NO DEBE TENER LA SIGUIENTE REGLA DE VALIDACIÓN? A NIVEL DE BASE ESTÁ DEFINIDA COMO NOT NULL
		//[Rules("Required", "Min(0)")]
		public byte estado { get; set; }


		public virtual void FromEntity(ICommonEntity entity)
		{
			descripcion_larga = entity.DESCRIPCION_LARGA;
			descripcion_mediana = entity.DESCRIPCION_MED;
			descripcion_corta = entity.DESCRIPCION_CORTA;
			abreviacion = entity.ABREVIACION;
			observaciones = entity.OBSERVACIONES;
			estado = entity.ESTADO_ID;
		}

		public virtual void ToEntity(ICommonEntity entity)
		{
			entity.DESCRIPCION_LARGA = descripcion_larga;
			entity.DESCRIPCION_MED = descripcion_mediana;
			entity.DESCRIPCION_CORTA = descripcion_corta;
			entity.ABREVIACION = abreviacion;
			entity.OBSERVACIONES = observaciones;
			entity.ESTADO_ID = estado;
		}

	}
}
