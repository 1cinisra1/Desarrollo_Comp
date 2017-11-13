using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Objects.DataClasses;
using GPSMonitoreo.Data.Extensions;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;

namespace GPSMonitoreo.Data.Models
{

	//public class DbConfig : System.Data.Entity.DbConfiguration
	//{
	//	public DbConfig()
	//	{
	//		Console.WriteLine("db config");
	//		var xx = Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance;
	//		SetProviderServices("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance);
	//		SetProviderFactory("Oracle.ManagedDataAccess.Client", Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance);
	//	}
	//}
	public partial class EntitiesContext : DbContext
	{

		public static System.Data.Entity.Core.EntityClient.EntityConnection GetEntityConnection(string connectionString)
		{
			//Console.WriteLine("connection is: " + System.Reflection.Assembly.GetExecutingAssembly());
			//var xx = new DbConfig();

			var instance = Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance;
			


			//Console.WriteLine(instance.ToString());
			var workspace = new System.Data.Entity.Core.Metadata.Edm.MetadataWorkspace(new List<String>(){
																@"res://*/Models.EntitiesModel.csdl",
																@"res://*/Models.EntitiesModel.ssdl",
																@"res://*/Models.EntitiesModel.msl"
														}
													   , new List<System.Reflection.Assembly>() { System.Reflection.Assembly.GetExecutingAssembly() });

			Console.WriteLine("creating connection");

			//var xxx = new Oracle.ManagedDataAccess.Client.OracleDataReader();

			//Oracle.ManagedDataAccess.Client.OracleDataReader reader;
			//reader.UseEdmMapping=

			var oracleConnection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
			
			var connection = new System.Data.Entity.Core.EntityClient.EntityConnection(workspace, oracleConnection);

			connection.Open();

			return connection;
		}

		//public static System.Data.Entity.Core.EntityClient.EntityConnection GetEntityConnection(string connectionString, string modelName, System.Collections.Specialized.StringDictionary schemaMapping, List<string> skipNodeNames, List<string> skipEntityNames)
		public static System.Data.Entity.Core.EntityClient.EntityConnection GetEntityConnection(string connectionString, string modelName, Dictionary<string, string> entitiesSchemaMapping, Dictionary<string, string> methodsSchemaMapping)
		{
			Console.WriteLine("connection is2: " + System.Reflection.Assembly.GetExecutingAssembly());
			//var xx = new DbConfig();

			var instance = Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance;


			var csdl = Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".csdl");
			var msl = Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".msl");
			var ssdl = Assembly.GetExecutingAssembly().GetManifestResourceStream(modelName + ".ssdl");


			//Console.WriteLine("ssdl:" + (new System.IO.StreamReader(ssdl)).ReadToEnd());
			//Console.WriteLine("-----------------");


			var storageXml = System.Xml.Linq.XElement.Load(ssdl);


			//Console.WriteLine(storageXml.Name.Namespace + "EntityType");

			var ns = storageXml.Name.Namespace;
			//XNamespace ab = "http://whatever-the-url-is"
			XNamespace ab = "http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator";


			var entities = storageXml.Element(ns + "EntityContainer").Elements(ns + "EntitySet")
				.Where(item =>  item.Attribute("Schema") != null || item.Attribute(ab + "Schema") != null);


			//Console.WriteLine(qry.Count());
			//Console.WriteLine("-----------------");

			XAttribute schemaAttribute;
			XAttribute storeTypeAttribute;

			string newSchemaName;

			foreach (var entitySet in entities)
			{
				//Console.WriteLine("Name: " + entitySet.Name);

				//Console.WriteLine(entitySet.Name.LocalName);



				storeTypeAttribute = entitySet.Attribute(ab + "Type");

				schemaAttribute = null;

				//Console.WriteLine("Type: " + storeTypeAttribute.Value);

				switch(storeTypeAttribute.Value)
				{
					case "Tables":
						schemaAttribute = entitySet.Attribute("Schema");

						break;

					case "Views":
						schemaAttribute = entitySet.Attribute(ab + "Schema");

						break;

				}

				if(schemaAttribute != null)
				{
					//Console.WriteLine(entitySet.Name);
					//Console.WriteLine(entitySet.Value);

					var nameAttribute = entitySet.Attribute("Name");
					//Console.WriteLine(nameAttribute.Value);


					if (entitiesSchemaMapping.ContainsKey(schemaAttribute.Value))
					{
						//Console.WriteLine(entitySet.Name);
						newSchemaName = entitiesSchemaMapping[schemaAttribute.Value];

						//Console.WriteLine("new schema name " + newSchemaName + " for: " + nameAttribute.Value);

						//Console.WriteLine(entitySet.Name);
						//Console.WriteLine("entitySet.Value:");
						//Console.WriteLine(entitySet.Value);

						if (string.IsNullOrWhiteSpace(newSchemaName))
							schemaAttribute.Remove();
						else
							schemaAttribute.SetValue(newSchemaName);
					}
				}

				switch (storeTypeAttribute.Value)
				{
					case "Tables":
						

						break;

					case "Views":

						var queryElement = entitySet.Element(ns + "DefiningQuery");
							
						//(XElement)entitySet.FirstNode;
						if (queryElement != null)
						{
							//Console.WriteLine("the query is: " + queryElement.Value);

							queryElement.SetValue(queryElement.Value.Replace("\"" + schemaAttribute.Value + "\".", ""));
							
							//var queryElement = (XElement)firstElement.FirstNode
						}
						//Console.WriteLine("view firstnode: " + firstElement.Name.LocalName);
						//schemaAttribute = entitySet.FirstNode.nam

						break;

				}
			}


