using GPSMonitoreo.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Libraries.Trees;
using GPSMonitoreo.Core.Authorization;

namespace GPSMonitoreo.Web.Helpers
{
    public static class Alarms
    {

		public static RoledTree<string, Role, int> GetTreeAlarms(EntitiesContext dbContext)
		{
			var alarmsTree = new RoledTree<string, Role, int>(true);
			var alarmsTreeDataTable = dbContext.CategoriesTwoTablesTree<ALARMAS_CATS, ALARMAS>();

			alarmsTree.Populate(alarmsTreeDataTable, "ID", "DESCRIPCION_LARGA", "LEV", (node, row) =>
			{
				//Console.WriteLine("Adding node: " + node.Description);
			});

			alarmsTree.AllNodes["B11"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["B12"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["B14"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["B15"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["B5"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["B3"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["A6"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };
			alarmsTree.AllNodes["A3"].Roles = new List<Role> { Role.SuperAdmin, Role.InternalMonitorist };

			//velocidad
			//alarmsTree.AllNodes["B6"].Roles = new List<Authorization.Enums.Role> { Authorization.Enums.Role.SuperAdmin, Authorization.Enums.Role.InternalMonitorist };

			return alarmsTree;
		}

		public static List<Int16> GetAccessibleAlarmsIds(EntitiesContext dbContext, Role role )
		{
			var ids = new List<Int16>();

			var alarmsTree = GetTreeAlarms(dbContext);

			alarmsTree.VisitNodesWithRoleAccess(role, (node) =>
			{
				if(node.Id.StartsWith("B"))
				{
					ids.Add(Convert.ToInt16(node.Id.Substring(1)));
				}
			});
			return ids;
		}
	}
}
