using GPSMonitoreo.Dtos.Base.AlarmGrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using GPSMonitoreo.Services.Validation;
using System.Text.RegularExpressions;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using System.Collections.Specialized;

namespace GPSMonitoreo.Services.Base.AlarmGrades
{
    public class AlarmGradeService : BaseService
	{
		private GPSMonitoreo.Data.Models.EntitiesContext _dbContext;

		public AlarmGradeService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetResult<AlarmGradeInputDto>> GetByIdAsync(byte id)
		{
			var result = new GetResult<AlarmGradeInputDto>();
			var entity = await _dbContext.ALARMAS_GRADOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new AlarmGradeInputDto();

				entity.AssignToDto(dto);
				dto.Color = entity.COLOR;
				dto.Blinking = entity.PARPADEANTE;

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}


		public async Task<GetListResult<List<AlarmGradeListDto>>> GetListAsync(AlarmGradeFilterInputDto input)
		{
			var result = new GetListResult<List<AlarmGradeListDto>>();

			List<AlarmGradeListDto> items = null;

			var query = _dbContext.ALARMAS_GRADOS.AsQueryable();

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
					.Join(_dbContext.ALARMAS_GRADOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			items = await query.Select(x => new AlarmGradeListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				Color = x.COLOR,
				Blinking = x.PARPADEANTE
			}).ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		public InputValidationResult Validate(AlarmGradeInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if (!errors.ContainsKey(nameof(input.Color)))
					{
						var re = new Regex(@"^#[0-9a-f]{3}([0-9a-f]{3})?$", RegexOptions.IgnoreCase);

						if (!re.IsMatch(input.Color))
						{
							errors.AddError(nameof(input.Color), "Formato inválido");
						}
					}
				}
			});

			return result;
		}

		private void PrepareEntity(AlarmGradeInputDto input, ALARMAS_GRADOS entity)
		{
			input.AssignToEntity(entity);
			entity.COLOR = input.Color;
			entity.PARPADEANTE = input.Blinking;
		}

		public async Task<CreateUpdateResult<byte>> CreateAsync(AlarmGradeInputDto input)
		{
			var result = new CreateUpdateResult<byte>();

			var entity = new ALARMAS_GRADOS();

			PrepareEntity(input, entity);

			_dbContext.ALARMAS_GRADOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<byte>> UpdateAsync(AlarmGradeInputDto input)
		{
			var result = new CreateUpdateResult<byte>();

			var entity = await _dbContext.ALARMAS_GRADOS.FirstOrDefaultAsync(x => x.ID == input.Id);

			if (entity != null)
			{
				PrepareEntity(input, entity);

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
