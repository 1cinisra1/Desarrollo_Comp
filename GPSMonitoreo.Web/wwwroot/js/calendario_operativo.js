$(document).ready(function () {


});

/**************************************************************************************************************************************************
*************************************************************VARIABLES****************************************************************************
***************************************************************************************************************************************************/

var objCalendario = null;
var ObjArregloTipoDias = new Array();

var TildeIMinuscula = '\u00ED';
var IdContenedorError = 'windowError';
var SemanaIteracion = 0;

var ArregloCantidadDiasImpresion = new Array();
var CantidadDiasSemana = 7;

var IdSabadoNoLaboral = 4;
var TextoSabadoLaboral = 'Sabados Laborales';
var TextoSabadoNoLaboral = 'Sabados No Laborales';

var IdDomingoNoLaboral = 6;
var TextoDomingoLaboral = 'Domingos Laborales';
var TextoDomingoNoLaboral = 'Domingos No Laborales';

var NombreDiaSemana = '_dsemana_';

var NumeroDiaSabado = 6;
var NumeroDiaDomingo = 7;


var TextoSeparacion = '==================================================================================================';


/**************************************************************************************************************************************************
***************************************************DEFINICION DE CLASES****************************************************************************
***************************************************************************************************************************************************/



function CalendarioOperativo(type){
	this.idCalendarioOperativo = 0;
	this.cantidadSemanas = 0;
	this.primeroEneroEnSemanaUno = true;
	this.arregloMes = new Array();
	this.arregloTipoDia = new Array();
}

function Mes(type) {
	this.idAnio = 0;
	this.idMes = 0;
	this.numMes = 0;
	this.abv = "";
	this.nombre = "";
	this.cantidadDias = 0;
	this.cantidadSemanas = 0;
	this.numPrimeraSemana = 0;
	this.numUltimaSemana = 0;
	this.arregloDias = new Array();


}

function Dia(type) {
	this.idMes = 0;
	this.idDia = 0;
	this.numDiaMes = 0;
	this.numDiaSemana = 0;
	this.abvDia = "";
	this.numSemana = 0;

	this.arregloTipoDia = new Array();
}

function TipoDia(type) {
	this.idTipo = 0;
	this.codigo = "";
	this.descripcion = "";
	this.chequeado = false;
	this.idChbDiaPorTipo = "";

	this.arregloCombinacionesPermitidas = new Array();
	this.arregloDiasMostrarCheckbox = new Array();
}


/**************************************************************************************************************************************************
**************************************************************FUNCIONES****************************************************************************
***************************************************************************************************************************************************/

function SetearValoresIniciales() {


	var idSpanSabado = '#span_sabado';
	var spanSabado = $(idSpanSabado);
	spanSabado.text(TextoSabadoLaboral);
	spanSabado.attr('data-tipo', (IdSabadoNoLaboral + 1));

	var idSpanDomingo = '#span_domingo';
	var spanDomingo = $(idSpanDomingo);
	spanDomingo.text(TextoDomingoLaboral);
	spanDomingo.attr('data-tipo', (IdDomingoNoLaboral + 1));

}


function ActivarDesactivarMasivamente(objeto, tipoDia) {

	var obj = $(objeto);
	var idCheckbox = obj.attr('id');
	var dia = idCheckbox.split('_')[1];

	var idSpan = '#span_' + dia;
	var span = $(idSpan);
	span.text('');
	var dataTipo = span.attr('data-tipo');

	var arreglo = ObtenerArregloNuevoTextoTipo(dataTipo, tipoDia);
	span.text(arreglo[0].texto);
	span.attr('data-tipo', arreglo[0].idDataTipo);

	////imprimirConsola(objCalendario);
	//////fila_mes_1_tipo_4_dia_2_dsemana_6
	var esSabado = EsDiaSabado(dataTipo);
	var numeroDiaSemana = 0;
	if (esSabado) {
		numeroDiaSemana = NumeroDiaSabado;
	}
	else {
		numeroDiaSemana = NumeroDiaDomingo;
	}
	var dataTipoAnterior = ObtenerAnteriorValorTipoDato(dataTipo);
	var objArregloMes = objCalendario.arregloMes;
	for (var i = 0; i < objArregloMes.length; i++)
	{
		var objMes = objArregloMes[i];
		for(var j = 0; j < objMes.arregloDias.length; j++)
		{
			var objDia = objMes.arregloDias[j];
			if (objDia.numDiaSemana == numeroDiaSemana) {

				var idObjCheckAnterior = 'fila_mes_' + objMes.numMes + '_tipo_' + dataTipoAnterior + '_dia_' + objDia.numDiaMes + NombreDiaSemana + numeroDiaSemana;
				RealizarChequeoDeschequeo(idObjCheckAnterior);
				CambiarEstadoCheckEnObjetoMasivo(idObjCheckAnterior);

				var idObjCheck = 'fila_mes_' + objMes.numMes + '_tipo_' + dataTipo + '_dia_' + objDia.numDiaMes + NombreDiaSemana + numeroDiaSemana;
				RealizarChequeoDeschequeo(idObjCheck);
				CambiarEstadoCheckEnObjetoMasivo(idObjCheck);
			}
			
		}
		
	}

	ImprimirConsola(objCalendario);

}

