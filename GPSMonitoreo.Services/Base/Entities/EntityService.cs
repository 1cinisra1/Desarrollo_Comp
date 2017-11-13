using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.Entity;

namespace GPSMonitoreo.Services.Base.Entities
{
    public class EntityService : BaseService
    {


		EntitiesContext _dbContext;

		public EntityService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetListResult<List<EntityListDto>>> GetListAsync(EntityFilterInputDto input = null)
		{
			var result = new GetListResult<List<EntityListDto>>();

			var query = _dbContext.ENTIDADES.AsQueryable().AsNoTracking();

			List<EntityListDto> items;


			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.IdentificationTypeId > 0)
				{
					query = query.Where(item => item.IDENT_TIPO_ID == input.IdentificationTypeId);
				}

				if (!string.IsNullOrWhiteSpace(input.Identification))
				{
					query = query.Where(item => item.IDENTIFICACION.ToLower().Contains(input.Identification.ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(input.Names))
				{
					query = query.Where(item => item.NOMBRES.ToLower().Contains(input.Names.ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(input.LastNames))
				{
					query = query.Where(item => item.APELLIDOS.ToLower().Contains(input.LastNames.ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(input.BusinessName))
				{
					query = query.Where(item => item.RAZON_SOCIAL == input.BusinessName);
				}


				if (input.EntityTypeId > 0)
				{
					query = query.Where(item => item.TIPO_ID == input.EntityTypeId);
				}

				//Categorías pueden ser varias para una entidad:
				if (input.CategoryIds?.Count > 0)
				{
					query = query.Where(item => item.CATS_RELS.Any(item2 => input.CategoryIds.Contains(item2.CATEGORIA_ID)));
				}


				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}


			if (input != null && input.Paging)
			{
				query = query.OrderByDescending(x => x.ID)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.ENTIDADES.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderByDescending(x => x.ID);

			items = await query.Select(x => new EntityListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				BusinessName = x.RAZON_SOCIAL,
				EntityTypeId = x.TIPO_ID,
				EntityTypeDescription = x.TIPO.DESCRIPCION_LARGA,
				IdentificationTypeDescription = x.IDENT_TIPO.DESCRIPCION_LARGA,
				Identification = x.IDENTIFICACION,
				Names = x.NOMBRES,
				LastNames = x.APELLIDOS
			}).ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}


		public async Task<GetListResult<List<EntityEmailListDto>>> GetEmailListAsync(EntityEmailFilterInputDto input)
		{
			var result = new GetListResult<List<EntityEmailListDto>>();

			var query = _dbContext.ENTIDADES_EMAILS.AsQueryable().AsNoTracking();

			List<EntityEmailListDto> items;

			query = query.Where(x => x.ENTIDAD_ID == input.EntityId);

			if (input.Paging)
			{
				query = query.OrderBy(x => x.EMAIL)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.ENTIDADES_EMAILS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderBy(x => x.EMAIL);

			//items = await query.Select(x => new EntityEmailListDto
			//{
			//	Id = x.ID,
			//	Email = x.EMAIL,
			//	EmailTypeDescription = x.TIPO.DESCRIPCION_LARGA,
			//	//EmailPurposes = x.PROPOSITOS.OrderBy(p => p.PROPOSITO.DESCRIPCION_LARGA).Select(p => p.PROPOSITO.DESCRIPCION_LARGA).Aggregate((a, b) => a + ", " + b)
			//	//(a, b) => a + "," + b
			//	EmailPurposes = string.Join(", ", x.PROPOSITOS.OrderBy(p => p.EMAIL).Select(p => p.PROPOSITO.DESCRIPCION_LARGA).ToList())
			//}).ToListAsync();

			var tempItems = await query.Select(x => new 
			{
				Id = x.ID,
				Email = x.EMAIL,
				EmailTypeDescription = x.TIPO.DESCRIPCION_LARGA,
				//EmailPurposes = x.PROPOSITOS.OrderBy(p => p.PROPOSITO.DESCRIPCION_LARGA).Select(p => p.PROPOSITO.DESCRIPCION_LARGA).Aggregate((a, b) => a + ", " + b)
				//(a, b) => a + "," + b
				EmailPurposes = x.PROPOSITOS.OrderBy(p => p.PROPOSITO.DESCRIPCION_LARGA).Select(p => p.PROPOSITO.DESCRIPCION_LARGA).ToList()
			}).ToListAsync();


			result.Items = tempItems.Select(x => new EntityEmailListDto
			{
				Id = x.Id,
				Email = x.Email,
				EmailTypeDescription = x.EmailTypeDescription,
				EmailPurposes = string.Join(", ", x.EmailPurposes.ToList())
			}).ToList();

			result.SetSuccess();

			return result;
		}

	}
}
