using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.HybridMapper
{

	public class MapPair : Attribute
	{
		private Tuple<object, object> _pair;
		public MapPair()
		{
			_pair = new Tuple<object, object>(null, null);
			Console.WriteLine("pair constructed");
		}

	}


	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MapSetting: Attribute
	{
		public List<Tuple<object, object>> mappings;

		public MapSetting()
		{
			Console.WriteLine("MapSetting()");
			mappings = new List<Tuple<object, object>>();

		}

		public void AddMapping(object item1, object item2)
		{
			mappings.Add(new Tuple<object, object>(item1, item2));
		}
	}

	public class HybridMapper
    {
		public static void Map(Type t)
		{
			RuntimeHelpers.RunClassConstructor(t.BaseType.TypeHandle);

			var setting = new MapSetting();
			setting.AddMapping(null, null);
			//setting.mappings.Add(new )

			//var l = t.GetCustomAttributesData();
			//TypeDescriptor.AddAttributes(t, new MapPair());
			//TypeDescriptor.AddAttributes(t, new MapPair());
			//TypeDescriptor.AddAttributes(t, new MapPair());

			TypeDescriptor.AddAttributes(t, setting);
			TypeDescriptor.AddAttributes(t, new MapPair());

			//Console.WriteLine(TypeDescriptor.GetAttributes(t).Count);



			//l.Add(new MapPair());
			//l.Add(new MapPair());
			//l.Add(new MapPair());


			Console.WriteLine("base: " + t.BaseType.Name);
			Console.WriteLine("mapping: " + t.Name);
			
		}

		public static void Test(Type type)
		{
			//var attrs = type.GetCustomAttributes();
			//Console.WriteLine("after type.GetCustomAttributes()");

			//foreach (var attr in attrs)
			//{
			//	Console.WriteLine(attr.GetType().ToString());
			//}

			//Console.WriteLine("after loop of type.GetCustomAttributes()");

			var attrs = TypeDescriptor.GetAttributes(type);

			Console.WriteLine("after TypeDescriptor.GetAttributes(type)");


			
			
			

			foreach (Attribute attr in attrs)
			{
				var usage = (AttributeUsageAttribute)attr;
				
				//Console.WriteLine(attr.GetType().ToString() + ": " + attr.ToString());
				Console.WriteLine(attr.ToString() + ":" + type.IsDefined(attr.GetType(), false));
			}

			Console.WriteLine("after loop of TypeDescriptor.GetAttributes(type)");



		}
	}
}