function ObtenerAnteriorValorTipoDato(dataTipo) {
	var anterior = 0;
	if (dataTipo == IdSabadoNoLaboral) {
		anterior = (IdSabadoNoLaboral + 1);
	}
	else {
		if ((dataTipo == (IdSabadoNoLaboral + 1))) {
			anterior = IdSabadoNoLaboral;
		}
		else {
			if (dataTipo == IdDomingoNoLaboral) {
				anterior = (IdDomingoNoLaboral + 1);
			}
			else {
				anterior = IdDomingoNoLaboral;
			}
		}
	}
	return anterior;
}

function RealizarChequeoDeschequeo(idElemento) {
	
	var obj = $('#'+idElemento);
	var attr = obj.attr('checked');

	// For some browsers, `attr` is undefined; for others, `attr` is false. Check for both.
	if (typeof attr !== typeof undefined && attr !== false) {
		obj.removeAttr('checked');
	}
	else {
		obj.attr('checked', 'checked');
	}
}

function EsDiaSabado(dataTipo)
{
	 
	var valor = false;

	if (dataTipo == IdSabadoNoLaboral) {
		valor = true;
	}
	else {
		if ((dataTipo == (IdSabadoNoLaboral + 1))) {
			valor = true;
		}
	}

	return valor;

}

function CambiarEstadoCheckEnObjetoMasivo(idCheckbox) {

	////fila_mes_1_tipo_4_dia_2_dsemana_6
	var arreglo = ObtenerArregoIdsCheckbox(idCheckbox);
	var iMes = arreglo[0];
	var iTipo = arreglo[1];
	var iDia = arreglo[2];

	var objEncontrado = objCalendario.arregloMes[iMes].arregloDias[iDia].arregloTipoDia[iTipo];
	var estadoActual = objEncontrado.chequeado;
	objEncontrado.chequeado = !estadoActual;
	////imprimirConsola(objCalendario);
}


function RefrescarCheckbox(valor, objCheck) {
	
	var obj = $(objCheck);
	obj.prop('checked', valor);
	
}


function CambiarEstadoCheckEnObjeto(idCheckbox) {

	var retorno = false;
	
	////fila_mes_1_tipo_4_dia_2_dsemana_6

	var arreglo = ObtenerArregoIdsCheckbox(idCheckbox);
	var iMes = arreglo[0];
	var iTipo = arreglo[1];
	var iDia = arreglo[2];

	var chequeo = false;
	var objCheck = $('#' + idCheckbox);
	var valorCheck = objCheck.is(":checked");
	
	if (valorCheck) {
		chequeo = true;
	}

	var objDia = objCalendario.arregloMes[iMes].arregloDias[iDia];
	if (chequeo) {
		var objTD = objArregloTipoDias[iTipo];
		var arregloCombinaciones = objTD.arregloCombinacionesPermitidas;
		var arregloChequeadosActuales = ObtenerArregloChequeadosActuales(objDia);
		var esValido = EsValidoNuevoChequeo(arregloCombinaciones, arregloChequeadosActuales);
		if (esValido) {
			var objEncontrado = objDia.arregloTipoDia[iTipo];
			objEncontrado.chequeado = true;
			
			retorno = true;
		}
		else {
			///alert('No se puede');
			var mensaje = 'No se puede definir este d' + TildeIMinuscula + 'a como: \"' + objTD.descripcion + '\"';
			LlamarVentanaError(mensaje);

			retorno = false;
		}
	}
	else {
		var objEncontrado = objDia.arregloTipoDia[iTipo];
		objEncontrado.chequeado = !objEncontrado.chequeado;
	}
	
	return retorno;
}


