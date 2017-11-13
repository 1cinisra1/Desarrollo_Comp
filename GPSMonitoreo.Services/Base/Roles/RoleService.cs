using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Roles;
using GPSMonitoreo.Services.Validation;
using GPSMonitoreo.Data.Models;

using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;

using System.Data.Entity;
using GPSMonitoreo.Services.Resources;
using System.Data.Entity;
using GPSMonitoreo.Core.Authorization;

using GPSMonitoreo.Libraries.Extensions.EnumExtensions;

namespace GPSMonitoreo.Services.Base.Roles
{
    public class RoleService : BaseService
	{
		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public RoleService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private ROLES PrepareEntity(RoleInputDto input)
		{
			var entity = new ROLES();
			input.AssignToEntity(entity);

			foreach (var permission in input.EntitiesPermissions)
			{
				if(permission.Actions?.Count > 0)
				{
					foreach(var action in permission.Actions)
					{
						entity.PERMISOS.Add(new ROLES_PERMISOS()
						{
							ROL_ID = entity.ID,
							PERMISO_TIPO_ID = (Int16)Core.Authorization.PermissionElementType.Entity,
							ELEMENTO_ID = permission.ElementId,
							ACCION_ID = (byte)action
						});
					}
				}
			}

			int elementId;

			PermissionElementType permissionType;

			foreach (var permission in input.AlarmsFiltersPermissions)
			{
				if (permission.Actions?.Count > 0)
				{
					if(permission.ElementId.StartsWith("A")) //is category
					{
						permissionType = PermissionElementType.AlarmFiltersCategories;
					}
					else // "B"
					{
						permissionType = PermissionElementType.AlarmFiltersAlarms;
					}

					elementId = Convert.ToInt32(permission.ElementId.Substring(1));

					foreach (var action in permission.Actions)
					{
						entity.PERMISOS.Add(new ROLES_PERMISOS()
						{
							ROL_ID = entity.ID,
							PERMISO_TIPO_ID = (Int16)permissionType,
							ELEMENTO_ID = elementId,
							ACCION_ID = (byte)action
						});
					}
				}
			}




			return entity;
		}


		public async Task<CreateUpdateResult<int>> CreateAsync(RoleInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = PrepareEntity(input);

			_dbContext.ROLES.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(RoleInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			if (_dbContext.ROLES.Any(x => x.ID == input.Id))
			{
				_dbContext.Delete<ROLES_PERMISOS>(x => x.ROL_ID == input.Id);

				var entity = PrepareEntity(input);

				_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				_dbContext.ROLES_PERMISOS.AddRange(entity.PERMISOS);

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


		public async Task<InputValidationResult> ValidateAsync(RoleInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

				}
			});

			return result;
			//GPSMonitoreo.Dtos.Validation.InputValidator
		}


		public async Task<GetResult<RoleInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<RoleInputDto>();
			var entity = await _dbContext.ROLES.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new RoleInputDto();
				entity.AssignToDto(dto);

				//var entitiesEnumList = EnumHelper.GetLocalizedPairs(Core.Resources.Entities.ResourceManager, typeof(Core.Enums.Entity));

				//var entitiesPermissions = entitiesEnumList.Select(x => new Dtos.Base.Roles.PermissionListInputDto
				//{
				//	ElementId = x.Key,
				//	ElementDescription = x.Value
				//})
				//.OrderBy(x => x.ElementDescription)
				//.ToList();
				//;

				var entitiesPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)PermissionElementType.Entity)
					.GroupBy(x => x.ELEMENTO_ID)
					.Select(x => new PermissionListInputDto<int>
					{
						ElementId = x.Key,
						ElementDescription = ((Core.Enums.Entity)x.Key).ToLocalizedString(),
						Actions = x.Select(p => (PermissionAction)p.ACCION_ID).ToList()
					}).ToList();

				dto.EntitiesPermissions = entitiesPermissions;


				var alarmFiltersPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersCategories || x.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersAlarms)
					.GroupBy(x => new { x.PERMISO_TIPO_ID, x.ELEMENTO_ID })
					.Select(x => new PermissionListInputDto<string>
					{
						ElementId = x.Key.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersCategories ? "A" + x.Key.ELEMENTO_ID.ToString() : "B" + x.Key.ELEMENTO_ID.ToString(),
						ElementDescription = "", /*no description is needed, for now*/
						Actions = x.Select(p => (PermissionAction)p.ACCION_ID).ToList()
					}).ToList();

				dto.AlarmsFiltersPermissions = alarmFiltersPermissions;

				//Utils.ObjectJsonDumper.Dump(permissions, 3);


				//dto.EntitiesPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)Core.Authorization.PermissionType.Entity)

				//dto.Routes = entity.TRAYECTO_RUTAS.OrderBy(x => x.ORDEN).Select(x => new RouteListItemInputDto
				//{
				//	Id = x.RUTA_ID,
				//	Description = x.RUTA.DESCRIPCION_LARGA
				//}).ToList();

				result.Item = dto;
				result.SetSuccess();
			}




			return result;
		}

		public async Task<GetListResult<List<RoleListDto>>> GetListAsync(RoleFilterInputDto input = null)
		{
			var result = new GetListResult<List<RoleListDto>>();

			var query = _dbContext.ROLES.AsQueryable().AsNoTracking();

			List<RoleListDto> items;


			if(input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
			}



			if(input != null && input.Paging)
			{
				var pagedSelect = query.OrderBy(x => x.ID)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.ROLES.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => new RoleListDto
					{
						Id = b.ID,
						Description = b.DESCRIPCION_LARGA
					})
					.OrderBy(x => x.Id)
					;

				items = await pagedSelect.ToListAsync();
			}
			else
			{
				items = await query.Select(x => new RoleListDto
				{
					Id = x.ID,
					Description = x.DESCRIPCION_LARGA
				}).ToListAsync();
			}



			result.Items = items;

			result.SetSuccess();

			return result;
		}

	}
}
