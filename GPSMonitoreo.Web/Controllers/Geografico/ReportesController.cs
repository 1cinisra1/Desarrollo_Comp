using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.PostModels.Geografico;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Web.Classes;

using System.Data.Entity;
using GPSMonitoreo.Data.Models;
using Newtonsoft.Json.Linq;
namespace GPSMonitoreo.Web.Controllers.Geografico
{
    public class ReportesController : BaseController
	{

		private const string _VALOR_NO_APLICA = "na";
		private const string _ID_LISTA_TREE_PRODUCTOS_CATEGORIA = "lista_tree_categoria_producto";
		private const string _ID_LISTA_TREE_EQUIPOS_CATEGORIA = "lista_tree_categoria_equipo";

		private const string _ID_LISTA_CMB_REGION = "lista_cmb_region";
		private const string _ID_LISTA_CMB_ALARMAS = "lista_cmb_alarma";
		private const string _ID_LISTA_CMB_ALARMAS_NIVELES = "lista_cmb_alarma_nivel";

		private const string _ID_SOURCE_JSON_ALARMAS = "arreglo_json_categorias_alarmas";

		public IActionResult Alarmasylogs()
		{
			


			ViewData[_ID_LISTA_CMB_REGION] = DBContext.REGIONES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();

			ViewData[_ID_LISTA_TREE_EQUIPOS_CATEGORIA] = DBContext.CategoriesTwoTablesTree<EQUIPOS_CATS, EQUIPOS>() .ToJqwidgetsTree();
			ViewData[_ID_LISTA_TREE_PRODUCTOS_CATEGORIA] = DBContext.CategoriesTree<PRODUCTOS_CATS>().ToJqwidgetsTree();

			ViewData[_ID_LISTA_CMB_ALARMAS_NIVELES] = DBContext.ALARMAS_NIVELES.OrderBy(item => item.DESCRIPCION_LARGA).ToJqwidgets().ToJsonString();
			///ViewData[_ID_LISTA_CMB_ALARMAS_NIVELES] = DBContext.REGIONES.ToJqwidgets().ToJsonString();

			List<ALARMAS_NIVELES> listaNiveles = DBContext.ALARMAS_NIVELES.OrderBy(s => s.ID).ToList();

			var query_select_relacion = (
									from an in DBContext.ALARMAS_NIVELES
									join anr in DBContext.ALARMAS_NIVELES_REL
									on an.ID equals anr.NIVEL_ID
									orderby anr.ALARMA_ID, anr.NIVEL_ID, an.DESCRIPCION_LARGA
									select new FilaNivelQuery
									{
										IDA = anr.ALARMA_ID
										,
										IDAN = anr.NIVEL_ID
										,
										DLAN = an.DESCRIPCION_LARGA
									}
								);

			List<FilaNivelQuery> listaAlarmasRelaciones = query_select_relacion.ToList();


			var query_select = (
									from a in DBContext.ALARMAS
									join ac in DBContext.ALARMAS_CATS
									on a.CATEGORIA_ID equals ac.ID
									orderby ac.ID, a.ID
									select new  FilaQuery
									{ 
										IDAC = ac.ID
										, DLAC = ac.DESCRIPCION_LARGA
										, IDA = a.ID
										, DLA = a.DESCRIPCION_LARGA
									}
								);

			List<FilaAlarmaCats> lista = new List<FilaAlarmaCats>();
			if (query_select.ToList().Count > 0)
			{
				int i = 0;
				short idACTmp = 0;
				List<FilaAlarma> listaAlarmas = null;
				FilaAlarmaCats objAC = null;
				foreach (FilaQuery fila in query_select.ToList())
				{
					if (i == 0)
					{
						idACTmp = fila.IDAC;

						objAC = generarObjAlarmaCats(idACTmp, fila.DLAC);
						//objAC.id = idACTmp;
						//objAC.descripcion = fila.DLAC;
						listaAlarmas = new List<FilaAlarma>();

						FilaAlarma objA = generarObjAlarma(listaAlarmasRelaciones, listaNiveles, fila);
						listaAlarmas.Add(objA);
					}
					else
					{
						if (fila.IDAC == idACTmp)
						{
							FilaAlarma objA = generarObjAlarma(listaAlarmasRelaciones, listaNiveles, fila);
							listaAlarmas.Add(objA);
						}
						else
						{
							idACTmp = fila.IDAC;
							objAC.arregloFilaAlarma = listaAlarmas.ToArray();
							lista.Add(objAC);

							objAC = generarObjAlarmaCats(idACTmp, fila.DLAC);
							////objAC.id = idACTmp;
							////objAC.descripcion = fila.DLAC;
							listaAlarmas = new List<FilaAlarma>();

							FilaAlarma objA = generarObjAlarma(listaAlarmasRelaciones, listaNiveles, fila);
							listaAlarmas.Add(objA);

						}
					}

					i++;

				}

				objAC.arregloFilaAlarma = listaAlarmas.ToArray();
				lista.Add(objAC);

			}
			

			
			ViewData[_ID_SOURCE_JSON_ALARMAS] = lista.ToArray().ToJsonString();


			return View();
			///return JsonRecords(records);
		}

		private FilaAlarmaCats generarObjAlarmaCats(short idAlarma, string descripcion)
		{
			FilaAlarmaCats objAC = new FilaAlarmaCats();

			objAC.id = idAlarma;
			objAC.descripcion = descripcion;
			objAC.viaje = _VALOR_NO_APLICA;
			objAC.ruta = _VALOR_NO_APLICA;
			objAC.tramo = _VALOR_NO_APLICA;
			objAC.segmento = _VALOR_NO_APLICA;
			objAC.cerca = _VALOR_NO_APLICA;


			return objAC;


		}

		/**
		 	TABLA ALARMAS_NIVELES
			1	VIAJE
			2	RUTA
			3	TRAMO
			4	SEGMENTO
			5	CERCA
			6	SIN VIAJE
		**/
		private FilaAlarma generarObjAlarma(List<FilaNivelQuery> listaAlarmasNiveles, List<ALARMAS_NIVELES>  listaNiveles, FilaQuery fila)
		{
			FilaAlarma objA = new FilaAlarma();
			objA.id = fila.IDA;
			objA.descripcion = fila.DLA;

			ALARMAS_NIVELES objAN = listaNiveles.ElementAt(0);
			objA.viaje = obtenerRelacionNivel(listaAlarmasNiveles, fila.IDA, objAN.ID);

			objAN = listaNiveles.ElementAt(1);
			objA.ruta = obtenerRelacionNivel(listaAlarmasNiveles, fila.IDA, objAN.ID);

			objAN = listaNiveles.ElementAt(2);
			objA.tramo = obtenerRelacionNivel(listaAlarmasNiveles, fila.IDA, objAN.ID);

			objAN = listaNiveles.ElementAt(3);
			objA.segmento = obtenerRelacionNivel(listaAlarmasNiveles, fila.IDA, objAN.ID);

			objAN = listaNiveles.ElementAt(4);
			objA.cerca = obtenerRelacionNivel(listaAlarmasNiveles, fila.IDA, objAN.ID);

			return objA;
		}

		private string obtenerRelacionNivel(List<FilaNivelQuery> listaAlarmasNiveles, short idAlarma, byte idAlarmaNivel)
		{
			string encontro = _VALOR_NO_APLICA;

			foreach (FilaNivelQuery obj in listaAlarmasNiveles)
			{
				if ((obj.IDA == idAlarma) && (obj.IDAN == idAlarmaNivel))
				{
					encontro = "true";
					break;
				}
			}

			return encontro;

		}

	}
}