function EsValidoNuevoChequeo(arregloCombinaciones, arregloChequeados) {

	var valido = false;
	
	var encontro = 0
	for (var i = 0; i < arregloChequeados.length; i++)
	{
		var idCh = arregloChequeados[i];
		for (var j = 0; j < arregloCombinaciones.length; j++) {
			var idCo = arregloCombinaciones[j];
			if (idCh == idCo) {
				encontro++;
				break;
			}
		}
	}

	if (arregloChequeados.length == encontro) {
		valido = true;
	}

	return valido;

}


function ObtenerArregoIdsCheckbox(idCheckbox) {
	var nuevoArreglo = new Array();
	var arreglo = idCheckbox.split('_');

	nuevoArreglo.push(arreglo[2] - 1);
	nuevoArreglo.push(arreglo[4] - 1);
	nuevoArreglo.push(arreglo[6] - 1);

	return nuevoArreglo;
}


function ObtenerArregloChequeadosActuales(objDia){

	var arreglo = new Array();

	for (var i = 0; i < objDia.arregloTipoDia.length; i++)
	{
		var objTD = objDia.arregloTipoDia[i];
		if (objTD.chequeado) {
			arreglo.push(objTD.idTipo);
		}
	}

	return arreglo;

}






function GenerarTablaMeses(objCalendario, cTablasMeses) {

	var arregloMeses = objCalendario.arregloMes;

	ArregloCantidadDiasImpresion = [];
	for (var i = 0; i < arregloMeses.length; i++) {
		
		var objMes = arregloMeses[i];
		var nombreMes = objMes.nombre;
		var idContenedor = 'c_mes' + '_' + objMes.nombre;

		var nuevoElemento = '';

		var htmlContenedor = '<div id="' + idContenedor + '" class="cls_div_mes">';

		var htmlTabla = GenerarTabla(objMes, objCalendario.cantidadSemanas, objCalendario.arregloTipoDia);

		nuevoElemento = htmlContenedor + htmlTabla + '</div>';
		cTablasMeses.append(nuevoElemento);

	}
	////console.log(arregloCantidadDiasImpresion);
}

function GenerarTabla(objMes, cantidadSemanasAnual, arregloTipoDias) {

	var htmlTabla = '';
	var idTabla = 'tabla_' + objMes.nombre;
	var idBodyTabla = 'body_tabla_' + objMes.nombre;

	htmlTabla = '<table id="' + idTabla + '" ' + DefinirAtributosTabla() + ' >';
	htmlTabla += '<thead>';
	htmlTabla += GenerarCabecerasTabla(objMes, cantidadSemanasAnual);
	htmlTabla += '</thead>';
	htmlTabla += '<tbody id="' + idBodyTabla + '" >';
	htmlTabla += GenerarCuerpoTabla(objMes, arregloTipoDias);
	htmlTabla += '</tbody>';
	htmlTabla += '</table>';

	return htmlTabla;

}

function GenerarCabecerasTabla(objMes, cantidadSemanasAnual) {

	var htmlCabecera =  '';


	var idFilaCabecera1 = 'fila_cabecera_' + objMes.nombre + '_1';
	var idFilaCabecera2 = 'fila_cabecera_' + objMes.nombre + '_2';
	var idFilaCabecera3 = 'fila_cabecera_' + objMes.nombre + '_3';

	htmlCabecera += '<tr id="' + idFilaCabecera1 + '" >';
	htmlCabecera += GenerarHtmlCabeceraSemana(objMes, cantidadSemanasAnual);
	htmlCabecera += '</tr>';

	htmlCabecera += '<tr id="' + idFilaCabecera2 + '" >';
	////htmlCabecera += '<th></th>';
	htmlCabecera += GenerarHtmlCabeceraNombreDias(objMes);
	htmlCabecera += '</tr>';

	htmlCabecera += '<tr id="' + idFilaCabecera3 + '" >';
	////html_cabecera += '<th></th>';
	htmlCabecera += GenerarHtmlCabeceraNumeroDias(objMes);
	htmlCabecera += '</tr>';

	

	return htmlCabecera;

}


