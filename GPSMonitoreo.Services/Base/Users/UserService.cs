using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using System.Data.Entity;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.Users;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;

namespace GPSMonitoreo.Services.Base.Users
{
    public class UserService: BaseService
    {
		EntitiesContext _dbContext;

		public UserService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void PrepareEntity(USUARIOS entity,  UserInputDto input)
		{
			entity.ROL_ID = input.RoleId;
			entity.USUARIO = input.Username;
			entity.ENTIDAD_ID = input.EntityId;
			entity.CLAVE = input.Password;
		}

		public async Task<InputValidationResult> ValidateAsync(UserInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<int>> CreateOrUpdateAsync(UserInputDto input)
		{
			if(input.Id == 0)
			{
				return await CreateAsync(input);
			}
			else
			{
				return await UpdateAsync(input);
			}
		}


		public async Task<CreateUpdateResult<int>> CreateAsync(UserInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = new USUARIOS();

			PrepareEntity(entity, input);

			_dbContext.USUARIOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();

			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);

			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(UserInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			Utils.ObjectJsonDumper.Dump(input, 2);


			var entity = await _dbContext.USUARIOS.FirstOrDefaultAsync(x => x.ID == input.Id);

			if(entity != null)
			{
				PrepareEntity(entity, input);

				_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;

				await _dbContext.SaveChangesAsync();

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordUpdated);

				result.Id = entity.ID;
			}
			else
			{
				result.SetErrorByMessageName(ServicesErrorMessagesNames.ItemNotFound);
			}

			return result;
		}


		public async Task<BaseOutput> ValidateLoginAsync(string userName, string password)
		{
			var user = await _dbContext.USUARIOS.FirstOrDefaultAsync(x => x.USUARIO == userName && x.CLAVE == password);

			if (user == null)
			{
				return new BaseOutput
				{
					Status = Enums.OutputStatus.Error,
					ErrorMessage = "Usuario o contraseña incorrecta"
				};
			}
			else
			{
				return new BaseOutput
				{
					Status = Enums.OutputStatus.Ok
				};
			}
		}

		public async Task<GetResult<UserInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<UserInputDto>();

			var entity = await _dbContext.USUARIOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new UserInputDto
				{
					Id = entity.ID,
					EntityId = entity.ENTIDAD_ID ?? 0,
					EntityDescription = entity.ENTIDAD?.DESCRIPCION_LARGA,
					RoleId = entity.ROL_ID,
					Username = entity.USUARIO,
				};

				result.Item = dto;
				result.SetSuccess();
			}
			else
			{
				
			}

			return result;

		}

		public async Task<GetListResult<List<UserListDto>>> GetListAsync(UserFilterInputDto input = null)
		{
			var result = new GetListResult<List<UserListDto>>();

			var query = _dbContext.USUARIOS.AsQueryable().AsNoTracking();

			List<UserListDto> items;


			if (input != null)
			{
				if (!string.IsNullOrWhiteSpace(input.Username))
				{
					query = query.Where(x => x.USUARIO.ToLower().Contains(input.Username));
				}

				result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();
			}


			if (input != null && input.Paging)
			{
				query = query.OrderBy(x => x.ID)
					.Select(x => new { x.ID })
					.Skip(input.Page * input.PageSize).Take(input.PageSize)
					.Join(_dbContext.USUARIOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
				;
			}

			query = query.OrderBy(x => x.ID);

			items = await query.Select(x => new UserListDto
			{
				Id = x.ID,
				Username = x.USUARIO,
				EntityDescription = x.ENTIDAD.DESCRIPCION_LARGA,
				RoleDescription = x.ROLE.DESCRIPCION_LARGA
			}).ToListAsync();


			//if (input != null && input.Paging)
			//{
			//	var pagedSelect = query.OrderBy(x => x.ID)
			//		.Select(x => new { x.ID })
			//		.Skip(input.Page * input.PageSize).Take(input.PageSize)
			//		.Join(_dbContext.ROLES.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => new RoleListDto
			//		{
			//			Id = b.ID,
			//			Description = b.DESCRIPCION_LARGA
			//		})
			//		.OrderBy(x => x.Id)
			//		;

			//	items = await pagedSelect.ToListAsync();
			//}
			//else
			//{
			//	items = await query.Select(x => new RoleListDto
			//	{
			//		Id = x.ID,
			//		Description = x.DESCRIPCION_LARGA
			//	}).ToListAsync();
			//}



			result.Items = items;

			result.SetSuccess();

			return result;
		}

	}
}
