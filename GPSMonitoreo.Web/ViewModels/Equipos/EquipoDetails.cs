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

namespace GPSMonitoreo.Web.ViewModels.Equipos
{
	public class EquipoDetails
	{
		public int id { get; set; }

		public string descripcion { get; set; }

		public string estado_opera { get; set; }

		public string grupo { get; set; }

		public string categoria { get; set; }

		public string placa { get; set; }

		public string marca { get; set; }

		public short modelo_ano { get; set; }

		public string serial { get; set; }

		public string modelo { get; set; }

		public List<CapacidadDetails> relacion_capacidades { get; set; }

		////public List<EmailDetails> relacion_2 { get; set; }

		////public List<EmailDetails> relacion_3 { get; set; }



		public static EquipoDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			string separador = ", ";
			string concat_cats = "";
			string concat_rels = "";

			string concat_cap = "";

			var entity = dbContext.EQUIPOS.FirstOrDefault(item => item.ID == id);

			if (entity != null)
			{

				//Para evitar ArgumentNullException o se puede manejar en un bloque TRY CATCH...
				//Usando lambda expressions y LINQ:
				////concat_cats = entity.CATS_RELS.Count > 0 ? string.Join(separador, entity.CATS_RELS.Select(item => item.CATEGORIA.DESCRIPCION_LARGA)) : "";
				////concat_rels = entity.RELS_REL.Count > 0 ? concat_rels = string.Join(separador, entity.RELS_REL.Select(item => item.ENTIDADES_RELS.DESCRIPCION_LARGA)) : "";
				concat_cap = entity.CAPACIDADES.Count > 0 ? concat_cap = string.Join(separador, entity.CAPACIDADES.Select(item => item.CAPACIDAD.DESCRIPCION_LARGA)) : "";
				var details = new EquipoDetails()
				{
					id = id,
					descripcion = entity.DESCRIPCION_LARGA,
					estado_opera = entity.ESTADO_OPERACION.DESCRIPCION_LARGA,
					grupo = entity.GRUPO.DESCRIPCION_LARGA,
					categoria = entity.CATEGORIA.DESCRIPCION_LARGA,
					placa = entity.PLACA,
					marca = entity.MARCA.DESCRIPCION_LARGA,
					modelo_ano = entity.MODELO_ANO,
					serial = entity.SERIAL,
					modelo = entity.MODELO.DESCRIPCION_LARGA
				};

				details.relacion_capacidades = new List<CapacidadDetails>();
				////details.relacion_2 = new List<EmailDetails>();
				////details.relacion_3 = new List<EmailDetails>();

				foreach (var capacidadEntity in entity.CAPACIDADES)
				{
					details.relacion_capacidades.Add(CapacidadDetails.CreateFromEntity(capacidadEntity));
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
