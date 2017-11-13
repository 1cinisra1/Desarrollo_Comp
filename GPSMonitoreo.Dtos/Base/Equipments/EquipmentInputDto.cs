using GPSMonitoreo.Dtos.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Dtos.Base.Equipments
{
    public class EquipmentInputDto : CommonBaseInputDto<int>
    {
		[Rules("RequiredNumeric", "DBKeyExists('EQUIPOS_CATS')")]
		public short CategoryId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('MARCAS')")]
		//public byte marca { get; set; }
		public short BrandId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('EQUIPOS_GRUPOS')")]
		public byte GroupId { get; set; }

		[Rules("RequiredNumeric", "DBKeyExists('EQUIPOS_ESTADOS_OPERA')")]
		public byte OperationalStatusId { get; set; }

		[Rules("MaxLength(30)")]
		public string Plate { get; set; }

		////[Rules("Required")]
		[Rules("RequiredNumeric", "DBKeyExists('MODELOS')")]
		public short ModelId { get; set; }

		[Rules("RequiredNumeric")]
		public short ManufactureYear { get; set; }

		[Rules("Required", "MaxLength(10)")]
		public string AlternateId { get; set; }

		[Rules("Required", "MaxLength(30)")]
		public string SerialNumber { get; set; }


		[Rules("ValidateCollection")]
		public List<CapabilityListInputDto> Capabilities { get; set; }



	}
}
