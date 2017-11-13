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

namespace GPSMonitoreo.Web.PostModels.Equipos
{
    public class GpsEdit : PostModelEdit<int>
    {

		[Rules("RequiredNumeric", "DBKeyExists('MARCAS')")]
		public Int16 marca { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('MODELOS')")]
		public Int16 modelo { get; set; }

		////[Rules("RequiredNumeric", "DBKeyExists('EQUIPOS')")]
		[Rules("ExecuteIf('this.equipo > 0', 'DBKeyExists(\\'EQUIPOS\\')')")]

		public Int32 equipo { get; set; }

		[Rules("Required")]
		public string imei { get; set; }


		public int Save(EntitiesContext dbContext)
		{
			GPSMonitoreo.Data.Models.GPS entity;

			entity = new GPSMonitoreo.Data.Models.GPS()
			{
				ID = id
				, DESCRIPCION_LARGA = descripcion_larga
				, DESCRIPCION_MED = descripcion_mediana
				, DESCRIPCION_CORTA = descripcion_corta
				, ABREVIACION = abreviacion
				, ESTADO_ID = estado
				, IMEI = imei
				, MARCA_ID = marca
				, MODELO_ID = modelo
				////, EQUIPO_ID = equipo
				, EQUIPO_ID = equipo > 0 ? (int?)equipo : null
			};

			if (entity.ID == 0)
			{
				dbContext.GPS.Add(entity);
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}

			dbContext.SaveChanges();
			return entity.ID;

		}

		public static GpsEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.GPS.Where(item => item.ID == id).FirstOrDefault();


			if (entity != null)
			{
				var model = new GpsEdit()
				{
					id = entity.ID
					, descripcion_larga = entity.DESCRIPCION_LARGA
					, descripcion_mediana = entity.DESCRIPCION_MED
					, descripcion_corta = entity.DESCRIPCION_CORTA
					, abreviacion = entity.ABREVIACION
					, estado = entity.ESTADO_ID
					, imei = entity.IMEI
					, marca = entity.MARCA_ID
					, modelo = entity.MODELO_ID
					, equipo = entity.EQUIPO_ID ?? 0

				};
				return model;
			}

			return null;
		}

	}

}
