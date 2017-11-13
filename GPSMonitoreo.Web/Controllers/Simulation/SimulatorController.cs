//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using GPSMonitoreo.Web.Extensions;
//using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
//using GPSMonitoreo.Web.PostModels.Simulacion;
//using System.Text;

//using GPSMonitoreoServer.Reports.Enums;
//using GPSMonitoreoServer.Reports.Reports;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Net.Sockets;
//using System.Net;
//using GPSMonitoreo.Data.Models;

////using GPSMonitoreo.Dtos.Devices;
//using System.Data.Entity;
////using GPSMonitoreo.Dtos.Geo;
////using GPSMonitoreo.Dtos;
//using GPSMonitoreo.Data.Extensions;
//using GPSMonitoreo.Core.Authorization;
//using GPSMonitoreo.Services.Base.CommonDbEntities;
//using GPSMonitoreo.Dtos.Base.CommonDbEntities;
//using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;

//namespace GPSMonitoreo.Web.Controllers.Simulation
//{
//	public class SimulatorController : BaseController
//	{

//		private CommonDbEntityService _commonDbEntityService;

//		public SimulatorController(CommonDbEntityService commonDbEntityService)
//		{
//			_commonDbEntityService = commonDbEntityService;
//		}


//		public IActionResult Index()
//		{
//			var viewModel = new ViewModels.AppLayout()
//			{
//				Title = "SIMULACION :: SIMULADOR"

//			};


//			//var query = DBContext.GPS.

//			//var gps = DBContext.GPS.Select(item => new
//			//{
//			//	value = item.ID,
//			//	label = item.IMEI,
//			//	equipo = item.EQUIPO?.DESCRIPCION_LARGA
//			//})


//			var gpsList = DBContext.GPS.OrderBy(x => x.ID).Select(x => new
//			{
//				id = x.ID,
//				imei = x.IMEI,
//				descripcion = x.DESCRIPCION_LARGA,
//				equipo = x.EQUIPO.DESCRIPCION_LARGA
//			}).ToList().Select(x => new
//			{
//				value = x.id,
//				label = x.descripcion + " - " + x.imei + " (" + (x.equipo ?? "SIN EQUIPO") + ")"
//			});


//			//.Select(item => new
//			//{
//			//	value = item.id,
//			//	label = item.imei + "(" + (item.equipo != null ? item.equipo.DESCRIPCION_LARGA : "SIN EQUIPO" + ")")

//			//});

//			ViewData["gpsList"] = gpsList;

//			return View(viewModel);
//		}

//		public IActionResult Datos(SimuladorDatos datos)
//		{
//			Utils.dump(datos);
//			var errors = new StringBuilder();

//			if (datos.ruta == 0)
//				errors.Append("- Ruta es requerido<br/>");

//			if (datos.gps == 0)
//				errors.Append("- Equipo es requerido<br/>");


//			switch (datos.tipo)
//			{
//				case GPSMonitoreoServer.Reports.Enums.ReportType.Frequency:
//					if (datos.frecuencia_tipo == 0)
//						errors.Append("- Reporte de frecuencia es requerido<br/>");
//					else if (datos.frecuencia_tipo != GPSMonitoreoServer.Reports.Enums.FrequencyReportType.Timing)
//						errors.Append("- Reporte de frecuencia deshabilitado temporalmente.  Solo por tiempo es disponible<br/>");
//					break;

//				case GPSMonitoreoServer.Reports.Enums.ReportType.Event:
//					if (datos.evento_tipo == 0)
//						errors.Append("- Reporte de evento es requerido<br/>");
//					else
//					{
//						if (datos.evento_tipo != EventReportType.Motion && datos.evento_valor == 0)
//							errors.Append("- Valor para reporte de evento seleccionado es requerido<br/>");
//					}

//					break;

//				case GPSMonitoreoServer.Reports.Enums.ReportType.Alarm:
//					if (datos.alarma_tipo == 0)
//						errors.Append("- Reporte de alarma es requerido<br/>");
//					else
//					{


//					}
//					break;

//				default:
//					errors.Append("- Tipo de reporte es requerido<br/>");
//					break;
//			}

//			if (datos.movimiento == MotionStatus.Unknown)
//				errors.Append("- Estado Movimiento es requerido<br/>");

