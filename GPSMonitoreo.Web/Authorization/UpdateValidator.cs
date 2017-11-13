using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Authorization
{
	public static class UpdateValidator
	{
		public static async Task ValidateAsync(CookieValidatePrincipalContext context)
		{
			//check for changes to profile here

			//build new claims pricipal.
			//var newprincipal = new System.Security.Claims.ClaimsPrincipal();

			// set and renew

			//Console.WriteLine("validating user: " + context.Principal.Identity.Name);
			//context.Principal

			//context.ReplacePrincipal(newprincipal);
			//context.ShouldRenew = true;
			Console.WriteLine("ValidateAsync:");
			context.ShouldRenew = true;
		}
	}
}