			var functions = storageXml.Elements(ns + "Function").Where(item => item.Attribute("Schema") != null);

			foreach (var func in functions)
			{

				schemaAttribute = func.Attribute("Schema");

				if (methodsSchemaMapping.ContainsKey(schemaAttribute.Value))
				{
					newSchemaName = methodsSchemaMapping[schemaAttribute.Value];

					if (string.IsNullOrWhiteSpace(newSchemaName))
						schemaAttribute.Remove();
					else
						schemaAttribute.SetValue(newSchemaName);
				}
			}




			var storageCollection = new StoreItemCollection(new[] { storageXml.CreateReader() });
			var conceptualCollection = new EdmItemCollection(new[] { System.Xml.XmlReader.Create(csdl) });
			var mappingCollection = new StorageMappingItemCollection(conceptualCollection, storageCollection, new[] { System.Xml.XmlReader.Create(msl) });


			var workspace = new MetadataWorkspace(() => conceptualCollection, () => storageCollection, () => mappingCollection);


			
			

			//Console.WriteLine(instance.ToString());


			Console.WriteLine("creating connection");

			//var xxx = new Oracle.ManagedDataAccess.Client.OracleDataReader();

			//Oracle.ManagedDataAccess.Client.OracleDataReader reader;
			//reader.UseEdmMapping=