//			if (errors.Length > 0)
//				return JsonError("Se encontraron los siguientes errores:<br/><br/>" + errors.ToString());
//			else
//			{
//				//var rpt = new GPSMonitoreoServer.Reports.Reports.

//				LocationReport report = null;

//				var cmd = DBContext.Database.Connection.CreateCommand();
//				cmd.CommandText = $"UPDATE GPS_TEMP SET RUTA_ID = {datos.ruta} WHERE GPS_ID = {datos.gps} ";
//				cmd.ExecuteNonQuery();
//				cmd.Dispose();

//				switch (datos.tipo)
//				{
//					case ReportType.Alarm:
//						report = new GPSMonitoreoServer.Reports.Reports.AlarmReport(datos.alarma_tipo);
//						break;

//					case ReportType.Frequency:
//						report = new GPSMonitoreoServer.Reports.Reports.FrequencyReport(GPSMonitoreoServer.Reports.Enums.FrequencyReportType.Timing);
//						break;

//					case ReportType.Event:
//						report = new GPSMonitoreoServer.Reports.Reports.EventReport(datos.evento_tipo)
//						{
//							EventStatus = datos.evento_valor
//						};
//						break;
//				}

//				report.MotionStatus = datos.movimiento;
//				//report.IgnitionStatus = datos.movimiento,
//				report.GpsTime = DateTime.Now;
//				report.SendTime = DateTime.Now;

//				report.Latitude = System.Convert.ToDouble(datos.lat);
//				report.Longitude = System.Convert.ToDouble(datos.lng);
//				report.Device = new GPSMonitoreoServer.Reports.Structs.Device(DeviceBrand.Queclink, "GV300")
//				{
//					Imei = DBContext.GPS.First(item => item.ID == datos.gps).IMEI
//				};
//				report.Speed = (float)datos.velocidad;

//				Console.Write(report.ReportType);
//				Utils.dump(report);


//				var mem = new System.IO.MemoryStream();

//				var formatter = new BinaryFormatter();
//				formatter.Serialize(mem, report);

//				var bts = mem.ToArray();

//				var ip = IPAddress.Parse("127.0.0.1");


//				var ep = new IPEndPoint(ip, 1002);

//				var client = new UdpClient();
//				client.Send(bts, bts.Length, ep);
//				client.Dispose();

//				return JsonOk("okok");
//			}


//			//return null;
//		}

//		public IActionResult AlarmasLog()
//		{
//			var layoutModel = new ViewModels.AppLayout()
//			{
//				Title = "SIMULACION :: LOG DE ALARMAS"
//			};

//			return View(layoutModel);
//		}

//		public IActionResult EventosLog()
//		{
//			var layoutModel = new ViewModels.AppLayout()
//			{
//				Title = "SIMULACION :: LOG DE EVENTOS Y ACTIVIDADES"
//			};

//			return View(layoutModel);
//		}

//		public async Task<IActionResult> Equipos()
//		{

//			var layoutModel = new ViewModels.AppLayout()
//			{
//				Title = "SIMULACION :: ESTADO EQUIPOS"
//			};


//			var regions = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(REGIONES))).Items;


//			ViewData["regions"] = regions;


//			switch (this.CurrentUser.Role)
//			{
//				case Role.SuperAdmin:
//				case Role.InternalMonitorist:
//					var cats_equipos = 
//					//ViewData["lista_tree_categoria_equipo"] = cats_equipos.ToJqwidgetsTree(false);
//					ViewData["equipmentsTree"] = DBContext.ProcedureDataTable("SP_CATS_Y_EQUIPOS");
//					break;
//			}


//			//ViewData["lista_tree_categoria_producto"] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree(false);
//			//ViewData["lista_tree_categoria_producto"] = DBContext.CategoriesTwoTablesTree<PRODUCTOS_CATS, PRODUCTOS>().ToJqwidgetsTree(false);

//			ViewData["productsTree"] = DBContext.CategoriesTwoTablesTree<PRODUCTOS_CATS, PRODUCTOS>();


//			//ViewData["lista_cmb_alarma_nivel"] = DBContext.ALARMAS_NIVELES.ToJqwidgets().ToJsonString();

