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
	public class EntidadEdit : PostModelEdit<int>
	{

		//[Rules("MaxLength(20)")]
		public string alterno { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES_TIPOS')")]
		public byte tipo { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES_IDENT_TIPOS')")]
		public byte tipoiden { get; set; }

		[Rules("Required")]
		public string identificacion { get; set; }
		
		public string razon_social { get; set; }

		[Rules("ContinueIf('this.tipo == 2 || this.tipo == 4 || this.tipo == 5' )", "Required","MaxLength(100)")]
		public string apellidos { get; set; }

		[Rules("ContinueIf('this.tipo == 2 || this.tipo == 4 || this.tipo == 5' )", "Required", "MaxLength(100)")]
		public string nombres { get; set; }
		public string clasificador { get; set; }

		[Rules("ContinueIf('this.tipo == 4')", "DBKeyExists('PERSONA_SALUDOS')")]
		public byte saludo { get; set; }

		[Rules("ContinueIf('this.profesion > 0')", "DBKeyExists('PERSONA_PROFESIONES')")]
		public byte profesion { get; set; }


		[Rules("MinLength(1)", "DBListExist('ENTIDADES_CATS')")]
		public Int16[] categoria { get; set; }


		[Rules("MinCount(1)","DBListExist('ENTIDADES_RELS')")]
		public List<byte> relaciones { get; set; }


		public static EntidadEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.ENTIDADES.Where(item => item.ID == id).FirstOrDefault();

			List<int> entidadDirs = new List<int>();			

			if (entity != null)
			{
				var model = new EntidadEdit()
				{
					id = entity.ID,
					alterno = entity.ALTERNO_ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					abreviacion = entity.ABREVIACION,
					observaciones = entity.OBSERVACIONES,
					estado = entity.ESTADO_ID,
					tipo = entity.TIPO_ID,
					razon_social = entity.RAZON_SOCIAL,
					apellidos = entity.APELLIDOS,
					nombres = entity.NOMBRES,
					tipoiden = entity.IDENT_TIPO_ID ?? 0,
					identificacion = entity.IDENTIFICACION,
					clasificador = entity.CLASIFICADOR,
					categoria =  dbContext.ENTIDADES_CATS_REL.Where(item => item.ENTIDAD_ID == id). Select(r => r.CATEGORIA_ID).ToArray(),
					relaciones = dbContext.ENTIDADES_RELS_REL.Where(item => item.ENTIDAD_ID == id).Select(s => s.RELACION_ID).ToList()
				};

				if(entity.TIPO_ID == 4 && entity.PERSONA != null)
				{
					model.saludo = entity.PERSONA.SALUDO_ID;
					model.profesion = entity.PERSONA.PROFESION_ID ?? 0;
				}

				return model;
			}
			return null;
		}



		public int Save(EntitiesContext dbContext)
		{
			GPSMonitoreo.Data.Models.ENTIDADES entity;

			dbContext.Delete<ENTIDADES_CATS_REL>(item => item.ENTIDAD_ID == id);
			dbContext.Delete<ENTIDADES_RELS_REL>(item => item.ENTIDAD_ID == id);
			dbContext.Delete<ENTIDADES_PERSONA>(item => item.ENTIDAD_ID == id);

			//TODO: Determinar deletes de: entidad_dirs, frentes, entidad_dirs_distrs

			entity = new GPSMonitoreo.Data.Models.ENTIDADES()
			{
				ID = id,
				DESCRIPCION_LARGA = descripcion_larga,
				DESCRIPCION_MED = descripcion_mediana,
				DESCRIPCION_CORTA = descripcion_corta,
				ABREVIACION = abreviacion,
				OBSERVACIONES = observaciones,
				ESTADO_ID = estado,
				TIPO_ID = tipo,
				RAZON_SOCIAL = razon_social,
				APELLIDOS = apellidos,
				NOMBRES = nombres,
				IDENT_TIPO_ID = tipoiden,
				IDENTIFICACION = identificacion,
				CLASIFICADOR = clasificador,
				ALTERNO_ID = alterno
			};

			//Se redefinió la tabla entidades_cats_rel y se le agregó un nuevo campo para que sea visible desde acá como ENTIDADES_CATS_REL y poder hacer el insert y update
			foreach (var categoriaId in categoria)
			{
				entity.CATS_RELS.Add(new ENTIDADES_CATS_REL { ENTIDAD_ID = id, CATEGORIA_ID = categoriaId });
			}
						
			//Se almacena las relaciones de la entidad con la compañía:
			foreach (var relacionId in relaciones)
			{
				entity.RELS_REL.Add(new ENTIDADES_RELS_REL { ENTIDAD_ID = id, RELACION_ID = relacionId});
			}

			byte index = 0;


			//PERSONA
			if (tipo == 4) 
			{
				entity.PERSONA = new ENTIDADES_PERSONA()
				{
					ENTIDAD_ID = id,
					SALUDO_ID = saludo,
					PROFESION_ID = profesion > 1 ? profesion : (byte?)null
				};
			}			

			//Nuevo registro:
			if (entity.ID == 0)
			{
				dbContext.ENTIDADES.Add(entity);
			}
			//Edición de registro existente:
			else
			{

				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				//Importante para poder almacenar los datos en la tabla de relaciones ENTIDADES_CATS_REL:
				dbContext.ENTIDADES_CATS_REL.AddRange(entity.CATS_RELS);
				dbContext.ENTIDADES_RELS_REL.AddRange(entity.RELS_REL);

				if (tipo == 4)
				{
					dbContext.ENTIDADES_PERSONA.Add(entity.PERSONA);
				}
			}
	
			dbContext.SaveChanges();
			return entity.ID;
		}
	}
}