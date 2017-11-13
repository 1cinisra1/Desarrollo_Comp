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

namespace GPSMonitoreo.Web.PostModels.General.ControlVelocidad
{
    public class ControlVelocidadEdit : PostModelEdit<int>
    {

		public ReglaEvaluacionVelocidad[]  reglas { get; set; }



		public static ControlVelocidadEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			//var entity = dbContext.EQUIPOS.Where(item => item.ID == id).FirstOrDefault();


			//if (entity != null)
			//{
			//	var model = new ControlVelocidadEdit();
			//	model.FromEntity(entity);


			//	model.reglas = dbContext.EQUIPOS_CAPS_REL.Where(item => item.EQUIPO_ID == entity.ID).OrderBy(item => item.PRODUCTO_CATEGORIA_ID).Select(item => new ReglaEvaluacionVelocidad() { categoria = item.PRODUCTO_CATEGORIA_ID, producto = item.CAPACIDAD_ID, velocidad = item.VALOR }).ToArray();

				
			//	return model;

			//}

			return null;
		}



		public int Save(EntitiesContext dbContext)
		{


			//var entity = new EQUIPOS();
			//ToEntity(entity);


			//dbContext.Delete<EQUIPOS_CAPS_REL>(item => item.EQUIPO_ID == id);

			//foreach (var objRegla in reglas)
			//{
			//	entity.CAPACIDADES.Add(new EQUIPOS_CAPS_REL
			//	{
			//		EQUIPO_ID = id
			//													,
			//		PRODUCTO_CATEGORIA_ID = short.Parse(objRegla.categoria.ToString())
			//													,
			//		CAPACIDAD_ID = short.Parse(objRegla.producto.ToString())
			//													,
			//		VALOR = objRegla.velocidad
			//	});

			//}

			//if (entity.ID == 0)
			//{
			//	dbContext.EQUIPOS.Add(entity);
			//}
			//else
			//{
			//	dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			//	dbContext.EQUIPOS_CAPS_REL.AddRange(entity.CAPACIDADES);
			//}

			//dbContext.SaveChanges();
			//return entity.ID;

			return 0;

		}

		

	}

}
