using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
//using System.Linq.Dynamic.Core;
using MVCHelpers.ViewModels.Validators;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Data.Models;

using System.Data.Entity;


namespace GPSMonitoreo.Web.PostModels.Geografico
{
    public class RutasFasesInputDto: PostModel
    {

		//[Rules("Required", "DBKeyExists('RUTAS_CATS')")]

		[Rules("RequiredNumeric", "DBKeyExists('RUTAS')")]
		public int ruta { get; set; }


		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int entidad { get; set; }

		public List<RutasFasesCercaFaseInputDto> cercasfases { get; set; }



	

		//public void Save(EntitiesContext dbContext)
		//{


		//	Utils.dump(this);

		//	dbContext.Delete<RUTAS_CERCAS_FASES>(x => x.RUTA_ID == ruta && x.ENTIDAD_ID == entidad);

		//	foreach(var cercafase in cercasfases)
		//	{
		//		if(cercafase.cerca > 0 && cercafase.fase > 0)
		//			dbContext.RUTAS_CERCAS_FASES.Add(new RUTAS_CERCAS_FASES() { RUTA_ID = ruta, ENTIDAD_ID = entidad, CERCA_ID = cercafase.cerca, FASE_ID = cercafase.fase });
		//	}

		//	dbContext.SaveChanges();
		//}

		//public static RutasEdit Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		//{


		//	//var r = from item in dbContext.RUTAS.Where(item => item.ID == id).Include(b => b.TRAMOS)
		//	//		select new {id= item.ID, descripcion = item.DESCRIPCION_LARGA, tramos = item.TRAMOS.Select(sub => new { sub.TRAMO_ID }).ToList()};


		//	var entity = dbContext.RUTAS.Where(item => item.ID == id).FirstOrDefault();
			
		//	//dbContext.RUTAS.Where(item => item.ID == id).

		//	//dbContext.Entry(entity).Collection(item => item.TRAMOS).EntityEntry.Reference("TRAMO").Load();
		//	//dbContext.Entry(entity).Collection(item => item.TRAMOS).EntityEntry.
		//	//dbContext.Entry(entity.TRAMOS).Reference("TRAMO").Load();

		//	if (entity != null)
		//	{
		//		var model = new RutasEdit()
		//		{
		//			id = entity.ID,
		//			descripcion_larga = entity.DESCRIPCION_LARGA,
		//			abreviacion = entity.ABREVIACION,
		//			descripcion_corta = entity.DESCRIPCION_CORTA,
		//			descripcion_mediana = entity.DESCRIPCION_MED,
		//			observaciones = entity.OBSERVACIONES,
		//			categoria = entity.CATEGORIA_ID,
		//			//tramos = entity.TRAMOS.Select(item => new { id = item.TRAMO_ID, descripcion = item.TRAMO.DESCRIPCION_LARGA}).ToArray()
		//			//tramos = entity.TRAMOS.AsQueryable().Include(p => p.TRAMO).Select(item => item.TRAMO.DESCRIPCION_LARGA).ToArray()
		//			tramos = dbContext.RUTAS_TRAMOS.Where(item => item.RUTA_ID == id).OrderBy(item => item.ORDEN).Include(item => item.TRAMO).Select(item => new { id = item.TRAMO.ID, descripcion = item.TRAMO.DESCRIPCION_LARGA }).ToArray()
		//			//tramos = new object[] { new { count = entity.RUTAS_TRAMOS.Count() } }
		//		};
		//		return model;
		//	}

		//	return null;
		//}
	}

}
