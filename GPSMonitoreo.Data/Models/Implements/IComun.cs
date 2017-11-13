using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSMonitoreo.Data.Models
{

	public interface ICommonEntityByte: ICommonEntity<byte>
	{
		//byte ID { get; set; }

	}

	public interface ICommonEntityInt16 : ICommonEntity<short>
	{
		//Int16 ID { get; set; }
	}

	public interface ICommonEntityInt32 : ICommonEntity<int>
	{
		//Int32 ID { get; set; }
	}

	public interface IComunCats<T> : IComunCatsInt16
	{
		
		T PADRE { get; set;}
	}

	public interface IComunCatsInt16 : ICommonEntityInt16
	{
		Int16? PADRE_ID { get; set; }
		string ORDENADOR { get; set; }
	}


	public interface ICommonEntity<T> : ICommonEntity
	{
		T ID { get; set; }
	}
	
	/*
	public interface ICommonEntityCategory<TId, TParent> : ICommonEntity<TId> where TId : struct where TParent : ICommonEntityCategory<TId, TParent>
	{
		Nullable<TId> PADRE_ID { get; set; }

		TParent PADRE { get; set; }

		string ORDENADOR { get; set; }

	}
	*/

	public interface ICommonEntityCategory<TId> : ICommonEntity<TId> where TId : struct
	{
		Nullable<TId> PADRE_ID { get; set; }

		ICommonEntityCategory<TId> PADRE { get; set; }

		string ORDENADOR { get; set; }

	}

	public interface ICommonEntity
	{
		string DESCRIPCION_LARGA { get; set; }
		string DESCRIPCION_MED { get; set; }
		string DESCRIPCION_CORTA { get; set; }
		string ABREVIACION { get; set; }
		string OBSERVACIONES { get; set;}
		byte ESTADO_ID { get; set; }
	}
}
