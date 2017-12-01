using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Equipments;
using GPSMonitoreo.Dtos.Base.Models;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Services.Base.CommonDbEntities;
using GPSMonitoreo.Services.Base.Equipments;
using GPSMonitoreo.Services.Base.Models;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MVCHelpers.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers.Tecnicos
{
    public class TecnicoController : AdminBaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


       
        public async Task<IActionResult> EditForm()
        {
            //forma antigua
            ViewData["categories"] =  "";
           

            return View();
        }


      
    }
}
