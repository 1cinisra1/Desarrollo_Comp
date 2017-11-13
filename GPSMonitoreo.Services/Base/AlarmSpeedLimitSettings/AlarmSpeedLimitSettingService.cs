using GPSMonitoreo.Dtos.Base.AlarmSpeedLimitSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using GPSMonitoreo.Services.Validation;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Extensions.EntityExtensions;

namespace GPSMonitoreo.Services.Base.AlarmSpeedLimitSettings
{
    public class AlarmSpeedLimitSettingService : BaseService
	{

		private GPSMonitoreo.Data.Models.EntitiesContext _dbContext;

		public AlarmSpeedLimitSettingService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private List<SpeedLimitRangeInputDto> _GetSpeedRanges(ALARMAS_VELOCIDAD entity, byte mode)
		{
			return entity.RANGOS_VELOCIDADES.Where(x => x.MODO == mode).OrderBy(x => x.ORDEN).Select(x => new SpeedLimitRangeInputDto
			{
				From = x.VELOCIDAD_DESDE,
				To = x.VELOCIDAD_HASTA,
				TimeRangeSettings = x.RANGOS_TIEMPOS.OrderBy(t => t.ORDEN).Select(t => new TimeRangeSettingInputDto
				{
					From = t.TIEMPO_DESDE,
					To = t.TIEMPO_HASTA,
					Points = t.PUNTOS,
					GradeId = t.GRADO_ID,
					Blinking = t.PARPADEANTE,
					Audible = t.AUDIBLE
				}).ToList()
			}).ToList();
		}

		public async Task<GetResult<AlarmSpeedLimitSettingInputDto>> GetByIdAsync(short id)
		{
			var result = new GetResult<AlarmSpeedLimitSettingInputDto>();
			var entity = await _dbContext.ALARMAS_VELOCIDAD.AsNoTracking()
				.Include(x => x.RANGOS_VELOCIDADES)
				.FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new AlarmSpeedLimitSettingInputDto();

				entity.AssignToDto(dto);

				dto.SpeedLimitAudible = entity.LIMITE_AUDIBLE;
				dto.SpeedLimitStandard = entity.LIMITE_ESTANDAR;
				dto.SpeedLimitTolerance = entity.LIMITE_TOLERANCIA;
				dto.SpeedLimitRoad = entity.LIMITE_MOP;
				dto.SpeedLimitRoadRestricted = entity.LIMITE_MOPREST;

				dto.LowSpeed = _GetSpeedRanges(entity, 1);
				dto.HiSpeed = _GetSpeedRanges(entity, 2);

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		public async Task<GetListResult<List<AlarmSpeedLimitSettingListDto>>> GetListAsync(AlarmSpeedLimitSettingFilterInputDto input)
		{
			var result = new GetListResult<List<AlarmSpeedLimitSettingListDto>>();

			List<AlarmSpeedLimitSettingListDto> items = null;

			var query = _dbContext.ALARMAS_VELOCIDAD.AsQueryable();

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
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
					.Join(_dbContext.ALARMAS_VELOCIDAD.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderByDescending(x => x.ID);

			items = await query.Select(x => new AlarmSpeedLimitSettingListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				SpeedLimitStandard = x.LIMITE_ESTANDAR,
				SpeedLimitTolerance = x.LIMITE_TOLERANCIA
			}).ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		private string _ValidateSpeedRanges(AlarmSpeedLimitSettingInputDto input, byte mode, List<SpeedLimitRangeInputDto> ranges)
		{
			short prevTo = 0;

			foreach (var range in ranges)
			{
				if (range.From <=0 || range.To <= 0 )
				{
					return "El valor Desde y Hasta deben ser superior a 0";
				}

				if (range.To <= range.From )
				{
					return "El valor Hasta debe ser superior al valor Desde";
				}

				if (prevTo > 0 && ((range.From - prevTo) != 1))
				{
					return "El valor Desde debe ser igual al valor Hasta del rango anterior + 1";
				}
				prevTo = range.To;
			}

			if (input.SpeedLimitStandard > 0)
			{
				if (mode == 1)//lowspeed
				{
					if (ranges.Last().To >= (input.SpeedLimitStandard - input.SpeedLimitTolerance))
					{
						return "El valor Hasta del último rango debe ser inferior que (Velocidad Standard - Tolerancia)";
					}
				}
				else
				{
					if (ranges.First().From <= (input.SpeedLimitStandard + input.SpeedLimitTolerance))
					{
						return "El valor Desde del primer rango debe ser superior que (Velocidad Standard + Tolerancia)";
					}
				}
			}

			foreach (var range in ranges)
			{
				foreach (var timeRange in range.TimeRangeSettings)
				{
					if (timeRange.GradeId < 1)
					{
						return "El grado de alarma es requerido en todas las configuraciones";
					}
				}
			}

			return null;
		}


		public InputValidationResult Validate(AlarmSpeedLimitSettingInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					string speedRangeError;

					if (!errors.ContainsKey(nameof(input.LowSpeed)))
					{
						speedRangeError = _ValidateSpeedRanges(input, 1, input.LowSpeed);

						if (!string.IsNullOrEmpty(speedRangeError))
						{
							errors.AddError(nameof(input.LowSpeed), speedRangeError);
						}
					}

					if (!errors.ContainsKey(nameof(input.HiSpeed)))
					{
						speedRangeError = _ValidateSpeedRanges(input, 2, input.HiSpeed);

						if (!string.IsNullOrEmpty(speedRangeError))
						{
							errors.AddError(nameof(input.HiSpeed), speedRangeError);
						}
					}
				}
			});

