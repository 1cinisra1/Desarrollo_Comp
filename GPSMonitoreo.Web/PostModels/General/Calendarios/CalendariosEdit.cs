using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using MVCHelpers.ViewModels.Validators;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Data.Models;
using System.Data.Entity;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class CalendariosEdit : PostModelEdit<int>
    {

		//public Object  objCalendario { get; set; }

		[Rules("Required")]
		public string txt_anio { get; set; }

		[Rules("Required")]
		public Mes[] calendario_operativo { get; set; }



		public static CalendariosEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.EQUIPOS.Where(item => item.ID == id).FirstOrDefault();


			if (entity != null)
			{
				var model = new CalendariosEdit();
				model.FromEntity(entity);

				//model.categoria = entity.CATEGORIA_ID;
				//model.marca = entity.MARCA_ID;
				//model.grupo = entity.GRUPO_ID;
				//model.estado_opera = entity.ESTADO_OPERA_ID;
				//model.placa = entity.PLACA;
				//model.modelo = entity.MODELO_ID;
				//model.modelo_ano = entity.MODELO_ANO;
				//model.alterno = entity.ALTERNO_ID;
				//model.serial = entity.SERIAL;

				///model.calendario = dbContext.EQUIPOS_CAPS_REL.Where(item => item.EQUIPO_ID == entity.ID).OrderBy(item => item.PRODUCTO_CATEGORIA_ID).Select(item => new Capacidad() { categoria = item.PRODUCTO_CATEGORIA_ID, capacidad = item.CAPACIDAD_ID, unidad = item.CAPACIDAD.UNIDAD.DESCRIPCION_LARGA, valor = item.VALOR }).ToArray();

				
				return model;

			}

			return null;
		}



		public int Save(EntitiesContext dbContext)
		{
			
			var entity = new EQUIPOS();

			//ToEntity(entity);

			//entity.CATEGORIA_ID = categoria;
			//entity.MARCA_ID = marca;
			//entity.GRUPO_ID = grupo;
			//entity.ESTADO_OPERA_ID = estado_opera;
			//entity.PLACA = placa;
			//entity.MODELO_ID = modelo;
			//entity.MODELO_ANO = modelo_ano;
			//entity.ALTERNO_ID = alterno;
			//entity.SERIAL = serial;


			//dbContext.Delete<EQUIPOS_CAPS_REL>(item => item.EQUIPO_ID == id);

			//foreach (var objCapacidad in capacidades)
			//{
			//	entity.CAPACIDADES.Add(new EQUIPOS_CAPS_REL
			//	{
			//		EQUIPO_ID = id
			//													,
			//		PRODUCTO_CATEGORIA_ID = short.Parse(objCapacidad.categoria.ToString())
			//													,
			//		CAPACIDAD_ID = short.Parse(objCapacidad.capacidad.ToString())
			//													,
			//		VALOR = objCapacidad.valor
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

			return entity.ID;

		}

		

	}

}
