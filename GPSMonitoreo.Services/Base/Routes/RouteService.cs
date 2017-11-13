using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Routes;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using System.Data.Entity;
using GPSMonitoreo.Dtos.Base.Geographics.Routes;
using GPSMonitoreo.Libraries.Extensions.StringExtensions;
using GPSMonitoreo.Dtos.Base.Geofences;

using GPSMonitoreo.Data.Extensions;
using Newtonsoft.Json.Linq;
using GPSMonitoreo.Services.Utils;

namespace GPSMonitoreo.Services.Base.Routes
{
    public class RouteService : BaseService
	{

		GPSMonitoreo.Data.Models.EntitiesContext _dbContext;


		public RouteService(GPSMonitoreo.Data.Models.EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		private void PrepareEntity(RouteInputDto input, RUTAS entity)
		{
			input.AssignToEntity(entity);
			entity.AUXILIAR_ID = input.AuxiliaryId;
			entity.CATEGORIA_ID = input.CategoryId;
		}

		private void AddRelations(RouteInputDto input, RUTAS entity)
		{
			var order = (short)1;

			foreach (var section in input.Sections)
			{
				entity.RUTA_TRAMOS.Add(new RUTAS_TRAMOS { TRAMO_ID = section.Id, ORDEN = order++ });
			}

		}

		public InputValidationResult Validate(RouteInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{

					if (!input.AuxiliaryId.IsNullOrWhiteSpace())
					{
						if(_dbContext.RUTAS.Any(x => x.ID != input.Id && x.AUXILIAR_ID.ToLower() == input.AuxiliaryId.ToLower()))
						{
							errors.AddError("AuxiliaryId", "Código auxiliar ya existe");
						}
					}

					if(!errors.ContainsKey("Sections") && input.Sections != null)
					{
						var routesErrors = new InputCollectionValidationError();

						var x = 0;
						int matchIndex;

						foreach(var section in input.Sections)
						{
							if(x > 0)
							{
								matchIndex = input.Sections.FindLastIndex(x - 1, r => r.Id == section.Id);

								if(matchIndex > -1)
								{
									routesErrors.AddItemError(x, Resources.ServicesErrorMessages.CollectionItemDuplicated + ": " + section.Id);
								}
							}

							x++;
						}

						if(routesErrors.ItemsErrors.Count > 0)
						{
							routesErrors.Error = Dtos.Resources.ValidationErrors.ValidateCollection;
							errors.AddCollectionError("Sections", routesErrors);
						}
					}
				}
			});

			return result;
		}

		public async Task<CreateUpdateResult<int>> CreateAsync(RouteInputDto input)
		{
			var result = new CreateUpdateResult<int>();
			var entity = new RUTAS();

			PrepareEntity(input, entity);
			AddRelations(input, entity);

			_dbContext.RUTAS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;

			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(RouteInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.RUTAS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<RUTAS_TRAMOS>(x => x.RUTA_ID == input.Id);

				PrepareEntity(input, entity);
				AddRelations(input, entity);


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

		public async Task<GetResult<RouteInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<RouteInputDto>();
			var entity = await _dbContext.RUTAS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new RouteInputDto();
				entity.AssignToDto(dto);
				dto.AuxiliaryId = entity.AUXILIAR_ID;
				dto.CategoryId = entity.CATEGORIA_ID;

				
				dto.Sections = _dbContext.RUTAS_TRAMOS.Where(x => x.RUTA_ID == id).OrderBy(x => x.ORDEN).Select(x => new CommonBaseWithAuxiliarListInputDto<int>
				{
					Id = x.TRAMO_ID,
					AuxiliaryId = x.TRAMO.AUXILIAR_ID,
					Description = x.TRAMO.DESCRIPCION_LARGA
				}).ToList();

				result.Item = dto;
				result.SetSuccess();
			}




			return result;
		}


