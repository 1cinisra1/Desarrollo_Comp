using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Extensions.TaskExtensions
{
    public static class TaskExtensions
    {
		public static void RunSync(this Task t)
		{
			var task = Task.Run(async () => await t);
			task.Wait();
		}

		public static T RunSync<T>(this Task<T> t)
		{
			T res = default(T);
			var task = Task.Run(async () => res = await t);
			task.Wait();
			return res;
		}
	}
}
