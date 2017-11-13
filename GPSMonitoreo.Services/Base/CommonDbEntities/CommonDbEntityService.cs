using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.CommonDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using GPSMonitoreo.Services.Validation;
using GPSMonitoreo.Dtos;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Dtos.Resources;
using System.Collections.Specialized;

using System.Linq.Dynamic.Core;

namespace GPSMonitoreo.Services.Base.CommonDbEntities
{
    public class CommonDbEntityService : BaseService
	{
		EntitiesContext _dbContext;

		public CommonDbEntityService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetResult<CommonDbEntityInputDto<TId>>> GetByIdAsync<TId>(Type dbEntityType, TId id) where TId : struct, IEquatable<TId>
		{
			var result = new GetResult<CommonDbEntityInputDto<TId>>();

			var dbSet = _dbContext.Set(dbEntityType);

			var query = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable().AsNoTracking();

			var entity = await query.FirstOrDefaultAsync(x => x.ID.Equals(id));

			if (entity != null)
			{
				var dto = new CommonDbEntityInputDto<TId>();

				entity.AssignToDto(dto);

				result.Item = dto;

				result.SetSuccess();
			}

			return result;
		}

		public async Task<GetResult<CommonDbEntityCategoryInputDto<TId>>> GetCategoryByIdAsync<TId>(Type dbEntityType, TId id) where TId : struct, IEquatable<TId>
		{
			var result = new GetResult<CommonDbEntityCategoryInputDto<TId>>();

			var dbSet = _dbContext.Set(dbEntityType);

			var query = (IQueryable<ICommonEntityCategory<TId>>)dbSet.AsQueryable().AsNoTracking();

			var entity = await query.FirstOrDefaultAsync(x => x.ID.Equals(id));

			if (entity != null)
			{
				var dto = new CommonDbEntityCategoryInputDto<TId>();

				entity.AssignToDto(dto);
				//dto.ParentId = entity.PADRE_ID ?? default(TId);
				//dto.Order = entity.ORDENADOR;

				result.Item = dto;

				result.SetSuccess();
			}

			return result;
		}



		public async Task<GetStringResult> GetDescriptionAsync<TId>(Type dbEntityType, TId id) where TId : struct, IEquatable<TId>
		{
			var result = new GetStringResult();

			var dbSet = _dbContext.Set(dbEntityType);

			var query = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable().AsNoTracking();

			var entity = await query.FirstOrDefaultAsync(x => x.ID.Equals(id));

			if(entity != null)
			{
				result.SetSuccessResult(entity.DESCRIPCION_LARGA);
			}

			return result;
		}

		public async Task<GetListResult<List<CommonDbEntityListDto<TId>>>> GetListAsync<TId>(Type dbEntityType, CommonDbEntityFilterInputDto input = null)
		{
			var dbSet = _dbContext.Set(dbEntityType);

			var result = new GetListResult<List<CommonDbEntityListDto<TId>>>();

			List<CommonDbEntityListDto<TId>> items;

			var queryableDbSet = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable().AsNoTracking();

			var query = queryableDbSet;

			string orderBy = null;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.OrderBy != null)
				{
					orderBy = input.OrderBy.GetOrderBy(new StringDictionary
					{
						{ "Id", "ID" },
						{ "Description", "DESCRIPCION_LARGA" }
					});
				}

				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}

			if (input != null && input.Paging)
			{
				if (!string.IsNullOrEmpty(orderBy))
				{
					query = query.OrderBy(orderBy);
				}

				query = query.Select(x => new { x.ID })
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(queryableDbSet, j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			if (!string.IsNullOrEmpty(orderBy))
			{
				query = query.OrderBy(orderBy);
			}

			items = await query.Select(x => new CommonDbEntityListDto<TId>
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA
			}).ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}
		
		public async Task<GetListResult<List<CommonDbEntityCategoryListDto<TId>>>> GetCategoryListAsync<TId>(Type dbEntityType, CommonDbEntityFilterInputDto input = null) where TId : struct
		{
			var dbSet = _dbContext.Set(dbEntityType);

			var result = new GetListResult<List<CommonDbEntityCategoryListDto<TId>>>();

			var queryableDbSet = (IQueryable<ICommonEntityCategory<TId>>)dbSet.AsQueryable().AsNoTracking();

			var query = queryableDbSet;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}

