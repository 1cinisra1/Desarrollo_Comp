using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;


namespace GPSMonitoreo.Libraries.DataAnnotations
{

	//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false)]
    public class ResourceManagerAttribute: Attribute
    {
		public static Dictionary<Type, ResourceManager> ResourceManagers = new Dictionary<Type, ResourceManager>();

		public ResourceManager ResourceManager;

		public ResourceManagerAttribute(Type ResourceManagerType): base()
		{
			ResourceManager = ResourceManagers[ResourceManagerType];
		}
    }
}