//			var alarmsTree = GPSMonitoreo.Web.Helpers.Alarms.GetTreeAlarms(DBContext);


//			var ids = GPSMonitoreo.Web.Helpers.Alarms.GetAccessibleAlarmsIds(DBContext, Role.CustomerUser);



//			ViewData["alarmsTreeJson"] = alarmsTree.ToJqwidgetsTree(this.CurrentUser.Role);

//			//Utils.dump("jsong string", alarmsTreeJson);



//			//ViewData["alarmas"] = DBContext.CategoriesTwoTablesTree<ALARMAS_CATS, ALARMAS>().ToJqwidgetsTree(false);
//			//ViewData["alarmas"] = alarmsTreeJson;

//			ViewData["phases"] = (await _commonDbEntityService.GetSimpleListAsync<byte>(typeof(RUTAS_FASES))).Items;

//			///ViewData[_ID_LISTA_CMB_ALARMAS_NIVELES] = DBContext.REGIONES.ToJqwidgets().ToJsonString();
//			///

//			ViewData["currentUser"] = this.CurrentUser;


//			return View(layoutModel);
//		}

//		public IActionResult EquiposSearch(EquiposSearchModel searchModel)
//		{
//			var query = DBContext.GPS_MONI_ESTADO.AsQueryable().AsNoTracking();

//			if (searchModel.region?.Count > 0)
//			{
//				query = query.Where(x => searchModel.region.Contains(x.CERCA.REGION_ID));
//			}

//			if (searchModel.producto?.Count > 0)
//			{
//				var productIds = new List<int>(searchModel.producto.Count);
//				int productId;

//				foreach (var id in searchModel.producto)
//				{
//					if (id.StartsWith("B"))
//					{
//						if (Int32.TryParse(id.Substring(1), out productId))
//						{
//							productIds.Add(productId);
//						}
//					}
//				}


//				if (productIds.Count == 0)
//				{
//					//we force not to return any record
//					productIds.Add(-1);
//				}

//				query = query.Where(x => productIds.Contains(x.VIAJE.PRODUCTO_ID));
//			}


//			var searchAlarmIds = new List<Int16>();

//			if (searchModel.alarma != null)
//			{
//				foreach (var alarma in searchModel.alarma)
//				{
//					if (alarma.StartsWith("B"))
//					{
//						searchAlarmIds.Add(Convert.ToInt16(alarma.Substring(1)));
//					}
//				}
//			}

//			var isAlarmedQueryable = DBContext.GPS_ALARMAS.AsQueryable();

//			if (this.CurrentUser.Role == Role.CustomerUser)
//			{
//				var accesibleAlarmIds = GPSMonitoreo.Web.Helpers.Alarms.GetAccessibleAlarmsIds(DBContext, this.CurrentUser.Role);

//				isAlarmedQueryable = isAlarmedQueryable.Where(x => accesibleAlarmIds.Contains(x.ALARMA_ID));

//				if (searchAlarmIds.Count == 0)
//				{
//					searchAlarmIds = accesibleAlarmIds;
//				}
//				else
//				{
//					var tempAlarmIds = new List<Int16>();
//					foreach (var alarmId in searchAlarmIds)
//					{
//						if (accesibleAlarmIds.IndexOf(alarmId) > -1)
//						{
//							tempAlarmIds.Add(alarmId);
//						}
//					}

//					searchAlarmIds = tempAlarmIds;
//				}

//				if (searchAlarmIds.Count == 0)
//				{
//					return JsonRecords(new object[] { });
//				}
//			}


//			if (this.CurrentUser.Role == Role.CustomerUser)
//			{
//				var userEntity = DBContext.USUARIOS.FirstOrDefault(x => x.ID == this.CurrentUser.Id);
//				query = query.Where(x => x.VIAJE.CONTACTOS.Any(c => c.PERSONA.PERSONA_ID == userEntity.ENTIDAD_ID));
//			}

//			if (searchAlarmIds.Count > 0)
//			{
//				query = query.Where(x => DBContext.GPS_ALARMAS.Any(a => a.VIAJE_ID == x.VIAJE_ID && searchAlarmIds.Contains(a.ALARMA_ID)));
//			}

