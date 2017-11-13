//using AutoMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Geo
{
	//[MapsFrom(typeof(Data.Models.CERCAS))]
	public class GeoFenceDto
    {
		public int Id { get; set; }


		//[MapsFromProperty(typeof(Data.Models.CERCAS), "DESCRIPCION_LARGA")]
		public string Description { get; set; }
    }
}
