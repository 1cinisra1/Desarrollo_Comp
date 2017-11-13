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
    public class PaisesEdit : PostModelEdit<int>
	{


		//Los atributos miembros son estándares y se validan en la clase padre de la que esta hereda: PostModelEdit
		//No se hace uso de la vista genérica de países porque a futuro se va a extender

		

		public int Save(EntitiesContext dbContext)
		{

			var entity = new GPSMonitoreo.Data.Models.PAISES()
			{
				ID = id,
				DESCRIPCION_LARGA = descripcion_larga,
				ABREVIACION = abreviacion,
				DESCRIPCION_CORTA = descripcion_corta,
				DESCRIPCION_MED = descripcion_mediana,				
				ESTADO_ID = estado
			};


			if (entity.ID == 0)
			{
				dbContext.PAISES.Add(entity);
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}
			dbContext.SaveChanges();
			return entity.ID;
		}

		public static PaisesEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.PAISES.Where(item => item.ID == id).FirstOrDefault();


			if (entity != null)
			{
				var model = new PaisesEdit()
				{
					id = entity.ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					abreviacion = entity.ABREVIACION,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					estado = entity.ESTADO_ID
				};
				return model;
			}

			return null;
		}
	}
}
