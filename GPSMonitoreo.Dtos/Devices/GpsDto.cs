using GPSMonitoreo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Devices
{

	public class GpsDto : Dto<int>
	{

		public int? EquipmentId;
		public string EquipmentDescription;


		static GpsDto()
		{
			Console.WriteLine("static GpsDto()");
		}


		public static Expression<Func<GPS, GpsDto>> Selector = item => new GpsDto
		{
			Id = item.ID,
			Description = item.DESCRIPCION_LARGA,
			EquipmentId = item.EQUIPO_ID,
			EquipmentDescription = item.EQUIPO.DESCRIPCION_LARGA
		};

	}
}
