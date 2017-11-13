using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Sections;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using System.Data.Entity;
using GPSMonitoreo.Libraries.Extensions.StringExtensions;
using GPSMonitoreo.Dtos.Base.EquipmentCapabilities;
using GPSMonitoreo.Services.Utils;
using System.Collections.Specialized;

namespace GPSMonitoreo.Services.Base.EquipmentCapabilities
{
    public class EquipmentCapabilityService : BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public EquipmentCapabilityService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void _PrepareEntity(EquipmentCapabilityInputDto input, EQUIPOS_CAPS entity)
		{
			input.AssignToEntity(entity);
			entity.UNIDAD_ID = input.MeasureUnitId;
		}

		

		public InputValidationResult Validate(EquipmentCapabilityInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

					
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<short>> CreateAsync(EquipmentCapabilityInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = new EQUIPOS_CAPS();

			_PrepareEntity(input, entity);

			_dbContext.EQUIPOS_CAPS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<short>> UpdateAsync(EquipmentCapabilityInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = _dbContext.EQUIPOS_CAPS.FirstOrDefault(x => x.ID == input.Id);

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

		public async Task<GetResult<EquipmentCapabilityInputDto>> GetByIdAsync(short id)
		{
			var result = new GetResult<EquipmentCapabilityInputDto>();
			var entity = await _dbContext.EQUIPOS_CAPS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new EquipmentCapabilityInputDto();
				entity.AssignToDto(dto);
				dto.MeasureUnitId = entity.UNIDAD_ID;

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		public async Task<GetListResult<List<EquipmentCapabilityListDto>>> GetListAsync(EquipmentCapabilityFilterInputDto input)
		{
			var result = new GetListResult<List<EquipmentCapabilityListDto>>();

			var query = _dbContext.EQUIPOS_CAPS.AsQueryable();

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
					.Join(_dbContext.EQUIPOS_CAPS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			result.Items = await query.Select(x => new EquipmentCapabilityListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				MeasureUnitDescription = x.UNIDAD.DESCRIPCION_LARGA,
				Hierarchy = _dbContext.FN_HIERARCHY_PATH<short>(nameof(EQUIPOS_CAPS), x.ID)
			}).ToListAsync();

			result.SetSuccess();

			return result;
		}
	}
}
