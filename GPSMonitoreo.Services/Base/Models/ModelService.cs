using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Models;
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

namespace GPSMonitoreo.Services.Base.Models
{
    public class ModelService : BaseService
    {
		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public ModelService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetResult<ModelInputDto>> GetByIdAsync(short id)
		{
			var result = new GetResult<ModelInputDto>();
			var entity = await _dbContext.MODELOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new ModelInputDto();
				entity.AssignToDto(dto);
				dto.BrandId = entity.MARCA_ID;
				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		private IQueryable<Data.Models.MODELOS> _GetQueryable(ModelFilterInputDto input, ref int recordCount)
		{
			var query = _dbContext.MODELOS.AsQueryable();

			string orderBy = null;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.BrandId > 0)
				{
					query = query.Where(x => x.MARCA_ID == input.BrandId);
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
					recordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
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
					.Join(_dbContext.MODELOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			return query;
		}

		public async Task<GetListResult<List<ModelListDto>>> GetListAsync(ModelFilterInputDto input)
		{
			var result = new GetListResult<List<ModelListDto>>();

			int recordCount = 0;

			var query = _GetQueryable(input, ref recordCount);

			result.RecordCount = recordCount;

			result.Items = await query.Select(x => new ModelListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				BrandDescription = x.MARCA.DESCRIPCION_LARGA,
			}).ToListAsync();

			result.SetSuccess();

			return result;
		}

		public async Task<GetListResult<List<CommonBaseSimpleListDto<short>>>> GetSimpleListAsync(ModelFilterInputDto input)
		{
			var result = new GetListResult<List<CommonBaseSimpleListDto<short>>>();

			int recordCount = 0;

			var query = _GetQueryable(input, ref recordCount);

			result.RecordCount = recordCount;

			result.Items = await query.Select(x => new CommonBaseSimpleListDto<short>
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA
			}).ToListAsync();

			result.SetSuccess();

			return result;
		}

		public InputValidationResult Validate(ModelInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

				}
			});

			return result;
		}

		private void _PrepareEntity(ModelInputDto input, MODELOS entity)
		{
			input.AssignToEntity(entity);
			entity.MARCA_ID = input.BrandId;
		}

		public async Task<CreateUpdateResult<short>> CreateAsync(ModelInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = new MODELOS();

			_PrepareEntity(input, entity);

			_dbContext.MODELOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<short>> UpdateAsync(ModelInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = await _dbContext.MODELOS.FirstOrDefaultAsync(x => x.ID == input.Id);

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
