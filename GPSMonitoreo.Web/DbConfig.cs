using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.Entity;

namespace GPSMonitoreo.Web
{

	[DbConfigurationType(typeof(DbConfig))]
	public class ApplicationDbContext : GPSMonitoreo.Data.Models.EntitiesContext
	{
		public ApplicationDbContext(string connectionString)
			: base(connectionString)
		{
			Console.WriteLine("connecting");
			
		}

	}
	public class DbConfig: System.Data.Entity.DbConfiguration
    {
		public DbConfig()
		{
			Console.WriteLine("db config");
			var xx = Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance;
			SetProviderServices("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance);
			SetProviderFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
		}
    }
}
