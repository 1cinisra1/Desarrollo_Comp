using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Collections;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;

namespace GPSMonitoreo.Data.QueryModels
{
	public class SimpleByte
	{
		public byte codigo { get; set; }
		public string descripcion { get; set; }
	}


	[Serializable]
	[DataContract]
	[Table(Name = "SP_CERCAS_CATEGORIAS")]
	public class SimpleInt16: ISerializable
	{
		
		public Int16 codigo { get; set; }

		[System.ComponentModel.DataAnnotations.Schema.Column("CTX_DESCRIPCION_LARGA")]
		[DataMember(Name = "CTX_DESCRIPCION_LARGA")]
		[System.Data.Linq.Mapping.Column(Name = "CTX_DESCRIPCION_LARGA")]
		public string descripcion { get; set; }



		[OnSerializing()]
		internal void OnSerializingMethod(StreamingContext context)
		{
			Console.WriteLine("This value went into the data file during serialization.");
		}

		[OnSerialized()]
		internal void OnSerializedMethod(StreamingContext context)
		{
			Console.WriteLine("This value was reset after serialization.");
		}

		[OnDeserializing()]
		internal void OnDeserializingMethod(StreamingContext context)
		{
			Console.WriteLine("This value was set during deserialization");
		}

		[OnDeserialized()]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			Console.WriteLine("This value was set after deserialization.");
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}
	}

	public class SimpleInt32
	{
		public Int32 codigo { get; set; }
		public string descripcion { get; set; }
	}
}
