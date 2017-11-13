using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using GPSMonitoreo.Services.Base.Users;
using MVCHelpers.ActionResults;
using System.Security.Claims;
using GPSMonitoreo.Web.Authorization;
using GPSMonitoreo.Core.Enums;
using Microsoft.AspNetCore.Http.Authentication;

using Serilog;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers
{


	[Authorize]
	public class AccountController : BaseController
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		
		[HttpPost]
		public async Task<ActionResult> Login(string username, string password)
		{

			//ClaimTypes.Role
			//ClaimValueTypes.
			//var claim = new Claim()


			//var userService = ServiceProvider.GetService<UserService>();

			//var output = await userService.ValidateLoginAsync(username, password);

			//Utils.dump(output);

			var userEntity = DBContext.USUARIOS.AsNoTracking().FirstOrDefault(x => x.USUARIO == username && x.CLAVE == password);

			if(userEntity != null)
			{
				var claims = new List<Claim>();


				claims.Add(new Claim("name", "Fabian"));
				//claims.Add(new Claim("role", Role.SuperAdmin.ToString("D"), ClaimValueTypes.Integer));
				claims.Add(new Claim("role", userEntity.ROL_ID.ToString(), ClaimValueTypes.Integer));

				claims.Add(new Claim("id", userEntity.ID.ToString(), ClaimValueTypes.Integer));


				var userIdentity = new ClaimsIdentity(claims, "local", "name", "role");

				//userIdentity.AddClaims(claims);
				var user = new ClaimsPrincipal(userIdentity);


				//var user = new AppUser(userIdentity);




				await HttpContext.Authentication.SignInAsync("Cookies", user,
					new AuthenticationProperties
					{
						//ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
						ExpiresUtc = DateTime.UtcNow.AddMinutes(100),
						IsPersistent = true,
						AllowRefresh = false
					});

			}




			



			//return Content("nada");

			//return JsonResponse("OK", output);

			//return Content("posi");
			//return JsonOk("posi");
			return Redirect("/");

		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.Authentication.SignOutAsync("Cookies");
			return Redirect("/");
		}

		//[Role(Role.SuperAdmin, Role.InternalMonitorist, Role.CustomerUser)]
		[HttpGet]
		public IActionResult TestRole()
		{
			//HttpContext.u

			//HttpContext.prin

			//IdentityUser xx;

			Console.WriteLine("probando");

			Serilog.Log.Debug("probando: {@xxxx} ", new { prop = 1, prop2 = 3 });
			
			
			

			//var user = (AppUser)HttpContext.User;
			//var user = (AppUser)this.User;
			//var user = new AppUser(this.User);
			
			//Utils.dump(user.ToString());
			//return View();
			return Content("reposi");
		}


	}
}
