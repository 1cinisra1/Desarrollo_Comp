using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
namespace GPSMonitoreo.Web.ViewComponents
{
    public class Grid : ViewComponent
    {
		public Grid()
		{


		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			//var items = await GetItemsAsync(maxPriority, isDone);
			return View();
		}
		//private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
		//{
		//	return db.ToDo.Where(x => x.IsDone == isDone &&
		//						 x.Priority <= maxPriority).ToListAsync();
		//}

	}
}
