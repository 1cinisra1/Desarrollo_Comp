﻿using System;
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
	public class ContactoEdit : PostModel
	{

		public int id { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int empresa { get; set; }


		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int persona { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES_CARGOS')")]
		public Int16 cargo { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES_AREAS')")]
		public Int16 area { get; set; }

		[Rules("MaxLength(255)")]
		public string funcion { get; set; }

		[Rules("MaxLength(500)")]
		public string observaciones { get; set; }

		public List<int> direcciones { get; set; }


		public static ContactoEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.ENTIDADES_CTOS.Where(item => item.ID == id).FirstOrDefault();

			if (entity != null)
			{
				var model = new ContactoEdit()
				{
					id = entity.ID,
					area = entity.AREA_ID,
					cargo = entity.CARGO_ID,
					persona = entity.PERSONA_ID,
					empresa = entity.EMPRESA_ID,
					funcion = entity.FUNCION,
					observaciones = entity.OBSERVACIONES,
					direcciones = entity.DIRECCIONES_REL.Where(item => item.CONTACTO_ID == id).Select(item => item.DIRECCION_ID).ToList()
				};
				return model;
			}
			return null;
		}


		public int Save(EntitiesContext dbContext)
		{
			var entity = new ENTIDADES_CTOS()
			{
				ID = id,
				EMPRESA_ID = empresa,
				PERSONA_ID = persona,
				AREA_ID = area,
				FUNCION = funcion,
				CARGO_ID = cargo,
				ESTADO_ID = 1,
				OBSERVACIONES = observaciones
			};


			foreach(var direccionId in direcciones)
			{
				entity.DIRECCIONES_REL.Add(new ENTIDADES_DIRS_CTOS_REL()
				{
					CONTACTO_ID = id,
					DIRECCION_ID = direccionId
				});
			}

			//Nuevo registro:
			if (entity.ID == 0)
			{
				dbContext.ENTIDADES_CTOS.Add(entity);
			}
			//Edición de registro existente:
			else
			{
				dbContext.Delete<ENTIDADES_DIRS_CTOS_REL>(item => item.CONTACTO_ID == id);
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				dbContext.ENTIDADES_DIRS_CTOS_REL.AddRange(entity.DIRECCIONES_REL);
			}
			dbContext.SaveChanges();

			return entity.ID;
		}
	}
}