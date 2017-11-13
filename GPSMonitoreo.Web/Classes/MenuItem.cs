using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Core.Authorization;

namespace GPSMonitoreo.Web.Classes
{
    public class MenuItem: RestrictedElement
    {
		public List<MenuItem> Items;
		public string Title;
		public string Icon;
		public string Url;
		public string OnClick;
		public bool Expanded;

		public MenuItem(string title, string url = null, string onClick = null, string icon = null, bool expanded = false, List<MenuItem> items = null,  Role[] roles = null, RequiredPermission[] permissions = null)
		{
			Title = title;
			Url = url;
			OnClick = onClick;
			Icon = icon;
			Expanded = expanded;
			Items = items;
			RequiredRoles = roles;
			RequiredPermissions = permissions;
		}

		public MenuItem AddItem(MenuItem item)
		{
			if(Items == null)
			{
				Items = new List<MenuItem>();
			}
			Items.Add(item);

			return this;
		}

		public bool HasRoleAccess(Role role)
		{
			if(RequiredRoles != null && Array.IndexOf<Role>(RequiredRoles, role) == -1)
			{
				return false;
			}

			return true;

		}
    }



}