			if (input != null && input.Paging)
			{
				query = query.OrderBy(x => x.DESCRIPCION_LARGA)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(queryableDbSet, j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}


			query = query.OrderBy(x => x.DESCRIPCION_LARGA);


			result.Items = await query.Select(x => new CommonDbEntityCategoryListDto<TId>
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA,
				Hierarchy = _dbContext.FN_HIERARCHY_PATH(dbEntityType.Name, x.ID)
			}).ToListAsync();

			result.SetSuccess();

			return result;
		}



		public async Task<GetListResult<List<CommonDbEntitySimpleListDto<TId>>>> GetSimpleListAsync<TId>(Type dbEntityType, CommonDbEntityFilterInputDto input = null)
		{
			var dbSet = _dbContext.Set(dbEntityType);

			var result = new GetListResult<List<CommonDbEntitySimpleListDto<TId>>>();

			List<CommonDbEntitySimpleListDto<TId>> items;

			var queryableDbSet = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable().AsNoTracking();

			var query = queryableDbSet;

			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
				}

				if(input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
				}
			}

			if (input != null && input.Paging)
			{
				query = query.OrderBy(x => x.DESCRIPCION_LARGA)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(queryableDbSet, j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderBy(x => x.DESCRIPCION_LARGA);

			items = await query.Select(x => new CommonDbEntitySimpleListDto<TId>
			{
				Id = x.ID,
				Description = x.DESCRIPCION_LARGA
			}).ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		public InputValidationResult Validate<TId>(CommonDbEntityInputDto<TId> input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

				}
			});

			return result;
		}

		public InputValidationResult Validate<TId>(Type dbEntityType, CommonDbEntityCategoryInputDto<TId> input) where TId : struct, IComparable<TId>, IEquatable<TId>
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if (input.ParentId.CompareTo(default(TId)) > 0 )
					{
						var dbSet = _dbContext.Set(dbEntityType);

						var query = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable().AsNoTracking();

						if (!query.Any(x => x.ID.Equals(input.ParentId)))
						{
							errors.AddError(nameof(input.ParentId), ValidationErrors.DBKeyExists);
						}
					}
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<TId>> CreateAsync<TId>(Type dbEntityType, CommonDbEntityInputDto<TId> input) where TId : struct
		{
			var result = new CreateUpdateResult<TId>();

			var dbSet = _dbContext.Set(dbEntityType);
			var entity = (ICommonEntity<TId>)dbSet.Create();

			input.AssignToEntity(entity);

			dbSet.Add(entity);
			
			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}


		//PENDIENTE: FALTA AQUI VERIFICAR QUE EL CODIGO POSTEADO EXISTA Y ADEMAS QUE NO SE PUEDA RELACIONAR ASI MISMO
		public async Task<CreateUpdateResult<TId>> CreateAsync<TId>(Type dbEntityType, CommonDbEntityCategoryInputDto<TId> input) where TId : struct, IEquatable<TId>
		{
			var result = new CreateUpdateResult<TId>();

			var dbSet = _dbContext.Set(dbEntityType);
			var entity = (ICommonEntityCategory<TId>)dbSet.Create();

			input.AssignToEntity(entity);
			//entity.PADRE_ID = input.ParentId.Equals(default(TId)) ? null : (TId?)input.ParentId;
			//entity.ORDENADOR = input.Order;

			dbSet.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<TId>> UpdateAsync<TId>(Type dbEntityType, CommonDbEntityInputDto<TId> input) where TId : struct, IEquatable<TId>
		{
			var result = new CreateUpdateResult<TId>();

			var dbSet = _dbContext.Set(dbEntityType);

			var query = (IQueryable<ICommonEntity<TId>>)dbSet.AsQueryable();

			var entity = await query.FirstOrDefaultAsync(x => x.ID.Equals(input.Id));

			if (entity != null)
			{
				input.AssignToEntity(entity);

				await _dbContext.SaveChangesAsync();

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

				result.Id = input.Id;
			}
			else
			{
			    result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			}

			return result;
		}

		public async Task<CreateUpdateResult<TId>> UpdateAsync<TId>(Type dbEntityType, CommonDbEntityCategoryInputDto<TId> input) where TId : struct, IEquatable<TId>
		{
			var result = new CreateUpdateResult<TId>();

			var dbSet = _dbContext.Set(dbEntityType);

			var query = (IQueryable<ICommonEntityCategory<TId>>)dbSet.AsQueryable();

			var entity = await query.FirstOrDefaultAsync(x => x.ID.Equals(input.Id));

			if (entity != null)
			{
				input.AssignToEntity(entity);

				await _dbContext.SaveChangesAsync();

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

				result.Id = input.Id;
			}
			else
			{
				result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			}

			return result;
		}
	}
}
