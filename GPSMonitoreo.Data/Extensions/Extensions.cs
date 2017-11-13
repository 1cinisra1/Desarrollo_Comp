using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPSMonitoreo.Data.QueryModels;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using GPSMonitoreo.Data.Models;
using System.Data;

namespace GPSMonitoreo.Data.Extensions
{


	public static class Extensions
	{

		public static System.Data.Entity.DbSet<T> FilterByFooBar<T>(this System.Data.Entity.DbSet<T> entity) where T : class
		{
			Console.WriteLine(typeof(T).ToString());
			return null;
			//return items.Where(x => x.IsFooBar);
		}


		public static IQueryable<SimpleByte> ToSimple(this IQueryable<Models.ICommonEntityByte> entity)
		{
			var ret = from item in entity
					  select new SimpleByte {  codigo = item.ID, descripcion = item.DESCRIPCION_LARGA };

			return ret;
		}

		public static IQueryable<SimpleInt16> ToSimple(this IQueryable<Models.ICommonEntityInt16> entity)
		{

			var ret = from item in entity
					  select new SimpleInt16 { codigo = item.ID, descripcion = item.DESCRIPCION_LARGA };

			return ret;
		}

		public static IQueryable<SimpleInt32> ToSimple(this IQueryable<Models.ICommonEntityInt32> entity)
		{

			var ret = from item in entity
					  select new SimpleInt32 { codigo = item.ID, descripcion = item.DESCRIPCION_LARGA };

			return ret;
		}


		public static System.Data.DataTable SqlQuery(this System.Data.Entity.Database database, string sql, System.Data.CommandType commandType, params object[] parameters)
		{
			var conn = database.Connection;
			var connState = conn.State;

			if (connState != System.Data.ConnectionState.Open)
				conn.Open();

			var cmd = conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.CommandType = commandType;
			cmd.Parameters.AddRange(parameters);
			var reader = cmd.ExecuteReader();
			var dt = new System.Data.DataTable();
			dt.Load(reader);
			if (connState != System.Data.ConnectionState.Open)
				conn.Close();

			return dt;
		}

		public static DbDataReader ProcedureDataReader(this EntitiesContext dbContext, string name, List<KeyValuePair<string, object>> parameters = null)
		{
			var pars = new List<OracleParameter>();

			if(parameters != null)
			{
				foreach (var p in parameters)
				{
					var par = new Oracle.ManagedDataAccess.Client.OracleParameter(p.Key, p.Value);
					//switch (Type.GetTypeCode(p.Value.GetType()))
					//{
					//	case TypeCode.Int32:


					//}

					//Console.WriteLine(par.Direction.ToString() + par.GetType);

					pars.Add(par);
				}
			}


			pars.Add(new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) { Direction = System.Data.ParameterDirection.Output });



			

			var conn = dbContext.Database.Connection;
			var connState = conn.State;

			
			
			//dbContext.Database.SqlQuery()

			

			if (connState != System.Data.ConnectionState.Open)
				conn.Open();

			var cmd = conn.CreateCommand();

			(cmd as OracleCommand).UseEdmMapping = true;

			cmd.CommandText = name;
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddRange(pars.ToArray());
			
			var reader = cmd.ExecuteReader();
			cmd.Dispose();
			return reader;
		}




		public static DataTable ProcedureDataTable(this EntitiesContext dbContext, string name, List<KeyValuePair<string, object>> parameters = null)
		{
			var reader = dbContext.ProcedureDataReader(name, parameters);

			var dt = new DataTable();
			dt.Load(reader);
			return dt;
		}

		public static DbDataReader DataReader(this EntitiesContext dbContext, string stmt)
		{
			Console.WriteLine(stmt);
			var conn = dbContext.Database.Connection;
			var connState = conn.State;


			if (connState != System.Data.ConnectionState.Open)
				conn.Open();

			var cmd = conn.CreateCommand();

			(cmd as OracleCommand).UseEdmMapping = true;

			cmd.CommandText = stmt;
			cmd.CommandType = CommandType.Text;

			var reader = cmd.ExecuteReader();
			cmd.Dispose();


			if (connState != System.Data.ConnectionState.Open)
				conn.Close();

			return reader;
		}

		public static T FirstValue<T>(this EntitiesContext dbContext, string stmt)
		{
			T ret = default(T);

			var conn = dbContext.Database.Connection;
			var connState = conn.State;

			if (connState != System.Data.ConnectionState.Open)
			{
				conn.Open();
			}

			var cmd = conn.CreateCommand();

			(cmd as OracleCommand).UseEdmMapping = true;

			cmd.CommandText = stmt;

			var obj = cmd.ExecuteScalar();

			if (obj != DBNull.Value)
			{
				ret = (T)obj;
			}

			cmd.Dispose();

			if (connState != System.Data.ConnectionState.Open)
			{
				conn.Close();
			}

			return ret;
		}

		public static Dictionary<string, object> FirstRow(this EntitiesContext dbContext, string stmt)
		{
			Dictionary<string, object> ret = null;
			var reader = dbContext.DataReader(stmt);
			if (reader.Read())
			{
				ret = new Dictionary<string, object>(reader.FieldCount);

				for (var x = 0; x < reader.FieldCount; x++)
				{
					ret.Add(reader.GetName(x), reader.GetValue(x));
				}
			}
			reader.Dispose();
			return ret;
		}

	}
}
