using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Libraries.Extensions.LinqExtensions;
using GPSMonitoreo.Libraries.Extensions.CollectionExtensions;
using GPSMonitoreo.Web.Extensions;
using GPSMonitoreo.Web.Extensions.JqwidgetsExtensions;
using MVCHelpers.Extensions;
using GPSMonitoreo.Web.ViewModels;
using GPSMonitoreo.Web.PostModels.General.Calendarios;
using GPSMonitoreo.Web.ViewComponents.jqxGrid;
using GPSMonitoreo.Web.Classes;
using MVCHelpers.ActionResults;
using System.Data.Entity;
using GPSMonitoreo.Data.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace GPSMonitoreo.Web.Controllers.General
{
    public class CalendariosController : BaseController
	{

		private const string ValorNoAplica = "na";
		private const string IdTxtAnio = "txt_anio";
		private const string IdCalendario = "OBJ_CALENDARIO";


		private const string IdFechasDt = "OBJ_FECHAS_DT";
		private bool PrimeroEneroValido = true;
		private static int DiasRestantesSemanaCero = 0;
		private static int DiasRestantesPrimeraSemanaMes = 0;
		private static int SemanaActual = 0;
		private const string FormatoAbvDia = "ddd";
		private const string FormatoAbvMes = "MMM";
		private const string FormatoNombreMes = "MMMM";

		public IActionResult CalendarioOperativo()
		{

			int anio = ObtenerAnio();
			//ViewData[IdTxtAnio] = anio.ToString().ToJsonString();
			ViewData[IdTxtAnio] = anio.ToString();

			Calendario objCalendario = ObtenerCalendario(anio);
			ViewData[IdCalendario] = objCalendario.arregloCalendarioOperativo.ToJsonString();

			

			var layoutModel = new ViewModels.TabbedLayoutForm()
			{
				FormId = "form_calendario_operativo",
				Title = "CALENDARIO::OPERATIVO"
			};

			return View("~/Views/General/Calendarios/Operativo.cshtml", layoutModel);

		}

		private Calendario ObtenerCalendario(int anio)
		{
			Calendario objCalendario = new Calendario();

			CalendarioOperativo objCalendarioOperativo = new CalendarioOperativo();
			objCalendarioOperativo.cantidadSemanas = GetWeeksInYear(anio);
			DateTime fechaPrimeroEnero = new DateTime(anio, 1, 1);
			objCalendarioOperativo.primeroEneroEnSemanaUno = EsValidoPrimeroDeEnero(fechaPrimeroEnero);
			SemanaActual = 0;
			for (var i = 1; i <= 12; i++)
			{
				if (i == 1) {
					PrimeroEneroValido = objCalendarioOperativo.primeroEneroEnSemanaUno;
					DiasRestantesSemanaCero = ObtenerDiasRestanteSemanaCero(fechaPrimeroEnero);
				}
				else
				{
					PrimeroEneroValido = true;
					DiasRestantesSemanaCero = 0;
				}
				List<DateTime>  arregloFechas = GetDates(anio, i);
				Mes objMes = GenerarMes(arregloFechas, anio, i);
				objCalendarioOperativo.arregloMes.Add(objMes);
			}

			objCalendarioOperativo.arregloTipoDia = TipoDia.ObtenerTiposDias();
			objCalendario.arregloCalendarioOperativo.Add(objCalendarioOperativo);


			return objCalendario;

		}

		private static Mes GenerarMes(List<DateTime> arregloFechas, int anio, int mes) {
			Mes objMes = new Mes();
			objMes.numMes = mes;
			objMes.cantidadDias = DateTime.DaysInMonth(anio, mes);

			DateTime primerDia = arregloFechas[0];
			DateTime ultimoDia = arregloFechas[arregloFechas.Count - 1];

			objMes.nombre = ObtenerNombreOAbreviatura(ultimoDia, FormatoNombreMes);
			objMes.abv = ObtenerNombreOAbreviatura(ultimoDia, FormatoAbvMes);
			objMes.cantidadSemanas = ObtenerCantidadSemana(primerDia, ultimoDia);

			DiasRestantesPrimeraSemanaMes = 0;
			if (mes > 1)
			{
				DiasRestantesPrimeraSemanaMes = DiasPrimerosMes(primerDia);
			}
			int ultimaSemanaMes = 0;
			foreach (DateTime fecha in arregloFechas)
			{
				var numeroDiaSemana = ObtenerDiaSemana(fecha);
				Dia objDia = new Dia(numeroDiaSemana);
				objDia.numDiaMes = fecha.Day;
				objDia.numDiaSemana = numeroDiaSemana;
				objDia.abvDia = ObtenerNombreOAbreviatura(fecha, FormatoAbvDia);

				if(DiasRestantesSemanaCero == 0)
				{
					if(DiasRestantesPrimeraSemanaMes > 0)
					{
						objDia.numSemana = SemanaActual;
						DiasRestantesPrimeraSemanaMes--;
					}
					else
					{
						SemanaActual = fecha.GetWeekOfYearNew(SemanaActual);
						objDia.numSemana = SemanaActual;
					}
					
				}
				else
				{
					objDia.numSemana = 0;
					DiasRestantesSemanaCero--;
				}
				if (primerDia == fecha)
				{
					objMes.numPrimeraSemana = objDia.numSemana;
				}
				ultimaSemanaMes = objDia.numSemana;
				objMes.arregloDias.Add(objDia);

			}
			objMes.numUltimaSemana = ultimaSemanaMes;

			return objMes;

		}

		private static int DiasPrimerosMes(DateTime fechaPrimeroMes)
		{
			int diasSemanaRepetida = 0;

			int diaPrimeroMes = (int)fechaPrimeroMes.DayOfWeek;
			if (diaPrimeroMes == 0 || diaPrimeroMes == 1)
			{
				diasSemanaRepetida = 0;
			}
			else
			{
				diasSemanaRepetida = 7 - diaPrimeroMes + 1;
			}
			

			return diasSemanaRepetida;
		}

		private static bool EsValidoPrimeroDeEnero(DateTime fechaPrimeroEnero)
		{
			bool valido = false;

			var diaSemana = (int)fechaPrimeroEnero.DayOfWeek;

			if (diaSemana >= 1 && diaSemana <= 4)
			{
				valido = true;
			}

			return valido;

		}

		private static int ObtenerDiasRestanteSemanaCero(DateTime fechaPrimeroEnero)
		{
			int dias = 0;

			var diaSemana = (int)fechaPrimeroEnero.DayOfWeek;

			if (diaSemana != 0)
			{
				dias =  7 - diaSemana + 1;
			}

			return dias;

		}

		private static string ObtenerNombreOAbreviatura(DateTime fecha, string formato) {

			string nombreAbv = "";

			nombreAbv = fecha.ToString(formato,new CultureInfo("es-ES"));

			return nombreAbv;

		}

		///
		///	1. Dia Laboral
		///	2. Dia Laboral sin atencion
		///	3. Dia no laboral
		///	4. Sábado no laboral
		///	5. Sábado laboral
		///	6. Domingo no laboral
		///	7. Domingo laboral
		///	8. Feriado
		///	9. Especial #1
		///	10. Especial #2
		

		private static int ObtenerCantidadSemana(DateTime primerDia, DateTime ultimoDia)
		{
			int cantidad = 0;

			cantidad = ultimoDia.GetWeekOfMonth();

			if (primerDia.DayOfWeek == 0)
			{
				cantidad++;
			}

			if (ultimoDia.DayOfWeek == 0)
			{
				cantidad--;
			}

			return cantidad;
		}

		/***********************************************************************************************************************************************************/
		/*********************************************************************** METODOS ***************************************************************************/
		/***********************************************************************************************************************************************************/

		public IActionResult Save(CalendariosEdit editModel)
		{

			var errors = ModelState.ToJsonObject();
			//Object objCa = editModel.objCalendario;
			//var capacidadesCount = objCa.arregloCalendarioOperativo.ToArray() ?.Length ?? 0;

			//Console.WriteLine("calendario operativo count: " + capacidadesCount);


			if (errors.Count > 0)
			{
				return JsonFormErrors(errors);
			}
			else
			{
				var id = editModel.Save(DBContext);
				return JsonNotification(Resources.Messages.RecordSaved.ReplaceArgs(editModel.descripcion_larga), new { id = id });
			}
		}


		/***********************************************************************************************************************************************************/
		/*********************************************************************** GENERALES **************************************************************************/
		/***********************************************************************************************************************************************************/

		private static int ObtenerDiaSemana(DateTime fecha) {
			int diaSemana = 0;

			diaSemana = (int)fecha.DayOfWeek;
			if (diaSemana == 0)
			{
				diaSemana = 7;
			}

			return diaSemana;

		}


		private int ObtenerAnio() {
			int anio = 0;

			String sDate = DateTime.Now.ToString();
			DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

			String dy = datevalue.Day.ToString();
			String mn = datevalue.Month.ToString();
			String yy = datevalue.Year.ToString();
			anio = Int32.Parse(yy);

			return anio;

		}


		private static List<DateTime> GetDates(int year, int month)
		{
			var dates = new List<DateTime>();

			// Loop from the first day of the month until we hit the next month, moving forward a day at a time
			for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
			{
				dates.Add(date);
			}

			return dates;
		}


		public int GetWeeksInYear(int year)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			DateTime date1 = new DateTime(year, 12, 31);
			Calendar cal = dfi.Calendar;
			return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule,
												dfi.FirstDayOfWeek);
		}


	}


	static class DateTimeExtensions
	{
		static GregorianCalendar _gc = new GregorianCalendar();
		public static int GetWeekOfMonth(this DateTime time)
		{
			DateTime first = new DateTime(time.Year, time.Month, 1);
			return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
		}

		public static int GetWeekOfYear(this DateTime time)
		{			
			return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
		}

		public static int GetWeekOfYearNew(this DateTime time, int semana_actual_anterior)
		{
			var valor = 0;

			var dia_semana = (int)time.DayOfWeek;
			if (dia_semana == 1)
			{
				valor = semana_actual_anterior + 1;
			}
			else
			{
				valor = semana_actual_anterior;
			}
			
			
			return valor;


		}

	}


}
