using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using System.Data.Entity;
using GPSMonitoreo.Libraries.Extensions.StringExtensions;
using GPSMonitoreo.Dtos.Base.Segments;
using GPSMonitoreo.Dtos.Base.Geofences;
using GPSMonitoreo.Services.Utils;

namespace GPSMonitoreo.Services.Base.Segments
{
    public class SegmentService : BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public SegmentService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void PrepareEntity(SegmentInputDto input, SEGMENTOS entity)
		{
			input.AssignToEntity(entity);
			entity.AUXILIAR_ID = input.AuxiliaryId;
			entity.CATEGORIA_ID = input.CategoryId;
		}

		private void AddRelations(SegmentInputDto input, SEGMENTOS entity)
		{
			var order = (short)1;

			foreach (var geofence in input.Geofences)
			{
				entity.SEGMENTO_CERCAS.Add(new SEGMENTOS_CERCAS { CERCA_ID = geofence.Id, ORDEN = order++ });
			}
		}

		public InputValidationResult Validate(SegmentInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if (!input.AuxiliaryId.IsNullOrWhiteSpace())
					{
						if(_dbContext.SEGMENTOS.Any(x => x.ID != input.Id && x.AUXILIAR_ID.ToLower() == input.AuxiliaryId.ToLower()))
						{
							errors.AddError("AuxiliaryId", "Código auxiliar ya existe");
						}
					}

					if (!errors.ContainsKey("Geofences") && input.Geofences != null)
					{
						var validationErrors = new InputCollectionValidationError();

						var x = 0;
						int matchIndex;

						foreach(var geofence in input.Geofences)
						{
							if(x > 0)
							{
								matchIndex = input.Geofences.FindLastIndex(x - 1, r => r.Id == geofence.Id);

								if(matchIndex > -1)
								{
									validationErrors.AddItemError(x, Resources.ServicesErrorMessages.CollectionItemDuplicated + ": " + geofence.Id);
								}
							}

							x++;
						}

						if(validationErrors.ItemsErrors.Count > 0)
						{
							validationErrors.Error = Dtos.Resources.ValidationErrors.ValidateCollection;
							errors.AddCollectionError("Geofences", validationErrors);
						}
					}
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<int>> CreateAsync(SegmentInputDto input)
		{
			var result = new CreateUpdateResult<int>();
			var entity = new SEGMENTOS();

			PrepareEntity(input, entity);
			AddRelations(input, entity);

			_dbContext.SEGMENTOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(SegmentInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.SEGMENTOS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<SEGMENTOS_CERCAS>(x => x.SEGMENTO_ID == input.Id);

				PrepareEntity(input, entity);

				AddRelations(input, entity);

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

		public async Task<GetResult<SegmentInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<SegmentInputDto>();
			var entity = await _dbContext.SEGMENTOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new SegmentInputDto();
				entity.AssignToDto(dto);
				dto.AuxiliaryId = entity.AUXILIAR_ID;
				dto.CategoryId = entity.CATEGORIA_ID;

				dto.Geofences = _dbContext.SEGMENTOS_CERCAS.Where(x => x.SEGMENTO_ID == id).OrderBy(x => x.ORDEN).Select(x => new CommonBaseWithAuxiliarListInputDto<int>
				{
					Id = x.CERCA_ID,
					AuxiliaryId = x.CERCA.AUXILIAR_ID,
					Description = x.CERCA.DESCRIPCION_LARGA
				}).ToList();

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		public async Task<GetListResult<List<SegmentListDto>>> GetListAsync(SegmentFilterInputDto input)
		{
			var result = new GetListResult<List<SegmentListDto>>();

			List<SegmentListDto> items = null;

			var query = _dbContext.SEGMENTOS.AsQueryable();

			if (input != null)
			{

				if (!string.IsNullOrWhiteSpace(input.AuxiliaryId))
				{
					query = query.Where(item => item.AUXILIAR_ID.ToLower().Contains(input.AuxiliaryId.ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description.ToLower()));
				}

				
				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();

					query = query.OrderByDescending(x => x.ID)
						.Select(x => new { x.ID })
						.Skip(input.Page * input.PageSize).Take(input.PageSize)
						.Join(_dbContext.SEGMENTOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
					;
				}

				query = query.OrderByDescending(x => x.ID);


				items = await query.Select(x => new SegmentListDto
				{
					Id = x.ID,
					AuxiliaryId = x.AUXILIAR_ID,
					Description = x.DESCRIPCION_LARGA,
					CategoryDescription = _dbContext.FN_HIERARCHY_PATH(typeof(SEGMENTOS_CATS).Name, x.CATEGORIA_ID),
				}).ToListAsync();
			}

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_SEGMENTO_CERCAS", id);

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllOtherGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_SEGMENTO_CERCAS_OTRAS", id);

			return result;
		}
	}
}
