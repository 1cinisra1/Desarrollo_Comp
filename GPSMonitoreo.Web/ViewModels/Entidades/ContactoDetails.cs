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
    public class ContactoDetails
    {

		public int id { get; set; }		

		public string apellidos { get; set; }

		public string nombres { get; set; }

		public string cargo{ get; set; }

		public int personaId { get; set; }


		public void FromEntity(ENTIDADES_CTOS entity, bool deep = false)
		{
			id = entity.ID;
			apellidos = entity.PERSONA.APELLIDOS;
			nombres = entity.PERSONA.NOMBRES;
			cargo = entity.CARGO.DESCRIPCION_LARGA;
			personaId = entity.PERSONA_ID;
		}

		public static ContactoDetails CreateFromEntity(ENTIDADES_CTOS entity, bool deep = false)
		{
			var details = new ContactoDetails();
			details.FromEntity(entity, deep);
			return details;
		}

		public static ContactoDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id, bool deep = false)
		{
			return CreateFromEntity(dbContext.ENTIDADES_CTOS.FirstOrDefault(item => item.ID == id));

			//var personaId = dbContext.ENTIDADES_CTOS.FirstOrDefault(x => x.ID == id).PERSONA_ID;
			//return CreateFromEntity(dbContext.ENTIDADES_PERSONA.FirstOrDefault(item => item.ENTIDAD_ID == personaId), deep);

			/*return CreateFromEntity(dbContext.ENTIDADES_CTOS.FirstOrDefault(item => item.PERSONA_ID ==
				(dbContext.ENTIDADES_CTOS.Where(p => p.ID == id).Select(p => p.PERSONA_ID)).ToArray()[0]
			) ,deep);*/

			/*return CreateFromEntity(dbContext.ENTIDADES_CTOS.FirstOrDefault
				(item => item.PERSONA_ID == 
				(dbContext.ENTIDADES_CTOS.FirstOrDefault(x => x.ID == id).PERSONA_ID)
				, deep);*/
		}
	}
}
