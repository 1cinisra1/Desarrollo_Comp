using GPSMonitoreo.Core.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Authorization
{
    public class RoleManager : BaseService
	{

		private Data.Models.EntitiesContext _dbContext;

		public RoleManager(Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public RolePermissions GetRolePermissions(Role role)
		{
			var query = _dbContext.ROLES_PERMISOS.AsNoTracking()
				.Where(x => x.ROL_ID == (Int16)role)
				.GroupBy(x => new { x.PERMISO_TIPO_ID, x.ELEMENTO_ID })
				.GroupBy(x => x.Key.PERMISO_TIPO_ID);


			var rolePermissions = new RolePermissions();

			rolePermissions.Role = role;


			PermissionElements permissionElements;

			foreach(var typeGroup in query)
			{
				//Console.WriteLine("tipo:" + tipo.Key);
				permissionElements = new PermissionElements();

				foreach (var elementGroup in typeGroup)
				{
					permissionElements.Add(elementGroup.Key.ELEMENTO_ID, elementGroup.Select(a => (PermissionAction)a.ACCION_ID).ToList());
					//Console.WriteLine("element key:" + element.Key.ELEMENTO_ID);
				}

				rolePermissions.Add((PermissionElementType)typeGroup.Key, permissionElements);
			}

			//Utils.ObjectJsonDumper.Dump(rolePermissions, 4);

			return rolePermissions;

		}
	}
}
