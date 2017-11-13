using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base;
using GPSMonitoreo.Dtos.Base.Geofences;
using GPSMonitoreo.Services.Extensions.DtoExtensions;
using GPSMonitoreo.Services.Resources;
using GPSMonitoreo.Services.Validation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using GPSMonitoreo.Libraries.Extensions.StringExtensions;
using GPSMonitoreo.Services.Extensions.EntityExtensions;
using GPSMonitoreo.Services.Utils;

namespace GPSMonitoreo.Services.Base.Geofences
{
    public class GeofenceService : BaseService
	{
		EntitiesContext _dbContext;

		public GeofenceService(EntitiesContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetResult<GeofenceInputDto>> GetByIdAsync(int id)
		{
			var result = new GetResult<GeofenceInputDto>();
			var entity = await _dbContext.CERCAS.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new GeofenceInputDto();
				entity.AssignToDto(dto);
				dto.AuxiliaryId = entity.AUXILIAR_ID;
				dto.LayerId = entity.CAPA_ID;
				dto.CategoryId = entity.CATEGORIA_ID;
				dto.ShapeId = entity.FORMA_ID;
				dto.RegionId = entity.REGION_ID;
				dto.Radius = entity.RADIO;
				dto.InRouteGeofenceId = entity.CERCA_ENRUTA_ID ?? 0;
				dto.InRouteGeofenceDescription = entity.CERCA_ENRUTA?.DESCRIPCION_LARGA ?? null;
				dto.ParentGeofenceId = entity.PADRE_ID ?? 0;
				dto.ParentGeofenceDescription = entity.PADRE?.DESCRIPCION_LARGA ?? null;

				var inRouteGeofenceEntity = entity.ENRUTA;

				if (inRouteGeofenceEntity != null)
				{
					dto.RoadSurfaceId = inRouteGeofenceEntity.CALZADA_TIPO_ID;
					dto.RoadSurfaceStateId = inRouteGeofenceEntity.CALZADA_ESTADO_ID;
					dto.RoadLanesGoing = inRouteGeofenceEntity.CARRILES_IDA;
					dto.RoadLanesReturning = inRouteGeofenceEntity.CARRILES_RETORNO;
					dto.CurveTypeId = inRouteGeofenceEntity.CURVA_TIPO_ID ?? 0;
					dto.CurveGradeId = inRouteGeofenceEntity.CURVA_GRADO_ID ?? 0;
					dto.Distance = inRouteGeofenceEntity.DISTANCIA;
					dto.HillGoingTypeId = inRouteGeofenceEntity.PENDIENTE_IDA_ID ?? 0;
					dto.HillGoingGradeId = inRouteGeofenceEntity.PENDIENTE_IDA_GRADO_ID ?? 0;
					dto.HillReturningTypeId = inRouteGeofenceEntity.PENDIENTE_RETORNO_ID ?? 0;
					dto.HillReturningGradeId = inRouteGeofenceEntity.PENDIENTE_RETORNO_GRADO_ID ?? 0;
					dto.CurvesQty = inRouteGeofenceEntity.CURVA_CANTIDAD;
					dto.RoadTrafficId = inRouteGeofenceEntity.TRAFICO_ID;
					dto.RoadShapeId = inRouteGeofenceEntity.TRAZADO_ID;
					dto.SpeedGoingMop = inRouteGeofenceEntity.VELOCIDAD_IDA_MOP;
					dto.SpeedGoingMopRest = inRouteGeofenceEntity.VELOCIDAD_IDA_MOPREST;
					dto.SpeedGoingCustomRest = inRouteGeofenceEntity.VELOCIDAD_IDA_EMPREST;
					dto.SpeedReturningMop = inRouteGeofenceEntity.VELOCIDAD_RETORNO_MOP;
					dto.SpeedReturningMopRest = inRouteGeofenceEntity.VELOCIDAD_RETORNO_MOPREST;
					dto.SpeedReturningCustomRest = inRouteGeofenceEntity.VELOCIDAD_RETORNO_EMPREST;
					dto.OptimalSpeed = inRouteGeofenceEntity.VELOCIDAD_OPTIMA;
					dto.OptimalTiming = inRouteGeofenceEntity.TIEMPO_OPTIMO;
				}

				switch (dto.ShapeId)
				{
					case 1: //circular
						dto.Coords = new List<GeofenceCoord>
						{
							new GeofenceCoord { lat = entity.LAT, lng = entity.LNG }
						};
						break;

					case 2: //rectangular
						dto.Coords = new List<GeofenceCoord>
						{
							new GeofenceCoord { lat = entity.VERTICE1_LAT, lng = entity.VERTICE1_LNG },
							new GeofenceCoord { lat = entity.VERTICE2_LAT, lng = entity.VERTICE2_LNG }
						};
						break;

					case 3: //poligono
							//dto.Coords = dbContext.CERCAS_PUNTOS.Where(item => item.CERCA_ID == entity.ID).OrderBy(item => item.NO).Select(item => new Coordenada() { lat = item.LAT, lng = item.LNG }).ToArray();
						dto.Coords = _dbContext.CERCAS_PUNTOS.Where(x => x.CERCA_ID == id).OrderBy(x => x.NO).Select(x => new GeofenceCoord { lat = x.LAT, lng = x.LNG }).ToList();
						break;

				}

				result.Item = dto;
				result.SetSuccess();
			}

			return result;
		}

