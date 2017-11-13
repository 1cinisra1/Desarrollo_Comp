using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.HybridMapper.Attributes
{

	//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class MapAttribute: Attribute
    {
		private string _name;

		public MapAttribute(string name)
		{
			Console.WriteLine("MapAttribute()");
			_name = name;
		}

		public override string ToString()
		{
			return "MapAttribute: " + _name;
		}

		//public override object TypeId
		//{
		//	get
		//	{
		//		return base.TypeId;
		//	}
		//}


	}
}
