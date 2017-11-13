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

namespace GPSMonitoreo.Web.PostModels.Productos
{
    public class ProductosEdit : PostModelEdit<short>
    {

		[Rules("Required", "DBKeyExists('PRODUCTOS_CATS')")]
		public Int16 categoria { get; set; }

		[Rules("Required", "GreaterThan(0)")]
		[RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Peso debe tener máximo 2 decimales")]
		public double peso { get; set; }

		public int Save(EntitiesContext dbContext)
		{

			GPSMonitoreo.Data.Models.PRODUCTOS entity;
			////Int32 pesoD = Int32.Parse(peso);
			double pesoD = peso;
			entity = new GPSMonitoreo.Data.Models.PRODUCTOS()
			{
				ID = id,
				DESCRIPCION_LARGA = descripcion_larga,
				ABREVIACION = abreviacion,
				DESCRIPCION_CORTA = descripcion_corta,
				DESCRIPCION_MED = descripcion_mediana,
				////OBSERVACIONES = observaciones,
				PESO_ESPECIFICO = pesoD,
				CATEGORIA_ID = categoria,
				ESTADO_ID = estado
			};


			if (entity.ID == 0)
			{
				dbContext.PRODUCTOS.Add(entity);
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}
			dbContext.SaveChanges();
			return entity.ID;
		}

		public static ProductosEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{

			var entity = dbContext.PRODUCTOS.Where(item => item.ID == id).FirstOrDefault();

			////string pesoD = entity.PESO_ESPECIFICO.ToString();
			double pesoD = entity.PESO_ESPECIFICO;

			if (entity != null)
			{
				var model = new ProductosEdit()
				{
					id = entity.ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					abreviacion = entity.ABREVIACION,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					///observaciones = entity.OBSERVACIONES,
					peso = pesoD,
					categoria = entity.CATEGORIA_ID
				};
				return model;
			}

			return null;
		}

	}

}
