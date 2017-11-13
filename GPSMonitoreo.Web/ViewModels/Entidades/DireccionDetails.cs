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
    public class DireccionDetails
    {
		public int id { get; set; }
		
		public string tipos { get; set; }

		public string descripcion { get; set; }

		public string calle_principal { get; set; }

		public string calle_transversal { get; set; }

		public string numeracion { get; set; }

		public string ciudadela { get; set; }

		public string pais { get; set; }

		public string region { get; set; }

		public string provincia { get; set; }

		public string ciudad { get; set; }

		public string canton { get; set; }

		public string manzana { get; set; }

		public string cod_postal { get; set; }

		public string cerca { get; set; }

				
		// Se debe popular solo si from Entitiy se usa el parametro deep = true
		public List<TelefonoDetails> telefonos { get; set; }

		public List<UbicacionDetails> ubicaciones { get; set; }

		public List<ContactoDetails> contactos { get; set; }

		public void FromEntity(ENTIDADES_DIRS entity, bool deep = false)
		{
			Console.WriteLine("Debugging deep:" + deep);
			id = entity.ID;
			descripcion = entity.DESCRIPCION_LARGA;
			calle_principal = entity.CALLE_PRINCIPAL;
			calle_transversal = entity.CALLE_TRANSVERSAL;
			numeracion = entity.NUMERACION;
			ciudadela = entity.CIUDADELA;
			tipos = (entity.TIPOS_REL.Count > 0) ? string.Join(", ", entity.TIPOS_REL.Select(item => item.TIPO.DESCRIPCION_LARGA)) : "";
			pais = entity.PAIS.DESCRIPCION_LARGA;
			region = entity.REGION_ID.ToString();
			provincia = entity.PROVINCIA.DESCRIPCION_LARGA;
			ciudad = entity.CIUDAD.DESCRIPCION_LARGA;
			canton = entity.CANTON;
			manzana = entity.NUMERACION;
			cod_postal = entity.CODIGOPOSTAL;
			//NullPointerException: Debe ser definido como nullable a nivel del entity framework:
			cerca = entity.CERCA != null ? entity.CERCA.DESCRIPCION_LARGA : "";
			
			if (deep)
			{
				telefonos = new List<TelefonoDetails>();
				ubicaciones = new List<UbicacionDetails>();
				contactos = new List<ContactoDetails>();

				foreach (var telefono in entity.TELEFONOS.Where(item => item.UBICACION_ID == null))
				{
					telefonos.Add(TelefonoDetails.CreateFromEntity(telefono));
				}

				foreach (var ubicacion in entity.UBICACIONES.Where(item => item.UBICACION_ID == null))
				{
					ubicaciones.Add(UbicacionDetails.CreateFromEntity(ubicacion));
				}

				foreach (var contacto in entity.CONTACTOS_REL)
				{
					contactos.Add(ContactoDetails.CreateFromEntity(contacto.CONTACTO));
				}
			}
		}

		public static DireccionDetails CreateFromEntity(ENTIDADES_DIRS entity, bool deep = false)
		{
			var details = new DireccionDetails();
			details.FromEntity(entity, deep);
			return details;
		}

		public static DireccionDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id, bool deep = false)
		{
			return CreateFromEntity(dbContext.ENTIDADES_DIRS.FirstOrDefault(item => item.ID == id), deep);
		}

	}
}
