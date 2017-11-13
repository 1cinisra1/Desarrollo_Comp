using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using MVCHelpers.ViewModels.Validators;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Data.Models;

namespace GPSMonitoreo.Web.PostModels
{
    public class PostModelEdit<TId>: PostModelEditComun
    {
		public TId id { get; set; }


		public void FromEntity(ICommonEntity<TId> entity)
		{
			id = entity.ID;
			base.FromEntity(entity);
		}

		public void ToEntity(ICommonEntity<TId> entity)
		{
			entity.ID = id;
			base.ToEntity(entity);
		}


		public TId Save<T>(GPSMonitoreo.Data.Models.EntitiesContext dbContext) where T : class, GPSMonitoreo.Data.Models.ICommonEntity<TId>, new()
		{
			Utils.dump(this);

			var entity = new T()
			{
				ID = id,
				DESCRIPCION_LARGA = descripcion_larga,
				DESCRIPCION_MED = descripcion_mediana,
				DESCRIPCION_CORTA = descripcion_corta,
				ABREVIACION = abreviacion,
				OBSERVACIONES = observaciones,
				ESTADO_ID = estado
			};


			if (entity.ID.Equals(default(TId)))
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
			}
			else
			{
				dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}
			dbContext.SaveChanges();
			return entity.ID;
		}

		public static PostModelEdit<TR> Load<T, TR>(GPSMonitoreo.Data.Models.EntitiesContext dbContext, TR id) where TR : struct,
		  /*IComparable,
		  IComparable<TR>,
		  IConvertible,*/
		  IEquatable<TR>
		where T : class, GPSMonitoreo.Data.Models.ICommonEntity<TR> 
		{

			//var entity = dbContext.Set<T>().Where(item => item.ID == item.ID).FirstOrDefault();
			var entity = dbContext.Set<T>().Where(item => item.ID.Equals(id)).FirstOrDefault();

			if (entity != null)
			{
				var model = new PostModelEdit<TR>()
				{
					id = entity.ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					abreviacion = entity.ABREVIACION,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					estado = entity.ESTADO_ID,
					
				};
				return model;
			}

			return null;

		}


		public static PostModelEdit<byte> Load<T>(GPSMonitoreo.Data.Models.EntitiesContext dbContext, byte id) where T : class, GPSMonitoreo.Data.Models.ICommonEntityByte
		{


			return Load<T, byte>(dbContext, id);

			//var entity = dbContext.SEGMENTOS.Where(item => item.ID == id).FirstOrDefault();

			//var entity = dbContext.Set<T>().Where(item => item.ID == id).FirstOrDefault();


			//if (entity != null)
			//{
			//	var model = new PostModelEdit<byte>()
			//	{
			//		id = entity.ID,
			//		descripcion_larga = entity.DESCRIPCION_LARGA,
			//		abreviacion = entity.ABREVIACION,
			//		descripcion_corta = entity.DESCRIPCION_CORTA,
			//		descripcion_mediana = entity.DESCRIPCION_MED,
			//		estado = entity.ESTADO_ID
			//	};
			//	return model;
			//}

			//return null;
		}

		public static PostModelEdit<Int16> Load<T>(GPSMonitoreo.Data.Models.EntitiesContext dbContext, Int16 id) where T : class, GPSMonitoreo.Data.Models.ICommonEntityInt16
		{

			return Load<T, Int16>(dbContext, id);





			//return Load<T, int>(dbContext, id);

			//var entity = dbContext.SEGMENTOS.Where(item => item.ID == id).FirstOrDefault();

			//var entity = dbContext.Set<T>().Where(item => item.ID == id).FirstOrDefault();


			//if (entity != null)
			//{
			//	var model = new PostModelEdit<int>()
			//	{
			//		id = entity.ID,
			//		descripcion_larga = entity.DESCRIPCION_LARGA,
			//		abreviacion = entity.ABREVIACION,
			//		descripcion_corta = entity.DESCRIPCION_CORTA,
			//		descripcion_mediana = entity.DESCRIPCION_MED,
			//		estado = entity.ESTADO_ID
			//	};
			//	return model;
			//}

			//return null;
		}