//			if (searchModel.equipo?.Count > 0)
//			{
//				var equiposIds = new List<int>();

//				foreach (var equipo in searchModel.equipo)
//				{
//					if (equipo.StartsWith("B"))
//					{
//						equiposIds.Add(Convert.ToInt32(equipo.Substring(1)));
//					}
//				}

//				if (equiposIds.Count == 0)
//				{
//					return JsonRecords(new object[] { });
//				}
//				else
//				{
//					query = query.Where(x => equiposIds.Contains(x.GPS.EQUIPO_ID.Value));
//				}
//			}

//			if (searchModel.fase?.Count > 0)
//			{
//				Int16 eventId = 0;
//				var eventIds = new List<Int16>();

//				foreach (var fase in searchModel.fase)
//				{
//					switch (fase)
//					{
//						case 1://preparación
//							eventId = 21;
//							break;

//						case 2://posicionamiento
//							eventId = 22;
//							break;

//						case 3://carga
//							eventId = 23;
//							break;

//						case 4://en ruta
//							eventId = 24;
//							break;

//						case 5://transferencia
//							eventId = 25;
//							break;

//						case 6://descarga
//							eventId = 26;
//							break;

//						case 7: //cierre de viaje	
//							eventId = 27;
//							break;
//					}

//					eventIds.Add(eventId);
//				}

//				query = query.Where(x => DBContext.GPS_EVENTOS.Any(e => e.VIAJE_ID == x.VIAJE_ID && eventIds.Contains(e.EVENTO_ID) && e.GPS_EVENTO_ID == null));
//			}



//			var selectQuery = query.Select(x => new
//			{
//				id = x.GPS_ID,
//				imei = x.GPS.IMEI,
//				alternoId = x.GPS.EQUIPO.ALTERNO_ID,
//				placa = x.GPS.EQUIPO.PLACA,
//				lat = x.REPORTE.LAT,
//				lng = x.REPORTE.LNG,
//				//isAlarmed = DBContext.GPS_ALARMAS.Any(a => a.VIAJE_ID == x.VIAJE_ID),
//				isAlarmed = isAlarmedQueryable.Any(a => a.VIAJE_ID == x.VIAJE_ID && a.REPORTE_FIN_ID == null),
//				tripId = x.VIAJE_ID
//			});

//			var records = selectQuery.ToList().Select(r => new
//			{
//				r.id,
//				r.imei,
//				equipo = r.alternoId + " - " + r.placa,
//				r.lat,
//				r.lng,
//				r.isAlarmed,
//				r.tripId
//			});


//			//var query = DBContext.GPS.AsNoTracking()
//			//	.Select(x => new
//			//	{
//			//		id = x.ID,
//			//		imei = x.IMEI,
//			//		gps = x.DESCRIPCION_LARGA,
//			//		equipo = x.EQUIPO.DESCRIPCION_LARGA,
//			//		lat = x.MONITOREO_ESTADO.REPORTE.LAT,
//			//		lng = x.MONITOREO_ESTADO.REPORTE.LNG,
//			//		isAlarmed = DBContext.GPS_ALARMAS.Any(a => a.GPS_ID == x.ID && a.VIAJE_ID == x.MONITOREO_ESTADO.VIAJE_ID),
//			//		tripId = x.MONITOREO_ESTADO.VIAJE_ID
//			//		//alarmas = x.ALARMAS.Count()
//			//	});

//			Utils.dump(searchModel);


//			//var select = query.ToList()
//			//	.Select(x => new
//			//	{
//			//		x.id,
//			//		x.imei,
//			//		x.gps,
//			//		x.equipo,
//			//		x.lat,
//			//		x.lng,

//			//		//alarmas = DBContext.GPS_ALARMAS.Where(item => item.GPS_ID == x.id).Select(item => new
//			//		//{
//			//		//	alarma = item.ALARMA.DESCRIPCION_LARGA
//			//		//})



//			//	});






//			return JsonRecords(records);

//			//GPSMonitoreo.Core.Utils.ObjectJsonDumper.Dump(query.ToList(), 2);

//			return null;
//		}


//		public IActionResult AlarmasView()
//		{
//			return View();
//		}


