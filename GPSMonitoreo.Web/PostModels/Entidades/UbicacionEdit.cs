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
	public class UbicacionEdit: PostModelEdit<int>
	{

		//Entidad a la que pertenece la ubicación:
		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int entidad { get; set; }


		//Dirección de la ubicación:
		//Probar required pues es string no numeric
		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES_DIRS')")]
		public int direccion { get; set; }


		//Ubicación padre. Campo opcional
		[Rules("ContinueIf('this.ubicacion > 0')", "DBKeyExists('ENTIDADES_DIRS_UBICAS')")]
		public int ubicacion { get; set; }

		//Tipo de la ubicación:
		[Rules("MinCount(1)", "DBListExist('ENTIDADES_DIRS_UBICAS_TIPOS')")]
		public List<byte> tipos { get; set; }

		public int cerca { get; set; }




		public static UbicacionEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{

			var entity = dbContext.ENTIDADES_DIRS_UBICAS.Where(item => item.ID == id).FirstOrDefault();

			if (entity != null)
			{

				var model = new UbicacionEdit();
				model.FromEntity(entity);
				model.entidad = entity.ENTIDAD_ID;
				model.direccion = entity.DIRECCION_ID;
				model.ubicacion = entity.UBICACION_ID ?? 0;
				model.cerca = entity.CERCA_ID ?? 0;
				model.tipos = entity.TIPOS.Select(item => item.TIPO_ID).ToList();
				return model;
			}
			return null;
		}

		
		public int Save(EntitiesContext dbContext)
		{

			var entity = new ENTIDADES_DIRS_UBICAS();
			ToEntity(entity);
			entity.ENTIDAD_ID = entidad;
			entity.DIRECCION_ID = direccion;
			entity.UBICACION_ID = ubicacion > 0 ? (int?)ubicacion : null;
			entity.CERCA_ID = cerca > 0 ? (int?)cerca : null;
			
			foreach (var tipo in tipos)
			{
				entity.TIPOS.Add(new ENTIDADES_DIRS_UBICAS_TIPOSREL { UBICACION_ID = id, TIPO_ID = tipo });
			}

			
			if (id == 0)
			{
				dbContext.ENTIDADES_DIRS_UBICAS.Add(entity);
			}
			else
			{
				dbContext.Delete<ENTIDADES_DIRS_UBICAS_TIPOSREL>(item => item.UBICACION_ID == id);
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				dbContext.ENTIDADES_DIRS_UBICAS_TIPOSREL.AddRange(entity.TIPOS);
			}	

			dbContext.SaveChanges();

			return entity.ID;
		}
	}
}