		public async Task<GetResult<GeofenceDetailsDto>> GetDetailsByIdAsync(int id)
		{
			var result = new GetResult<GeofenceDetailsDto>();
			var entity = await _dbContext.CERCAS.AsNoTracking()
				.Include(x => x.REGION)
				.Include(x => x.CAPA)
				.Include(x => x.ENRUTA)
				.Include(x => x.ENRUTA.CALZADA)
				.Include(x => x.ENRUTA.CALZADA_ESTADO)
				.Include(x => x.ENRUTA.CURVA)
				.Include(x => x.ENRUTA.CURVA_GRADO)
				.Include(x => x.ENRUTA.PENDIENTE_IDA)
				.Include(x => x.ENRUTA.PENDIENTE_IDA_GRADO)
				.Include(x => x.ENRUTA.PENDIENTE_RETORNO)
				.Include(x => x.ENRUTA.PENDIENTE_RETORNO_GRADO)
				.Include(x => x.ENRUTA.TRAFICO)
				.Include(x => x.ENRUTA.TRAZADO)
				.FirstOrDefaultAsync(x => x.ID == id);

			if (entity != null)
			{
				var dto = new GeofenceDetailsDto
				{
					Id = entity.ID,
					Description = entity.DESCRIPCION_LARGA,
					Notes = entity.OBSERVACIONES,
					CategoryDescription = _dbContext.FN_HIERARCHY_PATH(typeof(CERCAS_CATS).Name, entity.CATEGORIA_ID),
					LayerId = entity.CAPA_ID,
					LayerDescription = entity.CAPA.DESCRIPCION_LARGA,
					StatusDescription = entity.ESTADO_ID == 1 ? "Activo" : "Inactivo",
					AuxiliaryId = entity.AUXILIAR_ID,
					RegionDescription = entity.REGION.DESCRIPCION_LARGA

				};

				var inRouteGeofenceEntity = entity.ENRUTA;

				if (inRouteGeofenceEntity != null)
				{
					dto.RoadSurfaceId = inRouteGeofenceEntity.CALZADA_TIPO_ID;
					dto.RoadSurfaceDescription = inRouteGeofenceEntity.CALZADA.DESCRIPCION_LARGA;
					dto.RoadSurfaceStateDescription = inRouteGeofenceEntity.CALZADA_ESTADO.DESCRIPCION_LARGA;
					dto.RoadLanesGoing = inRouteGeofenceEntity.CARRILES_IDA;
					dto.RoadLanesReturning = inRouteGeofenceEntity.CARRILES_RETORNO;
					dto.CurveTypeDescription = inRouteGeofenceEntity.CURVA?.DESCRIPCION_LARGA;
					dto.CurveGradeDescription = inRouteGeofenceEntity.CURVA_GRADO?.DESCRIPCION_LARGA;
					dto.Distance = inRouteGeofenceEntity.DISTANCIA;
					dto.HillGoingTypeDescription = inRouteGeofenceEntity.PENDIENTE_IDA?.DESCRIPCION_LARGA;
					dto.HillGoingGradeDescription = inRouteGeofenceEntity.PENDIENTE_IDA_GRADO?.DESCRIPCION_LARGA;
					dto.HillReturningTypeDescription = inRouteGeofenceEntity.PENDIENTE_RETORNO?.DESCRIPCION_LARGA;
					dto.HillReturningGradeDescription = inRouteGeofenceEntity.PENDIENTE_RETORNO_GRADO?.DESCRIPCION_LARGA;

					dto.CurvesQty = inRouteGeofenceEntity.CURVA_CANTIDAD;
					dto.RoadTrafficDescription = inRouteGeofenceEntity.TRAFICO.DESCRIPCION_LARGA;
					dto.RoadShapeId = inRouteGeofenceEntity.TRAZADO_ID;
					dto.RoadShapeDescription = inRouteGeofenceEntity.TRAZADO.DESCRIPCION_LARGA;
					dto.SpeedGoingMop = inRouteGeofenceEntity.VELOCIDAD_IDA_MOP;
					dto.SpeedGoingMopRest = inRouteGeofenceEntity.VELOCIDAD_IDA_MOPREST;
					dto.SpeedGoingCustomRest = inRouteGeofenceEntity.VELOCIDAD_IDA_EMPREST;
					dto.SpeedReturningMop = inRouteGeofenceEntity.VELOCIDAD_RETORNO_MOP;
					dto.SpeedReturningMopRest = inRouteGeofenceEntity.VELOCIDAD_RETORNO_MOPREST;
					dto.SpeedReturningCustomRest = inRouteGeofenceEntity.VELOCIDAD_RETORNO_EMPREST;
					dto.OptimalSpeed = inRouteGeofenceEntity.VELOCIDAD_OPTIMA;
					dto.OptimalTiming = inRouteGeofenceEntity.TIEMPO_OPTIMO;
				}

				result.Item = dto;
				result.SetSuccess();
			}

			return result;
		}

