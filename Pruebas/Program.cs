using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data;
using GPSMonitoreo.Data.Extensions;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using NReco.Linq;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Devices;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Core.Utils;
using GPSMonitoreo.Dtos.Extensions.LinqExtensions;
using GPSMonitoreo.Dtos.HybridMapper;
using GPSMonitoreo.Dtos.HybridMapper.Attributes;
using System.Runtime.Serialization;
using Pruebas.Libraries;

namespace Pruebas
{

	public class MyBaseClass
	{
		internal void InitializeCore()
		{


			OnInitialize();


		}

		protected virtual void OnInitialize()
		{
			Console.WriteLine("MyBaseClass initializer");
			
		}


		public MyBaseClass()
		{
			Console.WriteLine("MyBaseClass constructor");
			this.Initialize();
		}

		public string MyProp
		{
			get { return "from MyBaseClass"; }
		}
	}

	public class MyDerivedClass : MyBaseClass
	{

		public new string MyProp
		{
			get { return "from MyDerivedClass"; }
		}



		protected override void OnInitialize()
		{
			base.OnInitialize();
			Console.WriteLine("MyDerivedClass initializer");
			Console.WriteLine("MyDerivedClass finished");
		}

		public MyDerivedClass() : base()
		{
			Console.WriteLine("MyDerivedClass constructor");
			this.Initialize();
		}
	}

	public static class MyClassExtensions
	{
		public static void Initialize<TClass>(this TClass obj) where TClass : MyBaseClass
		{
			if (obj.GetType() == typeof(TClass))
				obj.InitializeCore();
		}
	}

	public class Person
	{
		public int Age { get; set; }
		public int Weight { get; set; }

		public object Height { get; set; }

		public Person():this(true)
		{
			Console.WriteLine("Person constructed, Age: " + Age.ToString());
		}

		public Person(bool dummy)
		{

			Console.WriteLine("Person constructed dummy");

		}

		
	}



	public class Program
	{

