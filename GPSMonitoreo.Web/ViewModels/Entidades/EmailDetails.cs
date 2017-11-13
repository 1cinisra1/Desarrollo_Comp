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
    public class EmailDetails
    {
		public int id { get; set; }
		public string tipo { get; set; }
		public string correo { get; set; }
		

		public void FromEntity(ENTIDADES_EMAILS entity)
		{
			id = entity.ID;
			tipo = entity.TIPO.DESCRIPCION_LARGA;
			correo = entity.EMAIL;
		}

		public static EmailDetails CreateFromEntity(ENTIDADES_EMAILS entity)
		{
			var details = new EmailDetails();
			details.FromEntity(entity);
			return details;
		}
    }
}
