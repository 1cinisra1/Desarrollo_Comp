using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//using GPSMonitoreo.Dtos;

using GPSMonitoreo.Services.Validation;
using GPSMonitoreo.Data.Models;

using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;

using System.Data.Entity;
using GPSMonitoreo.Services.Resources;
using System.Data.Entity;
using GPSMonitoreo.Core.Authorization;

using GPSMonitoreo.Libraries.Extensions.EnumExtensions;
using GPSMonitoreo.Dtos.Base.EntityAlarmNotifications;
using GPSMonitoreo.Services.Base.EntityAddresses;

using GPSMonitoreo.Libraries.Extensions.TaskExtensions;
using GPSMonitoreo.Dtos.Base.EntityAddress;
using System.Linq.Expressions;

namespace GPSMonitoreo.Services.Base.EntityAlarmNotifications
{


	public class EntityAlarmNotificationService : BaseService
	{
		private GPSMonitoreo.Data.Models.EntitiesContext _dbContext;

		private EntityAddressService _entityAddressService;


		public EntityAlarmNotificationService(
			GPSMonitoreo.Data.Models.EntitiesContext dbContext,
			EntityAddressService entityAddressService
		)
		{
			_dbContext = dbContext;
			_entityAddressService = entityAddressService;
		}

		


		public InputValidationResult Validate(EntityAlarmNotificationInputDto input)
		{
			Console.WriteLine("*******************");
			Console.WriteLine(nameof(input.PlaceIds));
			//Console.WriteLine(GetName(x => input.PlaceIds));

			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if (_dbContext.ENTIDADES_NOTIF_ALARMAS.Any(x => x.ENTIDAD_ID == input.EntityId && x.PERSONA_ID == input.RecipientId && x.ID != input.Id))
					{
						errors.AddError("RecipientId", "Destinatario ya existe dentro de la misma empresa (entidad)");
					}

					if (!errors.ContainsKey(nameof(input.PlaceIds)))
					{
						var collectionErrors = new InputCollectionValidationError();

						var places = _entityAddressService.GetListAsync(new EntityAddressFilterInputDto { EntityId = input.EntityId, TypeIds = new List<byte> { 3, 4, 6 }, Paging = false }).RunSync().Items;

						//var places = _dbContext.ENTIDADES_DIRS.Where(x => input.PlaceIds.Contains(x.ID)).Select(x => new
						//{
						//	x.ID,
						//	x.DESCRIPCION_LARGA,
						//	x.CERCA_ID
						//}).ToList();

						int rowIndex = 0;

						foreach (var place in places)
						{
							if (input.PlaceIds.Contains(place.Id))
							{
								if (place.GeofenceId == 0)
								{
									collectionErrors.AddItemError(rowIndex, "Lugar: " + place.Id + " - " + place.Description + " no tiene una cerca asignada");
								}
							}

							rowIndex ++;
						}

						//foreach(var placeId in input.PlaceIds)
						//{
						//	var place = places.FirstOrDefault(x => x.ID == placeId);

						//	if (place.CERCA_ID == null)
						//	{
						//		collectionErrors.AddItemError(rowIndex, "Lugar: " + place.ID + " - " + place.DESCRIPCION_LARGA + " no tiene una cerca asignada");
						//	}
						//	rowIndex++;
						//}

						if (collectionErrors.ItemsErrors.Count > 0)
						{
							collectionErrors.Error = Dtos.Resources.ValidationErrors.ValidateCollection;
							errors.AddCollectionError(nameof(input.PlaceIds), collectionErrors);
						}
					}
				}
			});

			return result;
			//GPSMonitoreo.Dtos.Validation.InputValidator
		}

		private void PrepareEntity(EntityAlarmNotificationInputDto input, ENTIDADES_NOTIF_ALARMAS entity)
		{
			entity.ALARMAS_ROL_ID = input.AlarmsRoleId > 0 ? input.AlarmsRoleId : (Int16?)null;
			entity.PERSONA_ID = input.RecipientId;
			entity.ENTIDAD_ID = input.EntityId;


			foreach (var placeId in input.PlaceIds)
			{
				entity.LUGARES.Add(new ENTIDADES_NOTIF_ALARMAS_DIRS { DIRECCION_ID = placeId });
			}

			foreach (var emailId in input.EmailIds)
			{
				entity.EMAILS.Add(new ENTIDADES_NOTIF_ALARMAS_EMS { EMAIL_ID = emailId });
			}
		}


		public async Task<CreateUpdateResult<int>> CreateAsync(EntityAlarmNotificationInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = new ENTIDADES_NOTIF_ALARMAS();

			PrepareEntity(input, entity);

			_dbContext.ENTIDADES_NOTIF_ALARMAS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(EntityAlarmNotificationInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = await _dbContext.ENTIDADES_NOTIF_ALARMAS.FirstOrDefaultAsync(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<ENTIDADES_NOTIF_ALARMAS_DIRS>(x => x.ALARMAS_NOTIF_ID == input.Id);
				_dbContext.Delete<ENTIDADES_NOTIF_ALARMAS_EMS>(x => x.ALARMAS_NOTIF_ID == input.Id);

				PrepareEntity(input, entity);

				await _dbContext.SaveChangesAsync();

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

				result.Id = input.Id;

			}
			else
			{
				result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			}



			//if (_dbContext.ROLES.Any(x => x.ID == input.Id))
			//{
			//	_dbContext.Delete<ROLES_PERMISOS>(x => x.ROL_ID == input.Id);

			//	var entity = PrepareEntity(input);

			//	_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			//	_dbContext.ROLES_PERMISOS.AddRange(entity.PERMISOS);

			//	await _dbContext.SaveChangesAsync();

			//	result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

			//	result.Id = input.Id;
			//}
			//else
			//{
			//	result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			//}



			return result;
		}

		public async Task<GetResult<EntityAlarmNotificationInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<EntityAlarmNotificationInputDto>();
			var entity = await _dbContext.ENTIDADES_NOTIF_ALARMAS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new EntityAlarmNotificationInputDto
				{
					Id = id,
					EntityId = entity.ENTIDAD_ID,
					RecipientId = entity.PERSONA_ID,
					RecipientDescription = entity.PERSONA.DESCRIPCION_LARGA,
					AlarmsRoleId = entity.ALARMAS_ROL_ID == null ? (Int16)0 : entity.ALARMAS_ROL_ID.Value,
					EmailIds = entity.EMAILS.Select(x => x.EMAIL_ID).ToList(),
					PlaceIds = entity.LUGARES.Select(x => x.DIRECCION_ID).ToList(),
					
				};

				result.Item = dto;
				result.SetSuccess();
			}
			return result;
		}





		//public async Task<GetResult<RoleInputDto>> GetByIdAsync(int id)
		//{
		//	var result = new GetResult<RoleInputDto>();
		//	var entity = await _dbContext.ROLES.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

		//	if (entity != null)
		//	{
		//		var dto = new RoleInputDto();
		//		entity.AssignToDto(dto);

		//		//var entitiesEnumList = EnumHelper.GetLocalizedPairs(Core.Resources.Entities.ResourceManager, typeof(Core.Enums.Entity));

		//		//var entitiesPermissions = entitiesEnumList.Select(x => new Dtos.Base.Roles.PermissionListInputDto
		//		//{
		//		//	ElementId = x.Key,
		//		//	ElementDescription = x.Value
		//		//})
		//		//.OrderBy(x => x.ElementDescription)
		//		//.ToList();
		//		//;

		//		var entitiesPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)PermissionElementType.Entity)
		//			.GroupBy(x => x.ELEMENTO_ID)
		//			.Select(x => new PermissionListInputDto<int>
		//			{
		//				ElementId = x.Key,
		//				ElementDescription = ((Core.Enums.Entity)x.Key).ToLocalizedString(),
		//				Actions = x.Select(p => (PermissionAction)p.ACCION_ID).ToList()
		//			}).ToList();

		//		dto.EntitiesPermissions = entitiesPermissions;


		//		var alarmFiltersPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersCategories || x.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersAlarms)
		//			.GroupBy(x => new { x.PERMISO_TIPO_ID, x.ELEMENTO_ID })
		//			.Select(x => new PermissionListInputDto<string>
		//			{
		//				ElementId = x.Key.PERMISO_TIPO_ID == (Int16)PermissionElementType.AlarmFiltersCategories ? "A" + x.Key.ELEMENTO_ID.ToString() : "B" + x.Key.ELEMENTO_ID.ToString(),
		//				ElementDescription = "", /*no description is needed, for now*/
		//				Actions = x.Select(p => (PermissionAction)p.ACCION_ID).ToList()
		//			}).ToList();

		//		dto.AlarmsFiltersPermissions = alarmFiltersPermissions;

		//		//Utils.ObjectJsonDumper.Dump(permissions, 3);


		//		//dto.EntitiesPermissions = entity.PERMISOS.Where(x => x.PERMISO_TIPO_ID == (Int16)Core.Authorization.PermissionType.Entity)

		//		//dto.Routes = entity.TRAYECTO_RUTAS.OrderBy(x => x.ORDEN).Select(x => new RouteListItemInputDto
		//		//{
		//		//	Id = x.RUTA_ID,
		//		//	Description = x.RUTA.DESCRIPCION_LARGA
		//		//}).ToList();

		//		result.Item = dto;
		//		result.SetSuccess();
		//	}




		//	return result;
		//}

		public async Task<GetListResult<List<EntityAlarmNotificationListDto>>> GetListAsync(EntityAlarmNotificationFilterInputDto input = null)
		{
			var result = new GetListResult<List<EntityAlarmNotificationListDto>>();

			var query = _dbContext.ENTIDADES_NOTIF_ALARMAS.AsQueryable().AsNoTracking();

			List<EntityAlarmNotificationListDto> items;


			if (input != null)
			{

				query = query.Where(x => x.ENTIDAD_ID == input.EntityId);

				if (!string.IsNullOrWhiteSpace(input.LastNames))
				{
					query = query.Where(item => item.PERSONA.APELLIDOS.ToLower().Contains(input.LastNames));
				}

				if (!string.IsNullOrWhiteSpace(input.Names))
				{
					query = query.Where(item => item.PERSONA.NOMBRES.ToLower().Contains(input.Names));
				}

				result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();


				if (input.Paging)
				{
					query = query.OrderByDescending(x => x.ID)
						.Select(x => new { x.ID })
						.Skip(input.Page * input.PageSize).Take(input.PageSize)
						.Join(_dbContext.ENTIDADES_NOTIF_ALARMAS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
					;
				}
			}



			query = query.OrderByDescending(x => x.ID);

			items = await query.Select(x => new EntityAlarmNotificationListDto
			{
				Id = x.ID,
				RecipientLastNames = x.PERSONA.APELLIDOS,
				RecipientNames = x.PERSONA.NOMBRES
			}).ToListAsync();


			result.Items = items;

			result.SetSuccess();

			return result;
		}

	}
}
