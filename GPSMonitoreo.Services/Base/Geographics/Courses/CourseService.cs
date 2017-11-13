using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//using GPSMonitoreo.Dtos;
using GPSMonitoreo.Dtos.Base.Geographics.Courses;
using GPSMonitoreo.Dtos.Base.Geographics.Routes;
using GPSMonitoreo.Services.Validation;
using GPSMonitoreo.Data.Models;

using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;

using System.Data.Entity;
using GPSMonitoreo.Services.Resources;

namespace GPSMonitoreo.Services.Base.Geographics.Courses
{
    public class CourseService: BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public CourseService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private TRAYECTOS PrepareEntity(CourseInputDto input)
		{
			var entity = new TRAYECTOS();
			input.AssignToEntity(entity);
			entity.ALTERNO_ID = input.AlternateId;

			byte order = 1;

			foreach(var route in input.Routes)
			{
				entity.TRAYECTO_RUTAS.Add(new TRAYECTOS_RUTAS() { TRAYECTO_ID = input.Id, RUTA_ID = route.Id, ORDEN = order++ });
			}

			return entity;
		}


		public async Task<CreateUpdateResult<int>> CreateAsync(CourseInputDto input)
		{
			var result = new CreateUpdateResult<int>();
			var entity = PrepareEntity(input);

			_dbContext.TRAYECTOS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			
			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(CourseInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			if(_dbContext.TRAYECTOS.Any(x => x.ID == input.Id))
			{
				_dbContext.Delete<TRAYECTOS_RUTAS>(x => x.TRAYECTO_ID == input.Id);

				var entity = PrepareEntity(input);

				_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
				_dbContext.TRAYECTOS_RUTAS.AddRange(entity.TRAYECTO_RUTAS);

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


		public InputValidationResult Validate(CourseInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if(!errors.ContainsKey("Routes") && input.Routes != null)
					{

						var routesErrors = new InputCollectionValidationError();


						var x = 0;
						int matchIndex;

						foreach(var route in input.Routes)
						{
							if(x > 0)
							{
								matchIndex = input.Routes.FindLastIndex(x - 1, r => r.Id == route.Id);

								if(matchIndex > -1)
								{
									routesErrors.AddItemError(x, Resources.ServicesErrorMessages.CollectionItemDuplicated + ": " + route.Id);
								}
							}

							x++;
						}

						if(routesErrors.ItemsErrors.Count > 0)
						{
							routesErrors.Error = Dtos.Resources.ValidationErrors.ValidateCollection;
							errors.AddCollectionError("Routes", routesErrors);
						}

						//routesErrors.AddItemError()


						//errors.Add()

					}

					Console.WriteLine("additional validator:");
					Utils.ObjectJsonDumper.Dump(errors, 10);
					//Resources.ServicesErrorMessages.CollectionItemDuplicated
				}
			});

			return result;
			//GPSMonitoreo.Dtos.Validation.InputValidator
		}


		public async Task<GetResult<CourseInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<CourseInputDto>();
			var entity = await _dbContext.TRAYECTOS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new CourseInputDto();
				entity.AssignToDto(dto);
				dto.AlternateId = entity.ALTERNO_ID;
				dto.Routes = _dbContext.TRAYECTOS_RUTAS.Where(x => x.TRAYECTO_ID == id).OrderBy(x => x.ORDEN).Select(x => new RouteListItemInputDto
				{
					Id = x.RUTA_ID,
					Description = x.RUTA.DESCRIPCION_LARGA
				}).ToList();

				result.Item = dto;
				result.SetSuccess();
			}




			return result;
		}

		public async Task<GetListResult<List<CourseListDto>>> GetListAsync(CourseFilterInputDto input)
		{
			var result = new GetListResult<List<CourseListDto>>();

			var query = _dbContext.TRAYECTOS.AsQueryable().AsNoTracking();

			if (!string.IsNullOrWhiteSpace(input.Description))
			{
				query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description));
			}

			if (!string.IsNullOrWhiteSpace(input.AlternateId))
			{
				query = query.Where(item => item.ALTERNO_ID == input.AlternateId);
			}

			result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();

			var query_select = query.OrderByDescending(x => x.ID)
				.Select(x => new { x.ID })
				.Skip(input.Page * input.PageSize).Take(input.PageSize)
				.Join(_dbContext.TRAYECTOS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => new CourseListDto
				{
					Id = b.ID,
					AlternateId = b.ALTERNO_ID,
					Description = b.DESCRIPCION_LARGA
				})
				.OrderByDescending(x => x.Id)
				;

			var items = await query_select.ToListAsync();

			result.Items = items;

			result.SetSuccess();

			return result;
		}
	}
}
