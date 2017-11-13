using GPSMonitoreo.Dtos.Base.Geographics.Routes;
using GPSMonitoreo.Dtos.Validation;
using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Users
{
    public class UserInputDto : BaseInputDto<int>
    {

		[Rules("ContinueIf('this.EntityId > 0')", "DBKeyExists('ENTIDADES')")]
		public int EntityId { get; set; }

		public string EntityDescription { get; set; }

		[Rules("Required")]
		public string Username { get; set; }

		[Rules("Required")]
		public string Password { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('ROLES')")]
		public Int16 RoleId { get; set; }

	}
}
