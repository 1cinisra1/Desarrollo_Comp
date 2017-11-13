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

namespace GPSMonitoreo.Web.ViewModels.ControlVelocidad
{
	public class ControlVelocidadDetails
	{
		public int id { get; set; }

		public string descripcion { get; set; }


		public List<ReglaEvaluacionVelocidadDetails> relacion_reglas { get; set; }




		public static ControlVelocidadDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			string separador = ", ";

			string concat_cap = "";

			var entity = dbContext.EQUIPOS.FirstOrDefault(item => item.ID == id);

			if (entity != null)
			{

				//Para evitar ArgumentNullException o se puede manejar en un bloque TRY CATCH...
				//Usando lambda expressions y LINQ:
				////concat_cats = entity.CATS_RELS.Count > 0 ? string.Join(separador, entity.CATS_RELS.Select(item => item.CATEGORIA.DESCRIPCION_LARGA)) : "";
				////concat_rels = entity.RELS_REL.Count > 0 ? concat_rels = string.Join(separador, entity.RELS_REL.Select(item => item.ENTIDADES_RELS.DESCRIPCION_LARGA)) : "";
				concat_cap = entity.CAPACIDADES.Count > 0 ? concat_cap = string.Join(separador, entity.CAPACIDADES.Select(item => item.CAPACIDAD.DESCRIPCION_LARGA)) : "";
				var details = new ControlVelocidadDetails()
				{
					id = id,
					descripcion = entity.DESCRIPCION_LARGA
				};

				details.relacion_reglas = new List<ReglaEvaluacionVelocidadDetails>();


				foreach (var capacidadEntity in entity.CAPACIDADES)
				{
					details.relacion_reglas.Add(ReglaEvaluacionVelocidadDetails.CreateFromEntity(capacidadEntity));
				}
				////foreach (var direccion in entity.DIRECCIONES)
				////{
				////	details.direcciones.Add(DireccionDetails.CreateFromEntity(direccion));
				////}
				////foreach (var email in entity.EMAILS)
				////{
				////	details.emails.Add(EmailDetails.CreateFromEntity(email));
				////}

				return details;
			}

			return null;
		}
	}
}
