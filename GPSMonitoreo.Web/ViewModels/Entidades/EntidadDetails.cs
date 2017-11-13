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

namespace GPSMonitoreo.Web.ViewModels.Entidades
{
	public class EntidadDetails
	{
		public int id { get; set; }

		public string descripcion { get; set; }

		public string categoria { get; set; }

		public string relacion_empresa { get; set; }

		public string tipo_identificacion { get; set; }

		public string identificacion { get; set; }

		public string tipo { get; set; }

		public byte tipoId { get; set; }

		public byte tipo_id { get; set; }

		public List<TelefonoDetails> telefonos { get; set; }

		public List<DireccionDetails> direcciones { get; set; }

		public List<EmailDetails> emails { get; set; }

		public List<ContactoDetails> contactos { get; set; }

		public List<EntidadDetails> contactosde { get; set; }

		public string nombres { get; set; }

		public string apellidos { get; set; }

		public string razon_social { get; set; }


		public static EntidadDetails Load(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id)
		{
			var entity = dbContext.ENTIDADES
				.Include(item => item.IDENT_TIPO)
				.Include(item => item.TIPO)
				.FirstOrDefault(item => item.ID == id);


			if (entity != null)
			{
				return EntidadDetails.CreateFromEntity(entity, true);
			}
			return null;
		}


		public void FromEntity(ENTIDADES entity, bool deep = false)
		{
			string separador = ", ";
			string concat_cats = "";
			string concat_rels = "";

			/*MÉTODO TRADICIONAL PARA CONCATENAR ELEMENTOS DE UNA LISTA:
				foreach (var categoria in entity.CATS_RELS)
				{
					cat = categoria.CATEGORIA.DESCRIPCION_LARGA + "";
				}
			*/

			//Usando lambda expressions: (error por tipos... es un list<cats_rels> y no string... se deberia remapear a string el list)
			//string q = entity.CATS_RELS.Aggregate((i, j) => i.CATEGORIA.DESCRIPCION_LARGA + "," + j.CATEGORIA.DESCRIPCION_LARGA).ToString();


			//Para evitar ArgumentNullException o se puede manejar en un bloque TRY CATCH...
			//Usando lambda expressions y LINQ:
			concat_cats = entity.CATS_RELS.Count > 0 ? string.Join(separador, entity.CATS_RELS.Select(item => item.CATEGORIA.DESCRIPCION_LARGA)) : "";
			concat_rels = entity.RELS_REL.Count > 0 ? concat_rels = string.Join(separador, entity.RELS_REL.Select(item => item.ENTIDADES_RELS.DESCRIPCION_LARGA)) : "";

			id = entity.ID;
			descripcion = entity.DESCRIPCION_LARGA;
			categoria = concat_cats;
			relacion_empresa = concat_rels;
			tipo_identificacion = entity.IDENT_TIPO.DESCRIPCION_LARGA;
			tipoId = entity.IDENT_TIPO.ID;
			identificacion = entity.IDENTIFICACION;
			tipo = entity.TIPO.DESCRIPCION_LARGA;
			nombres = entity.NOMBRES;
			apellidos = entity.APELLIDOS;
			razon_social = entity.RAZON_SOCIAL;
			tipo_id = entity.TIPO.ID;
			

			if(deep)
			{
				telefonos = new List<TelefonoDetails>();
				direcciones = new List<DireccionDetails>();
				emails = new List<EmailDetails>();
				contactos = new List<ContactoDetails>();
				contactosde = new List<EntidadDetails>();

				foreach (var telefonoEntity in entity.TELEFONOS.Where(item => item.DIRECCION_ID == null && item.UBICACION_ID == null))
				{
					telefonos.Add(TelefonoDetails.CreateFromEntity(telefonoEntity));
				}
				foreach (var direccion in entity.DIRECCIONES)
				{
					direcciones.Add(DireccionDetails.CreateFromEntity(direccion));
				}
				foreach (var email in entity.EMAILS)
				{
					emails.Add(EmailDetails.CreateFromEntity(email));
				}
				
				//PERSONA:
				if (entity.TIPO.ID == 4)
				{
					foreach (var contacto_de in entity.CTOS_PERSONA)
					{
						contactosde.Add(CreateFromEntity(contacto_de.EMPRESA));
					}
				}
				//JURÍDICA. CONTACTOS DE LA EMPRESA Y PERSONA NATURAL
				if (entity.TIPO.ID == 1 || entity.TIPO.ID == 2)
				{
					foreach (var contacto in entity.CTOS_EMPRESA)
					{
						contactos.Add(ContactoDetails.CreateFromEntity(contacto));
					}
				}

				//Puede ser un contacto_persona o un contacto_empresa. Se itera en ambas:
				/*foreach (var contacto in entity.CTOS_PERSONA.Concat(entity.CTOS_EMPRESA))
				{
					details.contactos.Add(ContactoDetails.CreateFromEntity(contacto,true));
				}*/
			}
		}

		public static EntidadDetails CreateFromEntity(ENTIDADES entity, bool deep = false)
		{
			var details = new EntidadDetails();
			details.FromEntity(entity, deep);
			return details;
		}
	}
}
