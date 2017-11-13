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

namespace GPSMonitoreo.Web.PostModels.Localidades
{
    public class ProvinciasEdit : PostModelEdit<int>
	{


		[Rules("RequiredNumeric", "DBKeyExists('PAISES')")]
		public Int32 pais { get; set; }

		//El resto de atributos miembros son estándares y se validan en la clase padre de la que esta hereda: PostModelEdit

		

		public int Save(EntitiesContext dbContext)
		{

			var entity = new GPSMonitoreo.Data.Models.PROVINCIAS()
			{
				ID = id,
				DESCRIPCION_LARGA = descripcion_larga,
				ABREVIACION = abreviacion,
				DESCRIPCION_CORTA = descripcion_corta,
				DESCRIPCION_MED = descripcion_mediana,
				PAIS_ID = pais,
				ESTADO_ID = estado
			};


			if (entity.ID == 0)
			{
				dbContext.PROVINCIAS.Add(entity);
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}
			dbContext.SaveChanges();
			return entity.ID;
		}

		public static ProvinciasEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.PROVINCIAS.Where(item => item.ID == id).FirstOrDefault();


			if (entity != null)
			{
				var model = new ProvinciasEdit()
				{
					id = entity.ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					abreviacion = entity.ABREVIACION,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					pais = entity.PAIS_ID
				};
				return model;
			}

			return null;
		}
	}
}