		public async Task<GetResult<GeofenceForMapDto>> GetCoordsByIdAsync(int id)
		{
			var result = new GetResult<GeofenceForMapDto>();
			var entity = await _dbContext.CERCAS.FirstOrDefaultAsync(x => x.ID == id);

			var dto = GeographicsUtils.GetGeofenceForMap(id, entity.DESCRIPCION_LARGA, entity.CAPA_ID, entity.FORMA_ID, _dbContext.FN_CERCA_COORDS(id));

			result.Item = dto;

			return result;
		}

		public InputValidationResult Validate(GeofenceInputDto input)
		{
			var result = InputValidator.Validate(input, new List<Action<InputValidationErrors>>()
			{
				(errors) =>
				{
					if (!input.AuxiliaryId.IsNullOrWhiteSpace())
					{
						if(_dbContext.CERCAS.Any(x => x.ID != input.Id && x.AUXILIAR_ID.ToLower() == input.AuxiliaryId.ToLower()))
						{
							errors.AddError("AuxiliaryId", "Código auxiliar ya existe");
						}
					}

					var coordsCount = input.Coords?.Count ?? 0;

					if (coordsCount == 0)
					{
						errors.AddError("Coords", "Se requieren los puntos de coordenadas");
					}
					else
					{
						switch (input.ShapeId)
						{
							case 1: //circular
								if (coordsCount != 1)
								{
									errors.AddError("Coords", "Circulo requiere de un punto");
								}
								break;

							case 2: //rectangular
								if (coordsCount != 2)
								{
									errors.AddError("Coords", "Rectangulo requiere de dos puntos");
								}
								break;

							case 3: //poligono
								if (coordsCount < 3)
								{
									errors.AddError("Coords", "Poligono requiere de tres o mas puntos");
								}
								break;
						}

					}

					if(input.LayerId > 1 && input.InRouteGeofenceId > 0)
					{
						var inRouteGeofence = _dbContext.CERCAS.Where(item => item.ID == input.InRouteGeofenceId).FirstOrDefault();

						if(inRouteGeofence == null)
						{
							errors.AddError("InRouteGeofenceId", "La cerca seleccionada no existe");
						}
						else
						{
							if(inRouteGeofence.CAPA_ID != 1) //es cerca en ruta
							{
								errors.AddError("InRouteGeofenceId", "Debe seleccionar una cerca que su capa sea EN RUTA");
							}
						}
					}

					CERCAS parentGeofence = null;

					switch(input.CategoryId)
					{
						case 20: //PARQUEO INGRESO LUGAR DE CARGA
						case 74: //BALANZA DE ENTRADA CARGA
						case 72: //PARQUEO INGRESO SITIO DE CARGA
						case 69: //SITIO DE CARGA
						case 21: //PARQUEO INGRESO PUNTO DE CARGA
						case 22: //PUNTO DE CARGA
						case 76: //BALANZA DE SALIDA CARGA
						case 23: //PARQUEO SALIDA LUGAR DE CARGA
							if(input.ParentGeofenceId == 0)
							{
								errors.AddError("ParentGeofenceId", "Debe indicar una cerca padre y que su categoria esté dentro de ENRUTA / CARGA");
							}
							else
							{
								parentGeofence = _dbContext.CERCAS.FirstOrDefault(item => item.ID == input.ParentGeofenceId);

								if(parentGeofence == null)
								{
									errors.AddError("ParentGeofenceId", "Cerca no existe");
								}
								else
								{
									switch(input.CategoryId)
									{
										case 20: //PARQUEO INGRESO LUGAR DE CARGA
										case 74: //BALANZA DE ENTRADA CARGA
										case 69: //SITIO DE CARGA
										case 76: //BALANZA DE SALIDA CARGA
										case 23: //PARQUEO SALIDA LUGAR DE CARGA
											if (parentGeofence.CATEGORIA_ID != 68)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / CARGA / LUGAR DE CARGA");
											}
											break;

										case 72: //PARQUEO INGRESO SITIO DE CARGA
											if (parentGeofence.CATEGORIA_ID != 69)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / CARGA / SITIO DE CARGA");
											}
											break;

										case 21: //PARQUEO INGRESO PUNTO DE CARGA
											if (parentGeofence.CATEGORIA_ID != 22)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / CARGA / PUNTO DE CARGA");
											}
											break;

										case 22: //PUNTO DE CARGA
											if (! new Int16[]{ 68, 69 }.Contains(parentGeofence.CATEGORIA_ID))
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / CARGA / LUGAR DE CARGA o SITIO DE CARGA");
											}
											break;
									}

								}

							}

							break;


						case 29: //PARQUEO INGRESO LUGAR DE DESCARGA
						case 75: //BALANZA DE ENTRADA DESCARGA
						case 73: //PARQUEO INGRESO SITIO DE DESCARGA
						case 71: //SITIO DE DESCARGA
						case 30: //PARQUEO INGRESO PUNTO DE DESCARGA
						case 31: //PUNTO DE DESCARGA
						case 77: //BALANZA DE SALIDA DESCARGA
						case 32: //PARQUEO SALIDA LUGAR DE DESCARGA
							if (input.ParentGeofenceId == 0)
							{
								errors.AddError("ParentGeofenceId", "Debe indicar una cerca padre y que su categoria esté dentro de ENRUTA / DESCARGA");
							}
							else
							{
								parentGeofence = _dbContext.CERCAS.FirstOrDefault(item => item.ID == input.ParentGeofenceId);

								if (parentGeofence == null)
								{
									errors.AddError("ParentGeofenceId", "Cerca no existe");
								}
								else
								{
									switch (input.CategoryId)
									{
										case 29: //PARQUEO INGRESO LUGAR DE DESCARGA
										case 75: //BALANZA DE ENTRADA DESCARGA
										case 71: //SITIO DE DESCARGA
										case 77: //BALANZA DE SALIDA DESCARGA
										case 32: //PARQUEO SALIDA LUGAR DE DESCARGA
											if (parentGeofence.CATEGORIA_ID != 70)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / DESCARGA / LUGAR DE DESCARGA");
											}
											
											break;

										case 73: //PARQUEO INGRESO SITIO DE DESCARGA
											if (parentGeofence.CATEGORIA_ID != 71)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / DESCARGA / SITIO DE DESCARGA");
											}
												
											break;

										case 30: //PARQUEO INGRESO PUNTO DE DESCARGA
											if (parentGeofence.CATEGORIA_ID != 31)
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / DESCARGA / PUNTO DE DESCARGA");
											}
											break;

										case 31: //PUNTO DE DESCARGA
											if (!new Int16[] { 70, 71 }.Contains(parentGeofence.CATEGORIA_ID))
											{
												errors.AddError("ParentGeofenceId", "La categoria de la cerca padre debe ser ENRUTA / DESCARGA / LUGAR DE DESCARGA o SITIO DE DESCARGA");
											}
											break;
									}

								}

							}

							break;

						default:
							if(input.CategoryId > 0 && input.ParentGeofenceId > 0)
							{
								errors.AddError("ParentGeofenceId", "No se puede asignar cerca padre a la cerca");
							}
							break;
					}
				}
			});

			return result;
			//GPSMonitoreo.Dtos.Validation.InputValidator
		}

		private void PrepareEntity(GeofenceInputDto input, CERCAS entity)
		{
			input.AssignToEntity(entity);
			entity.AUXILIAR_ID = input.AuxiliaryId;
			entity.CAPA_ID = input.LayerId;
			entity.CATEGORIA_ID = input.CategoryId;
			entity.FORMA_ID = input.ShapeId;
			entity.REGION_ID = input.RegionId;
			entity.PADRE_ID = input.ParentGeofenceId > 0 ? input.ParentGeofenceId : (int?)null;
			entity.CERCA_ENRUTA_ID = input.LayerId != 1 && input.InRouteGeofenceId > 0 ? input.InRouteGeofenceId : (int?)null;

		}

		private void PrepareInrouteGeofenceEntity(GeofenceInputDto input, CERCAS_ENRUTA entity)
		{
			entity.CALZADA_TIPO_ID = input.RoadSurfaceId;
			entity.CALZADA_ESTADO_ID = input.RoadSurfaceStateId;
			entity.CARRILES_IDA = input.RoadLanesGoing;
			entity.CARRILES_RETORNO = input.RoadLanesReturning;
			entity.CURVA_TIPO_ID = input.RoadShapeId == 2 ? input.CurveTypeId : (byte?)null;
			entity.CURVA_GRADO_ID = input.RoadShapeId == 2 ? input.CurveGradeId : (byte?)null;
			entity.DISTANCIA = input.Distance;
			entity.CURVA_CANTIDAD = input.RoadShapeId == 2 ? input.CurvesQty : (byte)0;
			entity.PENDIENTE_IDA_ID = input.HillGoingTypeId == 0 ? (byte?)null : input.HillGoingTypeId;
			entity.PENDIENTE_IDA_GRADO_ID = input.HillGoingGradeId == 0 ? (byte?)null : input.HillGoingGradeId;
			entity.PENDIENTE_RETORNO_ID = input.HillReturningTypeId == 0 ? (byte?)null : input.HillReturningTypeId;
			entity.PENDIENTE_RETORNO_GRADO_ID = input.HillReturningGradeId == 0 ? (byte?)null : input.HillReturningGradeId;
			entity.TRAFICO_ID = input.RoadTrafficId;
			entity.TRAZADO_ID = input.RoadShapeId;
			entity.VELOCIDAD_IDA_MOP = input.SpeedGoingMop;
			entity.VELOCIDAD_IDA_MOPREST = input.SpeedGoingMopRest;
			entity.VELOCIDAD_IDA_EMPREST = input.SpeedGoingCustomRest;
			entity.VELOCIDAD_RETORNO_MOP = input.SpeedReturningMop;
			entity.VELOCIDAD_RETORNO_MOPREST = input.SpeedReturningMopRest;
			entity.VELOCIDAD_RETORNO_EMPREST = input.SpeedReturningCustomRest;
			entity.VELOCIDAD_OPTIMA = input.OptimalSpeed;
			entity.TIEMPO_OPTIMO = input.OptimalTiming;
		}

		private void PrepareCoords(byte shapeId, double curveRadius, List<GeofenceCoord> coords, CERCAS entity)
		{
			double vertice1Lat = 0, vertice1Lng = 0, vertice2Lat = 0, vertice2Lng = 0;

			

			switch (shapeId)
			{
				case 1: //circular

					var lat = coords[0].lat;
					var lng = coords[0].lng;

					entity.LAT = lat;
					entity.LNG = lng;
					entity.RADIO = curveRadius;

					var offset = 360 / (2 * Math.PI * 6378100) * curveRadius;
					vertice1Lat = lat - offset;
					vertice1Lng = lng - offset;
					vertice2Lat = lat + offset;
					vertice2Lng = lng + offset;
					break;

				case 2: //rectangular
					vertice1Lat = coords[0].lat;
					vertice1Lng = coords[0].lng;
					vertice2Lat = coords[1].lat;
					vertice2Lng = coords[1].lng;
					break;

				case 3: //poligono
					var entityPuntos = new GPSMonitoreo.Data.Models.CERCAS_PUNTOS();
					byte x = 1;
					CERCAS_PUNTOS cercaPunto;
					foreach (var coord in coords)
					{
						cercaPunto = new GPSMonitoreo.Data.Models.CERCAS_PUNTOS() { LAT = coord.lat, LNG = coord.lng, NO = x++/*, CERCA_ID = id*/ };
						//if (entity.ID != 0)
						//	cercaPunto.CERCA_ID = id;


						entity.PUNTOS.Add(cercaPunto);
					}

					vertice1Lat = coords.Min(p => p.lat);
					vertice1Lng = coords.Min(p => p.lng);
					vertice2Lat = coords.Max(p => p.lat);
					vertice2Lng = coords.Max(p => p.lng);
					break;


			}


			entity.VERTICE1_LAT = vertice1Lat;
			entity.VERTICE1_LNG = vertice1Lng;
			entity.VERTICE2_LAT = vertice2Lat;
			entity.VERTICE2_LNG = vertice2Lng;
		}

		public Task<CreateUpdateResult<int>> CreateAsync2(GeofenceInputDto input)
		{
			return null;
		}

		public async Task<CreateUpdateResult<int>> CreateAsync(GeofenceInputDto input)
		{
			var result = new CreateUpdateResult<int>();
			var entity = new CERCAS();

			PrepareEntity(input, entity);

			if (input.LayerId == 1)
			{
				var inRouteGeofenceEntity = new CERCAS_ENRUTA();
				PrepareInrouteGeofenceEntity(input, inRouteGeofenceEntity);
				entity.ENRUTA = inRouteGeofenceEntity;
			}

			PrepareCoords(input.ShapeId, input.Radius, input.Coords, entity);

			_dbContext.CERCAS.Add(entity);

			var ret = await _dbContext.SaveChangesAsync();
			result.SetSuccessByMessageName(ServicesMessagesNames.RecordCreated);
			result.Id = entity.ID;


			return result;
		}

		public async Task<CreateUpdateResult<int>> UpdateAsync(GeofenceInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.CERCAS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<CERCAS_PUNTOS>(x => x.CERCA_ID == input.Id);

				var currentLayerId = entity.CAPA_ID;


				PrepareEntity(input, entity);
				//_dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;


				CERCAS_ENRUTA inRouteGeofenceEntity = null;

				if (currentLayerId == 1)
				{
					if (input.LayerId == 1)
					{
						inRouteGeofenceEntity = entity.ENRUTA;
						PrepareInrouteGeofenceEntity(input, inRouteGeofenceEntity);
					}
					else
					{
						_dbContext.Delete<CERCAS_ENRUTA>(x => x.CERCA_ID == input.Id);
						//entity.ENRUTA = null;
					}
				}
				else
				{
					if(input.LayerId == 1)
					{
						inRouteGeofenceEntity = new CERCAS_ENRUTA();
						PrepareInrouteGeofenceEntity(input, inRouteGeofenceEntity);
						entity.ENRUTA = inRouteGeofenceEntity;
					}
				}

				//PrepareCoords(input, entity);
				PrepareCoords(input.ShapeId, input.Radius, input.Coords, entity);



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

		public async Task<CreateUpdateResult<int>> UpdateCoordsAsync(GeofenceUpdateCoordsInputDto input)
		{
			var result = new CreateUpdateResult<int>();

			var entity = _dbContext.CERCAS.FirstOrDefault(x => x.ID == input.Id);

			if (entity != null)
			{
				_dbContext.Delete<CERCAS_PUNTOS>(x => x.CERCA_ID == input.Id);

				PrepareCoords(input.ShapeId, input.CurveRadius, input.Coords, entity);

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

		private List<CommonBaseWithAuxiliarSimpleListDto<int>> GetRoutesSectionsSegmentsList(JObject obj, string key)
		{
			var arr = obj[key] as JArray;

			var ret = new List<CommonBaseWithAuxiliarSimpleListDto<int>>();

			foreach (var item in arr)
			{
				ret.Add(new CommonBaseWithAuxiliarSimpleListDto<int>
				{
					Id = item.Value<int>("ID"),
					AuxiliaryId = item.Value<string>("AUXILIAR_ID"),
					Description = item.Value<string>("DESCRIPCION")
				});
			}

			return ret;
		}


		public async Task<GetListResult<List<GeofenceListDto>>> GetListAsync(GeofenceFilterInputDto input)
		{
			var result = new GetListResult<List<GeofenceListDto>>();

			List<GeofenceListDto> items = null;

			var query = _dbContext.CERCAS.AsQueryable();


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

				if (input.LayerIds != null && input.LayerIds.Count > 0)
				{
					query = query.Where(item => input.LayerIds.Contains(item.CAPA_ID));
				}


				if (input.RegionId > 0)
				{
					query = query.Where(item => item.REGION_ID == input.RegionId);
				}

				if (!string.IsNullOrWhiteSpace(input.Routes) || !string.IsNullOrWhiteSpace(input.Sections) || !string.IsNullOrWhiteSpace(input.Segments))
				{

					var query2 = _dbContext.CERCAS.GroupJoin(_dbContext.SEGMENTOS_CERCAS, cerca => cerca.ID, segmento => segmento.CERCA_ID, (a, b) => new { Cerca = a, Segmentos = b })
						.SelectMany(item => item.Segmentos.DefaultIfEmpty(), (a, b) => new { a.Cerca, SegmentoCercaRel = b })
						.GroupJoin(_dbContext.TRAMOS_SEGMENTOS, segmento => segmento.SegmentoCercaRel.SEGMENTO_ID, tramo => tramo.SEGMENTO_ID, (a, b) => new { a.Cerca, a.SegmentoCercaRel, Tramos = b })
						.SelectMany(item => item.Tramos.DefaultIfEmpty(), (a, b) => new { a.Cerca, a.SegmentoCercaRel, TramoSegmentoRel = b })
						.GroupJoin(_dbContext.RUTAS_TRAMOS, tramo => tramo.TramoSegmentoRel.TRAMO_ID, ruta => ruta.TRAMO_ID, (a, b) => new { a.Cerca, a.SegmentoCercaRel, a.TramoSegmentoRel, Rutas = b })
						.SelectMany(item => item.Rutas.DefaultIfEmpty(), (a, b) => new { a.Cerca, a.SegmentoCercaRel, a.TramoSegmentoRel, RutaTramoRel = b });


					if (!string.IsNullOrWhiteSpace(input.Routes))
					{
						switch (input.Routes)
						{
							case "with":
								query2 = query2.Where(item => item.RutaTramoRel != null);
								break;

							case "without":
								query2 = query2.Where(item => item.RutaTramoRel == null);
								break;

							default:
								query2 = query2.Where(item => item.RutaTramoRel.RUTA.DESCRIPCION_LARGA.ToLower().Contains(input.Routes.ToLower()));
								//query = query.Where(item => DBContext.SEGMENTOS_CERCAS.Any(
								//	segmentosCercas => segmentosCercas.CERCA_ID == item.ID && DBContext.TRAMOS_SEGMENTOS.Any(
								//		tramosSegmentos => tramosSegmentos.SEGMENTO_ID == segmentosCercas.SEGMENTO_ID && DBContext.RUTAS_TRAMOS.Any(
								//			rutasTramos => rutasTramos.TRAMO_ID == tramosSegmentos.TRAMO_ID && rutasTramos.RUTA.DESCRIPCION_LARGA.ToLower().Contains(searchModel.ruta.ToLower())
								//		)
								//	)
								//));

								//.Any(ruta => ruta.DESCRIPCION_LARGA.ToLower().Contains(searchModel.ruta.ToLower()) && ruta.ID == item.ID));
								break;
						}
					}

					if (!string.IsNullOrWhiteSpace(input.Sections))
					{
						switch (input.Sections)
						{
							case "with":
								query2 = query2.Where(item => item.TramoSegmentoRel != null);
								break;

							case "without":
								query2 = query2.Where(item => item.TramoSegmentoRel == null);
								break;

							default:
								query2 = query2.Where(item => item.TramoSegmentoRel.TRAMO.DESCRIPCION_LARGA.ToLower().Contains(input.Sections.ToLower()));
								break;
						}
					}


					if (!string.IsNullOrWhiteSpace(input.Segments))
					{
						switch (input.Segments)
						{
							case "with":
								query2 = query2.Where(item => item.SegmentoCercaRel != null);
								break;

							case "without":
								query2 = query2.Where(item => item.SegmentoCercaRel == null);
								break;

							default:
								query2 = query2.Where(item => item.SegmentoCercaRel.SEGMENTO.DESCRIPCION_LARGA.ToLower().Contains(input.Segments.ToLower()));
								break;
						}
					}

					query = query.Where(item => query2.Any(item2 => item2.Cerca.ID == item.ID));
				}


				if (input.SearchForMap)
				{

				}
				else
				{
					if (input.Paging)
					{
						result.RecordCount = input.RecordCount > 0 ? input.RecordCount : query.Count();

						query = query.OrderByDescending(x => x.ID)
							.Select(x => new { x.ID })
							.Skip(input.Page * input.PageSize).Take(input.PageSize)
							.Join(_dbContext.CERCAS.AsQueryable().AsNoTracking(), j => j.ID, j2 => j2.ID, (a, b) => b)
						;
					}

					query = query.OrderByDescending(x => x.ID);


					var list = await query.Select(x => new 
					{
						Id = x.ID,
						AuxiliaryId = x.AUXILIAR_ID,
						Description = x.DESCRIPCION_LARGA,
						CategoryDescription = _dbContext.FN_HIERARCHY_PATH(typeof(CERCAS_CATS).Name, x.CATEGORIA_ID),
						LayerDescription = x.CATEGORIA.DESCRIPCION_LARGA,
						RoutesSectionsSegments = _dbContext.FN_CERCA_SEGS_TRAMS_RUTS(x.ID)
					}).ToListAsync();

					

					items = list.Select(x => 
					{
						var obj = JObject.Parse(x.RoutesSectionsSegments);


						return new GeofenceListDto
						{
							Id = x.Id,
							AuxiliaryId = x.AuxiliaryId,
							Description = x.Description,
							LayerDescription = x.LayerDescription,
							CategoryDescription = x.CategoryDescription,
							Routes = GetRoutesSectionsSegmentsList(obj, "RUTAS"),
							Sections = GetRoutesSectionsSegmentsList(obj, "TRAMOS"),
							Segments = GetRoutesSectionsSegmentsList(obj, "SEGMENTOS")
						};
					}).ToList();
				}
			}

			result.Items = items;

			result.SetSuccess();

			return result;
		}
	}
}
