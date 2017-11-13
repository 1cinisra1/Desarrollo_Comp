using GPSMonitoreo.Core.Authorization;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.RequiredPermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.Entity;

namespace GPSMonitoreo.Services.Base.RequiredPermissions
{
	public class RequiredPermissionService : BaseService
	{
		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;



		public RequiredPermissionService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<OperationResult> UpdateAsync(List<PermissionElementType> types, List<RequiredPermissionListInputDto> requiredPermissions)
		{
			var result = new OperationResult();

			//var castedTypes = types.Select(x => (Int16)x);

			//_dbContext.Delete<PERMISOS_REQUERIDOS_ROLES>(x => castedTypes.Contains(x.ELEMENTO_TIPO_ID));
			//_dbContext.Delete<PERMISOS_REQUERIDOS_ACCS>(x => castedTypes.Contains(x.ELEMENTO_TIPO_ID));
			//_dbContext.Delete<PERMISOS_REQUERIDOS>(x => castedTypes.Contains(x.ELEMENTO_TIPO_ID));

			_dbContext.Delete<PERMISOS_REQUERIDOS_ROLES>(x => types.Contains(x.ELEMENTO_TIPO_ID));
			_dbContext.Delete<PERMISOS_REQUERIDOS_ACCS>(x => types.Contains(x.ELEMENTO_TIPO_ID));
			_dbContext.Delete<PERMISOS_REQUERIDOS>(x => types.Contains(x.ELEMENTO_TIPO_ID));



			var finalRequiredPermissions = requiredPermissions.FindAll(x => types.Contains(x.ElementType));

			//_dbContext.PERMISOS_REQUERIDOS.FirstOrDefault().ROLES

			PERMISOS_REQUERIDOS entity;

			foreach (var requiredPermission in finalRequiredPermissions)
			{

				if ((requiredPermission.Roles != null && requiredPermission.Roles.Count > 0)
					|| (requiredPermission.Actions != null && requiredPermission.Actions.Count > 0))
				{
					entity = new PERMISOS_REQUERIDOS()
					{
						//ELEMENTO_TIPO_ID = (Int16)requiredPermission.ElementType,
						ELEMENTO_TIPO_ID = requiredPermission.ElementType,
						ELEMENTO_ID = requiredPermission.ElementId
					};

					if (requiredPermission.Roles != null)
					{
						foreach (var role in requiredPermission.Roles)
						{

							entity.ROLES.Add(new PERMISOS_REQUERIDOS_ROLES
							{
								//ELEMENTO_TIPO_ID = entity.ELEMENTO_TIPO_ID,
								//ELEMENTO_ID = entity.ELEMENTO_ID,
								ROL_ID = (Int16)role
							});

							//_dbContext.PERMISOS_REQUERIDOS.Add(new PERMISOS_REQUERIDOS
							//{
							//	ELEMENTO_TIPO_ID = (Int16)requiredPermission.ElementType,
							//	ELEMENTO_ID = requiredPermission.ElementId,
							//	APLICACION_TIPO_ID = (byte)PermissionApplication.Role,
							//	APLICACION_ID = (Int16)role
							//});
						}

						if (requiredPermission.Actions != null)
						{
							foreach (var action in requiredPermission.Actions)
							{

								entity.ACCIONES.Add(new PERMISOS_REQUERIDOS_ACCS
								{
									//ELEMENTO_TIPO_ID = entity.ELEMENTO_TIPO_ID,
									//ELEMENTO_ID = entity.ELEMENTO_ID,
									ACCION_ID = action
								});

								//_dbContext.PERMISOS_REQUERIDOS.Add(new PERMISOS_REQUERIDOS
								//{
								//	ELEMENTO_TIPO_ID = (Int16)requiredPermission.ElementType,
								//	ELEMENTO_ID = requiredPermission.ElementId,
								//	APLICACION_TIPO_ID = (byte)PermissionApplication.Action,
								//	APLICACION_ID = (Int16)role
								//});
							}
						}
					}

					_dbContext.PERMISOS_REQUERIDOS.Add(entity);
				}
			}


			await _dbContext.SaveChangesAsync();

			result.SetSuccess();

			return result;
		}

		public async Task<GetListResult<List<RequiredPermissionListInputDto>>> GetListAsync(List<PermissionElementType> types)
		{

			var result = new GetListResult<List<RequiredPermissionListInputDto>>();

			//var castedTypes = types.Select(t => (Int16)t);


			//var query = _dbContext.PERMISOS_REQUERIDOS.AsNoTracking()
			//	.Where(x => castedTypes.Contains(x.ELEMENTO_TIPO_ID));

			var query = _dbContext.PERMISOS_REQUERIDOS.AsNoTracking()
				.Where(x => types.Contains(x.ELEMENTO_TIPO_ID));


			var select = query.Select(x => new RequiredPermissionListInputDto
			{
				//ElementType = (PermissionElementType)x.ELEMENTO_TIPO_ID,
				ElementType = x.ELEMENTO_TIPO_ID,
				ElementId = x.ELEMENTO_ID,
				//ElementDescription = "",
				Roles = x.ROLES.Select(r => r.ROL_ID).Cast<Role>().ToList(),
				Actions = x.ACCIONES.Select(a => (PermissionAction)a.ACCION_ID).ToList()
			});


			Console.WriteLine("before query");
			
			result.Items = await select.ToListAsync();

			Console.WriteLine("after query");

			result.SetSuccess();

			return result;
		}
	}
}
