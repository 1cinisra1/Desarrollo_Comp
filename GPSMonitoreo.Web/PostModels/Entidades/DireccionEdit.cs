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
	public class DireccionEdit : PostModelEdit<int>
	{

		public int entidad { get; set; }


		[Rules("MinCount(1)", "DBListExist('ENTIDADES_DIRS_TIPOS')")]
		public List<byte> tipos { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('PAISES')")]
		public int pais { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('PROVINCIAS')")]
		public int provincia { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('CIUDADES')")]
		public int ciudad { get; set; }

		public string cod_postal { get; set; }

		//Se comenta porque hereda de PostModelEdit que ya contiene este atributo
		//public int id { get; set; }

		[Rules("Required")]
		public string calle_principal { get; set; }

		[Rules("Required")]
		public string numeracion { get; set; }

		[Rules("Required")]
		public string calle_transversal { get; set; }

		public string canton { get; set; }

		[Rules("Required")]
		public string ciudadela { get; set; }

		public string manzana { get; set; }

		public int cerca { get; set; }

		public byte region { get; set; }


		public List<int> contactos { get; set; }


		public static DireccionEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{

			var model = PostModelEdit.Load<DireccionEdit, ENTIDADES_DIRS>(dbContext, id, (editModel, entity) =>
			{
				editModel.entidad = entity.ENTIDAD_ID;
				editModel.pais = entity.PAIS_ID;
				editModel.region = entity.REGION_ID ?? 0;
				editModel.provincia = entity.PROVINCIA_ID ?? 0;
				editModel.ciudad = entity.CIUDAD_ID ?? 0;
				editModel.cod_postal = entity.CODIGOPOSTAL;
				editModel.calle_principal = entity.CALLE_PRINCIPAL;
				editModel.numeracion = entity.NUMERACION;
				editModel.calle_transversal = entity.CALLE_TRANSVERSAL;
				editModel.canton = entity.CANTON;
				editModel.cerca = entity.CERCA_ID ?? 0;
				editModel.ciudadela = entity.CIUDADELA;
				editModel.manzana = entity.MANZANA;
				editModel.observaciones = entity.OBSERVACIONES;

				editModel.tipos = dbContext.ENTIDADES_DIRS_TIPOS_REL.Where(item => item.DIRECCION_ID == id).Select(s => s.TIPO_ID).ToList();
				//editModel.tipos = entity.TIPOS.Select(item => item.ID).ToList();
			});
			
			return model;
			//model.calle_principal

			//var entity = dbContext.ENTIDADES_DIRS.Where(item => item.ID == id).FirstOrDefault();


			//if (entity != null)
			//{

			//	var model = new DireccionEdit()
			//	{
			//		id = entity.ID,
			//		entidad = entity.ENTIDAD_ID,
			//		pais = entity.PAIS_ID,
			//		provincia = entity.PROVINCIA_ID ?? 0,
			//		ciudad = entity.CIUDAD_ID ?? 0,
			//		cod_postal = entity.CODIGOPOSTAL,
			//		calle_principal = entity.CALLE_PRINCIPAL,
			//		numeracion = entity.NUMERACION,
			//		calle_transversal = entity.CALLE_TRANSVERSAL,
					
			//	};
			//	return model;
			//}
			//return null;
		}



		public int Save(EntitiesContext dbContext)
		{
			byte index = 0;

			//dbContext.Delete<EN>(item => item.ENTIDAD_ID == id);
			dbContext.Delete<ENTIDADES_DIRS_TIPOS_REL>(item => item.DIRECCION_ID == id);
			var entity = new ENTIDADES_DIRS()			
			{
				ID = id,
				ENTIDAD_ID = entidad,
				DESCRIPCION_LARGA = descripcion_larga,
				DESCRIPCION_MED = descripcion_mediana,
				DESCRIPCION_CORTA = descripcion_corta,
				ABREVIACION = abreviacion,
				ESTADO_ID = estado,
				PAIS_ID = pais,
				PROVINCIA_ID = provincia,
				CIUDAD_ID = ciudad,
				CANTON = canton,
				CODIGOPOSTAL = cod_postal,
				CIUDADELA = ciudadela,
				CALLE_PRINCIPAL = calle_principal,
				CALLE_TRANSVERSAL = calle_transversal,
				NUMERACION = numeracion,
				MANZANA = manzana,
				CERCA_ID = cerca > 0 ? (int?)cerca : null,
				REGION_ID = region
			};

			
			foreach (var tipo in tipos)
			{
				entity.TIPOS_REL.Add(new ENTIDADES_DIRS_TIPOS_REL() { DIRECCION_ID = id, TIPO_ID = tipo });
			}

			
			//foreach (var contacto in contactos)
			//{
			//	entity.CONTACTOS_REL.Add(new ENTIDADES_DIRS_CTOS_REL() { DIRECCION_ID = id, CONTACTO_ID = contacto });
			//}


			//Nuevo registro:
			if (entity.ID == 0)
			{
				dbContext.ENTIDADES_DIRS.Add(entity);
			}
			//Edición de registro existente:
			else
			{

				//dbContext.Delete<ENTIDADES_DIRS_CTOS_REL>(item => item.DIRECCION_ID == id);
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				dbContext.ENTIDADES_DIRS_TIPOS_REL.AddRange(entity.TIPOS_REL);
				//dbContext.ENTIDADES_DIRS_CTOS_REL.AddRange(entity.CONTACTOS_REL);
			}	
			dbContext.SaveChanges();
			return entity.ID;
		}
	}
}