//		[HttpGet("/simulation/[controller]/[action]/{gpsId}/{tripId}")]
//		public IActionResult Detalle(int gpsId, int tripId)
//		{
//			var query = DBContext.GPS.AsNoTracking().Where(x => x.ID == gpsId)
//				.Select(x => new
//				{
//					id = x.ID,
//					description = x.DESCRIPCION_LARGA,
//					equipmentAlternId = x.EQUIPO.ALTERNO_ID,
//					equipmentPlate = x.EQUIPO.PLACA,
//					equipmentDescription = x.EQUIPO.DESCRIPCION_LARGA
//				})

//				;
//			//Console.WriteLine("alarms count: " + query.FirstOrDefault().ALARMAS.Count());
//			var alarmsQuery = DBContext.VW_ALARMAS_LOG.AsQueryable()
//				.Where(x => x.VIAJE_ID == tripId)
//				;

//			var alarmsSummary = DBContext.VW_ALARMAS_SUMARIO.AsQueryable()
//				.Where(x => x.VIAJE_ID == tripId && x.NIVEL == 1);



//			if (this.CurrentUser.Role == Role.CustomerUser)
//			{
//				var accesibleAlarmIds = GPSMonitoreo.Web.Helpers.Alarms.GetAccessibleAlarmsIds(DBContext, this.CurrentUser.Role);
//				alarmsQuery = alarmsQuery.Where(x => accesibleAlarmIds.Contains(x.ALARMA_ID));
//				alarmsSummary = alarmsSummary.Where(x => accesibleAlarmIds.Contains(x.ALARMA_ID));
//				//isAlarmedQueryable = isAlarmedQueryable.Where(x => accesibleAlarmIds.Contains(x.ALARMA_ID));
//			}










//			alarmsQuery = alarmsQuery.OrderBy(x => x.CATEGORIA_ORDENADOR).ThenByDescending(x => x.ID);


//			var alarms = alarmsQuery.Select(x => new
//			{
//				categoria = x.CATEGORIA,
//				cerca = x.CERCA,
//				duracion = x.DURACION,
//				alarma = x.ALARMA,
//				inicio = x.INICIO,
//				fin = x.FIN,
//				alarmaValorStandard = x.VALOR_STANDARD,
//				alarmaValor = x.VALOR_EFECTIVO,
//				alarmaValorDiferencia = x.VALOR_EFECTIVO - x.VALOR_STANDARD
//			}).ToList();



//			//.GroupBy(x => x.ALARMA_ID)
//			var ocurrences = alarmsSummary.Select(x => new
//			{
//				categoria = x.CATEGORIA,
//				alarma = x.ALARMA,
//				ocurrencias = x.OCURRENCIAS
//			}).ToList()
//			;

//			//Utils.dump(records);

//			return JsonRecord(new
//			{
//				gps = query.FirstOrDefault(),
//				alarms = alarms,
//				alarmsOcurrences = ocurrences
//			});
//		}

//		private string GetEventStatus(Int16 eventId, byte eventStatus)
//		{
//			switch (eventStatus)
//			{
//				case 1:
//					return "ON";
//				case 2:
//					return "OFF";
//				case 3:
//					return "INGRESO";
//				case 4:
//					return "EGRESO";
//				case 5:
//					return "INICIO";
//				case 6:
//					return "TERMINO";
//			}
//			//switch(eventId)
//			//{
//			//	case 1: //CERCA: EN RUTA
//			//	case 6: //CERCA: PARADAS OPERATIVAS
//			//	case 7: //CERCA: PARADAS CONDUCTOR
//			//	case 8: //CERCA: AVISOS LOGISTICOS
//			//	case 9: //CERCA: PARADAS NO AUTORIZ
//			//		switch(eventStatus)
//			//		{
//			//			case 1:
//			//				return "ON";
//			//			case 2:
//			//				return "OFF";
//			//			case 3:
//			//				return "INGRESO";
//			//			case 4:
//			//				return "EGRESO";
//			//			case 5:
//			//				return "INICIO";
//			//			case 6:
//			//				return "TERMINO";
//			//		}
//			//		break;
//			//	default:
//			//		switch(eventStatus)
//			//		{
//			//			case 1:
//			//				return "ON";
//			//			case 2:
//			//				return "OFF";
//			//		}
//			//		break;
//			//		//return ((EventStatus)eventStatus).ToString();
//			//}

