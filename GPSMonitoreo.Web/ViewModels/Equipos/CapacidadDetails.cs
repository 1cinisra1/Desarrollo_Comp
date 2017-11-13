using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using MVCHelpers.ViewModels.Validators;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Data.Models;
using System.Data.Entity;


namespace GPSMonitoreo.Web.ViewModels.Equipos
{
    public class CapacidadDetails
    {

		public string categoria_producto { get; set; }
		public string capacidad { get; set; }
		public string unidad { get; set; }
		public decimal valor { get; set; }

		public void FromEntity(EQUIPOS_CAPS_REL entity)
		{
			categoria_producto = entity.PRODUCTO_CATEGORIA.DESCRIPCION_LARGA;
			capacidad = entity.CAPACIDAD.DESCRIPCION_LARGA;
			unidad = entity.CAPACIDAD.UNIDAD.DESCRIPCION_LARGA;
			//valor = entity.VALOR;
		}

		public static CapacidadDetails CreateFromEntity(EQUIPOS_CAPS_REL entity)
		{
			var details = new CapacidadDetails();
			details.FromEntity(entity);
			return details;
		}
    }
}
