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

namespace GPSMonitoreo.Web.PostModels.Entidades
{
	public class EmailEdit : PostModel
	{

		//Id del email:
		public int id { get; set; }

		//Entidad a la que perteneces el email:
		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int entidad { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('EMAIL_TIPOS')")]
		//Tipo del correo:
		public byte tipo { get; set; }

		//[Rules("MinCount(1)", "DBListExist('EMAIL_PROPS')")]
		[Rules("ContinueIf('this.propositos.Count > 0')", "DBListExist('EMAIL_PROPS')")]
		public List<byte> propositos { get; set; }

		[Rules("Required","EmailAddress")]
		public string email { get; set; }
		


		public static EmailEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.ENTIDADES_EMAILS.Where(item => item.ID == id).FirstOrDefault();
			if (entity!= null)
			{
				var model = new EmailEdit()
				{
					id = id,
					entidad = entity.ENTIDAD_ID,
					email = entity.EMAIL,
					tipo = entity.TIPO_ID,
					propositos = dbContext.ENTIDADES_EMAILS_PROPS.Where(item => item.EMAIL_ID == id).Select(s => s.PROPOSITO_ID).ToList()
				};
				return model;
			}
			return null;
		}

		
		public int Save(EntitiesContext dbContext)
		{			
			var entity = new ENTIDADES_EMAILS()			
			{
				ID = id,
				ENTIDAD_ID = entidad,
				TIPO_ID = tipo,
				EMAIL = email
			};
			
			if(propositos.Count > 0)
			{
				foreach (var proposito in propositos)
				{
					entity.PROPOSITOS.Add(new ENTIDADES_EMAILS_PROPS() { EMAIL_ID = id, PROPOSITO_ID = proposito });
				}
			}			
			
			//Nuevo registro:
			if (entity.ID == 0)
			{
				dbContext.ENTIDADES_EMAILS.Add(entity);
			}
			//Edición de registro existente:
			else
			{
				//Primero se eliminan los emails existentes: 1) PROPOSITOS
				dbContext.Delete<ENTIDADES_EMAILS_PROPS>(item => item.EMAIL_ID == id);
				

				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				dbContext.ENTIDADES_EMAILS_PROPS.AddRange(entity.PROPOSITOS);
				//dbContext.ENTIDADES_EMAILS.Add(entity);
			}	
			dbContext.SaveChanges();
			return entity.ID;
		}
	}
}