			return result;
		}

		private void _AddSpeedRanges(ALARMAS_VELOCIDAD entity, byte mode, List<SpeedLimitRangeInputDto> speedRanges)
		{
			byte speedRangeOrder = 1;
			byte timeRangeOrder;

			ALARMAS_VELOCIDAD_RANG speedRangeEntity;


			foreach (var speedRange in speedRanges)
			{
				timeRangeOrder = 1;

				speedRangeEntity = new ALARMAS_VELOCIDAD_RANG
				{
					MODO = mode,
					ORDEN = speedRangeOrder,
					VELOCIDAD_DESDE = speedRange.From,
					VELOCIDAD_HASTA = speedRange.To,
					ALARMAS_VELOCIDAD = entity
				};

				entity.RANGOS_VELOCIDADES.Add(speedRangeEntity);

				foreach (var timeRange in speedRange.TimeRangeSettings)
				{
					speedRangeEntity.RANGOS_TIEMPOS.Add(new ALARMAS_VELOCIDAD_RANG_TIEM
					{
						RANGO_MODO = mode,
						RANGO_ORDEN = speedRangeOrder,
						ORDEN = timeRangeOrder,
						TIEMPO_DESDE = timeRange.From,
						TIEMPO_HASTA = timeRange.To,
						PUNTOS = timeRange.Points,
						GRADO_ID = timeRange.GradeId,
						PARPADEANTE = timeRange.Blinking,
						AUDIBLE = timeRange.Audible,
						RANGO_VELOCIDAD = speedRangeEntity
					});

					timeRangeOrder++;
				}

				speedRangeOrder++;
			}
		}

		private void PrepareEntity(AlarmSpeedLimitSettingInputDto input, ALARMAS_VELOCIDAD entity)
		{
			input.AssignToEntity(entity);
			entity.LIMITE_AUDIBLE = input.SpeedLimitAudible;
			entity.LIMITE_ESTANDAR = input.SpeedLimitStandard;
			entity.LIMITE_TOLERANCIA = input.SpeedLimitTolerance;
			entity.LIMITE_MOP = input.SpeedLimitRoad;
			entity.LIMITE_MOPREST = input.SpeedLimitRoadRestricted;

			_AddSpeedRanges(entity, 1, input.LowSpeed);
			_AddSpeedRanges(entity, 2, input.HiSpeed);

		}

		public async Task<CreateUpdateResult<short>> CreateAsync(AlarmSpeedLimitSettingInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = new ALARMAS_VELOCIDAD();

			

			_dbContext.ALARMAS_VELOCIDAD.Add(entity);

			PrepareEntity(input, entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<short>> UpdateAsync(AlarmSpeedLimitSettingInputDto input)
		{
			var result = new CreateUpdateResult<short>();

			var entity = await _dbContext.ALARMAS_VELOCIDAD.FirstOrDefaultAsync(x => x.ID == input.Id);

			if (entity != null)
			{

				_dbContext.Delete<ALARMAS_VELOCIDAD_RANG_TIEM>(x => x.ALARMA_VELOCIDAD_ID == input.Id);

				_dbContext.Delete<ALARMAS_VELOCIDAD_RANG>(x => x.ALARMA_VELOCIDAD_ID == input.Id);

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