//			return eventStatus.ToString();

//		}

//		public IActionResult EventosLogSearch(EventosLogSearchModel searchModel)
//		{


//			var query = DBContext.GPS_EVENTOS.AsNoTracking()
//				.Where(x => x.VIAJE_ID == searchModel.viaje);

//			if (searchModel.recordcount <= 0)
//				searchModel.recordcount = query.Count();


//			var query_select = query
//				.Join(DBContext.GPS_REPORTES, t1 => t1.REPORTE_ID, t2 => t2.ID, (t1, t2) => new { gpsEvento = t1, gpsReporte = t2 })
//				.OrderByDescending(x => x.gpsEvento.ID)
//				.Skip(searchModel.pagenum * searchModel.pagesize).Take(searchModel.pagesize)
//				.Select(x => new
//				{
//					eventoId = x.gpsEvento.EVENTO_ID,
//					evento = x.gpsEvento.EVENTO.DESCRIPCION_LARGA,
//					eventoEstado = x.gpsEvento.EVENTO_ESTADO,
//					fechahora = x.gpsEvento.FECHAHORA,
//					cerca = x.gpsEvento.CERCA.DESCRIPCION_LARGA,
//					lat = x.gpsReporte.LAT,
//					lng = x.gpsReporte.LNG,
//					relDescripcion = DBContext.FN_EVENTO_REL_DESCRIPCION(x.gpsEvento.EVENTO_ID, x.gpsEvento.REL_ID)
//				})
//				;





//			var records = query_select.ToList().Select(x => new
//			{
//				x.eventoId,
//				x.evento,
//				eventoEstado = GetEventStatus(x.eventoId, x.eventoEstado),
//				fechahora = x.fechahora.ToString("dd/MM/yyyy HH:mm:ss"),
//				x.cerca,
//				x.lat,
//				x.lng,
//				x.relDescripcion
//			});

//			return JsonRecords(records, searchModel.recordcount);

//		}

//		public IActionResult AlarmasLogSearch(AlarmasLogSearchModel searchModel)
//		{

//			var query = DBContext.VW_ALARMAS_LOG.AsQueryable()
//				.Where(x => x.VIAJE_ID == searchModel.viaje);



//			if (this.CurrentUser.Role == Role.CustomerUser)
//			{
//				var accesibleAlarmIds = GPSMonitoreo.Web.Helpers.Alarms.GetAccessibleAlarmsIds(DBContext, this.CurrentUser.Role);
//				query = query.Where(x => accesibleAlarmIds.Contains(x.ALARMA_ID));
//			}

//			query = query.OrderBy(x => x.CATEGORIA_ORDENADOR).ThenByDescending(x => x.ID);


//			var records = query.Select(x => new
//			{
//				categoria = x.CATEGORIA,
//				cerca = x.CERCA,
//				duracion = x.DURACION,
//				alarma = x.ALARMA,
//				inicio = x.INICIO,
//				fin = x.FIN,
//				alarmaValorStandard = x.VALOR_STANDARD,
//				alarmaValor = x.VALOR_EFECTIVO,
//				alarmaValorDiferencia = x.VALOR_EFECTIVO - x.VALOR_STANDARD
//			}).ToList()
//			//.Select(x => new
//			//{
//			//	x.categoria,
//			//	x.alarma,
//			//	x.cerca,
//			//	x.duracion,
//			//	inicio = x.inicio.ToString("dd/MM/yyyy HH:mm:ss"),
//			//	fin = x.fin?.ToString("dd/MM/yyyy HH:mm:ss"),
//			//	x.alarmaValorStandard,
//			//	x.alarmaValor,
//			//	alarmaValorDiferencia = x.alarmaValor - x.alarmaValorStandard
//			//})
//			;

//			//var query = DBContext.GPS_ALARMAS_LOG.AsNoTracking()
//			//		.GroupJoin(DBContext.GPS_ALARMAS_LOG, t1 => t1.FIN_LOG_ID, t2 => t2.ID, (a, b) => new { t1 = a, t2 = b})
//			//		.SelectMany(x => x.t2.DefaultIfEmpty(), (a, b) => new { t1 = a.t1, t2 = b})
//			//		.Where(x => x.t1.INICIO_LOG_ID == null)
//			//		.OrderBy(x => DBContext.FN_TOP_PATH("ALARMAS_CATS", x.t1.ALARMA.CATEGORIA_ID))
//			//		//.ThenByDescending(x => x.t1.ID)