		/// <summary>
		/// Método para soporte a tipos int32
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbContext"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static PostModelEdit<int> Load<T>(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id) where T : class, GPSMonitoreo.Data.Models.ICommonEntityInt32
		{

			return Load<T, int>(dbContext, id);





			//return Load<T, int>(dbContext, id);

			//var entity = dbContext.SEGMENTOS.Where(item => item.ID == id).FirstOrDefault();

			//var entity = dbContext.Set<T>().Where(item => item.ID == id).FirstOrDefault();


			//if (entity != null)
			//{
			//	var model = new PostModelEdit<int>()
			//	{
			//		id = entity.ID,
			//		descripcion_larga = entity.DESCRIPCION_LARGA,
			//		abreviacion = entity.ABREVIACION,
			//		descripcion_corta = entity.DESCRIPCION_CORTA,
			//		descripcion_mediana = entity.DESCRIPCION_MED,
			//		estado = entity.ESTADO_ID
			//	};
			//	return model;
			//}

			//return null;
		}

	}

	public static class PostModelEdit
	{
		public static ModelT Load<ModelT, EntityT>(GPSMonitoreo.Data.Models.EntitiesContext dbContext, int id, Action<ModelT, EntityT> assignOtherValues = null) where ModelT : PostModelEdit<int>, new() where EntityT : class, ICommonEntityInt32
		{
			var entity = dbContext.Set<EntityT>().Where(item => item.ID == id).FirstOrDefault();
			if (entity != null)
			{
				var model = new ModelT()
				{
					id = entity.ID,
					descripcion_larga = entity.DESCRIPCION_LARGA,
					descripcion_mediana = entity.DESCRIPCION_MED,
					descripcion_corta = entity.DESCRIPCION_CORTA,
					abreviacion = entity.ABREVIACION,
					estado = entity.ESTADO_ID
					//model.observaciones = entity.OBSERVACIONES //pendiente
				};

				assignOtherValues?.Invoke(model, entity);


				return model;
			};

			return null;

		}

	}

	//public static class Extensions
	//{

	//	public static TR Save<T, TR>(this PostModelEdit<TR> postModel, GPSMonitoreo.Data.Models.EntitiesContext dbContext) where TR : struct,
	//	  /*IComparable,
	//	  IComparable<TR>,
	//	  IConvertible,*/
	//	  IEquatable<TR>
	//	where T : class, GPSMonitoreo.Data.Models.IComun<TR>, new()
	//	{


	//		Utils.dump(postModel);

	//		var entity = new T()
	//		{
	//			ID = postModel.id,
	//			DESCRIPCION_LARGA = postModel.descripcion_larga,
	//			ABREVIACION = postModel.abreviacion,
	//			DESCRIPCION_CORTA = postModel.descripcion_corta,
	//			DESCRIPCION_MED = postModel.descripcion_mediana,
	//			ESTADO_ID = postModel.estado
	//		};


	//		if (entity.ID.Equals(0))
	//		{
	//			dbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
	//		}
	//		else
	//		{
	//			dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
	//		}
	//		dbContext.SaveChanges();
	//		return entity.ID;
	//	}

	//	public static byte Save<T>(this PostModelEdit<byte> postModel, GPSMonitoreo.Data.Models.EntitiesContext dbContext) where T : class, GPSMonitoreo.Data.Models.IComun<byte>, new()
	//	{

	//		postModel.Save<IComun<byte>, byte>(dbContext);


	//		return Save<GPSMonitoreo.Data.Models.IComunByte, byte>(dbContext);



	//		//Utils.dump(postModel);

	//		//var entity = new T()
	//		//{
	//		//	ID = postModel.id,
	//		//	DESCRIPCION_LARGA = postModel.descripcion_larga,
	//		//	ABREVIACION = postModel.abreviacion,
	//		//	DESCRIPCION_CORTA = postModel.descripcion_corta,
	//		//	DESCRIPCION_MED = postModel.descripcion_mediana,
	//		//	ESTADO_ID = postModel.estado
	//		//};

	//		//if (entity.ID == 0)
	//		//{
	//		//	dbContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
	//		//}
	//		//else
	//		//{
	//		//	dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
	//		//}
	//		//dbContext.SaveChanges();
	//		//return entity.ID;
	//	}
	//}


}