function GenerarHtmlCabeceraSemana(objMes, cantidadSemanasAnual) {
	var htmlSemana = '';
	var titulo = '';
	htmlSemana += '<th rowspan="3" style="width:10%; text-align:left;">' + objMes.nombre.toUpperCase() + '</th>';
	
	SemanaIteracion = objMes.numPrimeraSemana;
	for (var i = 1; i <= objMes.cantidadSemanas; i++)
	{
		if (objMes.numMes == 1) {
			if (i == 1) {
				///SemanaIteracion = 0;
				titulo = ' SEMAMA ANIO ANTERIOR ';
			}
			else {
				
				titulo = ' SEMAMA ' + SemanaIteracion + '/' + cantidadSemanasAnual;		
			}
			
		}
		else {
			titulo = ' SEMAMA ' + SemanaIteracion + '/' + cantidadSemanasAnual;
		}
		SemanaIteracion++;

		var colspan = 7;
		htmlSemana += '<th colspan="' + colspan + '" ' + '>' + titulo + '</th>';

	}

	var cantidad = objMes.cantidadSemanas * CantidadDiasSemana;
	ArregloCantidadDiasImpresion.push(cantidad);

	return htmlSemana;
}


function GenerarHtmlCabeceraNombreDias(objMes) {

	var htmlDias = '';


	for (var i = 1; i <= objMes.cantidadSemanas; i++) {
		htmlDias += '<th>LU</th>';
		htmlDias += '<th>MA</th>';
		htmlDias += '<th>MI</th>';
		htmlDias += '<th>JU</th>';
		htmlDias += '<th>VI</th>';
		htmlDias += '<th>SA</th>';
		htmlDias += '<th>DO</th>';


	}
	
	

	return htmlDias;

}

function GenerarHtmlCabeceraNumeroDias(objMes) {

	var htmlNumeroDias = '';

	var cantidadDiasImpresos = 0;
	var numeroInicialDia = ObtenerCantidadDiasAntes(objMes);
	for (var i = 1; i <= numeroInicialDia; i++) {

		htmlNumeroDias += '<th></th>';
		cantidadDiasImpresos++;
	}

	for (var j = 0; j < objMes.arregloDias.length; j++) {
		var objDia = objMes.arregloDias[j];
		htmlNumeroDias += '<th>' + objDia.numDiaMes + '</th>';
		cantidadDiasImpresos++
	}

	var cantidadDias = ObtenerCantidadDiasDespues(objMes, ArregloCantidadDiasImpresion, cantidadDiasImpresos);
	///console.log(ArregloCantidadDiasImpresion);
	///console.log(cantidadDiasImpresos);
	for (var k = 1; k <= cantidadDias; k++) {

		htmlNumeroDias += '<th></th>';
	}

	return htmlNumeroDias;

}

function ObtenerCantidadDiasAntes(objMes) {
	var cantidad = 0;
	
	cantidad = objMes.arregloDias[0].numDiaSemana - 1;

	return cantidad;
}

function ObtenerCantidadDiasDespues(objMes, arregloCantidadDiasImpresion, cantidadDiasImpresos) {
	var cantidad = 0;

	var idMes = objMes.numMes - 1;
	var cantidad = arregloCantidadDiasImpresion[idMes] - cantidadDiasImpresos;

	return cantidad;

}

