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
using GPSMonitoreo.Dtos.Base.Equipments;
using GPSMonitoreo.Services.Utils;
using System.Collections.Specialized;

namespace GPSMonitoreo.Services.Base.Equipments
{
    public class EquipmentService : BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public EquipmentService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void _PrepareEntity(EquipmentInputDto input, EQUIPOS entity)
		{
			input.AssignToEntity(entity);
			entity.ALTERNO_ID = input.AlternateId;
			entity.CATEGORIA_ID = input.CategoryId;
			entity.ESTADO_OPERA_ID = input.OperationalStatusId;
			entity.GRUPO_ID = input.GroupId;
			entity.MARCA_ID = input.BrandId;
			entity.MODELO_ID = input.ModelId;
			entity.MODELO_ANO = input.ManufactureYear;
			entity.PLACA = input.Plate;
			entity.SERIAL = input.SerialNumber;

			if(input.Capabilities != null)
			{
				foreach (var capability in input.Capabilities)
				{
					entity.CAPACIDADES.Add(new EQUIPOS_CAPS_REL
					{
						PRODUCTO_CATEGORIA_ID = capability.ProductCategoryId,
						CAPACIDAD_ID = capability.CapabilityId,
						VALOR = capability.Value
					});
				}
			}
		}

		

		public InputValidationResult Validate(EquipmentInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

					
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<int>> CreateAsync(EquipmentInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = new EQUIPOS();

			_PrepareEntity(input, entity);

			_dbContext.EQUIPOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(EquipmentInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.EQUIPOS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<EQUIPOS_CAPS_REL>(x => x.EQUIPO_ID == input.Id);

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

		public async Task<GetResult<EquipmentInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<EquipmentInputDto>();
			var entity = await _dbContext.EQUIPOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new EquipmentInputDto();
				entity.AssignToDto(dto);
				dto.AlternateId = entity.ALTERNO_ID;
				dto.BrandId = entity.MARCA_ID;
				dto.CategoryId = entity.CATEGORIA_ID;
				dto.GroupId = entity.GRUPO_ID;
				dto.ManufactureYear = entity.MODELO_ANO;
				dto.ModelId = entity.MODELO_ID;
				dto.OperationalStatusId = entity.ESTADO_OPERA_ID;
				dto.Plate = entity.PLACA;
				dto.SerialNumber = entity.SERIAL;

				dto.Capabilities = new List<CapabilityListInputDto>();

				foreach (var capability in entity.CAPACIDADES)
				{
					dto.Capabilities.Add(new CapabilityListInputDto
					{
						ProductCategoryId = capability.PRODUCTO_CATEGORIA_ID,
						ProductCategoryDescription = capability.PRODUCTO_CATEGORIA.DESCRIPCION_LARGA,
						CapabilityId = capability.CAPACIDAD_ID,
						CapabilityDescription = capability.CAPACIDAD.DESCRIPCION_LARGA,
						MeasureUnitDescription = capability.CAPACIDAD.UNIDAD.DESCRIPCION_LARGA,
						Value = capability.VALOR
					});
				}


				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}

		public async Task<GetListResult<List<EquipmentListDto>>> GetListAsync(EquipmentFilterInputDto input)
		{
			var result = new GetListResult<List<EquipmentListDto>>();

			var query = _dbContext.EQUIPOS.AsQueryable();

			string orderBy = null;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(x => x.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.CategoryId > 0)
				{
					query = query.Where(x => x.CATEGORIA_ID == input.CategoryId);
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
					.Join(_dbContext.EQUIPOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			result.Items = await query.Select(x => new EquipmentListDto
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				AlternateId = x.ALTERNO_ID,
				BrandDescription = x.MARCA.DESCRIPCION_LARGA,
                CategoryDescription = x.CATEGORIA.DESCRIPCION_LARGA,
                GroupDescription = x.GRUPO.DESCRIPCION_LARGA,
                ModelDescription = x.MODELO.DESCRIPCION_LARGA,
                SerialNumber = x.SERIAL,
                OperationalStatusDescription = x.ESTADO_OPERACION.DESCRIPCION_LARGA,
                ManufactureYear = x.MODELO_ANO,
                Plate = x.PLACA
            }).ToListAsync();

			result.SetSuccess();

			return result;
		}
	}
}
