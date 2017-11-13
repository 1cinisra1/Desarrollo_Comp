using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Equipments
{
    public class EquipmentListDto : CommonBaseListDto<int>
    {
		public string AlternateId { get; set; }

		public string OperationalStatusDescription { get; set; }

		public string GroupDescription { get; set; }

		public string CategoryDescription { get; set; }

		public string Plate { get; set; }

		public string BrandDescription { get; set; }

		public string ModelDescription { get; set; }

		public short ManufactureYear { get; set; }

		public string SerialNumber { get; set; }

	}
}