			var oracleConnection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);

			var connection = new System.Data.Entity.Core.EntityClient.EntityConnection(workspace, oracleConnection);

			

			connection.Open();

			return connection;
		}

		public EntitiesContext(bool debug) : base()
		{
			
			Console.WriteLine("EntitiesContext initialized");
			if (debug)
			{
				this.Database.Log = Console.Write;
			}

			
		}

		





		public EntitiesContext(string connectionString,  bool debug = false): base(GetEntityConnection(connectionString), true)
		{
			Console.WriteLine("EntitiesContext2 initialized");
			if (debug)
			{
				this.Database.Log = Console.Write;
			}
		}

		public EntitiesContext(string connectionString, string modelName, Dictionary<string, string> entitiesSchemaMapping, Dictionary<string, string> methodsSchemaMapping, bool debug = false) : base(GetEntityConnection(connectionString, modelName, entitiesSchemaMapping, methodsSchemaMapping), true)
		{
			Console.WriteLine("EntitiesContext2 initialized");
			if (debug)
			{
				this.Database.Log = Console.Write;
			}
		}

		protected override void Dispose(bool disposing)
		{
			Console.WriteLine("disposing DbContext");
			Console.WriteLine("connection state: " + this.Database.Connection.State.ToString());
			Console.WriteLine("clossing connection ");
			this.Database.Connection.Close();
			Console.WriteLine("connection state: " + this.Database.Connection.State.ToString());
			base.Dispose(disposing);
		}


		//[DbFunction("EntitiesModel.Store", "FN_PARENTS_PATH")]
		//public static string FN_PARENTS_PATH(string tableName, short ID)
		//{
		//	throw new NotSupportedException("Direct calls are not supported.");
		//}


		[DbFunction("EntitiesModel.Store", "FN_HIERARCHY_PATH")]
		public string FN_HIERARCHY_PATH(string tableName, short? ID)
		{
			var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

			var result = Database.SqlQuery<string>($"SELECT FN_HIERARCHY_PATH('{tableName}', {ID}) FROM DUAL", new object[] { });
			return result.FirstOrDefault();
		}

		[DbFunction("EntitiesModel.Store", "FN_HIERARCHY_PATH")]
		public string FN_HIERARCHY_PATH<TId>(string tableName, TId ID)
		{
			var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

			var result = Database.SqlQuery<string>($"SELECT FN_HIERARCHY_PATH('{tableName}', {ID}) FROM DUAL", new object[] { });
			return result.FirstOrDefault();
		}

		[DbFunction("EntitiesModel.Store", "FN_EVENTO_REL_DESCRIPCION")]
		public string FN_EVENTO_REL_DESCRIPCION(Int16 eventId, int? relId)
		{
			return "";
		}

		[DbFunction("EntitiesModel.Store", "FN_TOP_PATH")]
		public string FN_TOP_PATH(string tableName, short? ID)
		{
			return "";
		}

		[DbFunction("EntitiesModel.Store", "FN_CERCA_COORDS")]
		public string FN_CERCA_COORDS(Int32 ID)
		{

			var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;



			//var pars = new System.Data.Entity.Core.Objects.ObjectParameter[]
			//{
			//	new System.Data.Entity.Core.Objects.ObjectParameter("P_TABLE_NAME", tableName),
			//	new System.Data.Entity.Core.Objects.ObjectParameter("P_ID", ID )
			//	//new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", tableName),
			//	//new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", ID )

			//};

			var result = Database.SqlQuery<string>($"SELECT FN_CERCA_COORDS({ID}) FROM DUAL", new object[] { });
			return result.FirstOrDefault();
			//return result;

			//var parameter = new System.Data.Entity.Core.Objects.ObjectParameter("P_ID", ID);



			//var result = lObjectContext.CreateQuery<string>($"SELECT FN_CERCA_COORDS(ID) FROM DUAL")
			//	.Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking)
			//	.FirstOrDefault()
			//	;

			

			//return result;

			//return lObjectContext.
			//	CreateQuery<string>("EntitiesModel.Store.FN_SEGMENTOS_CATS_JERARQUIA",
			//		new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", parameter)).
			//	Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking).
			//	FirstOrDefault();

			//return "";
		}
		[DbFunction("EntitiesModel.Store", "FN_CERCA_SEGS_TRAMS_RUTS")]
		public string FN_CERCA_SEGS_TRAMS_RUTS(Int32 ID)
		{
			return "";
		}

		public System.Data.DataTable CategoriesTree<T>(string extraFields = null, bool includeOrdenador = false)
		{
			if (includeOrdenador)
				extraFields = "ORDENADOR";

			var pars = new[]
			{
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_TABLE_NAME", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = typeof(T).Name },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_EXTRA_FIELDS", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = extraFields  },
				//new Oracle.ManagedDataAccess.Client.OracleParameter("P_INCLUDE_ORDENADOR", Oracle.ManagedDataAccess.Client.OracleDbType.Int16) {Direction = System.Data.ParameterDirection.Input, Value = includeOrdenador ? 1 : 0 },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) {Direction = System.Data.ParameterDirection.Output }
			};

			var dt = Database.SqlQuery("SP_CATS", System.Data.CommandType.StoredProcedure, pars);

			if(includeOrdenador)
			{
				string ordenador;

				foreach(System.Data.DataRow row in dt.Rows)
				{
					
					
					if(row["ORDENADOR"] != DBNull.Value)
					{
						ordenador = (string)row["ORDENADOR"];

						if(!string.IsNullOrEmpty(ordenador))
						{
							row["DESCRIPCION_LARGA"] = ordenador + " - " + row["DESCRIPCION_LARGA"];
						}
					}
				}
			}


			return dt;


			//return GPSMonitoreo.Core.Utils.Data.DataTableToJsonTree(dt, new GPSMonitoreo.Core.Utils.Data.JsonTreeMapper { id = "ID", description = "DESCRIPCION_LARGA", level = "LEV", jsonNodeId = "value", jsonNodeDescription = "label", jsonNodeChildren = "items" }, true);
		}

		public System.Data.DataTable CategoriesTwoTablesTree<T1,T2>(string extraFields = null)
		{
			var pars = new[]
			{
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_TABLE_NAME_1", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = typeof(T1).Name },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_TABLE_NAME_2", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = typeof(T2).Name },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_EXTRA_FIELDS", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = extraFields  },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) {Direction = System.Data.ParameterDirection.Output }
			};


			return Database.SqlQuery("SP_CATS_TWO_TABLES", System.Data.CommandType.StoredProcedure, pars);


			//return GPSMonitoreo.Core.Utils.Data.DataTableToJsonTree(dt, new GPSMonitoreo.Core.Utils.Data.JsonTreeMapper { id = "ID", description = "DESCRIPCION_LARGA", level = "LEV", jsonNodeId = "value", jsonNodeDescription = "label", jsonNodeChildren = "items" }, true);
		}

		public System.Data.DataTable CategoriesTwoTablesTreeOrdenador<T1, T2>(string extraFields = null)
		{
			var pars = new[]
			{
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_TABLE_NAME_1", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = typeof(T1).Name },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_TABLE_NAME_2", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = typeof(T2).Name },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_EXTRA_FIELDS", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2) {Direction = System.Data.ParameterDirection.Input, Value = extraFields  },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) {Direction = System.Data.ParameterDirection.Output }
			};


			return Database.SqlQuery("SP_CATS_TWO_TABLES_ORDENADOR", System.Data.CommandType.StoredProcedure, pars);


			//return GPSMonitoreo.Core.Utils.Data.DataTableToJsonTree(dt, new GPSMonitoreo.Core.Utils.Data.JsonTreeMapper { id = "ID", description = "DESCRIPCION_LARGA", level = "LEV", jsonNodeId = "value", jsonNodeDescription = "label", jsonNodeChildren = "items" }, true);
		}

		//[DbFunction("EntitiesModel.Store", "FN_HIERARCHY_PATH")]
		//public static string FN_HIERARCHY_PATH2(string tableName, short ID)
		//{

		//Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
		//var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

		//var pars = new System.Data.Entity.Core.Objects.ObjectParameter[]
		//{
		//	new System.Data.Entity.Core.Objects.ObjectParameter("P_TABLE_NAME", tableName),
		//	new System.Data.Entity.Core.Objects.ObjectParameter("P_ID", ID )
		//	//new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", tableName),
		//	//new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", ID )

		//};



		//return lObjectContext
		//	.CreateQuery<string>("EntitiesModel.Store.FN_HIERARCHY_PATH", pars)
		//	.Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking)
		//	.FirstOrDefault();
		//}

		//[DbFunction("EntitiesModel.Store", "FN_CERCAS_CATS_JERARQUIA")]
		//public string FN_CERCAS_CATS_JERARQUIA(short parameter)
		//{
		//	var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

		//	return lObjectContext.
		//		CreateQuery<string>("EntitiesModel.Store.FN_CERCAS_CATS_JERARQUIA",
		//			new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", parameter)).
		//		Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking).
		//		FirstOrDefault();
		//}

		//[DbFunction("EntitiesModel.Store", "FN_SEGMENTOS_CATS_JERARQUIA")]
		//public string FN_SEGMENTOS_CATS_JERARQUIA(short parameter)
		//{
		//	var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

		//	return lObjectContext.
		//		CreateQuery<string>("EntitiesModel.Store.FN_SEGMENTOS_CATS_JERARQUIA",
		//			new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", parameter)).
		//		Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking).
		//		FirstOrDefault();
		//}

		//[DbFunction("EntitiesModel.Store", "FN_TRAMOS_CATS_JERARQUIA")]
		//public string FN_TRAMOS_CATS_JERARQUIA(short parameter)
		//{
		//	var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

		//	return lObjectContext.
		//		CreateQuery<string>("EntitiesModel.Store.FN_TRAMOS_CATS_JERARQUIA",
		//			new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", parameter)).
		//		Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking).
		//		FirstOrDefault();
		//}

		//[DbFunction("EntitiesModel.Store", "FN_RUTAS_CATS_JERARQUIA")]
		//public string FN_RUTAS_CATS_JERARQUIA(short parameter)
		//{
		//	var lObjectContext = ((IObjectContextAdapter)this).ObjectContext;

		//	return lObjectContext.
		//		CreateQuery<string>("EntitiesModel.Store.FN_RUTAS_CATS_JERARQUIA",
		//			new System.Data.Entity.Core.Objects.ObjectParameter("parameterName", parameter)).
		//		Execute(System.Data.Entity.Core.Objects.MergeOption.NoTracking).
		//		FirstOrDefault();
		//}


		public void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			var set = Set<TEntity>();

			var query = set.Where(predicate).Select(item => new { dummy = 1 });
			var selectSql = query.ToString();
			//string deleteSql = "DELETE [Extent1] " + selectSql.Substring(selectSql.IndexOf("FROM"));
			string deleteSql = "DELETE " + selectSql.Substring(selectSql.IndexOf("FROM"));

			var internalQuery = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_internalQuery").Select(field => field.GetValue(query)).First();
			var objectQuery = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field.Name == "_objectQuery").Select(field => field.GetValue(internalQuery)).First() as ObjectQuery;

			var cmd = Database.Connection.CreateCommand();

			//var parameters = objectQuery.Parameters.Select(p => new SqlParameter(p.Name, p.Value)).ToArray();

			var pars = new List<object>();
			System.Data.Common.DbParameter parameter;

			foreach (var p in objectQuery.Parameters)
			{
				Console.WriteLine(p.Name);
				parameter = cmd.CreateParameter();
				parameter.ParameterName = p.Name;
				parameter.Value = p.Value;
				pars.Add(parameter);
			}
			//objectQuery.Parameters.ea



			Database.ExecuteSqlCommand(deleteSql, pars.ToArray());

			//Console.WriteLine(deleteSql);
			//Console.WriteLine(internalQuery.ToString());
			//Console.WriteLine(objectQuery.ToString());
			//Utils.ObjectJsonDumper.Dump(objectQuery.Parameters, 1);




		}






	}


}
