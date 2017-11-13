using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base
{


	public interface ICommonBaseInputDto<TId>: ICommonBaseInputDto
    {
		TId Id { get; set; }
    }


	public interface ICommonBaseInputDto
	{
		string DescriptionLong { get; set; }


		string DescriptionMedium { get; set; }


		string DescriptionShort { get; set; }


		string Abbreviation { get; set; }

		byte StatusId { get; set; }


		string Notes { get; set; }
	}
}
