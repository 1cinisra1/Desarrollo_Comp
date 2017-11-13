using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Sections;
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
using GPSMonitoreo.Dtos.Base.Geofences;
using GPSMonitoreo.Services.Utils;

namespace GPSMonitoreo.Services.Base.Sections
{
    public class SectionService : BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public SectionService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void PrepareEntity(SectionInputDto input, TRAMOS entity)
		{
			input.AssignToEntity(entity);
			entity.AUXILIAR_ID = input.AuxiliaryId;
			entity.CATEGORIA_ID = input.CategoryId;
		}

		private void AddRelations(SectionInputDto input, TRAMOS entity)
		{
			var order = (short)1;

			foreach (var segment in input.Segments)
			{
				entity.TRAMO_SEGMENTOS.Add(new TRAMOS_SEGMENTOS { SEGMENTO_ID = segment.Id, ORDEN = order++ });
			}
		}

		public InputValidationResult Validate(SectionInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

					if (!input.AuxiliaryId.IsNullOrWhiteSpace())
					{
						if(_dbContext.TRAMOS.Any(x => x.ID != input.Id && x.AUXILIAR_ID.ToLower() == input.AuxiliaryId.ToLower()))
						{
							errors.AddError("AuxiliaryId", "Código auxiliar ya existe");
						}
					}

					if(!errors.ContainsKey("Segments") && input.Segments != null)
					{
						var validationErrors = new InputCollectionValidationError();

						var x = 0;
						int matchIndex;

						foreach(var segment in input.Segments)
						{
							if(x > 0)
							{
								matchIndex = input.Segments.FindLastIndex(x - 1, r => r.Id == segment.Id);

								if(matchIndex > -1)
								{
									validationErrors.AddItemError(x, Resources.ServicesErrorMessages.CollectionItemDuplicated + ": " + segment.Id);
								}
							}

							x++;
						}

						if(validationErrors.ItemsErrors.Count > 0)
						{
							validationErrors.Error = Dtos.Resources.ValidationErrors.ValidateCollection;
							errors.AddCollectionError("Segments", validationErrors);
						}
					}
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<int>> CreateAsync(SectionInputDto input)
		{
			var result = new CreateUpdateResult<int>();
			var entity = new TRAMOS();

			PrepareEntity(input, entity);
			AddRelations(input, entity);

			_dbContext.TRAMOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(SectionInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.TRAMOS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<TRAMOS_SEGMENTOS>(x => x.TRAMO_ID == input.Id);

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

		public async Task<GetResult<SectionInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<SectionInputDto>();
			var entity = await _dbContext.TRAMOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new SectionInputDto();
				entity.AssignToDto(dto);
				dto.AuxiliaryId = entity.AUXILIAR_ID;
				dto.CategoryId = entity.CATEGORIA_ID;


				dto.Segments = _dbContext.TRAMOS_SEGMENTOS.Where(x => x.TRAMO_ID == id).OrderBy(x => x.ORDEN).Select(x => new CommonBaseWithAuxiliarListInputDto<int>
				{
					Id = x.SEGMENTO_ID,
					AuxiliaryId = x.SEGMENTO.AUXILIAR_ID,
					Description = x.SEGMENTO.DESCRIPCION_LARGA
				}).ToList();

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		public async Task<GetListResult<List<SectionListDto>>> GetListAsync(SectionFilterInputDto input)
		{
			var result = new GetListResult<List<SectionListDto>>();

			List<SectionListDto> items = null;

			var query = _dbContext.TRAMOS.AsQueryable();

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
						.Join(_dbContext.TRAMOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
					;
				}

				query = query.OrderByDescending(x => x.ID);


				items = await query.Select(x => new SectionListDto
				{
					Id = x.ID,
					AuxiliaryId = x.AUXILIAR_ID,
					Description = x.DESCRIPCION_LARGA,
					CategoryDescription = _dbContext.FN_HIERARCHY_PATH(typeof(TRAMOS_CATS).Name, x.CATEGORIA_ID),
				}).ToListAsync();
			}

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_TRAMO_CERCAS", id);

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllOtherGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_TRAMO_CERCAS_OTRAS", id);

			return result;
		}
	}
}
