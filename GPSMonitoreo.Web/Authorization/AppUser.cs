using GPSMonitoreo.Core.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.Authorization
{
    public class AppUser : ClaimsPrincipal
	{
		public Role Role { get; set; }
		public int Id { get; set; }

		public AppUser(ClaimsPrincipal principal): base(principal)
		{
			var roleClaim = principal.FindFirst("role");
			if(roleClaim != null)
			{
				Role role;
				if(Enum.TryParse<Role>(roleClaim.Value, out role))
				{
					Role = role;
				}
			}

			//this.Role = Enums.Role.InternalMonitorist;

			var idClaim = principal.FindFirst("id");
			if(idClaim != null)
			{
				int id;
				if(Int32.TryParse(idClaim.Value, out id))
				{
					Id = id;
				}
			}
		}

		public AppUser(ClaimsIdentity identity) : base(identity)
		{

		}

		

		//public override void AddIdentities(IEnumerable<ClaimsIdentity> identities)
		//{
		//	base.AddIdentities(identities);
		//	Console.WriteLine("AddIdentities");
		//}

		//public override void AddIdentity(ClaimsIdentity identity)
		//{
		//	base.AddIdentity(identity);
		//	Console.WriteLine("AddIdentity");
		//}

		//protected override ClaimsIdentity CreateClaimsIdentity(BinaryReader reader)
		//{
		//	Console.WriteLine("CreateClaimsIdentity");
		//	return base.CreateClaimsIdentity(reader);
		//}

		//protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		//{
		//	Console.WriteLine("GetObjectData");
		//	base.GetObjectData(info, context);
		//}
	}
}