function GenerarCuerpoTabla(objMes, arregloTipoDias) {
	var htmlBody = '';
	///console.log(arregloTipoDias);
	var codeJsClick = 'if(event.stopPropagation){event.stopPropagation();}event.cancelBubble=true;'
	
	var colorFila = '';
	var color1 = 'background-color:#E6E6E6;';
	var color2 = 'background-color:#CEE3F6;';

	var celdaSinCheck = '<td></td>';
	for (var i = 0; i < arregloTipoDias.length; i++)
	{
		
		if (i == 0) {
			colorFila = color1;
			
		}
		else {
			if ((i % 2) == 0) {
				colorFila = color1;
				
			}
			else {
				colorFila = color2;
				
			}
		}
		
		var objTD = arregloTipoDias[i];
		var idFila = 'fila' + '_mes_' + objMes.numMes + '_tipo_' + objTD.idTipo;
		htmlBody += '<tr style="' + colorFila + '" >';
		htmlBody += '<td id="' + idFila + '" >' + objTD.descripcion + '</td>';
		var cantidadDiasAntes = ObtenerCantidadDiasAntes(objMes);
		var cantidadDiasImpresos = 0;
		for (var j = 1; j <= cantidadDiasAntes; j++) {
			htmlBody += celdaSinCheck;
			cantidadDiasImpresos++;
		}

		for (var k = 0; k < objMes.arregloDias.length; k++) {
			var objDia = objMes.arregloDias[k];
			var idCheckbox = objDia.arregloTipoDia[i].idChbDiaPorTipo;
			var estaChequeado = '';
			if (objDia.arregloTipoDia[i].chequeado) {
				estaChequeado = ' checked ';
			}

			htmlBody += '<td>';

			if (DiaEsAptoParaMostrarCheckbox(objTD.arregloDiasMostrarCheckbox, objDia.numDiaSemana)) {
				htmlBody += '<input type="checkbox" id="' + idCheckbox + '" ';
				htmlBody += ' class="cls_checkbox_dia" ';
				htmlBody += ' onclick="' + codeJsClick + ' RefrescarCheckbox(CambiarEstadoCheckEnObjeto(\'' + idCheckbox + '\'), this)" ';
				htmlBody += estaChequeado + ' />';
			}



			htmlBody += '</td>';

			cantidadDiasImpresos++;
		}

		var cantidadDiasDespues = ObtenerCantidadDiasDespues(objMes, ArregloCantidadDiasImpresion, cantidadDiasImpresos);
		for (var p = 1; p <= cantidadDiasDespues; p++) {

			htmlBody += celdaSinCheck;
		}

		htmlBody += '</tr>';
	}

	

	return htmlBody;
}


/**************************************************************************************************************************************************
**************************************************************COMUNES******************************************************************************
***************************************************************************************************************************************************/

function DefinirAtributosTabla() {
	var atrTabla = ' class="clase_table_mes table table-striped table-bordered dt-responsive nowrap no-footer" ' + ' role="grid" ' + ' style="width:100%" ';

	return atrTabla;
}


/**************************************************************************************************************************************************
**************************************************************GENERALES****************************************************************************
***************************************************************************************************************************************************/
function DiaEsAptoParaMostrarCheckbox (arreglo, numSemana)
{
	var valor = false;

	for (var i = 0; i < arreglo.length; i++)
	{
		var diaPermitido = arreglo[i];
		if (numSemana == diaPermitido) {
			valor = true;
			break;
		}
	}

	return valor;
}


function LlamarVentanaError(msg) {

	var ventanaError = $('#windowError').jqxWindow({
		height: 200, width: 400, isModal: true, autoOpen: false, draggable: false, resizable: false
	}).data('jqxWidget');
	ventanaError.setContent(msg);
	ventanaError.open();

}


function ImprimirConsola(objeto) {
	console.log(TextoSeparacion);
	console.log(objeto);
	console.log(TextoSeparacion);
}

function ObtenerArregloNuevoTextoTipo(dataTipo, tipoDia) {
	var arreglo = [];
	var nuevoIdDataTipo = 0;
	var nuevoTexto = '';

	if (tipoDia == 1) {
		if (dataTipo == IdSabadoNoLaboral) {
			nuevoTexto = TextoSabadoLaboral;
			nuevoIdDataTipo = (IdSabadoNoLaboral + 1);
		}
		else {
			nuevoTexto = TextoSabadoNoLaboral;
			nuevoIdDataTipo = (IdSabadoNoLaboral);
		}
	}
	else {
		if (dataTipo == IdDomingoNoLaboral) {
			nuevoTexto = TextoDomingoLaboral;
			nuevoIdDataTipo = (IdDomingoNoLaboral + 1);
		}
		else {
			nuevoTexto = TextoDomingoNoLaboral;
			nuevoIdDataTipo = (IdDomingoNoLaboral);
		}
	}

	arreglo.push({texto: nuevoTexto, idDataTipo: nuevoIdDataTipo});

	return arreglo;
}

function ConvertirJsonArray(arrayJson) {
	var arreglo = $.map(arrayJson, function (value, index) {
		return [value];
	});

	return arreglo;
}


