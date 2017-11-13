using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Brands;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Base.Brands
{
    public class BrandService : BaseService
    {
		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public BrandService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetResult<BrandInputDto>> GetByIdAsync(short id)
		{
			var result = new GetResult<BrandInputDto>();
			var entity = await _dbContext.MARCAS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new BrandInputDto();
				entity.AssignToDto(dto);
				dto.CategoryId = entity.CATEGORIA_ID;
				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}


		public async Task<GetListResult<List<BrandListDto>>> GetListAsync(BrandFilterInputDto input)
		{
			var result = new GetListResult<List<BrandListDto>>();

			var query = _dbContext.MARCAS.AsQueryable();

			string orderBy = null;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.OrderBy != null)
				{
					orderBy = input.OrderBy.GetOrderBy(new StringDictionary
					{
						{ "Id", "ID" },
						{ "Description", "DESCRIPCION_LARGA" }
					});
				}

				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}


			if (input != null && input.Paging)
			{
				if (!string.IsNullOrEmpty(orderBy))
				{
					query = query.OrderBy(orderBy);
				}

				query = query.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.MARCAS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			result.Items = await query.Select(x => new BrandListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				CategoryDescription = x.CATEGORIA.DESCRIPCION_LARGA
			}).ToListAsync();


			result.SetSuccess();

			return result;
		}


		
		public InputValidationResult Validate(BrandInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

				}
			});

			return result;
		}

		private void _PrepareEntity(BrandInputDto input, MARCAS entity)
		{
			input.AssignToEntity(entity);
			entity.CATEGORIA_ID = input.CategoryId;
		}

		public async Task<CreateUpdateResult<short>> CreateAsync(BrandInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = new MARCAS();

			_PrepareEntity(input, entity);

			_dbContext.MARCAS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<short>> UpdateAsync(BrandInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = await _dbContext.MARCAS.FirstOrDefaultAsync(x => x.ID == input.Id);

			if (entity != null)
			{
				_PrepareEntity(input, entity);

				await _dbContext.SaveChangesAsync();

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

				result.Id = input.Id;
			}
			else
			{
				result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			}

			return result;
		}
	}
}
