using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Users
{
    public class UserListDto: BaseListDto<int>
    {
		public string Username { get; set; }

        //public string password { get; set; }

        public string EntityDescription { get; set; }

		public string RoleDescription { get; set; }



		
    }
}