function ConvertirObjetoJsonToJs(arreglo) {

	var arregloNuevo = new CalendarioOperativo();

	for (var i = 0; i < arreglo.length; i++)
	{
		arregloNuevo.idCalendarioOperativo = arreglo[i].idCalendarioOperativo;
		arregloNuevo.cantidadSemanas = arreglo[i].cantidadSemanas;
		arregloNuevo.primeroEneroEnSemanaUno = arreglo[i].primeroEneroEnSemanaUno;

		var arregloTiposDias = arreglo[i].arregloTipoDia;
		for (var k = 0; k < arregloTiposDias.length ; k++)
		{
			var objTD = arregloTiposDias[k];
			var objTDNuevo = new TipoDia();

			objTDNuevo.idTipo = objTD.idTipo;
			objTDNuevo.codigo = objTD.codigo;
			objTDNuevo.descripcion = objTD.descripcion;
			objTDNuevo.chequeado = objTD.chequeado;
			objTDNuevo.idChbDiaPorTipo = objTD.idChbDiaPorTipo;
			objTDNuevo.arregloCombinacionesPermitidas = GenerarArregloEnteros(objTD.arregloCombinacionesPermitidas);
			objTDNuevo.arregloDiasMostrarCheckbox = GenerarArregloEnteros(objTD.arregloDiasMostrarCheckbox);
			arregloNuevo.arregloTipoDia.push(objTDNuevo);
		}

		for (var j = 0; j < arreglo[i].arregloMes.length ; j++)
		{
			var objMes = arreglo[i].arregloMes[j];
			var nuevoObjMes = GenerarObjMes(objMes);
			arregloNuevo.arregloMes.push(nuevoObjMes);
		}
		
	}

	objArregloTipoDias = arregloNuevo.arregloTipoDia;

	return arregloNuevo;

}

function GenerarArregloEnteros(arregloEnteros) {

	var arreglo = new Array();

	for (var i = 0; i < arregloEnteros.length; i++)
	{
		arreglo.push(arregloEnteros[i]);
	}

	return arreglo;

}


function GenerarObjMes(mes) {
	var objMes = new Mes();

	objMes.idAnio = mes.idAnio;
	objMes.idMes = mes.idMes;
	objMes.numMes = mes.numMes;
	objMes.abv = mes.abv;
	objMes.nombre = mes.nombre;
	objMes.cantidadDias = mes.cantidadDias;
	objMes.numPrimeraSemana = mes.numPrimeraSemana;
	objMes.numUltimaSemana = mes.numUltimaSemana;
	objMes.cantidadSemanas = mes.cantidadSemanas;
	objMes.arregloDias = GenerarArregloDias(mes);

	return objMes;
}


function GenerarArregloDias(mes) {

	var arregloNuevoDias = new Array();

	for (var i = 0; i < mes.arregloDias.length ; i++) {
		var objDia = mes.arregloDias[i];
		var nuevoObjDia = GenerarObjDia(objDia, mes);
		arregloNuevoDias.push(nuevoObjDia);
	}

	return arregloNuevoDias;

}


function GenerarObjDia(dia, mes) {
	var objDia = new Dia();

	objDia.idMes = dia.idMes;
	objDia.idDia = dia.idDia;
	objDia.numDiaMes = dia.numDiaMes;
	objDia.numDiaSemana = dia.numDiaSemana;
	objDia.abvDia = dia.abvDia;
	objDia.numSemana = dia.numSemana;
	objDia.arregloTipoDia = GenerarArregloTipoDia(dia, mes);

	return objDia;
}


function GenerarArregloTipoDia(dia, mes) {

	var arregloTipoDia = new Array();

	for (var i = 0; i < dia.arregloTipoDia.length ; i++) {
		var objTipoDia = dia.arregloTipoDia[i];
		var nuevoObjTipoDia = GenerarObjTipoDia(objTipoDia, mes.numMes, dia.numDiaMes, dia.numDiaSemana);
		arregloTipoDia.push(nuevoObjTipoDia);
	}

	return arregloTipoDia;

}


function GenerarObjTipoDia(tipoDia, numMes, numeroDiaMes, numDiaSemana) {
	var objTipoDia = new TipoDia();

	objTipoDia.idTipo = tipoDia.idTipo;
	objTipoDia.codigo = tipoDia.codigo;
	objTipoDia.descripcion = tipoDia.descripcion;
	objTipoDia.chequeado = tipoDia.chequeado;
	objTipoDia.idChbDiaPorTipo = 'fila' + '_mes_' + numMes + '_tipo_' + tipoDia.idTipo + '_dia_' + numeroDiaMes + NombreDiaSemana + numDiaSemana;

	return objTipoDia;
}