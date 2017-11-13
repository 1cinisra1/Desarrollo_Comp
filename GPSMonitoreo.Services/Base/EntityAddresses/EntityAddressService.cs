using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.EntityAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.Entity;

namespace GPSMonitoreo.Services.Base.EntityAddresses
{
    public class EntityAddressService : BaseService
    {
		EntitiesContext _dbContext;

		public EntityAddressService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetListResult<List<EntityAddressListDto>>> GetListAsync(EntityAddressFilterInputDto input = null)
		{
			var result = new GetListResult<List<EntityAddressListDto>>();

			var query = _dbContext.ENTIDADES_DIRS.AsQueryable().AsNoTracking();

			if (input != null)
			{

				query = query.Where(x => x.ENTIDAD_ID == input.EntityId);

				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if(input.TypeIds?.Count > 0)
				{
					query = query.Where(x => x.TIPOS_REL.Any(r => input.TypeIds.Contains(r.TIPO_ID)));
				}

				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}


			if (input != null && input.Paging)
			{
				query = query.OrderBy(x => x.DESCRIPCION_LARGA)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.ENTIDADES_DIRS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderBy(x => x.DESCRIPCION_LARGA);

			var tempItems = await query.Select(x => new 
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				CountryDescription = x.PAIS.DESCRIPCION_LARGA,
				CityDescription = x.CIUDAD.DESCRIPCION_LARGA,
				Neighborhood = x.CIUDADELA,
				Street1 = x.CALLE_PRINCIPAL,
				EntityAddressTypes = x.TIPOS_REL.OrderBy(r => r.TIPO.DESCRIPCION_LARGA).Select(r => r.TIPO.DESCRIPCION_LARGA).ToList(),
				GeofenceId = x.CERCA_ID ?? 0,
				GeofenceDescription = x.CERCA.DESCRIPCION_LARGA
			}).ToListAsync();

			result.Items = tempItems.Select(x => new EntityAddressListDto
			{
				Id = x.Id,
				Description = x.Description,
				CountryDescription = x.CountryDescription,
				CityDescription = x.CityDescription,
				Neighborhood = x.Neighborhood,
				Street1 = x.Street1,
				EntityAddressTypes = string.Join(", ", x.EntityAddressTypes),
				GeofenceId = x.GeofenceId,
				GeofenceDescription = x.GeofenceDescription
			}).ToList();

			result.SetSuccess();

			return result;
		}
	}
}