//			//		.Select(x => new
//			//		{
//			//			categoria = DBContext.FN_TOP_PATH("ALARMAS_CATS", x.t1.ALARMA.CATEGORIA_ID),
//			//			alarma = x.t1.ALARMA.DESCRIPCION_LARGA,
//			//			inicio = x.t1.FECHAHORA,
//			//			fin = (DateTime?)x.t2.FECHAHORA,
//			//			duracion = x.t1.DURACION,
//			//			cerca = x.t1.CERCA.DESCRIPCION_LARGA
//			//		})
//			//		.OrderBy(x => DBContext.FN_TOP_PATH("ALARMAS_CATS", x.t1.ALARMA.CATEGORIA_ID))
//			//		;

//			//var records = query.ToList()
//			//	.Select(x => new
//			//	{
//			//		x.categoria,
//			//		x.alarma,
//			//		inicio = x.inicio.ToString("dd/MM/yyyy HH:mm:ss"),
//			//		fin = x.fin?.ToString("dd/MM/yyyy HH:mm:ss"),
//			//		x.cerca
//			//	})
//			//	;

//			//Utils.dump(records);
//			return JsonRecords(records);
//			//return null;

//		}

//		public IActionResult AlarmasLogMap()
//		{
//			var query = DBContext.GPS_ALARMAS.AsNoTracking()
//					//.GroupJoin(DBContext.GPS_ALARMAS_LOG, t1 => t1.FIN_LOG_ID, t2 => t2.ID, (a, b) => new { t1 = a, t2 = b })
//					//.SelectMany(x => x.t2.DefaultIfEmpty(), (a, b) => new { t1 = a.t1, t2 = b })
//					//.Where(x => x.t1.INICIO_LOG_ID == null)
//					//.ThenByDescending(x => x.t1.ID)
//					.Join(DBContext.GPS_REPORTES, t1 => t1.REPORTE_ID, t2 => t2.ID, (t1, t2) => new { gpsAlarma = t1, gpsReporte = t2 })


//					.Select(x => new
//					{
//						alarma = x.gpsAlarma.ALARMA.DESCRIPCION_LARGA,
//						inicio = x.gpsAlarma.FECHAHORA_INICIO,
//						fin = x.gpsAlarma.FECHAHORA_FIN,
//						duracion = x.gpsAlarma.DURACION,
//						cerca = x.gpsAlarma.CERCA.DESCRIPCION_LARGA,
//						lat = x.gpsReporte.LAT,
//						lng = x.gpsReporte.LNG,
//						//velocidad = x.t1.REPORTE.VELOCIDAD
//					})
//					;

//			var records = query.ToList()
//				.Select(x => new
//				{
//					x.alarma,
//					inicio = x.inicio.ToString("dd/MM/yyyy HH:mm:ss"),
//					fin = x.fin?.ToString("dd/MM/yyyy HH:mm:ss"),
//					x.cerca,
//					x.lat,
//					x.lng,
//					//x.velocidad
//				})
//				;

//			return JsonRecords(records);
//		}

//		public IActionResult StatusMonitoreo()
//		{

//			//var status = DBContext.EQUIPOS_MONI_ESTADO.First();
//			//var resp = new
//			//{
//			//	monitoreo_estado = status.ESTADO.DESCRIPCION_LARGA,
//			//	cerca = status.CERCA?.DESCRIPCION_LARGA,
//			//	cerca_capa = status.CERCA?.CAPA.DESCRIPCION_LARGA,
//			//	lat = status.LAT,
//			//	lng = status.LNG,
//			//	alarma = status.ALARMA?.DESCRIPCION_LARGA,
//			//	alarma_valor = status.VALOR
//			//};

//			//return JsonResponse("OK", resp);
//			return null;
//		}
//	}

//	public interface IItem
//	{

//	}

//	public class BaseClass : IItem
//	{

//	}

//	public class DerivedClass : BaseClass, IItem
//	{

//	}
//}