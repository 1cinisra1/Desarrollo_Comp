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
    public class TelefonoEdit : PostModel
	{

		public int id { get; set; }


		public int entidad { get; set; }
		public int direccion { get; set; }

		public int ubicacion { get; set; }



		[Rules("RequiredNumeric", "DBKeyExists('TELEFONO_TIPOS')")]
		public byte tipo { get; set; }


		[Rules("RequiredNumeric")]
		public short codigo_pais { get; set; }

		[Rules("RequiredNumeric")]
		public short codigo_area { get; set; }

		public string telefono { get; set; }


		public static TelefonoEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.ENTIDADES_TELFS.Where(item => item.ID == id).FirstOrDefault();



			if (entity != null)
			{
				var model = new TelefonoEdit()
				{
					id = entity.ID,
					entidad = entity.ENTIDAD_ID,
					direccion = entity.DIRECCION_ID ?? 0,
					ubicacion = entity.UBICACION_ID ?? 0,
					tipo = entity.TIPO_ID,
					codigo_pais = entity.CODIGO_PAIS,
					codigo_area = entity.CODIGO_AREA,
					telefono = entity.TELEFONO
				};

				return model;
			}
			return null;
		}



		public int Save(EntitiesContext dbContext)
		{
			var entity = new ENTIDADES_TELFS()
			{
				ID = id,
				ENTIDAD_ID = entidad,
				DIRECCION_ID = direccion > 0 ? (int?)direccion : null,
				UBICACION_ID = ubicacion > 0 ? (int?)ubicacion : null,
				TIPO_ID = tipo,
				CODIGO_PAIS = codigo_pais,
				CODIGO_AREA = codigo_area,
				TELEFONO = telefono
			};
			
			if (entity.ID == 0)
			{
				dbContext.ENTIDADES_TELFS.Add(entity);
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}

			dbContext.SaveChanges();

			return entity.ID;
		}
	}
}