		public static void prueba()
		{

			var context = new GPSMonitoreo.Data.Models.EntitiesContext();
			context.Database.Log = Console.Write;

			var pars = new[]
			{
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CAPA", Oracle.ManagedDataAccess.Client.OracleDbType.Int32) {Value = 1 },
				new Oracle.ManagedDataAccess.Client.OracleParameter("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor) {Direction = ParameterDirection.Output }
			};

			//new Oracle.ManagedDataAccess.Client.OracleParameter("P_CAPA", Oracle.ManagedDataAccess.Client.OracleDbType.



			var dt = context.Database.SqlQuery("SP_CERCAS_CATEGORIAS", CommandType.StoredProcedure, pars);

			var json = GPSMonitoreo.Core.Utils.Data.DataTableToJsonTree(dt, new GPSMonitoreo.Core.Utils.Data.JsonTreeMapper { id = "CCI_CODIGO", description = "CTX_DESCRIPCION_LARGA", level = "LEV", jsonNodeId = "value", jsonNodeDescription = "label", jsonNodeChildren = "items" });

			Console.WriteLine(json);
			//GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(dt, 1);

			return;




			var data = context.Database.SqlQuery<GPSMonitoreo.Data.QueryModels.SimpleInt16>("SP_CERCAS_CATEGORIAS(:P_CAPA, :P_CUR); END;", pars);










			foreach (var item in data)
			{

				Console.WriteLine(item.codigo + item.descripcion);
			}

			//var data2 = from item in context.CERCAS_CATS.ToSimple()
			//			where item.codigo == 1
			//			select item;



			////Console.WriteLine(data2.Count().ToString());

			////var res = data2.ToList<GPSMonitoreo.Data.QueryModels.Simple>();


			//foreach (var item in data2)
			//	Console.WriteLine(item.codigo + ":" + item.descripcion);

			//Console.WriteLine(item.descripcionLarga);



			return;


			context.Database.Connection.Open();

			var cmd = (Oracle.ManagedDataAccess.Client.OracleCommand)context.Database.Connection.CreateCommand();


			cmd.CommandText = "SP_CERCAS_CATEGORIAS";
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("P_CAPA", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = 1;
			cmd.Parameters.Add("P_CUR", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor).Direction = ParameterDirection.Output;

			var reader = cmd.ExecuteReader();

			while (reader.Read())
			{

				Console.WriteLine(reader["CTX_DESCRIPCION_LARGA"].ToString());

			}

			context.Database.Connection.Close();






			Console.WriteLine(cmd.ToString());


			////var qry = context.Database.SqlQuery(typeof(int), "select * from USUARIOS", new object[] { });

			////var qry2 = context.Database.SqlQuery<GPSMonitoreo.Data.Models.TB_PAISES>("select * from USUARIOS", new object[] { });

			//var data = from item in context.CERCAS_CAPAS
			//		   select item;

			////context.Database.Connection.Open();



			//GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(data, 1);



			//foreach (var item in data)
			//{
			//	Console.WriteLine(item.CTX_DESCRIPCION_LARGA);
			//}


		}


	

		public static void pruebalinq2()
		{
			var lambdaParser = new NReco.Linq.LambdaParser();

			

			//var p = new Person() { Age = 10, Weight = 1, Height = new { min = 10, max = 20} };
			var p = new Person() { Age = 10, Weight = 1, Height = new { min = 10, max = 20} };



			var values = new Dictionary<string, object>();



			//values["p"] = p;
			values["p"] = "fabian";


			var compiled = lambdaParser.CompileExpression("p == 'fabians'");


			//compiled.Lambda.Method.Invoke(p, new object[] { });

			//compiled.Lambda.
			
			//compiled.Evaluate(values);
			//var temp = compiled.Lambda.Method.
			//Console.WriteLine(temp[0].Method.GetMethodBody().ToString());
			//GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(temp, 1);
			//Console.WriteLine(temp);

			//var context = new GPSMonitoreo.Data.Models.EntitiesContext();
			////context.CERCAS_CALZADAS.Where(compiled.Lambda);


			////Expression<Func<GPSMonitoreo.Data.Models.CERCAS_CALZADAS, bool>> expr = new Expression<Func<GPSMonitoreo.Data.Models.CERCAS_CALZADAS, bool>>();

			//Func<GPSMonitoreo.Data.Models.CERCAS_CALZADAS, bool> func = (item) =>
			//{
			//	item.wh

			//	return false;
			//};

			//context.CERCAS_CALZADAS.Where(func);









			
			var parsed = lambdaParser.Parse("p == 'fabians' && (p == 'aaa')");

			var pars = LambdaParser.GetExpressionParameters(parsed);
			

			var valuesList = new List<object>();

			foreach (var par in pars)
			{
				valuesList.Add(new LambdaParameterWrapper(values[par.Name]));
			}




			//Console.WriteLine(parsed.ToString());
			
			//var xx = Expression.Lambda(parsed, valuesList.ToArray());
			//Console.WriteLine(xx.ToString());
			return;
			//parsed

			//Console.WriteLine(parsed.ToString());
			//var body = compiled.
			//Console.WriteLine(compiled.Lambda.Method.GetMethodBody().ToString());
			//Console.WriteLine(compiled.Evaluate(values));

			var result = compiled.Evaluate(values);
			Console.WriteLine(result.GetType().ToString());





			//var valuesList = new List<object>();
			//foreach (var paramExpr in compiledExpr.Parameters)
			//{
			//	valuesList.Add(new NReco.Linq.LambdaParameterWrapper(varContext[paramExpr.Name]));
			//}

			//var lambdaRes = compiledExpr.Lambda.DynamicInvoke(valuesList.ToArray());
			//if (lambdaRes is NReco.Linq.LambdaParameterWrapper)
			//	lambdaRes = ((NReco.Linq.LambdaParameterWrapper)lambdaRes).Value;


			//Console.WriteLine(lambdaRes);



			//NReco.Linq.LambdaParser.CompiledExpression.

			//"p.Age > 5 && p.Weight > 10"


			//lambdaParser.Eval()

			//Console.WriteLine(lambdaParser.Eval("p.Age < 5", values)); // --> 5


		}

		public static void prueba4()
		{
			//var schemaMapping = new System.Collections.Specialized.StringDictionary() { { "MONITOREO", null } };
			////var ctx = new GPSMonitoreo.Data.Models.EntitiesContext("DATA SOURCE=localhost:1521/XE;PASSWORD=fromberg100;USER ID=MONITOREO", "Models.EntitiesModel", null, true);
			//var ctx = new GPSMonitoreo.Data.Models.EntitiesContext("DATA SOURCE=localhost:1521/XE;PASSWORD=fromberg100;USER ID=MONITOREO", "Models.EntitiesModel", schemaMapping, true);
			//Console.WriteLine(ctx.CERCAS.Count());
		}

		public static void prueba5()
		{
			var entitiesSchemaMapping = new Dictionary<string, string>() { { "MONITOREO", null } };
			var methodsSchemaMapping = new Dictionary<string, string>() { { "MONITOREO", "MONITOREO" } };

			var ctx = new GPSMonitoreo.Data.Models.EntitiesContext("DATA SOURCE=localhost:1521/XE;PASSWORD=fromberg100;USER ID=MONITOREO", "Models.EntitiesModel", entitiesSchemaMapping, methodsSchemaMapping, true);

			//GPSMonitoreo.Data.Models.IComunByte ii;

			//var entity = new GPSMonitoreo.Data.Models.EQUIPOS_GRUPOS();
			//entity.ID = 0;
			//entity.ABREVIACION = "aa";
			//entity.DESCRIPCION_CORTA = "bb";
			//entity.DESCRIPCION_MED = "asdf";
			//entity.DESCRIPCION_LARGA = "prueba";
			//entity.ESTADO_ID = 0;

			

			

			//ctx.BaseEntitySet.Add(entity);
			//ctx.SaveChanges();

			//ctx

			//ii = new GPSMonitoreo.Data.Models.EQUIPOS_GRUPOS();


			//ii = new GPSMonitoreo.Data.Models.eq



		}



		[Map("BaseDto")]
		public class BaseDto
		{
			static BaseDto()
			{
				HybridMapper.Map(typeof(BaseDto));
				//Mappit();
				Console.WriteLine("static BaseDto");
			}


			//public new static void Mappit()
			//{
			//	TypeInfo xx;
			//	Type tt;
			//	CustomAttributeData data;
			//	CustomAttributeNamedArgument named;
			//	Console.WriteLine("descriptor: " + TypeDescriptor.GetAttributes(typeof(BaseDto)).Count);

			//	//TypeDescriptor.AddAttributes()
			//	//Console.WriteLine("static BaseDto Mappit");

			//}

		}

		//[Map("DerivedDto")]
		public class DerivedDto: BaseDto
		{
			static DerivedDto()
			{
				HybridMapper.Map(typeof(DerivedDto));
				
				
				Console.WriteLine("static DerivedDto");
			}


			//public new static void Mappit()
			//{
			//	Console.WriteLine("static DerivedDto Mappit");

			//}
		}

		public static void pruebaTree()
		{
			//ViewData["alarmas"] = DBContext.CategoriesTwoTablesTree<ALARMAS_CATS, ALARMAS>()


			var context = new GPSMonitoreo.Data.Models.EntitiesContext(true);
			context.Database.Log = Console.WriteLine;

			var dt = context.CategoriesTwoTablesTree<ALARMAS_CATS, ALARMAS>();

			Console.WriteLine(dt.Rows.Count);


			Node<string> currentNode;
			Node<string> childNode;

			var tree = new Tree<string>(true);


			currentNode = tree;




			int level = 0;
			int nextLevel;
			var json = new System.Text.StringBuilder();
			json.Append("[");


			int x = 0;

			System.Data.DataRow next;

			foreach (System.Data.DataRow row in dt.Rows)
			{
				level = (int)((decimal)row["LEV"]);


				childNode = new Node<string>()
				{
					Id = (string)row["ID"],
					Title = (string)row["DESCRIPCION_LARGA"]
				};

				currentNode.Add(childNode);

				//json.Append("{\"" + mapping.jsonNodeId + "\": " + JsonConvert.SerializeObject(row[mapping.id]) + ", \"" + mapping.jsonNodeDescription + "\": " + JsonConvert.ToString(row[mapping.description]));

				if ((x + 1) < dt.Rows.Count)
				{
					next = dt.Rows[x + 1];

					nextLevel = (int)((decimal)next["LEV"]);

					if (nextLevel > level)
					{
						//json.Append(", \"" + mapping.jsonNodeChildren + "\": [");
						currentNode = childNode;
					}
					else if (nextLevel < level)
					{
						//json.Append(string.Join("", Enumerable.Repeat("}]", level - nextLevel)) + "}, ");

						int diff = level - nextLevel;
						while(diff > 0)
						{
							currentNode = currentNode.Parent;
							diff--;
						}
					}
					//else
					//	json.Append("}, ");
				}
				x++;
			}



			//if(level > 0)
			//json.Append(string.Join("", Enumerable.Repeat("}]", level)) + "}");




			//json.Append("]");

			//GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(tree.AllNodes, 1);

			//var mm = new Menu();

			//mm.Add(
			//	new MenuItem()
			//	{
			//		Id = "1",
			//		Nodes = new List<MenuItem>
			//		{
			//			new MenuItem()
			//			{
			//				Id = "2"
			//			},
			//			new MenuItem()
			//			{
			//				Id = "3"
			//			}
			//		}
			//	}
			//);

			var tt = new RoledTree<string, int, byte>(true);

			tt.Populate(dt, "ID", "DESCRIPCION_LARGA", "LEV", (node, row) =>
			{
				Console.WriteLine("Adding node: " + node.Description);
			});



			GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(tt.AllNodes, 1);
			//Console.WriteLine("root is: " + child2.Tree.Id);


		}

		public static void Main(string[] args)
		{
			//var x = new MyDerivedClass();
			//var c = (MyBaseClass)x;
			//Console.WriteLine(x.MyProp);
			//Console.WriteLine(c.MyProp);

			//var inst1 = new BaseDto();
			//var inst2 = new DerivedDto();
			////var inst3 = new DerivedDto();


			////HybridMapper.Test(inst2.GetType());
			//HybridMapper.Test(inst2.GetType());
			//HybridMapper.Test(inst2.GetType().BaseType);
			//HybridMapper.Test(inst2.GetType());

			pruebaTree();

			return;

			var context = new GPSMonitoreo.Data.Models.EntitiesContext();
			context.Database.Log = Console.WriteLine;

			//var query = (IQueryable<IComunInt32>)context.GPS.Where(x => x.ID == 1).AsQueryable();
			//var query = (IQueryable<IComunInt32>)context.GPS.Where(x => x.ID == 1);

			//Console.WriteLine(query.Select(x => new { id = x.ID, descrip = x.DESCRIPCION_LARGA }));

			//prueba5();


			//Expression<Func<IComunInt32, Dto<int>>> exp1 = x => new Dto<int>
			//{
			//	Id = x.ID,
			//	Description = x.DESCRIPCION_LARGA
			//};

			//var exp2 = exp1.Ext2<GPS, GpsDto>(x => new GpsDto
			//{
			//	EquipmentId = x.EQUIPO_ID,
			//	EquipmentDescription = x.EQUIPO.DESCRIPCION_LARGA
			//});


			Expression<Func<GPS, GpsDto>> exp1 = x => new GpsDto()
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				EquipmentDescription = x.EQUIPO.DESCRIPCION_LARGA
			};

			//var exp2 = exp1.Extend2(x => new AlarmedGpsDto
			//{
			//	Alarms = x.ALARMAS.Select(item => new GpsAlarmDto
			//	{

			//	}).ToList()

			//});



			//Console.WriteLine(exp2.ToString());

			//var exp2 = exp1.Extend((x) => new GpsDto
			//{
			//	EquipmentId = ((GPS)x).EQUIPO_ID,
			//	EquipmentDescription = ((GPS)x).EQUIPO.DESCRIPCION_LARGA
			//});

			//exp1.Extend<GPS>((GPS x) => new GpsDto
			//{
			//	EquipmentId = ((GPS)x).EQUIPO_ID,
			//	EquipmentDescription = ((GPS)x).EQUIPO.DESCRIPCION_LARGA
			//});



			//var exp3 = exp1.Extend2<GPS, GpsDto>((GPS x) => new GpsDto
			//{
			//	EquipmentId = x.EQUIPO_ID,
			//	EquipmentDescription = x.EQUIPO.DESCRIPCION_LARGA
			//});

			//var exp3 = exp1.Extend(x => new GpsDto
			//{
			//	EquipmentId = x.EQUIPO_ID,
			//	EquipmentDescription = x.EQUIPO.DESCRIPCION_LARGA
			//});


			//var exp4 = exp2.Extend(x => new AlarmedGpsDto
			//{
			//	Alarms = x.ALARMAS.Select(y => new GpsAlarmDto {

			//	}).ToList()
			//});



			//var query = context.GPS.Where(x => x.ID == 1);

			//Console.WriteLine("before select");
			//var sel = query.Select(exp2);
			//Console.WriteLine("after select");
			//var sel = query.Select(x => new
			//{
			//	Id = x.ID,
			//	Description = x.DESCRIPCION_LARGA,
			//	EquipmentId = x.EQUIPO_ID,
			//	EquipmentDescription = x.EQUIPO.DESCRIPCION_LARGA,
			//	Alarms = x.ALARMAS.Select(item => new GpsAlarmDto
			//	{

			//	}).ToList()


			//});

			//ObjectJsonDumper.Dump(sel, 1);

			//foreach(var item in sel.ToList())
			//{
			//	Console.WriteLine(item.Description);
			//}





			//var p = new Person() { Age = 100 };



			//prueba();
			//pruebalinq2();

			//var data = from user in context.USUARIOS
			//		   select new { xx = user.NOMBRE };


			//Console.WriteLine(data.Count());


			//var data2 = from user in context.USUARIOS
			//		   select new { xx = user.NOMBRE };

			//Console.WriteLine(data2.Count());

			//foreach(var user in data)
			//{
			//	Console.WriteLine(user.NOMBRE);
			//}

			Console.WriteLine("fin");



			

			
        }
    }
}
