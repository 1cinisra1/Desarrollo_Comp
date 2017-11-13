using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.PostModels.General.Calendarios
{
    public class TipoDia
	{
		//
		//	1. Dia Laboral
		//	2. Dia Laboral sin atencion
		//	3. Dia no laboral
		//	4. Sábado no laboral
		//	5. Sábado laboral
		//	6. Domingo no laboral
		//	7. Domingo laboral
		//	8. Feriado
		//	9. Especial #1
		//	10. Especial #2
		//
		public int idTipo { get; set; }

		public string codigo { get; set; }//4 caracteres

		public string descripcion { get; set; }

		public Boolean chequeado { get; set; }

		public string idChbDiaPorTipo { get; set; }

		public List<int> arregloCombinacionesPermitidas { get; set; }

		public List<int> arregloDiasMostrarCheckbox { get; set; }


		public TipoDia() {

			this.idTipo = 1;
			this.descripcion = "Dia Laboral";
			this.chequeado = true;
			this.arregloCombinacionesPermitidas = new List<int>();
			this.arregloDiasMostrarCheckbox = new List<int>();

		}

		public TipoDia(int id, string descripcion, bool chequeado)
		{

			this.idTipo = id;
			this.descripcion = descripcion;
			this.chequeado = chequeado;
			this.arregloCombinacionesPermitidas = new List<int>();
			this.arregloDiasMostrarCheckbox = new List<int>();
		}

		public TipoDia(int id, string descripcion)
		{

			this.idTipo = id;
			this.descripcion = descripcion;
			this.chequeado = false;
			this.arregloCombinacionesPermitidas = new List<int>();
			this.arregloDiasMostrarCheckbox = new List<int>();
		}



		public static List<TipoDia> ObtenerTiposDiasConCheck(int numDiaSemana)
		{
			List<TipoDia> arreglo = ObtenerTiposDias();

			if (numDiaSemana == 7)
			{
				arreglo[5].chequeado = true;
			}
			else
			{
				if ((numDiaSemana >= 1)  && (numDiaSemana <= 5))
				{
					arreglo[0].chequeado = true;
					
				}
				else
				{
					arreglo[3].chequeado = true;
				}
					
			}
			
			

			return arreglo;
		}


		public static List<TipoDia> ObtenerTiposDias()
		{

			List<TipoDia> arreglo = new List<TipoDia>();
			int i = 1;

			TipoDia obj_1 = new TipoDia(i++, "Día Laboral");
			obj_1.arregloCombinacionesPermitidas = new List<int>(new int[] { 8, 9, 10 });
			obj_1.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5 });
			arreglo.Add(obj_1);

			TipoDia obj_2 = new TipoDia(i++, "Día Laboral sin atencion");
			obj_2.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5 });
			arreglo.Add(obj_2);

			TipoDia obj_3 = new TipoDia(i++, "Día no laboral");
			obj_3.arregloCombinacionesPermitidas = new List<int>(new int[] { 8, 9, 10 });
			obj_3.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5 });
			arreglo.Add(obj_3);

			TipoDia obj_4 = new TipoDia(i++, "Sábado no laboral");
			obj_4.arregloCombinacionesPermitidas = new List<int>(new int[] { 8, 9, 10 });
			obj_4.arregloDiasMostrarCheckbox = new List<int>(new int[] { 6 });
			arreglo.Add(obj_4);

			TipoDia obj_5 = new TipoDia(i++, "Sábado laboral");
			obj_5.arregloDiasMostrarCheckbox = new List<int>(new int[] { 6 });
			arreglo.Add(obj_5);

			TipoDia obj_6 = new TipoDia(i++, "Domingo no laboral");
			obj_6.arregloCombinacionesPermitidas = new List<int>(new int[] { 8, 9, 10 });
			obj_6.arregloDiasMostrarCheckbox = new List<int>(new int[] { 7 });
			arreglo.Add(obj_6);

			TipoDia obj_7 = new TipoDia(i++, "Domingo laboral");
			obj_7.arregloDiasMostrarCheckbox = new List<int>(new int[] { 7 });
			arreglo.Add(obj_7);

			TipoDia obj_8 = new TipoDia(i++, "Feriado");
			obj_8.arregloCombinacionesPermitidas = new List<int>(new int[] { 1, 3, 4, 6 });
			obj_8.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
			arreglo.Add(obj_8);

			TipoDia obj_9 = new TipoDia(i++, "Especial #1");
			obj_9.arregloCombinacionesPermitidas = new List<int>(new int[] { 1, 3, 4, 6, 8 });
			obj_9.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
			arreglo.Add(obj_9);

			TipoDia obj_10 = new TipoDia(i++, "Especial #2");
			obj_10.arregloCombinacionesPermitidas = new List<int>(new int[] { 1, 3, 4, 6, 8 });
			obj_10.arregloDiasMostrarCheckbox = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
			arreglo.Add(obj_10);

			return arreglo;
		}

	}

}
