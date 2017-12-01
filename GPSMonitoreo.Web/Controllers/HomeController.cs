using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVCHelpers.ActionResults;
using GPSMonitoreo.Web.Classes;

using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using GPSMonitoreo.Core.Authorization;
using GPSMonitoreo.Core.Enums;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GPSMonitoreo.Web.Controllers
{

	[Authorize]
    public class HomeController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

			//return Content("sadf");
			return View();
        }

		public IActionResult Test()
		{
			return View();
		}

		public void BuildMenu(MenuItem item, System.Text.StringBuilder builder, int parentId, ref int currentId)
		{
			builder.Append("<MenuItem>");



			


			currentId++;

			int childParentId = currentId;



			builder.Append("<id>" + currentId.ToString() + "</id>");

			if(parentId > 0 )
			{
				builder.Append("<padre>" + parentId.ToString() + "</padre>");
			}

			builder.Append("<label>" + item.Title + "</label>");
			builder.Append("<expanded>" + item.Expanded + "</expanded>");
			builder.Append("<icon>" + item.Icon + "</icon>");
			builder.Append("<onclick>" + item.OnClick + "</onclick>");

			if(item.RequiredRoles != null)
			{
				builder.Append("<roles>" + string.Join(",", item.RequiredRoles.Select(x => x.ToString("D"))) + "</roles>");
			}

			if(item.Items != null)
			{
				builder.Append("<items>");
				foreach(var child in item.Items)
				{
					BuildMenu(child, builder, childParentId, ref currentId);
				}
				builder.Append("</items>");
			}

			






			builder.Append("</MenuItem>");
		}

		public IActionResult Menu()
		{
			var roleManager = (Services.Authorization.RoleManager)ServiceProvider.GetService(typeof(Services.Authorization.RoleManager));

			

			var menu = new Menu();

			menu.AddItem(
				new MenuItem(
					"Home",
					icon: "<webicon class=\"oliver_essential-collection_home\" style=\"margin-right:5px\" ></webicon>",
					expanded: true,
					items: new List<MenuItem>
					{
						new MenuItem(
							"Simulación",
							expanded: true,
							roles: new Role[] { Role.SuperAdmin },
							items: new List<MenuItem>
							{
								new MenuItem(
									"Simulador",
									onClick: "App.loadLayout('/simulation/simulator', 'Simulador')"
								)
							}
						),
						new MenuItem(
							"Monitoreo",
							expanded: true,
							roles: new Role[] { Role.SuperAdmin },
							icon: "<webicon class=\"oliver_business-collection_graph-1\" style=\"margin-right:5px\"></webicon>",
							items: new List<MenuItem>
							{
								new MenuItem(
									"Estado Equipos",
									onClick: "App.loadLayout('/simulation/simulator/equipos', 'Estado Equipos')",
									icon: "<webicon class=\"oliver_delivery_truck\" style=\"margin-right:5px\"></webicon>"
								),
								new MenuItem(
									"Log Alarmas",
									onClick: "App.loadLayout('/simulation/simulator/alarmaslog', 'Log Alarmas')",
									icon: "<webicon class=\"oliver_essential-collection_alarm-1\" style=\"margin-right:5px\"></webicon>"
								),
								new MenuItem(
									"Log Eventos",
									onClick: "App.loadLayout('/simulation/simulator/eventoslog', 'Log Eventos')",
									icon: "<webicon class=\"oliver_essential-collection_clock-1\" style=\"margin-right:5px\"></webicon>"
								)
							}
						),
						new MenuItem(
							"Entidades",
							icon: "<webicon class=\"oliver_essential-collection_id-card-3\" style=\"margin-right:5px\"></webicon>",
							roles: new Role[] { Role.SuperAdmin },
							permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Core.Enums.Entity.Entity) },
							items: new List<MenuItem>
							{
								new MenuItem(
									"Nueva entidad",
									icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
									onClick: "App.entidades.entidad.editNew()",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Entity, PermissionAction.Create) }
								),
								new MenuItem(
									"Búsqueda",
									icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
									onClick: "App.entities.entity.index()",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Entity, PermissionAction.View) }
								)
							}
						),
						new MenuItem(
							"Equipos",
							icon: "<webicon class=\"oliver_essential-collection_id-card-3\" style=\"margin-right:5px\"></webicon>",
							roles: new Role[] { Role.SuperAdmin },
							permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Core.Enums.Entity.Entity) },
							items: new List<MenuItem>
							{
								new MenuItem(
									"Nuevo equipo",
									icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
                                    onClick: "App.equipments.equipment.editNew()",
                                   
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Equipment, PermissionAction.Create) }
								),
								new MenuItem(
									"Búsqueda",
									icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
									onClick: "App.equipments.equipment.index()",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Equipment, PermissionAction.View) }
								)
							}
						),
						new MenuItem(
							"Geografico",
							icon: "<webicon class=\"oliver_essential-collection_map-1\" style=\"margin-right:5px\"></webicon>",
							roles: new Role[] { Role.SuperAdmin },
							permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, new int[] { (int)Entity.Geofence, (int)Entity.RouteSegment, (int)Entity.RouteSection, (int)Entity.Route, (int)Entity.RouteCourse }) },
							items: new List<MenuItem>
							{
								new MenuItem(
									"Cercas",
									icon: "<webicon class=\"oliver_designer-set_distort-1\" style=\"margin-right:5px\"></webicon>",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, new int[] { (int)Entity.Geofence }) },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Nueva cerca2",
											onClick: "App.geographics.geofences.editNew()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Geofence, PermissionAction.Create) }
										),
										new MenuItem(
											"Búsqueda",
											onClick: "App.geographics.geofences.index()",
											icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Geofence, PermissionAction.View) }
										)
									}
								),
								new MenuItem(
									"Segmentos",
									icon: "<webicon class=\"oliver_essential-collection_placeholder-1\" style=\"margin-right:5px\"></webicon>",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, new int[] { (int)Entity.RouteSegment }) },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Nuevo segmento",
											onClick: "App.geographics.segments.editNew()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSegment, PermissionAction.Create) }

										),
										new MenuItem(
											"Búsqueda",
											onClick: "App.geographics.segments.index()",
											icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSegment, PermissionAction.View) }

										)
									}
								),
								new MenuItem(
									"Tramos",
									icon: "<webicon class=\"oliver_pins-and-locations_route\" style=\"margin-right:5px\"></webicon>",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSection) },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Nuevo tramo",
											onClick: "App.geographics.sections.editNew()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSection, PermissionAction.Create) }
										),
										new MenuItem(
											"Búsqueda",
											onClick: "App.geographics.sections.index()",
											icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSection, PermissionAction.View) }

										)
									}
								),
								new MenuItem(
									"Rutas",
									icon: "<webicon class=\"oliver_essential-collection_route\" style=\"margin-right:5px\"></webicon>",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Route) },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Nueva ruta",
											onClick: "App.geographics.routes.editNew()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Route, PermissionAction.Create) }
										),

										new MenuItem(
											"Plantilla Rutas",
											onClick: "App.geographics.routes.routeTemplate()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Route, PermissionAction.Create) }
										),



										new MenuItem(
											"Búsqueda",
											onClick: "App.geographics.routes.index()",
											icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.Route, PermissionAction.View) }
										),
										new MenuItem(
											"Fases",
											onClick: "App.loadLayout('/geografico/rutas/fases', 'RUTA - FASES', 'TabbedForm')",
											icon: "<webicon class=\"oliver_arrow-collection_fast-fwd\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								//new MenuItem(
								//	"Rutas exernas",
								//	icon: "<webicon class=\"oliver_essential-collection_route\" style=\"margin-right:5px\"></webicon>",
								//	items: new List<MenuItem>
								//	{
								//		new MenuItem(
								//			"Nuevo tramo",
								//			onClick: "App.geografico.rutasexternas.edit()",
								//			icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
								//		),
								//		new MenuItem(
								//			"Búsqueda",
								//			onClick: "App.geografico.rutasexternas.index()",
								//			icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
								//		)
								//	}
								//),
								new MenuItem(
									"Trayectos",
									icon: "<webicon class=\"oliver_essential-collection_route\" style=\"margin-right:5px\"></webicon>",
									roles: new Role[] { Role.SuperAdmin },
									permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteCourse) },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Nuevo trayecto",
											onClick: "App.geographics.courses.editNew()",
											icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteCourse, PermissionAction.Create)  }
										),
										new MenuItem(
											"Búsqueda trayectos",
											onClick: "App.geographics.courses.index()",
											icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteCourse, PermissionAction.View)  }
										)
									}
								)
							}
						),
						new MenuItem(
							"Administración General",
							icon: "<webicon class=\"oliver_essential-collection_settings-4\" style=\"margin-right:5px\"></webicon>",
							roles: new Role[] { Role.SuperAdmin },
							items: new List<MenuItem>
							{
								new MenuItem(
									"Accesos y Seguridad",
									roles: new Role[] { Role.SuperAdmin },
									items: new List<MenuItem>
									{
										new MenuItem(
											"Usuarios",
											roles: new Role[] { Role.SuperAdmin },
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nuevo usuario",
													onClick: "App.authorization.users.editNew()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Usuarios",
													onClick: "App.authorization.users.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Roles",
											roles: new Role[] { Role.SuperAdmin },
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nuevo rol",
													onClick: "App.authorization.roles.editNew()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Roles",
													onClick: "App.authorization.roles.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										)
									}
								),
								new MenuItem(
									"Alarmas y Logs",
									icon: "<webicon class=\"oliver_essential-collection_alarm-1\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Categoria Alarmas",
											onClick: "App.generalparameters.alarmcategory.index()",
											icon: "<webicon class=\"oliver_essential-collection_alarm\" style=\"margin-right:5px\"></webicon>"
										),

										new MenuItem(
											"Grado Alarmas",
											onClick: "App.generalparameters.alarmgrade.index()",
											icon: "<webicon class=\"oliver_essential-collection_alarm\" style=\"margin-right:5px\"></webicon>"
										),

										new MenuItem(
											"Limite Velocidades",
											onClick: "App.generalparameters.alarmspeedlimit.index()"
											//icon: ""
										),

										new MenuItem(
											"Filtros Alarmas",
											roles: new Role[] { Role.SuperAdmin },
											permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Section, (int)Section.AlarmFilters)},
											onClick: "App.admin.alarmfilters.index()"
										),

										new MenuItem(
											"Unidades",
											onClick: "App.general.alarmas.aunidades()",
											icon: "<webicon class=\"oliver_constructions_drawing\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Permans",
											onClick: "App.general.alarmas.permans()",
											icon: "<webicon class=\"oliver_essential-collection_stopwatch-1\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Permans Resets",
											onClick: "App.general.alarmas.permansresets()"
										),
										new MenuItem(
											"Niveles",
											onClick: "App.general.alarmas.niveles()",
											icon: "<webicon class=\"oliver_arrow-collection_transfer\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Tipos Logs",
											onClick: "App.general.logtipos.cats()",
											icon: "<webicon class=\"oliver_office-elements_notes\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Matrices Control Velocidad",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nuevo",
													onClick: "App.general.controlvelocidad.velocidad.edit()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Búsqueda",
													onClick: "App.general.controlvelocidad.velocidad.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										)
									}
								),
								new MenuItem(
									"Calendarios",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Calendario Operativo",
											onClick: "App.loadTab('/general/calendarios/calendariooperativo', 'Calendarios: Operativo')"
										)
									}
								),
								new MenuItem(
									"Viajes",
									icon: "<webicon class=\"oliver_transport-collection_truck-2\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Estado",
											onClick: "App.general.viajes.estado()"
										)
									}
								),
								new MenuItem(
									"Fases",
									icon: "<webicon class=\"oliver_arrow-collection_fast-fwd\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Geografico",
											onClick: "App.general.viajes.geografico()",
											icon: "<webicon class=\"oliver_essential-collection_map-1\" style=\"margin-right:5px\"></webicon>"

										),
										new MenuItem(
											"Operacion Ruta",
											onClick: "App.general.viajes.operacionruta()",
											icon: "<webicon class=\"oliver_essential-collection_route\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								new MenuItem(
									"Geografico",
									icon: "<webicon class=\"oliver_essential-collection_map-1\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Cercas",
											icon: "<webicon class=\"oliver_designer-set_distort-1\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Tráfico Cercas",
													onClick: "App.generalparameters.roadtraffic.index()",
													icon: "<webicon class=\"\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Categorias",
													onClick: "App.generalparameters.geofencecategory.index()",
													icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Jerarquia Categorias",
													onClick: "App.general.geografico.cercastree()",
													icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Segmentos",
											icon: "<webicon class=\"oliver_essential-collection_placeholder-1\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Categorias",
													onClick: "App.generalparameters.segmentcategory.index()",
													icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Jerarquia Categorias",
													onClick: "App.general.geografico.segmentostree()",
													icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Tramos",
											icon: "<webicon class=\"oliver_pins-and-locations_route\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Categorias",
													onClick: "App.generalparameters.sectioncategory.index()",
													icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Jerarquia Categorias",
													onClick: "App.general.geografico.tramostree()",
													icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Rutas",
											icon: "<webicon class=\"oliver_essential-collection_route\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Categorias",
													onClick: "App.generalparameters.routecategory.index()",
													icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Jerarquia Categorias",
													onClick: "App.general.geografico.rutastree()",
													icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
												)
											}
										)
									}
								),
								new MenuItem(
									"Equipos",
									icon: "<webicon class=\"oliver_transport-collection_crane-2\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Grupos",
											onClick: "App.generalparameters.equipmentgroup.index()",
											icon: "<webicon class=\"oliver_designer-set_group\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Estados Operativos",
											onClick: "App.generalparameters.equipmentoperationalstatus.index()",
											icon: "<webicon class=\"oliver_transport-collection_car-repair-3\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Categorias",
											onClick: "App.generalparameters.equipmentcategory.index()",
											icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Jerarquia Categorias",
											onClick: "App.general.equipos.catstree()",
											icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Capacidades",
											onClick: "App.generalparameters.equipmentcapability.index()",
											icon: "<webicon class=\"oliver_delivery_weight-scale\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								new MenuItem(
									"Productos",
									icon: "",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Categorias",
											onClick: "App.generalparameters.productcategory.index()",
											icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								new MenuItem(
									"Marcas y Modelos",
									icon: "<webicon class=\"oliver_business-collection_price-tag-1\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Marcas",
											onClick: "App.generalparameters.brand.index()",
											icon: "<webicon class=\"oliver_business-collection_price-tag-7\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Modelos",
											onClick: "App.generalparameters.model.index()",
											icon: "<webicon class=\"oliver_business-collection_price-tag-9\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Categoria Marcas",
											onClick: "App.generalparameters.brandcategory.index()",
											icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Jerarquia Marcas",
											onClick: "App.general.marcasmodelos.categoriastree()",
											icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								new MenuItem(
									"Entidades",
									icon: "<webicon class=\"oliver_essential-collection_id-card-1\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Tipos",
											onClick: "App.generalparameters.entitytype.index()",
											icon: "<webicon class=\"oliver_office-elements_id-card\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Categorias",
											onClick: "App.generalparameters.entitycategory.index()",
											icon: "<webicon class=\"oliver_arrow-collection_multiply-3\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Jerarquia Entidades",
											onClick: "App.general.entidades.categoriastree()",
											icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Relaciones/Empresa",
											onClick: "App.generalparameters.entitycontacttype.index()",
											icon: "<webicon class=\"oliver_real-state-collection_cityscape\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Cargos",
											onClick: "App.generalparameters.entityposition.index()",
											icon: "<webicon class=\"oliver_essential-collection_user-6\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Emails Proposito",
											onClick: "App.generalparameters.emailpurpose.index()",
											icon: "<webicon class=\"oliver_dialog-assets_mail-1\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Emails Tipos",
											onClick: "App.generalparameters.emailtype.index()",
											icon: "<webicon class=\"oliver_interaction-assets_mail-6\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Telefonos Tipos",
											onClick: "App.generalparameters.phonetype.index()",
											icon: "<webicon class=\"oliver_interaction-assets_phone-call-9\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Direcciones Tipos",
											onClick: "App.generalparameters.addresstype.index()",
											icon: "<webicon class=\"oliver_pins-and-locations_map-10\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Ubicaciones Tipos",
											onClick: "App.generalparameters.addresslocationtype.index()",
											icon: "<webicon class=\"oliver_pins-and-locations_pin-2\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Persona - Profesiones",
											onClick: "App.generalparameters.entitypersonprofession.index()",
											icon: "<webicon class=\"oliver_constructions_engineer\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Persona - Saludos",
											onClick: "App.generalparameters.entitypersonsalutation.index()",
											icon: "<webicon class=\"oliver_essential-collection_users-1\" style=\"margin-right:5px\"></webicon>"
										)
									}
								),
								new MenuItem(
									"Localidades",
									icon: "<webicon class=\"oliver_essential-collection_worldwide\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Países",
											icon: "<webicon class=\"oliver_essential-collection_map-location\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nuevo País",
													onClick: "App.general.localidades.paises.edit()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Búsqueda",
													onClick: "App.general.localidades.paises.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Provincias",
											icon: "<webicon class=\"oliver_essential-collection_map-2\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nueva provincia",
													onClick: "App.general.localidades.provincias.edit()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Búsqueda",
													onClick: "App.general.localidades.provincias.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										),
										new MenuItem(
											"Ciudades",
											icon: "<webicon class=\"oliver_essential-collection_map\" style=\"margin-right:5px\"></webicon>",
											items: new List<MenuItem>
											{
												new MenuItem(
													"Nueva ciudad",
													onClick: "App.general.localidades.ciudades.edit()",
													icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>"
												),
												new MenuItem(
													"Búsqueda",
													onClick: "App.general.localidades.ciudades.index()",
													icon: "<webicon class=\"oliver_security_search\" style=\"margin-right:5px\"></webicon>"
												)
											}
										)
									}
								),
								new MenuItem(
									"Otros",
									icon: "<webicon class=\"\" style=\"margin-right:5px\"></webicon>",
									items: new List<MenuItem>
									{
										new MenuItem(
											"Unidades",
											onClick: "App.generalparameters.measureunit.index()",
											icon: "<webicon class=\"oliver_constructions_drawing\" style=\"margin-right:5px\"></webicon>"
										),
										new MenuItem(
											"Jerarquia Unidades",
											onClick: "App.general.otros.unidadestree()",
											icon: "<webicon class=\"oliver_business-collection_diagram-1\" style=\"margin-right:5px\"></webicon>"
										)
									}
								)
							}
						),
                        new MenuItem(
                                    "Tecnicos",
                                    icon: "<webicon class=\"oliver_pins-and-locations_route\" style=\"margin-right:5px\"></webicon>",
                                    roles: new Role[] { Role.SuperAdmin },
                                    permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSection) },
                                    items: new List<MenuItem>
                                    {
                                        new MenuItem(
                                            "Nuevo Apertura Caso",
                                            onClick: "App.tecnicos.tecnico.editNew()",
                                           //onClick: "App.equipments.equipment.editNew()",
                                            icon: "<webicon class=\"oliver_essential-collection_add-1\" style=\"margin-right:5px\"></webicon>",
                                            roles: new Role[] { Role.SuperAdmin },
                                            permissions: new RequiredPermission[] { new RequiredPermission(PermissionElementType.Entity, (int)Entity.RouteSection, PermissionAction.Create) }
                                        )
                                       
                                    }
                                )
                    }
				)
			);


			//Utils.dump(menu);

			//ViewData["menuData"] = menu.ToJqwidgetsTree(Role.SuperAdmin);
			//ViewData["menuData"] = menu.ToJqwidgetsTree(Role.InternalMonitorist);

			var rolePermissions = roleManager.GetRolePermissions(this.CurrentUser.Role);

			Utils.dump(rolePermissions);

			//ViewData["menuData"] = menu.ToJqwidgetsTree(this.CurrentUser.Role);
			ViewData["menuData"] = menu.ToJqwidgetsTree(rolePermissions);


			//roleManager.GetRolePermissions(Role.Prueba7);


			//var str = new System.Text.StringBuilder();

			//var child = menu.Items[0];
			//var currentId = 0;

			//BuildMenu(child, str, 0, ref currentId);

			//Console.WriteLine("--------------");

			//Console.WriteLine(str.ToString());

			return View();
		}

		[AllowAnonymous]
		public IActionResult TestAlarmEmail()
		{
			ViewData["dbContext"] = DBContext;

			return View();
		}
    }
}
