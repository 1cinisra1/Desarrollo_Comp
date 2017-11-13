using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Extensions.EntityExtensions
{
    public static class EntityExtensions
	{
		/*
		public static void AssignToDto(this ICommonEntity entity, ICommonBaseInputDto dto)
		{
			dto.DescriptionLong = entity.DESCRIPCION_LARGA;
			dto.DescriptionMedium = entity.DESCRIPCION_MED;
			dto.DescriptionShort  = entity.DESCRIPCION_CORTA;
			dto.Abbreviation = entity.ABREVIACION;
			dto.Notes = entity.OBSERVACIONES;
			dto.StatusId = entity.ESTADO_ID;
		}

		public static void AssignToDto<TId>(this ICommonEntity<TId> entity, ICommonBaseInputDto<TId> dto)
		{
			dto.Id = entity.ID;
			(entity as ICommonEntity).AssignToDto(dto);
		}
		*/

		public static void AssignToDto<TId>(this ICommonEntity<TId> entity, CommonBaseInputDto<TId> dto) where TId : struct
		{
			dto.Id = entity.ID;
			dto.DescriptionLong = entity.DESCRIPCION_LARGA;
			dto.DescriptionMedium = entity.DESCRIPCION_MED;
			dto.DescriptionShort = entity.DESCRIPCION_CORTA;
			dto.Abbreviation = entity.ABREVIACION;
			dto.Notes = entity.OBSERVACIONES;
			dto.StatusId = entity.ESTADO_ID;
		}

		public static void AssignToDto<TId>(this ICommonEntityCategory<TId> entity, CommonBaseCategoryInputDto<TId> dto) where TId : struct
		{
			(entity as ICommonEntity<TId>).AssignToDto(dto);

			dto.ParentId = entity.PADRE_ID ?? default(TId);
			dto.Order = entity.ORDENADOR;
		}
	}
}
