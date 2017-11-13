using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos
{


	public class FilterInputOrderBy : List<FilterInputOrderByItem>
	{
		public static FilterInputOrderBy DefaultOrderByIdAscending
		{
			get
			{
				return new FilterInputOrderBy
				{
					new FilterInputOrderByItem("Id")
				};
			}
		}

		public static FilterInputOrderBy DefaultOrderByIdDescending
		{
			get
			{
				return new FilterInputOrderBy
				{
					new FilterInputOrderByItem("Id", FilterInputOrderByItem.OrderByDirection.Descending)
				};
			}
		}

		public static FilterInputOrderBy DefaultOrderByDescriptionAscending
		{
			get
			{
				return new FilterInputOrderBy
				{
					new FilterInputOrderByItem("Description")
				};
			}
		}

		public static FilterInputOrderBy DefaultOrderByDescriptionDescending
		{
			get
			{
				return new FilterInputOrderBy
				{
					new FilterInputOrderByItem("Description", FilterInputOrderByItem.OrderByDirection.Descending)
				};
			}
		}


		public string GetOrderBy(StringDictionary mapping)
		{
			string mappedField;
			var mappedList = new List<string>(this.Count);

			foreach (var item in this)
			{
				mappedField = mapping[item.Field];

				if (!string.IsNullOrEmpty(mappedField))
				{
					mappedList.Add(mappedField + (item.Direction == FilterInputOrderByItem.OrderByDirection.Descending ? " DESC" : ""));
				}
			}

			return string.Join(", ", mappedList);
		}

		//public string GetOrderBy(Func<string, string> fieldMapper)
		//{
		//	string mappedField;
		//	var mappedList = new List<string>(OrderBy.Count);

		//	foreach(var item in OrderBy)
		//	{
		//		mappedField = fieldMapper(item.Field);
		//		//mappedList.Add()
		//		if (!string.IsNullOrEmpty(mappedField))
		//		{
		//			mappedList.Add(mappedField + (item.Direction == OrderByDirection.Descending ? " DESC" : ""));
		//		}
		//	}

		//	return string.Join(", ", mappedList);
		//}


	}



	public class FilterInputOrderByItem
	{

		public enum OrderByDirection
		{
			Ascending = 0,
			Descending = 1
		}

		public string Field { get; set; }

		public OrderByDirection Direction { get; set; }

		public FilterInputOrderByItem()
		{

		}

		public FilterInputOrderByItem(string field)
		{
			Field = field;
			Direction = OrderByDirection.Ascending;
		}

		public FilterInputOrderByItem(string field, OrderByDirection direction)
		{
			Field = field;
			Direction = direction;
		}
	}



	public class FilterInputDto: InputDto
    {


		public int Page { get; set; }

		public int PageSize { get; set; }

		public int RecordCount { get; set; }

		public bool Paging { get; set; } = true;

		public FilterInputOrderBy OrderBy { get; set; }

		public override void OnBindingFinished()
		{
			if (OrderBy == null)
			{
				OrderBy = FilterInputOrderBy.DefaultOrderByIdAscending;
			}
		}
	}
}
