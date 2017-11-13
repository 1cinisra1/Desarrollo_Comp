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


namespace GPSMonitoreo.Web.ViewModels.Entidades
{
    public class UbicacionDetails
    {
		public int id { get; set; }
		public string descripcion { get; set; }
		public string tipos { get; set; }
		public List<TelefonoDetails> telefonos { get; set; }
		public List<UbicacionDetails> ubicaciones { get; set; }


		public void FromEntity(ENTIDADES_DIRS_UBICAS entity, bool deep = false)
		{
			id = entity.ID;
			descripcion = entity.DESCRIPCION_LARGA;
			tipos = (entity.TIPOS.Count > 0) ? string.Join(", ", entity.TIPOS.Select(item => item.TIPO.DESCRIPCION_LARGA)) : "";


			if(deep)
			{
				telefonos = new List<TelefonoDetails>();
				ubicaciones = new List<UbicacionDetails>();

				//foreach (var telefono in entity.TELEFONOS.Where(item => item.UBICACION_ID == null))
				foreach (var telefono in entity.TELEFONOS.Where(item => item.UBICACION_ID == id))
				{
					telefonos.Add(TelefonoDetails.CreateFromEntity(telefono));
				}
				Console.WriteLine("-------------:" + id);

				foreach (var ubicacion in entity.HIJOS)
				{
					Console.WriteLine("-----------:" + ubicacion.DESCRIPCION_LARGA);
					ubicaciones.Add(CreateFromEntity(ubicacion));
				}

				Console.WriteLine("------------||-");
			}
		}

		public static UbicacionDetails CreateFromEntity(ENTIDADES_DIRS_UBICAS entity, bool deep = false)
		{
			var details = new UbicacionDetails();
			details.FromEntity(entity, deep);
			return details;
		}

		public static UbicacionDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id, bool deep = false)
		{
			Console.WriteLine("*******************DEPURANDO EL ID*******************");
			Console.WriteLine("id: " + id);
			return CreateFromEntity(dbContext.ENTIDADES_DIRS_UBICAS.FirstOrDefault(item => item.ID == id), deep);
		}
	}
}
