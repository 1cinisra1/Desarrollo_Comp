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
using System.Data.Entity;


namespace GPSMonitoreo.Web.ViewModels.ControlVelocidad
{
    public class ReglaEvaluacionVelocidadDetails
    {

		public int entidad { get; set; }
		
		public int categoria { get; set; }

		public int producto { get; set; }

		public int tipo_volumen { get; set; }

		public int tipo_direccion { get; set; }
		
		public decimal tiempo { get; set; }
		
		public decimal velocidad { get; set; }

		public void FromEntity(EQUIPOS_CAPS_REL entity)
		{
			categoria = entity.PRODUCTO_CATEGORIA.ID;
			producto = entity.CAPACIDAD.ID;
			tiempo = entity.CAPACIDAD.UNIDAD.ID;
			//velocidad = entity.VALOR;
		}

		public static ReglaEvaluacionVelocidadDetails CreateFromEntity(EQUIPOS_CAPS_REL entity)
		{
			var details = new ReglaEvaluacionVelocidadDetails();
			details.FromEntity(entity);
			return details;
		}
    }
}
