using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Extensions.ListDtoExtensions
{
    public static class ListDtoExtensions
    {
		public static List<CommonBaseSimpleListDto<TId>> PrependBlankItem<TId>(this List<CommonBaseSimpleListDto<TId>> list)
		{
			var temp = list.ToList();
			list.Capacity++;
			list.Clear();
			list.Add(new CommonBaseSimpleListDto<TId>());
			list.AddRange(temp);

			return list;
		}

		public static List<CommonDbEntitySimpleListDto<TId>> PrependBlankItem<TId>(this List<CommonDbEntitySimpleListDto<TId>> list)
		{
			var temp = list.ToList();
			list.Capacity++;
			list.Clear();
			list.Add(new CommonDbEntitySimpleListDto<TId>());
			list.AddRange(temp);

			return list;
		}
	}
}
