using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using System.Reflection;

namespace GPSMonitoreo.Libraries.Extensions.LinqExtensions
{
	public static class LinqExtensions
	{




		//public static void Delete<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		//{
		//	var set = context.Set<TEntity>();
		//	//if (predicate != null)
		//	//	dbSet.RemoveRange(dbSet.Where(predicate));
		//	//else
		//	//	dbSet.RemoveRange(dbSet);

		//	//context.SaveChanges();

		//	var query = set.Where(predicate).Select(item => new { dummy = 1 });
		//	var selectSql = query.ToString();
		//	//string deleteSql = "DELETE [Extent1] " + selectSql.Substring(selectSql.IndexOf("FROM"));
		//	string deleteSql = "DELETE " + selectSql.Substring(selectSql.IndexOf("FROM"));

		//	var internalQuery = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_internalQuery").Select(field => field.GetValue(query)).First();
		//	var objectQuery = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_objectQuery").Select(field => field.GetValue(internalQuery)).First() as ObjectQuery;

		//	var cmd = context.Database.Connection.CreateCommand();

		//	//var parameters = objectQuery.Parameters.Select(p => new SqlParameter(p.Name, p.Value)).ToArray();

		//	var pars = new List<object>();
		//	System.Data.Common.DbParameter parameter;

		//	foreach (var p in objectQuery.Parameters)
		//	{
		//		Console.WriteLine(p.Name);
		//		parameter = cmd.CreateParameter();
		//		parameter.ParameterName = p.Name;
		//		parameter.Value = p.Value;
		//		pars.Add(parameter);
		//	}
		//	//objectQuery.Parameters.ea



		//	context.Database.ExecuteSqlCommand(deleteSql, pars.ToArray());

		//	//Console.WriteLine(deleteSql);
		//	//Console.WriteLine(internalQuery.ToString());
		//	//Console.WriteLine(objectQuery.ToString());
		//	//Utils.ObjectJsonDumper.Dump(objectQuery.Parameters, 1);




		//}

		//public static void Delete<TEntity>(this DbSet<TEntity> set, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		//{
		//	//var dbSet = context.Set<TEntity>();


		//	//var query = db.Set<T>().Where(filter);



		//	//string selectSql = query.ToString();
		//	//string deleteSql = "DELETE [Extent1] " + selectSql.Substring(selectSql.IndexOf("FROM"));

		//	//var internalQuery = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_internalQuery").Select(field => field.GetValue(query)).First();
		//	//var objectQuery = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_objectQuery").Select(field => field.GetValue(internalQuery)).First() as ObjectQuery;
		//	//var parameters = objectQuery.Parameters.Select(p => new SqlParameter(p.Name, p.Value)).ToArray();



		//	//if (predicate != null)
		//	//	set.RemoveRange(set.Where(predicate));
		//	//else
		//	//	set.RemoveRange(set);
		//}

		public static bool Exists<TEntity>(this DbSet<TEntity> entity, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			var query = from item in entity.Where(predicate)
						select new { dummy = 1 };

			if (query.FirstOrDefault() == null)
				return false;
			else
				return true;


		}
	}
}
