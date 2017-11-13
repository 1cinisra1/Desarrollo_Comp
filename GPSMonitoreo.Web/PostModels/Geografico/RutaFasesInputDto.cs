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
    public class RutaFasesInputDto: PostModel
    {

		//[Rules("Required", "DBKeyExists('RUTAS_CATS')")]

		[Rules("RequiredNumeric", "DBKeyExists('RUTAS')")]
		public int ruta { get; set; }


		[Rules("RequiredNumeric", "DBKeyExists('ENTIDADES')")]
		public int entidad { get; set; }

		public List<RutaFasesItemInputDto> fases { get; set; }



	

		public void Save(EntitiesContext dbContext)
		{


			//Utils.dump(this);

			dbContext.Delete<RUTAS_FASES_REL>(x => x.RUTA_ID == ruta && x.ENTIDAD_ID == entidad);

			foreach(var item in fases)
			{
				if(item.fase > 0)
					dbContext.RUTAS_FASES_REL.Add(new RUTAS_FASES_REL { RUTA_ID = ruta, ENTIDAD_ID = entidad, TIPO_ID = item.tipo, REL_ID = item.id, FASE_ID = item.fase });
			}

			dbContext.SaveChanges();
		}
	}

}
