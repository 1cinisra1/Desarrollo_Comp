using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Web.Classes;
using Newtonsoft.Json;
using GPSMonitoreo.Libraries.Trees;
using GPSMonitoreo.Core.Authorization;
using GPSMonitoreo.Web.QueryModels;
using GPSMonitoreo.Dtos.Base;
using static GPSMonitoreo.Libraries.Utils.Data;

namespace GPSMonitoreo.Web.Extensions.JqwidgetsExtensions
{
    public static class JqwidgetsExtensions
    {

		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseSimpleListDto<byte>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseSimpleListDto<Int16>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseSimpleListDto<int>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}



		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseListDto<byte>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseListDto<Int16>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseListDto<int>> enumerable)
		{
			return enumerable.Select(x => new JqwidgetsItem
			{
				value = x.Id,
				label = x.Description
			}).ToList();
		}


		//public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseSimpleListDto<int>> enumerable, bool prependBlankItem)
		//{
		//	var list = enumerable.ToJqwidgets();

		//	list.Insert(0, new JqwidgetsItem { value = 0, label = "" });

		//	return list;
		//}



		//public static List<JqwidgetsItem> ToJqwidgets(this IEnumerable<CommonBaseSimpleListDto<Int16>> enumerable, bool prependBlankItem)
		//{
		//	var list = enumerable.ToJqwidgets();

		//	list.Insert(0, new JqwidgetsItem { value = 0, label = "" });

		//	return list;
		//}

		public static List<JqwidgetsItem> ToJqwidgets(this IQueryable<GPSMonitoreo.Data.Models.ICommonEntityByte> entity)
		{

			var ret = from item in entity
					  select new JqwidgetsItem { label = item.DESCRIPCION_LARGA, value = item.ID };

			return ret.ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IQueryable<GPSMonitoreo.Data.Models.ICommonEntityInt16> entity)
		{
			var ret = from item in entity
					  select new JqwidgetsItem { label = item.DESCRIPCION_LARGA, value = item.ID };

			return ret.ToList();
		}

		public static List<JqwidgetsItem> ToJqwidgets(this IQueryable<GPSMonitoreo.Data.Models.ICommonEntityInt32> entity)
		{
			var ret = from item in entity
					  select new JqwidgetsItem { label = item.DESCRIPCION_LARGA, value = item.ID };

			return ret.ToList();
		}


		public static List<JqwidgetsItem> AddBlankItem(this List<JqwidgetsItem> list)
		{
			list.Insert(0, new JqwidgetsItem { value = 0, label = "" });
			return list;
		}

		public static List<JqwidgetsItem> PrependItem(this List<JqwidgetsItem> list, int value, string label)
		{
			list.Insert(0, new JqwidgetsItem { value = value, label = label });
			return list;
		}

		public static List<JqwidgetsItem> PrependBlankItem(this List<JqwidgetsItem> list)
		{
			list.Insert(0, new JqwidgetsItem { value = 0, label = "" });
			return list;
		}

		public static string ToJqwidgetsTree(this System.Data.DataTable dt, bool addBlankItem = true)
		{
			Dictionary<string, string> extraFields = null;
			return ToJqwidgetsTree(dt, extraFields, addBlankItem);
		}

		public static string ToJqwidgetsTree(this System.Data.DataTable dt, JsonTreeMapper mapper, bool addBlankItem = true)
		{

			return DataTableToJsonTree(dt, mapper, addBlankItem);
		}

		public static string ToJqwidgetsTree(this System.Data.DataTable dt, Dictionary<string, string> extraFields, bool addBlankItem = true)
		{

			var mapper = new JsonTreeMapper
			{
				id = "ID",
				description = "DESCRIPCION_LARGA",
				level = "LEV",
				jsonNodeId = "value",
				jsonNodeDescription = "label",
				jsonNodeChildren = "items",
				extraFields = extraFields
			};

			return ToJqwidgetsTree(dt, mapper, addBlankItem);
		}



		private static string MenuItemToJqwidgetTreeItem(MenuItem menuItem, RolePermissions rolePermissions)
		{
			var ret = new System.Text.StringBuilder();

			ret.Append("{");
			if (menuItem.OnClick == null)
			{
				ret.Append("\"html\": " + JsonConvert.ToString(menuItem.Icon + menuItem.Title));
			}
			else
			{
				ret.Append("\"html\": " + JsonConvert.ToString("<div onclick=\"" + menuItem.OnClick + "\">" + menuItem.Icon + menuItem.Title + "</div>"));
			}


			if (menuItem.Expanded)
			{
				ret.Append(", \"expanded\": true");
			}

			if (menuItem.OnClick != null)
			{
				ret.Append(", \"onClick\": " + JsonConvert.ToString(menuItem.OnClick));
			}

			if (menuItem.Items != null)
			{
				ret.Append(", \"items\": " + MenuItemsToJqwidgetTreeItems(menuItem.Items, rolePermissions));
			}
			ret.Append("}");
			return ret.ToString();
		}


		//private static string MenuItemToJqwidgetTreeItem(MenuItem menuItem, Role role)
		//{
		//	var ret = new System.Text.StringBuilder();

		//	ret.Append("{");
		//	if(menuItem.OnClick == null)
		//	{
		//		ret.Append("\"html\": " + JsonConvert.ToString(menuItem.Icon + menuItem.Title));
		//	}
		//	else
		//	{
		//		ret.Append("\"html\": " + JsonConvert.ToString("<div onclick=\"" + menuItem.OnClick + "\">" + menuItem.Icon + menuItem.Title + "</div>"));
		//	}
			

		//	if(menuItem.Expanded)
		//	{
		//		ret.Append(", \"expanded\": true");
		//	}

		//	if(menuItem.OnClick != null)
		//	{
		//		ret.Append(", \"onClick\": " + JsonConvert.ToString(menuItem.OnClick));
		//	}

		//	if(menuItem.Items != null)
		//	{
		//		ret.Append(", \"items\": " + MenuItemsToJqwidgetTreeItems(menuItem.Items, role));
		//	}
		//	ret.Append("}");
		//	return ret.ToString();
		//}

		//private static string MenuItemsToJqwidgetTreeItems(List<MenuItem> items, Role role)
		//{
		//	var ret = new System.Text.StringBuilder();
			
		//	MenuItem menuItem;

		//	for (int x = 0; x < items.Count; x++)
		//	{
		//		menuItem = items[x];

		//		if (menuItem.HasRoleAccess(role))
		//		{
		//			if (ret.Length > 0)
		//				ret.Append(", ");

		//			ret.Append(MenuItemToJqwidgetTreeItem(menuItem, role));
		//		}
		//	}
			
		//	return "[" + ret.ToString() + "]";
		//}

		private static string MenuItemsToJqwidgetTreeItems(List<MenuItem> items, RolePermissions rolePermissions)
		{
			var ret = new System.Text.StringBuilder();

			MenuItem menuItem;

			for (int x = 0; x < items.Count; x++)
			{
				menuItem = items[x];



				//if (menuItem.HasRoleAccess(role))
				if (rolePermissions.IsGranted(menuItem))
				{
					if (ret.Length > 0)
						ret.Append(", ");

					ret.Append(MenuItemToJqwidgetTreeItem(menuItem, rolePermissions));
				}
			}

			return "[" + ret.ToString() + "]";
		}

		//public static string ToJqwidgetsTree(this Classes.Menu menu, Role role)
		//{
		//	if(menu.Items != null)
		//	{
		//		return MenuItemsToJqwidgetTreeItems(menu.Items, role);
		//	}
		//	else
		//	{
		//		return "[]";
		//	}
		//}

		public static string ToJqwidgetsTree(this Classes.Menu menu, RolePermissions rolePermissions)
		{
			if (menu.Items != null)
			{
				return MenuItemsToJqwidgetTreeItems(menu.Items, rolePermissions);
			}
			else
			{
				return "[]";
			}
		}

		public static string ToJqwidgetsTree(this RoledTree<string, Role, int> tree, Role role)
		{
			if (tree.Children != null)
			{
				return RoledTreeChildrenToJqwidgetTreeItems(tree.Children, role);
			}
			else
			{
				return "[]";
			}
		}

		private static string RoledTreeChildrenToJqwidgetTreeItems<TId, TRoles, TPermissions>(List<RoledNode<TId, TRoles, TPermissions>> children, TRoles role)
		{
			var ret = new System.Text.StringBuilder();

			RoledNode<TId, TRoles, TPermissions> child;

			for (int x = 0; x < children.Count; x++)
			{
				child = children[x];

				

				if (child.HasRoleAccess(role))
				{
					if (ret.Length > 0)
						ret.Append(", ");

					ret.Append(RoledTreeChildToJqwidgetTreeItem(child, role));
				}
			}

			return "[" + ret.ToString() + "]";
		}


		private static string RoledTreeChildToJqwidgetTreeItem<TId, TRoles, TPermissions>(RoledNode<TId, TRoles, TPermissions> child, TRoles role)
		{
			var ret = new System.Text.StringBuilder();

			ret.Append("{");
			//if (menuItem.OnClick == null)
			//{
			//	ret.Append("\"html\": " + JsonConvert.ToString(menuItem.Icon + menuItem.Title));
			//}
			//else
			//{
			//	ret.Append("\"html\": " + JsonConvert.ToString("<div onclick=\"" + menuItem.OnClick + "\">" + menuItem.Icon + menuItem.Title + "</div>"));
			//}

			//JsonConvert.ToString()

			ret.Append("\"value\": " + JsonConvert.ToString(child.Id));
			ret.Append(", \"label\": " + JsonConvert.ToString(child.Description));


			//if (menuItem.Expanded)
			//{
			//	ret.Append(", \"expanded\": true");
			//}

			//if (menuItem.OnClick != null)
			//{
			//	ret.Append(", \"onClick\": " + JsonConvert.ToString(menuItem.OnClick));
			//}

			if (child.Children != null)
			{
				ret.Append(", \"items\": " + RoledTreeChildrenToJqwidgetTreeItems(child.Children, role));
			}
			ret.Append("}");
			return ret.ToString();
		}
	}
}