		public async Task<InputValidationResult> ValidateTemplateAsync(RouteTemplateInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if(input.Direction == 2) //reversed, need to check new descriptions
					{
						if(!errors.ContainsKey("Sections"))
						{
							var itemsError = new System.Text.StringBuilder();

							Action<IEnumerable<CommonBaseListInputDto<int>>, int> checkItems = null;

							int rowNumber = 0;


							checkItems = (items, level) =>
							{
								foreach(var item in items)
								{


									rowNumber++;


									//Console.WriteLine("checking item: " + rowNumber + ":" + item.Description);

									if(item.Description.Length > 52)
									{
										itemsError.Append("- Fila #" + rowNumber + ", descripción excede el límite de 52 carácteres (" + item.Description + ")<br/>");
									}

									switch(level)
									{
										case 0:
											RouteTemplateRouteSectionInputDto section = (RouteTemplateRouteSectionInputDto)item;
											if(section.Segments != null && section.Segments.Count > 0)
											{
												checkItems(section.Segments, level + 1);
											}
											break;

										case 1:
											RouteTemplateRouteRouteSegmentInputDto segment = (RouteTemplateRouteRouteSegmentInputDto)item;
											foreach(var geofence in segment.Geofences)
											{
												rowNumber++;
											}

											break;
									}
								}
							};

							checkItems(input.Sections, 0);

							if(itemsError.Length > 0)
							{
								errors.AddError("Sections", itemsError.ToString());

							}
						}
					}
				}
			});

			return result;
			//GPSMonitoreo.Dtos.Validation.InputValidator
		}


		private short dummy(short order)
		{
			return order;
		}


		private int? FindMatchedReversedRouteSectionId(RouteTemplateRouteSectionInputDto routeSectionInput)
		{

			//var query = _dbContext.TRAMOS.AsNoTracking().AsQueryable()

			//	.Where(
			//		x => x.TRAMO_SEGMENTOS.Count() == routeSectionInput.Segments.Count

			//	);

			//short segmentOrder = 1; 

			//foreach(var routeSegmentInput in routeSectionInput.Segments)
			//{
			//	query = query.Where(x => x.TRAMO_SEGMENTOS.Any(ss => ss.ORDEN == segmentOrder && ss.SEGMENTO.SEGMENTO_CERCAS.Count() == routeSegmentInput.Geofences.Count));

			//	segmentOrder++;
			//}

			var query = _dbContext.TRAMOS.AsNoTracking().AsQueryable()

				.Where(
					x => x.TRAMO_SEGMENTOS.Count() == routeSectionInput.Segments.Count

				);

			short segmentOrder = 1;

			foreach (var routeSegmentInput in routeSectionInput.Segments)
			{
				
				//copy is important because of referenced varible on tree expression
				short segmentOrderCopy = segmentOrder;

				//query = query.Where(x => x.TRAMO_SEGMENTOS.Any(ss => ss.ORDEN == segmentOrder && ss.SEGMENTO.SEGMENTO_CERCAS.Count() == routeSegmentInput.Geofences.Count));
				query = query.Where(x => _dbContext.TRAMOS_SEGMENTOS.Any(ss => ss.TRAMO_ID == x.ID && ss.ORDEN == segmentOrderCopy && ss.SEGMENTO.SEGMENTO_CERCAS.Count() == routeSegmentInput.Geofences.Count));

				segmentOrder++;
			}

			//Console.WriteLine("count count count count count count");
			//var count = query.Count();
			//Console.WriteLine(count);
			//Console.WriteLine("--------------------count count count count count count - end");


			var candidates = query.Select(x => new
			{
				Id = x.ID,
				Segments = x.TRAMO_SEGMENTOS.OrderBy(ss => ss.ORDEN).Select(ss => new
				{
					Id = ss.SEGMENTO_ID,
					Geofences = ss.SEGMENTO.SEGMENTO_CERCAS.OrderBy(sc => sc.ORDEN).Select(sc => new
					{
						Id = sc.CERCA_ID
					}).ToList()
				}).ToList()
			}).ToList();



			//Utils.ObjectJsonDumper.Dump(candidates, 5);

			int segmentIndex;
			int geofenceIndex;

			var candidatesCount = candidates.Count;

			//Console.WriteLine("candidatesCount: " + candidatesCount);

			foreach (var candidateSection in candidates)
			{
				//Console.WriteLine("looping section");
				if (candidateSection.Segments.Count == routeSectionInput.Segments.Count)
				{
					//Console.WriteLine("looping section: same count");
					segmentIndex = 0;
					
					foreach (var candidateSegment in candidateSection.Segments)
					{
						//Console.WriteLine("looping candidateSegment");

						var segmentInput = routeSectionInput.Segments[segmentIndex];

						//Console.WriteLine("looping candidateSegment - same id: " + (candidateSegment.Id == segmentInput.Id));

						if (/*candidateSegment.Id == segmentInput.Id &&*/ candidateSegment.Geofences.Count == segmentInput.Geofences.Count)
						{
							//Console.WriteLine("looping candidateSegment: same id and same count");

							geofenceIndex = 0;

							foreach(var candidateGeofence in candidateSegment.Geofences)
							{
								var geofenceInput = segmentInput.Geofences[geofenceIndex];
								

								if(candidateGeofence.Id == geofenceInput.Id)
								{
									//Console.WriteLine("looping geofence: same");

								}
								else
								{
									//Console.WriteLine("looping geofence: not same");
									goto outerloop;
								}

								geofenceIndex++;
							}

							
						}
						else
						{
							goto outerloop;
						}

						segmentIndex++;
					}

					return candidateSection.Id;
				}
				//else
				//{
				//	continue;
				//}
				outerloop:;
			}


			return null;
		}


		public async Task<CreateUpdateRouteTemplateResult> CreateFromTemplateAsync(RouteTemplateInputDto input)
		{
			var result = new CreateUpdateRouteTemplateResult();


			var routeEntity = new RUTAS();

			input.AssignToEntity(routeEntity);

			routeEntity.ES_RETORNO = input.RouteType == 2 ? true : false;

			routeEntity.CATEGORIA_ID = input.CategoryId;

			_dbContext.RUTAS.Add(routeEntity);

			var sectionOrder = (short)1;

			if(input.Direction == 1) //same direction
			{
				foreach (var section in input.Sections)
				{
					routeEntity.RUTA_TRAMOS.Add(new RUTAS_TRAMOS
					{
						TRAMO_ID = section.Id,
						ORDEN = sectionOrder++
					});
				}

				result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			}
			else //reversed direction
			{

				var skippedSections = new List<RouteTemplateRouteSectionInputDto>();


				int? matchedRouteSectionId;

				foreach (var section in input.Sections)
				{
					matchedRouteSectionId = FindMatchedReversedRouteSectionId(section);
					Console.WriteLine("-------------matchedRouteSectionId: " + matchedRouteSectionId);

					if(matchedRouteSectionId != null)
					{
						skippedSections.Add(section);

						routeEntity.RUTA_TRAMOS.Add(new RUTAS_TRAMOS
						{
							TRAMO_ID = matchedRouteSectionId.Value,
							ORDEN = sectionOrder++
						});

						
					}
					else
					{
						var sectionEntity = new TRAMOS()
						{
							DESCRIPCION_LARGA = section.Description,
							CATEGORIA_ID = 1,
							ESTADO_ID = 1
						};

						_dbContext.TRAMOS.Add(sectionEntity);

						routeEntity.RUTA_TRAMOS.Add(new RUTAS_TRAMOS()
						{
							//TRAMO_ID = sectionEntity.ID,
							RUTA = routeEntity,
							TRAMO = sectionEntity,
							ORDEN = sectionOrder++,
						});

						var segmentOrder = (short)1;

						foreach (var segment in section.Segments)
						{
							var segmentEntity = new SEGMENTOS()
							{
								CATEGORIA_ID = 1,
								DESCRIPCION_LARGA = segment.Description,
								ESTADO_ID = 1
							};

							_dbContext.SEGMENTOS.Add(segmentEntity);

							sectionEntity.TRAMO_SEGMENTOS.Add(new TRAMOS_SEGMENTOS
							{
								TRAMO = sectionEntity,
								SEGMENTO = segmentEntity,
								ORDEN = segmentOrder++
							});


							var geofenceOrder = (short)1;

							foreach (var geofence in segment.Geofences)
							{
								segmentEntity.SEGMENTO_CERCAS.Add(new SEGMENTOS_CERCAS
								{
									SEGMENTO = segmentEntity,
									CERCA_ID = geofence.Id,
									ORDEN = geofenceOrder++
								});
							}

						}
					}

					//return null;
				}



				if(skippedSections.Count == 0)
				{
					result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
				}
				else
				{
					result.SetSuccessAndWarningByMessageName(ServicesMessagesNames.RecordCreated, ServicesMessagesNames.RoutesTemplateCreatedSkippedSections);
					result.SkippedSections = skippedSections;
				}

				
				
				//result.Id = 0;


				//return null;

				//foreach (var section in input.Sections)
				//{
					
				//}
			}


			await _dbContext.SaveChangesAsync();

			result.Id = routeEntity.ID;







			return result;
		}

		public async Task<GetListResult<List<RouteListDto>>> GetListAsync(RouteFilterInputDto input)
		{
			var result = new GetListResult<List<RouteListDto>>();

			List<RouteListDto> items = null;

			var query = _dbContext.RUTAS.AsQueryable();


			if (input != null)
			{

				if (!string.IsNullOrWhiteSpace(input.AuxiliaryId))
				{
					query = query.Where(item => item.AUXILIAR_ID.ToLower().Contains(input.AuxiliaryId.ToLower()));
				}

				if (!string.IsNullOrWhiteSpace(input.Description))
				{
					query = query.Where(item => item.DESCRIPCION_LARGA.ToLower().Contains(input.Description.ToLower()));
				}

				

				if (input.Paging)
				{
					result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();

					query = query.OrderByDescending(x => x.ID)
						.Select(x => new { x.ID })
						.Skip(input.Page * input.PageSize).Take(input.PageSize)
						.Join(_dbContext.RUTAS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
					;
				}

				query = query.OrderByDescending(x => x.ID);


				items = await query.Select(x => new RouteListDto
				{
					Id = x.ID,
					AuxiliaryId = x.AUXILIAR_ID,
					Description = x.DESCRIPCION_LARGA,
					CategoryDescription = _dbContext.FN_HIERARCHY_PATH(typeof(RUTAS_CATS).Name, x.CATEGORIA_ID),
				}).ToListAsync();
			}

			result.Items = items;

			result.SetSuccess();

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_RUTA_CERCAS", id);

			return result;
		}

		public GetListResult<List<GeofenceForMapDto>> GetAllOtherGeofencesForMap(int id)
		{
			var result = new GetListResult<List<GeofenceForMapDto>>();

			result.Items = GeographicsUtils.GetAllGeofencesForMap(_dbContext, "SP_RUTA_CERCAS_OTRAS", id);

			return result;
		}
	}
}
