
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Data.Models;

namespace GPSMonitoreo.Services.Extensions.DtoExtensions
{
    public static class DtoExtensions
    {
		/*
		public static void AssignToEntity(this ICommonBaseInputDto dto,  ICommonEntity entity)
		{
			entity.DESCRIPCION_LARGA = dto.DescriptionLong;
			entity.DESCRIPCION_MED = dto.DescriptionMedium;
			entity.DESCRIPCION_CORTA = dto.DescriptionShort;
			entity.ABREVIACION = dto.Abbreviation;
			entity.OBSERVACIONES = dto.Notes;
			entity.ESTADO_ID = dto.StatusId;
		}

		public static void AssignToEntity<TId>(this ICommonBaseInputDto<TId> dto, ICommonEntity<TId> entity)
		{
			entity.ID = dto.Id;
			(dto as ICommonBaseInputDto).AssignToEntity(entity);

			//dto.AssignToEntity(entity);
		}
		*/

		public static void AssignToEntity<TId>(this CommonBaseInputDto<TId> dto, ICommonEntity<TId> entity) where TId : struct
		{
			entity.ID = dto.Id;
			entity.DESCRIPCION_LARGA = dto.DescriptionLong;
			entity.DESCRIPCION_MED = dto.DescriptionMedium;
			entity.DESCRIPCION_CORTA = dto.DescriptionShort;
			entity.ABREVIACION = dto.Abbreviation;
			entity.OBSERVACIONES = dto.Notes;
			entity.ESTADO_ID = dto.StatusId;

			//dto.AssignToEntity(entity);
		}

		public static void AssignToEntity<TId>(this CommonBaseCategoryInputDto<TId> dto, ICommonEntityCategory<TId> entity) where TId : struct
		{
			(dto as CommonBaseInputDto<TId>).AssignToEntity(entity);
			entity.PADRE_ID = dto.ParentId.Equals(default(TId)) ? null : (TId?)dto.ParentId;
			entity.ORDENADOR = dto.Order;

			//dto.AssignToEntity(entity);
		}
	}
}
