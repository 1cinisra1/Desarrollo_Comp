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


namespace GPSMonitoreo.Web.ViewModels.Entidades
{
    public class TelefonoDetails
    {
		public int id { get; set; }
		public string tipo { get; set; }

		public short codigo_pais { get; set; }

		public short codigo_area { get; set; }

		public string telefono { get; set; }


		public void FromEntity(ENTIDADES_TELFS entity)
		{
			id = entity.ID;
			tipo = entity.TIPO.DESCRIPCION_LARGA;
			codigo_pais = entity.CODIGO_PAIS;
			codigo_area = entity.CODIGO_AREA;
			telefono = entity.TELEFONO;
		}

		public static TelefonoDetails CreateFromEntity(ENTIDADES_TELFS entity)
		{
			var details = new TelefonoDetails();
			details.FromEntity(entity);
			return details;
		}
    }
}
